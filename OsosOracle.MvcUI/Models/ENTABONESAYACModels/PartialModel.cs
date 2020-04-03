using OsosOracle.Entities.ComplexType.ENTABONESAYACComplexTypes;
using OsosOracle.Entities.ComplexType.ENTSATISComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.ENTABONESAYACModels
{
    public class PartialModel
    {
        public int AboneKayitNo { get; set; }
        public string ReturnUrl { get; set; }
        public ENTABONESAYACAra EntAboneSayacAra { get; set; }
        public bool YeniKayitBtnVisible { get; set; }
        public bool GuncelleBtnVisible { get; set; }
        public bool SilBtnVisible { get; set; }

        public ENTSATISAra EntSatisAra { get; set; }
    }
}