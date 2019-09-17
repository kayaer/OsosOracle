using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.Framework.Entities
{
    public interface IEntity
    {
         int VERSIYON { get; set; }
         int OLUSTURAN { get; set; }
         DateTime OLUSTURMATARIH { get; set; }
         int? GUNCELLEYEN { get; set; }
         DateTime? GUNCELLEMETARIH { get; set; }
    }
}
