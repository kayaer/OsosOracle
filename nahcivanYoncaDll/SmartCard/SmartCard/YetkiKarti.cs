using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCard
{
    public struct YetkiKarti
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
        BorcTarihi, VanaAcilmaTarihi, VanaKapanmaTarihi, SicaklikHataTarihi;

        public string MotorPil;
        public byte HaftaninGunu;
        public string ArizaTipi, ETransDurum;
        public byte VanaPulseSure, VanaCntSure,
             KartNo, IslemNo, Vop, TestSaat;
        public string TestReel;
        public byte SayacDurumu, ResetSayisi;

        public string AnaPil, Format;

        public byte Versiyon, Kademe, DonemGun, DonemGunNo, VanaBekleme, Dil, SensorTipi, NoktaHane, PulseCarpan, YanginModuSuresi, Leakage;
        public int SicaklikHataSeviyesi;
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

        public YetkiKarti(byte[] OkunanDegerler)
        {


            Degiskenler = "BilgiTipi;KartKodu;Cap;Tip;CihazNo;AboneNo;KalanKredi;HarcananKredi;KritikKredi;Limit1;Limit2;Katsayi1;Katsayi2;Katsayi3;DonemTuketimi;NegatifTuketim;GerCekTuketim;SonYuklenenKredi;Kademe1Tuketimi;Kademe2Tuketimi;Kademe3Tuketimi;DonemTuketim1;DonemTuketim2;DonemTuketim3;DonemTuketim4;DonemTuketim5;DonemTuketim6;SayacTarihi;SonKrediTarihi;SonPulseTarihi;SonCezaTarihi;SonArizaTarihi;Ceza3Tarihi;Ceza4Tarihi;BorcTarihi;VanaAcilmaTarihi;VanaKapanmaTarihi;MotorPil;HaftaninGunu;ArizaTipi;ETransDurum;VanaPulseSure;VanaCntSure;KartNo;IslemNo;Vop;TestSaat;TestReel;SayacDurumu;AnaPil;Format;Versiyon;Kademe;DonemGun;DonemGunNo;MaxdebiTarihi;MaxdebiSeviye;MaxdebiSinir;VanaDurumu;DonemTuketim7;DonemTuketim8;DonemTuketim9;DonemTuketim10;DonemTuketim11;DonemTuketim12;Mektuk;HaftaSonuOnay;ResetSayısı;ToplamYuk.Kredi;YazılımVersiyon;İlkPulseTarihi;Katsayi4;Katsayi5;Limit3;Limit4;Kademe4Tuketimi;Kademe5Tuketimi;VanaBekleme;Dil;SensorTipi;NoktaHane;PulseCarpan;SicaklikHataSeviyesi;SicaklikHataTarihi;YanginModuSuresi;Leakage";
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
            BorcTarihi = VanaAcilmaTarihi = VanaKapanmaTarihi = SicaklikHataTarihi = "";

            MotorPil = "";

            HaftaninGunu = 0;
            ArizaTipi = ETransDurum = "";
            VanaPulseSure = VanaCntSure =
            KartNo = IslemNo = Vop = TestSaat = 0;
            TestReel = "";
            SayacDurumu = 0;

            AnaPil = Format = "";

            Versiyon = Kademe = DonemGun = DonemGunNo = VanaBekleme = Dil = SensorTipi = NoktaHane = PulseCarpan = YanginModuSuresi = Leakage = 0;
            SicaklikHataSeviyesi = 0;
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

            CihazNo = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            AboneNo = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            KalanKredi = Hexcon.Byte4toInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            HarcananKredi = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            KritikKredi = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Limit1 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Limit2 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Katsayi1 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Katsayi2 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Katsayi3 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketimi = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            NegatifTuketim = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            GerCekTuketim = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            SonYuklenenKredi = Hexcon.Byte4toInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            Kademe1Tuketimi = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Kademe2Tuketimi = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Kademe3Tuketimi = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim1 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim2 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim3 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim4 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim5 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim6 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            SayacTarihi = Hexcon.TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            SonKrediTarihi = Hexcon.TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            SonPulseTarihi = Hexcon.TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            SonCezaTarihi = Hexcon.TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            SonArizaTarihi = Hexcon.TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            Ceza3Tarihi = Hexcon.TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            Ceza4Tarihi = Hexcon.TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            BorcTarihi = Hexcon.TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            VanaAcilmaTarihi = Hexcon.TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            VanaKapanmaTarihi = Hexcon.TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;

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

            MaxdebiTarihi = Hexcon.TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;

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

            DonemTuketim7 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim8 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim9 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim10 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim11 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim12 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            MekanikTuketim = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            HaftaSonuOnay = OkunanDegerler[Index + 1]; Index++;
            ResetSayisi = OkunanDegerler[Index + 1]; Index++;

            ToplamYuklenenKredi = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            YazilimVersiyon = Convert.ToChar(OkunanDegerler[Index + 1]).ToString() + Convert.ToChar(OkunanDegerler[Index + 2]).ToString() + Convert.ToChar(OkunanDegerler[Index + 3]).ToString() + Convert.ToChar(OkunanDegerler[Index + 4]).ToString() +
                              Convert.ToChar(OkunanDegerler[Index + 5]).ToString() + Convert.ToChar(OkunanDegerler[Index + 6]).ToString() + Convert.ToChar(OkunanDegerler[Index + 7]).ToString() + Convert.ToChar(OkunanDegerler[Index + 8]).ToString() +
                              Convert.ToChar(OkunanDegerler[Index + 9]).ToString() + Convert.ToChar(OkunanDegerler[Index + 10]).ToString();
            Index += 10;
            IlkPulseTarihi = Hexcon.TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;

            Katsayi4 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Katsayi5 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Limit3 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Limit4 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Kademe4Tuketimi = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Kademe5Tuketimi = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            VanaBekleme = OkunanDegerler[Index + 1]; Index++; ;
            Dil = OkunanDegerler[Index + 1]; Index++; ;
            SensorTipi = OkunanDegerler[Index + 1]; Index++; ;
            NoktaHane = OkunanDegerler[Index + 1]; Index++; ;
            PulseCarpan = OkunanDegerler[Index + 1]; Index++; ;

            SicaklikHataSeviyesi = (int)(sbyte)OkunanDegerler[Index + 1]; Index++;
            SicaklikHataTarihi = Hexcon.TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            YanginModuSuresi = OkunanDegerler[Index + 1]; Index++;
            Leakage = OkunanDegerler[Index + 1]; Index++;

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
            DegiskenDegerleri[77] = VanaBekleme.ToString();
            DegiskenDegerleri[78] = Dil.ToString();
            DegiskenDegerleri[79] = SensorTipi.ToString();
            DegiskenDegerleri[80] = NoktaHane.ToString();
            DegiskenDegerleri[81] = PulseCarpan.ToString();
            DegiskenDegerleri[82] = SicaklikHataSeviyesi.ToString();
            DegiskenDegerleri[83] = SicaklikHataTarihi.ToString();
            DegiskenDegerleri[84] = YanginModuSuresi.ToString();
            DegiskenDegerleri[85] = Leakage.ToString();




            #endregion
        }
    }
}
