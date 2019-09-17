using System;
using SCLibWin;
using System.Collections;
using System.Globalization;

namespace SmartCard
{
    public class ACR1281U : IKart
    {
        Util util = new Util();
        private Int32 zone = 84; //82

        private string sprtr = "|";

        #region Bufferlar
        byte[] inBuf = new byte[128];
    //    byte[] outBuf = new byte[128];
        byte[] buffer = new byte[100];
        #endregion

        SCResMgr mng;
        SCReader rd;
        string rdName;


        /// <summary>
        /// Comment 
        /// </summary>
        public ACR1281U(int Zone)
        {
            zone = 84;  //zone = Zone; 82
            DilSecimi = EnmDil.Turkce;
        }

        internal enum EnmDil
        {
            English = 0,
            Turkce = 1
        }

        internal EnmDil DilSecimi { get; set; }

        internal int Zone { get { return 84;}  }//82

        private void Sleep(int time)
        {
            System.Threading.Thread.Sleep(time);
        }


        #region Omni Kart Reader fonksyonları
        
        public ArrayList GetKartReaders()
        {
            try
            {
                mng = new SCResMgr();
                mng.EstablishContext(SCardContextScope.System);
                rd = new SCReader(mng);
                ArrayList al = new ArrayList();
                mng.ListReaders(al);
                return al;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int InitCard()
        {
            try
            {
                mng = new SCResMgr();
                mng.EstablishContext(SCardContextScope.System);
                rd = new SCReader(mng);
                ArrayList al = new ArrayList();
                mng.ListReaders(al);


                foreach (string st in al)
                {
                    rdName = st;
                    //if (rdName.Contains("ACR128U"))
                    //{
                    if (!rd.IsConnected) rd.Connect(rdName, SCardAccessMode.Shared, SCardProtocolIdentifiers.T0);
                    else break;
                    //}
                }

                if (rd.IsConnected) rd.BeginTransaction();

                return Convert.ToInt32(rd.IsConnected);
            }
            catch (Exception ex)
            {
                return Convert.ToInt32(rd.IsConnected);
            }
        }

        public void FinishCard()
        {
            if (rd.IsConnected) rd.EndTransaction(SCardDisposition.ResetCard);
        }

        public int ReadCard(byte adres)
        {
            byte[] command = new byte[5];
            byte[] response = new byte[6];

            command[0] = 0X80;
            command[1] = 0XBE;
            command[2] = 0X00;
            command[3] = adres; //Read adres
            command[4] = 0X04;

            rd.Transmit(command, out response);
            if (response[4] == 0X90 && response[5] == 0X00)
            {
                Array.Copy(response, 0, inBuf, 0, 4);
                return 1;
            }
            else return 0;
        }

        public int Verify(byte adres, byte[] data)
        {
            byte[] command = new byte[9];
            byte[] response = new byte[2];

            command[0] = 0X00;
            command[1] = 0X20;
            command[2] = 0X00;
            command[3] = adres;  //verify  adres  = 7
            command[4] = 0X04;
            Array.Copy(data, 0, command, 5, 4);

            rd.Transmit(command, out response);
            if (response[0] == 0X90 && response[1] == 0X00) return 1;
            else return 0;
        }

        public int Update(byte adres, byte[] data)
        {
            byte[] command = new byte[9];
            byte[] response = new byte[2];

            command[0] = 0X80;
            command[1] = 0XDE;
            command[2] = 0X00;
            command[3] = adres;  //update adres
            command[4] = 0X04;
            Array.Copy(data, 0, command, 5, 4);

            rd.Transmit(command, out response);
            if (response[0] == 0X90 && response[1] == 0X00) return 1;
            else return 0;
        }

        #endregion

        #region IKart Fonksyonları

        private byte[] GetIssuer(Int32 zone)
        {
            byte[] issue_area = new byte[2];
            //48 = 0, 65 = A, 66 = B, 67 = C, 49 = 1, 

            if (zone == 1)
            {
                issue_area[0] = (byte)'A';
                issue_area[1] = (byte)'A';
            }
            else
                if (zone < 11)
                {
                    issue_area[0] = (byte)'A';
                    issue_area[1] = (byte)(49 + zone - 2);
                }
                else
                    if (zone < 36)
                    {
                        issue_area[0] = (byte)'A';
                        issue_area[1] = (byte)(66 + zone - 11); //'B';
                    }
                    else
                        if (zone < 72)
                        {
                            issue_area[0] = (byte)'B';
                            if (zone < 46)
                                issue_area[1] = (byte)(48 + zone - 36);
                            else
                                issue_area[1] = (byte)(65 + zone - 46);

                        }
                        else
                            if (zone < 108)
                            {
                                issue_area[0] = (byte)'C';
                                if (zone < 82)
                                    issue_area[1] = (byte)(48 + zone - 72);
                                else
                                    issue_area[1] = (byte)(65 + zone - 82);
                            }
                            else
                                if (zone < 144)
                                {
                                    issue_area[0] = (byte)'D';
                                    if (zone < 118)
                                        issue_area[1] = (byte)(48 + zone - 108);
                                    else
                                        issue_area[1] = (byte)(65 + zone - 118);
                                }

            return issue_area;
        }

        private UInt32 SendAboneCsc(UInt32 dn, UInt32 alfa)
        {
            UInt32 carry;
            UInt32 rmask, pmask, qmask;
            UInt32 t1, t2, t3, t4;
            UInt32 sr;
            UInt32 i;

            pmask = 1 << 1;
            rmask = 1 << 2;
            qmask = 1 << 3;

            sr = dn;
            sr = sr + alfa;
            if (sr == 0) sr = 1;
            for (i = 0; i < 16; i++)
            {
                if ((sr & pmask) != 0) t1 = 1; else t1 = 0;
                if ((sr & rmask) != 0) t2 = 1; else t2 = 0;
                if ((sr & qmask) != 0) t3 = 1; else t3 = 0;
                if ((sr & 0x8000) != 0) t4 = 1; else t4 = 0;
                carry = t1 ^ t2 ^ t3 ^ t4;
                sr <<= 1;
                if (carry != 0)
                {
                    sr |= 1;
                }
            }
            return sr;
        }

        #region Abone Kartı Fonksyonları
        public string KrediYaz(uint devNo, uint AnaKredi, uint YedekKredi, ITarife trf, byte Bayram1Gunn, byte Bayram1Ayy, byte Bayram1Suree, byte Bayram2Gunn, byte Bayram2Ayy, byte Bayram2Suree,uint KritikKredi)
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            if (((trf.Fiyat1 == trf.Fiyat2) && (trf.Fiyat2 == trf.Fiyat3)) || (trf.Lim1 >= 5000000) || (trf.Lim2 >= 5000000))
            {
                trf.Lim1 = 10000;
                trf.Lim2 = 10000;

                trf.Fiyat1 = 1;
                trf.Fiyat2 = 1;
                trf.Fiyat3 = 1;
            }

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";
            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'A') || (inBuf[3] != ' '))
            {
                FinishCard();
                return "0";
            }

            UInt32 sr;

            sr = SendAboneCsc(devNo, 0X3D3D);
            command[0] = (byte)(sr / 256);
            command[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devNo, 0X5A5A);
            command[2] = (byte)(sr / 256);
            command[3] = (byte)(sr % 256);

            i = Verify(7, command);
            if (i == 0) return "0";

            sr = SendAboneCsc(devNo, 0X2F2F);
            command[0] = (byte)(sr / 256);
            command[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devNo, 0X1515);
            command[2] = (byte)(sr / 256);
            command[3] = (byte)(sr % 256);

            i = Verify(0X39, command);
            if (i == 0) return "0";

            sr = SendAboneCsc(devNo, 0XABAB);
            command[0] = (byte)(sr / 256);
            command[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devNo, 0XC2C2);
            command[2] = (byte)(sr / 256);
            command[3] = (byte)(sr % 256);

            i = Verify(0X3B, command);
            if (i == 0) return "0";


            i = ReadCard(0X3F);
            if (i == 0) return "0";

            Hexcon.ByteToBit learnKart = new Hexcon.ByteToBit(inBuf[2]);
            UInt32 islem = (UInt32)inBuf[3];

            if (((int)inBuf[0] & (0X80)) == 0) islem++;

            if (learnKart.Bit7 != 1) // yeni kart değilse artır
            {
                if (((int)inBuf[0] & (0X40)) == 0) islem++;
            }
            learnKart.Deger = Hexcon.SetBitZero(learnKart.Deger); // hata set
            learnKart.Deger = Hexcon.SetBitBir(learnKart.Deger, 0X30); // major set

            command[0] = 0;
            command[1] = 0;
            command[2] = 0;
            command[3] = 0;

            i = Update(0X36, command);
            if (i == 0) return "0";

            i = Update(0X37, command);
            if (i == 0) return "0";

            i = Update(0X28, command);
            if (i == 0) return "0";

            i = Update(0X2A, command);
            if (i == 0) return "0";

            i = Update(0X2B, command);
            if (i == 0) return "0";

            i = Update(0X31, command);
            if (i == 0) return "0";

            i = Update(0X29, command);
            if (i == 0) return "0";

            UInt32 Kad1 = 10000;
            UInt32 Kad2 = 10000 * trf.Fiyat2 / trf.Fiyat1;
            UInt32 Kad3 = 10000 * trf.Fiyat3 / trf.Fiyat1;

            Hexcon.UInt32toByte4(Kad1, command);
            i = Update(0X10, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(Kad2, command);
            i = Update(0X11, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(Kad3, command);
            i = Update(0X12, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(trf.Lim1, command);
            i = Update(0X13, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(trf.Lim2, command);
            i = Update(0X14, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(0, command);
            command[0] = (byte)trf.LoadLimit;
            command[1] = (byte)trf.Aksam;
            command[2] = (byte)trf.Sabah;
            i = Update(0X15, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(0, command);
            command[0] = (byte)trf.HaftaSonuAksam;
            i = Update(0X16, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(trf.FixCharge, command);
            i = Update(0X17, command);
            if (i == 0) return "0";

            UInt16 Bayram1, Bayram2;

            Bayram1 = Bayram1Suree;
            Bayram1 <<= 5;
            Bayram1 |= Bayram1Gunn;
            Bayram1 <<= 4;
            Bayram1 |= Bayram1Ayy;

            Bayram2 = Bayram2Suree;
            Bayram2 <<= 5;
            Bayram2 |= Bayram2Gunn;
            Bayram2 <<= 4;
            Bayram2 |= Bayram2Ayy;

            Integer2Byte Bayram1Deger = new Integer2Byte(Bayram1);
            Integer2Byte Bayram2Deger = new Integer2Byte(Bayram2);

            command[0] = Bayram1Deger.bir;
            command[1] = Bayram1Deger.iki;
            command[2] = Bayram2Deger.bir;
            command[3] = Bayram2Deger.iki;
            i = Update(0X2C, command);
            if (i == 0) return "0";

            //Hexcon.UInt32toByte4(trf.OtherCharge, command);
            //i = Update(0X19, command);
            //if (i == 0) return "0";

            //Hexcon.UInt32toByte4(trf.DailyDebt, command);
            //i = Update(0X2B, command);
            //if (i == 0) return "0";

            //Hexcon.UInt32toByte4(trf.TotalDebt, command);
            //i = Update(0X2E, command);
            //if (i == 0) return "0";

            //i = ReadCard(0X2C);
            //if (i == 0) return "0";

            //command[0] = (byte)trf.DebtMode;
            //command[1] = inBuf[1];
            //i = Update(0X2C, command);
            //if (i == 0) return "0";

            //command[0] = 0;
            //command[1] = 0;
            //command[2] = 0;
            //command[3] = 0;

            //byte[] lowV = new byte[2];
            //Hexcon.UInt16toByte2(trf.LowVoltage, lowV);

            //byte[] highV = new byte[2];
            //Hexcon.UInt16toByte2(trf.HighVoltage, highV);

            //Array.Copy(lowV, 0, command, 0, 2);
            //Array.Copy(highV, 0, command, 2, 2);

            //i = Update(0X1B, command);
            //if (i == 0) return "0";

            //command[0] = Convert.ToByte(trf.BuzzerInterval);
            //command[1] = Convert.ToByte(trf.BuzzerDuration);
            //command[2] = 0x0;
            //command[3] = 0x0;

            //i = Update(0X1C, command);
            //if (i == 0) return "0";

            if (learnKart.Bit7 == 1)
            {
                AnaKredi = AnaKredi + YedekKredi;
                command[0] = 0XBF;
            }
            else command[0] = 0XC0;

            //byte[] buffer = new byte[4];
            //i = ReadCard(0X3F);
            //if (i == 0) return "0";
            //buffer[0] = inBuf[1];

            command[1] = 0XFF;//buffer[0];
            command[2] = (byte)learnKart.Deger;
            command[3] = (byte)islem;

            i = Update(0X3F, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(AnaKredi, command);
            i = Update(0X35, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(YedekKredi, command);
            i = Update(0X34, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(KritikKredi, command);
            i = Update(0X2E, command);
            if (i == 0) return "0";


            FinishCard();

            return "1";
        }

        public string AboneOku()
        {
            string str = "";
            byte[] issue_area = GetIssuer(zone);
            int i = 0;

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";
            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'A') || (inBuf[3] != ' '))
            {
                FinishCard();
                return "0";
            }

            i = ReadCard(0X30);
            if (i == 0) return "0";
            UInt32 devNo = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X10);
            if (i == 0) return "0";
            UInt32 kad1 = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X11);
            if (i == 0) return "0";
            UInt32 kad2 = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X12);
            if (i == 0) return "0";
            UInt32 kad3 = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X13);
            if (i == 0) return "0";
            UInt32 lim1 = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X14);
            if (i == 0) return "0";
            UInt32 lim2 = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X15);
            if (i == 0) return "0";

            UInt32 loadLim = (UInt32)inBuf[0];
            UInt32 aksam = (UInt32)inBuf[1];
            UInt32 sabah = (UInt32)inBuf[2];
            UInt32 kademe = (UInt32)inBuf[3];

            i = ReadCard(0X16);
            if (i == 0) return "0";
            UInt32 hSonuAksam = (UInt32)inBuf[0];

            i = ReadCard(0X17);
            if (i == 0) return "0";
            UInt32 fixCharge = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X18);
            if (i == 0) return "0";
            UInt32 totalFixCharge = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            //i = ReadCard(0X19);
            //if (i == 0) return "0";
            //UInt32 otherCharge = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            //i = ReadCard(0X1A);
            //if (i == 0) return "0";
            //UInt32 otherTotalCharge = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            //i = ReadCard(0X2B);
            //if (i == 0) return "0";
            //UInt32 dailyDebt = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            //i = ReadCard(0X2F);
            //if (i == 0) return "0";
            //UInt32 remainingDebt = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            //i = ReadCard(0X2E);
            //if (i == 0) return "0";
            //UInt32 totalDebt = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            //i = ReadCard(0X2C);
            //if (i == 0) return "0";
            //UInt32 debtMode = inBuf[0];
            //UInt32 Reverse = inBuf[1];

            //i = ReadCard(0X1B);
            //if (i == 0) return "0";
            //UInt32 lowVoltage = (UInt32)Hexcon.ByteToDecimal(inBuf, 2);

            //byte[] high = new byte[2];
            //Array.Copy(inBuf, 2, high, 0, 2);
            //UInt32 highVoltage = (UInt32)Hexcon.ByteToDecimal(high, 2);

            //i = ReadCard(0X1C);
            //if (i == 0) return "0";
            //int buzzerInterval = inBuf[0];
            //int buzzerDuration = inBuf[1];

            i = ReadCard(0X35);
            if (i == 0) return "0";
            UInt32 elkAna = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X36);
            if (i == 0) return "0";
            UInt32 elkKalan = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X29);
            if (i == 0) return "0";
            UInt32 elkHarcanan = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X37);
            if (i == 0) return "0";
            UInt32 k1Tuk = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X28);
            if (i == 0) return "0";
            UInt32 k2Tuk = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X2A);
            if (i == 0) return "0";
            UInt32 k3Tuk = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X31);
            if (i == 0) return "0";
            UInt32 gerTuk = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X02);
            if (i == 0) return "0";
            UInt32 elkEkim = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X03);
            if (i == 0) return "0";
            UInt32 elkAralik = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X1E);
            if (i == 0) return "0";
            string elkTarih = Hexcon.TarihOlustur(Hexcon.ByteDiziOlustur(inBuf, 0, 2));

            i = ReadCard(0X3F);
            if (i == 0) return "0";

            string ako = "";
            string yko = "";

            if (((int)inBuf[0] & (int)0X80) == 0) ako = "*"; else ako = "b";
            if (((int)inBuf[0] & (int)0X40) == 0) yko = "*"; else yko = "b";

            string KlemensCeza = "";
            string Ariza = "";
            string PilZayif = "";
            string PilBitik = "";

            Hexcon.ByteToBit state = new Hexcon.ByteToBit(inBuf[1]);
            KlemensCeza = state.Bit1.ToString();
            Ariza = state.Bit2.ToString();
            PilZayif = state.Bit4.ToString();
            PilBitik = state.Bit5.ToString();

            string yeniKart = "";
            if (((int)inBuf[2] & (int)0X40) == 0) yeniKart = "0"; else yeniKart = "1";


            i = ReadCard(0X34);
            if (i == 0) return "0";
            UInt32 yedekkredi = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);
            i = ReadCard(0X2E);
            if (i == 0) return "0";
            UInt32 kritikkredi = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);


            str += sprtr + devNo;
            str += sprtr + elkAna;
            str += sprtr + ako;
            str += sprtr + yko;
            str += sprtr + yeniKart;
            str += sprtr + elkKalan;
            str += sprtr + elkHarcanan;
            str += sprtr + elkTarih;
            str += sprtr + KlemensCeza;
            str += sprtr + Ariza;
            str += sprtr + PilZayif;
            str += sprtr + PilBitik;
            str += sprtr + k1Tuk;
            str += sprtr + k2Tuk;
            str += sprtr + k3Tuk;
            str += sprtr + gerTuk;
            str += sprtr + elkEkim;
            str += sprtr + elkAralik;
            str += sprtr + kad1;
            str += sprtr + kad2;
            str += sprtr + kad3;
            str += sprtr + lim1;
            str += sprtr + lim2;
            str += sprtr + loadLim;
            str += sprtr + aksam;
            str += sprtr + sabah;
            str += sprtr + kademe;
            str += sprtr + hSonuAksam;
            str += sprtr + fixCharge;
            str += sprtr + totalFixCharge;
            str += sprtr + yedekkredi;
            str += sprtr + kritikkredi;
            //str += sprtr + otherCharge;
            //str += sprtr + otherTotalCharge;
            //str += sprtr + dailyDebt;
            //str += sprtr + totalDebt;
            //str += sprtr + remainingDebt;
            //str += sprtr + Reverse;
            //str += sprtr + buzzerInterval;
            //str += sprtr + buzzerDuration;
            //str += sprtr + lowVoltage;
            //str += sprtr + highVoltage;

            FinishCard();

            return "1" + str;
        }

        public string KrediOku()
        {
            string str = "";
            byte[] issue_area = GetIssuer(zone);
            int i = 0;

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";
            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'A') || (inBuf[3] != ' '))
            {
                FinishCard();
                return "0";
            }

            i = ReadCard(0X30);
            if (i == 0) return "0";
            UInt32 devNo = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X35);
            if (i == 0) return "0";
            UInt32 elkAna = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X3F);
            if (i == 0) return "0";

            string ako = "";
            string yko = "";

            if (((int)inBuf[0] & (int)0X80) == 0) ako = "*"; else ako = "b";
            if (((int)inBuf[0] & (int)0X40) == 0) yko = "*"; else yko = "b";

            string yeniKart = "";
            if (((int)inBuf[2] & (int)0X40) == 0) yeniKart = "0"; else yeniKart = "1";

            i = ReadCard(0X17);
            if (i == 0) return "0";
            UInt32 fixCharge = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X18);
            if (i == 0) return "0";
            UInt32 totalFixCharge = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            FinishCard();

            str += sprtr + devNo;
            str += sprtr + elkAna;
            str += sprtr + ako;
            str += sprtr + yko;
            str += sprtr + yeniKart;
            str += sprtr + fixCharge;
            str += sprtr + totalFixCharge;

            return "1" + str;

        }

        public string AboneYap(uint devNo, ITarife trf, byte Bayram1Gunn, byte Bayram1Ayy, byte Bayram1Suree, byte Bayram2Gunn, byte Bayram2Ayy, byte Bayram2Suree, uint KritikKredi)
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;

            if (((trf.Fiyat1 == trf.Fiyat2) && (trf.Fiyat2 == trf.Fiyat3)) || (trf.Lim1 >= 5000000) || (trf.Lim2 >= 5000000))
            {
                trf.Lim1 = 10000;
                trf.Lim2 = 10000;

                trf.Fiyat1 = 1;
                trf.Fiyat2 = 1;
                trf.Fiyat3 = 1;
            }

            i = InitCard();
            if (i == 0) return "0";

            byte[] command = new byte[4];


            command[0] = 0X79;
            command[1] = 0X81;
            command[2] = 0X17;
            command[3] = 0X2F;
            i = Verify(0X07, command);
            if (i == 0) return "0";

            command[0] = 0X2E;
            command[1] = 0X4C;
            command[2] = 0X16;
            command[3] = 0X41;
            i = Verify(0X39, command);
            if (i == 0) return "0";

            command[0] = 0X22;
            command[1] = 0X22;
            command[2] = 0X22;
            command[3] = 0X22;
            i = Verify(0X3B, command);
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";
            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'A') || (inBuf[3] != ' '))
            {
                command[0] = issue_area[0];
                command[1] = issue_area[1];
                command[2] = (byte)'A';
                command[3] = (byte)' ';

                i = Update(1, command);
                if (i == 0) return "0";
            }

            UInt32 sr;

            sr = SendAboneCsc(devNo, 0X3D3D);
            command[0] = (byte)(sr / 256);
            command[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devNo, 0X5A5A);
            command[2] = (byte)(sr / 256);
            command[3] = (byte)(sr % 256);

            i = Update(6, command);
            if (i == 0) return "0";

            sr = SendAboneCsc(devNo, 0X2F2F);
            command[0] = (byte)(sr / 256);
            command[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devNo, 0X1515);
            command[2] = (byte)(sr / 256);
            command[3] = (byte)(sr % 256);

            i = Update(0X38, command);
            if (i == 0) return "0";

            sr = SendAboneCsc(devNo, 0XABAB);
            command[0] = (byte)(sr / 256);
            command[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devNo, 0XC2C2);
            command[2] = (byte)(sr / 256);
            command[3] = (byte)(sr % 256);

            i = Update(0X3A, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(devNo, command);
            i = Update(0X30, command);
            if (i == 0) return "0";

            UInt32 kad1 = 10000;
            UInt32 kad2 = 10000 * trf.Fiyat2 / trf.Fiyat1;
            UInt32 kad3 = 10000 * trf.Fiyat3 / trf.Fiyat1;

            Hexcon.UInt32toByte4(kad1, command);
            i = Update(0X10, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(kad2, command);
            i = Update(0X11, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(kad3, command);
            i = Update(0X12, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(trf.Lim1, command);
            i = Update(0X13, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(trf.Lim2, command);
            i = Update(0X14, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(0, command);
            command[0] = (byte)trf.LoadLimit;
            command[1] = (byte)trf.Aksam;
            command[2] = (byte)trf.Sabah;
            i = Update(0X15, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(0, command);
            command[0] = (byte)trf.HaftaSonuAksam;
            i = Update(0X16, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(trf.FixCharge, command);
            i = Update(0X17, command);
            if (i == 0) return "0";


            UInt16 Bayram1, Bayram2;

            Bayram1 = Bayram1Suree;
            Bayram1 <<= 5;
            Bayram1 |= Bayram1Gunn;
            Bayram1 <<= 4;
            Bayram1 |= Bayram1Ayy;

            Bayram2 = Bayram2Suree;
            Bayram2 <<= 5;
            Bayram2 |= Bayram2Gunn;
            Bayram2 <<= 4;
            Bayram2 |= Bayram2Ayy;

            Integer2Byte Bayram1Deger = new Integer2Byte(Bayram1);
            Integer2Byte Bayram2Deger = new Integer2Byte(Bayram2);

            command[0] = Bayram1Deger.bir;
            command[1] = Bayram1Deger.iki;
            command[2] = Bayram2Deger.bir;
            command[3] = Bayram2Deger.iki;
            i = Update(0X2C, command);
            if (i == 0) return "0";


            //Hexcon.UInt32toByte4(trf.OtherCharge, command);
            //i = Update(0X19, command);
            //if (i == 0) return "0";

            //Hexcon.UInt32toByte4(trf.DailyDebt, command);
            //i = Update(0X2B, command);
            //if (i == 0) return "0";

            //Hexcon.UInt32toByte4(trf.TotalDebt, command);
            //i = Update(0X2E, command);
            //if (i == 0) return "0";

            //command[0] = (byte)trf.DebtMode;
            //i = Update(0X2C, command);
            //if (i == 0) return "0";

            int ekarno = 0;
            ekarno &= 0X0F;
            ekarno |= 0X70;

            command[0] = 0XBF;  //  ff idi yeni kart hazirlarken okunmadi-okundu olarak hazirla
            command[1] = 0XFF; // sayaç state
            command[2] = (byte)ekarno;
            command[3] = 0;
            i = Update(0X3F, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(KritikKredi, command);
            i = Update(0X2E, command);
            if (i == 0) return "0";

            //command[0] = 0;
            //command[1] = 0;
            //command[2] = 0;
            //command[3] = 0;

            #region eklenti buzzer ayarları
            //byte[] lowV = new byte[2];
            //Hexcon.UInt16toByte2(trf.LowVoltage, lowV);

            //byte[] highV = new byte[2];
            //Hexcon.UInt16toByte2(trf.HighVoltage, highV);

            //Array.Copy(lowV, 0, command, 0, 2);
            //Array.Copy(highV, 0, command, 2, 2);

            //i = Update(0X1B, command);
            //if (i == 0) return "0";

            //command[0] = Convert.ToByte(trf.BuzzerInterval);
            //command[1] = Convert.ToByte(trf.BuzzerDuration);
            //command[2] = 0x0;
            //command[3] = 0x0;

            //i = Update(0X1C, command);
            //if (i == 0) return "0";

            #endregion

            //command[0] = 0;
            //command[1] = 0;
            //command[2] = 0;
            //command[3] = 0;
            ////i = Update(0X2D, command);
            ////if (i == 0) return "0";

            //for (int t = 0X19; t < 0X20; t++)
            //{
            //    i = Update((byte)t, command);
            //    if (i == 0) return "0";
            //}

            //for (int j = 0X2B; j < 0X38; j++)
            //{
            //    if (j == 0x30) j = 0x31;
            //    else if (j == 0x2c) j = 0x2d;
            //    else if (j == 0x2f) j = 0x31;

            //    i = Update((byte)j, command);
            //    if (i == 0) return "0";

            //}

            FinishCard();
            return "1"; 
        }

        public string AboneBosalt()
        {
            string str = "";
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";
            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'A') || (inBuf[3] != ' '))
            {
                FinishCard();
                return "0";
            }

            i = ReadCard(0X30);
            if (i == 0) return "0";

            UInt32 devNo = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            UInt32 sr;

            sr = SendAboneCsc(devNo, 0X3D3D);
            command[0] = (byte)(sr / 256);
            command[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devNo, 0X5A5A);
            command[2] = (byte)(sr / 256);
            command[3] = (byte)(sr % 256);

            i = Verify(7, command);
            if (i == 0) return "0";

            sr = SendAboneCsc(devNo, 0X2F2F);
            command[0] = (byte)(sr / 256);
            command[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devNo, 0X1515);
            command[2] = (byte)(sr / 256);
            command[3] = (byte)(sr % 256);

            i = Verify(0X39, command);
            if (i == 0) return "0";

            sr = SendAboneCsc(devNo, 0XABAB);
            command[0] = (byte)(sr / 256);
            command[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devNo, 0XC2C2);
            command[2] = (byte)(sr / 256);
            command[3] = (byte)(sr % 256);

            i = Verify(0X3B, command);
            if (i == 0) return "0";


            command[0] = 0;
            command[1] = 0;
            command[2] = 0;
            command[3] = 0;

            for (int j = 0x10; j < 0x20; j++)
            {
                i = Update((byte)j, command);
                if (i == 0) return "0";
            }

            for (int j = 0x28; j < 0x40; j++)
            {
                if (j == 0x38) j = 0x3C;
                i = Update((byte)j, command);
                if (i == 0) return "0";
            }

            i = Update(1, command);
            if (i == 0) return "0";

            command[0] = 0x79;
            command[1] = 0x81;
            command[2] = 0x17;
            command[3] = 0x2F;
            i = Update(6, command);
            if (i == 0) return "0";

            command[0] = 0x2E;
            command[1] = 0x4C;
            command[2] = 0x16;
            command[3] = 0x41;
            i = Update(0X38, command);
            if (i == 0) return "0";

            command[0] = 0x22;
            command[1] = 0x22;
            command[2] = 0x22;
            command[3] = 0x22;
            i = Update(0X3A, command);
            if (i == 0) return "0";

            FinishCard();
            return "1"; 
        }

        public string SifreExeHazirla()
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";
            if ((inBuf[0] != 0) || (inBuf[1] != 0) || (inBuf[2] != 0) || (inBuf[3] != 0))
            {
                FinishCard();
                return "0";
            }

            command[0] = 0XAA;
            command[1] = 0XAA;
            command[2] = 0XAA;
            command[3] = 0XAA;

            i = Verify(0X07, command);
            if (i == 1)
            {
                command[0] = 0X11;
                command[1] = 0X11;
                command[2] = 0X11;
                command[3] = 0X11;

                i = Verify(0X39, command);

                if (i == 1)
                {
                    command[0] = 0X79;
                    command[1] = 0X81;
                    command[2] = 0X17;
                    command[3] = 0X2F;

                    i = Update(0x06, command);
                    
                    if (i == 1)
                    {
                        command[0] = 0X2E;
                        command[1] = 0X4C;
                        command[2] = 0X16;
                        command[3] = 0X41;

                        i = Update(0X38, command);
                        FinishCard();
                        return i.ToString();
                    }
                }
            }
            FinishCard();
            return "0";

        }

        public string KartSifreBosalt()
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";
            if ((inBuf[0] != 0) || (inBuf[1] != 0) || (inBuf[2] != 0) )
            {
                FinishCard();
                return "0";
            }

            command[0] = 0X79;
            command[1] = 0X81;
            command[2] = 0X17;
            command[3] = 0X2F;

            i = Verify(0X07, command);
            if (i == 0) { return "0"; }

            command[0] = 0X2E;
            command[1] = 0X4C;
            command[2] = 0X16;
            command[3] = 0X41;

            i = Verify(0X39, command);
            if (i == 0) {  return "0"; }

            command[0] = 0XAA;
            command[1] = 0XAA;
            command[2] = 0XAA;
            command[3] = 0XAA;

            i = Update(6, command);
            if (i == 0) { return "0"; }

            command[0] = 0X11;
            command[1] = 0X11;
            command[2] = 0X11;
            command[3] = 0X11;

            i = Update(0X38, command);            
            if (i == 0) { return "0"; }

            FinishCard();

            return "1";
        }

        public string KartTipi()
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            FinishCard();

            //string stIssuer = Convert.ToChar(inBuf[0]).ToString() + Convert.ToChar(inBuf[1]).ToString() + Convert.ToChar(inBuf[2]).ToString() + Convert.ToChar(inBuf[3]).ToString();

            //if ((inBuf[0] == 0) || (inBuf[1] == 0) || (inBuf[2] == 0))
            //{
            //    stIssuer = "Bos Kart";
            //}

            //return stIssuer;

            return TipTanimlama(Convert.ToChar(inBuf[0]).ToString() + Convert.ToChar(inBuf[1]).ToString() + Convert.ToChar(inBuf[2]).ToString() + Convert.ToChar(inBuf[3]).ToString());
        }
        

        public string TipTanimlama(string IssuerArea)
        {
            byte[] issue_area = GetIssuer(zone);        

            string stIssuer = Convert.ToChar(issue_area[0]).ToString() + Convert.ToChar(issue_area[1]).ToString();

            if (DilSecimi == EnmDil.English)
            {
                if (stIssuer + "A " == IssuerArea)
                {
                    IssuerArea = "Subscriber Card";
                    return IssuerArea;
                }
                if (stIssuer + "YA" == IssuerArea)
                {
                    IssuerArea = "Open Card";
                    return IssuerArea;
                }
                if (stIssuer + "YE" == IssuerArea)
                {
                    IssuerArea = "Info Card";
                    return IssuerArea;
                }
                if (stIssuer + "YI" == IssuerArea)
                {
                    IssuerArea = "Payback Card";
                    return IssuerArea;
                }
                if (stIssuer + "YK" == IssuerArea)
                {
                    IssuerArea = "Close Card";
                    return IssuerArea;
                }
                if (stIssuer + "YR" == IssuerArea)
                {
                    IssuerArea = "Reset Card";
                    return IssuerArea;
                }
                if (stIssuer + "YT" == IssuerArea)
                {
                    IssuerArea = "EChange Card";
                    return IssuerArea;
                }
                if (stIssuer + "YS" == IssuerArea)
                {
                    IssuerArea = "Time Card";
                    return IssuerArea;
                }
                if (stIssuer + "Y4" == IssuerArea)
                {
                    IssuerArea = "Penalty 4 Card";
                    return IssuerArea;
                }
                if (stIssuer + "YV" == IssuerArea)
                {
                    IssuerArea = "Advance Card";
                    return IssuerArea;
                }
                if (stIssuer + "YC" == IssuerArea)
                {
                    IssuerArea = "Cancellation Card";
                    return IssuerArea;
                }

                switch (IssuerArea)
                {
                    case "ALUS":
                        IssuerArea = "Üretim Sıfırlama";
                        break;
                    case "ALUR":
                        IssuerArea = "Üretim Reel";
                        break;
                    case "ALUT":
                        IssuerArea = "Üretim Test";
                        break;
                    case "ALUA":
                        IssuerArea = "Üretim Açma";
                        break;
                    case "ALUK":
                        IssuerArea = "Üretim Kapama";
                        break;
                    case "ALUF":
                        IssuerArea = "Üretim Format";
                        break;
                    case "ALUZ":
                        IssuerArea = "Üretim Cihaz No";
                        break;
                    case "ALUI":
                        IssuerArea = "Zone Kartı";
                        break;
                    case "BTLD":
                        IssuerArea = "BootLoader";
                        break;
                    case "\0\0\0\0":
                        IssuerArea = "Empty Card";
                        break;
                }
                
            }
            else
            {
                if (stIssuer + "A " == IssuerArea)
                {
                    IssuerArea = "Abone Kartı";
                    return IssuerArea;
                }
                if (stIssuer + "YA" == IssuerArea)
                {
                    IssuerArea = "Yetki Açma Kartı";
                    return IssuerArea;
                }
                if (stIssuer + "YE" == IssuerArea)
                {
                    IssuerArea = "Yetki Bilgi Kartı";
                    return IssuerArea;
                }
                if (stIssuer + "YI" == IssuerArea)
                {
                    IssuerArea = "Yetki Iade Kartı";
                    return IssuerArea;
                }
                if (stIssuer + "YK" == IssuerArea)
                {
                    IssuerArea = "Yetki Kapatma Kartı";
                    return IssuerArea;
                }
                if (stIssuer + "YR" == IssuerArea)
                {
                    IssuerArea = "Yetki Reset Kartı";
                    return IssuerArea;
                }
                if (stIssuer + "YT" == IssuerArea)
                {
                    IssuerArea = "EEDegiş Kartı";
                    return IssuerArea;
                }
                if (stIssuer + "YS" == IssuerArea)
                {
                    IssuerArea = "Yetki Saat Kartı";
                    return IssuerArea;
                }
                if (stIssuer + "Y4" == IssuerArea)
                {
                    IssuerArea = "Yetki Ceza 4 Kartı";
                    return IssuerArea;
                }
                if (stIssuer + "YV" == IssuerArea)
                {
                    IssuerArea = "Yetki Avans Kartı";
                    return IssuerArea;
                }
                if (stIssuer + "YC" == IssuerArea)
                {
                    IssuerArea = "Yetki Iptal Kartı";
                    return IssuerArea;
                }

                switch (IssuerArea)
                {
                    case "ALUS":
                        IssuerArea = "Üretim Sıfırlama";
                        break;
                    case "ALUR":
                        IssuerArea = "Üretim Reel";
                        break;
                    case "ALUT":
                        IssuerArea = "Üretim Test";
                        break;
                    case "ALUA":
                        IssuerArea = "Üretim Açma";
                        break;
                    case "ALUK":
                        IssuerArea = "Üretim Kapama";
                        break;
                    case "ALUF":
                        IssuerArea = "Üretim Format";
                        break;
                    case "ALUZ":
                        IssuerArea = "Üretim Cihaz No";
                        break;
                    case "ALUI":
                        IssuerArea = "Zone Kartı";
                        break;
                    case "BTLD":
                        IssuerArea = "BootLoader";
                        break;
                    case "\0\0\0\0":
                        IssuerArea = "Bos Kart";
                        break;
                }
            
            }
            return IssuerArea;

        }


        public string Eject()
        {
            return "0";
        }
        #endregion 

        #region Yetki Kartı Fonksyonları
        public string FormYet()
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != 0 ) || (inBuf[1] != 0 ) || (inBuf[2] != 0 ))
            {
                FinishCard();
                return "0";
            }

            command[0] = 0xAA;
            command[1] = 0xAA;
            command[2] = 0xAA;
            command[3] = 0xAA;

            i = Verify(7, command);
            if (i == 0) return "0";

            command[0] = issue_area[0];
            command[1] = issue_area[1];
            command[2] = (byte)'Y';
            command[3] = (byte)'A';

            i = Update(1, command);
            if (i == 0) return "0";

            command[0] = 0x3A;
            command[1] = 0xDF;
            command[2] = 0x1D;
            command[3] = 0x80;

            i = Update(6, command);
            if (i == 0) return "0";

            FinishCard();

            return "1";

        }

        public string YetkiSaat()
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                FinishCard();
                return "0";
            }

            command[0] = 0x3A;
            command[1] = 0xDF;
            command[2] = 0x1D;
            command[3] = 0x80;

            i = Verify(7, command);
            if (i == 0) return "0";

            command[0] = issue_area[0];
            command[1] = issue_area[1];
            command[2] = (byte)'Y';
            command[3] = (byte)'S';

            i = Update(1, command);
            if (i == 0) return "0";

            DateTime tar = DateTime.Now;

            TarihAl tarih = new TarihAl(tar);
            command = tarih.TarihDondur4Byte(tar);

            i = Update(0x10, command);
            if (i == 0) return "0";

            command[0] = 0;
            command[1] = 0;
            command[2] = 0;
            command[3] = 0;

            i = Update(0x12, command);
            if (i == 0) return "0";

            i = ReadCard(0X3D);
            if (i == 0) return "0";

            DayOfWeek dow = tar.DayOfWeek;

            command[0] = (byte)dow;
            command[1] = inBuf[1];
            command[2] = 0;
            command[3] = inBuf[3];

            i = Update(0x3D, command);
            if (i == 0) return "0";


            FinishCard();

            return "1";
        }

        public string YetkiSaat(DateTime date)
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                FinishCard();
                return "0";
            }

            command[0] = 0x3A;
            command[1] = 0xDF;
            command[2] = 0x1D;
            command[3] = 0x80;

            i = Verify(7, command);
            if (i == 0) return "0";

            command[0] = issue_area[0];
            command[1] = issue_area[1];
            command[2] = (byte)'Y';
            command[3] = (byte)'S';

            i = Update(1, command);
            if (i == 0) return "0";

            DateTime tar = date;

            TarihAl tarih = new TarihAl(tar);
            command = tarih.TarihDondur4Byte(tar);

            i = Update(0x10, command);
            if (i == 0) return "0";

            command[0] = 0;
            command[1] = 0;
            command[2] = 0;
            command[3] = 0;

            i = Update(0x12, command);
            if (i == 0) return "0";

            i = ReadCard(0X3D);
            if (i == 0) return "0";

            DayOfWeek dow = tar.DayOfWeek;

            command[0] = (byte)dow;
            command[1] = inBuf[1];
            command[2] = 0;
            command[3] = inBuf[3];

            i = Update(0x3D, command);
            if (i == 0) return "0";


            FinishCard();

            return "1";
        }

        public string YetkiAc(uint devNo)
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                FinishCard();
                return "0";
            }

            command[0] = 0x3A;
            command[1] = 0xDF;
            command[2] = 0x1D;
            command[3] = 0x80;

            i = Verify(7, command);
            if (i == 0) return "0";

            command[0] = issue_area[0];
            command[1] = issue_area[1];
            command[2] = (byte)'Y';
            command[3] = (byte)'A';

            i = Update(1, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(devNo, command);
            i = Update(0X3C, command);
            if (i == 0) return "0";

            command[0] = 0;
            command[1] = 0;
            command[2] = 0;
            command[3] = 0;

            i = Update(0X10, command);
            if (i == 0) return "0";

            i = Update(0X3D, command);
            if (i == 0) return "0";


            FinishCard();

            return "1";
        }

        public string YetkiKapat(uint devNo)
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                FinishCard();
                return "0";
            }

            command[0] = 0x3A;
            command[1] = 0xDF;
            command[2] = 0x1D;
            command[3] = 0x80;

            i = Verify(7, command);
            if (i == 0) return "0";

            command[0] = issue_area[0];
            command[1] = issue_area[1];
            command[2] = (byte)'Y';
            command[3] = (byte)'K';

            i = Update(1, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(devNo, command);
            i = Update(0X3C, command);
            if (i == 0) return "0";

            command[0] = 0;
            command[1] = 0;
            command[2] = 0;
            command[3] = 0;

            i = Update(0X10, command);
            if (i == 0) return "0";

            FinishCard();

            return "1";
        }

        public string YetkiBilgiYap()
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                FinishCard();
                return "0";
            }

            command[0] = 0x3A;
            command[1] = 0xDF;
            command[2] = 0x1D;
            command[3] = 0x80;

            i = Verify(7, command);
            if (i == 0) return "0";

            command[0] = issue_area[0];
            command[1] = issue_area[1];
            command[2] = (byte)'Y';
            command[3] = (byte)'E';

            i = Update(1, command);
            if (i == 0) return "0";

            command[0] = 0XFF;
            command[1] = 0XFF;
            command[2] = 0XFF;
            command[3] = 0XFF;

            i = Update(0X3D, command);
            if (i == 0) return "0";

            command[0] = 0;
            command[1] = 0;
            command[2] = 0;
            command[3] = 0;

            i = Update(0X3E, command);
            if (i == 0) return "0";

            i = Update(0X10, command);
            if (i == 0) return "0";

            i = Update(0X11, command);
            if (i == 0) return "0";

            i = Update(0X12, command);
            if (i == 0) return "0";

            i = Update(0X13, command);
            if (i == 0) return "0";

            i = Update(0X14, command);
            if (i == 0) return "0";

            i = Update(0X15, command);
            if (i == 0) return "0";

            i = Update(0X16, command);
            if (i == 0) return "0";

            i = Update(0X17, command);
            if (i == 0) return "0";

            i = Update(0X18, command);
            if (i == 0) return "0";

            i = Update(0X19, command);
            if (i == 0) return "0";

            i = Update(0X3C, command);
            if (i == 0) return "0";

            FinishCard();

            return "1";

        }

        public string YetkiBilgiOku()
        {
            TarihAl bilgi;
            TarihAl bilgionkap, bilgisonp, bilgiarzt, bilgikr;

            string str = "";
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y') || (inBuf[3] != 'E'))
            {
                FinishCard();
                return "0";
            }

            command[0] = 0x3A;
            command[1] = 0xDF;
            command[2] = 0x1D;
            command[3] = 0x80;

            i = Verify(7, command);
            if (i == 0) return "0";

            i = ReadCard(0X3C);
            if (i == 0) return "0";

            if (inBuf[0] != 0x05)
            {
                FinishCard();
                return "3";
            }

            byte[] temp = new byte[2];

            i = ReadCard(0X10);
            if (i == 0) return "0";

            Array.Copy(inBuf, 0, temp, 0, 2);
            bilgiarzt = new TarihAl(temp);

            buffer[0] = inBuf[2];
            buffer[1] = inBuf[3];

            Array.Copy(buffer, 0, temp, 0, 2);
            bilgisonp = new TarihAl(temp);

            i = ReadCard(0X11);
            if (i == 0) return "0";

            Array.Copy(inBuf, 0, temp, 0, 2);
            bilgionkap = new TarihAl(temp);

            buffer[0] = inBuf[2];
            buffer[1] = inBuf[3];

            Array.Copy(buffer, 0, temp, 0, 2);
            bilgikr = new TarihAl(temp);

            i = ReadCard(0X12);
            if (i == 0) return "0";
            UInt32 lim1 = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X14);
            if (i == 0) return "0";
            UInt32 lim2 = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X15);
            if (i == 0) return "0";
            UInt32 kad1 = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X16);
            if (i == 0) return "0";
            UInt32 kad2 = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X17);
            if (i == 0) return "0";
            UInt32 kad3 = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X18);
            if (i == 0) return "0";

            UInt32 kademe = (UInt32)inBuf[0];
            UInt32 loadLim = (UInt32)inBuf[1];
            UInt32 aksam = (UInt32)inBuf[2];
            UInt32 sabah = (UInt32)inBuf[3];

            i = ReadCard(0X1A);
            if (i == 0) return "0";
            UInt32 hSonuAksam = (UInt32)inBuf[0];

            i = ReadCard(0X1B);
            if (i == 0) return "0";
            UInt32 fixCharge = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X1C);
            if (i == 0) return "0";
            UInt32 totalFixCharge = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            //i = ReadCard(0X1D);
            //if (i == 0) return "0";
            //UInt32 otherCharge = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            //i = ReadCard(0X1E);
            //if (i == 0) return "0";
            //UInt32 otherTotalCharge = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            //i = ReadCard(0X2A);
            //if (i == 0) return "0";
            //UInt32 dailyDebt = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            //i = ReadCard(0X2D);
            //if (i == 0) return "0";
            //UInt32 remainingDebt = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            //i = ReadCard(0X2C);
            //if (i == 0) return "0";
            //UInt32 totalDebt = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X29);
            if (i == 0) return "0";

            string KlemensCeza = "";
            string Ariza = "";
            string PilZayif = "";
            string PilBitik = "";

            Hexcon.ByteToBit state = new Hexcon.ByteToBit(inBuf[3]);
            KlemensCeza = state.Bit1.ToString();
            Ariza = state.Bit2.ToString();
            PilZayif = state.Bit4.ToString();
            PilBitik = state.Bit5.ToString();

            i = ReadCard(0X2D);
            if (i == 0) return "0";
            UInt32 kritikkredi = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);            

            i = ReadCard(0X2E);
            if (i == 0) return "0";
            bilgi = new TarihAl(inBuf);

            i = ReadCard(0X2F);
            if (i == 0) return "0";
            UInt32 gerTuk = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X30);
            if (i == 0) return "0";
            UInt32 elkHarcanan = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X31);
            if (i == 0) return "0";
            UInt32 elkKalan = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X32);
            if (i == 0) return "0";
            UInt32 devNo = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X33);
            if (i == 0) return "0";

            Int32 kartNo = inBuf[0];
            Int32 islem = inBuf[1];
            Int32 hangigun = inBuf[3];

            double pilsev = inBuf[2];
            double pilsev1 = pilsev;
            pilsev1 = pilsev1 / 51;
            pilsev1.ToString("F03");

            i = ReadCard(0X35);
            if (i == 0) return "0";
            UInt32 elkEkim = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X036);
            if (i == 0) return "0";
            UInt32 elkAralik = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X37);
            if (i == 0) return "0";
            UInt32 k1Tuk = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X3D);
            if (i == 0) return "0";
            UInt32 k2Tuk = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X3E);
            if (i == 0) return "0";
            UInt32 k3Tuk = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X19);
            if (i == 0) return "0";
            UInt32 donemTuk = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            str += devNo + sprtr + elkKalan + sprtr + elkHarcanan + sprtr + k1Tuk + sprtr + k2Tuk + sprtr + k3Tuk + sprtr + gerTuk + sprtr + bilgi.gun + "/" + bilgi.ay + "/" + bilgi.yil + " " +
                    bilgi.saat + ":" + bilgi.dakika + sprtr + bilgionkap.gun + "/" + bilgionkap.ay + "/" + bilgionkap.yil + sprtr + bilgisonp.gun + "/" + bilgisonp.ay + "/" + bilgisonp.yil + sprtr +
                    bilgiarzt.gun + "/" + bilgiarzt.ay + "/" + bilgiarzt.yil + sprtr + bilgikr.gun + "/" + bilgikr.ay + "/" + bilgikr.yil + sprtr + hangigun + sprtr +
                    kad1 + sprtr + kad2 + sprtr + kad3 + sprtr + lim1 + sprtr + lim2 + sprtr + "0" + sprtr + loadLim + sprtr + aksam + sprtr + sabah + sprtr + kademe + sprtr +
                    KlemensCeza + sprtr + Ariza + sprtr + PilZayif + sprtr + PilBitik + sprtr + elkEkim + sprtr + elkAralik + sprtr +
                    hSonuAksam + sprtr + fixCharge + sprtr + totalFixCharge + sprtr + kartNo + sprtr + donemTuk + sprtr + islem + sprtr + kritikkredi;


            FinishCard();

            return "1" + sprtr + str;
        }

        public string YetkiIadeYap(uint devNo)
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                FinishCard();
                return "0";
            }

            command[0] = 0x3A;
            command[1] = 0xDF;
            command[2] = 0x1D;
            command[3] = 0x80;

            i = Verify(7, command);
            if (i == 0) return "0";

            command[0] = issue_area[0];
            command[1] = issue_area[1];
            command[2] = (byte)'Y';
            command[3] = (byte)'I';

            i = Update(1, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(devNo, command);
            i = Update(0X3C, command);
            if (i == 0) return "0";

            command[0] = 0XFF;
            command[1] = 0XFF;
            command[2] = 0XFF;
            command[3] = 0XFF;

            i = Update(0X3D, command);
            if (i == 0) return "0";

            command[0] = 0X0;
            command[1] = 0X0;
            command[2] = 0X0;
            command[3] = 0X0;

            i = Update(0X3E, command);
            if (i == 0) return "0";


            i = Update(0x3F, command);
            if (i == 0) return "0";

            FinishCard();

            return "1";


        }

        public string YetkiIadeOku()
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y') || (inBuf[3] != 'I'))
            {
                FinishCard();
                return "0";
            }

            command[0] = 0x3A;
            command[1] = 0xDF;
            command[2] = 0x1D;
            command[3] = 0x80;

            i = Verify(7, command);
            if (i == 0) return "0";

            i = ReadCard(2);
            if (i == 0) return "0";

            int sayacTip = inBuf[0];

            i = ReadCard(0X3C);
            if (i == 0) return "0";

            UInt32 devNo = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X3D);
            if (i == 0) return "0";

            int takilmismi;

            if (inBuf[3] != 0X0) takilmismi = 0;
            else takilmismi = 1;

            i = ReadCard(0X3E);
            if (i == 0) return "0";

            UInt32 kredi = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X3F);
            if (i == 0) return "0";

            UInt32 harcanan = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            if (takilmismi == 0)
            {
                kredi = 0;
                harcanan = 0;
            }


            FinishCard();

            return "1" + sprtr + takilmismi + sprtr + devNo + sprtr + kredi + sprtr + harcanan;

        }

        public string YetkiIptal(uint devNo)
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                FinishCard();
                return "0";
            }

            command[0] = 0x3A;
            command[1] = 0xDF;
            command[2] = 0x1D;
            command[3] = 0x80;

            i = Verify(7, command);
            if (i == 0) return "0";

            command[0] = issue_area[0];
            command[1] = issue_area[1];
            command[2] = (byte)'Y';
            command[3] = (byte)'C';

            i = Update(1, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(devNo, command);
            i = Update(0X3C, command);
            if (i == 0) return "0";

            FinishCard();

            return "1";

        }

        public string EDegis(UInt32 devNo, DateTime kapamaTarih, UInt32 gerTuk, UInt32 kalan, UInt32 harcanan, byte kartNo, byte islemNo,
                             byte kademe, byte loadLimit, byte aksam, byte sabah, byte haftasonuAksam, UInt32 donemTuketim,
                             UInt32 fixCharge, UInt32 totalFixCharge, UInt32 k1Tuk, UInt32 k2Tuk, UInt32 k3Tuk, UInt32 Lim1, UInt32 Lim2,
                             UInt32 kad1, UInt32 kad2, UInt32 kad3,UInt32 kritikKredi)
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                FinishCard();
                return "0";
            }

            command[0] = 0x3A;
            command[1] = 0xDF;
            command[2] = 0x1D;
            command[3] = 0x80;

            i = Verify(7, command);
            if (i == 0) return "0";

            command[0] = issue_area[0];
            command[1] = issue_area[1];
            command[2] = (byte)'Y';
            command[3] = (byte)'E';

            i = Update(1, command);
            if (i == 0) return "0";

            command[0] = 0X0A;  // Edeğiş için 10  // Etrans için 12
            command[1] = 0X0;
            command[2] = 0X0;
            command[3] = 0X0;

            i = Update(0x3C, command);
            if (i == 0) return "0";

            command[0] = 0X0;
            command[1] = 0X0;
            command[2] = 0X0;
            command[3] = 0X0;

            i = Update(0x13, command);
            if (i == 0) return "0";

            //TarihAl kapTarih = new TarihAl(kapamaTarih);
            //command = kapTarih.TarihDondur4Byte(kapamaTarih);

            //i = Update(0x29, command);
            //if (i == 0) return "0";

            Hexcon.UInt32toByte4(gerTuk, command);
            i = Update(0X2F, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(kalan, command);
            i = Update(0X31, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(harcanan, command);
            i = Update(0X30, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(devNo, command);
            i = Update(0X32, command);
            if (i == 0) return "0";

            i = ReadCard(0X33);
            if (i == 0) return "0";

            command[0] = kartNo;
            command[1] = islemNo;
            command[2] = inBuf[2];
            command[3] = inBuf[3];

            i = Update(0X33, command);
            if (i == 0) return "0";

            command[0] = kademe;
            command[1] = loadLimit;
            command[2] = aksam;
            command[3] = sabah;

            i = Update(0X18, command);
            if (i == 0) return "0";


            command[0] = haftasonuAksam;
            command[1] = 0X0;
            command[2] = 0X0;
            command[3] = 0X0;

            i = Update(0X1A, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(fixCharge, command);
            i = Update(0X1B, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(totalFixCharge, command);
            i = Update(0X1C, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(k1Tuk, command);
            i = Update(0X37, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(k2Tuk, command);
            i = Update(0X3D, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(k3Tuk, command);
            i = Update(0X3E, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(Lim1, command);
            i = Update(0X12, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(Lim2, command);
            i = Update(0X14, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(kad1, command);
            i = Update(0X15, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(kad2, command);
            i = Update(0X16, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(kad3, command);
            i = Update(0X17, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(kritikKredi, command);
            i = Update(0X2D, command);
            if (i == 0) return "0";


            FinishCard();

            return "1";
        }

        public string ETrans(UInt32 devNo, DateTime kapamaTarih, UInt32 gerTuk, UInt32 kalan, UInt32 harcanan, byte kartNo, byte islemNo,
                             byte kademe, byte loadLimit, byte aksam, byte sabah, byte haftasonuAksam, UInt32 donemTuketim,
                             UInt32 fixCharge, UInt32 totalFixCharge, UInt32 k1Tuk, UInt32 k2Tuk, UInt32 k3Tuk, UInt32 Lim1, UInt32 Lim2,
                             UInt32 kad1, UInt32 kad2, UInt32 kad3)
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                FinishCard();
                return "0";
            }

            command[0] = 0x3A;
            command[1] = 0xDF;
            command[2] = 0x1D;
            command[3] = 0x80;

            i = Verify(7, command);
            if (i == 0) return "0";

            command[0] = issue_area[0];
            command[1] = issue_area[1];
            command[2] = (byte)'Y';
            command[3] = (byte)'E';

            i = Update(1, command);
            if (i == 0) return "0";

            command[0] = 0X0C;
            command[1] = 0X0;
            command[2] = 0X0;
            command[3] = 0X0;

            i = Update(0x3C, command);
            if (i == 0) return "0";

            command[0] = 0X0;
            command[1] = 0X0;
            command[2] = 0X0;
            command[3] = 0X0;

            i = Update(0x13, command);
            if (i == 0) return "0";

            TarihAl kapTarih = new TarihAl(kapamaTarih);
            command = kapTarih.TarihDondur4Byte(kapamaTarih);

            i = Update(0x29, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(gerTuk, command);
            i = Update(0X2F, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(kalan, command);
            i = Update(0X31, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(harcanan, command);
            i = Update(0X30, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(devNo, command);
            i = Update(0X32, command);
            if (i == 0) return "0";

            command[0] = kartNo;
            command[1] = islemNo;

            i = Update(0X33, command);
            if (i == 0) return "0";

            command[0] = kademe;
            command[1] = loadLimit;
            command[2] = aksam;
            command[3] = sabah;

            i = Update(0X18, command);
            if (i == 0) return "0";


            command[0] = haftasonuAksam;
            command[1] = 0X0;
            command[2] = 0X0;
            command[3] = 0X0;

            i = Update(0X1A, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(fixCharge, command);
            i = Update(0X1B, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(totalFixCharge, command);
            i = Update(0X1C, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(k1Tuk, command);
            i = Update(0X37, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(k2Tuk, command);
            i = Update(0X3D, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(k3Tuk, command);
            i = Update(0X3E, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(Lim1, command);
            i = Update(0X12, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(Lim2, command);
            i = Update(0X14, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(kad1, command);
            i = Update(0X15, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(kad2, command);
            i = Update(0X16, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(kad3, command);
            i = Update(0X17, command);
            if (i == 0) return "0";


            FinishCard();

            return "1";
        }

        public string YetkiAvans(UInt32 devNo, UInt32 _Avans_Limiti)
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                FinishCard();
                return "0";
            }

            command[0] = 0x3A;
            command[1] = 0xDF;
            command[2] = 0x1D;
            command[3] = 0x80;

            i = Verify(7, command);
            if (i == 0) return "0";

            command[0] = issue_area[0];
            command[1] = issue_area[1];
            command[2] = (byte)'Y';
            command[3] = (byte)'V';

            i = Update(1, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(devNo, command);
            i = Update(0X3C, command);
            if (i == 0) return "0";


            Hexcon.UInt32toByte4(_Avans_Limiti, command);
            i = Update(0X10, command);
            if (i == 0) return "0";

            FinishCard();

            return "1";
        }

        public string YetkiBosalt()
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                FinishCard();
                return "0";
            }

            command[0] = 0x3A;
            command[1] = 0xDF;
            command[2] = 0x1D;
            command[3] = 0x80;

            i = Verify(7, command);
            if (i == 0) return "0";


            command[0] = 0;
            command[1] = 0;
            command[2] = 0;
            command[3] = 0;

            for (int j = 0x10; j < 0x20; j++)
            {
                i = Update((byte)j, command);
                if (i == 0) return "0";
            }

            for (int j = 0x28; j < 0x40; j++)
            {
                if (j == 0x38) j = 0x3C;
                i = Update((byte)j, command);
                if (i == 0) return "0";
            }

            i = Update(1, command);
            if (i == 0) return "0";



            command[0] = 0xAA;
            command[1] = 0xAA;
            command[2] = 0xAA;
            command[3] = 0xAA;

            i = Update(6, command);
            if (i == 0) return "0";



            FinishCard();

            return "1";
        }

        #endregion

        #region Üretim Kartı Fonksyonları
        public string FormUret()
        {
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != 0 ) || (inBuf[1] !=  0 ) || (inBuf[2] != 0 ))
            {
                FinishCard();
                return "0";
            }

            command[0] = 0xAA;
            command[1] = 0xAA;
            command[2] = 0xAA;
            command[3] = 0xAA;

            i = Verify(7, command);
            if (i == 0) return "0";

            command[0] = (byte)'K';
            command[1] = (byte)'I';
            command[2] = (byte)'U';
            command[3] = (byte)'A';

            i = Update(1, command);
            if (i == 0) return "0";

            command[0] = 0x7B;
            command[1] = 0x8A;
            command[2] = 0x13;
            command[3] = 0xEC;

            i = Update(6, command);
            if (i == 0) return "0";

            FinishCard();

            return "1";
        }

        public string UretimBosalt()
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[2] != 'U'))
            {
                FinishCard();
                return "0";
            }

            command[0] = 0x7B;
            command[1] = 0x8A;
            command[2] = 0x13;
            command[3] = 0xEC;

            i = Verify(7, command);
            if (i == 0) return "0";


            i = ReadCard(4);
            if (i == 0) { return "0"; }
            command[0] = 0;
            command[1] = 0;
            command[2] = 0;
            command[3] = inBuf[3];
            i = Update(0x04, command);
            if (i == 0) { return "0"; }

            command[0] = 0;
            command[1] = 0;
            command[2] = 0;
            command[3] = 0;

            for (int j = 0x02; j <= 0x03; j++)      //for (int j = 0x01; j <= 0x03; j++)        // Issuer bosaltmayalım sorun oluyor.
            {
                i = Update((byte)j , command);
                if (i == 0) { return "0"; }
            }

            i = Update(0x05, command);
            if (i == 0) { return "0"; }


            for (int j = 0x10; j < 0x20; j++)
            {
                i = Update((byte)j, command);
                if (i == 0) return "0";
            }

            for (int j = 0x28; j < 0x40; j++)
            {
                if (j == 0x38) j = 0x3C;
                i = Update((byte)j, command);
                if (i == 0) return "0";
            }

            i = Update(1, command);
            if (i == 0) return "0";



            command[0] = 0xAA;
            command[1] = 0xAA;
            command[2] = 0xAA;
            command[3] = 0xAA;

            i = Update(6, command);
            if (i == 0) return "0";



            FinishCard();

            return "1";
        }

        public string Format(uint devNo, uint KritikKredi, uint Kat1, uint Kat2, uint Kat3, uint Lim1, uint Lim2, uint OverLim)
        {
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != 'K') || (inBuf[1] != 'I') || (inBuf[2] != 'U'))
            {
                FinishCard();
                return "0";
            }

            command[0] = 0x7B;
            command[1] = 0x8A;
            command[2] = 0x13;
            command[3] = 0xEC;

            i = Verify(7, command);
            if (i == 0) return "0";

            command[0] = (byte)'K';
            command[1] = (byte)'I';
            command[2] = (byte)'U';
            command[3] = (byte)'F';

            i = Update(1, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(devNo, command);
            i = Update(0X10, command);
            if (i == 0) return "0";

            DateTime tar = DateTime.Now;

            TarihAl tarih = new TarihAl(tar);
            command = tarih.TarihDondur4Byte(tar);

            i = Update(0x12, command);
            if (i == 0) return "0";

            DayOfWeek dow = tar.DayOfWeek;

            command[0] = (byte)dow;
            command[1] = 0;
            command[2] = (byte)dow;
            command[3] = 0;

            i = Update(0x11, command);
            if (i == 0) return "0";

            Integer4Byte krit = new Integer4Byte(KritikKredi);

            command[0] = krit.bir;
            command[1] = krit.iki;
            command[2] = 0;
            command[3] = 0;

            i = Update(0x13, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(Kat1, command);
            i = Update(0X16, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(Kat2, command);
            i = Update(0X17, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(Kat3, command);
            i = Update(0X18, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(Lim1, command);
            i = Update(0X19, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(Lim2, command);
            i = Update(0X1A, command);
            if (i == 0) return "0";

            command[0] = 0X0;
            command[1] = 0X0;
            command[2] = 0X0;
            command[3] = 0X0;

            command[0] = Convert.ToByte(OverLim);

            i = Update(0X1B, command);
            if (i == 0) return "0";

            FinishCard();

            return "1   ";
        }

        public string ReelMod()
        {
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != 'K') || (inBuf[1] != 'I') || (inBuf[2] != 'U'))
            {
                FinishCard();
                return "0";
            }

            command[0] = 0x7B;
            command[1] = 0x8A;
            command[2] = 0x13;
            command[3] = 0xEC;

            i = Verify(7, command);
            if (i == 0) return "0";

            command[0] = (byte)'K';
            command[1] = (byte)'I';
            command[2] = (byte)'U';
            command[3] = (byte)'R';

            i = Update(1, command);
            if (i == 0) return "0";

            FinishCard();

            return "1";
        }

        public string TestMod(uint devNo)
        {
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != 'K') || (inBuf[1] != 'I') || (inBuf[2] != 'U'))
            {
                FinishCard();
                return "0";
            }

            command[0] = 0x7B;
            command[1] = 0x8A;
            command[2] = 0x13;
            command[3] = 0xEC;

            i = Verify(7, command);
            if (i == 0) return "0";

            command[0] = (byte)'K';
            command[1] = (byte)'I';
            command[2] = (byte)'U';
            command[3] = (byte)'T';

            i = Update(1, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(devNo, command);
            i = Update(0X3C, command);
            if (i == 0) return "0";

            command[0] = 0xFF;
            command[1] = 0xFF;
            command[2] = 0xFF;
            command[3] = 0xFF;

            i = Update(0X3D, command);
            if (i == 0) return "0";

            FinishCard();

            return "1";
        }

        public string CihazNo(uint devNo, uint KritikKredi, ushort CokUcuz, ushort Ucuz, ushort Normal, ushort Pahali)
        {
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != 'K') || (inBuf[1] != 'I') || (inBuf[2] != 'U'))
            {
                FinishCard();
                return "0";
            }

            command[0] = 0x7B;
            command[1] = 0x8A;
            command[2] = 0x13;
            command[3] = 0xEC;

            i = Verify(7, command);
            if (i == 0) return "0";

            command[0] = (byte)'K';
            command[1] = (byte)'I';
            command[2] = (byte)'U';
            command[3] = (byte)'C';

            i = Update(1, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(devNo, command);
            i = Update(0X10, command);
            if (i == 0) return "0";

            int w;

            DateTime dt = DateTime.Now;

            CultureInfo myCI = new CultureInfo("tr-TR");

            Calendar myCal = myCI.Calendar;

            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;

            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            w = myCal.GetWeekOfYear(dt, myCWR, myFirstDOW);

            DateTime tar = DateTime.Now.AddHours(2);

            if ((w >= 14) && ((w <= 44))) tar = DateTime.Now.AddHours(1);

            TarihAl tarih = new TarihAl(tar);
            command = tarih.TarihDondur4Byte(tar);

            i = Update(0x12, command);
            if (i == 0) return "0";

            DayOfWeek dow = tar.DayOfWeek;

            command[0] = (byte)dow;
            command[1] = 0;
            command[2] = (byte)dow;
            command[3] = 0;

            i = Update(0x11, command);
            if (i == 0) return "0";

            Integer4Byte krit = new Integer4Byte(KritikKredi);

            command[0] = krit.bir;
            command[1] = krit.iki;
            command[2] = 0;
            command[3] = 0;

            i = Update(0x13, command);
            if (i == 0) return "0";

            Integer2Byte cUcuz = new Integer2Byte(CokUcuz);
            Integer2Byte ucuzz = new Integer2Byte(Ucuz);
            Integer2Byte normall = new Integer2Byte(Normal);
            Integer2Byte pahalii = new Integer2Byte(Pahali);

            command[0] = cUcuz.bir;
            command[1] = cUcuz.iki;
            command[2] = ucuzz.bir;
            command[3] = ucuzz.iki;

            i = Update(0x14, command);
            if (i == 0) return "0";

            command[0] = normall.bir;
            command[1] = normall.iki;
            command[2] = pahalii.bir;
            command[3] = pahalii.iki;

            i = Update(0x15, command);
            if (i == 0) return "0";


            FinishCard();

            return "1";

        }

        public string UretimAc(int Acma)
        {
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != 'K') || (inBuf[1] != 'I') || (inBuf[2] != 'U'))
            {
                FinishCard();
                return "0";
            }

            command[0] = 0x7B;
            command[1] = 0x8A;
            command[2] = 0x13;
            command[3] = 0xEC;

            i = Verify(7, command);
            if (i == 0) return "0";

            command[0] = (byte)'K';
            command[1] = (byte)'I';
            command[2] = (byte)'U';
            command[3] = (byte)'A';

            i = Update(1, command);
            if (i == 0) return "0";

            if (Acma == 2)
                command[0] = 0X66;
            else
                command[0] = 0X00;

            command[1] = 0X00;
            command[2] = 0X00;
            command[3] = 0X00;

            i = Update(0X3C, command);
            if (i == 0) return "0";

            FinishCard();

            return "1";
        }

        public string FormIssuer(uint devNo)
        {
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            command[0] = 0xAA;
            command[1] = 0xAA;
            command[2] = 0xAA;
            command[3] = 0xAA;

            i = Verify(7, command);
            if (i == 0) return "0";

            command[0] = (byte)'K';
            command[1] = (byte)'K';
            command[2] = (byte)'X';

            i = Update(1, command);
            if (i == 0) return "0";

            command[0] = 0x3A;
            command[1] = 0xDF;
            command[2] = 0x1D;
            command[3] = 0x80;

            i = Update(6, command);
            if (i == 0) return "0";

            Hexcon.UInt32toByte4(devNo, command);
            i = Update(0X10, command);
            if (i == 0) return "0";

            i = Update(2, command);
            if (i == 0) return "0";

            FinishCard();

            return "1";
        }

        public string Issuer()
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitCard();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != 'K') || (inBuf[1] != 'K') || (inBuf[2] != 'X'))
            {
                FinishCard();
                return "0";
            }

            command[0] = 0x3A;
            command[1] = 0xDF;
            command[2] = 0x1D;
            command[3] = 0x80;

            i = Verify(7, command);
            if (i == 0) return "0";

            command[0] = issue_area[0];
            command[1] = issue_area[1];


            i = Update(0X3C, command);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            command[0] = 0xFF;
            command[1] = 0xFF;
            command[2] = 0xFF;
            command[3] = 0xFF;

            i = Update(0X3d, command);
            if (i == 0) return "0";

            command[0] = 0x0;
            command[1] = 0x0;
            command[2] = 0x0;
            command[3] = 0x0;

            FinishCard();

            return "1";

        }
        #endregion

        #endregion
        
    }
}


