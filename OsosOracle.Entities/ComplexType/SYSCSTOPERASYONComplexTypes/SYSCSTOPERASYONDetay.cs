using System;
using OsosOracle.Entities.Concrete;
namespace OsosOracle.Entities.ComplexType.SYSCSTOPERASYONComplexTypes
{
    public class SYSCSTOPERASYONDetay
    {

        public int KAYITNO { get; set; }
        public string AD { get; set; }
        public string ACIKLAMA { get; set; }
        public int OPERASYONTUR { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int GUNCELLEYEN { get; set; }
        public DateTime GUNCELLEMETARIH { get; set; }
        public int MENUKAYITNO { get; set; }
        public string Menu { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }

}