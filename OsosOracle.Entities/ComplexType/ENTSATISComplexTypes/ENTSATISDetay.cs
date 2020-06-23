using System;
using OsosOracle.Entities.Concrete;
namespace OsosOracle.Entities.ComplexType.ENTSATISComplexTypes
{
    public class ENTSATISDetay
    {

        public int KAYITNO { get; set; }
        public int ABONEKAYITNO { get; set; }

        public string AboneNo { get; set; }
        public int SAYACKAYITNO { get; set; }

        public string SayacSeriNo { get; set; }
        public string  KapakSeriNo { get; set; }
        public int? FATURANO { get; set; }
        public decimal ODEME { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int GUNCELLEYEN { get; set; }
        public DateTime GUNCELLEMETARIH { get; set; }
        public int? IPTAL { get; set; }
        public decimal? KREDI { get; set; }
        public int YEDEKKREDI { get; set; }
        public int SatisTipi { get; set; }
        public string SatisTipAdi { get; set; }
        public decimal AylikBakimBedeli { get; set; }
        public decimal Kdv { get; set; }
        public decimal Ctv { get; set; }
        public string AboneAdSoyad { get; set; }
        public decimal? ToplamKredi { get; set; }
        public decimal? SatisTutarý { get; set; }
        public string OlusturanKullaniciAdi { get; set; }
        public string SayacTipi { get; set; }
        public string Adres { get; set; }
    }

}