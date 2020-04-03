using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCard.Model
{
    public struct Abone072095
    {
        public Int32 KalanKredi;

        public UInt32 HarcananKredi, GercekTuketim, KademeTuketim1, KademeTuketim2, KademeTuketim3;

        private UInt32 Byte4toUInt32(byte bir, byte iki, byte uc, byte dort)
        {
            return (UInt32)((dort << 24) + (uc << 16) + (iki << 8) + bir);
        }

        private Int32 Byte4toInt32(byte bir, byte iki, byte uc, byte dort)
        {
            return ((dort << 24) + (uc << 16) + (iki << 8) + bir);
        }

        public Abone072095(byte[] OkunanDegerler)
        {
            KalanKredi = 0;
            HarcananKredi = GercekTuketim = KademeTuketim1 = KademeTuketim2 = KademeTuketim3 = 0;

            KalanKredi = Byte4toInt32(OkunanDegerler[0], OkunanDegerler[1], OkunanDegerler[2], OkunanDegerler[3]);
            HarcananKredi = Byte4toUInt32(OkunanDegerler[4], OkunanDegerler[5], OkunanDegerler[6], OkunanDegerler[7]);
            GercekTuketim = Byte4toUInt32(OkunanDegerler[8], OkunanDegerler[9], OkunanDegerler[10], OkunanDegerler[11]);
            KademeTuketim1 = Byte4toUInt32(OkunanDegerler[12], OkunanDegerler[13], OkunanDegerler[14], OkunanDegerler[15]);
            KademeTuketim2 = Byte4toUInt32(OkunanDegerler[16], OkunanDegerler[17], OkunanDegerler[18], OkunanDegerler[19]);
            KademeTuketim3 = Byte4toUInt32(OkunanDegerler[20], OkunanDegerler[21], OkunanDegerler[22], OkunanDegerler[23]);
        }
    }
}
