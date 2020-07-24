using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.Entities.Enums
{
    public enum enumDil
    {
        Ingilizce = 1,
        Turkce = 2,
        Arapca = 3
    }
    public enum enumKurum
    {
        Elektromed=1,
        YesilVadi=2,
        Mısır=3,
        Apex1=4
    }

    public enum enumSatisTipi
    {
        Satis=5,
        SatisIptal=6,
        BedelsizSatis=7
    }
    public enum enumSayacDurum
    {
        Aktif=8,
        Pasif=9
    }

    public enum enumOdemeTipi
    {
        Nakit=10,
        KrediKarti=11
    }
    public enum enumSayacModeli
    {
        Mercan = 1,
        Mcm = 2,
        As20 = 3,
        Kalorimetre = 4,
        Gaz = 5,
        MercanYd = 6,
        As25Yd = 7
    }

    public enum enumIsEmirleriDurum
    {
        Bekliyor = 12,
        Cevaplandi = 13,
        KomutGonderildi = 14
    }
    public enum enumIsEmirleri
    {
        VanaIslemi=15,//Veritabanı değerleri NEsneDeger tablosunda Deger kolonunda komut un protokol karşılığı var
        Reset=16,
        KrediYukle=17,
        ZamanlanmisGorevEkleme=18,
        ServerIpPortSet=19,
        ApnSet=20,
        YetkiAc=21
        
    }
}
