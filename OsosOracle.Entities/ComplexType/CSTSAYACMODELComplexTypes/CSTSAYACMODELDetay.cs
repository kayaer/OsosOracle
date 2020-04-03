using System;
using OsosOracle.Entities.Concrete;
namespace OsosOracle.Entities.ComplexType.CSTSAYACMODELComplexTypes
{
	public class CSTSAYACMODELDetay
	{
	
		        public int KAYITNO { get; set; }
        public string AD { get; set; }
        public string ACIKLAMA { get; set; }
        public int DURUM { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int GUNCELLEYEN { get; set; }
        public DateTime GUNCELLEMETARIH { get; set; }
        public int MARKAKAYITNO { get; set; }
       
        public string FLAG { get; set; }
        public string CONTROLLER { get; set; }
        public int SayacTuruKayitNo { get; set; }
        public string SayacTuru { get; set; }
    }

}