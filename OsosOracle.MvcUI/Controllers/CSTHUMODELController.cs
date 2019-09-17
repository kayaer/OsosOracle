using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.CSTHUMODELComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.CSTHUMODELModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.MvcUI.Filters;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class CSTHUMODELController : BaseController
    {
        private readonly ICSTHUMODELService _cSTHUMODELService;

        public CSTHUMODELController(ICSTHUMODELService cSTHUMODELService)
        {
            _cSTHUMODELService = cSTHUMODELService;
        }


        public ActionResult Index()
        {
            SayfaBaslik($"Modem Model İşlemleri");
            var model = new CSTHUMODELIndexModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, CSTHUMODELAra cSTHUMODELAra)
        {

            cSTHUMODELAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                cSTHUMODELAra.AD = dtParameterModel.Search.Value;
            }



            var kayitlar = _cSTHUMODELService.Ara(cSTHUMODELAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.CSTHUMODELDetayList.Select(t => new
                {
                    //TODO: Bu bölümü düzenle
                    t.KAYITNO,
                    t.ACIKLAMA,
                    t.DURUM,
                    t.VERSIYON,
                    t.MARKAKAYITNO,
                    t.YAZILIMVERSIYON,
                    t.CONTROLLER,
                    t.AD,

                    Islemler = $@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "CSTHUMODEL", new { id = t.KAYITNO })}' title='Düzenle'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "CSTHUMODEL", new { id = t.KAYITNO })}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "CSTHUMODEL", new { id = t.KAYITNO })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }

     
        public ActionResult Ekle()
        {
            SayfaBaslik($"Modem Model Ekle");

            var model = new CSTHUMODELKaydetModel
            {
                CSTHUMODEL = new CSTHUMODEL()
            };



            return View("Kaydet", model);
        }


        public ActionResult Guncelle(int id)
        {
            SayfaBaslik($"Modem Model Güncelle");

            var model = new CSTHUMODELKaydetModel
            {
                CSTHUMODEL = _cSTHUMODELService.GetirById(id)
            };


            return View("Kaydet", model);
        }



        public ActionResult Detay(int id)
        {
            SayfaBaslik($"Modem Model Detay");

            var model = new CSTHUMODELDetayModel
            {
                CSTHUMODELDetay = _cSTHUMODELService.DetayGetirById(id)
            };


            return View("Detay", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(CSTHUMODELKaydetModel cSTHUMODELKaydetModel)
        {
            cSTHUMODELKaydetModel.CSTHUMODEL.DURUM = 1;
            if (cSTHUMODELKaydetModel.CSTHUMODEL.KAYITNO > 0)
            {
                cSTHUMODELKaydetModel.CSTHUMODEL.GUNCELLEYEN = AktifKullanici.KayitNo;
                _cSTHUMODELService.Guncelle(cSTHUMODELKaydetModel.CSTHUMODEL.List());
            }
            else
            {
                cSTHUMODELKaydetModel.CSTHUMODEL.OLUSTURAN = AktifKullanici.KayitNo;
                _cSTHUMODELService.Ekle(cSTHUMODELKaydetModel.CSTHUMODEL.List());
            }

            return Yonlendir(Url.Action("Index"), $"Modem Model kayıdı başarıyla gerçekleştirilmiştir.");
            //return Yonlendir(Url.Action("Detay","CSTHUMODEL",new{id=cSTHUMODELKaydetModel.CSTHUMODEL.Id}), $"CSTHUMODEL kayıdı başarıyla gerçekleştirilmiştir.");
        }



        public ActionResult Sil(int id)
        {
            SayfaBaslik($"Modem Model Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel = $"/CSTHUMODEL/Index/{id}" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
            _cSTHUMODELService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"), $"Modem Model Başarıyla silindi");
        }


        public ActionResult AjaxAra(string key, CSTHUMODELAra cSTHUMODELAra = null, int limit = 10, int baslangic = 0, int rootId = -1)
        {

            if (cSTHUMODELAra == null)
            {
                cSTHUMODELAra = new CSTHUMODELAra();
            }

            cSTHUMODELAra.Ara = new Ara
            {
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {
                    new Siralama
                    {
                        KolonAdi = LinqExtensions.GetPropertyName((CSTHUMODEL t) => t.KAYITNO),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }
                }
            };


            //TODO: Bu bölümü düzenle
            cSTHUMODELAra.AD = key;

            if (rootId > 0)
            {
                cSTHUMODELAra.MARKAKAYITNO = rootId;
            }
            var cSTHUMODELList = _cSTHUMODELService.Getir(cSTHUMODELAra);


            var data = cSTHUMODELList.Select(cSTHUMODEL => new AutoCompleteData
            {
                //TODO: Bu bölümü düzenle
                id = cSTHUMODEL.KAYITNO.ToString(),
                text = cSTHUMODEL.AD.ToString(),
                description = cSTHUMODEL.KAYITNO.ToString(),
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AjaxTekDeger(int id)
        {
            var cSTHUMODEL = _cSTHUMODELService.GetirById(id);


            var data = new AutoCompleteData
            {//TODO: Bu bölümü düzenle
                id = cSTHUMODEL.KAYITNO.ToString(),
                text = cSTHUMODEL.AD.ToString(),
                description = cSTHUMODEL.KAYITNO.ToString(),
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxCokDeger(string id)
        {
            var cSTHUMODELList = _cSTHUMODELService.Getir(new CSTHUMODELAra() { KAYITNOlar = id });


            var data = cSTHUMODELList.Select(cSTHUMODEL => new AutoCompleteData
            { //TODO: Bu bölümü düzenle
                id = cSTHUMODEL.KAYITNO.ToString(),
                text = cSTHUMODEL.AD.ToString(),
                description = cSTHUMODEL.KAYITNO.ToString()
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}

