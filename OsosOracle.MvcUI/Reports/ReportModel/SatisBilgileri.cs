using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Reports.ReportModel
{
    public class SatisBilgileri
    {
        public string SatisTutari { get; set; }

        public string BakimHizmetleriBedeli { get; set; }
        public string CtvBedeli { get; set; }
        public string KdvBedeli { get; set; }
        public string GenelToplam { get; set; }
    }
}