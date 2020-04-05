
using OsosOracle.Framework.DataAccess.Filter;
using System;

namespace OsosOracle.Entities.ComplexType.ENTSATISComplexTypes
{
    public class ENTSATISAra
    {
        public Ara Ara { get; set; }
        public int? KAYITNO { get; set; }
        public string KAYITNOlar { get; set; }
        public int? ABONEKAYITNO { get; set; }
        public int? SAYACKAYITNO { get; set; }
        public int? FATURANO { get; set; }
        public int? ODEME { get; set; }
        public int? VERSIYON { get; set; }
        public int? IPTAL { get; set; }
        public int? KREDI { get; set; }
        public int? YEDEKKREDI { get; set; }
        public string SayacSeriNo { get; set; }

        public DateTime? SatisTarihBaslangic { get; set; }
        public DateTime? SatisTarihBitis { get; set; }
        public int? SatisTipi { get; set; }
        public int? KurumKayitNo { get; set; }

        public string Blok { get; set; }
        public bool AylikBakimBedeliOlanSatislariGetir { get; set; }
    }
}
