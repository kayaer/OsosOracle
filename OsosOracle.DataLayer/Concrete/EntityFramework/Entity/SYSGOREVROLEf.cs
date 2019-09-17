using System.Collections.Generic;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Entity
{
    public sealed class SYSGOREVROLEf : SYSGOREVROL
    {


        //public SYSKULLANICIEf SYSKULLANICIOLUSTURANEf { get; set; }
        //public SYSKULLANICIEf SYSKULLANICIGUNCELLEYENEf { get; set; }
        public SYSROLEf SysRolEf { get; set; }
        public SYSGOREVEf SysGorevEf { get; set; }
    }
}
