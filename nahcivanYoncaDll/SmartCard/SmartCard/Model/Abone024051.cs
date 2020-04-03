using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCard.Model
{
    public struct Abone024051
    {
        public uint AnaKredi, YedekKredi, AboneNo;
        public byte AkoYko, IslemNo, KartNo, Cap, Tip, DonemGun, VanaPulseSure, VanaCntSure, Iade,
        MaxdebiSiniri, HaftaSonuOnay, bos;
        public UInt16 Bayram1, Bayram2;

        private UInt16 Byte2toUInt16(byte bir, byte iki)
        {
            return Convert.ToUInt16((iki << 8) + bir);

        }

        private UInt32 Byte4toUInt32(byte bir, byte iki, byte uc, byte dort)
        {
            return (UInt32)((dort << 24) + (uc << 16) + (iki << 8) + bir);
        }

        private uint Byte4toInt32(byte bir, byte iki, byte uc, byte dort)
        {
            return (uint)((dort << 24) + (uc << 16) + (iki << 8) + bir);
        }

        public Abone024051(byte[] OkunanDegerler)
        {
            AnaKredi = YedekKredi = AboneNo = 0;
            AkoYko = IslemNo = KartNo = Cap = Tip = DonemGun = VanaPulseSure = VanaCntSure = Iade =
            MaxdebiSiniri = HaftaSonuOnay = bos = 0;
            Bayram1 = Bayram2 = 0;

            AnaKredi = Byte4toInt32(OkunanDegerler[0], OkunanDegerler[1], OkunanDegerler[2], OkunanDegerler[3]);
            YedekKredi = Byte4toInt32(OkunanDegerler[4], OkunanDegerler[5], OkunanDegerler[6], OkunanDegerler[7]);
            AkoYko = OkunanDegerler[8];
            IslemNo = OkunanDegerler[9];
            KartNo = OkunanDegerler[10];
            Cap = OkunanDegerler[11];
            Tip = OkunanDegerler[12];
            DonemGun = OkunanDegerler[13];
            VanaPulseSure = OkunanDegerler[14];
            VanaCntSure = OkunanDegerler[15];
            Iade = OkunanDegerler[16];
            MaxdebiSiniri = OkunanDegerler[17];
            HaftaSonuOnay = OkunanDegerler[18];
            Bayram1 = Byte2toUInt16(OkunanDegerler[24], OkunanDegerler[25]);
            Bayram2 = Byte2toUInt16(OkunanDegerler[26], OkunanDegerler[27]);
        }
    }
}
