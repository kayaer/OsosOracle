using OsosOracle.Entities.ComplexType.SYSCSTOPERASYONComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.SYSGOREVModels
{
    public class SysGorevDetay
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

        #region Sonradan eklenenler

        public List<SYSCSTOPERASYONDetay> SysOperasyonList { get; set; }
        #endregion

    }
}