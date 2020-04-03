using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCard.Model
{
    public struct Abone120143
    {
        public UInt32 DonemTuketimi4, DonemTuketimi5, DonemTuketimi6;
        public string SonKrediTarihi, SonPulseTarihi, SonCezaTarihi, SonArizaTarihi, BorcTarihi, VanaAcmaTarihi;
        public UInt16 SonKrediTarihiForLogs;
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

        public Abone120143(byte[] OkunanDegerler)
        {
            DonemTuketimi4 = DonemTuketimi5 = DonemTuketimi6 = 0;
            SonKrediTarihiForLogs = 0;
            SonKrediTarihi = SonPulseTarihi = SonCezaTarihi = SonArizaTarihi = BorcTarihi = VanaAcmaTarihi = "";

            DonemTuketimi4 = Byte4toUInt32(OkunanDegerler[0], OkunanDegerler[1], OkunanDegerler[2], OkunanDegerler[3]);
            DonemTuketimi5 = Byte4toUInt32(OkunanDegerler[4], OkunanDegerler[5], OkunanDegerler[6], OkunanDegerler[7]);
            DonemTuketimi6 = Byte4toUInt32(OkunanDegerler[8], OkunanDegerler[9], OkunanDegerler[10], OkunanDegerler[11]);

            SonKrediTarihi = TarihDuzenle(OkunanDegerler[12], OkunanDegerler[13]);
            SonKrediTarihiForLogs = Hexcon.Byte2toUInt16(OkunanDegerler[12], OkunanDegerler[13]);
            SonPulseTarihi = TarihDuzenle(OkunanDegerler[14], OkunanDegerler[15]);
            SonCezaTarihi = TarihDuzenle(OkunanDegerler[16], OkunanDegerler[17]);
            SonArizaTarihi = TarihDuzenle(OkunanDegerler[18], OkunanDegerler[19]);
            BorcTarihi = TarihDuzenle(OkunanDegerler[20], OkunanDegerler[21]);
            VanaAcmaTarihi = TarihDuzenle(OkunanDegerler[22], OkunanDegerler[23]);
        }
    }

}
