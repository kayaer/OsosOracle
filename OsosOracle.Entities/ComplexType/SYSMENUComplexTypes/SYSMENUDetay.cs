using System;
using System.Collections.Generic;
using OsosOracle.Entities.Concrete;
namespace OsosOracle.Entities.ComplexType.SYSMENUComplexTypes
{
	public class SYSMENUDetay
	{
        public int KAYITNO { get; set; }
        public string TR { get; set; }
        public string EN { get; set; }
        public string YEREL { get; set; }
        public int? PARENTKAYITNO { get; set; }
        public int MENUORDER { get; set; }
        public string EXTERNALURL { get; set; }
        public string AREA { get; set; }
        public string ACTION { get; set; }
        public string CONTROLLER { get; set; }
        public string PARAMETERS { get; set; }
        public int? DURUM { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int GUNCELLEYEN { get; set; }
        public DateTime GUNCELLEMETARIH { get; set; }
        public string ICON { get; set; }
        public List<SYSMENUDetay> Children { get; set; }
    }

}