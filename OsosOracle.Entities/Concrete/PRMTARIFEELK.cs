using System;
using OsosOracle.Framework.Entities;
namespace OsosOracle.Entities.Concrete
{
    public class PRMTARIFEELK : IEntity
    {
        public int KREDIKATSAYI { get; set; }
        public int KAYITNO { get; set; }
        public string AD { get; set; }
        public int YEDEKKREDI { get; set; }
        public int KRITIKKREDI { get; set; }
        public int CARPAN { get; set; }
        public string ACIKLAMA { get; set; }
        public int DURUM { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int? GUNCELLEYEN { get; set; }
        public DateTime? GUNCELLEMETARIH { get; set; }
        public decimal FIYAT1 { get; set; }
        public decimal FIYAT2 { get; set; }
        public decimal FIYAT3 { get; set; }
        public decimal LIMIT1 { get; set; }
        public decimal LIMIT2 { get; set; }
        public int YUKLEMELIMIT { get; set; }
        public string SABAHSAAT { get; set; }
        public string AKSAMSAAT { get; set; }
        public string HAFTASONUAKSAM { get; set; }
        public int SABITUCRET { get; set; }
        public int BAYRAM1GUN { get; set; }
        public int BAYRAM1AY { get; set; }
        public int BAYRAM1SURE { get; set; }
        public int BAYRAM2GUN { get; set; }
        public int BAYRAM2AY { get; set; }
        public int BAYRAM2SURE { get; set; }
        public int KURUMKAYITNO { get; set; }
    }
}
