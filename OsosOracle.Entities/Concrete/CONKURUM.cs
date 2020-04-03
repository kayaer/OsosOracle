using OsosOracle.Framework.Entities;
using System;
namespace OsosOracle.Entities.Concrete
{
    public class CONKURUM : IEntity
    {
        public int KAYITNO { get; set; }
        public string AD { get; set; }
        public string ACIKLAMA { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int? GUNCELLEYEN { get; set; }
        public DateTime? GUNCELLEMETARIH { get; set; }
       
    }
}
