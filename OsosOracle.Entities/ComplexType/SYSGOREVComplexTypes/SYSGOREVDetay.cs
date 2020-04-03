using System;
using System.Collections.Generic;
using OsosOracle.Entities.ComplexType.SYSCSTOPERASYONComplexTypes;
using OsosOracle.Entities.Concrete;
namespace OsosOracle.Entities.ComplexType.SYSGOREVComplexTypes
{
    public class SYSGOREVDetay
    {

        public int KAYITNO { get; set; }
        public string AD { get; set; }
        public string ACIKLAMA { get; set; }
        public int VERSIYON { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
        public int GUNCELLEYEN { get; set; }
        public DateTime GUNCELLEMETARIH { get; set; }
        public int KURUMKAYITNO { get; set; }

        #region Sonradan eklenenler

        public List<SYSCSTOPERASYONDetay> SysOperasyonList { get; set; }
        public string KurumAdi { get; set; }
        #endregion
    }

}