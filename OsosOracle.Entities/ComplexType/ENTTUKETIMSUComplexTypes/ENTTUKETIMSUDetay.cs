using System;
using OsosOracle.Entities.Concrete;
namespace OsosOracle.Entities.ComplexType.ENTTUKETIMSUComplexTypes
{
    public class ENTTUKETIMSUDetay
    {

        public int KAYITNO { get; set; }
        public string SAYACID { get; set; }
        public int? TUKETIM { get; set; }
        public int TUKETIM1 { get; set; }
        public int TUKETIM2 { get; set; }
        public int TUKETIM3 { get; set; }
        public int TUKETIM4 { get; set; }
        public DateTime? SAYACTARIH { get; set; }
        public DateTime? OKUMATARIH { get; set; }
        public Byte[] DATA { get; set; }
        public int HGUN { get; set; }
        public int? HARCANANKREDI { get; set; }
        public int? KALANKREDI { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int GUNCELLEYEN { get; set; }
        public DateTime GUNCELLEMETARIH { get; set; }
        public int VERSIYON { get; set; }
        public int HEADERNO { get; set; }
        public int FATURAMOD { get; set; }
        public string KrediDurumu { get; set; }
    }

}