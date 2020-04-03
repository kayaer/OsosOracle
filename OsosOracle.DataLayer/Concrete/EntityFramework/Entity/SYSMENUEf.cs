using System.Collections.Generic;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Entity
{
    public sealed class SYSMENUEf : SYSMENU
    {
        public SYSMENUEf()
        {
            SysCstOperasyonEfCollection = new List<SYSCSTOPERASYONEf>();
        }

        //public ICollection<SYSCSTOPERASYONMENUEf> MENUKAYITNOSYSCSTOPERASYONMENUEfCollection { get; set; }

        //public CSTDURUMEf CSTDURUMDURUMEf { get; set; }
        //public SYSKULLANICIEf SYSKULLANICIOLUSTURANEf { get; set; }
        //public SYSKULLANICIEf SYSKULLANICIGUNCELLEYENEf { get; set; }
        public ICollection<SYSCSTOPERASYONEf> SysCstOperasyonEfCollection { get; set; }
    }
}
