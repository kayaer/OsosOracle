
using OsosOracle.Entities.ComplexType.PRMTARIFEELKComplexTypes;
using OsosOracle.Entities.ComplexType.PRMTARIFEKALORIMETREComplexTypes;
using OsosOracle.Entities.ComplexType.PRMTARIFESUComplexTypes;
using OsosOracle.Entities.Concrete;
namespace OsosOracle.MvcUI.Models.ENTSATISModels
{
    public class ENTSATISKaydetModel
    {
        public ENTSATISKaydetModel()
        {
            ElektrikYuklenecek = new ElkKartYazilan();
            KalorimetreYazilacak = new KalorimetreYazilacak();
            SogukSuYazilacak = new SogukSuYazilacak();
        }
        public PRMTARIFESUDetay PrmTarifeSuDetay { get; set; }
        public PRMTARIFEELKDetay PrmTarifeElkDetay { get; set; }
        public PRMTARIFEKALORIMETREDetay PrmTarifeKALORIMETREDetay { get; set; }

        public ElektrikOkunan ElektrikOkunan { get; set; }

        public ElkKartYazilan ElektrikYuklenecek { get; set; }

        public KalorimetreOkunan KalorimetreOkunan { get; set; }
        public KalorimetreYazilacak KalorimetreYazilacak { get; set; }

        public SogukSuOkunan SogukSuOkunan { get; set; }
        public SogukSuYazilacak SogukSuYazilacak { get; set; }

        public ENTSATIS ENTSATIS { get; set; }
        public string SayacTipi { get; set; }

        public int SayacSeriNo { get; set; }

        public decimal GirilenTutar { get; set; }

        public decimal AnaKredi { get; set; }

        public decimal YedekKredi { get; set; }

        public string HamData { get; set; }

        public string fileName { get; set; }

        public string SayacCesidi { get; set; }//Elektrik,Su,Dogalgaz,Kalorimetre olabilir
    }
}
