
using System;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.Entities.ComplexType.EntIsEmriComplexTypes
{
    public class EntIsEmriAra
    {
        public Ara Ara { get; set; }
        public int? KayitNo { get; set; }

        public int? SayacKayitNo { get; set; }
        public int? IsEmriKayitNo { get; set; }
        public int? IsEmriDurumKayitNo { get; set; }
        public string IsEmriCevap { get; set; }
        public string KonsSeriNo { get; set; }
    }
}
