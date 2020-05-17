using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.ReportModels
{
    public class RaporParametreModel
    {
        public int? SatisTipi { get; set; }
        public int? SayacModelId { get; set; }
        public string SatisTarihiBaslangic { get; set; }
        public string SatisTarihiBitis { get; set; }
        public int KurumKayitNo { get; set; }
    }
}