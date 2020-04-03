using System.Collections.Generic;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Entity
{
    public sealed class SYSCSTOPERASYONEf : SYSCSTOPERASYON
    {
        public SYSCSTOPERASYONEf()
        {
            OperasyonSysOperasyonGorevEfCollection = new List<SYSOPERASYONGOREVEf>();
        }

        //public ICollection<SYSCSTOPERASYONMENUEf> OPERASYONKAYITNOSYSCSTOPERASYONMENUEfCollection { get; set; }

        //public SYSCSTOPERASYONTUREf SYSCSTOPERASYONTUROPERASYONTUREf { get; set; }
        //public SYSKULLANICIEf SYSKULLANICIOLUSTURANEf { get; set; }
        //public SYSKULLANICIEf SYSKULLANICIGUNCELLEYENEf { get; set; }
        public ICollection<SYSOPERASYONGOREVEf> OperasyonSysOperasyonGorevEfCollection { get; set; }
        public SYSMENUEf SysMenuEf { get; set; }
    }
}
