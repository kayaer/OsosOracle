using OsosOracle.Framework.Entities;
using System;
namespace OsosOracle.Entities.Concrete
{
    public class SYSOPERASYONGOREV : IEntity
    {
        public int OPERASYONKAYITNO { get; set; }
        public int GOREVKAYITNO { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int? GUNCELLEYEN { get; set; }
        public DateTime? GUNCELLEMETARIH { get; set; }
        public int KAYITNO { get; set; }
    }
}
