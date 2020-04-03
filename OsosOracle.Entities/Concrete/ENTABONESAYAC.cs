using OsosOracle.Framework.Entities;
using System;
namespace OsosOracle.Entities.Concrete
{
    public class ENTABONESAYAC : IEntity, IEntitySoftDelete
    {
        public int KAYITNO { get; set; }
        public int ABONEKAYITNO { get; set; }
        public int SAYACKAYITNO { get; set; }
        public string SOKULMENEDEN { get; set; }
        public int? SONENDEKS { get; set; }
        public DateTime? SOKULMETARIH { get; set; }
        public int? KARTNO { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int? GUNCELLEYEN { get; set; }
        public DateTime? GUNCELLEMETARIH { get; set; }
        public int? TARIFEKAYITNO { get; set; }
       
        public DateTime? TAKILMATARIH { get; set; }
        public int? SONSATISKAYITNO { get; set; }
        public DateTime? SONSATISTARIH { get; set; }
        public int DURUM { get; set; }
    }
}
