using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.PRMTARIFEORTAKAVMComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.PRMTARIFEORTAKAVMModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.MvcUI.Filters;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class PRMTARIFEORTAKAVMController : BaseController
    {
        private readonly IPRMTARIFEORTAKAVMService _pRMTARIFEORTAKAVMService;

        public PRMTARIFEORTAKAVMController(IPRMTARIFEORTAKAVMService pRMTARIFEORTAKAVMService)
        {
            _pRMTARIFEORTAKAVMService = pRMTARIFEORTAKAVMService;
        }


        public ActionResult Index()
        {
            SayfaBaslik($"Ortak Avm Tarife İşlemleri");
            var model = new PRMTARIFEORTAKAVMIndexModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, PRMTARIFEORTAKAVMAra pRMTARIFEORTAKAVMAra)
        {

            pRMTARIFEORTAKAVMAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                pRMTARIFEORTAKAVMAra.AD = dtParameterModel.Search.Value;
            }



            var kayitlar = _pRMTARIFEORTAKAVMService.Ara(pRMTARIFEORTAKAVMAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.PRMTARIFEORTAKAVMDetayList.Select(t => new
                {
                    t.Kurum,
                    t.KAYITNO,
                    t.AD,
                    t.YEDEKKREDI,
                    t.ACIKLAMA,
                    t.DURUM,
                    t.FIYAT1,
                    t.FIYAT2,
                    t.FIYAT3,
                    t.FIYAT4,
                    t.LIMIT1,
                    t.LIMIT2,
                    t.CARPAN,
                    t.SAYACCAP,
                    t.SAYACTARIH,
                    t.SAYACTIP,
                    t.TUKETIMKATSAYI,
                    t.KREDIKATSAYI,
                    t.BAYRAM1GUN,
                    t.BAYRAM1AY,
                    t.BAYRAM1SURE,
                    t.BAYRAM2GUN,
                    t.BAYRAM2AY,
                    t.BAYRAM2SURE,
                    t.KURUMKAYITNO,
                    t.AylikBakimBedeli,

                    Islemler = $@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "PRMTARIFEORTAKAVM", new { id = t.KAYITNO })}' title='Düzenle'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "PRMTARIFEORTAKAVM", new { id = t.KAYITNO })}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "PRMTARIFEORTAKAVM", new { id = t.KAYITNO })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Ekle()
        {
            SayfaBaslik($"Ortak Avm Tarife Ekle");

            var model = new PRMTARIFEORTAKAVMKaydetModel
            {
                PRMTARIFEORTAKAVM = new PRMTARIFEORTAKAVM()
            };



            return View("Kaydet", model);
        }


        public ActionResult Guncelle(int id)
        {
            SayfaBaslik($"Ortak Avm Tarife Güncelle");

            var model = new PRMTARIFEORTAKAVMKaydetModel
            {
                PRMTARIFEORTAKAVM = _pRMTARIFEORTAKAVMService.GetirById(id)
            };


            return View("Kaydet", model);
        }



        public ActionResult Detay(int id)
        {
            SayfaBaslik($"Ortak Avm Tarife Detay");

            var model = new PRMTARIFEORTAKAVMDetayModel
            {
                PRMTARIFEORTAKAVMDetay = _pRMTARIFEORTAKAVMService.DetayGetirById(id)
            };


            return View("Detay", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(PRMTARIFEORTAKAVMKaydetModel pRMTARIFEORTAKAVMKaydetModel)
        {
            pRMTARIFEORTAKAVMKaydetModel.PRMTARIFEORTAKAVM.KURUMKAYITNO = AktifKullanici.KurumKayitNo;
            if (pRMTARIFEORTAKAVMKaydetModel.PRMTARIFEORTAKAVM.KAYITNO > 0)
            {
                pRMTARIFEORTAKAVMKaydetModel.PRMTARIFEORTAKAVM.GUNCELLEYEN = AktifKullanici.KayitNo;
                _pRMTARIFEORTAKAVMService.Guncelle(pRMTARIFEORTAKAVMKaydetModel.PRMTARIFEORTAKAVM.List());
            }
            else
            {
                pRMTARIFEORTAKAVMKaydetModel.PRMTARIFEORTAKAVM.OLUSTURAN = AktifKullanici.KayitNo;
                _pRMTARIFEORTAKAVMService.Ekle(pRMTARIFEORTAKAVMKaydetModel.PRMTARIFEORTAKAVM.List());
            }

            return Yonlendir(Url.Action("Index"), $"Ortak Avm Tarife kayıdı başarıyla gerçekleştirilmiştir.");
            //return Yonlendir(Url.Action("Detay","PRMTARIFEORTAKAVM",new{id=pRMTARIFEORTAKAVMKaydetModel.PRMTARIFEORTAKAVM.Id}), $"PRMTARIFEORTAKAVM kayıdı başarıyla gerçekleştirilmiştir.");
        }



        public ActionResult Sil(int id)
        {
            SayfaBaslik($"Ortak Avm Tarife Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel = $"/PRMTARIFEORTAKAVM/Index/{id}" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
            _pRMTARIFEORTAKAVMService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"), $"Ortak Avm Tarife Başarıyla silindi");
        }


        public ActionResult AjaxAra(string key, PRMTARIFEORTAKAVMAra pRMTARIFEORTAKAVMAra = null, int limit = 10, int baslangic = 0)
        {

            if (pRMTARIFEORTAKAVMAra == null)
            {
                pRMTARIFEORTAKAVMAra = new PRMTARIFEORTAKAVMAra();
            }

            pRMTARIFEORTAKAVMAra.Ara = new Ara
            {
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {
                    new Siralama
                    {
                        KolonAdi = LinqExtensions.GetPropertyName((PRMTARIFEORTAKAVM t) => t.KAYITNO),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }
                }
            };


            //TODO: Bu bölümü düzenle
            pRMTARIFEORTAKAVMAra.AD = key;

            var pRMTARIFEORTAKAVMList = _pRMTARIFEORTAKAVMService.Getir(pRMTARIFEORTAKAVMAra);


            var data = pRMTARIFEORTAKAVMList.Select(pRMTARIFEORTAKAVM => new AutoCompleteData
            {
                //TODO: Bu bölümü düzenle
                id = pRMTARIFEORTAKAVM.KAYITNO.ToString(),
                text = pRMTARIFEORTAKAVM.AD.ToString(),
                description = pRMTARIFEORTAKAVM.AD.ToString(),
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AjaxTekDeger(int id)
        {
            var pRMTARIFEORTAKAVM = _pRMTARIFEORTAKAVMService.GetirById(id);


            var data = new AutoCompleteData
            {//TODO: Bu bölümü düzenle
                id = pRMTARIFEORTAKAVM.KAYITNO.ToString(),
                text = pRMTARIFEORTAKAVM.AD.ToString(),
                description = pRMTARIFEORTAKAVM.AD.ToString(),
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxCokDeger(string id)
        {
            var pRMTARIFEORTAKAVMList = _pRMTARIFEORTAKAVMService.Getir(new PRMTARIFEORTAKAVMAra() { KAYITNOlar = id });


            var data = pRMTARIFEORTAKAVMList.Select(pRMTARIFEORTAKAVM => new AutoCompleteData
            { //TODO: Bu bölümü düzenle
                id = pRMTARIFEORTAKAVM.KAYITNO.ToString(),
                text = pRMTARIFEORTAKAVM.AD.ToString(),
                description = pRMTARIFEORTAKAVM.AD.ToString()
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}

