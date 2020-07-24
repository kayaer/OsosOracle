using System;
using OsosOracle.Framework.Entities;
namespace OsosOracle.Entities.Concrete
{
    public class EntIsEmri
    {
        public int KAYITNO { get; set; }
        public int SayacKayitNo { get; set; }
        public int IsEmriKayitNo { get; set; }
        public string Parametre { get; set; }
        public int? IsEmriDurumKayitNo { get; set; }
        public string IsEmriCevap { get; set; }
        public string Cevap { get; set; }
        public DateTime OlusturmaTarih { get; set; }
        public DateTime? GuncellemeTarih { get; set; }

    }
}
