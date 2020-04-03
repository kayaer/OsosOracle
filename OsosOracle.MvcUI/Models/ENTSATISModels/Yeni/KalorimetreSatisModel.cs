using OsosOracle.Entities.ComplexType.ENTABONESAYACComplexTypes;
using OsosOracle.Entities.ComplexType.PRMTARIFEKALORIMETREComplexTypes;
using OsosOracle.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.ENTSATISModels.Yeni
{
    public class KalorimetreSatisModel
    {
        public KalorimetreSatisModel()
        {
            Satis = new ENTSATIS();
            SatisIptal = new SatisIptal();
        }
        public ENTSATIS Satis { get; set; }
        public PRMTARIFEKALORIMETREDetay PrmTarifeKALORIMETREDetay { get; set; }
        public KalorimetreOkunan KalorimetreOkunan { get; set; }
        public ENTABONESAYACDetay AboneSayacDetay { get; set; }

        public SatisIptal SatisIptal { get; set; }
    }
}