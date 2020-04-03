using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCard
{
    public interface IKartOkuyucu
    {
        byte[] InBuf { get; set; }
        byte[] OutBuf { get; set; }

        int InitCard();
        string FinishCard();

        string ATR { get; }

        int ReadCard(UInt16 adres, byte Adet);
        int VerifyCard(byte adres);
        int UpdateCard(UInt16 adres);
        int Eject();
    }
}
