using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCard.CiniGaz.Global
{
    internal class Map
    {
        public struct Abone
        {
            public UInt32 CihazNo, Katsayi1, Katsayi2, Katsayi3, Limit1, Limit2, AboneNo;
            public Int32 AnaKredi, YedekKredi;
            public byte AkoYko;
            public string AnaKrediDurum, YedekKrediDurum;
            public byte IslemNo, KartNo, Cap, Tip, DonemGun1, VanaPulseSure, VanaCntSure, Iade, MaxdebiSiniri, HaftaSonuOnay, bos;
            public UInt16 Bayram1, Bayram2;
            public Int32 KalanKredi;
            public UInt32 HarcananKredi, GercekTuketim, KademeTuketim1, KademeTuketim2, KademeTuketim3;
            public string SayacTarihi;
            public UInt32 NegatifTuketim, DonemTuketimi, DonemTuketimi1, DonemTuketimi2, DonemTuketimi3;
            public UInt32 DonemTuketimi4, DonemTuketimi5, DonemTuketimi6;
            public string SonKrediTarihi, SonPulseTarihi, SonCezaTarihi, SonArizaTarihi, BorcTarihi, VanaAcmaTarihi;
            public string VanaKapamaTarihi;
            public byte Versiyon, VanaOperasyonSayisi, SayacDurumu;
            public string SayacDurumAciklama;
            public byte AnaPil, DonemGun2, KartHata, HaftaninGunu, VanaDurumu, ArizaTipi, MaxdebiSeviyesi;
            public string SonTakilanYetkiKartiOzellik1, SonTakilanYetkiKartiOzellik2, SonTakilanYetkiKartiOzellik3, MaxDebiTarihi;
            public UInt32 DonemTuketimi7, DonemTuketimi8, DonemTuketimi9, DonemTuketimi10, DonemTuketimi11, DonemTuketimi12;
            public UInt16 DonemTuketimi13, DonemTuketimi14;
            public UInt16 DonemTuketimi15, DonemTuketimi16, DonemTuketimi17, DonemTuketimi18;
            public UInt16 DonemTuketimi19, DonemTuketimi20, DonemTuketimi21, DonemTuketimi22, DonemTuketimi23, DonemTuketimi24;
            public UInt32 Mektuk;
            public Int32 IadeKalan;
            public byte ResetSayisi;

            public Abone(byte[] inBuf)
            {
                CihazNo = Katsayi1 = Katsayi2 = Katsayi3 = Limit1 = Limit2 = AboneNo = 0;
                AnaKredi = YedekKredi = 0;
                AkoYko = IslemNo = KartNo = Cap = Tip = DonemGun1 = VanaPulseSure = VanaCntSure = Iade = MaxdebiSiniri = HaftaSonuOnay = bos = 0;
                Bayram1 = Bayram2 = 0;
                KalanKredi = 0;
                HarcananKredi = GercekTuketim = KademeTuketim1 = KademeTuketim2 = KademeTuketim3 = 0;
                SayacDurumAciklama = AnaKrediDurum = YedekKrediDurum = SayacTarihi = "";
                NegatifTuketim = DonemTuketimi = DonemTuketimi1 = DonemTuketimi2 = DonemTuketimi3 = 0;
                DonemTuketimi4 = DonemTuketimi5 = DonemTuketimi6 = 0;
                SonKrediTarihi = SonPulseTarihi = SonCezaTarihi = SonArizaTarihi = BorcTarihi = VanaAcmaTarihi = "";
                VanaKapamaTarihi = MaxDebiTarihi = "";
                Versiyon = VanaOperasyonSayisi = SayacDurumu = AnaPil = DonemGun2 = KartHata = HaftaninGunu = VanaDurumu = ArizaTipi = MaxdebiSeviyesi = 0;
                SonTakilanYetkiKartiOzellik1 = SonTakilanYetkiKartiOzellik2 = SonTakilanYetkiKartiOzellik3 = "";
                DonemTuketimi7 = DonemTuketimi8 = DonemTuketimi9 = DonemTuketimi10 = DonemTuketimi11 = DonemTuketimi12 = 0;
                DonemTuketimi13 = DonemTuketimi14 = 0;
                DonemTuketimi15 = DonemTuketimi16 = DonemTuketimi17 = DonemTuketimi18 = 0;
                DonemTuketimi19 = DonemTuketimi20 = DonemTuketimi21 = DonemTuketimi22 = DonemTuketimi23 = DonemTuketimi24 = 0;
                Mektuk = 0;
                IadeKalan = 0;
                ResetSayisi = 0;

                CihazNo = Converter.Byte4toUInt32(inBuf[0], inBuf[1], inBuf[2], inBuf[3]);
                Katsayi1 = Converter.Byte4toUInt32(inBuf[4], inBuf[5], inBuf[6], inBuf[7]);
                Katsayi2 = Converter.Byte4toUInt32(inBuf[8], inBuf[9], inBuf[10], inBuf[11]);
                Katsayi3 = Converter.Byte4toUInt32(inBuf[12], inBuf[13], inBuf[14], inBuf[15]);
                Limit1 = Converter.Byte4toUInt32(inBuf[16], inBuf[17], inBuf[18], inBuf[19]);
                Limit2 = Converter.Byte4toUInt32(inBuf[20], inBuf[21], inBuf[22], inBuf[23]);
                AnaKredi = Converter.Byte4toInt32(inBuf[24], inBuf[25], inBuf[26], inBuf[27]);
                YedekKredi = Converter.Byte4toInt32(inBuf[28], inBuf[29], inBuf[30], inBuf[31]);
                AkoYko = inBuf[32];
                AnaKrediDurum = "";
                if (((AkoYko & 128) >> 7) == 0) { AnaKrediDurum = "YÜKLENDİ"; } else { AnaKrediDurum = "YÜKLENMEDİ"; }
                YedekKrediDurum = "";
                if (((AkoYko & 64) >> 6) == 0) { YedekKrediDurum = "YÜKLENDİ"; } else { YedekKrediDurum = "YÜKLENMEDİ"; }
                IslemNo = inBuf[33];
                KartNo = inBuf[34];
                Cap = inBuf[35];
                Tip = inBuf[36];
                DonemGun1 = inBuf[37];
                VanaPulseSure = inBuf[38];
                VanaCntSure = inBuf[39];
                Iade = inBuf[40];
                MaxdebiSiniri = inBuf[41];
                HaftaSonuOnay = inBuf[42];
                AboneNo = Converter.Byte4toUInt32(inBuf[44], inBuf[45], inBuf[46], inBuf[47]);
                Bayram1 = Converter.Byte2toUInt16(inBuf[48], inBuf[49]);
                Bayram2 = Converter.Byte2toUInt16(inBuf[50], inBuf[51]);
                KalanKredi = Converter.Byte4toInt32(inBuf[72], inBuf[73], inBuf[74], inBuf[75]);
                HarcananKredi = Converter.Byte4toUInt32(inBuf[76], inBuf[77], inBuf[78], inBuf[79]);
                GercekTuketim = Converter.Byte4toUInt32(inBuf[80], inBuf[81], inBuf[82], inBuf[83]);
                KademeTuketim1 = Converter.Byte4toUInt32(inBuf[84], inBuf[85], inBuf[86], inBuf[87]);
                KademeTuketim2 = Converter.Byte4toUInt32(inBuf[88], inBuf[89], inBuf[90], inBuf[91]);
                KademeTuketim3 = Converter.Byte4toUInt32(inBuf[92], inBuf[93], inBuf[94], inBuf[95]);
                SayacTarihi = Converter.TarihDuzenle(inBuf[96], inBuf[97]);
                NegatifTuketim = Converter.Byte4toUInt32(inBuf[100], inBuf[101], inBuf[102], inBuf[103]);
                DonemTuketimi = Converter.Byte4toUInt32(inBuf[104], inBuf[105], inBuf[106], inBuf[107]);
                DonemTuketimi1 = Converter.Byte4toUInt32(inBuf[108], inBuf[109], inBuf[110], inBuf[111]);
                DonemTuketimi2 = Converter.Byte4toUInt32(inBuf[112], inBuf[113], inBuf[114], inBuf[115]);
                DonemTuketimi3 = Converter.Byte4toUInt32(inBuf[116], inBuf[117], inBuf[118], inBuf[119]);
                DonemTuketimi4 = Converter.Byte4toUInt32(inBuf[120], inBuf[121], inBuf[122], inBuf[123]);
                DonemTuketimi5 = Converter.Byte4toUInt32(inBuf[124], inBuf[125], inBuf[126], inBuf[127]);
                DonemTuketimi6 = Converter.Byte4toUInt32(inBuf[128], inBuf[129], inBuf[130], inBuf[131]);
                SonKrediTarihi = Converter.TarihDuzenle(inBuf[132], inBuf[133]);
                SonPulseTarihi = Converter.TarihDuzenle(inBuf[134], inBuf[135]);
                SonCezaTarihi = Converter.TarihDuzenle(inBuf[136], inBuf[137]);
                SonArizaTarihi = Converter.TarihDuzenle(inBuf[138], inBuf[139]);
                BorcTarihi = Converter.TarihDuzenle(inBuf[140], inBuf[141]);
                VanaAcmaTarihi = Converter.TarihDuzenle(inBuf[142], inBuf[143]);
                VanaKapamaTarihi = Converter.TarihDuzenle(inBuf[144], inBuf[145]);
                Versiyon = inBuf[146];
                VanaOperasyonSayisi = inBuf[147];
                SayacDurumu = inBuf[148];
                SayacDurumu = Convert.ToByte((SayacDurumu & 128) + (SayacDurumu & 64) + (SayacDurumu & 32) + (SayacDurumu & 2) + (SayacDurumu & 1));
                SayacDurumAciklama = ((SayacDurumu & 1) == 1 ? "PULSE HATA, " : ""); // pulse hata
                SayacDurumAciklama += (((SayacDurumu & 2) >> 1) == 1 ? "ARIZA, " : "");  // arıza
                SayacDurumAciklama += (((SayacDurumu & 4) >> 2) == 1 ? "IPTAL, " : ""); // iptal
                SayacDurumAciklama += (((SayacDurumu & 128) >> 7) == 1 ? "CEZA1, " : ""); // Ceza1
                AnaPil = inBuf[149];
                DonemGun2 = inBuf[150];
                KartHata = inBuf[151];
                HaftaninGunu = inBuf[152];
                VanaDurumu = inBuf[153];
                ArizaTipi = inBuf[154];
                MaxdebiSeviyesi = inBuf[155];
                SonTakilanYetkiKartiOzellik1 = Converter.TarihDuzenle(inBuf[156], inBuf[157]) + " - " + inBuf[158].ToString() + " - " + inBuf[159].ToString();
                SonTakilanYetkiKartiOzellik2 = Converter.TarihDuzenle(inBuf[160], inBuf[161]) + " - " + inBuf[162].ToString() + " - " + inBuf[163].ToString();
                SonTakilanYetkiKartiOzellik3 = Converter.TarihDuzenle(inBuf[164], inBuf[165]) + " - " + inBuf[166].ToString() + " - " + inBuf[167].ToString();
                MaxDebiTarihi = Converter.TarihDuzenle(inBuf[168], inBuf[169]);
                DonemTuketimi7 = Converter.Byte4toUInt32(inBuf[170], inBuf[171], inBuf[172], inBuf[173]);
                DonemTuketimi8 = Converter.Byte4toUInt32(inBuf[174], inBuf[175], inBuf[176], inBuf[177]);
                DonemTuketimi9 = Converter.Byte4toUInt32(inBuf[178], inBuf[179], inBuf[180], inBuf[181]);
                DonemTuketimi10 = Converter.Byte4toUInt32(inBuf[182], inBuf[183], inBuf[184], inBuf[185]);
                DonemTuketimi11 = Converter.Byte4toUInt32(inBuf[186], inBuf[187], inBuf[188], inBuf[189]);
                DonemTuketimi12 = Converter.Byte4toUInt32(inBuf[190], inBuf[191], inBuf[192], inBuf[193]);
                DonemTuketimi13 = Converter.Byte2toUInt16(inBuf[194], inBuf[195]);
                DonemTuketimi14 = Converter.Byte2toUInt16(inBuf[196], inBuf[197]);
                DonemTuketimi15 = Converter.Byte2toUInt16(inBuf[198], inBuf[199]);
                DonemTuketimi16 = Converter.Byte2toUInt16(inBuf[200], inBuf[201]);
                DonemTuketimi17 = Converter.Byte2toUInt16(inBuf[202], inBuf[203]);
                DonemTuketimi18 = Converter.Byte2toUInt16(inBuf[204], inBuf[205]);
                DonemTuketimi19 = Converter.Byte2toUInt16(inBuf[206], inBuf[207]);
                DonemTuketimi20 = Converter.Byte2toUInt16(inBuf[208], inBuf[209]);
                DonemTuketimi21 = Converter.Byte2toUInt16(inBuf[210], inBuf[211]);
                DonemTuketimi22 = Converter.Byte2toUInt16(inBuf[212], inBuf[213]);
                DonemTuketimi23 = Converter.Byte2toUInt16(inBuf[214], inBuf[215]);
                DonemTuketimi24 = Converter.Byte2toUInt16(inBuf[216], inBuf[217]);
                Mektuk = Converter.Byte4toUInt32(inBuf[218], inBuf[219], inBuf[220], inBuf[221]);
                IadeKalan = Converter.Byte4toInt32(inBuf[222], inBuf[223], inBuf[224], inBuf[225]);
                ResetSayisi = inBuf[226];
            }
        }

        public struct Yetki
        {

        }

        public struct Uretim
        {

        }
    }
}
