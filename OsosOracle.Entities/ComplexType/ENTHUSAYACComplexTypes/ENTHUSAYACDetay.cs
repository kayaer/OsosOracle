using System;
using OsosOracle.Entities.Concrete;
namespace OsosOracle.Entities.ComplexType.ENTHUSAYACComplexTypes
{
    public class ENTHUSAYACDetay
    {

        public int KAYITNO { get; set; }
        public int HUKAYITNO { get; set; }
        public int SAYACKAYITNO { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int GUNCELLEYEN { get; set; }
        public DateTime GUNCELLEMETARIH { get; set; }

        public string SAYACID { get; set; }

    }

}