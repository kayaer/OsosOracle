using System;
using OsosOracle.Entities.Concrete;
namespace OsosOracle.Entities.ComplexType.ENTKOMUTLARBEKLEYENComplexTypes
{
    public class ENTKOMUTLARBEKLEYENDetay
    {

        public int KOMUTID { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int GUNCELLEYEN { get; set; }
        public DateTime GUNCELLEMETARIH { get; set; }
        public int VERSIYON { get; set; }
        public int BAGLANTIDENEMESAYISI { get; set; }
        public string ACIKLAMA { get; set; }
        public Guid GUIDID { get; set; }
        public string KONSSERINO { get; set; }
        public int KOMUTKODU { get; set; }
        public string KOMUT { get; set; }
        public DateTime ISLEMTARIH { get; set; }

    }

}