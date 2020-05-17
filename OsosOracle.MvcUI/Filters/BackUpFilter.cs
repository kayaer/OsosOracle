using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using OsosOracle.Business.Abstract;
using OsosOracle.MvcUI.Helpers;
using OsosOracle.MvcUI.RabbitMq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OsosOracle.MvcUI.Filters
{
    //Rabbit mq ile satış tablosunun yedeğinin alınması
    public class BackUpFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var SonIslem = OraDbHelper.YapilanSonSatisGetir();
            var data=JsonConvert.SerializeObject(SonIslem);

            Publisher _publisher = new Publisher("ENTSATIS", data);

            base.OnActionExecuted(filterContext);
        }

    }
}