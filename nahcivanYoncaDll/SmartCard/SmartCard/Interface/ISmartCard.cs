using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCard.Interface
{
    public interface ISmartCard
    {
        string AboneOku();

        string AboneYap(UInt32 devNo, UInt32 AboneNo, byte KartNo, byte Cap, byte Tip, byte Donem,
                              UInt32 Fiyat1, UInt32 Fiyat2, UInt32 Fiyat3, UInt32 Fiyat4, UInt32 Fiyat5,
                              UInt32 Limit1, UInt32 Limit2, UInt32 Limit3, UInt32 Limit4,
                              byte Bayram1Gunn, byte Bayram1Ayy, byte Bayram1Suree,
                              byte Bayram2Gunn, byte Bayram2Ayy, byte Bayram2Suree,
                              byte AvansOnayi, byte IleriSaat, byte YanginModuSuresi, byte MaxDebiSiniri, UInt32 KritikKredi, byte BaglantiPeriyot);
        //işlem id sistemden geliyor
        //string AboneYaz(UInt32 devNo, UInt32 AnaKredi,
        //                     byte Cap, byte Donem,
        //                     UInt32 Fiyat1, UInt32 Fiyat2, UInt32 Fiyat3, UInt32 Fiyat4, UInt32 Fiyat5,
        //                     UInt32 Limit1, UInt32 Limit2, UInt32 Limit3, UInt32 Limit4,
        //                     byte Bayram1Gunn, byte Bayram1Ayy, byte Bayram1Suree,
        //                     byte Bayram2Gunn, byte Bayram2Ayy, byte Bayram2Suree,
        //                     byte AvansOnayi, byte IleriSaat, byte YanginModuSuresi, byte MaxDebiSiniri, UInt16 Unique0);

        string AboneYaz(UInt32 devNo, UInt32 AnaKredi,
                          byte Cap, byte Donem,
                          UInt32 Fiyat1, UInt32 Fiyat2, UInt32 Fiyat3, UInt32 Fiyat4, UInt32 Fiyat5,
                          UInt32 Limit1, UInt32 Limit2, UInt32 Limit3, UInt32 Limit4,
                          byte Bayram1Gunn, byte Bayram1Ayy, byte Bayram1Suree,
                          byte Bayram2Gunn, byte Bayram2Ayy, byte Bayram2Suree,
                          byte AvansOnayi, byte IleriSaat, byte YanginModuSuresi, byte MaxDebiSiniri, UInt32 KritikKredi);



        //string AboneYap(AboneInfo abone);
        //string AboneYaz(AboneInfo abone);

        string KartBosaltKontrolsuz();

        string AboneBosalt();

        string KartTipi();
        string Eject();

        string KrediOku();

        string Iade(UInt32 devNo);

        string Versiyon();
    }
}
