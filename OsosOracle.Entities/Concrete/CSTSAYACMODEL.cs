using System;
namespace OsosOracle.Entities.Concrete
{
    public class CSTSAYACMODEL 
    {
        public int KAYITNO { get; set; }
        public string AD { get; set; }
        public string ACIKLAMA { get; set; }
        public int? DURUM { get; set; }
        public int? VERSIYON { get; set; }
        public int? OLUSTURAN { get; set; }
        public DateTime? OLUSTURMATARIH { get; set; }
        public int? GUNCELLEYEN { get; set; }
        public DateTime? GUNCELLEMETARIH { get; set; }
        public int? MARKAKAYITNO { get; set; }
        public string YAZILIMVERSIYON { get; set; }
        public string FLAG { get; set; }
        public string CONTROLLER { get; set; }
    }
}
