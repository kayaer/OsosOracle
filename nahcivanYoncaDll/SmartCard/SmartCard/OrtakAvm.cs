using SCLibWin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SmartCard
{
    public class OrtakAvm
    {
        private string zone = "BK";
        private string _hata = "";

        #region Bufferlar
        byte[] inBuf = new byte[4];
        byte[] buffer = new byte[100];
        byte[] outBuf = new byte[10];
        byte[] Yetki = new byte[152];

        byte[] csc0 = new byte[4];
        byte[] csc1 = new byte[4];
        byte[] csc2 = new byte[4];

        #endregion

        byte EXOR, newEXOR;

        SCResMgr mng;
        SCReader rd;
        string rdName;

        public OrtakAvm()
        {
        }

        private string HataMesaji
        {
            get
            {
                return _hata;
            }
            set
            {
                _hata = value;
            }
        }

        #region ACR128 Reader fonksyonları

        private ArrayList GetKartReaders()
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

        private int InitCard()
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

        private void FinishCard()
        {
            if (rd.IsConnected) rd.EndTransaction(SCardDisposition.ResetCard);
        }

        private int ReadCard(byte adres)
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

        private int VerifyCard(byte adres)
        {
            byte[] command = new byte[9];
            byte[] response = new byte[2];

            command[0] = 0X00;
            command[1] = 0X20;
            command[2] = 0X00;
            command[3] = adres;  //verify  adres  = 7
            command[4] = 0X04;
            Array.Copy(outBuf, 0, command, 5, 4);

            rd.Transmit(command, out response);
            if (response[0] == 0X90 && response[1] == 0X00) return 1;
            else return 0;
        }

        private int UpdateCard(byte adres)
        {
            byte[] command = new byte[9];
            byte[] response = new byte[2];

            command[0] = 0X80;
            command[1] = 0XDE;
            command[2] = 0X00;
            command[3] = adres;  //update adres
            command[4] = 0X04;
            Array.Copy(outBuf, 0, command, 5, 4);

            rd.Transmit(command, out response);
            if (response[0] == 0X90 && response[1] == 0X00) return 1;
            else return 0;
        }

        #endregion

        #region IKart Fonksyonları

        /*struct TARIH_SAAT
        {
            public UInt32 ay;
            public UInt32 gun;
            public UInt32 yil;
            public UInt32 saat;
            public UInt32 dakika;

            public TARIH_SAAT(byte[] data)
            {
                UInt32 u32 = Hexcon.Byte4toUInt32(data);

                ay = u32 & 0x0000000F;
                u32 = u32 >> 4;
                gun = u32 & 0x0000001F;
                u32 = u32 >> 5;
                yil = u32 & 0x0000007F;
                u32 = u32 >> 7;
                saat = u32 & 0x000000FF;
                u32 = u32 >> 8;
                dakika = u32 & 0x000000FF;
            }
        }
        struct TARIH
        {
            public UInt32 ay;
            public UInt32 gun;
            public UInt32 yil;

            public TARIH(byte[] data)
            {
                UInt32 u32 = Hexcon.Byte4toUInt32(data);

                ay = u32 & 0x0000000F;
                u32 = u32 >> 4;
                gun = u32 & 0x0000001F;
                u32 = u32 >> 5;
                yil = u32 & 0x0000007F;
            }
        }
        struct STATE
        {
            public UInt32 ceza;
            public UInt32 ariza;
            public UInt32 iptal;
            public UInt32 pzayif;
            public UInt32 pbitik;
            public UInt32 rtc_hata;
            public UInt32 yedek_pil_bitik;
            public UInt32 ceza1;

            public STATE(byte[] data)
            {
                UInt32 u32 = Hexcon.Byte4toUInt32(data);

                ceza = u32 & 0x00000001;
                u32 = u32 >> 1;
                ariza = u32 & 0x00000001;
                u32 = u32 >> 1;
                iptal = u32 & 0x00000001;
                u32 = u32 >> 1;
                pzayif = u32 & 0x00000001;
                u32 = u32 >> 1;
                pbitik = u32 & 0x00000001;
                u32 = u32 >> 1;
                rtc_hata = u32 & 0x00000001;
                u32 = u32 >> 1;
                yedek_pil_bitik = u32 & 0x00000001;
                u32 = u32 >> 1;
                ceza1 = u32 & 0x00000001;
                u32 = u32 >> 1;
            }
        }*/

        private void HataSet(int Kod)
        {
            _hata = Kod + ":";
            switch (Kod)
            {
                case 0:
                    _hata += "Successful";
                    break;
                case 1:
                    _hata += "Card Connection Error";
                    break;
                case 2:
                    _hata += "Card Issuer Reading Error";
                    break;
                case 3:
                    _hata += "Csc01Verify Error";
                    break;
                case 4:
                    _hata += "Csc2Verify Error";
                    break;
                case 5:
                    _hata += "Card Reading Error";
                    break;
                case 6:
                    _hata += "NewExor Error";
                    break;
                case 7:
                    _hata += "Card write Error";
                    break;
                case 8:
                    _hata += "Not Subscriber Card";
                    break;
                case 9:
                    _hata += "Card Verify 1 Error";
                    break;
                case 10:
                    _hata += "Card Verify 2 Error";
                    break;
                case 11:
                    _hata += "Card Verify 3 Error";
                    break;
                case 12:
                    _hata += "Card Issuer write Error";
                    break;
                case 13:
                    _hata += "Not Authorization Card";
                    break;
                case 14:
                    _hata += "Not Info Card";
                    break;
                case 15:
                    _hata += "Info Card not inserted to the meter";
                    break;
                case 16:
                    _hata += "Not Payback Card";
                    break;
                case 17:
                    _hata += "Not Production Card";
                    break;
                default:
                    _hata = "0:";
                    _hata += "Successful";
                    break;
            }
        }

        #region Ortak Fonksiyonlar

        public string KartTipi()
        {
            HataSet(0);

            byte[] issue_area = GetIssuer();
            int i = 0;

            i = InitCard();
            if (i == 0) { FinishCard(); HataSet(1); return "0"; }

            i = ReadCard(1);
            if (i == 5) return "0";
            else if (i == 0) { FinishCard(); HataSet(2); return "0"; }

            FinishCard();


            return TipTanimlama(Convert.ToChar(inBuf[0]).ToString() + Convert.ToChar(inBuf[1]).ToString() + Convert.ToChar(inBuf[2]).ToString() + Convert.ToChar(inBuf[3]).ToString());
        }

        private string TipTanimlama(string IssuerArea)
        {
            byte[] issue_area = GetIssuer();

            string stIssuer = Convert.ToChar(issue_area[0]).ToString() + Convert.ToChar(issue_area[1]).ToString();

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
            if (stIssuer + "YB" == IssuerArea)
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
            return IssuerArea;

        }

        private byte[] GetIssuer()
        {
            byte[] issue_area = new byte[2];

            issue_area[0] = (byte)Convert.ToChar(zone.Substring(0, 1));
            issue_area[1] = (byte)Convert.ToChar(zone.Substring(1, 1));

            return issue_area;
        }

        private UInt32 SendAboneCsc(UInt32 dn, UInt32 alfa)
        {
            short carry;
            Int32 p, r, q;
            Int32 rmask, pmask, qmask;
            UInt32 t1, t2, t3, t4;
            UInt32 sr;
            int i;

            p = 2;
            r = 3;
            q = 4;
            pmask = 1 << (p - 1);
            rmask = 1 << (r - 1);
            qmask = 1 << (q - 1);

            sr = dn;
            sr = sr + alfa;
            if (sr == 0)
                sr = 1;
            for (i = 0; i < 16; i++)
            {
                t1 = (uint)((sr & pmask) != 0 ? 1 : 0);
                t2 = (uint)((sr & rmask) != 0 ? 1 : 0);
                t3 = (uint)((sr & qmask) != 0 ? 1 : 0);
                t4 = (uint)((sr & 0x8000) != 0 ? 1 : 0);
                carry = (short)(t1 ^ t2 ^ t3 ^ t4);
                sr <<= 1;
                if (carry > 0)
                {
                    sr |= 1;
                }
            }
            return (UInt32)sr;
        }

        private void EXOR_PROCESS(byte x)
        {
            for (int i = 0; i < 4; i++)
            {
                inBuf[i] ^= x;
                outBuf[i] ^= x;
            }
        }

        private void outBuf_Randomize()
        {
            UInt32 sId = (UInt32)((DateTime.Now.Ticks / 100000) % 0xffffffff);
            outBuf[0] = (byte)(sId % (256 * 256 * 256));
            sId = sId / 256;
            outBuf[1] = (byte)(sId % (256 * 256));
            sId = sId / 256;
            outBuf[2] = (byte)(sId % 256);
            outBuf[3] = (byte)(sId / 256);

        }

        private int Csc01Verify(UInt32 devNo)
        {
            int i = 0;
            UInt32 sr;
            byte[] buf = new byte[2];

            #region csc 0 1

            i = ReadCard(0X3F);
            if (i == 0) return 0;
            EXOR = inBuf[3];

            i = ReadCard(0X3D);
            if (i == 0) return 0;
            EXOR_PROCESS(EXOR);

            buf[0] = inBuf[0];
            buf[1] = inBuf[1];
            UInt16 alfa1 = Hexcon.Byte2toUInt16(buf[0], buf[1]);

            buf[0] = inBuf[2];
            buf[1] = inBuf[3];
            UInt32 alfa2 = Hexcon.Byte2toUInt16(buf[0], buf[1]);

            i = ReadCard(0X3E);
            if (i == 0) return 0;
            EXOR_PROCESS(EXOR);

            buf[0] = inBuf[0];
            buf[1] = inBuf[1];
            UInt32 alfa3 = Hexcon.Byte2toUInt16(buf[0], buf[1]);

            buf[0] = inBuf[2];
            buf[1] = inBuf[3];
            UInt32 alfa4 = Hexcon.Byte2toUInt16(buf[0], buf[1]);

            sr = SendAboneCsc(devNo, alfa1);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devNo, alfa2);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            i = VerifyCard(7);
            if (i == 0) return 0;

            sr = SendAboneCsc(devNo, alfa3);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devNo, alfa4);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            i = VerifyCard(0X39);
            if (i == 0) return 0;

            #endregion

            return 1;
        }

        private int Csc2Verify(UInt32 devNo)
        {
            int i = 0;
            UInt32 sr;
            byte[] buf = new byte[2];

            #region csc 2

            i = ReadCard(0X1C);
            if (i == 0) return 0;
            EXOR_PROCESS(Convert.ToByte(EXOR + 1));

            buf[0] = inBuf[0];
            buf[1] = inBuf[1];
            UInt16 alfa5 = Hexcon.Byte2toUInt16(buf[0], buf[1]);

            buf[0] = inBuf[2];
            buf[1] = inBuf[3];
            UInt16 alfa6 = Hexcon.Byte2toUInt16(buf[0], buf[1]);

            sr = SendAboneCsc(devNo, alfa5);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devNo, alfa6);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            i = VerifyCard(0X3B);
            if (i == 0) return 0;

            #endregion

            return 1;
        }

        private int YeniExor(UInt32 devNo)
        {
            int i = 0;
            UInt32 sr;
            byte[] buf = new byte[2];

            #region yeni exor csc, alfa, beta

            i = ReadCard(0X3F);
            if (i == 0) return 0;

            outBuf[0] = 0X58;
            outBuf[1] = 0X52;
            outBuf[2] = 0X1F;

            outBuf[3] = EXOR;
            newEXOR = EXOR;

            i = UpdateCard(0X3F);
            if (i == 0) return 0;

            outBuf_Randomize();

            buf[0] = outBuf[0];
            buf[1] = outBuf[1];
            UInt16 alfa1 = Hexcon.Byte2toUInt16(buf[0], buf[1]);

            buf[0] = outBuf[2];
            buf[1] = outBuf[3];
            UInt16 alfa2 = Hexcon.Byte2toUInt16(buf[0], buf[1]);

            EXOR_PROCESS(newEXOR);

            i = UpdateCard(0X3D);
            if (i == 0) return 0;

            outBuf_Randomize();

            buf[0] = outBuf[0];
            buf[1] = outBuf[1];
            UInt16 alfa3 = Hexcon.Byte2toUInt16(buf[0], buf[1]);

            buf[0] = outBuf[2];
            buf[1] = outBuf[3];
            UInt16 alfa4 = Hexcon.Byte2toUInt16(buf[0], buf[1]);

            EXOR_PROCESS(newEXOR);

            i = UpdateCard(0X3E);
            if (i == 0) return 0;

            outBuf_Randomize();

            buf[0] = outBuf[0];
            buf[1] = outBuf[1];
            UInt16 alfa5 = Hexcon.Byte2toUInt16(buf[0], buf[1]);

            buf[0] = outBuf[2];
            buf[1] = outBuf[3];
            UInt16 alfa6 = Hexcon.Byte2toUInt16(buf[0], buf[1]);

            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));

            i = UpdateCard(0X1C);
            if (i == 0) return 0;

            sr = SendAboneCsc(devNo, alfa1);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devNo, alfa2);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            csc0[0] = outBuf[0];
            csc0[1] = outBuf[1];
            csc0[2] = outBuf[2];
            csc0[3] = outBuf[3];

            i = UpdateCard(6);
            if (i == 0) return 0;

            sr = SendAboneCsc(devNo, alfa3);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devNo, alfa4);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            csc1[0] = outBuf[0];
            csc1[1] = outBuf[1];
            csc1[2] = outBuf[2];
            csc1[3] = outBuf[3];

            i = UpdateCard(0X38);
            if (i == 0) return 0;

            sr = SendAboneCsc(devNo, alfa5);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devNo, alfa6);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            csc2[0] = outBuf[0];
            csc2[1] = outBuf[1];
            csc2[2] = outBuf[2];
            csc2[3] = outBuf[3];

            i = UpdateCard(0X3A);
            if (i == 0) return 0;

            outBuf[0] = 0;
            outBuf[1] = 0;
            outBuf[2] = 0;
            outBuf[3] = 0;

            Random rnd = new Random();
            outBuf[0] = (byte)rnd.Next(0XFE);
            outBuf[1] = (byte)rnd.Next(0XFE);

            UInt16 beta1 = Hexcon.Byte2toUInt16(outBuf[0], outBuf[1]);

            EXOR_PROCESS(Convert.ToByte(newEXOR + 2));

            i = UpdateCard(0X35);
            if (i == 0) return 0;

            buf[0] = 0;
            buf[1] = 0;

            buf[0] = csc0[0];
            buf[1] = csc0[2];
            UInt16 fsc1 = Hexcon.Byte2toUInt16(buf[0], buf[1]);

            buf[0] = csc1[1];
            buf[1] = csc1[3];
            UInt16 fsc2 = Hexcon.Byte2toUInt16(buf[0], buf[1]);

            buf[0] = csc2[0];
            buf[1] = csc2[2];
            UInt16 fsc3 = Hexcon.Byte2toUInt16(buf[0], buf[1]);

            sr = SendAboneCsc(devNo, fsc1);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devNo, fsc2);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            EXOR_PROCESS(Convert.ToByte(newEXOR + 2));

            i = UpdateCard(0X33);
            if (i == 0) return 0;

            sr = SendAboneCsc(devNo, fsc3);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devNo, beta1);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            EXOR_PROCESS(Convert.ToByte(newEXOR + 2));

            i = UpdateCard(0X34);
            if (i == 0) return 0;

            #endregion

            return 1;
        }

        #region şifre

        private int IssuerKontrol()
        {
            int i = 0;

            i = InitCard();
            if (i == 0) { FinishCard(); return 0; }

            i = ReadCard(1);
            if (i == 0) { FinishCard(); return 0; }
            if ((inBuf[0] != 0) || (inBuf[1] != 0) || (inBuf[2] != 0))
            {
                FinishCard();
                return 0;
            }


            FinishCard();
            return 1;

        }

        private int BoskartKontrol()
        {
            int i = 0;

            i = InitCard();
            if (i == 0) { FinishCard(); return 0; }

            i = ReadCard(1);
            if (i == 0) { FinishCard(); return 0; }
            if ((inBuf[0] != 0) || (inBuf[1] != 0) || (inBuf[2] != 0))
            {
                FinishCard();
                return 0;
            }

            outBuf[0] = 0XAA;
            outBuf[1] = 0XAA;
            outBuf[2] = 0XAA;
            outBuf[3] = 0XAA;

            i = VerifyCard(0X07);
            if (i == 0) { FinishCard(); return 0; }

            outBuf[0] = 0X11;
            outBuf[1] = 0X11;
            outBuf[2] = 0X11;
            outBuf[3] = 0X11;

            i = VerifyCard(0X39);
            if (i == 0) { FinishCard(); return 0; }

            outBuf[0] = 0X22;
            outBuf[1] = 0X22;
            outBuf[2] = 0X22;
            outBuf[3] = 0X22;

            i = VerifyCard(0X3B);
            if (i == 0) { FinishCard(); return 0; }

            FinishCard();

            return 1;

        }

        private int SifreExeKontrol()
        {
            int i = 0;

            i = InitCard();
            if (i == 0) { FinishCard(); return 0; }

            i = ReadCard(1);
            if (i == 0) { FinishCard(); return 0; }
            if ((inBuf[0] != 0) || (inBuf[1] != 0) || (inBuf[2] != 0))
            {
                FinishCard();
                return 0;
            }

            outBuf[0] = 0X79;
            outBuf[1] = 0X81;
            outBuf[2] = 0X17;
            outBuf[3] = 0X2F;

            i = VerifyCard(0X07);
            if (i == 0) { FinishCard(); return 0; }

            outBuf[0] = 0X2E;
            outBuf[1] = 0X4C;
            outBuf[2] = 0X16;
            outBuf[3] = 0X41;

            i = VerifyCard(0X39);
            if (i == 0) { FinishCard(); return 0; }

            outBuf[0] = 0X22;
            outBuf[1] = 0X22;
            outBuf[2] = 0X22;
            outBuf[3] = 0X22;

            i = VerifyCard(0X3B);
            if (i == 0) { FinishCard(); return 0; }

            FinishCard();

            return 1;
        }

        private int SifreExeHazirla()
        {
            int i = 0;

            i = InitCard();
            if (i == 0) { FinishCard(); return 0; }

            outBuf[0] = 0XAA;
            outBuf[1] = 0XAA;
            outBuf[2] = 0XAA;
            outBuf[3] = 0XAA;

            i = VerifyCard(0X07);

            if (i == 1)
            {
                outBuf[0] = 0X11;
                outBuf[1] = 0X11;
                outBuf[2] = 0X11;
                outBuf[3] = 0X11;

                i = VerifyCard(0X39);

                if (i == 1)
                {
                    outBuf[0] = 0X79;
                    outBuf[1] = 0X81;
                    outBuf[2] = 0X17;
                    outBuf[3] = 0X2F;

                    i = UpdateCard(6);

                    if (i == 1)
                    {
                        outBuf[0] = 0X2E;
                        outBuf[1] = 0X4C;
                        outBuf[2] = 0X16;
                        outBuf[3] = 0X41;

                        i = UpdateCard(0X38);

                        FinishCard();
                        return i;
                    }
                }
            }
            FinishCard();
            return 0;

        }

        private int KartSifreBosalt()
        {
            int i = 0;

            i = InitCard();
            if (i == 0) { FinishCard(); return 0; }

            i = ReadCard(1);
            if (i == 0) { FinishCard(); return 0; }
            if ((inBuf[0] != 0) || (inBuf[1] != 0) || (inBuf[2] != 0))
            {
                FinishCard();
                return 0;
            }

            outBuf[0] = 0X79;
            outBuf[1] = 0X81;
            outBuf[2] = 0X17;
            outBuf[3] = 0X2F;

            i = VerifyCard(7);
            if (i == 0) { FinishCard(); return 0; }

            outBuf[0] = 0X2E;
            outBuf[1] = 0X4C;
            outBuf[2] = 0X16;
            outBuf[3] = 0X41;

            i = VerifyCard(0X39);
            if (i == 0) { FinishCard(); return 0; }

            outBuf[0] = 0XAA;
            outBuf[1] = 0XAA;
            outBuf[2] = 0XAA;
            outBuf[3] = 0XAA;

            i = UpdateCard(6);
            if (i == 0) { FinishCard(); return 0; }

            outBuf[0] = 0X11;
            outBuf[1] = 0X11;
            outBuf[2] = 0X11;
            outBuf[3] = 0X11;

            i = UpdateCard(0X38);
            if (i == 0) { FinishCard(); return 0; }

            FinishCard();

            return 1;
        }

        #endregion

        #endregion

        struct TARIH
        {
            public UInt32 ay2;
            public UInt32 gun2;
            public UInt32 saat2;

            public TARIH(byte[] data)
            {
                UInt32 u32 = Hexcon.Byte4toUInt32(data);

                ay2 = u32 & 0x0000000F;
                u32 = u32 >> 4;
                gun2 = u32 & 0x0000001F;
                u32 = u32 >> 5;
                saat2 = u32 & 0x0000007F;
            }
        }

        struct STATE7
        {
            public UInt32 onkapak;
            public UInt32 ariza;
            public UInt32 klemens;
            public UInt32 pzayif;
            public UInt32 pbitik;
            public UInt32 bos1;
            public UInt32 bos2;
            public UInt32 bos3;

            public STATE7(byte[] data)
            {
                UInt32 u32 = Hexcon.Byte4toUInt32(data);

                onkapak = u32 & 0x00000001;
                u32 = u32 >> 1;
                ariza = u32 & 0x00000001;
                u32 = u32 >> 1;
                klemens = u32 & 0x00000001;
                u32 = u32 >> 1;
                pzayif = u32 & 0x00000001;
                u32 = u32 >> 1;
                pbitik = u32 & 0x00000001;
                u32 = u32 >> 1;
                bos1 = u32 & 0x00000001;
                u32 = u32 >> 1;
                bos2 = u32 & 0x00000001;
                u32 = u32 >> 1;
                bos3 = u32 & 0x00000001;
                u32 = u32 >> 1;
            }
        }

        struct STATE5
        {
            public UInt32 ceza2;
            public UInt32 ariza;
            public UInt32 iptal;
            public UInt32 pzayif;
            public UInt32 pbitik;
            public UInt32 pkapak;
            public UInt32 bos1;
            public UInt32 ceza1;

            public STATE5(byte[] data)
            {
                UInt32 u32 = Hexcon.Byte4toUInt32(data);

                ceza2 = u32 & 0x00000001;
                u32 = u32 >> 1;
                ariza = u32 & 0x00000001;
                u32 = u32 >> 1;
                iptal = u32 & 0x00000001;
                u32 = u32 >> 1;
                pzayif = u32 & 0x00000001;
                u32 = u32 >> 1;
                pbitik = u32 & 0x00000001;
                u32 = u32 >> 1;
                pkapak = u32 & 0x00000001;
                u32 = u32 >> 1;
                bos1 = u32 & 0x00000001;
                u32 = u32 >> 1;
                ceza1 = u32 & 0x00000001;
                u32 = u32 >> 1;
            }
        }

        struct STATE6
        {
            public UInt32 ceza1;
            public UInt32 ceza2;
            public UInt32 ariza;
            public UInt32 iptal;
            public UInt32 paz;
            public UInt32 pbitik;
            public UInt32 ceza3;
            public UInt32 pulse_hata;

            public STATE6(byte[] data)
            {
                UInt32 u32 = Hexcon.Byte4toUInt32(data);

                ceza1 = u32 & 0x00000001;
                u32 = u32 >> 1;
                ceza2 = u32 & 0x00000001;
                u32 = u32 >> 1;
                ariza = u32 & 0x00000001;
                u32 = u32 >> 1;
                iptal = u32 & 0x00000001;
                u32 = u32 >> 1;
                paz = u32 & 0x00000001;
                u32 = u32 >> 1;
                pbitik = u32 & 0x00000001;
                u32 = u32 >> 1;
                ceza3 = u32 & 0x00000001;
                u32 = u32 >> 1;
                pulse_hata = u32 & 0x00000001;
                u32 = u32 >> 1;
            }
        }

        struct LEARN1
        {
            public UInt32 islem;
            public LEARN1(byte[] data)
            {
                UInt32 u32 = Hexcon.Byte4toUInt32(data);

                islem = (u32 & 0x000000FF);
                u32 = u32 >> 8;
            }
        }

        struct LEARN2
        {
            public UInt32 kart_numarasi;
            public UInt32 major;
            public UInt32 yeni_kart;
            public UInt32 hata;

            public LEARN2(byte[] data)
            {
                UInt32 u32 = Hexcon.Byte4toUInt32(data);

                kart_numarasi = u32 & 0x0F;
                u32 = u32 >> 4;
                major = u32 & 0x03;
                u32 = u32 >> 2;
                yeni_kart = u32 & 0x01;
                u32 = u32 >> 1;
                hata = u32 & 0x01;
                u32 = u32 >> 1;
            }
        }

        public string AboneYap(UInt32 cihazNo, Int32 kartNo, Int32 s1lim1, Int32 s1lim2, Int32 sfiyat1, Int32 sfiyat2, Int32 sfiyat3, Int32 efiyat1, Int32 efiyat2, Int32 efiyat3, Int32 efiyat4)
        {
            int g;
            byte i;
            uint sr;
            Int32 maxfiyat;
            UInt32 kad1, kad2, kad3, lim1, lim2, s1kat1, s1kat2, T1kat, T2kat, T3kat, T4kat;
            int ekarno, gkarno, s1karno, s2karno;

            byte[] issue_area = GetIssuer();

            g = InitCard();
            if (g == 5) return "5";
            else if (g == 0) return "0";

            g = ReadCard(1);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            outBuf[0] = 0xaa;
            outBuf[1] = 0xaa;
            outBuf[2] = 0xaa;
            outBuf[3] = 0xaa;
            g = VerifyCard(7);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            outBuf[0] = 0x11;
            outBuf[1] = 0x11;
            outBuf[2] = 0x11;
            outBuf[3] = 0x11;
            g = VerifyCard(0x39);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            outBuf[0] = 0x22;
            outBuf[1] = 0x22;
            outBuf[2] = 0x22;
            outBuf[3] = 0x22;
            g = VerifyCard(0x3b);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            ekarno = kartNo;
            gkarno = kartNo;
            s1karno = kartNo;
            s2karno = kartNo;


            g = ReadCard(1);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'A') || (inBuf[3] != ' '))
            {
                outBuf[0] = issue_area[0];
                outBuf[1] = issue_area[1];
                outBuf[2] = (byte)'A';
                outBuf[3] = (byte)' ';
                g = UpdateCard(1);
                if (g == 5) return "5";
                else if (g == 0) return "0";
            }

            sr = SendAboneCsc(cihazNo, 0x3d3d);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);

            sr = SendAboneCsc(cihazNo, 0x5a5a);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            g = UpdateCard(6);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            sr = SendAboneCsc(cihazNo, 0x2f2f);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);

            sr = SendAboneCsc(cihazNo, 0x1515);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            g = UpdateCard(0x38);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            sr = SendAboneCsc(cihazNo, 0xabab);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);

            sr = SendAboneCsc(cihazNo, 0xc2c2);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            g = UpdateCard(0x3a);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            Hexcon.UInt32toByte4((uint)cihazNo, outBuf);//*(long*)outBuf = cihazNo;
            g = UpdateCard(0x30);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            gkarno = gkarno & 0x0f;
            gkarno = gkarno | 0x70;

            outBuf[0] = 0xE0;
            outBuf[1] = 0xff;
            outBuf[2] = (byte)gkarno;
            outBuf[3] = 0;

            g = UpdateCard(0x3c);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            s1karno = s1karno & 0x0f;
            s1karno = s1karno | 0x70;
            outBuf[0] = 0xff;
            outBuf[1] = 0xff;
            outBuf[2] = (byte)s1karno;
            outBuf[3] = 0;

            g = UpdateCard(0x3d);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            Integer2Byte _s1lim1 = new Integer2Byte((UInt16)s1lim1);
            outBuf[0] = _s1lim1.bir;
            outBuf[1] = _s1lim1.iki;

            Integer2Byte _s1lim2 = new Integer2Byte((UInt16)s1lim2);
            outBuf[2] = _s1lim1.bir;
            outBuf[3] = _s1lim1.iki;

            g = UpdateCard(0x17);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            s1kat1 = (uint)(65535.0 * sfiyat1 / sfiyat3);
            s1kat2 = (uint)(65535.0 * sfiyat2 / sfiyat3);

            Integer2Byte _s1kat1 = new Integer2Byte((UInt16)s1kat1);
            outBuf[0] = _s1kat1.bir;
            outBuf[1] = _s1kat1.iki;

            Integer2Byte _s1kat2 = new Integer2Byte((UInt16)s1kat2);
            outBuf[2] = _s1kat2.bir;
            outBuf[3] = _s1kat2.iki;


            g = UpdateCard(0x14);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            maxfiyat = efiyat1;

            if (efiyat2 > maxfiyat) maxfiyat = efiyat2;
            if (efiyat3 > maxfiyat) maxfiyat = efiyat3;
            if (efiyat4 > maxfiyat) maxfiyat = efiyat4;

            T1kat = (uint)(65535.0 * efiyat1 / maxfiyat);
            T2kat = (uint)(65535.0 * efiyat2 / maxfiyat);
            T3kat = (uint)(65535.0 * efiyat3 / maxfiyat);
            T4kat = (uint)(65535.0 * efiyat4 / maxfiyat);

            Integer2Byte _T1kat = new Integer2Byte((UInt16)T1kat);
            outBuf[0] = _T1kat.bir;
            outBuf[1] = _T1kat.iki;

            Integer2Byte _T2kat = new Integer2Byte((UInt16)T2kat);
            outBuf[2] = _T2kat.bir;
            outBuf[3] = _T2kat.iki;

            g = UpdateCard(0x2c);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            Integer2Byte _T3kat = new Integer2Byte((UInt16)T3kat);
            outBuf[0] = _T3kat.bir;
            outBuf[1] = _T3kat.iki;

            Integer2Byte _T4kat = new Integer2Byte((UInt16)T4kat);
            outBuf[2] = _T4kat.bir;
            outBuf[3] = _T4kat.iki;


            g = UpdateCard(0x2f);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            s2karno = s2karno & 0x0f;
            s2karno = s2karno | 0x70;
            outBuf[0] = 0xff;
            outBuf[1] = 0xff;
            outBuf[2] = (byte)s2karno;
            outBuf[3] = 0;

            g = UpdateCard(0x3e);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            ekarno = ekarno & 0x0f;
            ekarno = ekarno | 0x70;
            outBuf[0] = 0xff;
            outBuf[1] = 0xff;
            outBuf[2] = (byte)ekarno;
            outBuf[3] = 0;

            g = UpdateCard(0x3f);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            outBuf[0] = 0x0;
            outBuf[1] = 0x0;
            outBuf[2] = 0x0;
            outBuf[3] = 0x0;

            for (i = 0x10; i < 0x20; i++)
            {
                if (i == 0x14)
                    i = 0x15;
                else
                    if (i == 0x17)
                    i = 0x18;

                g = UpdateCard(i);
                if (g == 5) return "5";
                else if (g == 0) return "0";
            }

            for (i = 0x28; i < 0x38; i++)
            {
                if (i == 0x2c)
                    i = 0x2d;
                if (i == 0x2f)
                    i = 0x31;


                g = UpdateCard(i);
                if (g == 5) return "5";
                else if (g == 0) return "0";
            }

            FinishCard();

            return "1";
        }

        public string AboneBosalt()
        {
            int g;
            uint sr, alfa1, alfa2;
            UInt32 dn;

            byte[] issue_area = GetIssuer();

            g = InitCard();
            if (g == 5) return "5";
            else if (g == 0) return "0";

            g = ReadCard(1);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            if ((inBuf[0] == issue_area[0]) && (inBuf[1] == issue_area[1]) && (inBuf[2] == 'A'))
            {
                g = ReadCard(0x30);
                if (g == 5) return "5";
                else if (g == 0) return "0";

                dn = (uint)Hexcon.Byte4toInt32(inBuf);//dn = *(long*)inBuf;

                alfa1 = 0x3d3d;
                sr = SendAboneCsc(dn, alfa1);
                outBuf[0] = (byte)(sr / 256);
                outBuf[1] = (byte)(sr % 256);

                alfa2 = 0x5a5a;
                sr = SendAboneCsc(dn, alfa2);
                outBuf[2] = (byte)(sr / 256);
                outBuf[3] = (byte)(sr % 256);

                g = VerifyCard(7);
                if (g == 5) return "5";
                else if (g == 0) return "0";

                alfa1 = 0x2f2f;
                sr = SendAboneCsc(dn, alfa1);
                outBuf[0] = (byte)(sr / 256);
                outBuf[1] = (byte)(sr % 256);

                alfa2 = 0x1515;
                sr = SendAboneCsc(dn, alfa2);
                outBuf[2] = (byte)(sr / 256);
                outBuf[3] = (byte)(sr % 256);

                g = VerifyCard(0x39);
                if (g == 5) return "5";
                else if (g == 0) return "0";

                alfa1 = 0xabab;
                sr = SendAboneCsc(dn, alfa1);
                outBuf[0] = (byte)(sr / 256);
                outBuf[1] = (byte)(sr % 256);

                alfa2 = 0xc2c2;
                sr = SendAboneCsc(dn, alfa2);
                outBuf[2] = (byte)(sr / 256);
                outBuf[3] = (byte)(sr % 256);

                g = VerifyCard(0x3b);
                if (g == 5) return "5";
                else if (g == 0) return "0";
            }
            else
                if ((inBuf[0] == issue_area[0]) && (inBuf[1] == issue_area[1]) && (inBuf[2] == 'Y'))
            {
                outBuf[0] = 0x3a;
                outBuf[1] = 0xdf;
                outBuf[2] = 0x1d;
                outBuf[3] = 0x80;

                g = VerifyCard(7);
                if (g == 5) return "5";
                else if (g == 0) return "0";
            }
            else
                    if ((inBuf[0] == 'A') && (inBuf[1] == 'L') && (inBuf[2] == 'U'))
            {
                outBuf[0] = 0x7B;
                outBuf[1] = 0x8A;
                outBuf[2] = 0x13;
                outBuf[3] = 0xEC;

                g = VerifyCard(7);    //rat counter adresi
                if (g == 5) return "5";
                else if (g == 0) return "0";
            }
            else
                        if ((inBuf[0] == 'K') && (inBuf[1] == 'I') && (inBuf[2] == 'U'))
            {
                outBuf[0] = 0x7b;
                outBuf[1] = 0x8a;
                outBuf[2] = 0x13;
                outBuf[3] = 0xec;

                g = VerifyCard(7);
                if (g == 5) return "5";
                else if (g == 0) return "0";
            }
            else
                            if ((inBuf[0] == issue_area[0]) && (inBuf[1] == issue_area[1]) && (inBuf[2] == 'U'))
            {
                outBuf[0] = 0xa5;
                outBuf[1] = 0x5a;
                outBuf[2] = 0x78;
                outBuf[3] = 0x84;

                g = VerifyCard(7);
                if (g == 5) return "5";
                else if (g == 0) return "0";
            }
            else
            {
                FinishCard();
                return "0";
            }


            Hexcon.UInt32toByte4(0, outBuf);//*(long*)outBuf = 0;

            g = UpdateCard(0x01);
            g = UpdateCard(0x02);
            g = UpdateCard(0x03);

            g = UpdateCard(0x3C);
            g = UpdateCard(0x3D);
            g = UpdateCard(0x3E);
            g = UpdateCard(0x3F);
            g = UpdateCard(0x10);
            g = UpdateCard(0x11);
            g = UpdateCard(0x12);
            g = UpdateCard(0x13);
            g = UpdateCard(0x14);
            g = UpdateCard(0x15);
            g = UpdateCard(0x16);
            g = UpdateCard(0x17);
            g = UpdateCard(0x18);
            g = UpdateCard(0x19);
            g = UpdateCard(0x1A);
            g = UpdateCard(0x1B);
            g = UpdateCard(0x1C);
            g = UpdateCard(0x1D);
            g = UpdateCard(0x1E);
            g = UpdateCard(0x1F);
            g = UpdateCard(0x28);
            g = UpdateCard(0x29);
            g = UpdateCard(0x2A);
            g = UpdateCard(0x2B);
            g = UpdateCard(0x2C);
            g = UpdateCard(0x2D);
            g = UpdateCard(0x2E);
            g = UpdateCard(0x2F);
            g = UpdateCard(0x30);
            g = UpdateCard(0x31);
            g = UpdateCard(0x32);
            g = UpdateCard(0x33);
            g = UpdateCard(0x34);
            g = UpdateCard(0x35);
            g = UpdateCard(0x36);
            g = UpdateCard(0x37);

            outBuf[0] = 0xaa;
            outBuf[1] = 0xaa;
            outBuf[2] = 0xaa;
            outBuf[3] = 0xaa;

            g = UpdateCard(0x06);

            outBuf[0] = 0x11;
            outBuf[1] = 0x11;
            outBuf[2] = 0x11;
            outBuf[3] = 0x11;

            g = UpdateCard(0x38);


            outBuf[0] = 0x22;
            outBuf[1] = 0x22;
            outBuf[2] = 0x22;
            outBuf[3] = 0x22;

            g = UpdateCard(0x3A);


            FinishCard();
            return "1";
        }

        public string AboneOku() // dll
        {
            TARIH elk_bilgi2, su1_bilgi2, klrmtre_bilgi2, gaz_bilgi2;

            STATE7 elk_state, su1_bilgi7, klrmtre_bilgi7;
            STATE5 gaz_state, klrmtre_state;
            STATE6 su1_state;

            int g;
            int vaa = 1;
            uint devno, Bayram1, Bayram2;
            int elk_ocak, elk_kalan, elk_harcanan;
            uint elk_krd, kkrd, gaz_krd, gaz_kalan, gaz_harcanan, gaz_ocak, klrmtre_ocak;
            uint su1_krd, su1_kalan, su1_harcanan, su1_ocak, klrmtre_krd, klrmtre_kalan, klrmtre_harcanan;
            uint servis, elk_yedekkrd, gaz_yedekkrd, su1_yedekkrd, klrmtre_yedekkrd;
            int elk_gertuk, su1_gertuk, klrmtre_gertuk;
            float pilsev1, pilsev4;
            int agul1, s1lim1, s1lim2, s1kat1, s1kat2;
            int T1, T2, T3, T4;
            uint sr;
            string elk_ako, elk_yko, gaz_ako, gaz_yko, su1_ako, su1_yko, klrmtre_ako, klrmtre_yko;
            int gul = '1';
            string a1 = "*", a2 = "*", a3 = "*", a4 = "*", a5 = "*", a6 = "*", a7 = "*", a8 = "*", a9 = "*", a10 = "*", a11 = "*", a12 = "*";
            string a13 = "*", a14 = "*", a15 = "*", a16 = "*", a17 = "*", a18 = "*", a19 = "*", a20 = "*";
            string a21 = "*", a22 = "*", a23 = "*", a24 = "*", a25 = "*", a26 = "*", a27 = "*", a28 = "*", g_yenikart, s1_yenikart, s2_yenikart, elk_yenikart;
            uint T1kat, T2kat, T3kat, T4kat;
            uint elkarno, elislem, gkarno, gislem, s1karno, s1islem, s2karno, s2islem;
            byte Bayram1Ay, Bayram1Gun, Bayram1Sure, Bayram2Ay, Bayram2Gun, Bayram2Sure;

            byte[] issue_area = GetIssuer();

            g = InitCard();
            if (g == 5) return "5";
            else if (g == 0) return "0";

            g = ReadCard(1);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'A'))
            {
                FinishCard();
                return "0";
            }

            vaa = 1;

            g = ReadCard(0x30);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            devno = (uint)Hexcon.Byte4toInt32(inBuf);//devno = *(long*)inBuf;

            sr = SendAboneCsc(devno, 0x3d3d);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devno, 0x5a5a);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            g = VerifyCard(7);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            sr = SendAboneCsc(devno, 0x2f2f);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devno, 0x1515);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            g = VerifyCard(0x39);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            sr = SendAboneCsc(devno, 0xabab);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devno, 0xc2c2);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            g = VerifyCard(0x3b);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            g = ReadCard(0x10);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            gaz_krd = (uint)Hexcon.Byte4toInt32(inBuf);//gaz_krd = *(long*)inBuf;

            g = ReadCard(0x11);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            gaz_kalan = (uint)Hexcon.Byte4toInt32(inBuf);//gaz_kalan = *(long*)inBuf;

            g = ReadCard(0x15);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            su1_kalan = (uint)Hexcon.Byte4toInt32(inBuf);//su1_kalan = *(long*)inBuf;

            g = ReadCard(0x13);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            su1_krd = (uint)Hexcon.Byte4toInt32(inBuf);//su1_krd = *(long*)inBuf;

            g = ReadCard(0x1f);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            klrmtre_krd = (uint)Hexcon.Byte4toInt32(inBuf);//klrmtre_krd = *(long*)inBuf;

            g = ReadCard(0x12);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            gaz_harcanan = (uint)Hexcon.Byte4toInt32(inBuf);//gaz_harcanan = *(long*)inBuf;

            g = ReadCard(0x1c);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            klrmtre_harcanan = (uint)Hexcon.Byte4toInt32(inBuf);//klrmtre_harcanan = *(long*)inBuf;

            g = ReadCard(0x1b);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            klrmtre_kalan = (uint)Hexcon.Byte4toInt32(inBuf);//klrmtre_kalan = *(long*)inBuf;

            g = ReadCard(0x16);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            su1_harcanan = (uint)Hexcon.Byte4toInt32(inBuf);//su1_harcanan = *(long*)inBuf;

            g = ReadCard(0x03);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            elk_ocak = (int)Hexcon.Byte4toInt32(inBuf);//elk_ocak = *(long*)inBuf;

            g = ReadCard(0x33);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            gaz_ocak = (uint)Hexcon.Byte4toInt32(inBuf);//gaz_ocak = *(long*)inBuf;

            g = ReadCard(0x02);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            klrmtre_ocak = (uint)Hexcon.Byte4toInt32(inBuf);//klrmtre_ocak = *(long*)inBuf;

            g = ReadCard(0x35);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            elk_krd = (uint)Hexcon.Byte4toInt32(inBuf);//elk_krd = *(long*)inBuf;

            g = ReadCard(0x1a);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            gaz_yedekkrd = inBuf[2];
            su1_yedekkrd = inBuf[0];
            elk_yedekkrd = inBuf[1];
            klrmtre_yedekkrd = inBuf[3];

            g = ReadCard(0x36);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            elk_kalan = (int)Hexcon.Byte4toInt32(inBuf);//elk_kalan = *(long*)inBuf;

            g = ReadCard(0x29);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            elk_harcanan = (int)Hexcon.Byte4toInt32(inBuf);//elk_harcanan = *(long*)inBuf;

            g = ReadCard(0x19);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            su1_gertuk = (int)Hexcon.Byte4toInt32(inBuf);//su1_gertuk = *(long*)inBuf;

            // kalorimetre gerçek tüketim değeri yok   0aok 11 aralik

            g = ReadCard(0x37);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            T1 = (int)Hexcon.Byte4toInt32(inBuf);//T1 = *(long*)inBuf;

            g = ReadCard(0x28);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            T2 = (int)Hexcon.Byte4toInt32(inBuf);//T2 = *(long*)inBuf;

            g = ReadCard(0x2a);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            T3 = (int)Hexcon.Byte4toInt32(inBuf);//T3 = *(long*)inBuf;

            g = ReadCard(0x2b);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            T4 = (int)Hexcon.Byte4toInt32(inBuf);//T4 = *(long*)inBuf;

            g = ReadCard(0x18);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            su1_ocak = (uint)Hexcon.Byte4toInt32(inBuf);//su1_ocak = *(long*)inBuf;

            g = ReadCard(0x14);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            Hexcon.UInt32toByte4(0, buffer);//*(long*)buffer = 0;
            buffer[0] = inBuf[0];
            buffer[1] = inBuf[1];
            s1kat1 = (int)Hexcon.Byte4toInt32(buffer);//s1kat1 = *(int*)buffer;

            buffer[0] = inBuf[2];
            buffer[1] = inBuf[3];
            s1kat2 = (int)Hexcon.Byte4toInt32(buffer);//s1kat2 = *(int*)buffer;

            g = ReadCard(0x17);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            Hexcon.UInt32toByte4(0, buffer);//*(long*)buffer = 0;
            buffer[0] = inBuf[0];
            buffer[1] = inBuf[1];
            s1lim1 = (int)Hexcon.Byte4toInt32(buffer);//s1lim1 = *(int*)buffer;

            buffer[0] = inBuf[2];
            buffer[1] = inBuf[3];
            s1lim2 = (int)Hexcon.Byte4toInt32(buffer);//s1lim2 = *(int*)buffer;

            g = ReadCard(0x31);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            elk_gertuk = (int)Hexcon.Byte4toInt32(inBuf);//elk_gertuk = *(long*)inBuf;

            g = ReadCard(0x2c);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            Hexcon.UInt32toByte4(0, buffer);//*(long*)buffer = 0;
            buffer[0] = inBuf[0];
            buffer[1] = inBuf[1];
            T1kat = (uint)Hexcon.Byte4toInt32(buffer);//T1kat = *(int*)buffer;

            buffer[0] = inBuf[2];
            buffer[1] = inBuf[3];
            T2kat = (uint)Hexcon.Byte4toInt32(buffer);//T2kat = *(int*)buffer;

            g = ReadCard(0x2f);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            Hexcon.UInt32toByte4(0, buffer);//*(long*)buffer = 0;
            buffer[0] = inBuf[0];
            buffer[1] = inBuf[1];
            T3kat = (uint)Hexcon.Byte4toInt32(buffer);//T3kat = *(int*)buffer;

            buffer[0] = inBuf[2];
            buffer[1] = inBuf[3];
            T4kat = (uint)Hexcon.Byte4toInt32(buffer);//T4kat = *(int*)buffer;

            g = ReadCard(0x2e);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            elk_bilgi2 = new TARIH(inBuf);//elk_bilgi2 = *(LEARN2*)inBuf;

            g = ReadCard(0x2d);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            su1_bilgi2 = new TARIH(inBuf);//su1_bilgi2 = *(LEARN2*)inBuf;

            g = ReadCard(0x1d);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            klrmtre_bilgi2 = new TARIH(inBuf);//klrmtre_bilgi2 = *(LEARN2*)inBuf;

            g = ReadCard(0x1e);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            buffer[0] = inBuf[2];
            buffer[1] = inBuf[3];

            gaz_bilgi2 = new TARIH(buffer);//gaz_bilgi2 = *(LEARN2*)buffer;

            g = ReadCard(0x3c);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            if ((inBuf[0] & 0x80) == 0) gaz_ako = "*";
            else gaz_ako = "b";

            if ((inBuf[0] & 0x40) == 0) gaz_yko = "*";
            else gaz_yko = "b";

            buffer[0] = inBuf[1];
            gaz_state = new STATE5(buffer);//gaz_state = *(LEARN5*)buffer;

            g_yenikart = Convert.ToString((inBuf[2] & 0x40) >> 6);
            gkarno = (uint)(inBuf[2] & 0x0F);
            gislem = inBuf[3];

            g = ReadCard(0x02);
            Bayram1 = Hexcon.Byte2toUInt16(inBuf[0], inBuf[1]);
            Bayram2 = Hexcon.Byte2toUInt16(inBuf[2], inBuf[3]);

            Bayram1Ay = Convert.ToByte(Bayram1 & 0X000F);
            Bayram1Gun = Convert.ToByte((Bayram1 >> 4) & 0X001F);
            Bayram1Sure = Convert.ToByte(Bayram1 >> 9);

            Bayram2Ay = Convert.ToByte(Bayram2 & 0X000F);
            Bayram2Gun = Convert.ToByte((Bayram2 >> 4) & 0X001F);
            Bayram2Sure = Convert.ToByte(Bayram2 >> 9);

            g = ReadCard(0x3d);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            if ((inBuf[0] & 0x80) == 0)
                su1_ako = "*";
            else
                su1_ako = "b";

            if ((inBuf[0] & 0x40) == 0)
                su1_yko = "*";
            else
                su1_yko = "b";

            buffer[0] = inBuf[1];
            su1_state = new STATE6(buffer);//su1_state = *(LEARN6*)buffer;

            s1_yenikart = Convert.ToString((inBuf[2] & 0x40) >> 6);
            s1karno = (uint)(inBuf[2] & 0x0F);
            s1islem = inBuf[3];

            g = ReadCard(0x3e);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            if ((inBuf[0] & 0x80) == 0)
                klrmtre_ako = "*";
            else
                klrmtre_ako = "b";

            if ((inBuf[0] & 0x40) == 0)
                klrmtre_yko = "*";
            else
                klrmtre_yko = "b";

            buffer[0] = inBuf[1];
            klrmtre_state = new STATE5(buffer);//klrmtre_state = *(LEARN5*)buffer;

            s2_yenikart = Convert.ToString((inBuf[2] & 0x40) >> 6);
            s2karno = (uint)(inBuf[2] & 0x0F);
            s2islem = inBuf[3];


            g = ReadCard(0x3f);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            if ((inBuf[0] & 0x80) == 0)
                elk_ako = "*";
            else
                elk_ako = "b";

            if ((inBuf[0] & 0x40) == 0)
                elk_yko = "*";
            else
                elk_yko = "b";

            buffer[0] = inBuf[1];
            elk_state = new STATE7(buffer);//elk_state = *(LEARN7*)buffer;

            elk_yenikart = Convert.ToString((inBuf[2] & 0x40) >> 6);
            elkarno = (uint)(inBuf[2] & 0x0F);
            elislem = inBuf[3];


            g = ReadCard(0x3E);   // B0AYZ
            if (g == 5) return "5";
            else if (g == 0) return "0";


            buffer[0] = inBuf[2];
            LEARN2 islem_yeni = new LEARN2(buffer);//islem_yeni = *(LEARN2*)buffer;

            buffer[0] = inBuf[3];
            LEARN1 islem_no = new LEARN1(buffer);//islem_no = *(LEARN1*)buffer;


            if (elk_state.onkapak == 1) a1 = "b"; else a1 = "*";
            if (elk_state.ariza == 1) a2 = "b"; else a2 = "*";
            if (elk_state.klemens == 1) a3 = "b"; else a3 = "*";
            if (elk_state.pzayif == 1) a4 = "b"; else a4 = "*";
            if (elk_state.pbitik == 1) a5 = "b"; else a5 = "*";

            if (gaz_state.ceza2 == 1) a6 = "b"; else a6 = "*";
            if (gaz_state.ariza == 1) a7 = "b"; else a7 = "*";
            if (gaz_state.iptal == 1) a8 = "b"; else a8 = "*";
            if (gaz_state.pzayif == 1) a9 = "b"; else a9 = "*";
            if (gaz_state.pbitik == 1) a10 = "b"; else a10 = "*";
            if (gaz_state.pkapak == 1) a11 = "b"; else a11 = "*";
            if (gaz_state.ceza1 == 1) a12 = "b"; else a12 = "*";

            if (su1_state.ceza1 == 1) a13 = "b"; else a13 = "*";
            if (su1_state.ceza2 == 1) a14 = "b"; else a14 = "*";
            if (su1_state.ariza == 1) a15 = "b"; else a15 = "*";
            if (su1_state.iptal == 1) a16 = "b"; else a16 = "*";
            if (su1_state.paz == 1) a17 = "b"; else a17 = "*";
            if (su1_state.pbitik == 1) a18 = "b"; else a18 = "*";
            if (su1_state.ceza3 == 1) a19 = "b"; else a19 = "*";
            if (su1_state.pulse_hata == 1) a20 = "b"; else a20 = "*";

            if (klrmtre_state.ceza1 == 1) a21 = "b"; else a21 = "*";
            if (klrmtre_state.ceza2 == 1) a22 = "b"; else a22 = "*";  // 0aok
            if (klrmtre_state.ariza == 1) a23 = "b"; else a23 = "*";
            //if (klrmtre_state.iptal == 1)    		a24="b";  else   a24="*";
            //if (klrmtre_state.paz == 1)    		a25="b";  else   a25="*"; 0aok
            if (klrmtre_state.pbitik == 1) a26 = "b"; else a26 = "*"; // 0aok
            //if (klrmtre_state.ceza3 == 1)     	a27="b";  else   a27="*"; 0aok
            //if (klrmtre_state.pulse_hata == 1)	a28="b";  else   a28="*"; 0aok




            string result = "E" + "#" +
                            1 + "#" +
                          devno + "#" +
                          elk_bilgi2.gun2 + "/" +
                          elk_bilgi2.ay2 + "/" +
                          elk_bilgi2.saat2 + "#" +
                          elk_krd + "#" +
                          elk_yedekkrd + "#" +
                          elk_ako + "#" +
                          elk_yko + "#" +
                          elk_yenikart + "#" +
                          elk_kalan + "#" +
                          elk_harcanan + "#" +
                          elk_gertuk + "#" +
                          elkarno + "#" +
                          elislem + "#" +
                          a1 + "#" +
                          a2 + "#" +
                          a3 + "#" +
                          a4 + "#" +
                          a5 + "#" +
                          elk_ocak + "#" +
                          T1 + "#" +
                          T2 + "#" +
                          T3 + "#" +
                          T4 + "#" +
                          T1kat + "#" +
                          T2kat + "#" +
                          T3kat + "#" +
                          T4kat + "#" +
                          islem_yeni.hata + "#" +
                          islem_yeni.kart_numarasi + "#" +
                          islem_yeni.major + "#" +
                          islem_yeni.yeni_kart + "#" +
                          islem_no.islem + "#" +
                          "|" +
                          "G" + "#" +
                          1 + "#" +
                          devno + "#" +
                          gaz_bilgi2.gun2 + "/" +
                          gaz_bilgi2.ay2 + "/" +
                          gaz_bilgi2.saat2 + "#" +
                          gaz_krd + "#" +
                          gaz_yedekkrd + "#" +
                          gaz_ako + "#" +
                          gaz_yko + "#" +
                          g_yenikart + "#" +
                          gaz_kalan + "#" +
                          gaz_harcanan + "#" +
                          gkarno + "#" +
                          gislem + "#" +
                          a6 + "#" +
                          a7 + "#" +
                          a8 + "#" +
                          a9 + "#" +
                          a10 + "#" +
                          a11 + "#" +
                          a12 + "#" +
                          gaz_ocak + "#" +
                          islem_yeni.hata + "#" +
                          islem_yeni.kart_numarasi + "#" +
                          islem_yeni.major + "#" +
                          islem_yeni.yeni_kart + "#" +
                          islem_no.islem + "#" +
                          "|" +
                          "S" + "#" +
                          1 + "#" +
                          devno + "#" +
                          su1_bilgi2.gun2 + "/" +
                          su1_bilgi2.ay2 + "/" +
                          su1_bilgi2.saat2 + "#" +
                          su1_krd + "#" +
                          su1_yedekkrd + "#" +
                          su1_ako + "#" +
                          su1_yko + "#" +
                          s1_yenikart + "#" +
                          su1_kalan + "#" +
                          su1_harcanan + "#" +
                          su1_gertuk + "#" +
                          su1_ocak + "#" +
                          s1karno + "#" +
                          s1islem + "#" +
                          s1lim1 + "#" +
                          s1lim2 + "#" +
                          s1kat1 + "#" +
                          s1kat2 + "#" +
                          a13 + "#" +
                          a14 + "#" +
                          a15 + "#" +
                          a16 + "#" +
                          a17 + "#" +
                          a18 + "#" +
                          a19 + "#" +
                          a20 + "#" +
                          islem_yeni.hata + "#" +
                          islem_yeni.kart_numarasi + "#" +
                          islem_yeni.major + "#" +
                          islem_yeni.yeni_kart + "#" +
                          islem_no.islem + "#" +
                          "|" +
                          "K" + "#" +
                          1 + "#" +
                          devno + "#" +
                          klrmtre_bilgi2.gun2 + "/" +
                          klrmtre_bilgi2.ay2 + "/" +
                          klrmtre_bilgi2.saat2 + "#" +
                          klrmtre_krd + "#" +
                          klrmtre_yedekkrd + "#" +
                          klrmtre_ako + "#" +
                          "b" + "#" +
                          s2_yenikart + "#" +
                          klrmtre_kalan + "#" +
                          klrmtre_harcanan + "#" +
                          klrmtre_ocak + "#" +
                          s2karno + "#" +
                          s2islem + "#" +
                          a21 + "#" +
                          a22 + "#" +
                          a23 + "#" +
                          a26 + "#" +
                          Bayram1Ay + "#" +
                          Bayram1Gun + "#" +
                          Bayram1Sure + "#" +
                          Bayram2Ay + "#" +
                          Bayram2Gun + "#" +
                          Bayram2Sure + "#" +
                          islem_yeni.hata + "#" +
                          islem_yeni.kart_numarasi + "#" +
                          islem_yeni.major + "#" +
                          islem_yeni.yeni_kart + "#" +
                          islem_no.islem + "#" +
                          "|";

            FinishCard();

            return result;
        }

        public string KalorimetreYaz(UInt32 devno, Int32 anakr, Int32 yedekkr, int Bayram1Gunn, int Bayram1Ayy, int Bayram1Suree, int Bayram2Gunn, int Bayram2Ayy, int Bayram2Suree)
        {

            uint[] tissue = new uint[9];
            uint[] tissue1 = new uint[9];
            uint[] tissue2 = new uint[9];
            uint[] tissue3 = new uint[9];
            uint[] tissue4 = new uint[9];
            uint kad1, kad2, kad3;
            LEARN1 islem_no;
            LEARN2 islem_yeni;

            byte[] issue_area = GetIssuer();

            uint sr;
            int ch, ch1, kartno, islemsira;
            uint devno1;
            int yedkre;
            int password;
            int g;
            uint yeni, isl;
            int Bayram1, Bayram2;

            yeni = 0;
            isl = 0;

            g = InitCard();
            if (g == 5) return "5";
            else if (g == 0) return "0";

            g = ReadCard(1);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'A') || (inBuf[3] != ' '))
            {
                FinishCard();
                return "0";
            }

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

            g = ReadCard(0x30);
            if (g == 5) return "5";
            else if (g == 0) return "0";
            devno1 = (uint)Hexcon.Byte4toInt32(inBuf);//devno1 = *(long*)inBuf;


            sr = SendAboneCsc(devno1, 0x3d3d);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devno1, 0x5a5a);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            g = VerifyCard(7);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            sr = SendAboneCsc(devno1, 0x2f2f);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devno1, 0x1515);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            g = VerifyCard(0x39);
            if (g == 5) return "5";
            else if (g == 0) return "0";


            sr = SendAboneCsc(devno1, 0xabab);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devno1, 0xc2c2);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            g = VerifyCard(0x3b);
            if (g == 5) return "5";
            else if (g == 0) return "0";



            sr = SendAboneCsc(devno, 0x3d3d);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devno, 0x5a5a);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            g = UpdateCard(6);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            sr = SendAboneCsc(devno, 0x2f2f);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devno, 0x1515);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            g = UpdateCard(0x38);
            if (g == 5) return "5";
            else if (g == 0) return "0";


            sr = SendAboneCsc(devno, 0xabab);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            sr = SendAboneCsc(devno, 0xc2c2);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            g = UpdateCard(0x3a);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            Hexcon.UInt32toByte4((uint)devno, outBuf);//*(long*)outBuf = devno;
            g = UpdateCard(0x30);
            if (g == 5) return "5";
            else if (g == 0) return "0";

            Integer2Byte _Bayram1 = new Integer2Byte((UInt16)Bayram1);
            outBuf[0] = _Bayram1.bir;
            outBuf[1] = _Bayram1.iki;

            Integer2Byte _Bayram2 = new Integer2Byte((UInt16)Bayram2);
            outBuf[2] = _Bayram2.bir;
            outBuf[3] = _Bayram2.iki;

            g = UpdateCard(0x02);
            if (g == 5) return "5";
            else if (g == 0) return "0";



            Hexcon.UInt32toByte4((uint)anakr, outBuf);//*(long*)outBuf = anakr;
            g = UpdateCard(0x1F);   	// B0AYZ
            if (g == 5) return "5";
            else if (g == 0) return "0";

            Hexcon.UInt32toByte4((uint)yedekkr, outBuf);//*(int*)outBuf = yedekkr;  		// B0AYZ
            g = UpdateCard(0x32);
            if (g == 5) return "5";
            else if (g == 0) return "0";


            Hexcon.UInt32toByte4(0, outBuf);//*(long*)outBuf = 0;
            g = UpdateCard(0x1B);////kalan kr // B0AYZ
            if (g == 5) return "5";
            else if (g == 0) return "0";

            g = UpdateCard(0x1C); // B0AYZ
            if (g == 5) return "5";
            else if (g == 0) return "0";




            g = ReadCard(0x3E);   // B0AYZ
            if (g == 5) return "5";
            else if (g == 0) return "0";


            buffer[0] = inBuf[2];
            islem_yeni = new LEARN2(buffer);//islem_yeni = *(LEARN2*)buffer;

            buffer[0] = inBuf[3];
            islem_no = new LEARN1(buffer);//islem_no = *(LEARN1*)buffer;



            if ((inBuf[0] & 0x80) == 0) islem_no.islem = islem_no.islem + 1;
            // if ((inBuf[0]&0x40)==0)  islem_no.islem=islem_no.islem+1; // B0AYZ yedek kredi okundu bilgisi kalorimetrelerde gerekmiyor

            //islem_yeni.yeni_kart = 0;  /*eski kart*/
            if (islem_yeni.yeni_kart == 0) islem_yeni.yeni_kart = 0;
            islem_yeni.major = 3;
            islem_yeni.hata = 0;


            outBuf[0] = 0xc0;



            outBuf[1] = 0xff;
            buffer[0] = 0;

            UInt32 u32 = 0;

            u32 = islem_yeni.hata & 0x01;
            u32 = u32 << 1;
            u32 += islem_yeni.yeni_kart & 0x01;
            u32 = u32 << 2;
            u32 += islem_yeni.major & 0x03;
            u32 = u32 << 4;
            u32 += islem_yeni.kart_numarasi & 0x0F;

            Hexcon.UInt32toByte4(u32, buffer);//*(LEARN2*)buffer = islem_yeni;

            outBuf[2] = buffer[0];

            u32 = 0;

            u32 = islem_no.islem;

            Hexcon.UInt32toByte4(u32, buffer);//*(LEARN1*)buffer = islem_no;

            outBuf[3] = buffer[0];

            g = UpdateCard(0x3e);  // B0AYZ
            if (g == 5) return "5";
            else if (g == 0) return "0";

            FinishCard();

            return "1";
        }

        /*public string Iade() // dll
        {
            int g = 0;
            uint aboneno, sr;
            byte[] issue_area = GetIssuer();
            g = InitCard();
            if (g == 0) { FinishCard(); HataSet(4); return "0"; }

            g = ReadCard(1);
            if (g == 0) { FinishCard(); HataSet(4); return "0"; }

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'A') || (inBuf[3] != ' '))
            {
                FinishCard();
                return "0";
            }

            g = ReadCard(0x03);

            if (g == 0) { FinishCard(); HataSet(4); return "0"; }
            aboneno = (uint)Hexcon.Byte4toInt32(inBuf);//*(long*)inBuf;

            sr = SendAboneCsc(aboneno, 0x4f4f);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            sr = SendAboneCsc(aboneno, 0x1e1e);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            g = VerifyCard(7);
            if (g == 0) { FinishCard(); HataSet(4); return "0"; }

            sr = SendAboneCsc(aboneno, 0x1c1c);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            sr = SendAboneCsc(aboneno, 0x2a2a);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            g = VerifyCard(0x39);
            if (g == 0) { FinishCard(); HataSet(4); return "0"; }

            sr = SendAboneCsc(aboneno, 0x8e8e);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            sr = SendAboneCsc(aboneno, 0x3c3c);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            g = VerifyCard(0x3b);
            if (g == 0) { FinishCard(); HataSet(4); return "0"; }


            g = ReadCard(0x15);
            if (g == 0) { FinishCard(); HataSet(4); return "0"; }

            if ((inBuf[2] & 0xF0) == 0x20)
            {
                FinishCard();
                return "6";
            }

            outBuf = inBuf;

            outBuf[2] &= 0x0f;
            outBuf[2] |= 0x10;

            g = UpdateCard(0x15);
            if (g == 0) { FinishCard(); HataSet(4); return "0"; }

            FinishCard();

            return "1";
        }

        public string SatisIptal(UInt32 aboneNo, Int32 anaKredi, Int32 yedekKredi, byte anaKrediOkundu)
        {
            int g = 0;
            byte islemno;
            uint abone_no, sr;
            byte[] issue_area = GetIssuer();

            if (aboneNo == 0 || anaKrediOkundu < 0 || anaKredi < 0 || yedekKredi < 0)
            {
                FinishCard();
                return "0";
            }

            g = InitCard();
            if (g == 0) { FinishCard(); HataSet(4); return "0"; }

            g = ReadCard(1);
            if (g == 0) { FinishCard(); HataSet(4); return "0"; }

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'A') || (inBuf[3] != ' '))
            {
                FinishCard();
                return "0";
            }


            g = ReadCard(0x03);
            if (g == 0) { FinishCard(); HataSet(4); return "0"; }

            abone_no = (uint)Hexcon.Byte4toInt32(inBuf); //*(long*)inBuf;

            if (abone_no != aboneNo)
            {
                FinishCard();
                return "0";
            }

            sr = SendAboneCsc(aboneNo, 0x4f4f);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            sr = SendAboneCsc(aboneNo, 0x1e1e);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            g = VerifyCard(7);
            if (g == 0) { FinishCard(); HataSet(4); return "0"; }

            sr = SendAboneCsc(aboneNo, 0x1c1c);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            sr = SendAboneCsc(aboneNo, 0x2a2a);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            g = VerifyCard(0x39);
            if (g == 0) { FinishCard(); HataSet(4); return "0"; }

            sr = SendAboneCsc(aboneNo, 0x8e8e);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            sr = SendAboneCsc(aboneNo, 0x3c3c);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            g = VerifyCard(0x3b);
            if (g == 0) { FinishCard(); HataSet(4); return "0"; }



            g = ReadCard(0x15);
            if (g == 0) { FinishCard(); HataSet(4); return "0"; }

            islemno = inBuf[3];

            if ((inBuf[0] & 0x60) != 0)
                islemno++;

            Hexcon.UInt32toByte4((uint)anaKredi, outBuf);            //*(long*)outBuf = anakredi;
            g = UpdateCard(0x16);
            if (g == 0) { FinishCard(); HataSet(4); return "0"; }

            Hexcon.UInt32toByte4((uint)yedekKredi, outBuf);            //*(long*)outBuf = reserve;
            g = UpdateCard(0x17);
            if (g == 0) { FinishCard(); HataSet(4); return "0"; }

            g = ReadCard(0x15);
            if (g == 0) { FinishCard(); HataSet(4); return "0"; }
            outBuf[0] = inBuf[0];
            outBuf[0] &= 0x0F;

            if (anaKrediOkundu != 0)
                outBuf[0] |= 0x60;
            else
                outBuf[0] |= 0x90;

            outBuf[1] = inBuf[1];
            outBuf[2] = inBuf[2];

            outBuf[3] = islemno;
            g = UpdateCard(0x15);
            if (g == 0) { FinishCard(); HataSet(4); return "0"; }


            g = ReadCard(0x3c);
            if (g == 0) { FinishCard(); HataSet(4); return "0"; }

            outBuf[0] = inBuf[0];
            outBuf[1] = inBuf[1];
            outBuf[2] = 0x00;  				// kart hata
            outBuf[3] = inBuf[3];
            g = UpdateCard(0x3c);
            if (g == 0) { FinishCard(); HataSet(4); return "0"; }

            FinishCard();          // dll

            return "1";

        }*/

        #endregion
    }
}
