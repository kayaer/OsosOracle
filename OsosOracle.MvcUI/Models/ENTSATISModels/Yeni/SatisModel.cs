
namespace OsosOracle.MvcUI.Models.ENTSATISModels.Yeni
{
    public class SatisModel
    {
        public SatisModel()
        {
            SuSatisModel = new SuSatisModel();
            KalorimetreSatisModel = new KalorimetreSatisModel();
            GazSatisModel = new GazSatisModel();
        }
        
        public SuSatisModel SuSatisModel { get; set; }
        public KalorimetreSatisModel KalorimetreSatisModel { get; set; }

        public GazSatisModel GazSatisModel { get; set; }

        public string HamData { get; set; }

       
    }
}