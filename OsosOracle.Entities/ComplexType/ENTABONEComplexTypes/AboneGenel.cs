using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.Entities.ComplexType.ENTABONEComplexTypes
{
    public class AboneGenel
    {
        public int KayitNo { get; set; }
        public string TcKimlikNo { get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public string AdiSoyadi { get; set; }
        public string AboneNo { get; set; }
        public string DogumYeri { get; set; }
        public DateTime? DogumTarihi { get; set; }
        public string DogumTarihiStr => DogumTarihi?.ToShortDateString();
        public string Ogrenim { get; set; }
        public string DurumAdi { get; set; }
        public string Mesaj { get; set; }

        public DateTime? SonSatisTarihi { get; set; }
        public string SonSatisTarihiStr => SonSatisTarihi?.ToShortDateString();

    }
}
