﻿using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.CSTSAYACMODELComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.MvcUI.Filters;
using OsosOracle.MvcUI.Models.CSTSAYACMODELModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OsosOracle.MvcUI.Resources;

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
                    t.SayacTuru,
                    t.AD,
                    t.ACIKLAMA,
                    Islemler = $@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "SayacModel", new { id = t.KAYITNO })}' title='{Dil.Duzenle}'><i class='fa fa-edit'></i></a>							 
								<a class='btn btn-xs btn-danger modalizer ' href='{Url.Action("Sil", "SayacModel", new { id = t.KAYITNO })}' title='{Dil.Sil}'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Sil(int id)
        {
            SayfaBaslik("Sayaç Model Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel { Id = id, RedirectUrlForCancel = $"/SayacModel/Index" });
        }

        public ActionResult Ekle(int? Id)
        {
            var model = new CSTSAYACMODELKaydetModel
            {
                CSTSAYACMODEL = new CSTSAYACMODEL()
            };

            return View("Kaydet", model);
        }


        public ActionResult Guncelle(int Id)
        {
            var model = new CSTSAYACMODELKaydetModel
            {
                CSTSAYACMODEL = _cstSayacModelService.GetirById(Id)
            };


            return View("Kaydet", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(CSTSAYACMODELKaydetModel sayacModel)
        {

            sayacModel.CSTSAYACMODEL.DURUM = 1;
            if (sayacModel.CSTSAYACMODEL.KAYITNO > 0)
            {
                sayacModel.CSTSAYACMODEL.GUNCELLEYEN = AktifKullanici.KayitNo;
                _cstSayacModelService.Guncelle(sayacModel.CSTSAYACMODEL.List());
            }
            else
            {
                sayacModel.CSTSAYACMODEL.OLUSTURAN = AktifKullanici.KayitNo;
                _cstSayacModelService.Ekle(sayacModel.CSTSAYACMODEL.List());
            }

            return Yonlendir(Url.Action("Index", "SayacModel"), $"Sayaç Model kayıdı başarıyla gerçekleştirilmiştir.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {

            _cstSayacModelService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"), "Sayaç Model Başarıyla Silindi");
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