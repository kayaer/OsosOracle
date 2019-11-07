using OsosOracle.Framework.Web.Mvc;
using OsosOracle.MvcUI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class KartController : BaseController
    {
        // GET: Kart
        public ActionResult Index()
        {
            return View();
        }
    }
}