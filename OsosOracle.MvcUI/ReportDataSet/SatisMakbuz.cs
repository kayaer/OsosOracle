using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.ReportDataSet
{
    public class SatisMakbuz
    {
        public int MakbuzNo { get; set; }

        public DateTime OdemeTarih { get; set; } 

        public decimal OdemeMiktar { get; set; }

        public string AboneAdiSoyadi { get; set; }

        public string AboneNo { get; set; }

        public string SayacSeriNo { get; set; }
    }
}