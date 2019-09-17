using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.SYSCSTOPERASYONComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.SYSCSTOPERASYONModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.MvcUI.Filters;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class SYSCSTOPERASYONController : BaseController
    {
        private readonly ISYSCSTOPERASYONService _sYSCSTOPERASYONService;

        public SYSCSTOPERASYONController(ISYSCSTOPERASYONService sYSCSTOPERASYONService)
        {
            _sYSCSTOPERASYONService = sYSCSTOPERASYONService;
        }


        public ActionResult Index()
        {
            SayfaBaslik($"Operasyon İşlemleri");
            var model = new SYSCSTOPERASYONIndexModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, SYSCSTOPERASYONAra sYSCSTOPERASYONAra)
        {

            sYSCSTOPERASYONAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                sYSCSTOPERASYONAra.AD = dtParameterModel.Search.Value;
            }



            var kayitlar = _sYSCSTOPERASYONService.Ara(sYSCSTOPERASYONAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.SYSCSTOPERASYONDetayList.Select(t => new
                {
                    //TODO: Bu bölümü düzenle
                    t.KAYITNO,
                    t.AD,
                    t.ACIKLAMA,
                    t.OPERASYONTUR,
                    t.VERSIYON,
                    t.Menu,

                    Islemler = $@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "SYSCSTOPERASYON", new { id = t.KAYITNO })}' title='Düzenle'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "SYSCSTOPERASYON", new { id = t.KAYITNO })}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "SYSCSTOPERASYON", new { id = t.KAYITNO })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Ekle()
        {
            SayfaBaslik($"Operasyon Ekle");

            var model = new SYSCSTOPERASYONKaydetModel
            {
                SYSCSTOPERASYON = new SYSCSTOPERASYON()
            };



            return View("Kaydet", model);
        }


        public ActionResult Guncelle(int id)
        {
            SayfaBaslik($"Operasyon Güncelle");

            var model = new SYSCSTOPERASYONKaydetModel
            {
                SYSCSTOPERASYON = _sYSCSTOPERASYONService.GetirById(id)
            };


            return View("Kaydet", model);
        }



        public ActionResult Detay(int id)
        {
            SayfaBaslik($"Operasyon Detay");

            var model = new SYSCSTOPERASYONDetayModel
            {
                SYSCSTOPERASYONDetay = _sYSCSTOPERASYONService.DetayGetirById(id)
            };


            return View("Detay", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(SYSCSTOPERASYONKaydetModel sYSCSTOPERASYONKaydetModel)
        {
            sYSCSTOPERASYONKaydetModel.SYSCSTOPERASYON.OPERASYONTUR = 4;

            if (sYSCSTOPERASYONKaydetModel.SYSCSTOPERASYON.KAYITNO > 0)
            {
                _sYSCSTOPERASYONService.Guncelle(sYSCSTOPERASYONKaydetModel.SYSCSTOPERASYON.List());
            }
            else
            {
                _sYSCSTOPERASYONService.Ekle(sYSCSTOPERASYONKaydetModel.SYSCSTOPERASYON.List());
            }

            return Yonlendir(Url.Action("Index"), $"Operasyon kayıdı başarıyla gerçekleştirilmiştir.");
            //return Yonlendir(Url.Action("Detay","SYSCSTOPERASYON",new{id=sYSCSTOPERASYONKaydetModel.SYSCSTOPERASYON.Id}), $"SYSCSTOPERASYON kayıdı başarıyla gerçekleştirilmiştir.");
        }



        public ActionResult Sil(int id)
        {
            SayfaBaslik($"Operasyon Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel = $"/SYSCSTOPERASYON/Index/{id}" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
            _sYSCSTOPERASYONService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"), $"Operasyon Başarıyla silindi");
        }


        public ActionResult AjaxAra(string key, SYSCSTOPERASYONAra sYSCSTOPERASYONAra = null, int limit = 10, int baslangic = 0)
        {

            if (sYSCSTOPERASYONAra == null)
            {
                sYSCSTOPERASYONAra = new SYSCSTOPERASYONAra();
            }

            sYSCSTOPERASYONAra.Ara = new Ara
            {
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {
                    new Siralama
                    {
                        KolonAdi = LinqExtensions.GetPropertyName((SYSCSTOPERASYON t) => t.KAYITNO),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }
                }
            };


            //TODO: Bu bölümü düzenle
            sYSCSTOPERASYONAra.AD = key;

            var sYSCSTOPERASYONList = _sYSCSTOPERASYONService.Getir(sYSCSTOPERASYONAra);


            var data = sYSCSTOPERASYONList.Select(sYSCSTOPERASYON => new AutoCompleteData
            {
                //TODO: Bu bölümü düzenle
                id = sYSCSTOPERASYON.KAYITNO.ToString(),
                text = sYSCSTOPERASYON.AD.ToString(),
                description = sYSCSTOPERASYON.AD.ToString(),
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AjaxTekDeger(int id)
        {
            var sYSCSTOPERASYON = _sYSCSTOPERASYONService.GetirById(id);


            var data = new AutoCompleteData
            {//TODO: Bu bölümü düzenle
                id = sYSCSTOPERASYON.KAYITNO.ToString(),
                text = sYSCSTOPERASYON.AD.ToString(),
                description = sYSCSTOPERASYON.AD.ToString(),
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxCokDeger(string id)
        {
            var sYSCSTOPERASYONList = _sYSCSTOPERASYONService.Getir(new SYSCSTOPERASYONAra() { KAYITNOlar = id });


            var data = sYSCSTOPERASYONList.Select(sYSCSTOPERASYON => new AutoCompleteData
            { //TODO: Bu bölümü düzenle
                id = sYSCSTOPERASYON.KAYITNO.ToString(),
                text = sYSCSTOPERASYON.AD.ToString(),
                description = sYSCSTOPERASYON.AD.ToString()
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}

