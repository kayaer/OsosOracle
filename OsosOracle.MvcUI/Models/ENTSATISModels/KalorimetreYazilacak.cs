using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.ENTSATISModels
{
    public class KalorimetreYazilacak
    {
        public string TarifeAdi { get; set; }
        public UInt32 devno { get; set; }
        public Int32 anakr { get; set; }
        public Int32 yedekkr { get; set; }
        public int Bayram1Gunn { get; set; }
        public int Bayram1Ayy { get; set; }
        public int Bayram1Suree { get; set; }
        public int Bayram2Gunn { get; set; }
        public int Bayram2Ayy { get; set; }
        public int Bayram2Suree { get; set; }
        public int Yuklenecek { get; set; }
    }
}