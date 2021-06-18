using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.SYSROLKULLANICIComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.SYSROLKULLANICIModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.MvcUI.Filters;
using OsosOracle.MvcUI.Resources;
namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class SYSROLKULLANICIController : BaseController
    {
        private readonly ISYSROLKULLANICIService _sYSROLKULLANICIService;

        public SYSROLKULLANICIController(ISYSROLKULLANICIService sYSROLKULLANICIService)
        {
            _sYSROLKULLANICIService = sYSROLKULLANICIService;
        }


        public ActionResult Index()
        {
           // SayfaBaslik(Dil.kullanirol);
            var model = new SYSROLKULLANICIIndexModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, SYSROLKULLANICIAra sYSROLKULLANICIAra)
        {

            sYSROLKULLANICIAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
              //sYSROLKULLANICIAra. = dtParameterModel.Search.Value;
            }



            var kayitlar = _sYSROLKULLANICIService.Ara(sYSROLKULLANICIAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.SYSROLKULLANICIDetayList.Select(t => new
                {
                    //TODO: Bu bölümü düzenle
                    t.KAYITNO,
                    t.KULLANICIKAYITNO,
                    t.ROLKAYITNO,
                    t.VERSIYON,
                    t.RolAdi,
                    t.KullaniciAdi,

                    Islemler = $@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "SYSROLKULLANICI", new { id = t.KAYITNO })}' title='Düzenle'><i class='fa fa-edit'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "SYSROLKULLANICI", new { id = t.KAYITNO })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Ekle()
        {
            SayfaBaslik($"Rol-Kullanıcı Ekle");

            var model = new SYSROLKULLANICIKaydetModel
            {
                SYSROLKULLANICI = new SYSROLKULLANICI()
            };



            return View("Kaydet", model);
        }


        public ActionResult Guncelle(int id)
        {
            SayfaBaslik($"Rol-Kullanıcı Güncelle");

            var model = new SYSROLKULLANICIKaydetModel
            {
                SYSROLKULLANICI = _sYSROLKULLANICIService.GetirById(id)
            };


            return View("Kaydet", model);
        }



        public ActionResult Detay(int id)
        {
            SayfaBaslik($"Rol-Kullanıcı Detay");

            var model = new SYSROLKULLANICIDetayModel
            {
                SYSROLKULLANICIDetay = _sYSROLKULLANICIService.DetayGetirById(id)
            };


            return View("Detay", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(SYSROLKULLANICIKaydetModel sYSROLKULLANICIKaydetModel)
        {
            if (sYSROLKULLANICIKaydetModel.SYSROLKULLANICI.KAYITNO > 0)
            {
                sYSROLKULLANICIKaydetModel.SYSROLKULLANICI.GUNCELLEYEN = AktifKullanici.KayitNo;
                _sYSROLKULLANICIService.Guncelle(sYSROLKULLANICIKaydetModel.SYSROLKULLANICI.List());
            }
            else
            {
                sYSROLKULLANICIKaydetModel.SYSROLKULLANICI.OLUSTURAN = AktifKullanici.KayitNo;
                _sYSROLKULLANICIService.Ekle(sYSROLKULLANICIKaydetModel.SYSROLKULLANICI.List());
            }

            return Yonlendir(Url.Action("Index"), $"Kullanıcı-Rol kayıdı başarıyla gerçekleştirilmiştir.");
            //return Yonlendir(Url.Action("Detay","SYSROLKULLANICI",new{id=sYSROLKULLANICIKaydetModel.SYSROLKULLANICI.Id}), $"SYSROLKULLANICI kayıdı başarıyla gerçekleştirilmiştir.");
        }



        public ActionResult Sil(int id)
        {
            SayfaBaslik($"SYSROLKULLANICI Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel = $"/SYSROLKULLANICI/Index/{id}" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
            _sYSROLKULLANICIService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"), $"SYSROLKULLANICI Başarıyla silindi");
        }


        //public ActionResult AjaxAra(string key, SYSROLKULLANICIAra sYSROLKULLANICIAra = null, int limit = 10, int baslangic = 0)
        //{

        //    if (sYSROLKULLANICIAra == null)
        //    {
        //        sYSROLKULLANICIAra = new SYSROLKULLANICIAra();
        //    }

        //    sYSROLKULLANICIAra.Ara = new Ara
        //    {
        //        Baslangic = baslangic,
        //        Uzunluk = limit,
        //        Siralama = new List<Siralama>
        //        {
        //            new Siralama
        //            {
        //                KolonAdi = LinqExtensions.GetPropertyName((SYSROLKULLANICI t) => t.KAYITNO),
        //                SiralamaTipi = EnumSiralamaTuru.Asc
        //            }
        //        }
        //    };


        //    //TODO: Bu bölümü düzenle
        //    //sYSROLKULLANICIAra.Adi = key;

        //    var sYSROLKULLANICIList = _sYSROLKULLANICIService.Getir(sYSROLKULLANICIAra);


        //    var data = sYSROLKULLANICIList.Select(sYSROLKULLANICI => new AutoCompleteData
        //    {
        //        //TODO: Bu bölümü düzenle
        //        id = sYSROLKULLANICI.Id.ToString(),
        //        text = sYSROLKULLANICI.Id.ToString(),
        //        description = sYSROLKULLANICI.Id.ToString(),
        //    }).ToList();
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}


        //public ActionResult AjaxTekDeger(int id)
        //{
        //    var sYSROLKULLANICI = _sYSROLKULLANICIService.GetirById(id);


        //    var data = new AutoCompleteData
        //    {//TODO: Bu bölümü düzenle
        //        id = sYSROLKULLANICI.Id.ToString(),
        //        text = sYSROLKULLANICI.Id.ToString(),
        //        description = sYSROLKULLANICI.Id.ToString(),
        //    };

        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult AjaxCokDeger(string id)
        //{
        //    var sYSROLKULLANICIList = _sYSROLKULLANICIService.Getir(new SYSROLKULLANICIAra() { KAYITNOlar = id });


        //    var data = sYSROLKULLANICIList.Select(sYSROLKULLANICI => new AutoCompleteData
        //    { //TODO: Bu bölümü düzenle
        //        id = sYSROLKULLANICI.Id.ToString(),
        //        text = sYSROLKULLANICI.Id.ToString(),
        //        description = sYSROLKULLANICI.Id.ToString()
        //    });

        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

    }
}

