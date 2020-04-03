using OsosOracle.Framework.Entities;
using System;
namespace OsosOracle.Entities.Concrete
{
    public class ENTSATIS : IEntity
    {
        public int KAYITNO { get; set; }
        public int ABONEKAYITNO { get; set; }
        public int SAYACKAYITNO { get; set; }
        public int? FATURANO { get; set; }
        public decimal ODEME { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int? GUNCELLEYEN { get; set; }
        public DateTime? GUNCELLEMETARIH { get; set; }
        public int? IPTAL { get; set; }
        public decimal? KREDI { get; set; }
        public int? YEDEKKREDI { get; set; }

        public int SatisTipi { get; set; }
        public decimal AylikBakimBedeli { get; set; }
        public decimal Kdv { get; set; }
        public decimal Ctv { get; set; }
        public decimal? ToplamKredi { get; set; }
        public decimal? SatisTutari { get; set; }

        public int? OdemeTipiKayitNo { get; set; }
    }
}
