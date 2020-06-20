using System;
using System.Collections.Generic;
using System.Text;

namespace Listener.Helpers
{
    public struct ByteToBit
    {
        public int Bit0, Bit1, Bit2, Bit3, Bit4, Bit5, Bit6, Bit7,
        Bit8, Bit9, Bit10, Bit11, Bit12, Bit13, Bit14, Bit15;
        public int Deger;
        public ByteToBit(int Alinacak)
        {
            Bit0 = Alinacak & 1;
            Bit1 = (Alinacak & 2) >> 1;
            Bit2 = (Alinacak & 4) >> 2;
            Bit3 = (Alinacak & 8) >> 3;
            Bit4 = (Alinacak & 16) >> 4;
            Bit5 = (Alinacak & 32) >> 5;
            Bit6 = (Alinacak & 64) >> 6;
            Bit7 = (Alinacak & 128) >> 7;
            Bit8 = (Alinacak & 256) >> 8;
            Bit9 = (Alinacak & 512) >> 9;
            Bit10 = (Alinacak & 1024) >> 10;
            Bit11 = (Alinacak & 2048) >> 11;
            Bit12 = (Alinacak & 4096) >> 12;
            Bit13 = (Alinacak & 8192) >> 13;
            Bit14 = (Alinacak & 16384) >> 14;
            Bit15 = (Alinacak & 32768) >> 15;
            Deger = Alinacak;
        }
    }
}
