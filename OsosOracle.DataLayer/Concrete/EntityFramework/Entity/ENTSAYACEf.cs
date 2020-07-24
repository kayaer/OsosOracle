using System.Collections.Generic;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Entity
{
    public sealed class ENTSAYACEf : ENTSAYAC
    {
        public ENTSAYACEf()
        {

            EntHuSayacEfCollection = new List<ENTHUSAYACEf>();
            EntSatisEfCollection = new List<ENTSATISEf>();
            EntAboneSayacEfCollection = new List<ENTABONESAYACEf>();
            EntIsEmriEfCollection = new List<EntIsEmriEf>();
            //EntSayacSonDurumSuEfCollection = new List<ENTSAYACSONDURUMSUEf>();
        }


        public ICollection<ENTHUSAYACEf> EntHuSayacEfCollection { get; set; }

        public ICollection<ENTSATISEf> EntSatisEfCollection { get; set; }

        public ICollection<ENTABONESAYACEf> EntAboneSayacEfCollection { get; set; }
        public ICollection<EntIsEmriEf> EntIsEmriEfCollection { get; set; }

        //public ICollection<ENTSAYACSONDURUMSUEf> EntSayacSonDurumSuEfCollection { get; set; }
        public CSTSAYACMODELEf CstSayacModelEf { get; set; }

        public CONKURUMEf ConKurumEf { get; set; }

        public ENTSAYACSONDURUMSUEf EntSayacSonDurumSuEf { get; set; }

    }
}
