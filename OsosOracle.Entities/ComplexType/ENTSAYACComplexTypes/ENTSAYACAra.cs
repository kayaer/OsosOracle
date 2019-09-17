
using OsosOracle.Framework.DataAccess.Filter;
using System;

namespace OsosOracle.Entities.ComplexType.ENTSAYACComplexTypes
{
    public class ENTSAYACAra
    {
        public Ara Ara { get; set; }
        public int? KAYITNO { get; set; }
        public string KAYITNOlar { get; set; }
        public DateTime? SAYACMONTAJTARIH { get; set; }
        public string SAYACID { get; set; }
        public int? KURUMKAYITNO { get; set; }
        public int? SERINO { get; set; }
        public int? SAYACTUR { get; set; }
        public string ACIKLAMA { get; set; }
        public int? DURUM { get; set; }
        public int? VERSIYON { get; set; }

        public string SayacSeriNoIceren { get; set; }
    }
}
