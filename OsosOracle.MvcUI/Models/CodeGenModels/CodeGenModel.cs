using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.CodeGenModels
{
    public class CodeGenModel
    {
        public string DbserverIp { get; set; }
        public string DatabaseAdi { get; set; }
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
        public string ProjeAdi { get; set; }

        public string Mesaj { get; set; }

        public List<string> Tablolar { get; set; }
        public string SemaAdi { get; set; }
    }
}