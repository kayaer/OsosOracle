using System.Collections.Generic;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Entity
{
    public sealed class CSTHUMARKAEf : CSTHUMARKA
    {
        public CSTHUMARKAEf()
        {
            EntHaberlesmeUnitesiEfCollection = new List<ENTHABERLESMEUNITESIEf>();
        }

        public ICollection<ENTHABERLESMEUNITESIEf> EntHaberlesmeUnitesiEfCollection { get; set; }

    }
}
