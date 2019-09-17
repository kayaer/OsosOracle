using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SmartCard
{
    public interface IKart
    {
        #region Abone Kartı İşlemleri

        string KrediYaz(uint devNo, uint AnaKredi, uint YedekKredi, ITarife trf, byte Bayram1Gunn, byte Bayram1Ayy, byte Bayram1Suree, byte Bayram2Gunn, byte Bayram2Ayy, byte Bayram2Suree, uint KritikKredi);
        string AboneOku();
        string KrediOku();

        string AboneYap(UInt32 devNo, ITarife trf, byte Bayram1Gunn, byte Bayram1Ayy, byte Bayram1Suree, byte Bayram2Gunn, byte Bayram2Ayy, byte Bayram2Suree, uint KritikKredi);
        string AboneBosalt();
        
        string SifreExeHazirla();
        string KartSifreBosalt();
        string KartTipi();

        string Eject();

        #endregion

        #region Yetki Kartı İşlemleri

        string FormYet();
        string YetkiBosalt();
        string YetkiSaat();
        string YetkiSaat(DateTime date);
        string YetkiAc(UInt32 devNo);
        string YetkiKapat(UInt32 devNo);
        string YetkiBilgiYap();
        string YetkiBilgiOku();
        string YetkiIadeYap(UInt32 devNo);
        string YetkiIadeOku();
        string YetkiIptal(UInt32 devNo);
        string ETrans(UInt32 devNo, DateTime kapamaTarih, UInt32 gerTuk, UInt32 kalan, UInt32 harcanan, byte kartNo, byte islemNo,
                     byte kademe, byte loadLimit, byte aksam, byte sabah, byte haftasonuAksam, UInt32 donemTuketim,
                     UInt32 fixCharge, UInt32 totalFixCharge, UInt32 k1Tuk, UInt32 k2Tuk, UInt32 k3Tuk, UInt32 Lim1, UInt32 Lim2,
                     UInt32 kad1, UInt32 kad2, UInt32 kad3);
        string EDegis(UInt32 devNo, DateTime kapamaTarih, UInt32 gerTuk, UInt32 kalan, UInt32 harcanan, byte kartNo, byte islemNo,
                             byte kademe, byte loadLimit, byte aksam, byte sabah, byte haftasonuAksam, UInt32 donemTuketim,
                             UInt32 fixCharge, UInt32 totalFixCharge, UInt32 k1Tuk, UInt32 k2Tuk, UInt32 k3Tuk, UInt32 Lim1, UInt32 Lim2,
                             UInt32 kad1, UInt32 kad2, UInt32 kad3, UInt32 kritikKredi);

        string YetkiAvans(UInt32 devNo, UInt32 _Avans_Limiti);

        #endregion

        #region Üretim Fonksyonları

        string FormUret();
        string UretimBosalt();
        string Format(UInt32 devNo, UInt32 KritikKredi, UInt32 Kat1, UInt32 Kat2, UInt32 Kat3, UInt32 Lim1, UInt32 Lim2, UInt32 OverLim);
        string ReelMod();
        string TestMod(UInt32 devNo);
        string CihazNo(UInt32 devNo, UInt32 KritikKredi, UInt16 CokUcuz, UInt16 Ucuz, UInt16 Normal, UInt16 Pahali);
        string UretimAc(Int32 Acma);
        string FormIssuer(UInt32 devNo);
        string Issuer();

        #endregion
       
    }
}
