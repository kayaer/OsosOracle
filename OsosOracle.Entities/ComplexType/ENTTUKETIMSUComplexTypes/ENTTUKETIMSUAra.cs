
using System;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.Entities.ComplexType.ENTTUKETIMSUComplexTypes
{
    public class ENTTUKETIMSUAra
    {
        public Ara Ara { get; set; }
        public int? KAYITNO { get; set; }
        public string KAYITNOlar { get; set; }
        public string SAYACID { get; set; }
        public int? TUKETIM { get; set; }
        public int? TUKETIM1 { get; set; }
        public int? TUKETIM2 { get; set; }
        public int? TUKETIM3 { get; set; }
        public int? TUKETIM4 { get; set; }
        public DateTime? SAYACTARIH { get; set; }
        public DateTime? OKUMATARIH { get; set; }
        public Byte[] DATA { get; set; }
        public int? HGUN { get; set; }
        public int? HARCANANKREDI { get; set; }
        public int? KALANKREDI { get; set; }
        public int? VERSIYON { get; set; }
        public int? HEADERNO { get; set; }
        public int? FATURAMOD { get; set; }

        public DateTime? OkumaTarihiBaslangic { get; set; }

        public DateTime? OkumaTarihiBitis { get; set; }

        public int? SayacKayitNo { get; set; }
    }
}
