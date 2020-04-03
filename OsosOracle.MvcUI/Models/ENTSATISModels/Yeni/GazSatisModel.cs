using OsosOracle.Entities.ComplexType.ENTABONESAYACComplexTypes;
using OsosOracle.Entities.ComplexType.PRMTARIFEGAZComplexTypes;
using OsosOracle.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.ENTSATISModels.Yeni
{
    public class GazSatisModel
    {
        public GazSatisModel()
        {
            Satis = new ENTSATIS();
            SatisIptal = new SatisIptal();
            GazOkunan = new GazOkunan();
        }
        public ENTSATIS Satis { get; set; }
        public PRMTARIFEGAZDetay PrmTarifeGazDetay { get; set; }
        public GazOkunan GazOkunan { get; set; }
        public ENTABONESAYACDetay AboneSayacDetay { get; set; }

        public SatisIptal SatisIptal { get; set; }
    }
}