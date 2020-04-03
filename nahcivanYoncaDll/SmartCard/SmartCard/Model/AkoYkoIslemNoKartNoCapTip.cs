using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCard.Model
{
    public struct AkoYkoIslemNoKartNoCapTip
    {
        public byte AkoYko, IslemNo, KartNo, Cap, Tip, MaxDebiSiniri;
        public UInt32 AboneNo;

        private UInt32 Byte4toUInt32(byte bir, byte iki, byte uc, byte dort)
        {
            return (UInt32)((dort << 24) + (uc << 16) + (iki << 8) + bir);
        }

        public AkoYkoIslemNoKartNoCapTip(byte[] OkunanDegerler)
        {
            AkoYko = IslemNo = KartNo = Cap = Tip = MaxDebiSiniri = 0;
            AboneNo = 0;

            AkoYko = OkunanDegerler[0];
            IslemNo = OkunanDegerler[1];
            KartNo = OkunanDegerler[2];
            Cap = OkunanDegerler[3];
            Tip = OkunanDegerler[4];
            MaxDebiSiniri = OkunanDegerler[9];

            AboneNo = Byte4toUInt32(OkunanDegerler[12], OkunanDegerler[13], OkunanDegerler[14], OkunanDegerler[15]);
        }
    }
}
