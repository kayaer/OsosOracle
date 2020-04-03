using System;
using OsosOracle.Framework.Entities;
namespace OsosOracle.Entities.Concrete
{
    public class PRMTARIFEGAZ : IEntity
    {
        public int KAYITNO { get; set; }
        public int VERSIYON { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int OLUSTURAN { get; set; }
        public int? GUNCELLEYEN { get; set; }
        public DateTime? GUNCELLEMETARIH { get; set; }
        public int TUKETIMLIMIT { get; set; }
        public string SABAHSAAT { get; set; }
        public string AKSAMSAAT { get; set; }
        public int PULSE { get; set; }
        public int BAYRAM1GUN { get; set; }
        public int BAYRAM1AY { get; set; }
        public int BAYRAM1SURE { get; set; }
        public int BAYRAM2GUN { get; set; }
        public int BAYRAM2SURE { get; set; }
        public int BAYRAM2AY { get; set; }
        public int KRITIKKREDI { get; set; }
        public int SAYACTUR { get; set; }
        public int SAYACCAP { get; set; }
        public string AD { get; set; }
        public int FIYAT1 { get; set; }
        public int YEDEKKREDI { get; set; }
        public int KURUMKAYITNO { get; set; }
        public int BIRIMFIYAT { get; set; }


        public int TuketimKatsayi { get; set; }
        public decimal Kdv { get; set; }
        public decimal Ctv { get; set; }
    }
}
