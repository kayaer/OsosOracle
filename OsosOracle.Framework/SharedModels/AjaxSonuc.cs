using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.Framework.SharedModels
{
    public class AjaxSonuc
    {
        public int Durum { get; set; }
        public string Mesaj { get; set; }
        public string RedirectUrl { get; set; }

        /// <summary>
        /// genellikle hata kodu için gerekli
        /// </summary>
        public int Kod { get; set; }

        public string Data { get; set; }

    }
}
