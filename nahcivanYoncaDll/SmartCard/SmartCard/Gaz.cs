using SCLibWin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SmartCard
{

    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Gaz
    {
        public SCResMgr mng;
        public SCReader rd;
        string rdName;
        byte EXOR, newEXOR;
        #region Bufferlar
        byte[] inBuf = new byte[4];
        byte[] buffer = new byte[100];
        byte[] outBuf = new byte[10];
        byte[] Yetki = new byte[152];

        byte[] csc0 = new byte[4];
        byte[] csc1 = new byte[4];
        byte[] csc2 = new byte[4];

        #endregion
        public string zone = "GH";
        private string _hata = "";
        internal EnmDil DilSecimi { get; set; }

        #region Abone Fonksiyonlari
        public string AboneOku()
        {
            HataSet(0);

            string str = "";
            byte[] issue_area = GetIssuer();
            int i = 0;
            byte[] buf = new byte[2];
            string SayacTarihi2;
            SayacTarihi2 = "";

            i = InitCard();
            if (i == 0) { FinishCard(); HataSet(1); return "0"; }

            i = ReadCard(1);
            if (i == 0) { FinishCard(); HataSet(2); return "0"; }
            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'A') || (inBuf[3] != ' '))
            {
                FinishCard();
                HataSet(8);
                return "0";
            }

            i = ReadCard(0X3C);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }
            UInt32 devNo = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = Csc01Verify(devNo);
            if (i == 0) { FinishCard(); HataSet(3); return "0"; }

            i = ReadCard(0X10);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }
            EXOR_PROCESS(Convert.ToByte(EXOR + 1));
            Int32 gertuk = Hexcon.Byte4toInt32(inBuf);

            i = ReadCard(0X12);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }
            EXOR_PROCESS(Convert.ToByte(EXOR + 1));
            Int32 kalan = Hexcon.Byte4toInt32(inBuf);

            i = ReadCard(0X13);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }
            EXOR_PROCESS(Convert.ToByte(EXOR + 1));
            //UInt32 harcanan = Hexcon.Byte4toUInt32(inBuf);
            Int32 harcanan = Hexcon.Byte4toInt32(inBuf);
            i = ReadCard(0X14); // sayac durumu
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }
            EXOR_PROCESS(Convert.ToByte(EXOR + 1));

            string PilKapak = ""; // Ceza 2
            string PilBitik = "";
            string PilZayif = "";
            string PilBitik2 = "";
            string Ariza = "";
            string MagnetCeza = "";
            string Iptal = "";
            string PulseHata = ""; // Ceza1 : pulse kablosu hatası
            if ((inBuf[1] & 0x80) == 0) PulseHata = "*"; else PulseHata = "b"; //Ceza1  //(*) hata yok demek
            if ((inBuf[1] & 0x40) == 0) PilBitik2 = "*"; else PilBitik2 = "b";    //pilbitik
            if ((inBuf[1] & 0x20) == 0) MagnetCeza = "*"; else MagnetCeza = "b";     //MagnetCeza
            if ((inBuf[1] & 0x10) == 0) PilBitik = "*"; else PilBitik = "b";    //pilbitik
            if ((inBuf[1] & 0x08) == 0) PilZayif = "*"; else PilZayif = "b";
            if ((inBuf[1] & 0x04) == 0) Iptal = "*"; else Iptal = "b";
            if ((inBuf[1] & 0x02) == 0) Ariza = "*"; else Ariza = "b";      //ariza//
            if ((inBuf[1] & 0x01) == 0) PilKapak = "*"; else PilKapak = "b";//PilKapak Ceza 2

            byte versiyon = inBuf[0];
            string arizatip = "0";

            switch (inBuf[2] & 0xf0)
            {
                case 0x80:
                    arizatip = "ServisA";
                    break;
                case 0x40:
                    arizatip = "ServisK";
                    break;
                case 0xc0:
                    arizatip = "ServisS";
                    break;
                case 0x20:
                    arizatip = "ServisP";
                    break;
            }
            string vanapos = "";
            if ((inBuf[2] & 0x08) == 0) vanapos = "Close"; else vanapos = "Open";
            string yedekpil = String.Format("{0,5:0.000}", (inBuf[3] * 6 / 255.0));

            i = ReadCard(0X15);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }

            EXOR_PROCESS(Convert.ToByte(EXOR + 1));
            buf[0] = inBuf[0];
            buf[1] = inBuf[1];
            string sonKrediTar = Hexcon.TarihDuzenle2(buf[0], buf[1]);
            buf[0] = inBuf[2];
            buf[1] = inBuf[3];
            string sonPulseTar = Hexcon.TarihDuzenle2(buf[0], buf[1]);

            i = ReadCard(0X16);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }

            EXOR_PROCESS(Convert.ToByte(EXOR + 1));
            buf[0] = inBuf[0];
            buf[1] = inBuf[1];
            string sonCezaTar = Hexcon.TarihDuzenle2(buf[0], buf[1]);
            buf[0] = inBuf[2];
            buf[1] = inBuf[3];
            string sonArizaTar = Hexcon.TarihDuzenle2(buf[0], buf[1]);

            i = ReadCard(0X17);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }

            EXOR_PROCESS(Convert.ToByte(EXOR + 1));
            string PilSev = String.Format("{0,5:0.000}", (inBuf[1] * 0.020));
            byte sabah = inBuf[3];

            i = ReadCard(0X18);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }

            EXOR_PROCESS(Convert.ToByte(EXOR + 1));

            byte tip = inBuf[0];
            byte cap = inBuf[1];
            byte kartyazmahata = inBuf[2];
            byte aksam = inBuf[3];


            List<byte> abn = new List<byte>();

            i = ReadCard(0X19);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }
            abn.AddRange(inBuf);

            i = ReadCard(0X1A);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }
            abn.AddRange(inBuf);

            i = ReadCard(0X11);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }
            abn.AddRange(inBuf);

            string aboneNo = "";
            foreach (byte b in abn)
            {
                aboneNo += Convert.ToChar(b).ToString();
            }

            i = Csc2Verify(devNo);
            if (i == 0) { FinishCard(); HataSet(4); return "0"; }


            i = ReadCard(0X30);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }
            EXOR_PROCESS(Convert.ToByte(EXOR + 2));

            byte islem = (byte)inBuf[3];
            byte kartNo = (byte)(inBuf[2] & 0X0F);

            string yenikart = "";
            if ((inBuf[2] & 0XF0) == 0X60) yenikart = "1";
            else yenikart = "0";

            string ako = "";
            string yko = "";

            if (inBuf[0] == 0X66) ako = "*"; else ako = "b";
            if (inBuf[1] == 0X66) yko = "*"; else yko = "b";

            i = ReadCard(0X31);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }

            EXOR_PROCESS(Convert.ToByte(EXOR + 2));
            Int32 AnaKredi = Hexcon.Byte4toInt32(inBuf);

            i = ReadCard(0X32);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }

            EXOR_PROCESS(Convert.ToByte(EXOR + 2));
            Int32 YedekKredi = Hexcon.Byte4toInt32(inBuf);


            if (AnaKredi > 99999999) AnaKredi = 99999999;
            if (YedekKredi > 99999999) YedekKredi = 99999999;
            if (AnaKredi > 99999999) AnaKredi = 99999999;
            if (devNo > 99999999) devNo = 99999999;


            if (AnaKredi < -9999999) AnaKredi = -9999999;
            if (YedekKredi < -9999999) YedekKredi = -9999999;
            if (AnaKredi < -9999999) AnaKredi = -9999999;

            //if (sonKrediTar.ay > 12) sonKrediTar.ay = 12;
            //if (sonKrediTar.saat > 99) sonKrediTar.saat = 99;

            //if (sonPulseTar.ay > 12) sonPulseTar.ay = 12;
            //if (sonPulseTar.saat > 99) sonPulseTar.saat = 99;

            //if (sonCezaTar.ay > 12) sonCezaTar.ay = 12;
            //if (sonCezaTar.saat > 99) sonCezaTar.saat = 99;

            //if (sonArizaTar.ay > 12) sonArizaTar.ay = 12;
            //if (sonArizaTar.saat > 99) sonArizaTar.saat = 99;

            i = ReadCard(0X03);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }

            EXOR_PROCESS(Convert.ToByte(EXOR + 1));
            UInt32 dontuk = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);


            i = ReadCard(0X1b);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }

            EXOR_PROCESS(Convert.ToByte(EXOR + 1));
            buf[0] = inBuf[0];
            buf[1] = inBuf[1];
            string Ceza1Tar = Hexcon.TarihDuzenle2(buf[0], buf[1]);
            buf[0] = inBuf[2];
            buf[1] = inBuf[3];
            string Ceza2Tar = Hexcon.TarihDuzenle2(buf[0], buf[1]);

            i = ReadCard(0X1e);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }

            EXOR_PROCESS(Convert.ToByte(EXOR + 1));
            buf[0] = inBuf[0];
            buf[1] = inBuf[1];
            string EtransTar = Hexcon.TarihDuzenle2(buf[0], buf[1]);

            byte haftagun = inBuf[2];
            byte CezaFlag = inBuf[3];

            string HarcamaLimitFlag = ""; // Ceza flag
            string MaxDebiFlag = "";
            string Ceza3Flag = "";
            string HarcamaLimitIptalFlag = "";

            if ((CezaFlag & 0x01) == 0) HarcamaLimitFlag = "*"; else HarcamaLimitFlag = "b"; //HarcamaLimitFlag  //(*) hata yok demek
            if ((CezaFlag & 0x02) == 0) MaxDebiFlag = "*"; else MaxDebiFlag = "b";    //DebiLimitFlag
            if ((CezaFlag & 0x04) == 0) Ceza3Flag = "*"; else Ceza3Flag = "b";     //Ceza3Flag
            if ((CezaFlag & 0x08) == 0) HarcamaLimitIptalFlag = "*"; else HarcamaLimitIptalFlag = "b";    //HarcamaLimitIptalFlag
            //if ((CezaFlag & 0x08) == 0) PilZayif = "*"; else PilZayif = "b";
            //if ((CezaFlag & 0x04) == 0) Iptal = "*"; else Iptal = "b";
            //if ((CezaFlag & 0x02) == 0) Ariza = "*"; else Ariza = "b";      //ariza//
            //if ((CezaFlag & 0x01) == 0) PilKapak = "*"; else PilKapak = "b";//PilKapak Ceza 2


            //if (Ceza1Tar.ay > 12) Ceza1Tar.ay = 12;
            //if (Ceza1Tar.saat > 99) Ceza1Tar.saat = 99;
            //if (Ceza2Tar.ay > 12) Ceza2Tar.ay = 12;
            //if (Ceza2Tar.saat > 99) Ceza2Tar.saat = 99;
            //if (EtransTar.ay > 12) EtransTar.ay = 12;
            //if (EtransTar.saat > 99) EtransTar.saat = 99;

            i = ReadCard(0X1f);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }

            EXOR_PROCESS(Convert.ToByte(EXOR + 1));
            byte[] temp4 = new byte[4];
            //Array.Copy(inBuf, 0, temp4, 0, 4);
            //TarihAl sayacTarihi = new TarihAl(temp4);
            Array.Copy(inBuf, 0, temp4, 0, 4);
            SayacTarihi2 = Hexcon.TarihDuzenle2(temp4[0], temp4[1], temp4[2], temp4[3]);

            i = ReadCard(0X28);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }

            EXOR_PROCESS(Convert.ToByte(EXOR + 1));
            UInt32 dontuk9 = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X29);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }

            EXOR_PROCESS(Convert.ToByte(EXOR + 1));
            UInt32 dontuk10 = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X2a);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }

            EXOR_PROCESS(Convert.ToByte(EXOR + 1));
            UInt32 dontuk11 = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X2b);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }

            EXOR_PROCESS(Convert.ToByte(EXOR + 1));
            UInt32 dontuk12 = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);


            i = ReadCard(0X2D);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }

            EXOR_PROCESS(Convert.ToByte(EXOR + 1));
            UInt32 HarcamaLimit = Hexcon.Byte4toUInt32(inBuf);

            i = ReadCard(0X35);
            if (i == 0) { FinishCard(); return "0"; }
            EXOR_PROCESS(Convert.ToByte(EXOR + 2));
            UInt32 MaxDebi = Hexcon.Byte2toUInt16(inBuf[2], inBuf[3]);

            i = ReadCard(0X36);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }

            EXOR_PROCESS(Convert.ToByte(EXOR + 1));
            UInt16 PulseCikis = Hexcon.Byte2toUInt16(inBuf[0], inBuf[1]);

            buf[0] = inBuf[2];
            buf[1] = inBuf[3];
            string MaxDebiTar = Hexcon.TarihDuzenle2(buf[0], buf[1]);

            i = ReadCard(0X37);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }

            EXOR_PROCESS(Convert.ToByte(EXOR + 1));
            UInt16 noktaninyeri = inBuf[0];             // Noktanin yeri


            i = ReadCard(0X2E);
            if (i == 0) { FinishCard(); return "0"; }
            EXOR_PROCESS(Convert.ToByte(EXOR + 1));
            buf[0] = inBuf[0];
            buf[1] = inBuf[1];
            string Ceza3Tar = Hexcon.TarihDuzenle2(buf[0], buf[1]);

            i = ReadCard(0X2F);
            if (i == 0) { FinishCard(); return "0"; }
            EXOR_PROCESS(Convert.ToByte(EXOR + 1));
            UInt32 KritikKredi = Hexcon.Byte4toUInt32(inBuf);

            // bayram günleri
            i = ReadCard(0X2C);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }

            EXOR_PROCESS(Convert.ToByte(EXOR + 1));
            UInt16 Bayram1 = Hexcon.Byte2toUInt16(inBuf[0], inBuf[1]);
            UInt16 Bayram2 = Hexcon.Byte2toUInt16(inBuf[2], inBuf[3]);

            byte Bayram1Ay = Convert.ToByte(Bayram1 & 0X000F);
            byte Bayram1Gun = Convert.ToByte((Bayram1 >> 4) & 0X001F);
            byte Bayram1Sure = Convert.ToByte(Bayram1 >> 9);

            byte Bayram2Ay = Convert.ToByte(Bayram2 & 0X000F);
            byte Bayram2Gun = Convert.ToByte((Bayram2 >> 4) & 0X001F);
            byte Bayram2Sure = Convert.ToByte(Bayram2 >> 9);

            str += "1";
            str += "|" + devNo;
            str += "|" + aboneNo;
            str += "|" + ako;
            str += "|" + AnaKredi;
            str += "|" + yko;
            str += "|" + YedekKredi;
            str += "|" + kalan;
            str += "|" + harcanan;
            str += "|" + SayacTarihi2;
            str += "|" + haftagun;
            str += "|" + kartNo;
            str += "|" + dontuk;
            str += "|" + dontuk12;
            str += "|" + dontuk11;
            str += "|" + dontuk10;
            str += "|" + dontuk9;
            str += "|" + PilSev;
            str += "|" + yedekpil;
            str += "|" + arizatip;
            str += "|" + vanapos;
            str += "|" + sonKrediTar.ToString();
            str += "|" + sonPulseTar.ToString();
            str += "|" + sonCezaTar.ToString();
            str += "|" + sonArizaTar.ToString();
            str += "|" + Ceza1Tar.ToString();
            str += "|" + Ceza2Tar.ToString();
            str += "|" + EtransTar.ToString();
            str += "|" + MaxDebiTar.ToString();
            str += "|" + PilKapak;
            str += "|" + Ariza;
            str += "|" + Iptal;
            str += "|" + PilZayif;
            str += "|" + PilBitik;
            str += "|" + MagnetCeza;
            str += "|" + PilBitik2;
            str += "|" + PulseHata;
            str += "|" + kartyazmahata;
            str += "|" + versiyon;
            str += "|" + HarcamaLimit;
            str += "|" + yenikart;
            str += "|" + PulseCikis;

            str += "|" + Bayram1Gun;
            str += "|" + Bayram1Ay;
            str += "|" + Bayram1Sure;
            str += "|" + Bayram2Gun;
            str += "|" + Bayram2Ay;
            str += "|" + Bayram2Sure;
            str += "|" + KritikKredi;
            str += "|" + MaxDebi;
            str += "|" + sabah;
            str += "|" + aksam;
            str += "|" + tip;
            str += "|" + cap;
            str += "|" + Ceza3Tar.ToString();
            str += "|" + HarcamaLimitFlag;
            str += "|" + MaxDebiFlag;
            str += "|" + Ceza3Flag;
            str += "|" + HarcamaLimitIptalFlag;
            str += "|" + islem;
            str += "|" + gertuk;
            str += "|" + noktaninyeri;


            FinishCard();

            return str;
        }
        public string AboneYaz(UInt32 devNo, Int32 AnaKredi, Int32 YedekKredi,
                              UInt32 HarcamaLimit,
                              byte aksam, byte sabah, UInt16 PulseCikis,
                              byte Bayram1Gunn, byte Bayram1Ayy, byte Bayram1Suree,
                              byte Bayram2Gunn, byte Bayram2Ayy, byte Bayram2Suree,
                              UInt32 KritikKredi)
        {


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

            byte[] issue_area = GetIssuer();
            int i = 0;

            i = InitCard();
            if (i == 0) { FinishCard(); HataSet(1); return "0"; }

            #region Kredi bilgisi ayarlama
            int j = 0;
            j = ReadCard(0X30);
            if (j == 0) { FinishCard(); HataSet(5); return "0"; }
            EXOR_PROCESS(Convert.ToByte(EXOR + 2));

            string ako = "";
            string yko = "";

            if (inBuf[0] == 0X66) ako = "*"; else ako = "b";
            if (inBuf[1] == 0X66) yko = "*"; else yko = "b";


            j = ReadCard(0X31);
            if (j == 0) { FinishCard(); HataSet(5); return "0"; }

            EXOR_PROCESS(Convert.ToByte(EXOR + 2));
            Int32 SayactakiAnaKredi = Hexcon.Byte4toInt32(inBuf);

            j = ReadCard(0X32);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }

            EXOR_PROCESS(Convert.ToByte(EXOR + 2));
            Int32 SayactakiYedekKredi = Hexcon.Byte4toInt32(inBuf);
            //if (ako == "b" && yko == "b")
            //{
            //    if (SayactakiYedekKredi >= YedekKredi)
            //    {
            //        AnaKredi = AnaKredi + SayactakiAnaKredi;
            //    }
            //    else
            //    {
            //        if (AnaKredi + SayactakiAnaKredi >= YedekKredi)
            //        {
            //            AnaKredi = AnaKredi + SayactakiAnaKredi - YedekKredi;
            //        }
            //        else
            //        {
            //            AnaKredi = 0;
            //            YedekKredi = AnaKredi + SayactakiAnaKredi;
            //        }
            //    }
            //}
            //else if (ako == "*" && yko == "b")
            //{

            //    if (AnaKredi >= YedekKredi)
            //    {
            //        AnaKredi = AnaKredi + SayactakiYedekKredi - YedekKredi;
            //    }

            //}
            //else if (ako == "*" && yko == "*")
            //{
            //    if (AnaKredi >= KritikKredi)
            //    {
            //        AnaKredi = AnaKredi - YedekKredi;

            //    }
            //    else
            //    {
            //        YedekKredi = 0;
            //    }
            //}

            #endregion

            i = ReadCard(1);
            if (i == 0) { FinishCard(); HataSet(2); return "0"; }
            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'A') || (inBuf[3] != ' '))
            {
                FinishCard();
                HataSet(8);
                return "0";
            }

            i = Csc01Verify(devNo);
            if (i == 0) { FinishCard(); HataSet(3); return "0"; }
            i = Csc2Verify(devNo);
            if (i == 0) { FinishCard(); HataSet(4); return "0"; }

            i = ReadCard(0X30);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }

            EXOR_PROCESS(Convert.ToByte(EXOR + 2));
            Hexcon.ByteToBit learnKart = new Hexcon.ByteToBit(inBuf[2]);
            UInt32 islem = (UInt32)inBuf[3];
            if (inBuf[0] == 0X66) islem++;
            if (inBuf[1] == 0X66) islem++;

            i = YeniExor(devNo);
            if (i == 0) { FinishCard(); HataSet(6); return "0"; }

            outBuf[0] = 0;
            outBuf[1] = 0;
            outBuf[2] = 0;
            outBuf[3] = 0;
            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
            i = UpdateCard(0X12);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X13);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X15);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X16);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X1B);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            i = ReadCard(0X17);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }
            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));

            outBuf[0] = 0; //donemgun no kaldırıldı
            outBuf[1] = inBuf[1];
            outBuf[2] = 0; //donemgun kaldırıldı
            outBuf[3] = sabah;
            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
            i = UpdateCard(0X17);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            i = ReadCard(0X1e);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }
            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
            outBuf[0] = inBuf[0];
            outBuf[1] = inBuf[1];
            outBuf[2] = inBuf[2];
            outBuf[3] = inBuf[3];//hsonu_aksam;
            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
            i = UpdateCard(0X1e);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            i = ReadCard(0X14);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }
            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
            byte versiyon = inBuf[0];
            outBuf[0] = versiyon;
            outBuf[1] = 0;
            outBuf[2] = 0;
            outBuf[3] = 0;
            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
            i = UpdateCard(0X14);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            i = ReadCard(0X18);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }
            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
            byte tip = inBuf[0];
            byte cap = inBuf[1];
            outBuf[0] = tip;
            outBuf[1] = cap;
            outBuf[2] = 0;
            outBuf[3] = aksam;
            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
            i = UpdateCard(0X18);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            Integer2Byte Bayram1Deger = new Integer2Byte(Bayram1);
            Integer2Byte Bayram2Deger = new Integer2Byte(Bayram2);

            outBuf[0] = Bayram1Deger.bir;
            outBuf[1] = Bayram1Deger.iki;
            outBuf[2] = Bayram2Deger.bir;
            outBuf[3] = Bayram2Deger.iki;
            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
            i = UpdateCard(0X2C);
            if (i == 0) { FinishCard(); return "0"; }

            Hexcon.UInt32toByte4(HarcamaLimit, outBuf);
            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
            i = UpdateCard(0X2D);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            Hexcon.UInt32toByte4(KritikKredi, outBuf);
            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
            i = UpdateCard(0X2F);
            if (i == 0) { FinishCard(); return "0"; }


            outBuf[0] = 0X99;
            outBuf[1] = 0X99;
            outBuf[2] = (byte)learnKart.Deger;
            outBuf[3] = (byte)islem;
            EXOR_PROCESS(Convert.ToByte(newEXOR + 2));
            i = UpdateCard(0X30);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            Integer4Byte ana = new Integer4Byte(AnaKredi);
            outBuf[0] = ana.bir;
            outBuf[1] = ana.iki;
            outBuf[2] = ana.uc;
            outBuf[3] = ana.dort;
            EXOR_PROCESS(Convert.ToByte(newEXOR + 2));
            i = UpdateCard(0X31);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            Integer4Byte yedek = new Integer4Byte(YedekKredi);
            outBuf[0] = yedek.bir;
            outBuf[1] = yedek.iki;
            outBuf[2] = yedek.uc;
            outBuf[3] = yedek.dort;
            EXOR_PROCESS(Convert.ToByte(newEXOR + 2));
            i = UpdateCard(0X32);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            i = ReadCard(0X36);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }
            EXOR_PROCESS(Convert.ToByte(EXOR + 1));

            Hexcon.UInt16toByte2(PulseCikis, outBuf);

            outBuf[2] = inBuf[2];
            outBuf[3] = inBuf[3];

            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
            i = UpdateCard(0X36);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            i = ReadCard(0X37);
            if (i == 0) { FinishCard(); HataSet(2); return "0"; }
            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
            outBuf[0] = 3;
            outBuf[1] = inBuf[1];
            outBuf[2] = inBuf[2];
            outBuf[3] = inBuf[3];
            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
            i = UpdateCard(0X37);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }


            FinishCard();

            return "1";
        }

        public string AboneYap(UInt32 devNo, string aboneNo,
                       byte Tip, byte Cap, byte KartNo,
                       byte aksam, byte sabah,
                       UInt32 HarcamaLimit, UInt16 PulseCikis,
                       byte Bayram1Gunn, byte Bayram1Ayy, byte Bayram1Suree,
                       byte Bayram2Gunn, byte Bayram2Ayy, byte Bayram2Suree,
                       UInt32 KritikKredi)
        {
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


            byte[] issue_area = GetIssuer();
            int i = 0;

            byte fuse = 0;

            i = InitCard();
            if (i == 0) { FinishCard(); HataSet(1); return "0"; }

            i = ReadCard(1);
            if (i == 0) { FinishCard(); HataSet(2); return "0"; }

            if (inBuf[0] == 'A')
            {
                FinishCard();
                HataSet(8);
                return "0";
            }

            #region verify

            i = ReadCard(0X3C);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }

            outBuf[0] = 0X79;
            outBuf[1] = 0X81;
            outBuf[2] = 0X17;
            outBuf[3] = 0X2F;

            i = VerifyCard(0X07);
            if (i == 0) { FinishCard(); HataSet(9); return "0"; }

            outBuf[0] = 0X2E;
            outBuf[1] = 0X4C;
            outBuf[2] = 0X16;
            outBuf[3] = 0X41;

            i = VerifyCard(0X39);
            if (i == 0) { FinishCard(); HataSet(10); return "0"; }

            outBuf[0] = 0X22;
            outBuf[1] = 0X22;
            outBuf[2] = 0X22;
            outBuf[3] = 0X22;

            i = VerifyCard(0X3B);
            if (i == 0) { FinishCard(); HataSet(11); return "0"; }

            #endregion

            i = ReadCard(4);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }

            if (inBuf[3] == 0X80)
            {
                fuse = 1;
            }
            else
            {
                outBuf[0] = issue_area[0];
                outBuf[1] = issue_area[1];
                outBuf[2] = (byte)'A';
                outBuf[3] = (byte)' ';

                i = UpdateCard(1);
                if (i == 0) { FinishCard(); HataSet(12); return "0"; }

            }

            i = ReadCard(1);
            if (i == 0) { FinishCard(); HataSet(2); return "0"; }
            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'A') || (inBuf[3] != ' '))
            {
                FinishCard();
                HataSet(8);
                return "0";
            }


            i = YeniExor(devNo);
            if (i == 0) { FinishCard(); HataSet(6); return "0"; }


            Hexcon.UInt32toByte4(devNo, outBuf);
            i = UpdateCard(0X3C);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            #region sıfırlama


            outBuf[0] = 0;
            outBuf[1] = 0;
            outBuf[2] = 0;
            outBuf[3] = 0;
            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));

            i = UpdateCard(0X03);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X28);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X29);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X2A);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X2B);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X10);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X12);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X13);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X14);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X15);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X16);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X1B);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X1E);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X1F);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X36);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            //outBuf[0] = 0;
            //outBuf[1] = 0;
            //outBuf[2] = 0;
            //outBuf[3] = 0;
            //// exorsuz limitler
            i = UpdateCard(0X37);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            #endregion

            int karno = 0;
            karno = KartNo & 0X0F;
            karno |= 0X60;

            outBuf[0] = 0X99;
            outBuf[1] = 0X99;
            outBuf[2] = (byte)karno;
            outBuf[3] = 0;

            EXOR_PROCESS(Convert.ToByte(newEXOR + 2));
            i = UpdateCard(0X30);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            outBuf[0] = 0;
            outBuf[1] = 0;
            outBuf[2] = 0;
            outBuf[3] = 0;

            EXOR_PROCESS(Convert.ToByte(newEXOR + 2));
            i = UpdateCard(0X31);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            i = UpdateCard(0X32);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }



            char[] abn = aboneNo.PadRight(12, ' ').ToCharArray();

            outBuf[0] = (byte)abn[0];
            outBuf[1] = (byte)abn[1];
            outBuf[2] = (byte)abn[2];
            outBuf[3] = (byte)abn[3];

            i = UpdateCard(0X19);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            outBuf[0] = (byte)abn[4];
            outBuf[1] = (byte)abn[5];
            outBuf[2] = (byte)abn[6];
            outBuf[3] = (byte)abn[7];

            i = UpdateCard(0X1A);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            outBuf[0] = (byte)abn[8];
            outBuf[1] = (byte)abn[9];
            outBuf[2] = (byte)abn[10];
            outBuf[3] = (byte)abn[11];

            i = UpdateCard(0X11);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }


            outBuf[0] = 0;
            outBuf[1] = 0;
            outBuf[2] = 0; //donemgun kaldırıldı
            outBuf[3] = sabah;
            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
            i = UpdateCard(0X17);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            outBuf[0] = (byte)Tip;
            outBuf[1] = (byte)Cap;
            outBuf[2] = 0;
            outBuf[3] = aksam;
            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
            i = UpdateCard(0X18);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            outBuf[0] = 0;
            outBuf[1] = 0;
            outBuf[2] = 0;
            outBuf[3] = 0;// hsonu_aksam;
            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
            i = UpdateCard(0X1e);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            Integer2Byte Bayram1Deger = new Integer2Byte(Bayram1);
            Integer2Byte Bayram2Deger = new Integer2Byte(Bayram2);

            outBuf[0] = Bayram1Deger.bir;
            outBuf[1] = Bayram1Deger.iki;
            outBuf[2] = Bayram2Deger.bir;
            outBuf[3] = Bayram2Deger.iki;
            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
            i = UpdateCard(0X2C);
            if (i == 0) { FinishCard(); return "0"; }

            Hexcon.UInt32toByte4(HarcamaLimit, outBuf);
            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
            i = UpdateCard(0X2D);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            Hexcon.UInt32toByte4(KritikKredi, outBuf);
            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
            i = UpdateCard(0X2F);
            if (i == 0) { FinishCard(); return "0"; }

            //i = ReadCard(0X35);
            //if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            //EXOR_PROCESS(Convert.ToByte(EXOR + 2));

            Hexcon.UInt16toByte2(PulseCikis, outBuf);
            outBuf[2] = 0;
            outBuf[3] = 0;
            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
            i = UpdateCard(0X36);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            i = ReadCard(0X37);
            if (i == 0) { FinishCard(); HataSet(2); return "0"; }
            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
            outBuf[0] = 3;
            outBuf[1] = inBuf[1];
            outBuf[2] = inBuf[2];
            outBuf[3] = inBuf[3];
            EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
            i = UpdateCard(0X37);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            FinishCard();
            return "1";

        }

        public string KartTipi()
        {
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

        public string Bosalt()
        {
            byte[] issue_area = GetIssuer();
            int i = 0;
            UInt32 sr;

            i = InitCard();
            if (i == 0) { FinishCard(); HataSet(1); return "0"; }

            i = ReadCard(1);
            if (i == 0) { FinishCard(); HataSet(2); return "0"; }
            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'A') || (inBuf[3] != ' '))
            {
                FinishCard();
                HataSet(8);
                return "0";
            }

            i = ReadCard(0X3C);
            if (i == 0) { FinishCard(); HataSet(5); return "0"; }
            UInt32 devNo = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = Csc01Verify(devNo);
            if (i == 0) { FinishCard(); HataSet(3); return "0"; }

            i = Csc2Verify(devNo);
            if (i == 0) { FinishCard(); HataSet(4); return "0"; }

            outBuf[0] = 0;
            outBuf[1] = 0;
            outBuf[2] = 0;
            outBuf[3] = 0;

            #region SIFIRLAMA
            i = UpdateCard(1);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X10);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X11);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X11);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X13);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X14);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X15);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X16);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X17);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X18);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X19);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X1A);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X1B);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X1C);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X1D);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X1E);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X1F);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X28);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X29);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X2A);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X2B);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X2C);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X2D);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X2E);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X2F);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X30);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X31);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X32);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X33);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X34);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X35);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X36);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X37);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X3C);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X3D);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X3E);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X3F);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            i = UpdateCard(0X05);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }
            #endregion

            outBuf[0] = 0x79;
            outBuf[1] = 0x81;
            outBuf[2] = 0x17;
            outBuf[3] = 0x2F;
            i = UpdateCard(6);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            outBuf[0] = 0x2E;
            outBuf[1] = 0x4C;
            outBuf[2] = 0x16;
            outBuf[3] = 0x41;
            i = UpdateCard(0X38);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            outBuf[0] = 0x22;
            outBuf[1] = 0x22;
            outBuf[2] = 0x22;
            outBuf[3] = 0x22;
            i = UpdateCard(0X3A);
            if (i == 0) { FinishCard(); HataSet(7); return "0"; }

            FinishCard();
            return "1";
        }


        public string TipTanimlama(string IssuerArea)
        {
            byte[] issue_area = GetIssuer();

            string stIssuer = Convert.ToChar(issue_area[0]).ToString() + Convert.ToChar(issue_area[1]).ToString();

            if (stIssuer + "A " == IssuerArea)
            {
                IssuerArea = "Abone Karti";
                return IssuerArea;
            }
            if (stIssuer + "YA" == IssuerArea)
            {
                IssuerArea = "Yetki Açma Karti";
                return IssuerArea;
            }
            if (stIssuer + "YB" == IssuerArea)
            {
                IssuerArea = "Yetki Bilgi Karti";
                return IssuerArea;
            }
            if (stIssuer + "YI" == IssuerArea)
            {
                IssuerArea = "Yetki İade Karti";
                return IssuerArea;
            }
            if (stIssuer + "YK" == IssuerArea)
            {
                IssuerArea = "Yetki Kapama";
                return IssuerArea;
            }
            if (stIssuer + "YR" == IssuerArea)
            {
                IssuerArea = "Yetki Reset";
                return IssuerArea;
            }
            if (stIssuer + "YT" == IssuerArea)
            {
                IssuerArea = "Yetki EDeğiş";
                return IssuerArea;
            }
            if (stIssuer + "YS" == IssuerArea)
            {
                IssuerArea = "Yetki Saat";
                return IssuerArea;
            }
            if (stIssuer + "Y4" == IssuerArea)
            {
                IssuerArea = "Yetki Ceza4";
                return IssuerArea;
            }
            if (stIssuer + "YV" == IssuerArea)
            {
                IssuerArea = "Yetki Avans";
                return IssuerArea;
            }
            if (stIssuer + "YC" == IssuerArea)
            {
                IssuerArea = "Yetki İptal";
                return IssuerArea;
            }

            switch (IssuerArea)
            {

                case "KSA ":
                    IssuerArea = "Abone Karti";
                    break;
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
                    IssuerArea = "Boş Kart";
                    break;
            }
            return IssuerArea;

        }

        #endregion



        private byte[] GetIssuer()
        {
            byte[] issue_area = new byte[2];

            issue_area[0] = (byte)Convert.ToChar(zone.Substring(0, 1));
            issue_area[1] = (byte)Convert.ToChar(zone.Substring(1, 1));

            return issue_area;
        }

        #region IAcr128
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
        public string HataSet(int Kod)
        {
            _hata = Kod + ":";
            if (DilSecimi == EnmDil.English)
            {
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
            else
            {
                switch (Kod)
                {
                    case 0:
                        _hata += "Başarılı";
                        break;
                    case 1:
                        _hata += "Kart Bağlantı Hata";
                        break;
                    case 2:
                        _hata += "Kart Issuer Okuma Hatası";
                        break;
                    case 3:
                        _hata += "Csc01Verify Hatası";
                        break;
                    case 4:
                        _hata += "Csc2Verify Hatası";
                        break;
                    case 5:
                        _hata += "Kart Okuma Hatası";
                        break;
                    case 6:
                        _hata += "NewExor Hatası";
                        break;
                    case 7:
                        _hata += "Kart Yazma Hatası";
                        break;
                    case 8:
                        _hata += "Abone Kartı Değil";
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
                        _hata += "Kart Issuer Yazma Hatası";
                        break;
                    case 13:
                        _hata += "Yetki Kartı Değil";
                        break;
                    case 14:
                        _hata += "Yetki Bilgi Kartı Değil";
                        break;
                    case 15:
                        _hata += "Yetki Kartı Sayaca Takılmamış";
                        break;
                    case 16:
                        _hata += "Yetki Iade Kartı Değil";
                        break;
                    case 17:
                        _hata += "Uretim Kartı Değil";
                        break;
                    default:
                        _hata = "0:";
                        _hata += "Başarılı";
                        break;
                }
            }

            return _hata;
        }

        #endregion

        public int Csc01Verify(UInt32 devNo)
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

        public int Csc2Verify(UInt32 devNo)
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

        private UInt32 SendAboneCsc(UInt32 dn, UInt32 alfa)
        {
            UInt32 carry;
            UInt32 rmask, pmask, qmask;
            UInt32 t1, t2, t3, t4;
            UInt32 sr;
            UInt32 i;

            pmask = 1 << 1;
            rmask = 1 << 2;
            qmask = 1 << 4;

            sr = dn;
            sr = sr + ((alfa * 5) + 2);
            sr = sr & 0x0000ffff;
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

        private void EXOR_PROCESS(byte x)
        {
            for (int i = 0; i < 4; i++)
            {
                inBuf[i] ^= x;
                outBuf[i] ^= x;
            }
        }

        public int VerifyCard(byte adres)
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

        public int YeniExor(UInt32 devNo)
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

            Outbuf_Randomize();

            buf[0] = outBuf[0];
            buf[1] = outBuf[1];
            UInt16 alfa1 = Hexcon.Byte2toUInt16(buf[0], buf[1]);

            buf[0] = outBuf[2];
            buf[1] = outBuf[3];
            UInt16 alfa2 = Hexcon.Byte2toUInt16(buf[0], buf[1]);

            EXOR_PROCESS(newEXOR);

            i = UpdateCard(0X3D);
            if (i == 0) return 0;

            Outbuf_Randomize();

            buf[0] = outBuf[0];
            buf[1] = outBuf[1];
            UInt16 alfa3 = Hexcon.Byte2toUInt16(buf[0], buf[1]);

            buf[0] = outBuf[2];
            buf[1] = outBuf[3];
            UInt16 alfa4 = Hexcon.Byte2toUInt16(buf[0], buf[1]);

            EXOR_PROCESS(newEXOR);

            i = UpdateCard(0X3E);
            if (i == 0) return 0;

            Outbuf_Randomize();

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

        public int UpdateCard(byte adres)
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

        public void Outbuf_Randomize()
        {
            UInt32 sId = (UInt32)((DateTime.Now.Ticks / 100000) % 0xffffffff);
            outBuf[0] = (byte)(sId % (256 * 256 * 256));
            sId = sId / 256;
            outBuf[1] = (byte)(sId % (256 * 256));
            sId = sId / 256;
            outBuf[2] = (byte)(sId % 256);
            outBuf[3] = (byte)(sId / 256);

        }
    }

    internal enum EnmDil
    {
        English = 0,
        Turkce = 1
    }

}
