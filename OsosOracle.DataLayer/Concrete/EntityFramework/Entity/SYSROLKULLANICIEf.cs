using System.Collections.Generic;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Entity
{
    public sealed class SYSROLKULLANICIEf : SYSROLKULLANICI
    {
        public SYSKULLANICIEf SysKullaniciEf { get; set; }
        public SYSROLEf SysRolEf { get; set; }
    }
}
