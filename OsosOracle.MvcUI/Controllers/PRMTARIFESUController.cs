using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.PRMTARIFESUComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.PRMTARIFESUModels;
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
    public class PRMTARIFESUController : BaseController
    {
        private readonly IPRMTARIFESUService _pRMTARIFESUService;

        public PRMTARIFESUController(IPRMTARIFESUService pRMTARIFESUService)
        {
            _pRMTARIFESUService = pRMTARIFESUService;
        }


        public ActionResult Index()
        {
            SayfaBaslik(Dil.TarifeIslemleri);
            var model = new PRMTARIFESUIndexModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, PRMTARIFESUAra pRMTARIFESUAra)
        {

            pRMTARIFESUAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                pRMTARIFESUAra.AD = dtParameterModel.Search.Value;
            }


            pRMTARIFESUAra.KURUMKAYITNO = AktifKullanici.KurumKayitNo;
            var kayitlar = _pRMTARIFESUService.Ara(pRMTARIFESUAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.PRMTARIFESUDetayList.Select(t => new
                {
                    t.Kurum,
                    t.KAYITNO,
                    t.BORCYUZDE,
                    t.AD,
                    t.YEDEKKREDI,
                    t.ACIKLAMA,
                    t.DURUM,
                    t.FIYAT1,
                    t.FIYAT2,
                    t.FIYAT3,
                    t.FIYAT4,
                    t.FIYAT5,
                    t.LIMIT1,
                    t.LIMIT2,
                    t.LIMIT3,
                    t.LIMIT4,
                    t.TUKETIMKATSAYI,
                    t.KREDIKATSAYI,
                    t.SABITUCRET,
                    t.SAYACCAP,
                    t.AVANSONAY,
                    t.DONEMGUN,
                    t.BAYRAM1GUN,
                    t.BAYRAM1AY,
                    t.BAYRAM1SURE,
                    t.BAYRAM2GUN,
                    t.BAYRAM2AY,
                    t.BAYRAM2SURE,
                    t.MAXDEBI,
                    t.KRITIKKREDI,
                    t.KURUMKAYITNO,
                    t.Ctv,
                    t.Kdv,
                    t.BIRIMFIYAT,
                    Islemler = $@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "PRMTARIFESU", new { id = t.KAYITNO })}' title='{Dil.Duzenle}'><i class='fa fa-edit'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "PRMTARIFESU", new { id = t.KAYITNO })}' title='{Dil.Sil}'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Ekle()
        {
            SayfaBaslik(Dil.TarifeEkle);

            var model = new PRMTARIFESUKaydetModel
            {
                PRMTARIFESU = new PRMTARIFESU()
            };



            return View("Kaydet", model);
        }


        public ActionResult Guncelle(int id)
        {
            SayfaBaslik(Dil.TarifeIslemleri);

            var model = new PRMTARIFESUKaydetModel
            {
                PRMTARIFESU = _pRMTARIFESUService.GetirById(id)
            };


            return View("Kaydet", model);
        }



        public ActionResult Detay(int id)
        {
            SayfaBaslik($"PRMTARIFESU Detay");

            var model = new PRMTARIFESUDetayModel
            {
                PRMTARIFESUDetay = _pRMTARIFESUService.DetayGetirById(id)
            };


            return View("Detay", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(PRMTARIFESUKaydetModel pRMTARIFESUKaydetModel)
        {
            pRMTARIFESUKaydetModel.PRMTARIFESU.KURUMKAYITNO = AktifKullanici.KurumKayitNo;
            pRMTARIFESUKaydetModel.PRMTARIFESU.DURUM = 1;
            if (pRMTARIFESUKaydetModel.PRMTARIFESU.KAYITNO > 0)
            {
                pRMTARIFESUKaydetModel.PRMTARIFESU.GUNCELLEYEN = AktifKullanici.KayitNo;
                _pRMTARIFESUService.Guncelle(pRMTARIFESUKaydetModel.PRMTARIFESU.List());
            }
            else
            {
                pRMTARIFESUKaydetModel.PRMTARIFESU.OLUSTURAN = AktifKullanici.KayitNo;
                _pRMTARIFESUService.Ekle(pRMTARIFESUKaydetModel.PRMTARIFESU.List());
            }

            return Yonlendir(Url.Action("Index"), Dil.Basarili);
            //return Yonlendir(Url.Action("Detay","PRMTARIFESU",new{id=pRMTARIFESUKaydetModel.PRMTARIFESU.Id}), $"PRMTARIFESU kayıdı başarıyla gerçekleştirilmiştir.");
        }



        public ActionResult Sil(int id)
        {
            SayfaBaslik($"PRMTARIFESU Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel = $"/PRMTARIFESU/Index/{id}" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
            _pRMTARIFESUService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"),Dil.Basarili);
        }


        public ActionResult AjaxAra(string key, PRMTARIFESUAra pRMTARIFESUAra = null, int limit = 10, int baslangic = 0)
        {

            if (pRMTARIFESUAra == null)
            {
                pRMTARIFESUAra = new PRMTARIFESUAra();
            }
            pRMTARIFESUAra.KURUMKAYITNO = AktifKullanici.KurumKayitNo;
            pRMTARIFESUAra.Ara = new Ara
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


            //TODO: Bu bölümü düzenle
            pRMTARIFESUAra.AD = key;

            var pRMTARIFESUList = _pRMTARIFESUService.Getir(pRMTARIFESUAra);


            var data = pRMTARIFESUList.Select(pRMTARIFESU => new AutoCompleteData
            {
                //TODO: Bu bölümü düzenle
                id = pRMTARIFESU.KAYITNO.ToString(),
                text = pRMTARIFESU.AD.ToString(),
                description = pRMTARIFESU.AD.ToString(),
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AjaxTekDeger(int id)
        {
            var pRMTARIFESU = _pRMTARIFESUService.GetirById(id);


            var data = new AutoCompleteData
            {//TODO: Bu bölümü düzenle
                id = pRMTARIFESU.KAYITNO.ToString(),
                text = pRMTARIFESU.AD.ToString(),
                description = pRMTARIFESU.AD.ToString(),
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxCokDeger(string id)
        {
            var pRMTARIFESUList = _pRMTARIFESUService.Getir(new PRMTARIFESUAra() { KAYITNOlar = id });


            var data = pRMTARIFESUList.Select(pRMTARIFESU => new AutoCompleteData
            { //TODO: Bu bölümü düzenle
                id = pRMTARIFESU.KAYITNO.ToString(),
                text = pRMTARIFESU.AD.ToString(),
                description = pRMTARIFESU.AD.ToString()
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}

