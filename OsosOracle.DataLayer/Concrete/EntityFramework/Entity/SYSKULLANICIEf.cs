using System.Collections.Generic;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Entity
{
    public sealed class SYSKULLANICIEf : SYSKULLANICI
    {
        public SYSKULLANICIEf()
        {
            SysKullaniciDetayEfCollection = new List<SYSKULLANICIDETAYEf>();
            SysRolKullaniciEfCollection = new List<SYSROLKULLANICIEf>();

        }

        public ICollection<SYSKULLANICIDETAYEf> SysKullaniciDetayEfCollection { get; set; }

        public ICollection<SYSROLKULLANICIEf> SysRolKullaniciEfCollection { get; set; }
        public CONKURUMEf ConKurumEf { get; set; }
    }
}
