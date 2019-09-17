using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.Entities.ComplexType.ENTABONEComplexTypes
{
    public class AboneAutoComplete
    {
        public int KayitNo { get; set; }
        public string AboneNo { get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }

        public string AutoCompleteStr => Adi + " " + Soyadi + " Abone No: " + AboneNo;
    }
}
