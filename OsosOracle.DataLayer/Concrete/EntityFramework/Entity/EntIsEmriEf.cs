using System.Collections.Generic;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Entity
{
    public sealed class EntIsEmriEf : EntIsEmri
    {
        public ENTSAYACEf EntSayacEf { get; set; }

        public NESNEDEGEREf NesneDegerIsEmriEf { get; set; }

        public NESNEDEGEREf NesneDegerIsEmriDurumEf { get; set; }
    }
}
