using System.Collections.Generic;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Entity
{
    public sealed class CSTSAYACMODELEf : CSTSAYACMODEL
    {
        public CSTSAYACMODELEf()
        {
            EntSayacEfCollection = new List<ENTSAYACEf>();

        }
        public ICollection<ENTSAYACEf> EntSayacEfCollection { get; set; }
    }
}
