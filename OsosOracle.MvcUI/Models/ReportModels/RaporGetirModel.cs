using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.ReportModels
{
    public class RaporGetirModel
    {
        public string RaporAdi { get; set; }
        public List<RaporParametre> RaporParametreler { get; set; }
        public EnumRaporUzanti Uzanti { get; set; }
        /// <summary>
        /// Dosya adı belirtilmezse RaporAdi dosya adı olarak kullanılacaktır.
        /// </summary>
        public string DosyaAdi { get; set; }
        public byte[] HazirByteData { get; set; }
    }
    public class RaporParametre
    {
        public string ParametreAdi { get; set; }
        public string Deger { get; set; }
    }
    public enum EnumRaporUzanti
    {
        Pdf = 275,
        Xls = 276,
    }

    public class RaporDosya
    {
        public byte[] ByteData { get; set; }
        public string ContentType { get; set; }
        public string RaporAdi { get; set; }
    }
}