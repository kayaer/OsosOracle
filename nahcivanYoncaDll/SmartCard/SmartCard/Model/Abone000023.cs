using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCard.Model
{
    public struct Abone000023
    {
        public UInt32 CihazNo, Katsayi1, Katsayi2, Katsayi3, Limit1, Limit2;

        private UInt32 Byte4toUInt32(byte bir, byte iki, byte uc, byte dort)
        {
            return (UInt32)((dort << 24) + (uc << 16) + (iki << 8) + bir);
        }

        public Abone000023(byte[] OkunanDegerler)
        {
            CihazNo = Katsayi1 = Katsayi2 = Katsayi3 = Limit1 = Limit2 = 0;

            CihazNo = Byte4toUInt32(OkunanDegerler[0], OkunanDegerler[1], OkunanDegerler[2], OkunanDegerler[3]);
            Katsayi1 = Byte4toUInt32(OkunanDegerler[4], OkunanDegerler[5], OkunanDegerler[6], OkunanDegerler[7]);
            Katsayi2 = Byte4toUInt32(OkunanDegerler[8], OkunanDegerler[9], OkunanDegerler[10], OkunanDegerler[11]);
            Katsayi3 = Byte4toUInt32(OkunanDegerler[12], OkunanDegerler[13], OkunanDegerler[14], OkunanDegerler[15]);
            Limit1 = Byte4toUInt32(OkunanDegerler[16], OkunanDegerler[17], OkunanDegerler[18], OkunanDegerler[19]);
            Limit2 = Byte4toUInt32(OkunanDegerler[20], OkunanDegerler[21], OkunanDegerler[22], OkunanDegerler[23]);
        }
    }
}
