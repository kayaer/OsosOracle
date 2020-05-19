using System;
using OsosOracle.Entities.Concrete;
namespace OsosOracle.Entities.ComplexType.ENTABONESAYACComplexTypes
{
    public class ENTABONESAYACDetay
    {

        public int KAYITNO { get; set; }
        public int ABONEKAYITNO { get; set; }
        public int SAYACKAYITNO { get; set; }
        public string SOKULMENEDEN { get; set; }
        public int SONENDEKS { get; set; }
        public DateTime? SOKULMETARIH { get; set; }
        public int KARTNO { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int GUNCELLEYEN { get; set; }
        public DateTime GUNCELLEMETARIH { get; set; }
        public int? TARIFEKAYITNO { get; set; }
        public DateTime? TAKILMATARIH { get; set; }
        public int SONSATISKAYITNO { get; set; }
        public DateTime? SONSATISTARIH { get; set; }
        public string AboneNo { get; set; }
        public string SayacSeriNo { get; set; }
        public string SuTarifeAdi { get; set; }
        public string KalorimetreTarifeAdi { get; set; }
        public string Aciklama { get; set; }
        public string AboneAdSoyad { get; set; }
        public string TcKimlikNo { get; set; }
        public string AboneDurum { get; set; }

        public DateTime? SonSatisTarihi { get; set; }
        public string SayacModel { get; set; }
        public string KapakSeriNo { get; set; }
        public string GazTarifeAdi { get; set; }
        public string SonSatisTarihiStr { get=>SonSatisTarihi.ToString() ;  }
        public int SayacModelKayitNo { get; set; }
    }

}