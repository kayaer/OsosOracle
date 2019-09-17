using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.SYSOPERASYONGOREVComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.SYSOPERASYONGOREVModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.MvcUI.Filters;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class SYSOPERASYONGOREVController : BaseController
    {
        private readonly ISYSOPERASYONGOREVService _sYSOPERASYONGOREVService;

        public SYSOPERASYONGOREVController(ISYSOPERASYONGOREVService sYSOPERASYONGOREVService)
        {
            _sYSOPERASYONGOREVService = sYSOPERASYONGOREVService;
        }


        public ActionResult Index()
        {
            SayfaBaslik($"SYSOPERASYONGOREV İşlemleri");
            var model = new SYSOPERASYONGOREVIndexModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, SYSOPERASYONGOREVAra sYSOPERASYONGOREVAra)
        {

            sYSOPERASYONGOREVAra.Ara = dtParameterModel.AramaKriteri;

            //if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            //{ //TODO: Bu bölümü düzenle
            //	sYSOPERASYONGOREVAra.A = dtParameterModel.Search.Value;
            //}



            var kayitlar = _sYSOPERASYONGOREVService.Ara(sYSOPERASYONGOREVAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.SYSOPERASYONGOREVDetayList.Select(t => new
                {
                    //TODO: Bu bölümü düzenle
                    t.KAYITNO,
                    t.OPERASYONKAYITNO,
                    t.GOREVKAYITNO,
                    t.VERSIYON,

                    Islemler = $@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "SYSOPERASYONGOREV", new { id = t.KAYITNO })}' title='Düzenle'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "SYSOPERASYONGOREV", new { id = t.KAYITNO })}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "SYSOPERASYONGOREV", new { id = t.KAYITNO })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Ekle()
        {
            SayfaBaslik($"SYSOPERASYONGOREV Ekle");

            var model = new SYSOPERASYONGOREVKaydetModel
            {
                SYSOPERASYONGOREV = new SYSOPERASYONGOREV()
            };



            return View("Kaydet", model);
        }


        public ActionResult Guncelle(int id)
        {
            SayfaBaslik($"SYSOPERASYONGOREV Güncelle");

            var model = new SYSOPERASYONGOREVKaydetModel
            {
                SYSOPERASYONGOREV = _sYSOPERASYONGOREVService.GetirById(id)
            };


            return View("Kaydet", model);
        }



        public ActionResult Detay(int id)
        {
            SayfaBaslik($"SYSOPERASYONGOREV Detay");

            var model = new SYSOPERASYONGOREVDetayModel
            {
                SYSOPERASYONGOREVDetay = _sYSOPERASYONGOREVService.DetayGetirById(id)
            };


            return View("Detay", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(SYSOPERASYONGOREVKaydetModel sYSOPERASYONGOREVKaydetModel)
        {
            if (sYSOPERASYONGOREVKaydetModel.SYSOPERASYONGOREV.KAYITNO > 0)
            {
                _sYSOPERASYONGOREVService.Guncelle(sYSOPERASYONGOREVKaydetModel.SYSOPERASYONGOREV.List());
            }
            else
            {
                _sYSOPERASYONGOREVService.Ekle(sYSOPERASYONGOREVKaydetModel.SYSOPERASYONGOREV.List());
            }

            return Yonlendir(Url.Action("Index"), $"SYSOPERASYONGOREV kayıdı başarıyla gerçekleştirilmiştir.");

        }



        public ActionResult Sil(int id)
        {
            SayfaBaslik($"SYSOPERASYONGOREV Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel = $"/SYSOPERASYONGOREV/Index/{id}" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
            _sYSOPERASYONGOREVService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"), $"SYSOPERASYONGOREV Başarıyla silindi");
        }


        //public ActionResult AjaxAra(string key, SYSOPERASYONGOREVAra sYSOPERASYONGOREVAra = null, int limit = 10, int baslangic = 0)
        //{

        //    if (sYSOPERASYONGOREVAra == null)
        //    {
        //        sYSOPERASYONGOREVAra = new SYSOPERASYONGOREVAra();
        //    }

        //    sYSOPERASYONGOREVAra.Ara = new Ara
        //    {
        //        Baslangic = baslangic,
        //        Uzunluk = limit,
        //        Siralama = new List<Siralama>
        //        {
        //            new Siralama
        //            {
        //                KolonAdi = LinqExtensions.GetPropertyName((SYSOPERASYONGOREV t) => t.KAYITNO),
        //                SiralamaTipi = EnumSiralamaTuru.Asc
        //            }
        //        }
        //    };


        //    //TODO: Bu bölümü düzenle
        //    sYSOPERASYONGOREVAra.Adi = key;

        //    var sYSOPERASYONGOREVList = _sYSOPERASYONGOREVService.Getir(sYSOPERASYONGOREVAra);


        //    var data = sYSOPERASYONGOREVList.Select(sYSOPERASYONGOREV => new AutoCompleteData
        //    {
        //        //TODO: Bu bölümü düzenle
        //        id = sYSOPERASYONGOREV.Id.ToString(),
        //        text = sYSOPERASYONGOREV.Id.ToString(),
        //        description = sYSOPERASYONGOREV.Id.ToString(),
        //    }).ToList();
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}


        //public ActionResult AjaxTekDeger(int id)
        //{
        //    var sYSOPERASYONGOREV = _sYSOPERASYONGOREVService.GetirById(id);


        //    var data = new AutoCompleteData
        //    {//TODO: Bu bölümü düzenle
        //        id = sYSOPERASYONGOREV.Id.ToString(),
        //        text = sYSOPERASYONGOREV.Id.ToString(),
        //        description = sYSOPERASYONGOREV.Id.ToString(),
        //    };

        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult AjaxCokDeger(string id)
        //{
        //    var sYSOPERASYONGOREVList = _sYSOPERASYONGOREVService.Getir(new SYSOPERASYONGOREVAra() { KAYITNOlar = id });


        //    var data = sYSOPERASYONGOREVList.Select(sYSOPERASYONGOREV => new AutoCompleteData
        //    { //TODO: Bu bölümü düzenle
        //        id = sYSOPERASYONGOREV.Id.ToString(),
        //        text = sYSOPERASYONGOREV.Id.ToString(),
        //        description = sYSOPERASYONGOREV.Id.ToString()
        //    });

        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

    }
}

