using OsosOracle.Entities.ComplexType.ENTABONESAYACComplexTypes;
using OsosOracle.Entities.ComplexType.PRMTARIFESUComplexTypes;
using OsosOracle.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.ENTSATISModels.Yeni
{
    public class SuSatisModel
    {
        public SuSatisModel()
        {
            Satis = new ENTSATIS();
            SatisIptal = new SatisIptal();
        }
        public ENTSATIS Satis { get; set; }
        public PRMTARIFESUDetay PrmTarifeSuDetay { get; set; }
        public SogukSuOkunan SogukSuOkunan { get; set; }
        public ENTABONESAYACDetay AboneSayacDetay { get; set; }

        public SatisIptal SatisIptal { get; set; }
    }
}