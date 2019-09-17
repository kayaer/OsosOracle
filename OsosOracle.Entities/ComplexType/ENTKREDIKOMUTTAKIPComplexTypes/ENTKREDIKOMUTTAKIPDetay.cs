using System;
using OsosOracle.Entities.Concrete;
namespace OsosOracle.Entities.ComplexType.ENTKREDIKOMUTTAKIPComplexTypes
{
    public class ENTKREDIKOMUTTAKIPDetay
    {

        public Guid GUIDID { get; set; }
        public string KONSSERINO { get; set; }
        public int? SATISKAYITNO { get; set; }
        public int? ISLEMID { get; set; }
        public int? INTERNET { get; set; }
        public int? KOMUTKODU { get; set; }
        public string KOMUT { get; set; }
        public int? KREDI { get; set; }
        public DateTime? ISLEMTARIH { get; set; }
        public int? KOMUTID { get; set; }
        public string ACIKLAMA { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int GUNCELLEYEN { get; set; }
        public DateTime GUNCELLEMETARIH { get; set; }
        public int? SAYACSERINO { get; set; }
        public string DURUM { get; set; }
        public DateTime MAKBUZTARIH { get; set; }
        public int MAKBUZNO { get; set; }
        public int ABONENO { get; set; }

    }

}