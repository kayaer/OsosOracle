using System;
using OsosOracle.Entities.Concrete;
namespace OsosOracle.Entities.ComplexType.CONKURUMComplexTypes
{
    public class CONKURUMDetay
    {

        public int KAYITNO { get; set; }
        public string AD { get; set; }
        public string ACIKLAMA { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int GUNCELLEYEN { get; set; }
        public DateTime GUNCELLEMETARIH { get; set; }
        public int SAYACURUNAILE { get; set; }
        public string DllAdi { get; set; }
    }

}