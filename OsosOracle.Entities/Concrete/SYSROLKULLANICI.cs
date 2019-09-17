using OsosOracle.Framework.Entities;
using System;
namespace OsosOracle.Entities.Concrete
{
    public class SYSROLKULLANICI : IEntity
    {
        public int KAYITNO { get; set; }
        public int KULLANICIKAYITNO { get; set; }
        public int ROLKAYITNO { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int? GUNCELLEYEN { get; set; }
        public DateTime? GUNCELLEMETARIH { get; set; }
    }
}
