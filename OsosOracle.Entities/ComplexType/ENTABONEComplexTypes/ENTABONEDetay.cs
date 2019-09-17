using System;
using OsosOracle.Entities.Concrete;
namespace OsosOracle.Entities.ComplexType.ENTABONEComplexTypes
{
    public class ENTABONEDetay
    {

        public int KAYITNO { get; set; }
        public string AD { get; set; }
        public string SOYAD { get; set; }
        public int DURUM { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int GUNCELLEYEN { get; set; }
        public DateTime GUNCELLEMETARIH { get; set; }
        public int KURUMKAYITNO { get; set; }
        public string AboneNo { get; set; }
        public string Eposta { get; set; }
        public string SayacModel { get; set; }
        public int SayacSeriNo { get; set; }
        public string TarifeAdi { get; set; }
        public int TarifeKayitNo { get; set; }

        public string AutoCompleteStr => AD + " " + SOYAD +" Abone No:"+ AboneNo;
    }

}