
using OsosOracle.Entities.ComplexType.PRMTARIFEELKComplexTypes;
using OsosOracle.Entities.ComplexType.PRMTARIFESUComplexTypes;
using OsosOracle.Entities.Concrete;
namespace OsosOracle.MvcUI.Models.ENTSATISModels
{
	public class ENTSATISKaydetModel
    {
        public ENTSATISKaydetModel()
        {
            ElkKartYuklenecek = new ElkKartYazilan();
        }
        public PRMTARIFESUDetay PrmTarifeSuDetay { get; set; }
        public PRMTARIFEELKDetay PrmTarifeElkDetay { get; set; }

        public ElkKartOkunan ElkKartOkunan { get; set; }

        public ElkKartYazilan ElkKartYuklenecek { get; set; }

        public ENTSATIS ENTSATIS { get; set; }
        public string SayacTipi { get; set; }
      
        public int SayacSeriNo { get;  set; }

        public decimal GirilenTutar { get; set; }

        public decimal AnaKredi { get; set; }

        public decimal YedekKredi { get; set; }

        public string HamData { get; set; }

        public string fileName { get; set; }
    }
}
