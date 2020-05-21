using FluentValidation.Mvc;
using OsosOracle.Business.DependencyResolvers.Ninject;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.CrossCuttingConcern.Logging;
using OsosOracle.Framework.ModelBinders;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.MvcUI.App_Start;
using OsosOracle.MvcUI.Infrastructure;
using OsosOracle.MvcUI.Resources;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace OsosOracle.MvcUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
            ModelValidatorProviders.Providers.Clear();
            ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider
            {
                AddImplicitRequiredValidator = false,
                ValidatorFactory = new NinjectValidatorFactory()
            });
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(typeof(DtParameterModel), new DtModelBinder());

            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());

            ModelValidatorProviders.Providers.Clear();
            ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider
            {
                AddImplicitRequiredValidator = false,
                ValidatorFactory = new NinjectValidatorFactory()
            });

            System.Web.Mvc.ModelBinders.Binders.DefaultBinder = new NoValidationDefaultModelBinder();

        }

        protected void Application_BeginRequest()
        {
            Response.AddHeader("X-Frame-Options", "SAMEORIGIN"); // sadece uygulama içinden iframe ile sayfa açmaya izin verir. DENY iframe ile sayfa açmayı engeller
        }


        private static bool IsAjax(ExceptionContext filterContext)
        {
            return filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

            var loglandiMi = false;

            // Get the exception object.
            Exception hata = Server.GetLastError();

            Response.Clear();


            HttpException httpException = hata as HttpException; //http hataları
            var statusCode = httpException?.GetHttpCode() ?? 500;

            #region Loglama
            NotificationException bilgiException = hata as NotificationException;

            //NotificationException loglama
            if (bilgiException == null)
            {
                loglandiMi = ErrorLogger.HataLogla(hata);
            }
            else //NotificationException içinde exception varsa logla
            {
                if (bilgiException.InnerException != null)
                {
                    loglandiMi = ErrorLogger.HataLogla(bilgiException.InnerException);
                }
            }
            #endregion



            // Clear the error on server.
            Server.ClearError();

            // Avoid IIS7 getting in the middle
            Response.TrySkipIisCustomErrors = true;

            #region AJAX requestler

            bool isAjaxCall = string.Equals("XMLHttpRequest", Context.Request.Headers["x-requested-with"], StringComparison.OrdinalIgnoreCase);


            Context.ClearError();
            if (isAjaxCall)
            {
                var hataMesaji = new AjaxSonuc()
                {
                    Mesaj = hata.Message,
                    Durum = 0,
                    Kod = 0,
                    RedirectUrl = hata.HelpLink ?? "",
                    Baslik=Dil.Uyari
                };

                if (bilgiException != null)
                {
                    var hataDetay = hata.Message.Split('|');
                    hataMesaji.Mesaj = hataDetay.First();
                    hataMesaji.Kod = hataDetay.Last().ToInt32();
                }

                Context.Response.ContentType = "application/json";
                Context.Response.StatusCode = statusCode;
                Context.Response.Write(
                    new JavaScriptSerializer().Serialize(
                       hataMesaji
                    )
                );
            }

            #endregion

            #region diğer requestler

            else
            {
                Response.ContentType = "text/HTML";
                Context.Response.StatusCode = statusCode;

                var logMesaj = loglandiMi
                    ? "<p>Sorun sistem tarafından yöneticiye bildirildi.<p> "
                    : "<p class=\"text-danger\">Hata sistem tarafından yöneticiye <b>bildirilemedi</b>, Lütfen bu hatayı bildiriniz<p>";
                Response.Write($@"<!DOCTYPE html>
<html>
<head>
<meta charset=""utf-8""/>
<title>Hata oluştu!</title>
<link href=""/Content/lib/bootstrap/css/bootstrap.min.css"" rel=""stylesheet"" />
</head>
<body>
<div class=""container"" style=""width:80%; margin:60px auto;"">

    <div class=""row"" >
        <div class=""col-md-12 text-center"" >
            <div class=""error-template"" >

            <span class=""glyphicon glyphicon-random"" style=""font-size:120px;transform: rotate(-28deg);"" ></span>
<h2>{statusCode}<h2>
            <h4>Bişeyler ters gitti..</h4>
            <p>{hata.Message}</p>
{logMesaj}
            <div class=""error-actions"" >
                <a href=""\"" class=""btn btn-info"" > <span class=""glyphicon glyphicon-home"" ></span>  Baştan Başla</a>
                 <a href=""javascript:window.history.back();"" class=""btn btn-primary"" ><span class=""glyphicon glyphicon-arrow-left"" ></span>  Geri</a>
            </div>
            </div>
        </div>
    </div>
</div>
</body>
</html>");
            }

            #endregion










        }





    }
}
