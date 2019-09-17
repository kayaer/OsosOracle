using System;
using OsosOracle.Entities.Concrete;
namespace OsosOracle.Entities.ComplexType.ENTSATISComplexTypes
{
    public class ENTSATISDetay
    {

        public int KAYITNO { get; set; }
        public int ABONEKAYITNO { get; set; }

        public string AboneNo { get; set; }
        public int SAYACKAYITNO { get; set; }

        public int SayacSeriNo { get; set; }
        public int? FATURANO { get; set; }
        public int ODEME { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int GUNCELLEYEN { get; set; }
        public DateTime GUNCELLEMETARIH { get; set; }
        public int? IPTAL { get; set; }
        public int? KREDI { get; set; }
        public int YEDEKKREDI { get; set; }

    }

}