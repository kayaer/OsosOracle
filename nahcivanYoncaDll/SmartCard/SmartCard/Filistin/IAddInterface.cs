using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCard.Filistin
{
    public interface IAddInterface
    {
        #region Abone Kartı İşlemleri

        string AboneYaz(UInt32 devNo, Int32 AnaKredi, Int32 YedekKredi,
                              byte Cap, byte DonemGun,
                              UInt32 Fiyat1, UInt32 Fiyat2, UInt32 Fiyat3, UInt32 Fiyat4, UInt32 Fiyat5,
                              UInt32 Limit1, UInt32 Limit2, UInt32 Limit3, UInt32 Limit4,
                              byte Bayram1Gun, byte Bayram1Ay, byte Bayram1Sure,
                              byte Bayram2Gun, byte Bayram2Ay, byte Bayram2Sure,
                              byte AvansOnay, UInt16 Maxdebi, byte ZoneIndex, UInt32 FixCharge);

        string AboneOku(byte ZoneIndex);

        string KrediOku(byte ZoneIndex);

        string AboneYap(UInt32 CihazNo, UInt32 AboneNo, byte KartNo,
                              byte Cap, byte Tip, byte DonemGun,
                              UInt32 Fiyat1, UInt32 Fiyat2, UInt32 Fiyat3, UInt32 Fiyat4, UInt32 Fiyat5,
                              UInt32 Limit1, UInt32 Limit2, UInt32 Limit3, UInt32 Limit4,
                              byte Bayram1Gun, byte Bayram1Ay, byte Bayram1Sure,
                              byte Bayram2Gun, byte Bayram2Ay, byte Bayram2Sure,
                              byte AvansOnay, UInt16 Maxdebi, byte ZoneIndex, UInt32 FixCharge);

        string Iade(UInt32 devNo, byte ZoneIndex);

        string SatisIptal(UInt32 CihazNo,
                          Int32 AnaKredi, Int32 YedekKredi,
                          byte Ako, byte Yko,
                          byte ZoneIndex);

        string AboneBosalt(byte ZoneIndex);

        string Eject();

        #endregion

        string HataMesaji { get; set; }
        string KartTipi(byte ZoneIndex);
        string KartBosaltKontrolsuz();

        #region Yetki Kartı İşlemleri

        string YetkiHazirla(byte ZoneIndex);
        string YetkiSaat(DateTime date, byte DonemGun, byte ZoneIndex);
        string YetkiAcma(UInt32 CihazNo, byte ZoneIndex);
        string YetkiKapat(UInt32 CihazNo, DateTime Tarih, byte KontrolDegeri, byte KapatmaEmri, byte ZoneIndex);
        string YetkiKapama(UInt32 CihazNo, byte KapatmaGun, byte KapatmaAy, byte KapatmaYil, byte KapatmaSaat, byte KapamaEmri, byte ZoneIndex);
        string BilgiYap(byte ZoneIndex);
        string BilgiOku(byte ZoneIndex);
        //string YetkiIadeYap(UInt32 devNo);
        //string YetkiIadeOku();
        string YetkiIptal(UInt32 CihazNo, byte ZoneIndex);
        string ETrans(YetkiKarti data, byte ZoneIndex);
        string EDegis(YetkiKarti data, byte ZoneIndex);
        string YetkiBilgiOkuETrans(byte ZoneIndex, ref YetkiKarti data);
        string YetkiCeza4(byte Ceza4, byte ZoneIndex);
        string YetkiTuketim(byte ZoneIndex);
        string YetkiTuketimOku(byte ZoneIndex);
        string YetkiReset(UInt32 CihazNo, byte ZoneIndex);
        string YetkiHariciEE(byte ZoneIndex);
        string YetkiAvans(UInt32 CihazNo, UInt32 kat1, UInt32 kat2, UInt32 kat3, UInt32 lim1, UInt32 lim2, byte ZoneIndex);
        string YetkiBosalt(byte ZoneIndex);


        #endregion

        #region Üretim Fonksyonları

        string FormUret();
        string UretimFormat(UInt32 CihazNo,
                                   UInt32 KademeKatsayi1, UInt32 KademeKatsayi2, UInt32 KademeKatsayi3, UInt32 KademeKatsayi4, UInt32 KademeKatsayi5,
                                   UInt32 Limit1, UInt32 Limit2, UInt32 Limit3, UInt32 Limit4, UInt32 KritikKredi,
                                   byte DonemGun, byte VanaPulseSure, byte VanaCntSure, byte Cap);
        string UretimCihazNo(UInt32 CihazNo,
                                    UInt32 KademeKatsayi1, UInt32 KademeKatsayi2, UInt32 KademeKatsayi3, UInt32 KademeKatsayi4, UInt32 KademeKatsayi5,
                                    UInt32 Limit1, UInt32 Limit2, UInt32 Limit3, UInt32 Limit4, UInt32 KritikKredi,
                                    byte DonemGun, UInt32 MekanikTuketim, byte Cap, Byte ZoneIndex);
        string UretimAc(byte KontrolDegeri);
        string UretimKapat(byte KontrolDegeri);
        string UretimReelMod();
        string UretimTestMod(UInt32 CihazNo);
        string UretimSifirlama();
        string UretimZone(byte ZoneIndex);
        string UretimBosalt();

        #endregion

    }
}
