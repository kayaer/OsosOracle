using System.Collections.Generic;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Entity
{
    public sealed class SYSROLEf : SYSROL
    {
        public SYSROLEf()
        {
            SysRolKullaniciEfCollection = new List<SYSROLKULLANICIEf>();

            RolSysGorevRolEfCollection = new List<SYSGOREVROLEf>();
        }

        public ICollection<SYSROLKULLANICIEf> SysRolKullaniciEfCollection { get; set; }
        public ICollection<SYSGOREVROLEf> RolSysGorevRolEfCollection { get; set; }

        public CONKURUMEf ConKurumEf { get; set; }
    }
}
