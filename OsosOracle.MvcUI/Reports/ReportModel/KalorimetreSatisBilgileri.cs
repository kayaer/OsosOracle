using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Reports.ReportModel
{
    public class KalorimetreSatisBilgileri
    {
        public string Tarih { get; set; }
        public string SayacTuru { get; set; }
        public string KontorMiktar { get; set; }

        public string BirimFiyat { get; set; }
        public string BakimBedeli { get; set; }
        public string Ctv { get; set; }
        public string Kdv { get; set; }
        public string Tutar { get; set; }

        public string TotalTutar { get; set; }
    }
}