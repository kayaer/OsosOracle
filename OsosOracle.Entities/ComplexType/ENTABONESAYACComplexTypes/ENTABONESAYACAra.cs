
using OsosOracle.Framework.DataAccess.Filter;
using System;

namespace OsosOracle.Entities.ComplexType.ENTABONESAYACComplexTypes
{
    public class ENTABONESAYACAra
    {
        public Ara Ara { get; set; }
        public int? KAYITNO { get; set; }
        public string KAYITNOlar { get; set; }
        public int? ABONEKAYITNO { get; set; }
        public int? SAYACKAYITNO { get; set; }
        public string SOKULMENEDEN { get; set; }
        public int? SONENDEKS { get; set; }
        public DateTime? SOKULMETARIH { get; set; }
        public int? KARTNO { get; set; }
        public int? VERSIYON { get; set; }
        public int? TARIFEKAYITNO { get; set; }
        public int? TARIFE { get; set; }
        public DateTime? TAKILMATARIH { get; set; }
        public int? SONSATISKAYITNO { get; set; }
        public DateTime? SONSATISTARIH { get; set; }

        public int? SayacModelKayitNo { get; set; }
        public int? SayacTur { get; set; }
        public int? Durum { get; set; }
        public string SayacSeriNo { get; set; }
        public string AboneAdi { get; set; }
        public int? KurumKayitNo { get; set; }
        public string SonSatisTarihBaslangic { get; set; }
        public string SonSatisTarihBitis { get; set; }
    }
}
