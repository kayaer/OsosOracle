using System.Collections.Generic;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Entity
{
    public sealed class NESNEDEGEREf : NESNEDEGER
    {
        public NESNEDEGEREf()
        {
            CstSayacModelEfCollection = new List<CSTSAYACMODELEf>();
            EntSatisTipiEfCollection = new List<ENTSATISEf>();
            EntOdemeTipiEfCollection = new List<ENTSATISEf>();
            EntIsEmriEfCollection = new List<EntIsEmriEf>();
            EntIsEmriDurumEfCollection = new List<EntIsEmriEf>();
        }

        public ICollection<CSTSAYACMODELEf> CstSayacModelEfCollection { get; set; }

        public ICollection<ENTSATISEf> EntSatisTipiEfCollection { get; set; }
        public ICollection<ENTSATISEf> EntOdemeTipiEfCollection { get; set; }

        public ICollection<EntIsEmriEf> EntIsEmriEfCollection { get; set; }

        public ICollection<EntIsEmriEf> EntIsEmriDurumEfCollection { get; set; }
        public NESNETIPEf NESNETIPNESNETIPKAYITNOEf { get; set; }
    }
}
