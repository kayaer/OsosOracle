using System.Collections.Generic;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Entity
 {
    public sealed class NESNETIPEf : NESNETIP
    {
public NESNETIPEf()
{
NESNETIPKAYITNONESNEDEGEREfCollection = new List<NESNEDEGEREf>();
}

public  ICollection<NESNEDEGEREf> NESNETIPKAYITNONESNEDEGEREfCollection { get; set; }

}
}
