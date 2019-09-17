using System.Collections.Generic;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Entity
{
    public sealed class CSTHUMODELEf : CSTHUMODEL
    {
        public CSTHUMODELEf()
        {
            EntHaberlesmeUnitesiEfCollection = new List<ENTHABERLESMEUNITESIEf>();

        }

        public ICollection<ENTHABERLESMEUNITESIEf> EntHaberlesmeUnitesiEfCollection { get; set; }
    }
}
