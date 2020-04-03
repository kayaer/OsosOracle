
using OsosOracle.Framework.DataAccess.Filter;
using System;

namespace OsosOracle.Entities.ComplexType.ENTABONEComplexTypes
{
    public class ENTABONEAra
    {
        public Ara Ara { get; set; }
        public int? KAYITNO { get; set; }
        public string KAYITNOlar { get; set; }
        public string AD { get; set; }
        public string SOYAD { get; set; }
        public int? DURUM { get; set; }
        public int? VERSIYON { get; set; }
        public int? KURUMKAYITNO { get; set; }

        public string AboneNo { get; set; }

        public string AboneNoVeyAdiVeyaSoyadi { get; set; }

        public int? SayacKayitNo { get; set; }
        public DateTime? SonSatisTarihiBaslangic { get; set; }
        public DateTime? SonSatisTarihBitis { get; set; }
        public string KimlikNo { get; set; }
    }
}
