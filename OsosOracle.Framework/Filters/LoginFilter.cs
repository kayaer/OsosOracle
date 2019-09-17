using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;


namespace OsosOracle.Framework.Filters
{
    public class LoginFilterAttribute : IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {


            object[] noYetkiSayfa =
                filterContext.Controller.GetType().GetCustomAttributes(typeof(NoLoginAttribute), true);

            object[] noYetkiAction = filterContext.ActionDescriptor.GetCustomAttributes(
                typeof(NoLoginAttribute),
                true);

            if (noYetkiAction.Length > 0 || noYetkiSayfa.Length > 0) return;

            //YetkiAraci.OnAuthentication();



            string redirectLocation = HttpContext.Current.Response.RedirectLocation;
            if (redirectLocation != null)
            {
                var isAjax = HttpContext.Current.Request.IsAjaxRequest();
                HttpContext.Current.Response.Clear();
                //ajax ile işlem yaparken session düştü ise
                if (isAjax)
                {
                    //var sonuc = JsonConvert.SerializeObject(new
                    //    AjaxSonuc()
                    //    {Durum = 1, Mesaj = "Yönlendiriliyorsunuz...", RedirectUrl = redirectLocation});

                    //HttpContext.Current.Response.RedirectLocation = null;
                    //HttpContext.Current.Response.Write(sonuc);
                    //HttpContext.Current.Response.Write(sonuc);

                    throw new NotificationException("Oturumunuz sonlandı, tekrar giriş yapınız ", redirectUrl: redirectLocation);

                }
                else
                {
                    filterContext.Result = new RedirectResult(redirectLocation);
                }

            }

        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            //
        }
    }

    public class NoLoginAttribute : Attribute { }
}
