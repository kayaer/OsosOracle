using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OsosOracle.MvcUI.Filters
{
    public class AuthorizeUser : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            var user = session["User"];

            if (SkipAuthorization(filterContext))
            {
                return;
            }

            if (user == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {

                    filterContext.HttpContext.Response.StatusCode = 403;
                    filterContext.HttpContext.Response.End();
                }
                else
                {
                    RedirectToRouteResult redirectTo = new RedirectToRouteResult("Default", new RouteValueDictionary { { "controller", "Login" }, { "action", "Index" }, { "ReturnUrl", filterContext.HttpContext.Request.RawUrl } });

                    filterContext.Result = redirectTo;
                }
            }
            else
            {
                if (!filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    var authorizeAttributes = filterContext.Controller.GetType().GetCustomAttributes(typeof(AuthorizeAttribute), true);
                    // string controllerName = filterContext.Controller.GetType().FullName;

                    string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                    var areaName = filterContext.RouteData.DataTokens["area"];
                    bool hasPermisson = true;
                    if (areaName != null)
                    {
                        //KullaniciKontrolViewModel userInfo = session["User"] as KullaniciKontrolViewModel;
                        //if (userInfo.KullaniciTur == 0)
                        //{
                        //    string baseurl = ConfigurationManager.AppSettings["BaseURLForMvc"];
                        //    string url = string.Concat(baseurl, "/Gen/HasPagePermission");
                        //    var client = new RestClient(url);
                        //    var request = new RestRequest(Method.GET);
                        //    request.AddParameter("area", areaName);
                        //    request.AddParameter("controller", controllerName);
                        //    request.AddParameter("action", filterContext.ActionDescriptor.ActionName);
                        //    request.AddParameter("userId", userInfo.KayitNo);

                        //    var model = client.Execute(request);

                        //    if (model.Content.Contains("true"))
                        //    {
                        //        hasPermisson = true;
                        //    }
                        //    else
                        //    {
                        //        hasPermisson = false;

                        //    }
                        //}
                        //else
                        //{
                        //    hasPermisson = true;
                        //}


                    }

                    if (!hasPermisson)
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Unauthorised", controller = "Home", area = "" }));
                    }
                }

            }
        }

        private static bool SkipAuthorization(AuthorizationContext filterContext)
        {
            System.Diagnostics.Contracts.Contract.Assert(filterContext != null);

            return filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)
                   || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);
        }
    }
}