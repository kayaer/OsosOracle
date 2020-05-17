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
        Mısır=3
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
}
