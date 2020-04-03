using OsosOracle.Framework.CodeGenerator;
using OsosOracle.MvcUI.Filters;
using OsosOracle.MvcUI.Models.CodeGenModels;
using System.Web.Mvc;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class CodeGenController : Controller
    {
        public ActionResult Index()
        {
            var model = new CodeGenModel
            {
                ProjeAdi = "OsosOracle",
                DbserverIp = "192.168.1.143:1521",
                KullaniciAdi = "YONCAPROD",
                Sifre = "prod9872",
                DatabaseAdi = "YONCAPROD"
            };

            return View(model);
        }


        [HttpPost]
        public ActionResult KodUret(CodeGenModel model)
        {

            var codeGen = new CodeGen(model.DbserverIp, model.KullaniciAdi, model.Sifre, model.DatabaseAdi, model.SemaAdi);
            codeGen.KodOlustur(model.ProjeAdi, model.Tablolar);
            model.Mesaj =
                $"Başarıyla üretildi. indrimek için <a class=\"report\" href=\"/CodeGen/Download?name={model.ProjeAdi}.zip\"><b>tıklayınız</b></a>";

            //return Yonlendir(Url.Action("Index", "CodeGen"), model.Mesaj);

            return View("Index", model.Mesaj);
        }

        [HttpPost]
        public ActionResult GetTables(CodeGenModel model)
        {

            var codeGen = new CodeGen(model.DbserverIp, model.KullaniciAdi, model.Sifre, model.DatabaseAdi, model.SemaAdi);
            return Json(codeGen.GetTableList());
        }

        [HttpPost]
        public ActionResult GetViews(CodeGenModel model)
        {
            var codeGen = new CodeGen(model.DbserverIp, model.KullaniciAdi, model.Sifre, model.DatabaseAdi, model.SemaAdi);
            return Json(codeGen.GetViewList());
        }

    }
}