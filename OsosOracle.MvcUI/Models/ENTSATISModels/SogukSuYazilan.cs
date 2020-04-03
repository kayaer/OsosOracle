using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.ENTSATISModels
{
    public class SogukSuYazilacak
    {
        public decimal Fiyat1 { get; set; }

        public decimal Fiyat2 { get; set; }

        public decimal Fiyat3 { get; set; }

        public decimal Fiyat4 { get; set; }

        public decimal Fiyat5 { get; set; }

        public decimal Limit1 { get; set; }

        public decimal Limit2 { get; set; }

        public decimal Limit3 { get; set; }

        public decimal Limit4 { get; set; }
        public decimal YuklenecekM3 { get; set; }
        public decimal FixCharge { get; set; }
        public int Bayram1Gunn { get; set; }
        public int Bayram1Ayy { get; set; }
        public int Bayram1Suree { get; set; }
        public int Bayram2Gunn { get; set; }
        public int Bayram2Ayy { get; set; }
        public int Bayram2Suree { get; set; }
        public decimal YedekKredi { get; set; }
        public UInt32 SayacSeriNo { get; set; }
        public decimal YeniYuklenecekM3 { get; set; }
    }
}