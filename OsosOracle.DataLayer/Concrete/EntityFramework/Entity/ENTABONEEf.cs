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
            AboneBilgiEfCollection = new List<ENTABONEBILGIEf>();
        }

        public ICollection<ENTSATISEf> SatisEfCollection { get; set; }
        public ICollection<ENTABONESAYACEf> AboneSayacEfCollection { get; set; }      
        public ICollection<ENTABONEBILGIEf>AboneBilgiEfCollection { get; set; }

    }
}
