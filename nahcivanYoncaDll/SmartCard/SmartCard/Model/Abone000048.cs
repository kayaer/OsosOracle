using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCard.Model
{
    public struct Abone000048
    {
        public UInt16 unique0, unique1, unique2, unique3, unique4, unique5;
        public UInt16 krediTarihi1, krediTarihi2, krediTarihi3, krediTarihi4, krediTarihi5;
        public Int32 kredi1, kredi2, kredi3, kredi4, kredi5;
        public byte ako0, ako1, ako2, ako3, ako4, ako5;

        public Abone000048(byte[] OkunanDegerler)
        {
            unique0 = unique1 = unique2 = unique3 = unique4 = unique5 = 0;
            krediTarihi1 = krediTarihi2 = krediTarihi3 = krediTarihi4 = krediTarihi5 = 0;
            kredi1 = kredi2 = kredi3 = kredi4 = kredi5 = 0;
            ako0 = ako1 = ako2 = ako3 = ako4 = ako5 = (byte)'*';

            unique0 = Hexcon.Byte2toUInt16(OkunanDegerler[0], OkunanDegerler[1]);
            ako0 = OkunanDegerler[2];

            unique1 = Hexcon.Byte2toUInt16(OkunanDegerler[3], OkunanDegerler[4]);
            kredi1 = Hexcon.Byte4toInt32(OkunanDegerler[5], OkunanDegerler[6], OkunanDegerler[7], OkunanDegerler[8]);
            krediTarihi1 = Hexcon.Byte2toUInt16(OkunanDegerler[9], OkunanDegerler[10]);
            ako1 = OkunanDegerler[11];

            unique2 = Hexcon.Byte2toUInt16(OkunanDegerler[12], OkunanDegerler[13]);
            kredi2 = Hexcon.Byte4toInt32(OkunanDegerler[14], OkunanDegerler[15], OkunanDegerler[16], OkunanDegerler[17]);
            krediTarihi2 = Hexcon.Byte2toUInt16(OkunanDegerler[18], OkunanDegerler[19]);
            ako2 = OkunanDegerler[20];

            unique3 = Hexcon.Byte2toUInt16(OkunanDegerler[21], OkunanDegerler[22]);
            kredi3 = Hexcon.Byte4toInt32(OkunanDegerler[23], OkunanDegerler[24], OkunanDegerler[25], OkunanDegerler[26]);
            krediTarihi3 = Hexcon.Byte2toUInt16(OkunanDegerler[27], OkunanDegerler[28]);
            ako3 = OkunanDegerler[29];

            unique4 = Hexcon.Byte2toUInt16(OkunanDegerler[30], OkunanDegerler[31]);
            kredi4 = Hexcon.Byte4toInt32(OkunanDegerler[32], OkunanDegerler[33], OkunanDegerler[34], OkunanDegerler[35]);
            krediTarihi4 = Hexcon.Byte2toUInt16(OkunanDegerler[36], OkunanDegerler[37]);
            ako4 = OkunanDegerler[38];

            unique5 = Hexcon.Byte2toUInt16(OkunanDegerler[39], OkunanDegerler[40]);
            kredi5 = Hexcon.Byte4toInt32(OkunanDegerler[41], OkunanDegerler[42], OkunanDegerler[43], OkunanDegerler[44]);
            krediTarihi5 = Hexcon.Byte2toUInt16(OkunanDegerler[45], OkunanDegerler[46]);
            ako5 = OkunanDegerler[47];
        }
    }
}
