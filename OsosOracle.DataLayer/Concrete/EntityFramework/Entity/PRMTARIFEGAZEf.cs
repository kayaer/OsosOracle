using System.Collections.Generic;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Entity
{
    public sealed class PRMTARIFEGAZEf : PRMTARIFEGAZ
    {
        public PRMTARIFEGAZEf()
        {
            EntAboneSayacEfCollection = new List<ENTABONESAYACEf>();
        }
        public CONKURUMEf ConKurumEf { get; set; }
        public ICollection<ENTABONESAYACEf> EntAboneSayacEfCollection { get; set; }
    }
}
