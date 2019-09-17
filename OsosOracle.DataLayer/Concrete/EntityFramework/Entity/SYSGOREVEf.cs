using System.Collections.Generic;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Entity
{
    public sealed class SYSGOREVEf : SYSGOREV
    {
        public SYSGOREVEf()
        {
            GOREVKAYITNOSYSOPERASYONGOREVEfCollection = new List<SYSOPERASYONGOREVEf>();
            GorevSysGorevRolEfCollection = new List<SYSGOREVROLEf>();
        }

        public ICollection<SYSOPERASYONGOREVEf> GOREVKAYITNOSYSOPERASYONGOREVEfCollection { get; set; }
        public ICollection<SYSGOREVROLEf> GorevSysGorevRolEfCollection { get; set; }

        //public SYSKULLANICIEf SYSKULLANICIOLUSTURANEf { get; set; }
        //public SYSKULLANICIEf SYSKULLANICIGUNCELLEYENEf { get; set; }
    }
}
