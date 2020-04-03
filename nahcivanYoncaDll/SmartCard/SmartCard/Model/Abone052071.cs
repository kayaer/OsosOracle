using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCard.Model
{
    public struct Abone052071
    {
        public UInt32 Katsayi4, Katsayi5, Limit3, Limit4;
        public string KademeSayisi;

        private UInt32 Byte4toUInt32(byte bir, byte iki, byte uc, byte dort)
        {
            return (UInt32)((dort << 24) + (uc << 16) + (iki << 8) + bir);
        }

        public Abone052071(byte[] OkunanDegerler)
        {
            Katsayi4 = Katsayi5 = Limit3 = Limit4 = 0;
            if ((OkunanDegerler[2] == 0X55) && (OkunanDegerler[3] == 0XED))
                KademeSayisi = "5";
            else KademeSayisi = "3";

            Katsayi4 = Byte4toUInt32(OkunanDegerler[4], OkunanDegerler[5], OkunanDegerler[6], OkunanDegerler[7]);
            Katsayi5 = Byte4toUInt32(OkunanDegerler[8], OkunanDegerler[9], OkunanDegerler[10], OkunanDegerler[11]);
            Limit3 = Byte4toUInt32(OkunanDegerler[12], OkunanDegerler[13], OkunanDegerler[14], OkunanDegerler[15]);
            Limit4 = Byte4toUInt32(OkunanDegerler[16], OkunanDegerler[17], OkunanDegerler[18], OkunanDegerler[19]);
        }
    }
}
