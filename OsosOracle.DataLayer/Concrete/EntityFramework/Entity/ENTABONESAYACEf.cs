using System.Collections.Generic;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Entity
{
    public sealed class ENTABONESAYACEf : ENTABONESAYAC
    {

        public ENTABONEEf EntAboneEf { get; set; }
        public ENTSATISEf EntSatisEf { get; set; }
        public ENTSAYACEf EntSayacEf { get; set; }
        public PRMTARIFESUEf PrmTarifeSuEf { get; set; }

        public PRMTARIFEELKEf PrmTarifeElkEf { get; set; }
    }
}
