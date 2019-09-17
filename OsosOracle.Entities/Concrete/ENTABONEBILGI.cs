using OsosOracle.Framework.Entities;
using System;
namespace OsosOracle.Entities.Concrete
{
    public class ENTABONEBILGI : IEntity
    {
        public int? BORC { get; set; }
        public int? TELEFON { get; set; }
        public int? IKIM3BILGI { get; set; }
        public int? BESM3BILGI { get; set; }
        public int? KIMLIKNO { get; set; }
        public int? BLOKE { get; set; }
        public string BLOKEACIKLAMA { get; set; }
        public int KAYITNO { get; set; }
        public string ABONENO { get; set; }
        public int ABONEKAYITNO { get; set; }
        public string EPOSTA { get; set; }
        public int? GSM { get; set; }
        public string ADRES { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int? GUNCELLEYEN { get; set; }
        public DateTime? GUNCELLEMETARIH { get; set; }
    }
}
