using System.Collections.Generic;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Entity
{
    public sealed class SYSKULLANICIEf : SYSKULLANICI
    {
        public SYSKULLANICIEf()
        {
            SysRolKullaniciEfCollection = new List<SYSROLKULLANICIEf>();
            EntSatisEfCollection = new List<ENTSATISEf>();
        }

    
        public ICollection<SYSROLKULLANICIEf> SysRolKullaniciEfCollection { get; set; }
        public ICollection<ENTSATISEf> EntSatisEfCollection { get; set; }
        public CONKURUMEf ConKurumEf { get; set; }
    }
}
