using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCard.Model
{
    public struct Abone144171
    {
        public string VanaKapamaTarihi;
        public byte Versiyon, VanaOperasyonSayisi, SayacDurumu, AnaPil, DonemGun, KartHata, HaftaninGunu, VanaDurumu, ArizaTipi, MaxdebiSeviyesi;
        public string SonTakilanYetkiKartiOzellik1, SonTakilanYetkiKartiOzellik2, SonTakilanYetkiKartiOzellik3, MaxDebiTarihi;

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
                return "01/01/2000";
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

            if ((Byte3 > 23) || (Byte4 > 59) || (DondurulecekData == "01/01/2000"))
            {
                return "01/01/2000 00:00";
            }
            else
            {
                DondurulecekData += " " + StringDuzenleBastan(Byte3.ToString(), 2) +
                                    ":" + StringDuzenleBastan(Byte4.ToString(), 2);

                return DondurulecekData;
            }

        }

        public Abone144171(byte[] OkunanDegerler)
        {
            VanaKapamaTarihi = MaxDebiTarihi = "";
            Versiyon = VanaOperasyonSayisi = SayacDurumu = AnaPil = DonemGun = KartHata = HaftaninGunu = VanaDurumu = ArizaTipi = MaxdebiSeviyesi = 0;
            SonTakilanYetkiKartiOzellik1 = SonTakilanYetkiKartiOzellik2 = SonTakilanYetkiKartiOzellik3 = "";

            VanaKapamaTarihi = TarihDuzenle(OkunanDegerler[0], OkunanDegerler[1]);
            Versiyon = OkunanDegerler[2];
            VanaOperasyonSayisi = OkunanDegerler[3];

            SayacDurumu = OkunanDegerler[4];
            AnaPil = OkunanDegerler[5];
            DonemGun = OkunanDegerler[6];
            KartHata = OkunanDegerler[7];
            HaftaninGunu = OkunanDegerler[8];
            VanaDurumu = OkunanDegerler[9];
            ArizaTipi = OkunanDegerler[10];
            MaxdebiSeviyesi = OkunanDegerler[11];

            SonTakilanYetkiKartiOzellik1 = TarihDuzenle(OkunanDegerler[12], OkunanDegerler[13]) + " - " +
                                           OkunanDegerler[14].ToString() + " - " + OkunanDegerler[15].ToString();
            SonTakilanYetkiKartiOzellik2 = TarihDuzenle(OkunanDegerler[16], OkunanDegerler[17]) + " - " +
                                           OkunanDegerler[18].ToString() + " - " + OkunanDegerler[19].ToString();
            SonTakilanYetkiKartiOzellik3 = TarihDuzenle(OkunanDegerler[20], OkunanDegerler[21]) + " - " +
                                           OkunanDegerler[22].ToString() + " - " + OkunanDegerler[23].ToString();

            MaxDebiTarihi = TarihDuzenle(OkunanDegerler[24], OkunanDegerler[25]);
        }
    }
}
