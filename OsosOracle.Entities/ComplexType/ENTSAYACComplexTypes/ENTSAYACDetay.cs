using System;
using OsosOracle.Entities.ComplexType.PRMTARIFEELKComplexTypes;
using OsosOracle.Entities.ComplexType.PRMTARIFESUComplexTypes;
using OsosOracle.Entities.Concrete;
namespace OsosOracle.Entities.ComplexType.ENTSAYACComplexTypes
{
    public class ENTSAYACDetay
    {
        public DateTime SAYACMONTAJTARIH { get; set; }
        public int KURUMKAYITNO { get; set; }
        public int KAYITNO { get; set; }
        public string SERINO { get; set; }
        public int SayacModelKayitNo { get; set; }
        public string ACIKLAMA { get; set; }
        public int DURUM { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int GUNCELLEYEN { get; set; }
        public DateTime GUNCELLEMETARIH { get; set; }

        public string SayacTipi { get; set; }

        public PRMTARIFESUDetay PrmTarifeSuDetay { get; set; }
        public PRMTARIFEELKDetay PrmTarifeElkDetay { get; set; }
        public string Kurum { get; set; }
        public int SayacTuru { get; set; }
        public int? TarifeKayitNo { get; set; }
        public DateTime? SonSatisTarih { get; set; }
        public string KapakSeriNo { get; set; }
        public int Cap { get; set; }
        public string KonsSeriNo { get; set; }
    }

}