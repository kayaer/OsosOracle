using OsosOracle.Framework.Entities;
using System;
namespace OsosOracle.Entities.Concrete
{
    public class ENTSAYAC : IEntity,IEntitySoftDelete
    {
        public DateTime? SAYACMONTAJTARIH { get; set; }
        public int? KURUMKAYITNO { get; set; }
        public int KAYITNO { get; set; }
        public string SERINO { get; set; }

        public string KapakSeriNo { get; set; }
        public int SayacModelKayitNo { get; set; }
        public string ACIKLAMA { get; set; }
        public int DURUM { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int? GUNCELLEYEN { get; set; }
        public DateTime? GUNCELLEMETARIH { get; set; }
        public string KonsSeriNo { get; set; }
    }
}
