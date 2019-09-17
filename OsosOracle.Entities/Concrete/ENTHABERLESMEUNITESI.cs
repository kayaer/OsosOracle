using OsosOracle.Framework.Entities;
using System;
namespace OsosOracle.Entities.Concrete
{
    public class ENTHABERLESMEUNITESI : IEntity
    {
        public int KAYITNO { get; set; }
        public string SERINO { get; set; }
        public string SIMTELNO { get; set; }
        public string IP { get; set; }
        public string ACIKLAMA { get; set; }
        public int DURUM { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int? GUNCELLEYEN { get; set; }
        public DateTime? GUNCELLEMETARIH { get; set; }
        public int? MARKA { get; set; }
        public int? MODEL { get; set; }
        public int KURUMKAYITNO { get; set; }
    }
}
