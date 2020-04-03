using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Reports.ReportModel
{
    public class Satis
    {
        public string KapakSeriNo { get; set; }
        public string AboneNo { get; set; }
        public string AdiSoyadi { get; set; }
        public string SatisTarihi { get; set; }
        public decimal Kredi { get; set; }
        public decimal Kdv { get; set; }
        public decimal Ctv { get; set; }
        public decimal AylikBakimBedeli { get; set; }
        public decimal Tutar { get; set; }

        public string SayacModeli { get; set; }
        public string SatisTipi { get; set; }
    }
}