
using OsosOracle.Framework.DataAccess.Filter;
using System;

namespace OsosOracle.Entities.ComplexType.SYSMENUComplexTypes
{
    public class SYSMENUAra
    {
        public Ara Ara { get; set; }
        public int? KAYITNO { get; set; }
        public string KAYITNOlar { get; set; }
        public string TR { get; set; }
     
        public int? PARENTKAYITNO { get; set; }
        public int? MENUORDER { get; set; }
   
        public string AREA { get; set; }
        public string ACTION { get; set; }
        public string CONTROLLER { get; set; }
      
        public int? DURUM { get; set; }
        public int? VERSIYON { get; set; }
        public string ICON { get; set; }
    }
}
