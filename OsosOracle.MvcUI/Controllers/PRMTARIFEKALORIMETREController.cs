using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.PRMTARIFEKALORIMETREComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.PRMTARIFEKALORIMETREModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.MvcUI.Filters;
using System;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class PRMTARIFEKALORIMETREController : BaseController
    {
        private readonly IPRMTARIFEKALORIMETREService _pRMTARIFEKALORIMETREService;

        public PRMTARIFEKALORIMETREController(IPRMTARIFEKALORIMETREService pRMTARIFEKALORIMETREService)
        {
            _pRMTARIFEKALORIMETREService = pRMTARIFEKALORIMETREService;
        }


        public ActionResult Index()
        {
            SayfaBaslik($"Ortak Avm Tarife İşlemleri");
            var model = new PRMTARIFEKALORIMETREIndexModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, PRMTARIFEKALORIMETREAra pRMTARIFEKALORIMETREAra)
        {

            pRMTARIFEKALORIMETREAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                pRMTARIFEKALORIMETREAra.AD = dtParameterModel.Search.Value;
            }


            pRMTARIFEKALORIMETREAra.KURUMKAYITNO = AktifKullanici.KurumKayitNo;
            var kayitlar = _pRMTARIFEKALORIMETREService.Ara(pRMTARIFEKALORIMETREAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.PRMTARIFEKALORIMETREDetayList.Select(t => new
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
                    t.Kdv,
                    t.Ctv,
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
                    t.BirimFiyat,
                    t.RezervKredi,

                    Islemler = $@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "PRMTARIFEKALORIMETRE", new { id = t.KAYITNO })}' title='Düzenle'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "PRMTARIFEKALORIMETRE", new { id = t.KAYITNO })}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "PRMTARIFEKALORIMETRE", new { id = t.KAYITNO })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Ekle()
        {
            SayfaBaslik($"Ortak Avm Tarife Ekle");

            var model = new PRMTARIFEKALORIMETREKaydetModel
            {
                PRMTARIFEKALORIMETRE = new PRMTARIFEKALORIMETRE()
            };



            return View("Kaydet", model);
        }


        public ActionResult Guncelle(int id)
        {
            SayfaBaslik($"Ortak Avm Tarife Güncelle");

            var model = new PRMTARIFEKALORIMETREKaydetModel
            {
                PRMTARIFEKALORIMETRE = _pRMTARIFEKALORIMETREService.GetirById(id)
            };


            return View("Kaydet", model);
        }



        public ActionResult Detay(int id)
        {
            SayfaBaslik($"Ortak Avm Tarife Detay");

            var model = new PRMTARIFEKALORIMETREDetayModel
            {
                PRMTARIFEKALORIMETREDetay = _pRMTARIFEKALORIMETREService.DetayGetirById(id)
            };


            return View("Detay", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(PRMTARIFEKALORIMETREKaydetModel pRMTARIFEKALORIMETREKaydetModel)
        {
            try
            {
                pRMTARIFEKALORIMETREKaydetModel.PRMTARIFEKALORIMETRE.KURUMKAYITNO = AktifKullanici.KurumKayitNo;
                if (pRMTARIFEKALORIMETREKaydetModel.PRMTARIFEKALORIMETRE.KAYITNO > 0)
                {
                    pRMTARIFEKALORIMETREKaydetModel.PRMTARIFEKALORIMETRE.GUNCELLEYEN = AktifKullanici.KayitNo;
                    _pRMTARIFEKALORIMETREService.Guncelle(pRMTARIFEKALORIMETREKaydetModel.PRMTARIFEKALORIMETRE.List());
                }
                else
                {
                    pRMTARIFEKALORIMETREKaydetModel.PRMTARIFEKALORIMETRE.OLUSTURAN = AktifKullanici.KayitNo;
                    _pRMTARIFEKALORIMETREService.Ekle(pRMTARIFEKALORIMETREKaydetModel.PRMTARIFEKALORIMETRE.List());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           

            return Yonlendir(Url.Action("Index"), $"Ortak Avm Tarife kayıdı başarıyla gerçekleştirilmiştir.");
            //return Yonlendir(Url.Action("Detay","PRMTARIFEKALORIMETRE",new{id=pRMTARIFEKALORIMETREKaydetModel.PRMTARIFEKALORIMETRE.Id}), $"PRMTARIFEKALORIMETRE kayıdı başarıyla gerçekleştirilmiştir.");
        }



        public ActionResult Sil(int id)
        {
            SayfaBaslik($"Ortak Avm Tarife Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel = $"/PRMTARIFEKALORIMETRE/Index/{id}" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
            _pRMTARIFEKALORIMETREService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"), $"Ortak Avm Tarife Başarıyla silindi");
        }


        public ActionResult AjaxAra(string key, PRMTARIFEKALORIMETREAra pRMTARIFEKALORIMETREAra = null, int limit = 10, int baslangic = 0)
        {

            if (pRMTARIFEKALORIMETREAra == null)
            {
                pRMTARIFEKALORIMETREAra = new PRMTARIFEKALORIMETREAra();
            }
            pRMTARIFEKALORIMETREAra.KURUMKAYITNO = AktifKullanici.KurumKayitNo;
            pRMTARIFEKALORIMETREAra.Ara = new Ara
            {
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {
                    new Siralama
                    {
                        KolonAdi = LinqExtensions.GetPropertyName((PRMTARIFEKALORIMETRE t) => t.KAYITNO),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }
                }
            };


            //TODO: Bu bölümü düzenle
            pRMTARIFEKALORIMETREAra.AD = key;

            var pRMTARIFEKALORIMETREList = _pRMTARIFEKALORIMETREService.Getir(pRMTARIFEKALORIMETREAra);


            var data = pRMTARIFEKALORIMETREList.Select(pRMTARIFEKALORIMETRE => new AutoCompleteData
            {
                //TODO: Bu bölümü düzenle
                id = pRMTARIFEKALORIMETRE.KAYITNO.ToString(),
                text = pRMTARIFEKALORIMETRE.AD.ToString(),
                description = pRMTARIFEKALORIMETRE.AD.ToString(),
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AjaxTekDeger(int id)
        {
            var pRMTARIFEKALORIMETRE = _pRMTARIFEKALORIMETREService.GetirById(id);


            var data = new AutoCompleteData
            {//TODO: Bu bölümü düzenle
                id = pRMTARIFEKALORIMETRE.KAYITNO.ToString(),
                text = pRMTARIFEKALORIMETRE.AD.ToString(),
                description = pRMTARIFEKALORIMETRE.AD.ToString(),
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxCokDeger(string id)
        {
            var pRMTARIFEKALORIMETREList = _pRMTARIFEKALORIMETREService.Getir(new PRMTARIFEKALORIMETREAra() { KAYITNOlar = id });


            var data = pRMTARIFEKALORIMETREList.Select(pRMTARIFEKALORIMETRE => new AutoCompleteData
            { //TODO: Bu bölümü düzenle
                id = pRMTARIFEKALORIMETRE.KAYITNO.ToString(),
                text = pRMTARIFEKALORIMETRE.AD.ToString(),
                description = pRMTARIFEKALORIMETRE.AD.ToString()
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}

