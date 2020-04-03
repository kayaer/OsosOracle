using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCard.Model
{
    public struct Abone
    {
        public UInt32 CihazNo, Katsayi1, Katsayi2, Katsayi3, Limit1, Limit2;

        public Int32 AnaKredi, YedekKredi, AboneNo;
        public byte AkoYko, IslemNo, KartNo, Cap, Tip, DonemGun, VanaPulseSure, VanaCntSure, Iade,
        MaxdebiSiniri, HaftaSonuOnay, IleriSaat;
        public UInt16 Bayram1, Bayram2;

        public UInt16 bos1;
        public UInt32 Katsayi4, Katsayi5, Limit3, Limit4;
        public string KademeSayisi;

        public Int32 KalanKredi;
        public UInt32 HarcananKredi, GercekTuketim, KademeTuketim1, KademeTuketim2, KademeTuketim3;

        public string SayacTarihi;
        public UInt32 NegatifTuketim, DonemTuketimi, DonemTuketimi1, DonemTuketimi2, DonemTuketimi3;

        public UInt32 DonemTuketimi4, DonemTuketimi5, DonemTuketimi6;
        public string SonKrediTarihi, SonPulseTarihi, SonCezaTarihi, SonArizaTarihi, BorcTarihi, VanaAcmaTarihi, SicaklikHataTarihi;


        public string VanaKapamaTarihi;
        public byte Versiyon, VanaOperasyonSayisi, SayacDurumu, AnaPil, DonemGunNo, KartHata, HaftaninGunu, VanaDurumu, ArizaTipi, MaxdebiSeviyesi;
        public string SonTakilanYetkiKartiOzellik1, SonTakilanYetkiKartiOzellik2, SonTakilanYetkiKartiOzellik3, MaxDebiTarihi;

        public UInt32 DonemTuketimi7, DonemTuketimi8, DonemTuketimi9, DonemTuketimi10, DonemTuketimi11, DonemTuketimi12;
        public UInt16 DonemTuketimi13, DonemTuketimi14;

        public UInt16 DonemTuketimi15, DonemTuketimi16, DonemTuketimi17, DonemTuketimi18;
        public UInt16 DonemTuketimi19, DonemTuketimi20, DonemTuketimi21, DonemTuketimi22, DonemTuketimi23, DonemTuketimi24;
        public UInt32 Mektuk, KademeTuketim4, KademeTuketim5;
        public Int32 IadeKalan;
        public byte ResetSayisi, YanginModuSuresi;
        public int SicaklikHataSeviyesi;
        public bool Sizinti;
        public UInt16 SizintiMiktar;

        public int Index;


        public Abone(byte[] d)
        {
            #region sıfırla

            Index = 0;

            CihazNo = Katsayi1 = Katsayi2 = Katsayi3 = Limit1 = Limit2 = 0;

            AnaKredi = YedekKredi = AboneNo = 0;
            AkoYko = IslemNo = KartNo = Cap = Tip = DonemGun = VanaPulseSure = VanaCntSure = Iade =
            MaxdebiSiniri = HaftaSonuOnay = IleriSaat = 0;
            Bayram1 = Bayram2 = 0;

            bos1 = 0;
            Katsayi4 = Katsayi5 = Limit3 = Limit4 = 0;


            KalanKredi = 0;
            HarcananKredi = GercekTuketim = KademeTuketim1 = KademeTuketim2 = KademeTuketim3 = 0;

            SayacTarihi = "";
            NegatifTuketim = DonemTuketimi = DonemTuketimi1 = DonemTuketimi2 = DonemTuketimi3 = 0;

            DonemTuketimi4 = DonemTuketimi5 = DonemTuketimi6 = 0;
            SonKrediTarihi = SonPulseTarihi = SonCezaTarihi = SonArizaTarihi = BorcTarihi = VanaAcmaTarihi = SicaklikHataTarihi = "";

            VanaKapamaTarihi = MaxDebiTarihi = "";
            Versiyon = VanaOperasyonSayisi = SayacDurumu = AnaPil = DonemGunNo = KartHata = HaftaninGunu = VanaDurumu = ArizaTipi = MaxdebiSeviyesi = 0;
            SonTakilanYetkiKartiOzellik1 = SonTakilanYetkiKartiOzellik2 = SonTakilanYetkiKartiOzellik3 = "";

            DonemTuketimi7 = DonemTuketimi8 = DonemTuketimi9 = DonemTuketimi10 = DonemTuketimi11 = DonemTuketimi12 = 0;
            DonemTuketimi13 = DonemTuketimi14 = 0;

            DonemTuketimi15 = DonemTuketimi16 = DonemTuketimi17 = DonemTuketimi18 = 0;
            DonemTuketimi19 = DonemTuketimi20 = DonemTuketimi21 = DonemTuketimi22 = DonemTuketimi23 = DonemTuketimi24 = 0;
            KademeTuketim4 = KademeTuketim5 = 0;
            Mektuk = 0;
            IadeKalan = 0;
            ResetSayisi = YanginModuSuresi = 0;
            SicaklikHataSeviyesi = 0;
            SizintiMiktar = 0;
            Sizinti = false;

            #endregion



            CihazNo = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            Katsayi1 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            Katsayi2 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            Katsayi3 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            Limit1 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            Limit2 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;

            AnaKredi = Hexcon.Byte4toInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            YedekKredi = Hexcon.Byte4toInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            AkoYko = d[Index]; Index += 1;
            IslemNo = d[Index]; Index += 1;
            KartNo = d[Index]; Index += 1;
            Cap = d[Index]; Index += 1;
            Tip = d[Index]; Index += 1;
            DonemGun = d[Index]; Index += 1;
            VanaPulseSure = d[Index]; Index += 1;
            VanaCntSure = d[Index]; Index += 1;
            Iade = d[Index]; Index += 1;
            MaxdebiSiniri = d[Index]; Index += 1;
            HaftaSonuOnay = d[Index]; Index += 1;
            IleriSaat = d[Index]; Index += 1;
            AboneNo = Hexcon.Byte4toInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            Bayram1 = Hexcon.Byte2toUInt16(d[Index], d[Index + 1]); Index += 2;
            Bayram2 = Hexcon.Byte2toUInt16(d[Index], d[Index + 1]); Index += 2;

            Index += 2; //2 byte BOŞ

            if ((d[Index] == 0X55) && (d[Index + 1] == 0XED))
                KademeSayisi = "5";
            else KademeSayisi = "3";
            Index += 2;

            Katsayi4 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            Katsayi5 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            Limit3 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            Limit4 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;

            KalanKredi = Hexcon.Byte4toInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            HarcananKredi = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            GercekTuketim = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            KademeTuketim1 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            KademeTuketim2 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            KademeTuketim3 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;

            SayacTarihi = Hexcon.TarihDuzenle(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            NegatifTuketim = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            DonemTuketimi = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            DonemTuketimi1 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            DonemTuketimi2 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            DonemTuketimi3 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;


            DonemTuketimi4 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            DonemTuketimi5 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            DonemTuketimi6 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            SonKrediTarihi = Hexcon.TarihDuzenle(d[Index], d[Index + 1]); Index += 2;
            SonPulseTarihi = Hexcon.TarihDuzenle(d[Index], d[Index + 1]); Index += 2;
            SonCezaTarihi = Hexcon.TarihDuzenle(d[Index], d[Index + 1]); Index += 2;
            SonArizaTarihi = Hexcon.TarihDuzenle(d[Index], d[Index + 1]); Index += 2;
            BorcTarihi = Hexcon.TarihDuzenle(d[Index], d[Index + 1]); Index += 2;
            VanaAcmaTarihi = Hexcon.TarihDuzenle(d[Index], d[Index + 1]); Index += 2;


            VanaKapamaTarihi = Hexcon.TarihDuzenle(d[Index], d[Index + 1]); Index += 2;
            Versiyon = d[Index]; Index += 1;
            VanaOperasyonSayisi = d[Index]; Index += 1;

            SayacDurumu = d[Index]; Index += 1;
            AnaPil = d[Index]; Index += 1;
            DonemGunNo = d[Index]; Index += 1;
            KartHata = d[Index]; Index += 1;
            HaftaninGunu = d[Index]; Index += 1;
            VanaDurumu = d[Index]; Index += 1;// 0X08:Açık, 0X00: Kapalı
            ArizaTipi = d[Index]; Index += 1;
            MaxdebiSeviyesi = d[Index]; Index += 1;
            SonTakilanYetkiKartiOzellik1 = Hexcon.TarihDuzenle(d[Index], d[Index + 1]) + " - " +
                                           d[Index + 3].ToString() + " - " + d[Index + 4].ToString(); Index += 4;
            SonTakilanYetkiKartiOzellik2 = Hexcon.TarihDuzenle(d[Index], d[Index + 1]) + " - " +
                                           d[Index + 3].ToString() + " - " + d[Index + 4].ToString(); Index += 4;
            SonTakilanYetkiKartiOzellik3 = Hexcon.TarihDuzenle(d[Index], d[Index + 1]) + " - " +
                                           d[Index + 3].ToString() + " - " + d[Index + 4].ToString(); Index += 4;
            MaxDebiTarihi = Hexcon.TarihDuzenle(d[Index], d[Index + 1]); Index += 2;


            DonemTuketimi7 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            DonemTuketimi8 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            DonemTuketimi9 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            DonemTuketimi10 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            DonemTuketimi11 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            DonemTuketimi12 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            DonemTuketimi13 = Hexcon.Byte2toUInt16(d[Index], d[Index + 1]); Index += 2;
            DonemTuketimi14 = Hexcon.Byte2toUInt16(d[Index], d[Index + 1]); Index += 2;


            DonemTuketimi15 = Hexcon.Byte2toUInt16(d[Index], d[Index + 1]); Index += 2;
            DonemTuketimi16 = Hexcon.Byte2toUInt16(d[Index], d[Index + 1]); Index += 2;
            DonemTuketimi17 = Hexcon.Byte2toUInt16(d[Index], d[Index + 1]); Index += 2;
            DonemTuketimi18 = Hexcon.Byte2toUInt16(d[Index], d[Index + 1]); Index += 2;
            DonemTuketimi19 = Hexcon.Byte2toUInt16(d[Index], d[Index + 1]); Index += 2;
            DonemTuketimi20 = Hexcon.Byte2toUInt16(d[Index], d[Index + 1]); Index += 2;
            DonemTuketimi21 = Hexcon.Byte2toUInt16(d[Index], d[Index + 1]); Index += 2;
            DonemTuketimi22 = Hexcon.Byte2toUInt16(d[Index], d[Index + 1]); Index += 2;
            DonemTuketimi23 = Hexcon.Byte2toUInt16(d[Index], d[Index + 1]); Index += 2;
            DonemTuketimi24 = Hexcon.Byte2toUInt16(d[Index], d[Index + 1]); Index += 2;
            Mektuk = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            IadeKalan = Hexcon.Byte4toInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            ResetSayisi = d[Index]; Index += 1;

            if (Convert.ToInt32(d[Index]) == 1)
            {
                Sizinti = true; Index += 1;
                SizintiMiktar = Hexcon.Byte2toUInt16(d[Index], d[Index + 1]); Index += 2;
            }
            else //if (Convert.ToInt32(d[Index]) == 0)
            {
                Sizinti = false;
                SizintiMiktar = 0;
                Index += 3;
            }

            KademeTuketim4 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;
            KademeTuketim5 = Hexcon.Byte4toUInt32(d[Index], d[Index + 1], d[Index + 2], d[Index + 3]); Index += 4;

            SicaklikHataSeviyesi = (int)(sbyte)d[Index]; Index += 1;
            SicaklikHataTarihi = Hexcon.TarihDuzenle(d[Index], d[Index + 1]); Index += 2;
            YanginModuSuresi = d[Index]; Index += 1;
        }
    }
}
