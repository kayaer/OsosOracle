using System.Collections.Generic;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Entity
{
    public sealed class ENTSATISEf : ENTSATIS
    {
        public ENTSATISEf()
        {
            EntAboneSayacEfCollection = new List<ENTABONESAYACEf>();
        }
        public ENTABONEEf EntAboneEf { get; set; }
        public ENTSAYACEf EntSayacEf { get; set; }
        public SYSKULLANICIEf SysKullaniciEf { get; set; }
        public NESNEDEGEREf NesneDegerSatisTipiEf { get; set; }
        public NESNEDEGEREf NesneDegerOdemeTipiEf { get; set; }

        public ICollection<ENTABONESAYACEf> EntAboneSayacEfCollection { get; set; }
    }
}
