using OsosOracle.Framework.Entities;
using System;
namespace OsosOracle.Entities.Concrete
{
    public class ENTABONE : IEntity,IEntitySoftDelete
    {
        public int KAYITNO { get; set; }
        public string AD { get; set; }
        public string SOYAD { get; set; }
        public int DURUM { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int? GUNCELLEYEN { get; set; }
        public DateTime? GUNCELLEMETARIH { get; set; }
        public int KURUMKAYITNO { get; set; }
        public string ABONENO { get; set; }
        public DateTime? SonSatisTarih { get; set; }
        public string KimlikNo { get; set; }
        public string Daire { get; set; }
        public string Blok { get; set; }
        public string Adres { get; set; }
    }
}
