using System;
using OsosOracle.Entities.Concrete;
namespace OsosOracle.Entities.ComplexType.EntIsEmriComplexTypes
{
    public class EntIsEmriDetay
    {
        public int KayitNo { get; set; }
        public int SayacKayitNo { get; set; }
        public int IsEmriKayitNo { get; set; }
        public string IsEmriKod { get; set; }
        public int? IsEmriDurumKayitNo { get; set; }
        public string IsEmriCevap { get; set; }
        public string Parametre { get; set; }
        public string Cevap { get; set; }

        public string IsEmriAdi { get; set; }
        public string IsEmriDurum { get; set; }
        public string SayacSeriNo { get; set; }
        public DateTime OlusturmaTarih { get; set; }
        public DateTime? GuncellemeTarih { get; set; }
    }

}