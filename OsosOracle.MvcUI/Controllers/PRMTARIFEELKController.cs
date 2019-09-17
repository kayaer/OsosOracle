using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.PRMTARIFEELKComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.PRMTARIFEELKModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.MvcUI.Filters;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class PRMTARIFEELKController : BaseController
    {
        private readonly IPRMTARIFEELKService _pRMTARIFEELKService;

        public PRMTARIFEELKController(IPRMTARIFEELKService pRMTARIFEELKService)
        {
            _pRMTARIFEELKService = pRMTARIFEELKService;
        }


        public ActionResult Index()
        {
            SayfaBaslik($"Tarife İşlemleri");
            var model = new PRMTARIFEELKIndexModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, PRMTARIFEELKAra pRMTARIFEELKAra)
        {

            pRMTARIFEELKAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                pRMTARIFEELKAra.AD = dtParameterModel.Search.Value;
            }



            var kayitlar = _pRMTARIFEELKService.Ara(pRMTARIFEELKAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.PRMTARIFEELKDetayList.Select(t => new
                {
                    t.Kurum,
                    t.KAYITNO,
                    t.KREDIKATSAYI,
                    t.AD,
                    t.YEDEKKREDI,
                    t.KRITIKKREDI,
                    t.CARPAN,
                    t.ACIKLAMA,
                    t.DURUM,
                    t.FIYAT1,
                    t.FIYAT2,
                    t.FIYAT3,
                    t.LIMIT1,
                    t.LIMIT2,
                    t.YUKLEMELIMIT,
                    t.SABAHSAAT,
                    t.AKSAMSAAT,
                    t.HAFTASONUAKSAM,
                    t.SABITUCRET,
                    t.BAYRAM1GUN,
                    t.BAYRAM1AY,
                    t.BAYRAM1SURE,
                    t.BAYRAM2GUN,
                    t.BAYRAM2AY,
                    t.BAYRAM2SURE,
                    t.KURUMKAYITNO,

                    Islemler = $@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "PRMTARIFEELK", new { id = t.KAYITNO })}' title='Düzenle'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "PRMTARIFEELK", new { id = t.KAYITNO })}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "PRMTARIFEELK", new { id = t.KAYITNO })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Ekle()
        {
            SayfaBaslik($"Tarife Ekle");

            var model = new PRMTARIFEELKKaydetModel
            {
                PRMTARIFEELK = new PRMTARIFEELK()
            };



            return View("Kaydet", model);
        }


        public ActionResult Guncelle(int id)
        {
            SayfaBaslik($"Tarife Güncelle");

            var model = new PRMTARIFEELKKaydetModel
            {
                PRMTARIFEELK = _pRMTARIFEELKService.GetirById(id)
            };


            return View("Kaydet", model);
        }



        public ActionResult Detay(int id)
        {
            SayfaBaslik($"Tarife Detay");

            var model = new PRMTARIFEELKDetayModel
            {
                PRMTARIFEELKDetay = _pRMTARIFEELKService.DetayGetirById(id)
            };


            return View("Detay", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(PRMTARIFEELKKaydetModel pRMTARIFEELKKaydetModel)
        {
            if (pRMTARIFEELKKaydetModel.PRMTARIFEELK.KAYITNO > 0)
            {
                pRMTARIFEELKKaydetModel.PRMTARIFEELK.GUNCELLEYEN = AktifKullanici.KayitNo;
                _pRMTARIFEELKService.Guncelle(pRMTARIFEELKKaydetModel.PRMTARIFEELK.List());
            }
            else
            {
                pRMTARIFEELKKaydetModel.PRMTARIFEELK.OLUSTURAN = AktifKullanici.KayitNo;
                _pRMTARIFEELKService.Ekle(pRMTARIFEELKKaydetModel.PRMTARIFEELK.List());
            }

            return Yonlendir(Url.Action("Index"), $"Tarife kayıdı başarıyla gerçekleştirilmiştir.");
            //return Yonlendir(Url.Action("Detay","PRMTARIFEELK",new{id=pRMTARIFEELKKaydetModel.PRMTARIFEELK.Id}), $"PRMTARIFEELK kayıdı başarıyla gerçekleştirilmiştir.");
        }



        public ActionResult Sil(int id)
        {
            SayfaBaslik($"Tarife Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel = $"/PRMTARIFEELK/Index/{id}" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
            _pRMTARIFEELKService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"), $"PRMTARIFEELK Başarıyla silindi");
        }


        public ActionResult AjaxAra(string key, PRMTARIFEELKAra pRMTARIFEELKAra = null, int limit = 10, int baslangic = 0)
        {

            if (pRMTARIFEELKAra == null)
            {
                pRMTARIFEELKAra = new PRMTARIFEELKAra();
            }

            pRMTARIFEELKAra.Ara = new Ara
            {
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {
                    new Siralama
                    {
                        KolonAdi = LinqExtensions.GetPropertyName((PRMTARIFEELK t) => t.KAYITNO),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }
                }
            };


            //TODO: Bu bölümü düzenle
            pRMTARIFEELKAra.AD = key;

            var pRMTARIFEELKList = _pRMTARIFEELKService.Getir(pRMTARIFEELKAra);


            var data = pRMTARIFEELKList.Select(pRMTARIFEELK => new AutoCompleteData
            {
                //TODO: Bu bölümü düzenle
                id = pRMTARIFEELK.KAYITNO.ToString(),
                text = pRMTARIFEELK.AD.ToString(),
                description = pRMTARIFEELK.AD.ToString(),
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AjaxTekDeger(int id)
        {
            var pRMTARIFEELK = _pRMTARIFEELKService.GetirById(id);


            var data = new AutoCompleteData
            {//TODO: Bu bölümü düzenle
                id = pRMTARIFEELK.KAYITNO.ToString(),
                text = pRMTARIFEELK.AD.ToString(),
                description = pRMTARIFEELK.AD.ToString(),
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxCokDeger(string id)
        {
            var pRMTARIFEELKList = _pRMTARIFEELKService.Getir(new PRMTARIFEELKAra() { KAYITNOlar = id });


            var data = pRMTARIFEELKList.Select(pRMTARIFEELK => new AutoCompleteData
            { //TODO: Bu bölümü düzenle
                id = pRMTARIFEELK.KAYITNO.ToString(),
                text = pRMTARIFEELK.AD.ToString(),
                description = pRMTARIFEELK.AD.ToString()
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}

