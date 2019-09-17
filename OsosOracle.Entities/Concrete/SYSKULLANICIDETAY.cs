using System;
using OsosOracle.Framework.Entities;
namespace OsosOracle.Entities.Concrete
{
    public class SYSKULLANICIDETAY : IEntity
    {
        public int KAYITNO { get; set; }
        public string EPOSTA { get; set; }
        public int? GSM { get; set; }
        public int KULLANICIKAYITNO { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int? GUNCELLEYEN { get; set; }
        public DateTime? GUNCELLEMETARIH { get; set; }
    }
}
