using System;
using System.Collections.Generic;
using System.Text;
using SCLibWin;
using System.Collections;

namespace SmartCard
{
    public class OmniKart : IKart
    {

        Util util = new Util();
        public Int32 zone;

        #region Bufferlar
        byte[] inBuf = new byte[128];
    //    byte[] outBuf = new byte[128];
        byte[] buffer = new byte[100];
        #endregion

        SCResMgr mng;
        SCReader rd;
        string rdName;
        
        public OmniKart(int Zone)
        {
            zone = Zone;
        }

        private void Sleep(int time)
        {
            System.Threading.Thread.Sleep(time);
        }


        #region Omni Kart Reader fonksyonları

        public int InitKart()
        {
            try
            {
                mng = new SCResMgr();
                mng.EstablishContext(SCardContextScope.System);
                rd = new SCReader(mng);
                ArrayList al = new ArrayList();
                mng.ListReaders(al);
                rdName = al[0].ToString();
                rd.Connect(rdName, SCardAccessMode.Shared, SCardProtocolIdentifiers.T0);
                rd.BeginTransaction();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public void FinishKart()
        {
            rd.EndTransaction(SCardDisposition.LeaveCard);
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
        public string KrediYaz(uint devNo, uint AnaKredi, ITarife trf)
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

            i = InitKart();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";
            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'A') || (inBuf[3] != ' '))
            {
                FinishKart();
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
            sr = SendAboneCsc(111, 0X1515);
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
                AnaKredi = AnaKredi + 20000;
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

            Hexcon.UInt32toByte4(20000, command);
            i = Update(0X34, command);
            if (i == 0) return "0";


            FinishKart();

            return "1";
        }

        public string AboneOku()
        {
            string str = "";
            byte[] issue_area = GetIssuer(zone);
            int i = 0;

            i = InitKart();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";
            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'A') || (inBuf[3] != ' '))
            {
                FinishKart();
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



            str += "_" + devNo;
            str += "_" + elkAna;
            str += "_" + ako;
            str += "_" + yko;
            str += "_" + yeniKart;
            str += "_" + elkKalan;
            str += "_" + elkHarcanan;
            str += "_" + elkTarih;
            str += "_" + KlemensCeza;
            str += "_" + Ariza;
            str += "_" + PilZayif;
            str += "_" + PilBitik;
            str += "_" + k1Tuk;
            str += "_" + k2Tuk;
            str += "_" + k3Tuk;
            str += "_" + gerTuk;
            str += "_" + elkEkim;
            str += "_" + elkAralik;
            str += "_" + kad1;
            str += "_" + kad2;
            str += "_" + kad3;
            str += "_" + lim1;
            str += "_" + lim2;
            str += "_" + loadLim;
            str += "_" + aksam;
            str += "_" + sabah;
            str += "_" + kademe;
            str += "_" + hSonuAksam;
            str += "_" + fixCharge;
            str += "_" + totalFixCharge;
            //str += "_" + otherCharge;
            //str += "_" + otherTotalCharge;
            //str += "_" + dailyDebt;
            //str += "_" + totalDebt;
            //str += "_" + remainingDebt;
            //str += "_" + Reverse;
            //str += "_" + buzzerInterval;
            //str += "_" + buzzerDuration;
            //str += "_" + lowVoltage;
            //str += "_" + highVoltage;

            FinishKart();

            return "1" + str;
        }

        public string KrediOku()
        {
            string str = "";
            byte[] issue_area = GetIssuer(zone);
            int i = 0;

            i = InitKart();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";
            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'A') || (inBuf[3] != ' '))
            {
                FinishKart();
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

            FinishKart();

            str += "_" + devNo;
            str += "_" + elkAna;
            str += "_" + ako;
            str += "_" + yko;
            str += "_" + yeniKart;
            str += "_" + fixCharge;
            str += "_" + totalFixCharge;

            return "1" + str;

        }

        public string AboneYap(uint devNo, ITarife trf)
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

            i = InitKart();
            if (i == 0) return "0";

            byte[] command = new byte[4];


            command[0] = 0XAA;
            command[1] = 0XAA;
            command[2] = 0XAA;
            command[3] = 0XAA;
            i = Verify(0X07, command);
            if (i == 0) return "0";

            command[0] = 0X11;
            command[1] = 0X11;
            command[2] = 0X11;
            command[3] = 0X11;
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
            sr = SendAboneCsc(111, 0X1515);
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

            command[0] = 0;
            command[1] = 0;
            command[2] = 0;
            command[3] = 0;
            //i = Update(0X2D, command);
            //if (i == 0) return "0";

            for (int t = 0X19; t < 0X20; t++)
            {
                i = Update((byte)t, command);
                if (i == 0) return "0";
            }

            for (int j = 0X2B; j < 0X38; j++)
            {
                if (j == 0x30) j = 0x31;
                else if (j == 0x2c) j = 0x2d;
                else if (j == 0x2f) j = 0x31;

                i = Update((byte)j, command);
                if (i == 0) return "0";

            }

            FinishKart();
            return "1"; 
        }

        public string Bosalt()
        {
            string str = "";
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitKart();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";
            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'A') || (inBuf[3] != ' '))
            {
                FinishKart();
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
            sr = SendAboneCsc(111, 0X1515);
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

            command[0] = 0xAA;
            command[1] = 0xAA;
            command[2] = 0xAA;
            command[3] = 0xAA;
            i = Update(6, command);
            if (i == 0) return "0";

            command[0] = 0x11;
            command[1] = 0x11;
            command[2] = 0x11;
            command[3] = 0x11;
            i = Update(0X38, command);
            if (i == 0) return "0";

            command[0] = 0x22;
            command[1] = 0x22;
            command[2] = 0x22;
            command[3] = 0x22;
            i = Update(0X3A, command);
            if (i == 0) return "0";

            FinishKart();
            return "1"; 
        }

        public string Eject()
        {
            return "1";
        }
        #endregion 

        #region Yetki Kartı Fonksyonları
        public string FormYet()
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitKart();
            if (i == 0) return "0";

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

            FinishKart();

            return "1";

        }

        public string YetkiSaat()
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitKart();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                FinishKart();
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


            FinishKart();

            return "1";
        }

        public string YetkiSaat(DateTime date)
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitKart();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                FinishKart();
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


            FinishKart();

            return "1";
        }

        public string YetkiAc(uint devNo)
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitKart();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                FinishKart();
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


            FinishKart();

            return "1";
        }

        public string YetkiKapat(uint devNo)
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitKart();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                FinishKart();
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

            FinishKart();

            return "1";
        }

        public string YetkiBilgiYap()
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitKart();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                FinishKart();
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

            FinishKart();

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

            i = InitKart();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y') || (inBuf[3] != 'E'))
            {
                FinishKart();
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
                FinishKart();
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

            str += devNo + "_" + elkKalan + "_" + elkHarcanan + "_" + k1Tuk + "_" + k2Tuk + "_" + k3Tuk + "_" + gerTuk + "_" + bilgi.gun + "/" + bilgi.ay + "/" + bilgi.yil + " " +
                    bilgi.saat + ":" + bilgi.dakika + "_" + bilgionkap.gun + "/" + bilgionkap.ay + "/" + bilgionkap.yil + "_" + bilgisonp.gun + "/" + bilgisonp.ay + "/" + bilgisonp.yil + "_" +
                    bilgiarzt.gun + "/" + bilgiarzt.ay + "/" + bilgiarzt.yil + "_" + bilgikr.gun + "/" + bilgikr.ay + "/" + bilgikr.yil + "/" + islem + "_" + hangigun + "_" +
                    kad1 + "_" + kad2 + "_" + kad3 + "_" + lim1 + "_" + lim2 + "_" + "0" + "_" + loadLim + "_" + aksam + "_" + sabah + "_" + kademe + "_" +
                    KlemensCeza + "_" + Ariza + "_" + PilZayif + "_" + PilBitik + "_" + elkEkim + "_" + elkAralik + "_" +
                    hSonuAksam + "_" + fixCharge + "_" + totalFixCharge + "_" + kartNo + "_" + donemTuk;


            FinishKart();

            return "1_" + str;
        }

        public string YetkiIadeYap(uint devNo)
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitKart();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                FinishKart();
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

            FinishKart();

            return "1";


        }

        public string YetkiIadeOku()
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitKart();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y') || (inBuf[3] != 'I'))
            {
                FinishKart();
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


            FinishKart();

            return "1_" + takilmismi + "_" + devNo + "_" + kredi + "_" + harcanan;

        }

        public string YetkiIptal(uint devNo)
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitKart();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                FinishKart();
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

            FinishKart();

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

            i = InitKart();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                FinishKart();
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


            FinishKart();

            return "1";
        }
        #endregion

        #region Üretim Kartı Fonksyonları
        public string FormUret()
        {
            int i = 0;
            byte[] command = new byte[4];

            i = InitKart();
            if (i == 0) return "0";

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

            FinishKart();

            return "1";



        }

        public string Format(uint devNo, uint KritikKredi, uint Kat1, uint Kat2, uint Kat3, uint Lim1, uint Lim2, uint OverLim)
        {
            int i = 0;
            byte[] command = new byte[4];

            i = InitKart();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != 'K') || (inBuf[1] != 'I') || (inBuf[2] != 'U'))
            {
                FinishKart();
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

            FinishKart();

            return "1   ";
        }

        public string ReelMod()
        {
            int i = 0;
            byte[] command = new byte[4];

            i = InitKart();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != 'K') || (inBuf[1] != 'I') || (inBuf[2] != 'U'))
            {
                FinishKart();
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

            FinishKart();

            return "1";
        }

        public string TestMod(uint devNo)
        {
            int i = 0;
            byte[] command = new byte[4];

            i = InitKart();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != 'K') || (inBuf[1] != 'I') || (inBuf[2] != 'U'))
            {
                FinishKart();
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

            FinishKart();

            return "1";
        }

        public string CihazNo(uint devNo, uint KritikKredi, ushort CokUcuz, ushort Ucuz, ushort Normal, ushort Pahali)
        {
            int i = 0;
            byte[] command = new byte[4];

            i = InitKart();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != 'K') || (inBuf[1] != 'I') || (inBuf[2] != 'U'))
            {
                FinishKart();
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


            FinishKart();

            return "1";

        }

        public string UretimAc(int Acma)
        {
            int i = 0;
            byte[] command = new byte[4];

            i = InitKart();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != 'K') || (inBuf[1] != 'I') || (inBuf[2] != 'U'))
            {
                FinishKart();
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

            FinishKart();

            return "1";
        }

        public string FormIssuer(uint devNo)
        {
            int i = 0;
            byte[] command = new byte[4];

            i = InitKart();
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

            FinishKart();

            return "1";
        }

        public string Issuer()
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
            byte[] command = new byte[4];

            i = InitKart();
            if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 0) return "0";

            if ((inBuf[0] != 'K') || (inBuf[1] != 'K') || (inBuf[2] != 'X'))
            {
                FinishKart();
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

            FinishKart();

            return "1";

        }
        #endregion

        #endregion
        
    }
}


