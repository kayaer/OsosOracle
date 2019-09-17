using System;
using OsosOracle.Entities.Concrete;
namespace OsosOracle.Entities.ComplexType.SYSROLKULLANICIComplexTypes
{
	public class SYSROLKULLANICIDetay
	{
	
		        public int KAYITNO { get; set; }
        public int KULLANICIKAYITNO { get; set; }
        public int ROLKAYITNO { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int GUNCELLEYEN { get; set; }
        public DateTime GUNCELLEMETARIH { get; set; }
        public string RolAdi { get; set; }
        public string KullaniciAdi { get; set; }
    }

}