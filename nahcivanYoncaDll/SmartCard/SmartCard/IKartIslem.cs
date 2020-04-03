using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCard
{
    public interface IKartIslem
    {
        //string Zone { get; set; }

        //IKartOkuyucu KartOkuyucu { get; set; }

        string HataMesaji { get; set; }

        #region Abone Kartı İşlemleri

        string KartTipi();

        string AboneYazElk(UInt32 CihazNo,
                           Int32 ElkAnaKredi, byte ElkYedekKredi,
                           UInt32 ElkFiyat1, UInt32 ElkFiyat2, UInt32 ElkFiyat3, UInt32 ElkFiyat4);

        string AboneYazSu(UInt32 CihazNo,
                          Int32 SuAnaKredi, byte SuYedekKredi,
                          UInt16 SuLimit1, UInt16 SuLimit2,
                          UInt32 SuFiyat1, UInt32 SuFiyat2, UInt32 SuFiyat3);

        string AboneYazSicakSu(UInt32 CihazNo,
                               Int32 SicakSuAnaKredi, byte SicakSuYedekKredi);

        string AboneYazGaz(UInt32 CihazNo,
                           Int32 GazAnaKredi, byte GazYedekKredi);

        string AboneOku();
        string KrediOku();

        string AboneYap(UInt32 CihazNo, byte KartNo,
                        UInt16 SuLimit1, UInt16 SuLimit2,
                        UInt32 SuFiyat1, UInt32 SuFiyat2, UInt32 SuFiyat3,
                        UInt32 ElkFiyat1, UInt32 ElkFiyat2, UInt32 ElkFiyat3, UInt32 ElkFiyat4);
        string Bosalt();
        string Eject();

        #endregion

        #region Yetki Kartı İşlemleri

        string FormYet(UInt32 CihazNo);
        string YetkiSaat();
        string YetkiSaat(DateTime date);
        string YetkiAc(UInt32 CihazNo);
        string YetkiKapat(UInt32 CihazNo);
        string YetkiBilgiYap();
        string YetkiBilgiOkuTip();
        string YetkiBilgiOkuElk();
        string YetkiBilgiOkuGaz();
        string YetkiIadeYap(byte SayacTip, UInt32 CihazNo);
        string YetkiIadeOku();
        string EDegis(UInt32 kalan, UInt32 harcanan);
        string ETrans(UInt32 CihazNo, DateTime kapamaTarih, UInt32 gerTuk, UInt32 kalan, UInt32 harcanan, byte kartNo, byte islemNo,
                     byte kademe, byte loadLimit, byte aksam, byte sabah, byte haftasonuAksam, UInt32 donemTuketim,
                     UInt32 fixCharge, UInt32 totalFixCharge, UInt32 k1Tuk, UInt32 k2Tuk, UInt32 k3Tuk, UInt32 Lim1, UInt32 Lim2,
                     UInt32 kad1, UInt32 kad2, UInt32 kad3);

        #endregion

        #region Üretim Fonksyonları

        string FormUret();
        string Format(uint devNo, uint KritikKredi, uint Kat1, uint Kat2, uint Kat3, uint Lim1, uint Lim2, uint OverLim, UInt16 LowVoltage, UInt16 HighVoltage);
        string ReelMod();
        string TestMod(UInt32 devNo);
        string CihazNo(uint devNo, uint KritikKredi, uint Kat1, uint Kat2, uint Kat3, uint Lim1, uint Lim2, uint OverLim, UInt16 LowVoltage, UInt16 HighVoltage);
        string UretimAc(Int32 Acma);
        string FormIssuer(UInt32 devNo);
        string Issuer();

        #endregion

    }
}
