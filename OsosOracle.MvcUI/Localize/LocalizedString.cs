using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Localize
{
    public class LocalizedString : MarshalByRefObject
    {
        /// <summary>
        /// {0} Language code: ex: tr-TR,en-EN
        /// {1} The Word Code in the table :  LangaugeWords
        /// </summary>


        private readonly string _word;

        public override string ToString()
        {
            return _word;
        }

        public LocalizedString(string code)
        {

            //if (ConfigurationManager.AppSettings["MultiLanguageSupport"] == "true")
            //    _word = CacheManager.Kelime(code).Deger;
            //else

            _word = code;


        }

    }
}