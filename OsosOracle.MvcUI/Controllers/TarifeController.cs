using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.PRMTARIFESUComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.MvcUI.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OsosOracle.MvcUI.Controllers
{
    public class TarifeController : BaseController
    {
        private readonly IPRMTARIFESUService _prmTarifeSuService;
        public TarifeController(IPRMTARIFESUService prmTarifeSuService)
        {
            _prmTarifeSuService = prmTarifeSuService;
        }
        public ActionResult Su()
        {
            return View();
        }

        public ActionResult DataTablesList(DtParameterModel dtParameterModel, PRMTARIFESUAra prmTarifeSuAra)
        {

            prmTarifeSuAra.Ara = dtParameterModel.AramaKriteri;

            
            var kayitlar = _prmTarifeSuService.Ara(prmTarifeSuAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.PRMTARIFESUDetayList.Select(t => new
                {
                    t.KAYITNO,
                    t.AD,
                    t.ACIKLAMA,
                    Islemler = $@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "Tarife", new { t.KAYITNO })}' title='{Dil.Duzenle}'><i class='fa fa-edit'></i></a>							 
								<a class='btn btn-xs btn-danger modalizer ' href='{Url.Action("Sil", "Tarife", new { t.KAYITNO })}' title='{Dil.Sil}'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult Sil(int KAYITNO)
        {
            SayfaBaslik($"Tarife Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel() { Id = KAYITNO, RedirectUrlForCancel = string.Format("/Tarife/Index") });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
            _prmTarifeSuService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Su"), $"Tarife Başarıyla silindi");
        }
        public ActionResult SuAjaxAra(string key, PRMTARIFESUAra prmTarifeSuAra = null, int limit = 10, int baslangic = 0)
        {

            if (prmTarifeSuAra == null)
            {
                prmTarifeSuAra = new PRMTARIFESUAra();
            }
            prmTarifeSuAra.Ara = new Ara
            {
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {
                    new Siralama
                    {
                        KolonAdi = LinqExtensions.GetPropertyName((PRMTARIFESU t) => t.KAYITNO),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }
                }
            };

            prmTarifeSuAra.AD = key;

            var sayacList = _prmTarifeSuService.Getir(prmTarifeSuAra);


            var data = sayacList.Select(sayac => new AutoCompleteData
            {
                //TODO: Bu bölümü düzenle
                id = sayac.KAYITNO.ToString(),
                text = sayac.AD.ToString(),
                description = sayac.KAYITNO.ToString(),
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SuAjaxTekDeger(int id)
        {
            var gorevdenUzaklastirma = _prmTarifeSuService.GetirById(id);


            var data = new AutoCompleteData
            {//TODO: Bu bölümü düzenle
                id = gorevdenUzaklastirma.KAYITNO.ToString(),
                text = gorevdenUzaklastirma.AD.ToString(),
                description = gorevdenUzaklastirma.KAYITNO.ToString(),
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SuAjaxCokDeger(string id)
        {
            var gorevdenUzaklastirmaList = _prmTarifeSuService.Getir(new PRMTARIFESUAra() { KAYITNOlar = id });


            var data = gorevdenUzaklastirmaList.Select(gorevdenUzaklastirma => new AutoCompleteData
            { //TODO: Bu bölümü düzenle
                id = gorevdenUzaklastirma.KAYITNO.ToString(),
                text = gorevdenUzaklastirma.AD.ToString(),
                description = gorevdenUzaklastirma.KAYITNO.ToString()
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}