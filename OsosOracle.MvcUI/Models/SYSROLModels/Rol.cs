using OsosOracle.Entities.ComplexType.SYSGOREVComplexTypes;
using OsosOracle.MvcUI.Models.SYSGOREVModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.SYSROLModels
{
    public class Rol
    {
        public int KayitNo { get; set; }
        public string Ad { get; set; }
        public string Aciklama { get; set; }

        public bool Secildi { get; set; }

        public List<SYSGOREVDetay> SysGorevList { get; set; }
    }
}