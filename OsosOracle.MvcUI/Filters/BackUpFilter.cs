using Oracle.ManagedDataAccess.Client;
using OsosOracle.Business.Abstract;
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
            string ConnectionString = ConfigurationManager.ConnectionStrings["AppContext"].ConnectionString;
            OracleConnection con = new OracleConnection();
            con.ConnectionString = ConnectionString;
            con.Open();
            string sql = "SELECT * FROM ENTSATIS WHERE ROWNUM=1 ORDER BY KAYITNO DESC ";

            OracleDataAdapter oda = new OracleDataAdapter(sql, con);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            con.Close();
            con.Dispose();


            base.OnActionExecuted(filterContext);
        }

    }
}