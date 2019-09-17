using System.Collections.Generic;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Entity
{
    public sealed class ENTHABERLESMEUNITESIEf : ENTHABERLESMEUNITESI
    {
        public ENTHABERLESMEUNITESIEf()
        {
          
            HUKAYITNOENTHUSAYACEfCollection = new List<ENTHUSAYACEf>();
          
        }

       
        public ICollection<ENTHUSAYACEf> HUKAYITNOENTHUSAYACEfCollection { get; set; }

        public CSTHUMARKAEf CstHuMarkaEf { get; set; }

        public CSTHUMODELEf CstHuModelEf { get; set; }

        public CONKURUMEf ConKurumEf { get; set; }
       
    }
}
