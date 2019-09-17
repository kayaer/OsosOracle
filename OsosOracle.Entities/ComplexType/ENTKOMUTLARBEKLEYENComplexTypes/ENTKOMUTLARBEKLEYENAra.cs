
using System;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.Entities.ComplexType.ENTKOMUTLARBEKLEYENComplexTypes
{
    public class ENTKOMUTLARBEKLEYENAra
    {
        public Ara Ara { get; set; }
        public int? KAYITNO { get; set; }
        public string KAYITNOlar { get; set; }
        public int? KOMUTID { get; set; }
        public int? BAGLANTIDENEMESAYISI { get; set; }
        public string ACIKLAMA { get; set; }
        public Guid GUIDID { get; set; }
        public string KONSSERINO { get; set; }
        public int? KOMUTKODU { get; set; }
        public string KOMUT { get; set; }
        public DateTime? ISLEMTARIH { get; set; }
    }
}
