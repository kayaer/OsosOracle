using OsosOracle.Entities.ComplexType.ENTABONEComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.ENTABONEModels
{
    public class AboneBilgileriModel
    {
        public ENTABONEDetay AboneDetay { get; set; }
        public string ActiveTab { get; set; }
        public string ReturnUrl { get; set; }
        public int KurumKayitNo { get; set; }
    }
}