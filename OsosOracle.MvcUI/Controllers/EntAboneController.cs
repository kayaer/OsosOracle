using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.ENTABONEComplexTypes;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.MvcUI.Filters;
using OsosOracle.MvcUI.Models.ENTABONEModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OsosOracle.MvcUI.Resources;
namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class EntAboneController : BaseController
    {
        private readonly IENTABONEService _entAboneService;

        public EntAboneController(IENTABONEService entAboneService)
        {
            _entAboneService = entAboneService;     
        }

        public ActionResult AboneSec()
        {
            SayfaBaslik(Dil.AboneBilgileri);
           
            var model = new AboneBilgileriModel
            {
                AboneDetay = new ENTABONEDetay()
            };

            return View(model);
        }
        public ActionResult FotografGoster(int id)
        {
            //var img = _dosyaService.GetListByTipRetDosya(NesneTipi.PersonelDosyaTuru.GetHashCode(), id).FirstOrDefault();

            //if (img != null)
            //    return File(img.Icerik, "image/jpg");

            return File("/Content/img/resimyok.jpg", "image/jpg");
        }
        public ActionResult AboneTumBilgileri(int AboneKayitNo, string activeTab = "", string returnUrl = "")
        {
            SayfaBaslik(Dil.AboneBilgileri);

            var model = new AboneBilgileriModel
            {
                AboneDetay = new ENTABONEDetay
                {
                    KAYITNO = AboneKayitNo
                },
                ActiveTab = activeTab,
                ReturnUrl = returnUrl,
                KurumKayitNo=AktifKullanici.KurumKayitNo
            };

            return View("AboneTumBilgileri", model);
        }

        public PartialViewResult AboneGenelPartial(AboneGenelPartialModel genelPartial)
        {
            SayfaBaslik(Dil.AboneBilgileri);

            genelPartial.AboneGenel = _entAboneService.AboneGenelBilgileriGetir(genelPartial.AboneKayitNo);

            return PartialView(genelPartial);
        }

    }
}