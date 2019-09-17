using System.Collections.Generic;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Entity
{
    public sealed class SYSOPERASYONGOREVEf : SYSOPERASYONGOREV
    {
        public SYSCSTOPERASYONEf SysCstOperasyonEf { get; set; }
        public SYSGOREVEf SysGorevEf { get; set; }
    }
}
