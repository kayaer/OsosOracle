using OsosOracle.Entities.ComplexType.ENTABONEComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.ENTABONEModels
{
    public class AboneGenelPartialModel
    {
        public int AboneKayitNo { get; set; }
        public string ReturnUrl { get; set; }
     
        public AboneGenel AboneGenel { get; set; }
    }
}