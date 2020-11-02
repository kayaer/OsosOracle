
using System;
using OsosOracle.Framework.DataAccess.Filter;

namespace OsosOracle.Entities.ComplexType.ENTSAYACDURUMSUComplexTypes
{
    public class EntSayacOkumaVeriAra
    {
        public Ara Ara { get; set; }
        public int? KayitNo { get; set; }
        public int? SayacKayitNo { get; set; }
        public string SayacId { get; set; }
        public DateTime? OkumaTarihiBaslangic { get; set; }

        public DateTime? OkumaTarihiBitis { get; set; }
        public string KonsSeriNo { get; set; }

    }
}
