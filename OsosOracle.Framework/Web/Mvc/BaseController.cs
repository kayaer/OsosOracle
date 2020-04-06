using OsosOracle.Framework.Entities;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Newtonsoft.Json;
using OsosOracle.Framework.SharedModels;
using System.Text;
using OsosOracle.Framework.CrossCuttingConcern.Caching.Microsoft;
using System.ComponentModel;
using static System.String;
using OsosOracle.Framework.Filters;
using System.Threading;
using System.Globalization;

namespace OsosOracle.Framework.Web.Mvc
{
    public class BaseController : Controller
    {
        protected AktifKullanici AktifKullanici
        {
            get
            {
                return (AktifKullanici)Session["User"];
            }

            set
            {
                Session["User"] = value;
            }
        }

        public enum MesajTipi
        {
            Error = 0,
            Success = 1,
            Info = 2,
            Warning = 3
        }

        public class Mesaj
        {


            public bool AutoHide { get; set; }
            public string Msg { get; set; }
            public MesajTipi Tip { get; set; }
        }
        protected void SayfaBaslik(string baslik)
        {
            ViewBag.Title = baslik;
        }
        /// <summary>
        /// sayfa içinde mesaj gösterme işlemi yapar
        /// </summary>
        /// <param name="mesaj">Mesaj içeriği HTML olabilir</param>
        /// <param name="mesajTipi">Mesaj tipi hata uyarı bilgi başarı</param>
        /// <param name="otoGizle">bir süre sonra otomatik gizlensin mi? 15 sn</param>
        protected virtual void MesajGoster(string mesaj, MesajTipi mesajTipi = MesajTipi.Info, bool otoGizle = true)
        {
            string dataKey = "Mesajlar";
            var webMesaj = new Mesaj()
            {
                Msg = mesaj,
                AutoHide = otoGizle,
                Tip = mesajTipi
            };

            if (TempData[dataKey] == null)
            {
                TempData[dataKey] = new List<Mesaj> { webMesaj };
            }
            else
            {
                var mesajlar = (List<Mesaj>)TempData[dataKey];
                mesajlar.Add(webMesaj);
                TempData[dataKey] = mesajlar;
            }

        }

        public ContentResult Yonlendir(string url, string mesaj = "", int durum = 1)
        {
            if (!IsNullOrEmpty(mesaj))
                MesajGoster(mesaj, durum == 1 ? MesajTipi.Success : MesajTipi.Error);

            if (IsAjax())
            {
                return new ContentResult
                {

                    Content = JsonConvert.SerializeObject(new
                        AjaxSonuc()
                    { Durum = durum, Mesaj = mesaj, RedirectUrl = url }),
                    ContentType = "application/json"

                };
            }

            return new ContentResult
            {


                Content = $"<script type='text/javascript'>window.parent.location.href = '{url}';</script>",
                ContentType = "text/html"
            };

        }

        public ContentResult JsonContent(object o)
        {
            return new ContentResult()
            {
                Content = JsonConvert.SerializeObject(o),
                ContentEncoding = Encoding.UTF8,
                ContentType = "application/json"
            };

        }
        private bool IsAjax()
        {
            return HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        public string ReturnUrl
        {
            get
            {
                if (!IsNullOrEmpty(Request.QueryString["returnurl"]))
                {
                    return Request.QueryString["returnurl"];
                }
                return $"/{ControllerContext.RouteData.Values["controller"]}";// ControllerContext.Controller.ToString();
            }
        }

        /// <summary>
        /// belirtilen anahtardaki cacheyi siler
        /// diğer sunuculardan istek gelebileceği için Nologin eklendi
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [NoLogin]
        public ActionResult CacheSil(string key)
        {
            if (!IsNullOrEmpty(key))
            {
                var cacheManager = new MemoryCacheManager();

                cacheManager.RemoveByPattern(key);
            }
            return Json(new AjaxSonuc() { Mesaj = "OK", Durum = 1, RedirectUrl = "" }, JsonRequestBehavior.AllowGet);
        }

        public string _T(string s, object[] args = null)
        {
            return s;// "";
            //  return Language.Get(s, args); 
        }
        public string SeciliDil()
        {
            return Request.Cookies["dil"] != null ? Request.Cookies["dil"].Value : (ConfigurationManager.AppSettings["varsayilanDil"] != null ? ConfigurationManager.AppSettings["varsayilanDil"] : "tr");
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (Session["CurrentCulture"] != null)
            {
                //Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["CurrentCulture"].ToString());
                Thread.CurrentThread.CurrentCulture = new CultureInfo("tr"); //new CultureInfo(Session["CurrentCulture"].ToString());
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Session["CurrentCulture"].ToString());
            }
            else
            {
                Session["CurrentCulture"] = "tr";

            }
        }
    }
}
