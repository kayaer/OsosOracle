using OsosOracle.MvcUI.Models.SYSGOREVModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.SYSROLModels
{
    public class SysRolDetay
    {
        public int KayitNo { get; set; }
        public string Ad { get; set; }
        public string Aciklama { get; set; }
        public int RowVersion { get; set; }
        public int EkleyenId { get; set; }
        public DateTime EklemeTarihi { get; set; }
        public int? DegistirenId { get; set; }
        public DateTime? DegistirmeTarihi { get; set; }
        public bool Silindi { get; set; }
        public int KurumId { get; set; }

        public List<SysGorevDetay> SysGorevList { get; set; }
    }
}