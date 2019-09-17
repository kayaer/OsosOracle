using System;
using OsosOracle.Framework.Entities;
namespace OsosOracle.Entities.Concrete
{
    public class PRMTARIFESU : IEntity
    {
        public int? BORCYUZDE { get; set; }
        public int KAYITNO { get; set; }
        public string AD { get; set; }
        public int YEDEKKREDI { get; set; }
        public string ACIKLAMA { get; set; }
        public int DURUM { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int? GUNCELLEYEN { get; set; }
        public DateTime? GUNCELLEMETARIH { get; set; }
        public int FIYAT1 { get; set; }
        public int FIYAT2 { get; set; }
        public int FIYAT3 { get; set; }
        public int FIYAT4 { get; set; }
        public int FIYAT5 { get; set; }
        public int LIMIT1 { get; set; }
        public int LIMIT2 { get; set; }
        public int LIMIT3 { get; set; }
        public int LIMIT4 { get; set; }
        public int TUKETIMKATSAYI { get; set; }
        public int KREDIKATSAYI { get; set; }
        public int SABITUCRET { get; set; }
        public int SAYACCAP { get; set; }
        public int AVANSONAY { get; set; }
        public int DONEMGUN { get; set; }
        public int BAYRAM1GUN { get; set; }
        public int BAYRAM1AY { get; set; }
        public int BAYRAM1SURE { get; set; }
        public int BAYRAM2GUN { get; set; }
        public int BAYRAM2AY { get; set; }
        public int BAYRAM2SURE { get; set; }
        public int MAXDEBI { get; set; }
        public int KRITIKKREDI { get; set; }
        public int KURUMKAYITNO { get; set; }
        public int BAGLANTIPERIYOT { get; set; }
        public int YANGINMODSURE { get; set; }
        public int BIRIMFIYAT { get; set; }
    }
}
