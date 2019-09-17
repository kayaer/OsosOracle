using OsosOracle.Framework.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.Framework.DataAccess.Filter
{
    public class Siralama
    {
        public Siralama()
        {
            SiralamaTipi = EnumSiralamaTuru.Asc;
        }

        public string KolonAdi { get; set; }
        public EnumSiralamaTuru SiralamaTipi { get; set; }
    }
}
