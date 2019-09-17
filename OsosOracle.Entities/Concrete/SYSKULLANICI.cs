using OsosOracle.Framework.Entities;
using System;
namespace OsosOracle.Entities.Concrete
{
    public class SYSKULLANICI 
    {
        public int KAYITNO { get; set; }
        public string KULLANICIAD { get; set; }
        public string SIFRE { get; set; }
        public int BIRIMKAYITNO { get; set; }
        public int VERSIYON { get; set; }
        public int? OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int? GUNCELLEYEN { get; set; }
        public DateTime? GUNCELLEMETARIH { get; set; }
        public string AD { get; set; }
        public string SOYAD { get; set; }
        public int? GRUPKAYITNO { get; set; }
        public int DURUM { get; set; }
        public string SIFREKOD { get; set; }
        public int DIL { get; set; }
        public int KULLANICITUR { get; set; }
        public int KURUMKAYITNO { get; set; }
    }
}
