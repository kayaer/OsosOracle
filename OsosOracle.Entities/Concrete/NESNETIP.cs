using System;
            using OsosOracle.Framework.Entities;
			namespace OsosOracle.Entities.Concrete
			{
				public class NESNETIP :  IEntity
				{        public int KAYITNO { get; set; }
        public string ADI { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int? GUNCELLEYEN { get; set; }
        public DateTime? GUNCELLEMETARIH { get; set; }
        public int VERSIYON { get; set; }
    }
}
