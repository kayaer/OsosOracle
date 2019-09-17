using System;
using OsosOracle.Framework.Entities;
namespace OsosOracle.Entities.Concrete
{
    public class ENTKOMUTLARSONUCLANAN : IEntity
    {
        public int KAYITNO { get; set; }
        public string KONSSERINO { get; set; }
        public int KOMUTKODU { get; set; }
        public string KOMUT { get; set; }
        public DateTime ISLEMTARIH { get; set; }
        public string SONUC { get; set; }
        public int ISLEMSURESI { get; set; }
        public int KOMUTID { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int? GUNCELLEYEN { get; set; }
        public DateTime? GUNCELLEMETARIH { get; set; }
        public int VERSIYON { get; set; }
        public string ACIKLAMA { get; set; }
        public string CEVAP { get; set; }
    }
}
