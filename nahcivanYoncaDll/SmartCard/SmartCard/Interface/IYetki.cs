using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCard.Interface
{
    public interface IYetki
    {
        string YetkiHazirla();

        string YetkiAcma(UInt32 devNo, byte KontrolDegeri);

        string YetkiKapama(uint devNo, string Tarih, byte KontrolDegeri, byte KapatmaEmri);


        string YetkiIptal(UInt32 devNo);

        string BilgiYap();

        string BilgiOku();
    }
}
