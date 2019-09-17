using System.Collections.Generic;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Entity
{
    public sealed class PRMTARIFESUEf : PRMTARIFESU
    {
        public PRMTARIFESUEf()
        {
            EntAboneSayacEfCollection = new List<ENTABONESAYACEf>();
        }
        public ICollection<ENTABONESAYACEf> EntAboneSayacEfCollection { get; set; }
        public CONKURUMEf ConKurumEf { get; set; }

     
    }
}
