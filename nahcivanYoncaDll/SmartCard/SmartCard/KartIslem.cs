using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace SmartCard
{
    #region tarife

    internal struct Trf
    {
        public string[] DegiskenDegerleri;
        public byte BilgiTipi, KartKodu, Cap, Tip;

        public UInt32 CihazNo, AboneNo;
        public Int32 KalanKredi;
        public UInt32 HarcananKredi, KritikKredi, Limit1, Limit2, Limit3, Limit4, Katsayi1, Katsayi2, Katsayi3, Katsayi4, Katsayi5,
               DonemTuketimi, NegatifTuketim, GerCekTuketim;
        public Int32 SonYuklenenKredi;
        public UInt32 Kademe1Tuketimi, Kademe2Tuketimi, Kademe3Tuketimi, Kademe4Tuketimi, Kademe5Tuketimi,
               DonemTuketim1, DonemTuketim2, DonemTuketim3, DonemTuketim4, DonemTuketim5, DonemTuketim6;

        //4 Byte    
        public string SayacTarihi;
        //2 Byte
        public string SonKrediTarihi, SonPulseTarihi, SonCezaTarihi, SonArizaTarihi, Ceza3Tarihi, Ceza4Tarihi,
        BorcTarihi, VanaAcilmaTarihi, VanaKapanmaTarihi;

        public string MotorPil;
        public byte HaftaninGunu;
        public string ArizaTipi, ETransDurum;
        public byte VanaPulseSure, VanaCntSure,
             KartNo, IslemNo, Vop, TestSaat;
        public string TestReel;
        public byte SayacDurumu, ResetSayisi;

        public string AnaPil, Format;

        public byte Versiyon, Kademe, DonemGun, DonemGunNo;

        //2 Byte
        public string MaxdebiTarihi;

        public byte MaxdebiSeviye, MaxdebiSinir;

        public string VanaDurumu;

        public UInt32 DonemTuketim7, DonemTuketim8, DonemTuketim9, DonemTuketim10,
               DonemTuketim11, DonemTuketim12, MekanikTuketim;

        public byte HaftaSonuOnay;

        public string Degiskenler;

        public UInt32 ToplamYuklenenKredi;
        public string YazilimVersiyon;
        public string IlkPulseTarihi;



        byte Index;

        private UInt32 Byte4toUInt32(byte bir, byte iki, byte uc, byte dort)
        {
            return (UInt32)((dort << 24) + (uc << 16) + (iki << 8) + bir);
        }

        private Int32 Byte4toInt32(byte bir, byte iki, byte uc, byte dort)
        {
            return ((dort << 24) + (uc << 16) + (iki << 8) + bir);
        }

        private string StringDuzenleBastan(string DuzenlenecekString, int IstenilenUzunluk)
        {
            int SifirSayisi = IstenilenUzunluk - DuzenlenecekString.Length;

            if (SifirSayisi > 0)
            {
                for (int i = 0; i < SifirSayisi; i++)
                {
                    DuzenlenecekString = "0" + DuzenlenecekString;
                }

            }
            return DuzenlenecekString;
        }

        private string StringDuzenleBastan(int DuzenlenecekSayi, int IstenilenUzunluk)
        {
            string DuzenlenecekString = DuzenlenecekSayi.ToString();
            int SifirSayisi = IstenilenUzunluk - DuzenlenecekString.Length;

            if (SifirSayisi > 0)
            {
                for (int i = 0; i < SifirSayisi; i++)
                {
                    DuzenlenecekString = "0" + DuzenlenecekString;
                }

            }
            return DuzenlenecekString;
        }

        private string TarihDuzenle(int Byte1, int Byte2)
        {
            int Gun = ((Byte1 & 240) >> 4) + ((Byte2 & 1) << 4);

            int Ay = Byte1 & 15;

            int Yil = (Byte2 & 254) >> 1;
            Yil += 2000;

            if ((Gun > 31) || (Ay > 12) || (Yil > 2099) || (Gun == 0) || (Ay == 0))
            {
                return "00/00/2000";
            }
            else
            {
                return StringDuzenleBastan(Gun.ToString(), 2) + "/" +
                       StringDuzenleBastan(Ay.ToString(), 2) + "/" +
                       Yil.ToString();
            }

        }

        private string TarihDuzenle(int Byte1, int Byte2, int Byte3, int Byte4)
        {
            string DondurulecekData = TarihDuzenle(Byte1, Byte2);

            if ((Byte3 > 23) || (Byte4 > 59) || (DondurulecekData == "00/00/2000"))
            {
                return "00/00/2000 00:00";
            }
            else
            {
                DondurulecekData += " " + StringDuzenleBastan(Byte3.ToString(), 2) +
                                    ":" + StringDuzenleBastan(Byte4.ToString(), 2);

                return DondurulecekData;
            }

        }

        public Trf(byte[] OkunanDegerler)
        {


            Degiskenler = "BilgiTipi;KartKodu;Cap;Tip;CihazNo;AboneNo;KalanKredi;HarcananKredi;KritikKredi;Limit1;Limit2;Katsayi1;Katsayi2;Katsayi3;DonemTuketimi;NegatifTuketim;GerCekTuketim;SonYuklenenKredi;Kademe1Tuketimi;Kademe2Tuketimi;Kademe3Tuketimi;DonemTuketim1;DonemTuketim2;DonemTuketim3;DonemTuketim4;DonemTuketim5;DonemTuketim6;SayacTarihi;SonKrediTarihi;SonPulseTarihi;SonCezaTarihi;SonArizaTarihi;Ceza3Tarihi;Ceza4Tarihi;BorcTarihi;VanaAcilmaTarihi;VanaKapanmaTarihi;MotorPil;HaftaninGunu;ArizaTipi;ETransDurum;VanaPulseSure;VanaCntSure;KartNo;IslemNo;Vop;TestSaat;TestReel;SayacDurumu;AnaPil;Format;Versiyon;Kademe;DonemGun;DonemGunNo;MaxdebiTarihi;MaxdebiSeviye;MaxdebiSinir;VanaDurumu;DonemTuketim7;DonemTuketim8;DonemTuketim9;DonemTuketim10;DonemTuketim11;DonemTuketim12;Mektuk;HaftaSonuOnay;ResetSayısı;ToplamYuk.Kredi;YazılımVersiyon;İlkPulseTarihi;Katsayi4;Katsayi5;Limit3;Limit4;Kademe4Tuketimi;Kademe5Tuketimi";
            string[] Degerler = Degiskenler.Split(';');
            DegiskenDegerleri = new string[Degerler.Length];

            BilgiTipi = KartKodu = Cap = Tip = ResetSayisi = HaftaSonuOnay = 0;

            CihazNo = AboneNo = 0;
            KalanKredi = 0;
            HarcananKredi = KritikKredi = Limit1 = Limit2 = Limit3 = Limit4 = Katsayi1 = Katsayi2 = Katsayi3 = Katsayi4 = Katsayi5 =
            DonemTuketimi = NegatifTuketim = GerCekTuketim = 0;
            SonYuklenenKredi = 0;
            Kademe1Tuketimi = Kademe2Tuketimi = Kademe3Tuketimi = Kademe4Tuketimi = Kademe5Tuketimi =
            DonemTuketim1 = DonemTuketim2 = DonemTuketim3 = DonemTuketim4 = DonemTuketim5 = DonemTuketim6 = 0;
            //4 Byte    
            SayacTarihi = "";
            //2 Byte
            SonKrediTarihi = SonPulseTarihi = SonCezaTarihi = SonArizaTarihi = Ceza3Tarihi = Ceza4Tarihi =
            BorcTarihi = VanaAcilmaTarihi = VanaKapanmaTarihi = "";

            MotorPil = "";

            HaftaninGunu = 0;
            ArizaTipi = ETransDurum = "";
            VanaPulseSure = VanaCntSure =
            KartNo = IslemNo = Vop = TestSaat = 0;
            TestReel = "";
            SayacDurumu = 0;

            AnaPil = Format = "";

            Versiyon = Kademe = DonemGun = DonemGunNo = 0;
            //2 Byte
            MaxdebiTarihi = "";

            MaxdebiSeviye = MaxdebiSinir = 0;

            VanaDurumu = "";

            DonemTuketim7 = DonemTuketim8 = DonemTuketim9 = DonemTuketim10 =
            DonemTuketim11 = DonemTuketim12 = MekanikTuketim = 0;

            HaftaSonuOnay = 0;

            ToplamYuklenenKredi = 0;
            YazilimVersiyon = "";
            IlkPulseTarihi = "";

            Index = 0;

            //-----------------------------------------

            BilgiTipi = OkunanDegerler[Index];
            KartKodu = OkunanDegerler[Index + 1]; Index++;
            Cap = OkunanDegerler[Index + 1]; Index++;
            Tip = OkunanDegerler[Index + 1]; Index++;

            CihazNo = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            AboneNo = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            KalanKredi = Byte4toInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            HarcananKredi = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            KritikKredi = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Limit1 = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Limit2 = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Katsayi1 = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Katsayi2 = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Katsayi3 = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketimi = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            NegatifTuketim = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            GerCekTuketim = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            SonYuklenenKredi = Byte4toInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            Kademe1Tuketimi = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Kademe2Tuketimi = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Kademe3Tuketimi = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim1 = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim2 = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim3 = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim4 = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim5 = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim6 = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            SayacTarihi = TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            SonKrediTarihi = TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            SonPulseTarihi = TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            SonCezaTarihi = TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            SonArizaTarihi = TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            Ceza3Tarihi = TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            Ceza4Tarihi = TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            BorcTarihi = TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            VanaAcilmaTarihi = TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            VanaKapanmaTarihi = TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;

            MotorPil = String.Format("{0,8:0.000}", OkunanDegerler[Index + 1] * 6 / 255.0); Index++;

            HaftaninGunu = OkunanDegerler[Index + 1]; Index++;

            switch (OkunanDegerler[Index + 1])
            {
                case 0x10:
                    ArizaTipi = "ServisA";
                    break;
                case 0x20:
                    ArizaTipi = "ServisK";
                    break;
                case 0x30:
                    ArizaTipi = "ServisS";
                    break;
                case 0x40:
                    ArizaTipi = "ServisP";
                    break;
            }
            Index++;

            switch (OkunanDegerler[Index + 1])
            {
                case 11:
                    ETransDurum = "Olumlu";
                    break;
                default:
                    ETransDurum = "Olumsuz";
                    break;
            }
            Index++;

            VanaPulseSure = OkunanDegerler[Index + 1]; Index++;
            VanaCntSure = OkunanDegerler[Index + 1]; Index++;
            KartNo = OkunanDegerler[Index + 1]; Index++;
            IslemNo = OkunanDegerler[Index + 1]; Index++;
            Vop = OkunanDegerler[Index + 1]; Index++;
            TestSaat = OkunanDegerler[Index + 1]; Index++;

            switch (OkunanDegerler[Index + 1])
            {
                case 0x00:
                    TestReel = "Test Mode";
                    break;
                case 0xFF:
                    TestReel = "Reel Mode";
                    break;
            }
            Index++;

            SayacDurumu = OkunanDegerler[Index + 1]; Index++;

            AnaPil = String.Format("{0,8:0.000}", OkunanDegerler[Index + 1] * 6 / 255.0); Index++;

            switch (OkunanDegerler[Index + 1])
            {
                case 0x00:
                    Format = "Sayaç Formatlı";
                    break;
                default:
                    Format = "Sayaç Formatsız";
                    break;
            }
            Index++;

            Versiyon = OkunanDegerler[Index + 1]; Index++;
            Kademe = OkunanDegerler[Index + 1]; Index++;
            DonemGun = OkunanDegerler[Index + 1]; Index++;
            DonemGunNo = OkunanDegerler[Index + 1]; Index++;

            MaxdebiTarihi = TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;

            MaxdebiSeviye = OkunanDegerler[Index + 1]; Index++;
            MaxdebiSinir = OkunanDegerler[Index + 1]; Index++;

            switch (OkunanDegerler[Index + 1])
            {
                case 0:
                    VanaDurumu = "Kapalı";
                    break;
                case 1:
                    VanaDurumu = "Açık";
                    break;
            }
            Index++;

            DonemTuketim7 = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim8 = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim9 = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim10 = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim11 = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim12 = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            MekanikTuketim = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            HaftaSonuOnay = OkunanDegerler[Index + 1]; Index++;
            ResetSayisi = OkunanDegerler[Index + 1]; Index++;

            ToplamYuklenenKredi = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            YazilimVersiyon = Convert.ToChar(OkunanDegerler[Index + 1]).ToString() + Convert.ToChar(OkunanDegerler[Index + 2]).ToString() + Convert.ToChar(OkunanDegerler[Index + 3]).ToString() + Convert.ToChar(OkunanDegerler[Index + 4]).ToString() +
                              Convert.ToChar(OkunanDegerler[Index + 5]).ToString() + Convert.ToChar(OkunanDegerler[Index + 6]).ToString() + Convert.ToChar(OkunanDegerler[Index + 7]).ToString() + Convert.ToChar(OkunanDegerler[Index + 8]).ToString() +
                              Convert.ToChar(OkunanDegerler[Index + 9]).ToString() + Convert.ToChar(OkunanDegerler[Index + 10]).ToString();
            Index += 10;
            IlkPulseTarihi = TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;

            Katsayi4 = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Katsayi5 = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Limit3 = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Limit4 = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Kademe4Tuketimi = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Kademe5Tuketimi = Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            #region Değişken Değerleri
            DegiskenDegerleri[0] = BilgiTipi.ToString();
            DegiskenDegerleri[1] = KartKodu.ToString();
            DegiskenDegerleri[2] = Cap.ToString();
            DegiskenDegerleri[3] = Tip.ToString();
            DegiskenDegerleri[4] = CihazNo.ToString();
            DegiskenDegerleri[5] = AboneNo.ToString();
            DegiskenDegerleri[6] = KalanKredi.ToString();
            DegiskenDegerleri[7] = HarcananKredi.ToString();
            DegiskenDegerleri[8] = KritikKredi.ToString();
            DegiskenDegerleri[9] = Limit1.ToString();
            DegiskenDegerleri[10] = Limit2.ToString();
            DegiskenDegerleri[11] = Katsayi1.ToString();
            DegiskenDegerleri[12] = Katsayi2.ToString();
            DegiskenDegerleri[13] = Katsayi3.ToString();
            DegiskenDegerleri[14] = DonemTuketimi.ToString();
            DegiskenDegerleri[15] = NegatifTuketim.ToString();
            DegiskenDegerleri[16] = GerCekTuketim.ToString();
            DegiskenDegerleri[17] = SonYuklenenKredi.ToString();
            DegiskenDegerleri[18] = Kademe1Tuketimi.ToString();
            DegiskenDegerleri[19] = Kademe2Tuketimi.ToString();
            DegiskenDegerleri[20] = Kademe3Tuketimi.ToString();
            DegiskenDegerleri[21] = DonemTuketim1.ToString();
            DegiskenDegerleri[22] = DonemTuketim2.ToString();
            DegiskenDegerleri[23] = DonemTuketim3.ToString();
            DegiskenDegerleri[24] = DonemTuketim4.ToString();
            DegiskenDegerleri[25] = DonemTuketim5.ToString();
            DegiskenDegerleri[26] = DonemTuketim6.ToString();
            DegiskenDegerleri[27] = SayacTarihi.ToString();
            DegiskenDegerleri[28] = SonKrediTarihi.ToString();
            DegiskenDegerleri[29] = SonPulseTarihi.ToString();
            DegiskenDegerleri[30] = SonCezaTarihi.ToString();
            DegiskenDegerleri[31] = SonArizaTarihi.ToString();
            DegiskenDegerleri[32] = Ceza3Tarihi.ToString();
            DegiskenDegerleri[33] = Ceza4Tarihi.ToString();
            DegiskenDegerleri[34] = BorcTarihi.ToString();
            DegiskenDegerleri[35] = VanaAcilmaTarihi.ToString();
            DegiskenDegerleri[36] = VanaKapanmaTarihi.ToString();
            DegiskenDegerleri[37] = MotorPil.ToString();
            DegiskenDegerleri[38] = HaftaninGunu.ToString();
            DegiskenDegerleri[39] = ArizaTipi.ToString();
            DegiskenDegerleri[40] = ETransDurum.ToString();
            DegiskenDegerleri[41] = VanaPulseSure.ToString();
            DegiskenDegerleri[42] = VanaCntSure.ToString();
            DegiskenDegerleri[43] = KartNo.ToString();
            DegiskenDegerleri[44] = IslemNo.ToString();
            DegiskenDegerleri[45] = Vop.ToString();
            DegiskenDegerleri[46] = TestSaat.ToString();
            DegiskenDegerleri[47] = TestReel.ToString();
            DegiskenDegerleri[48] = SayacDurumu.ToString();
            DegiskenDegerleri[49] = AnaPil.ToString();
            DegiskenDegerleri[50] = Format.ToString();
            DegiskenDegerleri[51] = Versiyon.ToString();
            DegiskenDegerleri[52] = Kademe.ToString();
            DegiskenDegerleri[53] = DonemGun.ToString();
            DegiskenDegerleri[54] = DonemGunNo.ToString();
            DegiskenDegerleri[55] = MaxdebiTarihi.ToString();
            DegiskenDegerleri[56] = MaxdebiSeviye.ToString();
            DegiskenDegerleri[57] = MaxdebiSinir.ToString();
            DegiskenDegerleri[58] = VanaDurumu.ToString();
            DegiskenDegerleri[59] = DonemTuketim7.ToString();
            DegiskenDegerleri[60] = DonemTuketim8.ToString();
            DegiskenDegerleri[61] = DonemTuketim9.ToString();
            DegiskenDegerleri[62] = DonemTuketim10.ToString();
            DegiskenDegerleri[63] = DonemTuketim11.ToString();
            DegiskenDegerleri[64] = DonemTuketim12.ToString();
            DegiskenDegerleri[65] = MekanikTuketim.ToString();
            DegiskenDegerleri[66] = HaftaSonuOnay.ToString();
            DegiskenDegerleri[67] = ResetSayisi.ToString();
            DegiskenDegerleri[68] = ToplamYuklenenKredi.ToString();
            DegiskenDegerleri[69] = YazilimVersiyon;
            DegiskenDegerleri[70] = IlkPulseTarihi;
            DegiskenDegerleri[71] = Katsayi4.ToString();
            DegiskenDegerleri[72] = Katsayi5.ToString();
            DegiskenDegerleri[73] = Limit3.ToString();
            DegiskenDegerleri[74] = Limit4.ToString();
            DegiskenDegerleri[75] = Kademe4Tuketimi.ToString();
            DegiskenDegerleri[76] = Kademe5Tuketimi.ToString();




            #endregion
        }
    }

    #endregion
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class KartIslem : IKartIslem
    {
        ContactReader rd = new ContactReader();
        Util util = new Util();

        private string zone="BK";
        string _hata;
        string sp = "|"; // seperator

        //public IKartOkuyucu KartOkuyucu
        //{
        //    get
        //    {
        //        if (rd == null) return new ContactReader();
        //        else return rd;
        //    }
        //    set
        //    {
        //        rd = value;
        //    }
        //}

        public KartIslem()
        {
        }

       
       

        public string HataMesaji
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

        private void HataSet(int Kod)
        {
            _hata = Kod + ":";
            switch (Kod)
            {
                #region kodlar

                case 0:
                    _hata += "Başarılı";
                    break;
                case 1:
                    _hata += "Kart bağlantı hata";
                    break;
                case 2:
                    _hata += "Kart Pin1 doğrulanamadı";
                    break;
                case 3:
                    _hata += "Kart Pin2 doğrulanamadı";
                    break;
                case 4:
                    _hata += "Kart Pin3 doğrulanamadı";
                    break;
                case 5:
                    _hata += "Kart okuma hata";
                    break;
                case 6:
                    _hata += "Kart yazma hata";
                    break;
                case 7:
                    _hata += "Tanımsız Kart veya farklı bölge";
                    break;
                case 8:
                    _hata += "Yetki kartı değil";
                    break;
                case 9:
                    _hata += "Yetki pini doğrulanamadı";
                    break;
                case 10:
                    _hata += "Yetki bilgi kartı değil";
                    break;
                case 11:
                    _hata += "Yetki bilgi kartı sayaca takılmamış";
                    break;
                case 12:
                    _hata += "Yetki iade kartı değil";
                    break;
                case 13:
                    _hata += "Gaz bilgi kartı değil";
                    break;
                case 14:
                    _hata += "Elektrik bilgi kartı değil";
                    break;
                case 15:
                    _hata += "Üretim kartı değil";
                    break;
                default:
                    _hata = "0:";
                    _hata += "Başarılı";
                    break;

                    #endregion
            }
        }

        private string Return(int kod)
        {
            HataSet(kod);
            return rd.FinishCard();
        }

        #region  Kart Okuma Yazma Fonksyonları

        private void SetOutBuf(byte byte0, byte byte1, byte byte2, byte byte3)
        {
            rd.OutBuf[0] = byte0;
            rd.OutBuf[1] = byte1;
            rd.OutBuf[2] = byte2;
            rd.OutBuf[3] = byte3;
        }
        private void SetOutBuf(UInt16 deger1, UInt16 deger2)
        {
            byte[] b1 = new byte[2];
            Hexcon.UInt16toByte2(deger1, b1);

            byte[] b2 = new byte[2];
            Hexcon.UInt16toByte2(deger2, b2);

            Array.Copy(b1, 0, rd.OutBuf, 0, 2);
            Array.Copy(b2, 0, rd.OutBuf, 2, 2);
        }

        #endregion
        private byte[] GetIssuer()
        {
            byte[] issue_area = new byte[2];

            issue_area[0] = (byte)Convert.ToChar(zone.Substring(0, 1));
            issue_area[1] = (byte)Convert.ToChar(zone.Substring(1, 1));

            return issue_area;
        }
        public string KartTipi()
        {
            HataSet(0);

            byte[] issue_area = GetIssuer();
            int i = 0;

            #region Init

           
            i = rd.InitCard();
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(1); }

            i = rd.ReadCard(1, 4);
            if (i == 5) { return Return(5); }
            else if (i == 0) { return Return(5); }

            rd.FinishCard();

            #endregion

            return util.TipTanimlama(Convert.ToChar(rd.InBuf[0]).ToString() + Convert.ToChar(rd.InBuf[1]).ToString() + Convert.ToChar(rd.InBuf[2]).ToString() + Convert.ToChar(rd.InBuf[3]).ToString(), zone);
        }

        #region csc

        private string CscVerify(UInt32 CihazNo)
        {
            int i = 0;

            UInt32 sr;

            sr = util.SendAboneCsc(CihazNo, 0X3D3D);
            rd.OutBuf[0] = (byte)(sr / 256);
            rd.OutBuf[1] = (byte)(sr % 256);
            sr = util.SendAboneCsc(CihazNo, 0X5A5A);
            rd.OutBuf[2] = (byte)(sr / 256);
            rd.OutBuf[3] = (byte)(sr % 256);

            i = rd.VerifyCard(7);
            if (i == 0) { return Return(2); }

            sr = util.SendAboneCsc(CihazNo, 0X2F2F);
            rd.OutBuf[0] = (byte)(sr / 256);
            rd.OutBuf[1] = (byte)(sr % 256);
            sr = util.SendAboneCsc(CihazNo, 0X1515);
            rd.OutBuf[2] = (byte)(sr / 256);
            rd.OutBuf[3] = (byte)(sr % 256);

            i = rd.VerifyCard(0X39);
            if (i == 0) { return Return(3); }

            sr = util.SendAboneCsc(CihazNo, 0XABAB);
            rd.OutBuf[0] = (byte)(sr / 256);
            rd.OutBuf[1] = (byte)(sr % 256);
            sr = util.SendAboneCsc(CihazNo, 0XC2C2);
            rd.OutBuf[2] = (byte)(sr / 256);
            rd.OutBuf[3] = (byte)(sr % 256);

            i = rd.VerifyCard(0X3B);
            if (i == 0) { return Return(4); }

            return i.ToString();
        }

        private string CscVerifyOkuYaz(UInt32 CihazNo1, UInt32 CihazNo)
        {
            int i = 0;

            UInt32 sr;

            #region csc verify

            sr = util.SendAboneCsc(CihazNo1, 0X3D3D);
            rd.OutBuf[0] = (byte)(sr / 256);
            rd.OutBuf[1] = (byte)(sr % 256);
            sr = util.SendAboneCsc(CihazNo1, 0X5A5A);
            rd.OutBuf[2] = (byte)(sr / 256);
            rd.OutBuf[3] = (byte)(sr % 256);

            i = rd.VerifyCard(7);
            if (i == 0) { return Return(2); }

            sr = util.SendAboneCsc(CihazNo1, 0X2F2F);
            rd.OutBuf[0] = (byte)(sr / 256);
            rd.OutBuf[1] = (byte)(sr % 256);
            sr = util.SendAboneCsc(CihazNo1, 0X1515);
            rd.OutBuf[2] = (byte)(sr / 256);
            rd.OutBuf[3] = (byte)(sr % 256);

            i = rd.VerifyCard(0X39);
            if (i == 0) { return Return(3); }

            sr = util.SendAboneCsc(CihazNo1, 0XABAB);
            rd.OutBuf[0] = (byte)(sr / 256);
            rd.OutBuf[1] = (byte)(sr % 256);
            sr = util.SendAboneCsc(CihazNo1, 0XC2C2);
            rd.OutBuf[2] = (byte)(sr / 256);
            rd.OutBuf[3] = (byte)(sr % 256);

            i = rd.VerifyCard(0X3B);
            if (i == 0) { return Return(4); }

            #endregion

            #region update csc

            sr = util.SendAboneCsc(CihazNo, 0X3D3D);
            rd.OutBuf[0] = (byte)(sr / 256);
            rd.OutBuf[1] = (byte)(sr % 256);
            sr = util.SendAboneCsc(CihazNo, 0X5A5A);
            rd.OutBuf[2] = (byte)(sr / 256);
            rd.OutBuf[3] = (byte)(sr % 256);

            i = rd.UpdateCard(6);
            if (i == 0) { return Return(6); }

            sr = util.SendAboneCsc(CihazNo, 0X2F2F);
            rd.OutBuf[0] = (byte)(sr / 256);
            rd.OutBuf[1] = (byte)(sr % 256);
            sr = util.SendAboneCsc(CihazNo, 0X1515);
            rd.OutBuf[2] = (byte)(sr / 256);
            rd.OutBuf[3] = (byte)(sr % 256);

            i = rd.UpdateCard(0X38);
            if (i == 0) { return Return(6); }

            sr = util.SendAboneCsc(CihazNo, 0XABAB);
            rd.OutBuf[0] = (byte)(sr / 256);
            rd.OutBuf[1] = (byte)(sr % 256);
            sr = util.SendAboneCsc(CihazNo, 0XC2C2);
            rd.OutBuf[2] = (byte)(sr / 256);
            rd.OutBuf[3] = (byte)(sr % 256);

            i = rd.UpdateCard(0X3A);
            if (i == 0) { return Return(6); }

            #endregion

            return i.ToString();
        }

        private string CscVerifyYaz(UInt32 CihazNo)
        {
            int i = 0;

            UInt32 sr;

            #region update csc

            sr = util.SendAboneCsc(CihazNo, 0X3D3D);
            rd.OutBuf[0] = (byte)(sr / 256);
            rd.OutBuf[1] = (byte)(sr % 256);
            sr = util.SendAboneCsc(CihazNo, 0X5A5A);
            rd.OutBuf[2] = (byte)(sr / 256);
            rd.OutBuf[3] = (byte)(sr % 256);

            i = rd.UpdateCard(6);
            if (i == 0) { return Return(6); }

            sr = util.SendAboneCsc(CihazNo, 0X2F2F);
            rd.OutBuf[0] = (byte)(sr / 256);
            rd.OutBuf[1] = (byte)(sr % 256);
            sr = util.SendAboneCsc(CihazNo, 0X1515);
            rd.OutBuf[2] = (byte)(sr / 256);
            rd.OutBuf[3] = (byte)(sr % 256);

            i = rd.UpdateCard(0X38);
            if (i == 0) { return Return(6); }

            sr = util.SendAboneCsc(CihazNo, 0XABAB);
            rd.OutBuf[0] = (byte)(sr / 256);
            rd.OutBuf[1] = (byte)(sr % 256);
            sr = util.SendAboneCsc(CihazNo, 0XC2C2);
            rd.OutBuf[2] = (byte)(sr / 256);
            rd.OutBuf[3] = (byte)(sr % 256);

            i = rd.UpdateCard(0X3A);
            if (i == 0) { return Return(6); }

            #endregion

            return i.ToString();
        }

        #endregion

        #region Abone Kartı Fonksyonları

        public void BayramDuzenle(UInt16 Bayram1Tarih, UInt16 Bayram2Tarih)
        {

            Integer2Byte Bayram1Deger = new Integer2Byte(Bayram1Tarih);
            Integer2Byte Bayram2Deger = new Integer2Byte(Bayram2Tarih);

            rd.OutBuf[0] = Bayram1Deger.bir;
            rd.OutBuf[1] = Bayram1Deger.iki;
            rd.OutBuf[2] = Bayram2Deger.bir;
            rd.OutBuf[3] = Bayram2Deger.iki;
        }

        public string AboneYap(UInt32 CihazNo, byte KartNo,
                               UInt16 SuLimit1, UInt16 SuLimit2,
                               UInt32 SuFiyat1, UInt32 SuFiyat2, UInt32 SuFiyat3,
                               UInt32 ElkFiyat1, UInt32 ElkFiyat2, UInt32 ElkFiyat3, UInt32 ElkFiyat4)
        {
            HataSet(0);

            #region init

            byte[] issue_area = GetIssuer();
            int i = 0;

            i = rd.InitCard();
            if (i == 0) { return Return(1); }

            rd.OutBuf[0] = 0XAA;
            rd.OutBuf[1] = 0XAA;
            rd.OutBuf[2] = 0XAA;
            rd.OutBuf[3] = 0XAA;
            i = rd.VerifyCard(0X07);
            if (i == 0) { return Return(2); }

            rd.OutBuf[0] = 0X11;
            rd.OutBuf[1] = 0X11;
            rd.OutBuf[2] = 0X11;
            rd.OutBuf[3] = 0X11;
            i = rd.VerifyCard(0X39);
            if (i == 0) { return Return(3); }

            rd.OutBuf[0] = 0X22;
            rd.OutBuf[1] = 0X22;
            rd.OutBuf[2] = 0X22;
            rd.OutBuf[3] = 0X22;
            i = rd.VerifyCard(0X3B);
            if (i == 0) { return Return(4); }

            i = rd.ReadCard(1, 4);
            if (i == 0) { return Return(5); }

            if ((rd.InBuf[0] != issue_area[0]) || (rd.InBuf[1] != issue_area[1]) || (rd.InBuf[2] != 'A') || (rd.InBuf[3] != ' '))
            {
                rd.OutBuf[0] = issue_area[0];
                rd.OutBuf[1] = issue_area[1];
                rd.OutBuf[2] = (byte)'A';
                rd.OutBuf[3] = (byte)' ';

                i = rd.UpdateCard(1);
                if (i == 0) { return Return(6); }
            }

            #endregion

            #region csc verify yaz

            string csc = CscVerifyYaz(CihazNo);
            if (csc != "1") { return Return(Convert.ToInt32(csc)); }

            #endregion

            #region kart yazma

            byte elkKartno = KartNo;
            byte SuKartNo = KartNo;
            byte SicakSuKartNo = KartNo;
            byte gazKartNo = KartNo;


            Hexcon.UInt32toByte4(CihazNo, rd.OutBuf);
            i = rd.UpdateCard(0X30);
            if (i == 0) { return Return(6); }

            gazKartNo &= 0X0F;
            gazKartNo |= 0X70;

            SetOutBuf(0XE0, 0XFF, gazKartNo, 0);

            i = rd.UpdateCard(0X3C);
            if (i == 0) { return Return(6); }

            SuKartNo &= 0X0F;
            SuKartNo |= 0X70;

            SetOutBuf(0XFF, 0XFF, SuKartNo, 0);

            i = rd.UpdateCard(0X3D);
            if (i == 0) { return Return(6); }

            SetOutBuf(SuLimit1, SuLimit2);

            i = rd.UpdateCard(0X17);
            if (i == 0) { return Return(6); }

            UInt16 s1kat1 = Convert.ToUInt16(Convert.ToDouble(65535.0 * SuFiyat1) / Convert.ToDouble(SuFiyat3));
            UInt16 s1kat2 = Convert.ToUInt16(Convert.ToDouble(65535.0 * SuFiyat2) / Convert.ToDouble(SuFiyat3));

            SetOutBuf(s1kat1, s1kat2);

            i = rd.UpdateCard(0X14);
            if (i == 0) { return Return(6); }

            UInt32 maxFiyat = ElkFiyat1;

            if (ElkFiyat2 > maxFiyat) maxFiyat = ElkFiyat2;
            if (ElkFiyat3 > maxFiyat) maxFiyat = ElkFiyat3;
            if (ElkFiyat4 > maxFiyat) maxFiyat = ElkFiyat4;

            UInt16 t1kat = Convert.ToUInt16(Convert.ToDouble(65535.0 * ElkFiyat1) / Convert.ToDouble(maxFiyat));
            UInt16 t2kat = Convert.ToUInt16(Convert.ToDouble(65535.0 * ElkFiyat2) / Convert.ToDouble(maxFiyat));
            UInt16 t3kat = Convert.ToUInt16(Convert.ToDouble(65535.0 * ElkFiyat3) / Convert.ToDouble(maxFiyat));
            UInt16 t4kat = Convert.ToUInt16(Convert.ToDouble(65535.0 * ElkFiyat4) / Convert.ToDouble(maxFiyat));

            SetOutBuf(t1kat, t2kat);

            i = rd.UpdateCard(0X2C);
            if (i == 0) { return Return(6); }

            SetOutBuf(t3kat, t4kat);

            i = rd.UpdateCard(0X2F);
            if (i == 0) { return Return(6); }


            SicakSuKartNo &= 0X0F;
            SicakSuKartNo |= 0X70;

            SetOutBuf(0XFF, 0XFF, SicakSuKartNo, 0);

            i = rd.UpdateCard(0X3E);
            if (i == 0) { return Return(6); }


            elkKartno &= 0X0F;
            elkKartno |= 0X70;

            SetOutBuf(0XFF, 0XFF, elkKartno, 0);

            i = rd.UpdateCard(0X3F);
            if (i == 0) { return Return(6); }


            SetOutBuf(0X0, 0X0, 0X0, 0X0);


            for (byte index = 0X10; index < 0X20; index++)
            {
                if (index == 0X14) index = 0X15;
                else
                    if (index == 0X17) index = 0X18;

                i = rd.UpdateCard(index);
                if (i == 0) { return Return(6); }
            }

            for (byte index = 0X28; index < 0X38; index++)
            {
                if (index == 0X2C) index = 0X2D;
                if (index == 0X2F) index = 0X31;

                i = rd.UpdateCard(index);
                if (i == 0) { return Return(6); }
            }

            #endregion

            rd.FinishCard();

            return "1";
        }


        public string AboneOku()
        {
            HataSet(0);

            byte[] issue_area = GetIssuer();
            int i = 0;


            i = rd.InitCard();
            if (i == 0) { return Return(1); }

            i = rd.ReadCard(1, 4);
            if (i == 0) { return Return(5); }

            if ((rd.InBuf[0] != issue_area[0]) || (rd.InBuf[1] != issue_area[1]) || (rd.InBuf[2] != 'A') || (rd.InBuf[3] != ' '))
            {
                return Return(7);
            }

            i = rd.ReadCard(0X30, 4);
            if (i == 0) { return Return(5); }

            UInt32 CihazNo = Hexcon.Byte4toUInt32(rd.InBuf);

            #region csc verify

            string csc = CscVerify(CihazNo);
            if (csc != "1") { return Return(Convert.ToInt32(csc)); }

            #endregion

            i = rd.ReadCard(0X1A, 4);
            if (i == 0) { return Return(5); }

            byte suYedekKredi = rd.InBuf[0];
            byte elkYedekKredi = rd.InBuf[1];
            byte gazYedekKredi = rd.InBuf[2];
            byte sicakSuYedekKredi = rd.InBuf[3];

            #region gaz

            i = rd.ReadCard(0X10, 4);
            if (i == 0) { return Return(5); }
            UInt32 gazKredi = Hexcon.Byte4toUInt32(rd.InBuf);

            i = rd.ReadCard(0X11, 4);
            if (i == 0) { return Return(5); }
            Int32 gazKalan = Hexcon.Byte4toInt32(rd.InBuf);

            i = rd.ReadCard(0X12, 4);
            if (i == 0) { return Return(5); }
            UInt32 gazHarcanan = Hexcon.Byte4toUInt32(rd.InBuf);

            i = rd.ReadCard(0X33, 4);
            if (i == 0) { return Return(5); }
            UInt32 gazOcak = Hexcon.Byte4toUInt32(rd.InBuf);

            i = rd.ReadCard(0X1E, 4);
            if (i == 0) { return Return(5); }
            string gazTarih = Hexcon.TarihOlustur(Hexcon.ByteDiziOlustur(rd.InBuf, 2, 2));

            i = rd.ReadCard(0X3C, 4);
            if (i == 0) { return Return(5); }

            string gazAko = "";
            string gazYko = "";

            if ((rd.InBuf[0] & 0X80) == 0) gazAko = "*"; else gazAko = "b";
            if ((rd.InBuf[0] & 0X40) == 0) gazYko = "*"; else gazYko = "b";

            string gazCeza2 = "";
            string gazAriza = "";
            string gazIptal = "";
            string gazPilZayif = "";
            string gazPilBitik = "";
            string gazPilKapak = "";
            string gazCeza1 = "";

            Hexcon.ByteToBit gazState = new Hexcon.ByteToBit(rd.InBuf[1]);
            gazCeza2 = (gazState.Bit1 == 1) ? "b" : "*";
            gazAriza = (gazState.Bit2 == 1) ? "b" : "*";
            gazIptal = (gazState.Bit3 == 1) ? "b" : "*";
            gazPilZayif = (gazState.Bit4 == 1) ? "b" : "*";
            gazPilBitik = (gazState.Bit5 == 1) ? "b" : "*";
            gazPilKapak = (gazState.Bit6 == 1) ? "b" : "*";
            gazCeza1 = (gazState.Bit8 == 1) ? "b" : "*";

            byte gazYeniKart = (byte)((rd.InBuf[2] & 0X40) >> 6);
            byte gazKartNo = (byte)(rd.InBuf[2] & 0X0F);
            byte gazIslemNo = rd.InBuf[3];


            Hexcon.Islem isl = new Hexcon.Islem(rd.InBuf[2]);

            Hexcon.Islem isl1 = new Hexcon.Islem(gazKartNo, 3, gazYeniKart, 0);

            #endregion

            #region soğuk su

            i = rd.ReadCard(0X13, 4);
            if (i == 0) { return Return(5); }
            UInt32 suKredi = Hexcon.Byte4toUInt32(rd.InBuf);

            i = rd.ReadCard(0X14, 4);
            if (i == 0) { return Return(5); }
            UInt16 suKat1 = Hexcon.Byte2toUInt16(rd.InBuf[0], rd.InBuf[1]);
            UInt16 suKat2 = Hexcon.Byte2toUInt16(rd.InBuf[2], rd.InBuf[3]);


            i = rd.ReadCard(0X15, 4);
            if (i == 0) { return Return(5); }
            Int32 suKalan = Hexcon.Byte4toInt32(rd.InBuf);

            i = rd.ReadCard(0X16, 4);
            if (i == 0) { return Return(5); }
            UInt32 suHarcanan = Hexcon.Byte4toUInt32(rd.InBuf);

            i = rd.ReadCard(0X17, 4);
            if (i == 0) { return Return(5); }
            UInt16 suLim1 = Hexcon.Byte2toUInt16(rd.InBuf[0], rd.InBuf[1]);
            UInt16 suLim2 = Hexcon.Byte2toUInt16(rd.InBuf[2], rd.InBuf[3]);

            i = rd.ReadCard(0X18, 4);
            if (i == 0) { return Return(5); }
            UInt32 suOcak = Hexcon.Byte4toUInt32(rd.InBuf);

            i = rd.ReadCard(0X19, 4);
            if (i == 0) { return Return(5); }
            UInt32 suGerTuk = Hexcon.Byte4toUInt32(rd.InBuf);

            i = rd.ReadCard(0X2D, 4);
            if (i == 0) { return Return(5); }
            string suTarih = Hexcon.TarihOlustur(Hexcon.ByteDiziOlustur(rd.InBuf, 0, 2));

            i = rd.ReadCard(0X3D, 4);
            if (i == 0) { return Return(5); }

            string suAko = "";
            string suYko = "";

            if ((rd.InBuf[0] & 0X80) == 0) suAko = "*"; else suAko = "b";
            if ((rd.InBuf[0] & 0X40) == 0) suYko = "*"; else suYko = "b";

            string suCeza1 = "";
            string suCeza2 = "";
            string suAriza = "";
            string suIptal = "";
            string suPilAz = "";
            string suPilBitik = "";
            string suCeza3 = "";
            string suPulseHata = "";

            Hexcon.ByteToBit suState = new Hexcon.ByteToBit(rd.InBuf[1]);
            suCeza1 = (suState.Bit1 == 1) ? "b" : "*";
            suCeza2 = (suState.Bit2 == 1) ? "b" : "*";
            suAriza = (suState.Bit3 == 1) ? "b" : "*";
            suIptal = (suState.Bit4 == 1) ? "b" : "*";
            suPilAz = (suState.Bit5 == 1) ? "b" : "*";
            suPilBitik = (suState.Bit6 == 1) ? "b" : "*";
            suCeza3 = (suState.Bit7 == 1) ? "b" : "*";
            suPulseHata = (suState.Bit8 == 1) ? "b" : "*";

            byte suYeniKart = (byte)((rd.InBuf[2] & 0X40) >> 6);
            byte suKartNo = (byte)(rd.InBuf[2] & 0X0F);
            byte suIslemNo = rd.InBuf[3];

            #endregion

            #region Sıcak Su

            i = rd.ReadCard(0X1F, 4);
            if (i == 0) { return Return(5); }
            UInt32 sicakSuKredi = Hexcon.Byte4toUInt32(rd.InBuf);

            i = rd.ReadCard(0X1B, 4);
            if (i == 0) { return Return(5); }
            Int32 sicakSuKalan = Hexcon.Byte4toInt32(rd.InBuf);

            i = rd.ReadCard(0X1C, 4);
            if (i == 0) { return Return(5); }
            UInt32 sicakSuHarcanan = Hexcon.Byte4toUInt32(rd.InBuf);

            i = rd.ReadCard(0X02, 4);
            if (i == 0) { return Return(5); }
            UInt32 sicakSuOcak = Hexcon.Byte4toUInt32(rd.InBuf);

            i = rd.ReadCard(0X34, 4);
            if (i == 0) { return Return(5); }
            UInt32 sicakSuGerTuk = Hexcon.Byte4toUInt32(rd.InBuf);

            i = rd.ReadCard(0X1D, 4);
            if (i == 0) { return Return(5); }
            string sicakSuTarih = Hexcon.TarihOlustur(Hexcon.ByteDiziOlustur(rd.InBuf, 0, 2));

            i = rd.ReadCard(0X3E, 4);
            if (i == 0) { return Return(5); }

            string sicakSuAko = "";
            string sicakSuYko = "";

            if ((rd.InBuf[0] & 0X80) == 0) sicakSuAko = "*"; else sicakSuAko = "b";
            if ((rd.InBuf[0] & 0X40) == 0) sicakSuYko = "*"; else sicakSuYko = "b";

            string sicakSuCeza1 = "";
            string sicakSuCeza2 = "";
            string sicakSuAriza = "";
            string sicakSuIptal = "";
            string sicakSuPilAz = "";
            string sicakSuPilBitik = "";
            string sicakSuCeza3 = "";
            string sicakSuPulseHata = "";

            Hexcon.ByteToBit sicakSuState = new Hexcon.ByteToBit(rd.InBuf[1]);
            sicakSuCeza1 = (sicakSuState.Bit1 == 1) ? "b" : "*";
            sicakSuCeza2 = (sicakSuState.Bit2 == 1) ? "b" : "*";
            sicakSuAriza = (sicakSuState.Bit3 == 1) ? "b" : "*";
            sicakSuIptal = (sicakSuState.Bit4 == 1) ? "b" : "*";
            sicakSuPilAz = (sicakSuState.Bit5 == 1) ? "b" : "*";
            sicakSuPilBitik = (sicakSuState.Bit6 == 1) ? "b" : "*";
            sicakSuCeza3 = (sicakSuState.Bit7 == 1) ? "b" : "*";
            sicakSuPulseHata = (sicakSuState.Bit7 == 1) ? "b" : "*";

            byte sicakSuYeniKart = (byte)((rd.InBuf[2] & 0X40) >> 6);
            byte sicakSuKartNo = (byte)(rd.InBuf[2] & 0X0F);
            byte sicakSuIslemNo = rd.InBuf[3];

            #endregion

            #region elektrik

            i = rd.ReadCard(0X03, 4);
            if (i == 0) { return Return(5); }
            UInt32 elkOcak = Hexcon.Byte4toUInt32(rd.InBuf);

            i = rd.ReadCard(0X35, 4);
            if (i == 0) { return Return(5); }
            UInt32 elkKredi = Hexcon.Byte4toUInt32(rd.InBuf);

            i = rd.ReadCard(0X36, 4);
            if (i == 0) { return Return(5); }
            Int32 elkKalan = Hexcon.Byte4toInt32(rd.InBuf);

            i = rd.ReadCard(0X29, 4);
            if (i == 0) { return Return(5); }
            UInt32 elkHarcanan = Hexcon.Byte4toUInt32(rd.InBuf);

            i = rd.ReadCard(0X37, 4);
            if (i == 0) { return Return(5); }
            UInt32 elkT1 = Hexcon.Byte4toUInt32(rd.InBuf);

            i = rd.ReadCard(0X28, 4);
            if (i == 0) { return Return(5); }
            UInt32 elkT2 = Hexcon.Byte4toUInt32(rd.InBuf);

            i = rd.ReadCard(0X2A, 4);
            if (i == 0) { return Return(5); }
            UInt32 elkT3 = Hexcon.Byte4toUInt32(rd.InBuf);

            i = rd.ReadCard(0X2B, 4);
            if (i == 0) { return Return(5); }
            UInt32 elkT4 = Hexcon.Byte4toUInt32(rd.InBuf);

            i = rd.ReadCard(0X31, 4);
            if (i == 0) { return Return(5); }
            UInt32 elkGerTuk = Hexcon.Byte4toUInt32(rd.InBuf);

            i = rd.ReadCard(0X2C, 4);
            if (i == 0) { return Return(5); }
            UInt16 elkT1Kat = Hexcon.Byte2toUInt16(rd.InBuf[0], rd.InBuf[1]);
            UInt16 elkT2Kat = Hexcon.Byte2toUInt16(rd.InBuf[2], rd.InBuf[3]);

            i = rd.ReadCard(0X2F, 4);
            if (i == 0) { return Return(5); }
            UInt16 elkT3Kat = Hexcon.Byte2toUInt16(rd.InBuf[0], rd.InBuf[1]);
            UInt16 elkT4Kat = Hexcon.Byte2toUInt16(rd.InBuf[2], rd.InBuf[3]);

            i = rd.ReadCard(0X2E, 4);
            if (i == 0) { return Return(5); }
            string elkTarih = Hexcon.TarihOlustur(Hexcon.ByteDiziOlustur(rd.InBuf, 0, 2));

            i = rd.ReadCard(0X3F, 4);
            if (i == 0) { return Return(5); }

            string elkAko = "";
            string elkYko = "";

            if ((rd.InBuf[0] & 0X80) == 0) elkAko = "*"; else elkAko = "b";
            if ((rd.InBuf[0] & 0X40) == 0) elkYko = "*"; else elkYko = "b";

            string elkOnKapak = "";
            string elkAriza = "";
            string elkKlemens = "";
            string elkPilZayif = "";
            string elkPilBitik = "";

            Hexcon.ByteToBit elkState = new Hexcon.ByteToBit(rd.InBuf[1]);
            elkOnKapak = (elkState.Bit1 == 1) ? "b" : "*";
            elkAriza = (elkState.Bit2 == 1) ? "b" : "*";
            elkKlemens = (elkState.Bit3 == 1) ? "b" : "*";
            elkPilZayif = (elkState.Bit4 == 1) ? "b" : "*";
            elkPilBitik = (elkState.Bit5 == 1) ? "b" : "*";

            byte elkYeniKart = (byte)((rd.InBuf[2] & 0X40) >> 6);
            byte elkKartNo = (byte)(rd.InBuf[2] & 0X0F);
            byte elkIslemNo = rd.InBuf[3];


            #endregion

            rd.FinishCard();

            #region return dosya işlemleri

            string DosyaAdi = "elk_aoku.txt";
            if (File.Exists(DosyaAdi))
            {
                File.Delete(DosyaAdi);
            }
            StreamWriter Dosya = new StreamWriter(DosyaAdi);

            Dosya.WriteLine(CihazNo);
            Dosya.WriteLine(elkTarih);
            Dosya.WriteLine(elkKredi);
            Dosya.WriteLine(elkYedekKredi);
            Dosya.WriteLine(elkAko);
            Dosya.WriteLine(elkYko + elkYeniKart);
            Dosya.WriteLine(elkKalan);
            Dosya.WriteLine(elkHarcanan);
            Dosya.WriteLine(elkGerTuk);
            Dosya.WriteLine(elkKartNo);
            Dosya.WriteLine(elkIslemNo);
            Dosya.WriteLine(elkOnKapak + elkAriza + elkKlemens + elkPilZayif + elkPilBitik);
            Dosya.WriteLine(elkOcak);
            Dosya.WriteLine(elkT1);
            Dosya.WriteLine(elkT2);
            Dosya.WriteLine(elkT3);
            Dosya.WriteLine(elkT4);
            Dosya.WriteLine(elkT1Kat);
            Dosya.WriteLine(elkT2Kat);
            Dosya.WriteLine(elkT3Kat);
            Dosya.WriteLine(elkT4Kat);
            Dosya.Close();

            DosyaAdi = "gaz_aoku.txt";
            if (File.Exists(DosyaAdi))
            {
                File.Delete(DosyaAdi);
            }
            Dosya = new StreamWriter(DosyaAdi);

            Dosya.WriteLine(CihazNo);
            Dosya.WriteLine(gazTarih);
            Dosya.WriteLine(gazKredi);
            Dosya.WriteLine(gazYedekKredi);
            Dosya.WriteLine(gazAko);
            Dosya.WriteLine(gazYko + gazYeniKart);
            Dosya.WriteLine(gazKalan);
            Dosya.WriteLine(gazHarcanan);
            Dosya.WriteLine(gazKartNo);
            Dosya.WriteLine(gazIslemNo);
            Dosya.WriteLine(gazCeza2 + gazAriza + gazIptal + gazPilZayif + gazPilBitik + gazPilKapak + gazCeza1);
            Dosya.Close();

            DosyaAdi = "su1_aoku.txt";
            if (File.Exists(DosyaAdi))
            {
                File.Delete(DosyaAdi);
            }
            Dosya = new StreamWriter(DosyaAdi);

            Dosya.WriteLine(CihazNo);
            Dosya.WriteLine(suTarih);
            Dosya.WriteLine(suKredi);
            Dosya.WriteLine(suYedekKredi);
            Dosya.WriteLine(suAko);
            Dosya.WriteLine(suYko + suYeniKart);
            Dosya.WriteLine(suKalan);
            Dosya.WriteLine(suHarcanan);
            Dosya.WriteLine(suGerTuk);
            Dosya.WriteLine(suOcak);
            Dosya.WriteLine(suKartNo);
            Dosya.WriteLine(suIslemNo);
            Dosya.WriteLine(suLim1);
            Dosya.WriteLine(suLim2);
            Dosya.WriteLine(suKat1);
            Dosya.WriteLine(suKat2);
            Dosya.WriteLine(suCeza1 + suCeza2 + suAriza + suIptal + suPilAz + suPilBitik + suCeza3 + suPulseHata);
            Dosya.Close();

            DosyaAdi = "su2_aoku.txt";
            if (File.Exists(DosyaAdi))
            {
                File.Delete(DosyaAdi);
            }
            Dosya = new StreamWriter(DosyaAdi);

            Dosya.WriteLine(CihazNo);
            Dosya.WriteLine(sicakSuTarih);
            Dosya.WriteLine(sicakSuKredi);
            Dosya.WriteLine(sicakSuYedekKredi);
            Dosya.WriteLine(sicakSuAko);
            Dosya.WriteLine(sicakSuAko + sicakSuYeniKart);
            Dosya.WriteLine(sicakSuKalan);
            Dosya.WriteLine(sicakSuHarcanan);
            Dosya.WriteLine(sicakSuGerTuk);
            Dosya.WriteLine(sicakSuOcak);
            Dosya.WriteLine(sicakSuKartNo);
            Dosya.WriteLine(sicakSuIslemNo);
            Dosya.WriteLine(sicakSuCeza1 + sicakSuCeza2 + sicakSuAriza + sicakSuIptal + sicakSuPilAz + sicakSuPilBitik + sicakSuCeza3 + sicakSuPulseHata);
            Dosya.Close();

            return "1";
            #endregion


        }

        public string AboneYazElk(UInt32 CihazNo,
                                  Int32 ElkAnaKredi, byte ElkYedekKredi,
                                  UInt32 ElkFiyat1, UInt32 ElkFiyat2, UInt32 ElkFiyat3, UInt32 ElkFiyat4)
        {
            HataSet(0);

            byte[] issue_area = GetIssuer();
            int i = 0;


            i = rd.InitCard();
            if (i == 0) { return Return(1); }

            i = rd.ReadCard(1, 4);
            if (i == 0) { return Return(5); }

            if ((rd.InBuf[0] != issue_area[0]) || (rd.InBuf[1] != issue_area[1]) || (rd.InBuf[2] != 'A') || (rd.InBuf[3] != ' '))
            {
                return Return(7);
            }

            i = rd.ReadCard(0X30, 4);
            if (i == 0) { return Return(5); }
            UInt32 CihazNo1 = Hexcon.Byte4toUInt32(rd.InBuf);

            #region csc verify oku yaz

            string csc = CscVerifyOkuYaz(CihazNo1, CihazNo);
            if (csc != "1") { return Return(Convert.ToInt32(csc)); }

            #endregion


            #region kart yazma

            Hexcon.UInt32toByte4(CihazNo, rd.OutBuf);
            i = rd.UpdateCard(0X30);
            if (i == 0) { return Return(6); }


            UInt32 maxFiyat = ElkFiyat1;

            if (ElkFiyat2 > maxFiyat) maxFiyat = ElkFiyat2;
            if (ElkFiyat3 > maxFiyat) maxFiyat = ElkFiyat3;
            if (ElkFiyat4 > maxFiyat) maxFiyat = ElkFiyat4;

            UInt16 t1kat = Convert.ToUInt16(Convert.ToDouble(65535.0 * ElkFiyat1) / Convert.ToDouble(maxFiyat));
            UInt16 t2kat = Convert.ToUInt16(Convert.ToDouble(65535.0 * ElkFiyat2) / Convert.ToDouble(maxFiyat));
            UInt16 t3kat = Convert.ToUInt16(Convert.ToDouble(65535.0 * ElkFiyat3) / Convert.ToDouble(maxFiyat));
            UInt16 t4kat = Convert.ToUInt16(Convert.ToDouble(65535.0 * ElkFiyat4) / Convert.ToDouble(maxFiyat));

            SetOutBuf(t1kat, t2kat);

            i = rd.UpdateCard(0X2C);
            if (i == 0) { return Return(6); }

            SetOutBuf(t3kat, t4kat);

            i = rd.UpdateCard(0X2F);
            if (i == 0) { return Return(6); }

            Hexcon.Int32toByte4(ElkAnaKredi, rd.OutBuf);
            i = rd.UpdateCard(0X35);
            if (i == 0) { return Return(6); }

            i = rd.ReadCard(0X1A, 4);
            if (i == 0) { return Return(5); }

            SetOutBuf(rd.InBuf[0], ElkYedekKredi, rd.InBuf[2], rd.InBuf[3]);

            i = rd.UpdateCard(0X1A);
            if (i == 0) { return Return(6); }

            SetOutBuf(0, 0, 0, 0);

            i = rd.UpdateCard(0X36);
            if (i == 0) { return Return(6); }

            i = rd.UpdateCard(0X37);
            if (i == 0) { return Return(6); }

            i = rd.UpdateCard(0X28);
            if (i == 0) { return Return(6); }

            i = rd.UpdateCard(0X2A);
            if (i == 0) { return Return(6); }

            i = rd.UpdateCard(0X2B);
            if (i == 0) { return Return(6); }

            i = rd.UpdateCard(0X31);
            if (i == 0) { return Return(6); }

            i = rd.UpdateCard(0X29);
            if (i == 0) { return Return(6); }

            i = rd.UpdateCard(0X1E);
            if (i == 0) { return Return(6); }


            i = rd.ReadCard(0X3F, 4);
            if (i == 0) { return Return(5); }

            byte IslemNo = rd.InBuf[3];

            if ((rd.InBuf[0] & 0X80) == 0) IslemNo += 1;
            if ((rd.InBuf[0] & 0X40) == 0) IslemNo += 1;

            Hexcon.Islem isl = new Hexcon.Islem(rd.InBuf[2]);

            Hexcon.Islem islYeni = new Hexcon.Islem(isl.KartNo, 3, 0, 0);

            //byte Major = rd.InBuf[2];
            //Major &= 0xF2;
            //Major |= 0x0C;       // major 3 yapılıp hata sıfır yapılmış...

            rd.OutBuf[0] = 0xC1;
            rd.OutBuf[1] = 0xFF;
            rd.OutBuf[2] = islYeni.ToByte();
            rd.OutBuf[3] = IslemNo;

            i = rd.UpdateCard(0X3F);
            if (i == 0) { return Return(6); }


            //i = rd.ReadCard(0X3F);
            //if (i == 0) return "0";

            //Hexcon.ByteToBit learnKart = new Hexcon.ByteToBit(rd.InBuf[2]);
            //UInt32 islem = (UInt32)rd.InBuf[3];

            //if (((int)rd.InBuf[0] & (0X80)) == 0) islem++;

            //if (learnKart.Bit7 != 1) // yeni kart değilse artır
            //{
            //    if (((int)rd.InBuf[0] & (0X40)) == 0) islem++;
            //}
            //learnKart.Deger = Hexcon.SetBitZero(learnKart.Deger); // hata set
            //learnKart.Deger = Hexcon.SetBitBir(learnKart.Deger, 0X30); // major set

            #endregion

            rd.FinishCard();

            return "1";
        }

        public string AboneYazSu(UInt32 CihazNo,
                                 Int32 SuAnaKredi, byte SuYedekKredi,
                                 UInt16 SuLimit1, UInt16 SuLimit2,
                                 UInt32 SuFiyat1, UInt32 SuFiyat2, UInt32 SuFiyat3)
        {
            HataSet(0);

            #region init

            byte[] issue_area = GetIssuer();
            int i = 0;

            i = rd.InitCard();
            if (i == 0) { return Return(1); }

            i = rd.ReadCard(1, 4);
            if (i == 0) { return Return(5); }

            if ((rd.InBuf[0] != issue_area[0]) || (rd.InBuf[1] != issue_area[1]) || (rd.InBuf[2] != 'A') || (rd.InBuf[3] != ' '))
            {
                return Return(7);
            }

            #endregion

            i = rd.ReadCard(0X30, 4);
            if (i == 0) { return Return(5); }
            UInt32 CihazNo1 = Hexcon.Byte4toUInt32(rd.InBuf);

            #region csc verify oku yaz

            string csc = CscVerifyOkuYaz(CihazNo1, CihazNo);
            if (csc != "1") { return Return(Convert.ToInt32(csc)); }

            #endregion

            #region kart yazma

            Hexcon.UInt32toByte4(CihazNo, rd.OutBuf);
            i = rd.UpdateCard(0X30);
            if (i == 0) { return Return(6); }

            Hexcon.Int32toByte4(SuAnaKredi, rd.OutBuf);
            i = rd.UpdateCard(0X13);
            if (i == 0) { return Return(6); }

            i = rd.ReadCard(0X1A, 4);
            if (i == 0) { return Return(5); }

            SetOutBuf(SuYedekKredi, rd.InBuf[1], rd.InBuf[2], rd.InBuf[3]);

            i = rd.UpdateCard(0X1A);
            if (i == 0) { return Return(6); }


            SetOutBuf(SuLimit1, SuLimit2);

            i = rd.UpdateCard(0X17);
            if (i == 0) { return Return(6); }

            UInt16 s1kat1 = Convert.ToUInt16(Convert.ToDouble(65535.0 * SuFiyat1) / Convert.ToDouble(SuFiyat3));
            UInt16 s1kat2 = Convert.ToUInt16(Convert.ToDouble(65535.0 * SuFiyat2) / Convert.ToDouble(SuFiyat3));

            SetOutBuf(s1kat1, s1kat2);

            i = rd.UpdateCard(0X14);
            if (i == 0) { return Return(6); }


            SetOutBuf(0, 0, 0, 0);

            i = rd.UpdateCard(0X15);
            if (i == 0) { return Return(6); }

            i = rd.UpdateCard(0X16);
            if (i == 0) { return Return(6); }

            i = rd.UpdateCard(0X2D);
            if (i == 0) { return Return(6); }


            i = rd.ReadCard(0X3D, 4);
            if (i == 0) { return Return(5); }

            byte IslemNo = rd.InBuf[3];

            if ((rd.InBuf[0] & 0X80) == 0) IslemNo += 1;
            if ((rd.InBuf[0] & 0X40) == 0) IslemNo += 1;

            Hexcon.Islem isl = new Hexcon.Islem(rd.InBuf[2]);

            Hexcon.Islem islYeni = new Hexcon.Islem(isl.KartNo, 3, 0, 0);

            //byte Major = rd.InBuf[2];
            //Major &= 0xF2;
            //Major |= 0x0C;       // major 3 yapılıp hata sıfır yapılmış...

            rd.OutBuf[0] = 0xD8;
            rd.OutBuf[1] = 0xFF;
            rd.OutBuf[2] = islYeni.ToByte();
            rd.OutBuf[3] = IslemNo;

            i = rd.UpdateCard(0X3D);
            if (i == 0) { return Return(6); }


            //i = rd.ReadCard(0X3F);
            //if (i == 0) return "0";

            //Hexcon.ByteToBit learnKart = new Hexcon.ByteToBit(rd.InBuf[2]);
            //UInt32 islem = (UInt32)rd.InBuf[3];

            //if (((int)rd.InBuf[0] & (0X80)) == 0) islem++;

            //if (learnKart.Bit7 != 1) // yeni kart değilse artır
            //{
            //    if (((int)rd.InBuf[0] & (0X40)) == 0) islem++;
            //}
            //learnKart.Deger = Hexcon.SetBitZero(learnKart.Deger); // hata set
            //learnKart.Deger = Hexcon.SetBitBir(learnKart.Deger, 0X30); // major set

            #endregion

            rd.FinishCard();

            return "1";
        }

        public string AboneYazSicakSu(UInt32 CihazNo,
                                 Int32 SicakSuAnaKredi, byte SicakSuYedekKredi)
        {
            HataSet(0);

            #region init

            byte[] issue_area = GetIssuer();
            int i = 0;

            i = rd.InitCard();
            if (i == 0) { return Return(1); }

            i = rd.ReadCard(1, 4);
            if (i == 0) { return Return(5); }

            if ((rd.InBuf[0] != issue_area[0]) || (rd.InBuf[1] != issue_area[1]) || (rd.InBuf[2] != 'A') || (rd.InBuf[3] != ' '))
            {
                return Return(7);
            }

            #endregion

            i = rd.ReadCard(0X30, 4);
            if (i == 0) { return Return(5); }
            UInt32 CihazNo1 = Hexcon.Byte4toUInt32(rd.InBuf);

            #region csc verify oku yaz

            string csc = CscVerifyOkuYaz(CihazNo1, CihazNo);
            if (csc != "1") { return Return(Convert.ToInt32(csc)); }

            #endregion

            #region kart yazma

            Hexcon.UInt32toByte4(CihazNo, rd.OutBuf);
            i = rd.UpdateCard(0X30);
            if (i == 0) { return Return(6); }

            Hexcon.Int32toByte4(SicakSuAnaKredi, rd.OutBuf);
            i = rd.UpdateCard(0X1F);
            if (i == 0) { return Return(6); }

            i = rd.ReadCard(0X1A, 4);
            if (i == 0) { return Return(5); }

            SetOutBuf(rd.InBuf[0], rd.InBuf[1], rd.InBuf[2], SicakSuYedekKredi);

            i = rd.UpdateCard(0X1A);
            if (i == 0) { return Return(6); }


            SetOutBuf(0, 0, 0, 0);

            i = rd.UpdateCard(0X1B);
            if (i == 0) { return Return(6); }

            i = rd.UpdateCard(0X1C);
            if (i == 0) { return Return(6); }

            i = rd.UpdateCard(0X1D);
            if (i == 0) { return Return(6); }


            i = rd.ReadCard(0X3E, 4);
            if (i == 0) { return Return(5); }

            byte IslemNo = rd.InBuf[3];

            if ((rd.InBuf[0] & 0X80) == 0) IslemNo += 1;
            if ((rd.InBuf[0] & 0X40) == 0) IslemNo += 1;

            Hexcon.Islem isl = new Hexcon.Islem(rd.InBuf[2]);

            Hexcon.Islem islYeni = new Hexcon.Islem(isl.KartNo, 3, 0, 0);

            //byte Major = rd.InBuf[2];
            //Major &= 0xF2;
            //Major |= 0x0C;       // major 3 yapılıp hata sıfır yapılmış...

            rd.OutBuf[0] = 0xD8;
            rd.OutBuf[1] = 0xFF;
            rd.OutBuf[2] = islYeni.ToByte();
            rd.OutBuf[3] = IslemNo;

            i = rd.UpdateCard(0X3E);
            if (i == 0) { return Return(6); }


            //i = rd.ReadCard(0X3F);
            //if (i == 0) return "0";

            //Hexcon.ByteToBit learnKart = new Hexcon.ByteToBit(rd.InBuf[2]);
            //UInt32 islem = (UInt32)rd.InBuf[3];

            //if (((int)rd.InBuf[0] & (0X80)) == 0) islem++;

            //if (learnKart.Bit7 != 1) // yeni kart değilse artır
            //{
            //    if (((int)rd.InBuf[0] & (0X40)) == 0) islem++;
            //}
            //learnKart.Deger = Hexcon.SetBitZero(learnKart.Deger); // hata set
            //learnKart.Deger = Hexcon.SetBitBir(learnKart.Deger, 0X30); // major set

            #endregion

            rd.FinishCard();

            return "1";
        }

        public string AboneYazGaz(UInt32 CihazNo,
                                 Int32 GazAnaKredi, byte GazYedekKredi)
        {
            HataSet(0);

            #region init

            byte[] issue_area = GetIssuer();
            int i = 0;

            i = rd.InitCard();
            if (i == 0) { return Return(1); }

            i = rd.ReadCard(1, 4);
            if (i == 0) { return Return(5); }

            if ((rd.InBuf[0] != issue_area[0]) || (rd.InBuf[1] != issue_area[1]) || (rd.InBuf[2] != 'A') || (rd.InBuf[3] != ' '))
            {
                return Return(7);
            }

            #endregion

            i = rd.ReadCard(0X30, 4);
            if (i == 0) { return Return(5); }
            UInt32 CihazNo1 = Hexcon.Byte4toUInt32(rd.InBuf);

            #region csc verify oku yaz

            string csc = CscVerifyOkuYaz(CihazNo1, CihazNo);
            if (csc != "1") { return Return(Convert.ToInt32(csc)); }

            #endregion

            #region kart yazma

            Hexcon.UInt32toByte4(CihazNo, rd.OutBuf);
            i = rd.UpdateCard(0X30);
            if (i == 0) { return Return(6); }

            Hexcon.Int32toByte4(GazAnaKredi, rd.OutBuf);
            i = rd.UpdateCard(0X10);
            if (i == 0) { return Return(6); }

            i = rd.ReadCard(0X1A, 4);
            if (i == 0) { return Return(5); }

            SetOutBuf(rd.InBuf[0], rd.InBuf[1], GazYedekKredi, rd.InBuf[3]);

            i = rd.UpdateCard(0X1A);
            if (i == 0) { return Return(6); }


            SetOutBuf(0, 0, 0, 0);

            i = rd.UpdateCard(0X11);
            if (i == 0) { return Return(6); }

            i = rd.UpdateCard(0X12);
            if (i == 0) { return Return(6); }


            i = rd.ReadCard(0X3C, 4);
            if (i == 0) { return Return(5); }

            byte IslemNo = rd.InBuf[3];

            if ((rd.InBuf[0] & 0X80) == 0) IslemNo += 1;
            if ((rd.InBuf[0] & 0X40) == 0) IslemNo += 1;

            Hexcon.Islem isl = new Hexcon.Islem(rd.InBuf[2]);

            Hexcon.Islem islYeni = new Hexcon.Islem(isl.KartNo, 3, 0, 0);

            //byte Major = rd.InBuf[2];
            //Major &= 0xF2;
            //Major |= 0x0C;       // major 3 yapılıp hata sıfır yapılmış...

            rd.OutBuf[0] = 0xC0;
            rd.OutBuf[1] = 0xFF;
            rd.OutBuf[2] = islYeni.ToByte();
            rd.OutBuf[3] = IslemNo;

            i = rd.UpdateCard(0X3C);
            if (i == 0) { return Return(6); }


            //i = rd.ReadCard(0X3F);
            //if (i == 0) return "0";

            //Hexcon.ByteToBit learnKart = new Hexcon.ByteToBit(rd.InBuf[2]);
            //UInt32 islem = (UInt32)rd.InBuf[3];

            //if (((int)rd.InBuf[0] & (0X80)) == 0) islem++;

            //if (learnKart.Bit7 != 1) // yeni kart değilse artır
            //{
            //    if (((int)rd.InBuf[0] & (0X40)) == 0) islem++;
            //}
            //learnKart.Deger = Hexcon.SetBitZero(learnKart.Deger); // hata set
            //learnKart.Deger = Hexcon.SetBitBir(learnKart.Deger, 0X30); // major set

            #endregion

            rd.FinishCard();

            return "1";
        }

        public string KrediOku()
        {
            string str = "";
            byte[] issue_area = GetIssuer();
            int i = 0;

            #region Init

            i = rd.InitCard();
            //util.Sleep(10);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(1); }
            i = rd.ReadCard(1, 4);
            //util.Sleep(10);
            if (i == 5) { return Return(5); }
            else if (i == 0) { return Return(5); }

            if ((rd.InBuf[0] != issue_area[0]) || (rd.InBuf[1] != issue_area[1]) || (rd.InBuf[2] != 'A') || (rd.InBuf[3] != ' '))
            {
                return Return(7);
            }

            #endregion

            i = rd.ReadCard(0X30, 4);
            if (i == 0) { return Return(5); }

            UInt32 CihazNo = Hexcon.Byte4toUInt32(rd.InBuf);

            #region csc verify

            string csc = CscVerify(CihazNo);
            if (csc != "1") { return Return(Convert.ToInt32(csc)); }

            #endregion

            #region Kart Bilgileri
            i = rd.ReadCard(0X35, 4);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            UInt32 elkAna = (UInt32)Hexcon.ByteToDecimal(rd.InBuf, 4);
            str += "_" + elkAna;


            i = rd.ReadCard(0X3F, 4);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            string ako = "";
            string yko = "";

            if (((int)rd.InBuf[0] & (int)0X80) == 0) ako = "*"; else ako = "b";
            if (((int)rd.InBuf[0] & (int)0X40) == 0) yko = "*"; else yko = "b";

            str += "_" + ako + "_" + yko;

            string yeniKart = "";
            if (((int)rd.InBuf[2] & (int)0X40) == 0) yeniKart = "0"; else yeniKart = "1";
            str += "_" + yeniKart;

            i = rd.ReadCard(0X17, 4);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            UInt32 fixCharge = (UInt32)Hexcon.ByteToDecimal(rd.InBuf, 4);
            str += "_" + fixCharge;

            i = rd.ReadCard(0X18, 4);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            UInt32 totalFixCharge = (UInt32)Hexcon.ByteToDecimal(rd.InBuf, 4);
            str += "_" + totalFixCharge;

            #endregion


            return "1_" + str;

        }

        public string Bosalt()
        {
            HataSet(0);

            byte[] issue_area = GetIssuer();
            int i = 0;

            #region Init

            i = rd.InitCard();
            if (i == 0) { return Return(1); }

            i = rd.ReadCard(1, 4);
            if (i == 0) { return Return(5); }

            #endregion

            #region kart boşaltma işlemleri

            if ((rd.InBuf[0] == issue_area[0]) && (rd.InBuf[1] == issue_area[1]) && (rd.InBuf[2] == 'A'))
            {

                i = rd.ReadCard(0X30, 4);
                if (i == 0) { return Return(5); }

                UInt32 CihazNo = Hexcon.Byte4toUInt32(rd.InBuf);

                #region csc verify

                string csc = CscVerify(CihazNo);
                if (csc != "1") { return Return(Convert.ToInt32(csc)); }

                #endregion
            }
            else if ((rd.InBuf[0] == issue_area[0]) && (rd.InBuf[1] == issue_area[1]) && (rd.InBuf[2] == 'Y'))
            {
                rd.OutBuf[0] = 0X3A;
                rd.OutBuf[1] = 0XDF;
                rd.OutBuf[2] = 0X1D;
                rd.OutBuf[3] = 0X80;

                i = rd.VerifyCard(7);
                if (i == 0) { return Return(2); }

            }
            else if ((rd.InBuf[0] == 'A') && (rd.InBuf[1] == 'L') && (rd.InBuf[2] == 'U'))
            {
                rd.OutBuf[0] = 0X7B;
                rd.OutBuf[1] = 0X8A;
                rd.OutBuf[2] = 0X13;
                rd.OutBuf[3] = 0XEC;

                i = rd.VerifyCard(7);
                if (i == 0) { return Return(2); }

            }
            else if ((rd.InBuf[0] == 'K') && (rd.InBuf[1] == 'I') && (rd.InBuf[2] == 'U'))
            {
                rd.OutBuf[0] = 0X7B;
                rd.OutBuf[1] = 0X8A;
                rd.OutBuf[2] = 0X13;
                rd.OutBuf[3] = 0XEC;

                i = rd.VerifyCard(7);
                if (i == 0) { return Return(2); }

            }
            else if ((rd.InBuf[0] == 'B') && (rd.InBuf[1] == 'K') && (rd.InBuf[2] == 'U'))
            {
                rd.OutBuf[0] = 0XA5;
                rd.OutBuf[1] = 0X5A;
                rd.OutBuf[2] = 0X78;
                rd.OutBuf[3] = 0X84;

                i = rd.VerifyCard(7);
                if (i == 0) { return Return(2); }

            }
            else
            {
                return Return(7);
            }

            rd.OutBuf[0] = 0;
            rd.OutBuf[1] = 0;
            rd.OutBuf[2] = 0;
            rd.OutBuf[3] = 0;

            i = rd.UpdateCard(0X01);
            if (i == 0) { return Return(6); }

            i = rd.UpdateCard(0X02);
            if (i == 0) { return Return(6); }

            i = rd.UpdateCard(0X03);
            if (i == 0) { return Return(6); }

            i = rd.UpdateCard(0X3C);
            if (i == 0) { return Return(6); }

            i = rd.UpdateCard(0X3D);
            if (i == 0) { return Return(6); }

            i = rd.UpdateCard(0X3F);
            if (i == 0) { return Return(6); }



            for (int j = 0x10; j < 0x20; j++)
            {
                i = rd.UpdateCard((byte)j);
                if (i == 0) { return Return(6); }
            }

            for (int j = 0x28; j < 0x40; j++)
            {
                if (j == 0x38) j = 0x3C;
                i = rd.UpdateCard((byte)j);
                if (i == 0) { return Return(6); }
            }


            rd.OutBuf[0] = 0xAA;
            rd.OutBuf[1] = 0xAA;
            rd.OutBuf[2] = 0xAA;
            rd.OutBuf[3] = 0xAA;
            i = rd.UpdateCard(6);
            if (i == 0) { return Return(6); }

            rd.OutBuf[0] = 0x11;
            rd.OutBuf[1] = 0x11;
            rd.OutBuf[2] = 0x11;
            rd.OutBuf[3] = 0x11;
            i = rd.UpdateCard(0X38);
            if (i == 0) { return Return(6); }

            rd.OutBuf[0] = 0x22;
            rd.OutBuf[1] = 0x22;
            rd.OutBuf[2] = 0x22;
            rd.OutBuf[3] = 0x22;
            i = rd.UpdateCard(0X3A);
            if (i == 0) { return Return(6); }

            #endregion

            rd.FinishCard();

            return "1";

        }

        public string Eject()
        {
            HataSet(0);

            int i = 0;

            i = rd.InitCard();
            if (i == 0) { return Return(1); }

            string str = rd.Eject().ToString();

            rd.FinishCard();

            return str;
        }

        #endregion

        #region Yetki Kartı Fonksyonları
        public string FormYet(UInt32 CihazNo)
        {
            HataSet(0);

            byte[] issue_area = GetIssuer();
            int i = 0;

            #region Init


            i = rd.InitCard();
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(1); }

            rd.OutBuf[0] = 0xAA;
            rd.OutBuf[1] = 0xAA;
            rd.OutBuf[2] = 0xAA;
            rd.OutBuf[3] = 0xAA;

            i = rd.VerifyCard(7);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(9); }

            rd.OutBuf[0] = issue_area[0];
            rd.OutBuf[1] = issue_area[1];
            rd.OutBuf[2] = (byte)'Y';
            rd.OutBuf[3] = (byte)'A';

            i = rd.UpdateCard(1);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }

            rd.OutBuf[0] = 0x3A;
            rd.OutBuf[1] = 0xDF;
            rd.OutBuf[2] = 0x1D;
            rd.OutBuf[3] = 0x80;

            i = rd.UpdateCard(6);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }

            Hexcon.UInt32toByte4(CihazNo, rd.OutBuf);
            i = rd.UpdateCard(0X02);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }

            #endregion

            rd.FinishCard();

            return "1";
        }

        public string YetkiSaat()
        {
            HataSet(0);

            byte[] issue_area = GetIssuer();
            int i = 0;

            #region Init

            i = rd.InitCard();
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(1); }

            i = rd.ReadCard(1, 4);
            if (i == 5) { return Return(5); }
            else if (i == 0) { return Return(5); }

            if ((rd.InBuf[0] != issue_area[0]) || (rd.InBuf[1] != issue_area[1]) || (rd.InBuf[2] != 'Y'))
            {
                return Return(8);
            }

            #endregion

            #region Saat kartı işlemleri

            rd.OutBuf[0] = 0x3A;
            rd.OutBuf[1] = 0xDF;
            rd.OutBuf[2] = 0x1D;
            rd.OutBuf[3] = 0x80;

            i = rd.VerifyCard(7);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(9); }

            rd.OutBuf[0] = issue_area[0];
            rd.OutBuf[1] = issue_area[1];
            rd.OutBuf[2] = (byte)'Y';
            rd.OutBuf[3] = (byte)'S';

            i = rd.UpdateCard(1);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }

            DateTime tar = DateTime.Now;

            TarihAl tarih = new TarihAl(tar);
            rd.OutBuf = tarih.TarihDondur4Byte(tar);

            i = rd.UpdateCard(0x10);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }

            rd.OutBuf[0] = 0;
            rd.OutBuf[1] = 0;
            rd.OutBuf[2] = 0;
            rd.OutBuf[3] = 0;

            i = rd.UpdateCard(0X12);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }

            i = rd.ReadCard(0X3D, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }

            DayOfWeek dow = tar.DayOfWeek;

            rd.OutBuf[0] = (byte)dow;
            rd.OutBuf[1] = rd.InBuf[1];
            rd.OutBuf[2] = 0;
            rd.OutBuf[3] = rd.InBuf[3];

            i = rd.UpdateCard(0x3D);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }

            #endregion

            rd.FinishCard();

            return "1";
        }

        public string YetkiSaat(DateTime date)
        {
            HataSet(0);

            byte[] issue_area = GetIssuer();
            int i = 0;


            #region Init

            i = rd.InitCard();
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(1); }

            i = rd.ReadCard(1, 4);
            if (i == 5) { return Return(5); }
            else if (i == 0) { return Return(5); }

            if ((rd.InBuf[0] != issue_area[0]) || (rd.InBuf[1] != issue_area[1]) || (rd.InBuf[2] != 'Y'))
            {
                return Return(8);
            }

            #endregion

            #region Saat kartı işlemleri

            rd.OutBuf[0] = 0x3A;
            rd.OutBuf[1] = 0xDF;
            rd.OutBuf[2] = 0x1D;
            rd.OutBuf[3] = 0x80;

            i = rd.VerifyCard(7);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(9); }

            rd.OutBuf[0] = issue_area[0];
            rd.OutBuf[1] = issue_area[1];
            rd.OutBuf[2] = (byte)'Y';
            rd.OutBuf[3] = (byte)'S';

            i = rd.UpdateCard(1);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }

            DateTime tar = date;

            TarihAl tarih = new TarihAl(tar);
            rd.OutBuf = tarih.TarihDondur4Byte(tar);

            i = rd.UpdateCard(0x10);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }

            rd.OutBuf[0] = 0;
            rd.OutBuf[1] = 0;
            rd.OutBuf[2] = 0;
            rd.OutBuf[3] = 0;

            i = rd.UpdateCard(0X12);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }

            i = rd.ReadCard(0X3D, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            DayOfWeek dow = tar.DayOfWeek;

            rd.OutBuf[0] = (byte)dow;
            rd.OutBuf[1] = rd.InBuf[1];
            rd.OutBuf[2] = 0;
            rd.OutBuf[3] = rd.InBuf[3];

            i = rd.UpdateCard(0x3D);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }

            #endregion

            rd.FinishCard();

            return "1";
        }

        public string YetkiAc(UInt32 CihazNo)
        {
            HataSet(0);

            byte[] issue_area = GetIssuer();
            int i = 0;


            #region Init

            i = rd.InitCard();
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(1); }

            i = rd.ReadCard(1, 4);
            if (i == 5) { return Return(5); }
            else if (i == 0) { return Return(5); }

            if ((rd.InBuf[0] != issue_area[0]) || (rd.InBuf[1] != issue_area[1]) || (rd.InBuf[2] != 'Y'))
            {
                return Return(8);
            }

            #endregion

            #region Açma işlemleri

            rd.OutBuf[0] = 0x3A;
            rd.OutBuf[1] = 0xDF;
            rd.OutBuf[2] = 0x1D;
            rd.OutBuf[3] = 0x80;

            i = rd.VerifyCard(7);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(9); }

            rd.OutBuf[0] = issue_area[0];
            rd.OutBuf[1] = issue_area[1];
            rd.OutBuf[2] = (byte)'Y';
            rd.OutBuf[3] = (byte)'A';

            i = rd.UpdateCard(1);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }

            Hexcon.UInt32toByte4(CihazNo, rd.OutBuf);
            i = rd.UpdateCard(0X3C);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }


            rd.OutBuf[0] = 0X0;
            rd.OutBuf[1] = 0X0;
            rd.OutBuf[2] = 0X0;
            rd.OutBuf[3] = 0X0;

            i = rd.UpdateCard(0x10);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }

            #endregion

            rd.FinishCard();

            return "1";


        }

        public string YetkiKapat(UInt32 CihazNo)
        {
            HataSet(0);

            byte[] issue_area = GetIssuer();
            int i = 0;


            #region Init

            i = rd.InitCard();
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(1); }

            i = rd.ReadCard(1, 4);
            if (i == 5) { return Return(5); }
            else if (i == 0) { return Return(5); }

            if ((rd.InBuf[0] != issue_area[0]) || (rd.InBuf[1] != issue_area[1]) || (rd.InBuf[2] != 'Y'))
            {
                return Return(8);
            }

            #endregion

            #region Kapatma işlemleri

            rd.OutBuf[0] = 0x3A;
            rd.OutBuf[1] = 0xDF;
            rd.OutBuf[2] = 0x1D;
            rd.OutBuf[3] = 0x80;

            i = rd.VerifyCard(7);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(9); }

            rd.OutBuf[0] = issue_area[0];
            rd.OutBuf[1] = issue_area[1];
            rd.OutBuf[2] = (byte)'Y';
            rd.OutBuf[3] = (byte)'K';

            i = rd.UpdateCard(1);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }

            Hexcon.UInt32toByte4(CihazNo, rd.OutBuf);
            i = rd.UpdateCard(0X3C);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }


            rd.OutBuf[0] = 0X0;
            rd.OutBuf[1] = 0X0;
            rd.OutBuf[2] = 0X0;
            rd.OutBuf[3] = 0X0;


            i = rd.UpdateCard(0x10);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }

            #endregion

            rd.FinishCard();

            return "1";

        }

        public string YetkiBilgiYap()
        {
            HataSet(0);

            byte[] issue_area = GetIssuer();
            int i = 0;


            #region Init

            i = rd.InitCard();
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(1); }

            i = rd.ReadCard(1, 4);
            if (i == 5) { return Return(5); }
            else if (i == 0) { return Return(5); }

            if ((rd.InBuf[0] != issue_area[0]) || (rd.InBuf[1] != issue_area[1]) || (rd.InBuf[2] != 'Y'))
            {
                return Return(8);
            }

            #endregion

            #region Bilgi yap işlemleri

            rd.OutBuf[0] = 0x3A;
            rd.OutBuf[1] = 0xDF;
            rd.OutBuf[2] = 0x1D;
            rd.OutBuf[3] = 0x80;

            i = rd.VerifyCard(7);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(9); }



            rd.OutBuf[0] = issue_area[0];
            rd.OutBuf[1] = issue_area[1];
            rd.OutBuf[2] = (byte)'Y';
            rd.OutBuf[3] = (byte)'E';

            i = rd.UpdateCard(1);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }

            rd.OutBuf[0] = 0;
            rd.OutBuf[1] = 0;
            rd.OutBuf[2] = 0;
            rd.OutBuf[3] = 0;

            for (int j = 0x10; j < 0x20; j++)
            {
                i = rd.UpdateCard((byte)j);
                if (i == 0) { return Return(6); }
            }

            for (int j = 0x28; j < 0x40; j++)
            {
                if (j == 0x38) j = 0x3C;
                i = rd.UpdateCard((byte)j);
                if (i == 0) { return Return(6); }
            }


            #endregion

            rd.FinishCard();

            return "1";

        }

        public string YetkiBilgiOkuTip()
        {
            HataSet(0);

            byte[] issue_area = GetIssuer();
            int i = 0;


            #region Init

            i = rd.InitCard();
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(1); }

            i = rd.ReadCard(1, 4);
            if (i == 5) { return Return(5); }
            else if (i == 0) { return Return(5); }

            if ((rd.InBuf[0] != issue_area[0]) || (rd.InBuf[1] != issue_area[1]) || (rd.InBuf[2] != 'Y') || (rd.InBuf[3] != 'E'))
            {
                return Return(10);
            }

            #endregion

            #region bilgi okuma işlemleri

            rd.OutBuf[0] = 0x3A;
            rd.OutBuf[1] = 0xDF;
            rd.OutBuf[2] = 0x1D;
            rd.OutBuf[3] = 0x80;

            i = rd.VerifyCard(7);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(9); }

            i = rd.ReadCard(0X3C, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            if (rd.InBuf[0] != 0X05)
            {
                return Return(11);
            }


            i = rd.ReadCard(0X02, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            byte tip = rd.InBuf[0];

            #endregion

            rd.FinishCard();


            return "1" +
                    sp + tip;

        }

        public string YetkiBilgiOkuGaz()
        {
            TarihAl bilgiceza, bilgipulse, bilgiarzt, bilgikr, bilgikapat, sayacTarih,
                    vanaAcTarih, vanaKapTarih, ceza1Tarih, ceza2Tarih;

            string str = "";
            byte[] issue_area = GetIssuer();
            int i = 0;


            i = rd.InitCard();
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(1); }

            i = rd.ReadCard(1, 4);
            if (i == 5) { return Return(5); }
            else if (i == 0) { return Return(5); }

            if ((rd.InBuf[0] != issue_area[0]) || (rd.InBuf[1] != issue_area[1]) || (rd.InBuf[2] != 'Y') || (rd.InBuf[3] != 'E'))
            {
                return Return(10);
            }

            rd.OutBuf[0] = 0x3A;
            rd.OutBuf[1] = 0xDF;
            rd.OutBuf[2] = 0x1D;
            rd.OutBuf[3] = 0x80;

            i = rd.VerifyCard(7);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(9); }

            i = rd.ReadCard(0X3C, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            if (rd.InBuf[0] != 0x05)
            {
                return Return(11);
            }

            i = rd.ReadCard(0X02, 4);
            if (i == 0) { return Return(5); }

            if (rd.InBuf[0] != 1)
            {
                return Return(13);
            }

            byte[] buffer = new byte[4];
            byte[] temp = new byte[2];
            byte[] temp4 = new byte[4];

            #region Kart Bilgileri


            i = rd.ReadCard(0X35, 4);
            if (i == 0) { return Return(5); }

            UInt32 ekim = Hexcon.Byte4toUInt32(rd.InBuf);

            i = rd.ReadCard(0X36, 4);
            if (i == 0) { return Return(5); }

            UInt32 temmuz = Hexcon.Byte4toUInt32(rd.InBuf);

            i = rd.ReadCard(0X37, 4);
            if (i == 0) { return Return(5); }

            UInt32 ocak = Hexcon.Byte4toUInt32(rd.InBuf);

            i = rd.ReadCard(0X10, 4);
            if (i == 0) { return Return(5); }

            Int32 kalan = Hexcon.Byte4toInt32(rd.InBuf);

            i = rd.ReadCard(0X11, 4);
            if (i == 0) { return Return(5); }

            UInt32 harcanan = Hexcon.Byte4toUInt32(rd.InBuf);


            i = rd.ReadCard(0X12, 4);
            if (i == 0) { return Return(5); }

            Array.Copy(rd.InBuf, 0, temp4, 0, 4);
            sayacTarih = new TarihAl(temp4);

            i = rd.ReadCard(0X13, 4);
            if (i == 0) { return Return(5); }

            UInt32 cihazNo = Hexcon.Byte4toUInt32(rd.InBuf);



            i = rd.ReadCard(0X14, 4);
            if (i == 0) { return Return(5); }

            Array.Copy(buffer, 0, temp, 0, 2);
            bilgikr = new TarihAl(temp);

            buffer[0] = rd.InBuf[2];
            buffer[1] = rd.InBuf[3];

            Array.Copy(buffer, 0, temp, 0, 2);
            bilgipulse = new TarihAl(temp);

            i = rd.ReadCard(0X15, 4);
            if (i == 0) { return Return(5); }

            Array.Copy(rd.InBuf, 0, temp, 0, 2);
            bilgiceza = new TarihAl(temp);

            buffer[0] = rd.InBuf[2];
            buffer[1] = rd.InBuf[3];

            Array.Copy(rd.InBuf, 0, temp, 0, 2);
            bilgiarzt = new TarihAl(temp);

            i = rd.ReadCard(0X17, 4);
            if (i == 0) { return Return(5); }

            byte vop = rd.InBuf[2];

            string gazCeza2 = "";
            string gazAriza = "";
            string gazIptal = "";
            string gazPilZayif = "";
            string gazPilBitik = "";
            string gazPilKapak = "";
            string gazCeza1 = "";

            Hexcon.ByteToBit gazState = new Hexcon.ByteToBit(rd.InBuf[3]);
            gazCeza2 = (gazState.Bit1 == 1) ? "b" : "*";
            gazAriza = (gazState.Bit2 == 1) ? "b" : "*";
            gazIptal = (gazState.Bit3 == 1) ? "b" : "*";
            gazPilZayif = (gazState.Bit4 == 1) ? "b" : "*";
            gazPilBitik = (gazState.Bit5 == 1) ? "b" : "*";
            gazPilKapak = (gazState.Bit6 == 1) ? "b" : "*";
            gazCeza1 = (gazState.Bit8 == 1) ? "b" : "*";

            i = rd.ReadCard(0X18, 4);
            if (i == 0) { return Return(5); }

            byte pilsevByte = rd.InBuf[3];
            double pilsevDouble = Convert.ToDouble(pilsevByte) / 51;
            //pilsev1 = pilsev1 / 51;
            //pilsev1.ToString("F03");

            UInt16 kritik = Hexcon.Byte2toUInt16(rd.InBuf[2], rd.InBuf[3]);

            i = rd.ReadCard(0X19, 4);
            if (i == 0) { return Return(5); }

            byte kartNo = rd.InBuf[0];
            byte islemNo = rd.InBuf[1];
            byte resetSayi = rd.InBuf[2];
            byte e2Hata = rd.InBuf[3];

            i = rd.ReadCard(0X1A, 4);
            if (i == 0) { return Return(5); }

            byte format = rd.InBuf[0];
            byte testMode = rd.InBuf[1];
            byte testReel = rd.InBuf[2];

            i = rd.ReadCard(0X1C, 4);
            if (i == 0) { return Return(5); }

            byte e2_2hata = rd.InBuf[2];
            byte pilsev3 = rd.InBuf[3];
            UInt16 cp = Hexcon.Byte2toUInt16(rd.InBuf[0], rd.InBuf[1]);

            byte pilsev3Byte = pilsev3;
            double pilsev3Double = Convert.ToDouble(pilsev3Byte) / 42.5;

            i = rd.ReadCard(0X1D, 4);
            if (i == 0) { return Return(5); }

            Array.Copy(buffer, 0, temp, 0, 2);
            vanaAcTarih = new TarihAl(temp);

            buffer[0] = rd.InBuf[2];
            buffer[1] = rd.InBuf[3];

            Array.Copy(buffer, 0, temp, 0, 2);
            vanaKapTarih = new TarihAl(temp);

            i = rd.ReadCard(0X1F, 4);
            if (i == 0) { return Return(5); }

            Array.Copy(rd.InBuf, 0, temp, 0, 2);
            ceza1Tarih = new TarihAl(temp);

            buffer[0] = rd.InBuf[2];
            buffer[1] = rd.InBuf[3];

            Array.Copy(rd.InBuf, 0, temp, 0, 2);
            ceza2Tarih = new TarihAl(temp);


            i = rd.ReadCard(0X33, 4);
            if (i == 0) { return Return(5); }

            byte versiyon = rd.InBuf[0];
            byte hangiGun = rd.InBuf[1];
            byte arizaTip = rd.InBuf[1];

            Hexcon.ByteToBit arz = new Hexcon.ByteToBit(arizaTip);

            string vanaDurumu = arz.Bit4.ToString();

            #endregion

            rd.FinishCard();


            str += "1";
            str += sp + cihazNo;
            str += sp + kalan;
            str += sp + harcanan;
            str += sp + kritik;
            str += sp + cp;
            str += sp + sayacTarih.gun.ToString().PadLeft(2, '0') + "." + sayacTarih.ay.ToString().PadLeft(2, '0') + ".20" + sayacTarih.yil.ToString().PadLeft(2, '0') + " " + sayacTarih.saat.ToString().PadLeft(2, '0') + ":" + sayacTarih.dakika.ToString().PadLeft(2, '0');
            str += sp + kartNo;
            str += sp + islemNo;
            str += sp + vop;
            str += sp + resetSayi;
            str += sp + pilsevDouble.ToString("F03");
            str += sp + bilgikr.gun.ToString().PadLeft(2, '0') + "." + bilgikr.ay.ToString().PadLeft(2, '0') + ".20" + bilgikr.yil.ToString().PadLeft(2, '0');
            str += sp + bilgipulse.gun.ToString().PadLeft(2, '0') + "." + bilgipulse.ay.ToString().PadLeft(2, '0') + ".20" + bilgipulse.yil.ToString().PadLeft(2, '0');
            str += sp + bilgiceza.gun.ToString().PadLeft(2, '0') + "." + bilgiceza.ay.ToString().PadLeft(2, '0') + ".20" + bilgiceza.yil.ToString().PadLeft(2, '0');
            str += sp + bilgiarzt.gun.ToString().PadLeft(2, '0') + "." + bilgiarzt.ay.ToString().PadLeft(2, '0') + ".20" + bilgiarzt.yil.ToString().PadLeft(2, '0');
            str += sp + format;
            str += sp + testReel;
            str += sp + vanaAcTarih.gun.ToString().PadLeft(2, '0') + "." + vanaAcTarih.ay.ToString().PadLeft(2, '0') + ".20" + vanaAcTarih.yil.ToString().PadLeft(2, '0');
            str += sp + vanaKapTarih.gun.ToString().PadLeft(2, '0') + "." + vanaKapTarih.ay.ToString().PadLeft(2, '0') + ".20" + vanaKapTarih.yil.ToString().PadLeft(2, '0');
            str += sp + ceza1Tarih.gun.ToString().PadLeft(2, '0') + "." + ceza1Tarih.ay.ToString().PadLeft(2, '0') + ".20" + ceza1Tarih.yil.ToString().PadLeft(2, '0');
            str += sp + ceza2Tarih.gun.ToString().PadLeft(2, '0') + "." + ceza2Tarih.ay.ToString().PadLeft(2, '0') + ".20" + ceza2Tarih.yil.ToString().PadLeft(2, '0');
            str += sp + ocak;
            str += sp + temmuz;
            str += sp + ekim;
            str += sp + vanaDurumu;
            str += sp + gazCeza2;
            str += sp + gazAriza;
            str += sp + gazIptal;
            str += sp + gazPilZayif;
            str += sp + gazPilBitik;
            str += sp + gazPilKapak;
            str += sp + gazCeza1;

            return str;
        }

        public string YetkiBilgiOkuElk()
        {

            TarihAl bilgiceza, bilgipulse, bilgiarzt, bilgikr, bilgikapat, sayacTarih;

            string str = "";
            byte[] issue_area = GetIssuer();
            int i = 0;

            #region Init

            i = rd.InitCard();
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(1); }

            i = rd.ReadCard(1, 4);
            if (i == 5) { return Return(5); }
            else if (i == 0) { return Return(5); }

            if ((rd.InBuf[0] != issue_area[0]) || (rd.InBuf[1] != issue_area[1]) || (rd.InBuf[2] != 'Y') || (rd.InBuf[3] != 'E'))
            {
                return Return(10);
            }

            #endregion

            #region Csc

            rd.OutBuf[0] = 0x3A;
            rd.OutBuf[1] = 0xDF;
            rd.OutBuf[2] = 0x1D;
            rd.OutBuf[3] = 0x80;

            i = rd.VerifyCard(7);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(9); }


            #endregion

            byte[] buffer = new byte[4];
            byte[] temp = new byte[2];
            byte[] temp4 = new byte[4];

            #region Kart Bilgileri

            i = rd.ReadCard(0X3C, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            if (rd.InBuf[0] != 0x05)
            {
                return Return(11);
            }

            UInt16 kritik = Hexcon.Byte2toUInt16(rd.InBuf[2], rd.InBuf[3]);

            i = rd.ReadCard(0X02, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            if (rd.InBuf[0] != 4)
            {
                return Return(14);
            }

            i = rd.ReadCard(0X35, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            UInt32 temmuz = Hexcon.Byte4toUInt32(rd.InBuf);

            i = rd.ReadCard(0X28, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            UInt32 ocak = Hexcon.Byte4toUInt32(rd.InBuf);

            i = rd.ReadCard(0X36, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            UInt32 ekim = Hexcon.Byte4toUInt32(rd.InBuf);



            i = rd.ReadCard(0X10, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            Array.Copy(rd.InBuf, 0, temp, 0, 2);
            bilgiarzt = new TarihAl(temp);

            buffer[0] = rd.InBuf[2];
            buffer[1] = rd.InBuf[3];

            Array.Copy(buffer, 0, temp, 0, 2);
            bilgipulse = new TarihAl(temp);

            i = rd.ReadCard(0X11, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            Array.Copy(rd.InBuf, 0, temp, 0, 2);
            bilgiceza = new TarihAl(temp);

            buffer[0] = rd.InBuf[2];
            buffer[1] = rd.InBuf[3];

            Array.Copy(buffer, 0, temp, 0, 2);
            bilgikr = new TarihAl(temp);

            // tarifeler

            byte[] hia = new byte[8];///saat
            byte[] hib = new byte[8];///dakika
            byte[] cta = new byte[8];///saat
            byte[] ctb = new byte[8];///dakika
            byte[] pza = new byte[8];///saat
            byte[] pzb = new byte[8];///dakika

            i = rd.ReadCard(0X12, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            buffer[0] = rd.InBuf[0];
            buffer[1] = rd.InBuf[1];
            Array.Copy(buffer, 0, temp, 0, 2);
            UInt32 pazar = (UInt32)Hexcon.ByteToDecimal(temp, 2);

            buffer[0] = rd.InBuf[2];
            buffer[1] = rd.InBuf[3];
            Array.Copy(buffer, 0, temp, 0, 2);
            UInt32 ctesi = (UInt32)Hexcon.ByteToDecimal(temp, 2);


            i = rd.ReadCard(0X13, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            buffer[0] = rd.InBuf[0];
            buffer[1] = rd.InBuf[1];
            Array.Copy(buffer, 0, temp, 0, 2);
            UInt32 hici = (UInt32)Hexcon.ByteToDecimal(temp, 2);

            bool format = false;
            byte flagFormat = rd.InBuf[3];
            flagFormat = Convert.ToByte((flagFormat & 0X04));
            if (flagFormat == 4) format = false; else format = true;

            bool eHata = false;
            byte eeHata = rd.InBuf[2];
            eeHata = Convert.ToByte((eeHata & 0X01));
            if (eeHata == 1) eHata = true; else eHata = false;

            // 
            //unsigned char hia[8];///saat
            //unsigned char hib[8];///dakika
            //unsigned char cta[8];///saat
            //unsigned char ctb[8];///dakika
            //unsigned char pza[8];///saat
            //unsigned char pzb[8];///dakika
            ///

            #region pazar tarife saat

            i = rd.ReadCard(0X14, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            pzb[0] = rd.InBuf[0];
            pza[0] = rd.InBuf[1];
            pzb[1] = rd.InBuf[2];
            pza[1] = rd.InBuf[3];

            i = rd.ReadCard(0X15, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            pzb[2] = rd.InBuf[0];
            pza[2] = rd.InBuf[1];
            pzb[3] = rd.InBuf[2];
            pza[3] = rd.InBuf[3];

            i = rd.ReadCard(0X16, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            pzb[4] = rd.InBuf[0];
            pza[4] = rd.InBuf[1];
            pzb[5] = rd.InBuf[2];
            pza[5] = rd.InBuf[3];

            i = rd.ReadCard(0X17, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            pzb[6] = rd.InBuf[0];
            pza[6] = rd.InBuf[1];
            pzb[7] = rd.InBuf[2];
            pza[7] = rd.InBuf[3];

            #endregion

            #region cumartesi tarife saat

            i = rd.ReadCard(0X18, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            ctb[0] = rd.InBuf[0];
            cta[0] = rd.InBuf[1];
            ctb[1] = rd.InBuf[2];
            cta[1] = rd.InBuf[3];

            i = rd.ReadCard(0X19, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            ctb[2] = rd.InBuf[0];
            cta[2] = rd.InBuf[1];
            ctb[3] = rd.InBuf[2];
            cta[3] = rd.InBuf[3];

            i = rd.ReadCard(0X1A, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            ctb[4] = rd.InBuf[0];
            cta[4] = rd.InBuf[1];
            ctb[5] = rd.InBuf[2];
            cta[5] = rd.InBuf[3];

            i = rd.ReadCard(0X1B, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            ctb[6] = rd.InBuf[0];
            cta[6] = rd.InBuf[1];
            ctb[7] = rd.InBuf[2];
            cta[7] = rd.InBuf[3];

            #endregion

            #region hafta içi tarife saat

            i = rd.ReadCard(0X1C, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            hib[0] = rd.InBuf[0];
            hia[0] = rd.InBuf[1];
            hib[1] = rd.InBuf[2];
            hia[1] = rd.InBuf[3];

            i = rd.ReadCard(0X1D, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            hib[2] = rd.InBuf[0];
            hia[2] = rd.InBuf[1];
            hib[3] = rd.InBuf[2];
            hia[3] = rd.InBuf[3];

            i = rd.ReadCard(0X1E, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            hib[4] = rd.InBuf[0];
            hia[4] = rd.InBuf[1];
            hib[5] = rd.InBuf[2];
            hia[5] = rd.InBuf[3];

            i = rd.ReadCard(0X1F, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            hib[6] = rd.InBuf[0];
            hia[6] = rd.InBuf[1];
            hib[7] = rd.InBuf[2];
            hia[7] = rd.InBuf[3];

            #endregion

            string HaftaIci = "";
            for (int j = 0; j < 8; j++)
            {
                HaftaIci += hia[j].ToString().PadLeft(2, '0') + ":" + hib[j].ToString().PadLeft(2, '0') + "#";
            }

            string cumartesi = "";
            for (int j = 0; j < 8; j++)
            {
                cumartesi += cta[j].ToString().PadLeft(2, '0') + ":" + ctb[j].ToString().PadLeft(2, '0') + "#";
            }

            string pzr = "";
            for (int j = 0; j < 8; j++)
            {
                pzr += pza[j].ToString().PadLeft(2, '0') + ":" + pzb[j].ToString().PadLeft(2, '0') + "#";
            }



            i = rd.ReadCard(0X28, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            UInt32 aboneNo = (UInt32)Hexcon.ByteToDecimal(rd.InBuf, 4);

            i = rd.ReadCard(0X29, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            byte kapa_set = rd.InBuf[2];

            Hexcon.ByteToBit state = new Hexcon.ByteToBit(rd.InBuf[3]);
            string Ceza = state.Bit1.ToString();
            string Ariza = state.Bit2.ToString();
            string Iptal = state.Bit3.ToString();
            string PilZayif = state.Bit4.ToString();
            string PilBitik = state.Bit5.ToString();
            string PilKapak = state.Bit6.ToString();
            string c1 = state.Bit7.ToString();
            string cMag1 = state.Bit8.ToString();


            i = rd.ReadCard(0X2A, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            byte aKat1 = rd.InBuf[0];
            byte aKat2 = rd.InBuf[1];
            byte aKat3 = rd.InBuf[2];
            byte aKat4 = rd.InBuf[3];

            i = rd.ReadCard(0X2B, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            Array.Copy(rd.InBuf, 0, temp4, 0, 4);
            bilgikapat = new TarihAl(temp4);


            #region ucuz ve pahali katsayı

            i = rd.ReadCard(0X2C, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            buffer[0] = rd.InBuf[0];
            buffer[1] = rd.InBuf[1];
            Array.Copy(buffer, 0, temp, 0, 2);
            UInt32 kUcuz = (UInt32)Hexcon.ByteToDecimal(temp, 2);

            buffer[0] = rd.InBuf[2];
            buffer[1] = rd.InBuf[3];
            Array.Copy(buffer, 0, temp, 0, 2);
            UInt32 kPahali = (UInt32)Hexcon.ByteToDecimal(temp, 2);

            #endregion

            #region normal ve çok ucuz katsayı

            i = rd.ReadCard(0X2D, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            buffer[0] = rd.InBuf[0];
            buffer[1] = rd.InBuf[1];
            Array.Copy(buffer, 0, temp, 0, 2);
            UInt32 kCokUcuz = (UInt32)Hexcon.ByteToDecimal(temp, 2);

            buffer[0] = rd.InBuf[2];
            buffer[1] = rd.InBuf[3];
            Array.Copy(buffer, 0, temp, 0, 2);
            UInt32 kNormal = (UInt32)Hexcon.ByteToDecimal(temp, 2);

            #endregion


            i = rd.ReadCard(0X2E, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            Array.Copy(rd.InBuf, 0, temp4, 0, 4);
            sayacTarih = new TarihAl(temp4);


            i = rd.ReadCard(0X2F, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            UInt32 gerTuk = (UInt32)Hexcon.ByteToDecimal(rd.InBuf, 4);

            i = rd.ReadCard(0X30, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            UInt32 harcanan = (UInt32)Hexcon.ByteToDecimal(rd.InBuf, 4);

            i = rd.ReadCard(0X31, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            UInt32 kalan = (UInt32)Hexcon.ByteToDecimal(rd.InBuf, 4);

            i = rd.ReadCard(0X32, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            UInt32 cihazNo = (UInt32)Hexcon.ByteToDecimal(rd.InBuf, 4);

            i = rd.ReadCard(0X33, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            Int32 kartNo = rd.InBuf[0];
            Int32 islem = rd.InBuf[1];
            Int32 hangigun = rd.InBuf[3];

            double pilsev = rd.InBuf[2];
            double pilsev1 = pilsev;
            pilsev1 = pilsev1 / 51;
            pilsev1.ToString("F03");
            //string AnaPilSeviyesi = String.Format("{0,8:0.000}", (pilsev * 6 / 255.0));

            i = rd.ReadCard(0X34, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            byte test = rd.InBuf[3];
            byte centik = rd.InBuf[2];
            byte rop = rd.InBuf[1];
            byte tSaat = rd.InBuf[0];

            bool ReelMod = false;
            if (test == 0XFF) ReelMod = true; else ReelMod = false;


            i = rd.ReadCard(0X37, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            UInt32 pahali = (UInt32)Hexcon.ByteToDecimal(rd.InBuf, 4);


            i = rd.ReadCard(0X3D, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            UInt32 ucuz = (UInt32)Hexcon.ByteToDecimal(rd.InBuf, 4);

            i = rd.ReadCard(0X3E, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            UInt32 c_Ucuz = (UInt32)Hexcon.ByteToDecimal(rd.InBuf, 4);

            i = rd.ReadCard(0X3F, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            UInt32 normal = (UInt32)Hexcon.ByteToDecimal(rd.InBuf, 4);





            #endregion

            str += cihazNo + "_" +
                   sayacTarih.gun.ToString().PadLeft(2, '0') + "." + sayacTarih.ay.ToString().PadLeft(2, '0') + ".20" + sayacTarih.yil.ToString().PadLeft(2, '0') + " " + sayacTarih.saat.ToString().PadLeft(2, '0') + ":" + sayacTarih.dakika.ToString().PadLeft(2, '0') + "_" +
                   hangigun + "_" +
                   bilgiarzt.gun.ToString().PadLeft(2, '0') + "." + bilgiarzt.ay.ToString().PadLeft(2, '0') + " " + bilgiarzt.saat.ToString().PadLeft(2, '0') + ":" + bilgiarzt.dakika.ToString().PadLeft(2, '0') + "_" +
                   bilgipulse.gun.ToString().PadLeft(2, '0') + "." + bilgipulse.ay.ToString().PadLeft(2, '0') + " " + bilgipulse.saat.ToString().PadLeft(2, '0') + ":" + bilgipulse.dakika.ToString().PadLeft(2, '0') + "_" +
                   bilgiceza.gun.ToString().PadLeft(2, '0') + "." + bilgiceza.ay.ToString().PadLeft(2, '0') + " " + bilgiceza.saat.ToString().PadLeft(2, '0') + ":" + bilgiceza.dakika.ToString().PadLeft(2, '0') + "_" +
                   bilgikr.gun.ToString().PadLeft(2, '0') + "." + bilgikr.ay.ToString().PadLeft(2, '0') + " " + bilgikr.saat.ToString().PadLeft(2, '0') + ":" + bilgikr.dakika.ToString().PadLeft(2, '0') + "_" +
                   kalan + "_" +
                   harcanan + "_" +
                   kritik + "_" +
                   gerTuk + "_" +
                   pilsev + "_" +
                   c_Ucuz + "_" +
                   ucuz + "_" +
                   normal + "_" +
                   pahali + "_" +
                   ekim + "_" +
                   bilgikapat.gun.ToString().PadLeft(2, '0') + "." + bilgikapat.ay.ToString().PadLeft(2, '0') + ".20" + bilgikapat.yil.ToString().PadLeft(2, '0') + " " + bilgikapat.saat.ToString().PadLeft(2, '0') + ":" + bilgikapat.dakika.ToString().PadLeft(2, '0') + "_" +
                   ReelMod + "_" +
                   Ceza + "_" +
                   Ariza + "_" +
                   Iptal + "_" +
                   PilZayif + "_" +
                   PilBitik + "_" +
                   PilKapak + "_" +
                   c1 + "_" +
                   cMag1 + "_" +
                   kCokUcuz + "_" +
                   kUcuz + "_" +
                   kNormal + "_" +
                   kPahali + "_" +
                   aKat1 + "_" +
                   aKat2 + "_" +
                   aKat3 + "_" +
                   aKat4 + "_" +
                   kartNo + "_" +
                   islem + "_" +
                   tSaat + "_" +
                   rop + "_" +
                   format + "_" +
                   eHata;



            return "1_" + str;
        }

        public string EDegis(UInt32 kalan, UInt32 harcanan)
        {
            byte[] issue_area = GetIssuer();
            int i = 0;


            #region Init           

            i = rd.InitCard();
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(1); }

            i = rd.ReadCard(1, 4);
            if (i == 5) { return Return(5); }
            else if (i == 0) { return Return(5); }

            if ((rd.InBuf[0] != issue_area[0]) || (rd.InBuf[1] != issue_area[1]) || (rd.InBuf[2] != 'Y') || (rd.InBuf[3] != 'E'))
            {
                return Return(10);
            }

            #endregion

            #region Edeğiş işlemleri

            rd.OutBuf[0] = 0x3A;
            rd.OutBuf[1] = 0xDF;
            rd.OutBuf[2] = 0x1D;
            rd.OutBuf[3] = 0x80;

            i = rd.VerifyCard(7);
            if (i == 5) { rd.FinishCard(); { rd.FinishCard(); return "5"; } }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            i = rd.ReadCard(0X3C, 4);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            if (rd.InBuf[0] == 0)
            {
                return Return(10);
            }

            i = rd.ReadCard(2, 4);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            if (rd.InBuf[0] != 4)
            {
                return Return(11);
            }

            Hexcon.UInt32toByte4(kalan, rd.OutBuf);
            i = rd.UpdateCard(0X31);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            Hexcon.UInt32toByte4(harcanan, rd.OutBuf);
            i = rd.UpdateCard(0X30);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }


            i = rd.ReadCard(0X3C, 4);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            rd.OutBuf[2] = rd.InBuf[2];
            rd.OutBuf[3] = rd.InBuf[3];

            if (rd.InBuf[0] == 5) rd.OutBuf[0] = 10;
            else rd.OutBuf[0] = 0;

            rd.OutBuf[1] = 0;

            i = rd.UpdateCard(0X3C);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            //tarifeler

            rd.OutBuf[0] = 0XFF;
            rd.OutBuf[1] = 0XFF;
            rd.OutBuf[2] = 0XFF;
            rd.OutBuf[3] = 0XFF;

            i = rd.UpdateCard(0X2C);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            i = rd.UpdateCard(0X2D);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            #endregion

            rd.FinishCard();

            return "1";
        }

        public string YetkiIadeYap(byte SayacTip, UInt32 CihazNo)
        {
            HataSet(0);

            byte[] issue_area = GetIssuer();
            int i = 0;

            i = rd.InitCard();
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(1); }

            i = rd.ReadCard(1, 4);
            if (i == 5) { return Return(5); }
            else if (i == 0) { return Return(5); }

            if ((rd.InBuf[0] != issue_area[0]) || (rd.InBuf[1] != issue_area[1]) || (rd.InBuf[2] != 'Y'))
            {
                return Return(8);
            }

            rd.OutBuf[0] = 0x3A;
            rd.OutBuf[1] = 0xDF;
            rd.OutBuf[2] = 0x1D;
            rd.OutBuf[3] = 0x80;

            i = rd.VerifyCard(7);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(9); }

            rd.OutBuf[0] = issue_area[0];
            rd.OutBuf[1] = issue_area[1];
            rd.OutBuf[2] = (byte)'Y';
            rd.OutBuf[3] = (byte)'I';

            i = rd.UpdateCard(1);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }

            if (SayacTip == 3)
            {
                rd.OutBuf[0] = 0X02;
                rd.OutBuf[1] = 0X03;
                rd.OutBuf[2] = 0X0;
                rd.OutBuf[3] = 0X0;
            }
            else
            {
                rd.OutBuf[0] = SayacTip;
                rd.OutBuf[1] = SayacTip;
                rd.OutBuf[2] = 0X0;
                rd.OutBuf[3] = 0X0;
            }

            i = rd.UpdateCard(2);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }


            Hexcon.UInt32toByte4(CihazNo, rd.OutBuf);
            i = rd.UpdateCard(0X3C);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }

            rd.OutBuf[0] = 0XFF;
            rd.OutBuf[1] = 0XFF;
            rd.OutBuf[2] = 0XFF;
            rd.OutBuf[3] = 0XFF;

            i = rd.UpdateCard(0X3D);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }

            rd.OutBuf[0] = 0X0;
            rd.OutBuf[1] = 0X0;
            rd.OutBuf[2] = 0X0;
            rd.OutBuf[3] = 0X0;

            i = rd.UpdateCard(0X3E);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }


            i = rd.UpdateCard(0x3F);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(6); }

            rd.FinishCard();

            return "1";

        }

        public string YetkiIadeOku()
        {
            HataSet(0);

            byte[] issue_area = GetIssuer();
            int i = 0;


            #region Init

            i = rd.InitCard();
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(1); }

            i = rd.ReadCard(1, 4);
            if (i == 5) { return Return(5); }
            else if (i == 0) { return Return(5); }

            if ((rd.InBuf[0] != issue_area[0]) || (rd.InBuf[1] != issue_area[1]) || (rd.InBuf[2] != 'Y') || (rd.InBuf[3] != 'I'))
            {
                return Return(12);
            }

            #endregion

            #region İade okuma işlemleri

            rd.OutBuf[0] = 0x3A;
            rd.OutBuf[1] = 0xDF;
            rd.OutBuf[2] = 0x1D;
            rd.OutBuf[3] = 0x80;

            i = rd.VerifyCard(7);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(9); }

            i = rd.ReadCard(0X3C, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            UInt32 CihazNo = (UInt32)Hexcon.ByteToDecimal(rd.InBuf, 4);


            i = rd.ReadCard(0X3D, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            int takilmismi;

            if (rd.InBuf[3] != 0X0) takilmismi = 0;
            else takilmismi = 1;

            i = rd.ReadCard(0X3E, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            UInt32 kredi = (UInt32)Hexcon.ByteToDecimal(rd.InBuf, 4);

            i = rd.ReadCard(0X3F, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            UInt32 harcanan = (UInt32)Hexcon.ByteToDecimal(rd.InBuf, 4);

            if (takilmismi == 0)
            {
                kredi = 0;
                harcanan = 0;
            }

            i = rd.ReadCard(2, 4);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(5); }

            byte IadeTip1 = rd.InBuf[0];
            byte IadeTip2 = rd.InBuf[1];

            #endregion

            rd.FinishCard();


            return "1" +
                    sp + IadeTip1 +
                    sp + CihazNo +
                    sp + kredi +
                    sp + harcanan +
                    sp + IadeTip2 +
                    sp + takilmismi;

        }


        public string ETrans(UInt32 CihazNo, DateTime kapamaTarih, UInt32 gerTuk, UInt32 kalan, UInt32 harcanan, byte kartNo, byte islemNo,
                             byte kademe, byte loadLimit, byte aksam, byte sabah, byte haftasonuAksam, UInt32 donemTuketim,
                             UInt32 fixCharge, UInt32 totalFixCharge, UInt32 k1Tuk, UInt32 k2Tuk, UInt32 k3Tuk, UInt32 Lim1, UInt32 Lim2,
                             UInt32 kad1, UInt32 kad2, UInt32 kad3)
        {
            byte[] issue_area = GetIssuer();
            int i = 0;


            #region Init



            i = rd.InitCard();
            //util.Sleep(10);
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(1); }
            i = rd.ReadCard(1, 4);
            //util.Sleep(10);
            if (i == 5) { return Return(5); }
            else if (i == 0) { return Return(5); }

            if ((rd.InBuf[0] != issue_area[0]) || (rd.InBuf[1] != issue_area[1]) || (rd.InBuf[2] != 'Y'))
            {
                return Return(8);
            }

            #endregion

            #region Bilgi yap işlemleri

            rd.OutBuf[0] = 0x3A;
            rd.OutBuf[1] = 0xDF;
            rd.OutBuf[2] = 0x1D;
            rd.OutBuf[3] = 0x80;

            i = rd.VerifyCard(7);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }


            rd.OutBuf[0] = issue_area[0];
            rd.OutBuf[1] = issue_area[1];
            rd.OutBuf[2] = (byte)'Y';
            rd.OutBuf[3] = (byte)'E';

            i = rd.UpdateCard(1);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            rd.OutBuf[0] = 0X0C;
            rd.OutBuf[1] = 0X0;
            rd.OutBuf[2] = 0X0;
            rd.OutBuf[3] = 0X0;

            i = rd.UpdateCard(0x3C);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            rd.OutBuf[0] = 0X0;
            rd.OutBuf[1] = 0X0;
            rd.OutBuf[2] = 0X0;
            rd.OutBuf[3] = 0X0;

            i = rd.UpdateCard(0x13);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            TarihAl kapTarih = new TarihAl(kapamaTarih);
            rd.OutBuf = kapTarih.TarihDondur4Byte(kapamaTarih);

            i = rd.UpdateCard(0x29);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }


            Hexcon.UInt32toByte4(gerTuk, rd.OutBuf);
            i = rd.UpdateCard(0X2F);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            Hexcon.UInt32toByte4(kalan, rd.OutBuf);
            i = rd.UpdateCard(0X31);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            Hexcon.UInt32toByte4(harcanan, rd.OutBuf);
            i = rd.UpdateCard(0X30);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            Hexcon.UInt32toByte4(CihazNo, rd.OutBuf);
            i = rd.UpdateCard(0X32);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            rd.OutBuf[0] = kartNo;
            rd.OutBuf[1] = islemNo;

            i = rd.UpdateCard(0X33);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            //rd.OutBuf[0] = role;
            //rd.OutBuf[1] = tsaat;
            //rd.OutBuf[2] = 0X0;
            //rd.OutBuf[3] = testreel;
            ////0X34 adres role operasyon sayısı,	tsaat degeri,	bos,	test reel ??
            //i = rd.UpdateCard(0X34);
            //if (i == 5) { rd.FinishCard(); return "5"; }
            //else if (i == 0){ rd.FinishCard(); return "0"; }


            rd.OutBuf[0] = kademe;
            rd.OutBuf[1] = loadLimit;
            rd.OutBuf[2] = aksam;
            rd.OutBuf[3] = sabah;

            i = rd.UpdateCard(0X18);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }


            rd.OutBuf[0] = haftasonuAksam;
            rd.OutBuf[1] = 0X0;
            rd.OutBuf[2] = 0X0;
            rd.OutBuf[3] = 0X0;

            i = rd.UpdateCard(0X1A);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }


            //Hexcon.UInt32toByte4(donemTuketim, rd.OutBuf);
            //i = rd.UpdateCard(0X19);
            //if (i == 5) { rd.FinishCard(); return "5"; }
            //else if (i == 0){ rd.FinishCard(); return "0"; }

            Hexcon.UInt32toByte4(fixCharge, rd.OutBuf);
            i = rd.UpdateCard(0X1B);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            Hexcon.UInt32toByte4(totalFixCharge, rd.OutBuf);
            i = rd.UpdateCard(0X1C);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            Hexcon.UInt32toByte4(k1Tuk, rd.OutBuf);
            i = rd.UpdateCard(0X37);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            Hexcon.UInt32toByte4(k2Tuk, rd.OutBuf);
            i = rd.UpdateCard(0X3D);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            Hexcon.UInt32toByte4(k3Tuk, rd.OutBuf);
            i = rd.UpdateCard(0X3E);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            Hexcon.UInt32toByte4(Lim1, rd.OutBuf);
            i = rd.UpdateCard(0X12);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            Hexcon.UInt32toByte4(Lim2, rd.OutBuf);
            i = rd.UpdateCard(0X14);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            Hexcon.UInt32toByte4(kad1, rd.OutBuf);
            i = rd.UpdateCard(0X15);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            Hexcon.UInt32toByte4(kad2, rd.OutBuf);
            i = rd.UpdateCard(0X16);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }

            Hexcon.UInt32toByte4(kad3, rd.OutBuf);
            i = rd.UpdateCard(0X17);
            if (i == 5) { rd.FinishCard(); return "5"; }
            else if (i == 0) { rd.FinishCard(); return "0"; }


            #endregion


            return "1";
        }

        #endregion

        #region Üretim Fonksyonları

        public string FormUret()
        {
            util.Sleep(10);

            int i = 0;

            #region FormUret işlemleri                       

            i = rd.InitCard();
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(1); }


            rd.OutBuf[0] = 0xAA;
            rd.OutBuf[1] = 0xAA;
            rd.OutBuf[2] = 0xAA;
            rd.OutBuf[3] = 0xAA;

            i = rd.VerifyCard(7);
            if (i == 5) { return Return(2); }
            else if (i == 0) { return Return(2); }


            rd.OutBuf[0] = (byte)'K';
            rd.OutBuf[1] = (byte)'I';
            rd.OutBuf[2] = (byte)'U';
            rd.OutBuf[3] = (byte)'A';

            i = rd.UpdateCard(1);
            if (i == 5) { return Return(6); }
            else if (i == 0) { return Return(6); }

            rd.OutBuf[0] = 0x7B;
            rd.OutBuf[1] = 0x8A;
            rd.OutBuf[2] = 0x13;
            rd.OutBuf[3] = 0xEC;


            i = rd.UpdateCard(6);
            if (i == 5) { return Return(6); }
            else if (i == 0) { return Return(6); }

            #endregion

            rd.FinishCard();


            return "1";
        }

        public string Format(uint CihazNo, uint KritikKredi, uint Kat1, uint Kat2, uint Kat3, uint Lim1, uint Lim2, uint OverLim, UInt16 LowVoltage, UInt16 HighVoltage)
        {
            util.Sleep(10);

            int i = 0;

            #region Init                

            i = rd.InitCard();
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(1); }

            i = rd.ReadCard(1, 4);
            if (i == 5) { return Return(5); }
            else if (i == 0) { return Return(5); }

            if ((rd.InBuf[0] != 'K') || (rd.InBuf[1] != 'I') || (rd.InBuf[2] != 'U'))
            {
                return Return(15);
            }

            #endregion

            #region Format işlemleri

            rd.OutBuf[0] = 0x7B;
            rd.OutBuf[1] = 0x8A;
            rd.OutBuf[2] = 0x13;
            rd.OutBuf[3] = 0xEC;

            i = rd.VerifyCard(7);
            if (i == 5) { return Return(2); }
            else if (i == 0) { return Return(2); }

            rd.OutBuf[0] = (byte)'K';
            rd.OutBuf[1] = (byte)'I';
            rd.OutBuf[2] = (byte)'U';
            rd.OutBuf[3] = (byte)'F';

            i = rd.UpdateCard(1);
            if (i == 5) { return Return(6); }
            else if (i == 0) { return Return(6); }


            Hexcon.UInt32toByte4(CihazNo, rd.OutBuf);
            i = rd.UpdateCard(0X10);
            if (i == 0) { return Return(6); }

            DateTime tar = DateTime.Now;

            TarihAl tarih = new TarihAl(tar);
            rd.OutBuf = tarih.TarihDondur4Byte(tar);

            i = rd.UpdateCard(0x12);
            if (i == 0) { return Return(6); }

            DayOfWeek dow = tar.DayOfWeek;

            rd.OutBuf[0] = (byte)dow;
            rd.OutBuf[1] = 0;
            rd.OutBuf[2] = (byte)dow;
            rd.OutBuf[3] = 0;

            i = rd.UpdateCard(0x11);
            if (i == 0) { return Return(6); }

            Integer4Byte krit = new Integer4Byte(KritikKredi);

            rd.OutBuf[0] = krit.bir;
            rd.OutBuf[1] = krit.iki;
            rd.OutBuf[2] = 0;
            rd.OutBuf[3] = 0;

            i = rd.UpdateCard(0x13);
            if (i == 0) { return Return(6); }

            Hexcon.UInt32toByte4(Kat1, rd.OutBuf);
            i = rd.UpdateCard(0X16);
            if (i == 0) { return Return(6); }

            Hexcon.UInt32toByte4(Kat2, rd.OutBuf);
            i = rd.UpdateCard(0X17);
            if (i == 0) { return Return(6); }

            Hexcon.UInt32toByte4(Kat3, rd.OutBuf);
            i = rd.UpdateCard(0X18);
            if (i == 0) { return Return(6); }

            Hexcon.UInt32toByte4(Lim1, rd.OutBuf);
            i = rd.UpdateCard(0X19);
            if (i == 0) { return Return(6); }

            Hexcon.UInt32toByte4(Lim2, rd.OutBuf);
            i = rd.UpdateCard(0X1A);
            if (i == 0) { return Return(6); }

            rd.OutBuf[0] = 0X0;
            rd.OutBuf[1] = 0X0;
            rd.OutBuf[2] = 0X0;
            rd.OutBuf[3] = 0X0;

            rd.OutBuf[0] = Convert.ToByte(OverLim);

            i = rd.UpdateCard(0X1C);
            if (i == 0) { return Return(6); }

            byte[] lowV = new byte[2];
            Hexcon.UInt16toByte2(LowVoltage, lowV);

            byte[] highV = new byte[2];
            Hexcon.UInt16toByte2(HighVoltage, highV);

            Array.Copy(lowV, 0, rd.OutBuf, 0, 2);
            Array.Copy(highV, 0, rd.OutBuf, 2, 2);

            i = rd.UpdateCard(0X1B);
            if (i == 0) { return Return(6); }

            #endregion

            rd.FinishCard();


            return "1";
        }

        public string ReelMod()
        {
            util.Sleep(10);

            int i = 0;

            #region Init                       

            i = rd.InitCard();
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(1); }

            i = rd.ReadCard(1, 4);
            if (i == 5) { return Return(5); }
            else if (i == 0) { return Return(5); }

            if ((rd.InBuf[0] != 'K') || (rd.InBuf[1] != 'I') || (rd.InBuf[2] != 'U'))
            {
                return Return(15);
            }

            #endregion

            #region Reel Kart İşlemleri

            rd.OutBuf[0] = 0x7B;
            rd.OutBuf[1] = 0x8A;
            rd.OutBuf[2] = 0x13;
            rd.OutBuf[3] = 0xEC;

            i = rd.VerifyCard(7);
            if (i == 5) { return Return(2); }
            else if (i == 0) { return Return(2); }

            rd.OutBuf[0] = (byte)'K';
            rd.OutBuf[1] = (byte)'I';
            rd.OutBuf[2] = (byte)'U';
            rd.OutBuf[3] = (byte)'R';

            i = rd.UpdateCard(1);
            if (i == 5) { return Return(6); }
            else if (i == 0) { return Return(6); }

            #endregion

            rd.FinishCard();

            return "1";
        }

        public string TestMod(UInt32 CihazNo)
        {
            util.Sleep(10);

            int i = 0;

            #region Init                                  

            i = rd.InitCard();
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(1); }

            i = rd.ReadCard(1, 4);
            if (i == 5) { return Return(5); }
            else if (i == 0) { return Return(5); }

            if ((rd.InBuf[0] != 'K') || (rd.InBuf[1] != 'I') || (rd.InBuf[2] != 'U'))
            {
                return Return(15);
            }

            #endregion

            #region Test Mod Kart İşlemleri

            rd.OutBuf[0] = 0x7B;
            rd.OutBuf[1] = 0x8A;
            rd.OutBuf[2] = 0x13;
            rd.OutBuf[3] = 0xEC;

            i = rd.VerifyCard(7);
            if (i == 5) { return Return(2); }
            else if (i == 0) { return Return(2); }

            rd.OutBuf[0] = (byte)'K';
            rd.OutBuf[1] = (byte)'I';
            rd.OutBuf[2] = (byte)'U';
            rd.OutBuf[3] = (byte)'T';

            i = rd.UpdateCard(1);
            if (i == 5) { return Return(6); }
            else if (i == 0) { return Return(6); }

            Hexcon.UInt32toByte4(CihazNo, rd.OutBuf);
            i = rd.UpdateCard(0X3C);
            if (i == 5) { return Return(6); }
            else if (i == 0) { return Return(6); }

            rd.OutBuf[0] = 0xFF;
            rd.OutBuf[1] = 0xFF;
            rd.OutBuf[2] = 0xFF;
            rd.OutBuf[3] = 0xFF;

            i = rd.UpdateCard(0X3D);
            if (i == 5) { return Return(6); }
            else if (i == 0) { return Return(6); }

            #endregion

            rd.FinishCard();

            return "1";
        }

        public string CihazNo(uint CihazNo, uint KritikKredi, uint Kat1, uint Kat2, uint Kat3, uint Lim1, uint Lim2, uint OverLim, UInt16 LowVoltage, UInt16 HighVoltage)
        {
            util.Sleep(10);

            int i = 0;


            #region Init                       

            i = rd.InitCard();
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(1); }

            i = rd.ReadCard(1, 4);
            if (i == 5) { return Return(5); }
            else if (i == 0) { return Return(6); }

            if ((rd.InBuf[0] != 'K') || (rd.InBuf[1] != 'I') || (rd.InBuf[2] != 'U'))
            {
                return Return(15);
            }

            #endregion

            #region Format işlemleri

            rd.OutBuf[0] = 0x7B;
            rd.OutBuf[1] = 0x8A;
            rd.OutBuf[2] = 0x13;
            rd.OutBuf[3] = 0xEC;

            i = rd.VerifyCard(7);
            if (i == 5) { return Return(2); }
            else if (i == 0) { return Return(2); }

            rd.OutBuf[0] = (byte)'K';
            rd.OutBuf[1] = (byte)'I';
            rd.OutBuf[2] = (byte)'U';
            rd.OutBuf[3] = (byte)'C';

            i = rd.UpdateCard(1);
            if (i == 5) { return Return(6); }
            else if (i == 0) { return Return(6); }


            Hexcon.UInt32toByte4(CihazNo, rd.OutBuf);
            i = rd.UpdateCard(0X10);
            if (i == 5) { return Return(6); }
            else if (i == 0) { return Return(6); }


            DateTime tar = DateTime.Now;

            TarihAl tarih = new TarihAl(tar);
            rd.OutBuf = tarih.TarihDondur4Byte(tar);

            i = rd.UpdateCard(0x12);
            if (i == 5) { return Return(6); }
            else if (i == 0) { return Return(6); }

            DayOfWeek dow = tar.DayOfWeek;

            rd.OutBuf[0] = 0XFF; // Üretim formattan tek farkı burası
            rd.OutBuf[1] = 0;
            rd.OutBuf[2] = (byte)dow;
            rd.OutBuf[3] = 0;

            i = rd.UpdateCard(0x11);
            if (i == 5) { return Return(6); }
            else if (i == 0) { return Return(6); }

            Integer4Byte krit = new Integer4Byte(KritikKredi);

            rd.OutBuf[0] = krit.bir;
            rd.OutBuf[1] = krit.iki;
            rd.OutBuf[2] = 0;
            rd.OutBuf[3] = 0;

            i = rd.UpdateCard(0x13);
            if (i == 5) { return Return(6); }
            else if (i == 0) { return Return(6); }

            Hexcon.UInt32toByte4(Kat1, rd.OutBuf);
            i = rd.UpdateCard(0X16);
            if (i == 0) { return Return(6); }

            Hexcon.UInt32toByte4(Kat2, rd.OutBuf);
            i = rd.UpdateCard(0X17);
            if (i == 0) { return Return(6); }

            Hexcon.UInt32toByte4(Kat3, rd.OutBuf);
            i = rd.UpdateCard(0X18);
            if (i == 0) { return Return(6); }

            Hexcon.UInt32toByte4(Lim1, rd.OutBuf);
            i = rd.UpdateCard(0X19);
            if (i == 0) { return Return(6); }

            Hexcon.UInt32toByte4(Lim2, rd.OutBuf);
            i = rd.UpdateCard(0X1A);
            if (i == 0) { return Return(6); }

            rd.OutBuf[0] = 0X0;
            rd.OutBuf[1] = 0X0;
            rd.OutBuf[2] = 0X0;
            rd.OutBuf[3] = 0X0;

            rd.OutBuf[0] = Convert.ToByte(OverLim);

            i = rd.UpdateCard(0X1C);
            if (i == 0) { return Return(6); }

            byte[] lowV = new byte[2];
            Hexcon.UInt16toByte2(LowVoltage, lowV);

            byte[] highV = new byte[2];
            Hexcon.UInt16toByte2(HighVoltage, highV);

            Array.Copy(lowV, 0, rd.OutBuf, 0, 2);
            Array.Copy(highV, 0, rd.OutBuf, 2, 2);

            i = rd.UpdateCard(0X1B);
            if (i == 0) { return Return(6); }

            #endregion

            rd.FinishCard();

            return "1";

        }

        public string UretimAc(Int32 Acma)
        {

            util.Sleep(10);

            int i = 0;

            #region Init

            i = rd.InitCard();
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(1); }

            i = rd.ReadCard(1, 4);
            if (i == 5) { return Return(5); }
            else if (i == 0) { return Return(5); }

            if ((rd.InBuf[0] != 'K') || (rd.InBuf[1] != 'I') || (rd.InBuf[2] != 'U'))
            {
                return Return(15);
            }

            #endregion

            #region Format işlemleri

            rd.OutBuf[0] = 0x7B;
            rd.OutBuf[1] = 0x8A;
            rd.OutBuf[2] = 0x13;
            rd.OutBuf[3] = 0xEC;

            i = rd.VerifyCard(7);
            if (i == 5) { return Return(2); }
            else if (i == 0) { return Return(2); }

            rd.OutBuf[0] = (byte)'K';
            rd.OutBuf[1] = (byte)'I';
            rd.OutBuf[2] = (byte)'U';
            rd.OutBuf[3] = (byte)'A';

            i = rd.UpdateCard(1);
            if (i == 5) { return Return(6); }
            else if (i == 0) { return Return(6); }

            if (Acma == 2)
                rd.OutBuf[0] = 0X66;
            else
                rd.OutBuf[0] = 0X00;

            rd.OutBuf[1] = 0X00;
            rd.OutBuf[2] = 0X00;
            rd.OutBuf[3] = 0X00;

            i = rd.UpdateCard(0X3C);
            if (i == 5) { return Return(6); }
            else if (i == 0) { return Return(6); }

            #endregion

            rd.FinishCard();

            return "1";
        }

        public string FormIssuer(UInt32 CihazNo)
        {
            util.Sleep(10);

            int i = 0;

            #region Init


            i = rd.InitCard();
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(1); }

            rd.OutBuf[0] = 0xAA;
            rd.OutBuf[1] = 0xAA;
            rd.OutBuf[2] = 0xAA;
            rd.OutBuf[3] = 0xAA;

            i = rd.VerifyCard(7);
            if (i == 5) { return Return(2); }
            else if (i == 0) { return Return(2); }

            rd.OutBuf[0] = (byte)'K';
            rd.OutBuf[1] = (byte)'K';
            rd.OutBuf[2] = (byte)'X';

            i = rd.UpdateCard(1);
            if (i == 5) { return Return(6); }
            else if (i == 0) { return Return(6); }

            rd.OutBuf[0] = 0x3A;
            rd.OutBuf[1] = 0xDF;
            rd.OutBuf[2] = 0x1D;
            rd.OutBuf[3] = 0x80;

            i = rd.UpdateCard(6);
            if (i == 5) { return Return(6); }
            else if (i == 0) { return Return(6); }

            Hexcon.UInt32toByte4(CihazNo, rd.OutBuf);
            i = rd.UpdateCard(0X10);
            if (i == 5) { return Return(6); }
            else if (i == 0) { return Return(6); }

            i = rd.UpdateCard(2);
            if (i == 5) { return Return(6); }
            else if (i == 0) { return Return(6); }

            #endregion

            rd.FinishCard();

            return "1";
        }

        public string Issuer()
        {
            util.Sleep(10);

            int i = 0;

            byte[] issue_area = GetIssuer();

            #region Init                      

            i = rd.InitCard();
            if (i == 5) { return Return(1); }
            else if (i == 0) { return Return(1); }

            i = rd.ReadCard(1, 4);
            if (i == 5) { return Return(5); }
            else if (i == 0) { return Return(5); }

            if ((rd.InBuf[0] != 'K') || (rd.InBuf[1] != 'K') || (rd.InBuf[2] != 'X'))
            {
                return Return(15);
            }

            rd.OutBuf[0] = 0x3A;
            rd.OutBuf[1] = 0xDF;
            rd.OutBuf[2] = 0x1D;
            rd.OutBuf[3] = 0x80;

            i = rd.VerifyCard(7);
            if (i == 5) { return Return(2); }
            else if (i == 0) { return Return(2); }

            //int Issuer1,Issuer2;

            //int.TryParse(Issuer.Substring(0, 1), out Issuer1);
            //int.TryParse(Issuer.Substring(1, 1), out Issuer2);


            rd.OutBuf[0] = issue_area[0];
            rd.OutBuf[1] = issue_area[1];


            i = rd.UpdateCard(0X3C);
            if (i == 5) { return Return(6); }
            else if (i == 0) { return Return(6); }

            rd.OutBuf[0] = 0xFF;
            rd.OutBuf[1] = 0xFF;
            rd.OutBuf[2] = 0xFF;
            rd.OutBuf[3] = 0xFF;

            i = rd.UpdateCard(0X3d);
            if (i == 5) { return Return(6); }
            else if (i == 0) { return Return(6); }

            rd.OutBuf[0] = 0x0;
            rd.OutBuf[1] = 0x0;
            rd.OutBuf[2] = 0x0;
            rd.OutBuf[3] = 0x0;

            #endregion

            rd.FinishCard();

            return "1";
        }

        #endregion
    }
}
