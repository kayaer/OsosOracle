using System;
using OsosOracle.Framework.Entities;
namespace OsosOracle.Entities.Concrete
{
    public class PRMTARIFEORTAKAVM : IEntity
    {
        public int KAYITNO { get; set; }
        public string AD { get; set; }
        public int YEDEKKREDI { get; set; }
        public string ACIKLAMA { get; set; }
        public int DURUM { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int? GUNCELLEYEN { get; set; }
        public DateTime? GUNCELLEMETARIH { get; set; }
        public int FIYAT1 { get; set; }
        public int FIYAT2 { get; set; }
        public int FIYAT3 { get; set; }
        public int FIYAT4 { get; set; }
        public int LIMIT1 { get; set; }
        public int LIMIT2 { get; set; }
        public int CARPAN { get; set; }
        public int SAYACCAP { get; set; }
        public string SAYACTARIH { get; set; }
        public int SAYACTIP { get; set; }
        public int TUKETIMKATSAYI { get; set; }
        public int KREDIKATSAYI { get; set; }
        public int BAYRAM1GUN { get; set; }
        public int BAYRAM1AY { get; set; }
        public int BAYRAM1SURE { get; set; }
        public int BAYRAM2GUN { get; set; }
        public int BAYRAM2AY { get; set; }
        public int BAYRAM2SURE { get; set; }
        public int KURUMKAYITNO { get; set; }
        public int? AylikBakimBedeli { get; set; }
    }
}
