using OsosOracle.Framework.Entities;
using System;
namespace OsosOracle.Entities.Concrete
{
    public class ENTSAYAC : IEntity
    {
        public DateTime? SAYACMONTAJTARIH { get; set; }
        public string SAYACID { get; set; }
        public int? KURUMKAYITNO { get; set; }
        public int KAYITNO { get; set; }
        public int SERINO { get; set; }
        public int SAYACTUR { get; set; }
        public string ACIKLAMA { get; set; }
        public int DURUM { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int? GUNCELLEYEN { get; set; }
        public DateTime? GUNCELLEMETARIH { get; set; }
    }
}
