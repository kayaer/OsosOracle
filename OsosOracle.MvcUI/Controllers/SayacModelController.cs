using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.CSTSAYACMODELComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
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
    public class SayacModelController : BaseController
    {
        private readonly ICSTSAYACMODELService _cstSayacModelService;

        public SayacModelController(ICSTSAYACMODELService cstSayacModelService)
        {
            _cstSayacModelService = cstSayacModelService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DataTablesList(DtParameterModel dtParameterModel, CSTSAYACMODELAra cstSayacModelAra)
        {

            cstSayacModelAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                //acigaAlinmaAra.Adi = dtParameterModel.Search.Value;
            }

            var kayitlar = _cstSayacModelService.Ara(cstSayacModelAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.CSTSAYACMODELDetayList.Select(t => new
                {
                    t.KAYITNO,
                    t.AD,
                    t.ACIKLAMA,
                    Islemler = $@"<a class='btn btn-xs btn-info ' href='{Url.Action("Guncelle", "Kurum", new { id = t.KAYITNO })}' title='Düzenle'><i class='fa fa-edit'></i></a>							 
								<a class='btn btn-xs btn-danger modalizer ' href='{Url.Action("Sil", "Kurum", new { id = t.KAYITNO })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxAra(string key, CSTSAYACMODELAra cstSayacModelAra = null, int limit = 10, int baslangic = 0)
        {

            if (cstSayacModelAra == null)
            {
                cstSayacModelAra = new CSTSAYACMODELAra();
            }
            cstSayacModelAra.Ara = new Ara
            {
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {
                    new Siralama
                    {
                        KolonAdi = LinqExtensions.GetPropertyName((CSTSAYACMODEL t) => t.KAYITNO),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }
                }
            };

            cstSayacModelAra.AD = key;

            var gorevdenUzaklastirmaList = _cstSayacModelService.Getir(cstSayacModelAra);


            var data = gorevdenUzaklastirmaList.Select(gorevdenUzaklastirma => new AutoCompleteData
            {
                //TODO: Bu bölümü düzenle
                id = gorevdenUzaklastirma.KAYITNO.ToString(),
                text = gorevdenUzaklastirma.AD.ToString(),
                description = gorevdenUzaklastirma.KAYITNO.ToString(),
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AjaxTekDeger(int id)
        {
            var gorevdenUzaklastirma = _cstSayacModelService.GetirById(id);


            var data = new AutoCompleteData
            {//TODO: Bu bölümü düzenle
                id = gorevdenUzaklastirma.KAYITNO.ToString(),
                text = gorevdenUzaklastirma.AD.ToString(),
                description = gorevdenUzaklastirma.KAYITNO.ToString(),
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxCokDeger(string id)
        {
            var gorevdenUzaklastirmaList = _cstSayacModelService.Getir(new CSTSAYACMODELAra() { KAYITNOlar = id });


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