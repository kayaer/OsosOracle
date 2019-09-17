using System.Configuration;
using System.Web.Mvc;

namespace OsosOracle.MvcUI.Localize
{
    public delegate LocalizedString Localizer(string localized, params object[] args);

    public abstract class WebViewBase : WebViewPage<dynamic>
    {
    }

    public abstract class WebViewBase<T> : WebViewPage<T>
    {
        private Localizer _localizer;

        /// <summary>
        /// Translates a keyword or a sentence
        /// </summary>
        public Localizer Word
        {
            get
            {
                return _localizer ?? (_localizer = (word, parameters) =>
                {
                    if (parameters == null || parameters.Length == 0)
                    {
                        return new LocalizedString(word);
                    }
                    return new LocalizedString(string.Format(word, parameters));
                });
            }
        }


        /// <summary>
        /// Translates a keyword or a sentence
        /// </summary>
        public Localizer _T
        {
            get
            {
                return _localizer ?? (_localizer = OnLocalizer);
            }
        }




        private LocalizedString OnLocalizer(string word, object[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return new LocalizedString(word);
            }
            return new LocalizedString(string.Format(word, parameters));
        }

        /// <summary>
        /// seçili dilin kodunu döner
        /// </summary>
        /// 


        public string CurrentLang
        {
            get
            {
                var kod = Request.Cookies["dil"] == null ? ConfigurationManager.AppSettings["defaultLang"] : Request.Cookies["dil"].Value;


                return kod;


            }
        }

    }

}