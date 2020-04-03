using System.Collections.Generic;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Entity
{
    public sealed class ENTABONEEf : ENTABONE
    {
        public ENTABONEEf()
        {
            SatisEfCollection = new List<ENTSATISEf>();
            AboneSayacEfCollection = new List<ENTABONESAYACEf>();
           
        }

        public ICollection<ENTSATISEf> SatisEfCollection { get; set; }
        public ICollection<ENTABONESAYACEf> AboneSayacEfCollection { get; set; }      
        

    }
}
