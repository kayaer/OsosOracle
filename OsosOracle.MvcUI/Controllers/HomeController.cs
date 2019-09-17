using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.ENTHUSAYACComplexTypes;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.MvcUI.Filters;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class HomeController : BaseController
    {
        private readonly ISYSMENUService _sysMenuService;
        private readonly IRPTDASHBOARDService _rptDashboardService;
        public HomeController(ISYSMENUService sysMenuService,IRPTDASHBOARDService rptDashboardService)
        {

            _sysMenuService = sysMenuService;
            _rptDashboardService = rptDashboardService;
        }
        public ActionResult Index()
        {
            var user = HttpContext.User.Identity;
            return View();
        }


        [ChildActionOnly]
        public PartialViewResult MenuItems()
        {
            var menuler = _sysMenuService.DetayGetir();
            menuler = menuler.Where(x => x.PARENTKAYITNO == null).ToList();
            foreach (var item in menuler)
            {
                item.Children = _sysMenuService.DetayGetir(new Entities.ComplexType.SYSMENUComplexTypes.SYSMENUAra { PARENTKAYITNO = item.KAYITNO });
            }
            return PartialView("MenuItems", menuler);

        }
        public ActionResult HaberlesenCihazSayisiGetir()
        {
            var kayitlar = _rptDashboardService.Getir(new Entities.ComplexType.RPTDASHBOARDComplexTypes.RPTDASHBOARDAra { KURUMKAYITNO = AktifKullanici.KurumKayitNo }).OrderBy(t => t.TARIH).Take(30);
            var model = kayitlar.Select(t => new { TARIH = t.TARIH.ToString(), t.ADET});
            return Json(model, JsonRequestBehavior.AllowGet);
        }

    }
}