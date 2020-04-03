using System.Collections.Generic;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Entity
{
    public sealed class PRMTARIFEKALORIMETREEf : PRMTARIFEKALORIMETRE
    {
        public PRMTARIFEKALORIMETREEf()
        {
            EntAboneSayacEfCollection = new List<ENTABONESAYACEf>();
        }
        public CONKURUMEf ConKurumEf { get; set; }
        public ICollection<ENTABONESAYACEf> EntAboneSayacEfCollection { get; set; }

    }
}
