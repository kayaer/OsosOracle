using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Reports.ReportModel
{
    public class YesilVadiMakbuzBilgileri
    {
        public string AboneAdiSoyadi { get; set; }
        public string AboneNo { get; set; }
        public string KalorimetreNo { get; set; }
        public string SuSayacNo { get; set; }
        public string FaturaTarihi { get; set; }
        public string Adres { get; set; }
        public string FaturaNo { get; set; }

        public string KurumAdi { get; set; }
    }
}