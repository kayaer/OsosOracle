using System;
using System.Collections.Generic;
using OsosOracle.Entities.ComplexType.SYSGOREVComplexTypes;
using OsosOracle.Entities.Concrete;
namespace OsosOracle.Entities.ComplexType.SYSROLComplexTypes
{
    public class SYSROLDetay
    {

        public int KAYITNO { get; set; }
        public string AD { get; set; }
        public string ACIKLAMA { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int GUNCELLEYEN { get; set; }
        public DateTime GUNCELLEMETARIH { get; set; }
        public int KURUMKAYITNO { get; set; }
        public List<SYSGOREVDetay> SysGorevList { get; set; }
        public string Kurum { get; set; }
    }

}