using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.ENTABONEComplexTypes;
using OsosOracle.Entities.ComplexType.ENTABONESAYACComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.MvcUI.Filters;
using OsosOracle.MvcUI.Models.ENTABONEModels;
using OsosOracle.MvcUI.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class AboneController : BaseController
    {
        private readonly IENTABONEService _entAboneService;
        private readonly IENTABONESAYACService _entAboneSayacService;
        private readonly IPRMTARIFEELKService _prmTarifeElkService;
        private readonly IPRMTARIFEKALORIMETREService _prmTarifeKALORIMETREService;
        private readonly IPRMTARIFESUService _prmTarifeSuService;
        private readonly IPRMTARIFEGAZService _prmTarifeGazService;
        private readonly IENTSAYACService _entSayacService;

        public AboneController(IENTABONEService entAboneService,
            IENTABONESAYACService entAboneSayacService,
            IPRMTARIFEELKService prmTarifeElkService,
            IENTSAYACService entSayacService,
            IPRMTARIFEKALORIMETREService prmTarifeKALORIMETREService,
            IPRMTARIFESUService prmTarifeSuService,
            IPRMTARIFEGAZService prmTarifeGazService
            )
        {
            _entAboneService = entAboneService;
            _entAboneSayacService = entAboneSayacService;
            _prmTarifeElkService = prmTarifeElkService;
            _entSayacService = entSayacService;
            _prmTarifeKALORIMETREService = prmTarifeKALORIMETREService;
            _prmTarifeSuService = prmTarifeSuService;
            _prmTarifeGazService = prmTarifeGazService;
        }

        public ActionResult Index()
        {
            return View(new ENTABONEIndexModel { KurumKayitNo = AktifKullanici.KurumKayitNo });
        }

        public ActionResult Genel()
        {
            return View(new ENTABONEIndexModel { KurumKayitNo = AktifKullanici.KurumKayitNo });
        }
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, ENTABONEAra entAboneAra)
        {

            entAboneAra.Ara = dtParameterModel.AramaKriteri;
            entAboneAra.KURUMKAYITNO = AktifKullanici.KurumKayitNo;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            {
                entAboneAra.AboneNoVeyAdiVeyaSoyadi = dtParameterModel.Search.Value;
            }

            //entAboneAra.Ara.Siralama = new List<Siralama>
            //{
            //    new Siralama
            //    {
            //        KolonAdi=LinqExtensions.GetPropertyName((ENTABONE t)=>t.OLUSTURMATARIH),
            //        SiralamaTipi=EnumSiralamaTuru.Desc
            //    }
            //};

            var kayitlar = _entAboneService.Ara(entAboneAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.ENTABONEDetayList.Select(t => new
                {
                    t.KAYITNO,
                    t.AD,
                    t.SOYAD,
                    t.AboneNo,
                    t.KimlikNo,
                    t.Daire,
                    t.Blok,
                    t.Eposta,
                    t.SayacModel,
                    t.SayacSeriNo,
                    t.TarifeAdi,
                    t.Adres,
                    Islemler =
                    $"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "Abone", new { aboneKayitNo = t.KAYITNO })}' title='{Dil.Duzenle}'><i class='fa fa-edit'></i></a>" +
                    $"<a class='btn btn-xs btn-danger modalizer ' href='{Url.Action("Sil", "Abone", new { aboneKayitNo = t.KAYITNO })}' title='{Dil.Sil}'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Ekle(int? aboneKayitNo)
        {
            var model = new AboneIslemleri
            {
                ENTABONE = new ENTABONE()
            };

            return View("Kaydet", model);
        }
        public ActionResult Guncelle(int aboneKayitNo)
        {
            var model = new AboneIslemleri
            {
                ENTABONE = _entAboneService.GetirById(aboneKayitNo)
            };
            return View("Kaydet", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(AboneIslemleri aboneKaydetModel)
        {
            aboneKaydetModel.ENTABONE.KURUMKAYITNO = AktifKullanici.KurumKayitNo;


            aboneKaydetModel.ENTABONE.DURUM = 1;
            if (aboneKaydetModel.ENTABONE.KAYITNO > 0)
            {
                aboneKaydetModel.ENTABONE.GUNCELLEYEN = AktifKullanici.KayitNo;
                aboneKaydetModel.ENTABONESAYAC.GUNCELLEYEN = AktifKullanici.KayitNo;
                _entAboneService.Guncelle(aboneKaydetModel.ENTABONE.List());
            }
            else
            {
                aboneKaydetModel.ENTABONE.OLUSTURAN = AktifKullanici.KayitNo;
                aboneKaydetModel.ENTABONESAYAC.OLUSTURAN = AktifKullanici.KayitNo;
                aboneKaydetModel.ENTABONE = _entAboneService.Ekle(aboneKaydetModel.ENTABONE.List()).FirstOrDefault();
            }

            //return Yonlendir(Url.Action("Index", "Abone", new { sayacKayitNo = aboneKaydetModel.ENTABONE.KAYITNO }), $"{Dil.Basarili}");
            return Yonlendir(Url.Action("AboneTumBilgileri", "EntAbone", new { AboneKayitNo = aboneKaydetModel.ENTABONE.KAYITNO }), $"{Dil.Basarili}");
        }

        public ActionResult Sil(int aboneKayitNo)
        {
            SayfaBaslik("Abone Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel { Id = aboneKayitNo, RedirectUrlForCancel = $"/Sayac/Index" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {

            _entAboneService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"), $"{Dil.Basarili}");
        }


        public ActionResult Tarife(int sayacKayitNo)
        {
            try
            {

                var abonesayac = _entAboneSayacService.DetayGetir(new ENTABONESAYACAra { SAYACKAYITNO = sayacKayitNo, Durum = 1 }).FirstOrDefault();
                var model = new AboneIslemleri
                {

                    PRMTARIFEELK = _prmTarifeElkService.GetirById((int)abonesayac.TARIFEKAYITNO),
                    PrmTarifeKALORIMETRE = _prmTarifeKALORIMETREService.GetirById((int)abonesayac.TARIFEKAYITNO),
                    PRMTARIFESU = _prmTarifeSuService.GetirById((int)abonesayac.TARIFEKAYITNO),
                    PrmTarifeGaz = _prmTarifeGazService.GetirById((int)abonesayac.TARIFEKAYITNO),
                    EntSayacDetay = _entSayacService.DetayGetirById(abonesayac.SAYACKAYITNO),
                    KurumKayitNo = AktifKullanici.KurumKayitNo,
                    AboneNo = abonesayac.AboneNo.ToInt32()


                };

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new NotificationException(ex.Message);
            }

        }
        public ActionResult AjaxAra(string key, ENTABONEAra entAboneAra = null, int limit = 10, int baslangic = 0)
        {

            if (entAboneAra == null)
            {
                entAboneAra = new ENTABONEAra();
            }
            entAboneAra.KURUMKAYITNO = AktifKullanici.KurumKayitNo;
            entAboneAra.Ara = new Ara
            {
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {
                    new Siralama
                    {
                        KolonAdi = LinqExtensions.GetPropertyName((ENTABONE t) => t.KAYITNO),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }
                }
            };

            entAboneAra.AboneNoVeyAdiVeyaSoyadi = key;

            var aboneList = _entAboneService.DetayGetir(entAboneAra);


            var data = aboneList.Select(abone => new AutoCompleteData
            {
                //TODO: Bu bölümü düzenle
                id = abone.KAYITNO.ToString(),
                text = abone.AutoCompleteStr,
                description = abone.KAYITNO.ToString(),
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AjaxTekDeger(int id)
        {
            var abone = _entAboneService.AutoCompleteBilgileriGetir(new ENTABONEAra { KAYITNO = id }).FirstOrDefault();


            var data = new AutoCompleteData
            {//TODO: Bu bölümü düzenle
                id = abone.KayitNo.ToString(),
                text = abone.AutoCompleteStr,
                description = abone.KayitNo.ToString(),
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxCokDeger(string id)
        {
            var aboneList = _entAboneService.AutoCompleteBilgileriGetir(new ENTABONEAra() { KAYITNOlar = id });


            var data = aboneList.Select(abone => new AutoCompleteData
            {
                id = abone.KayitNo.ToString(),
                text = abone.AutoCompleteStr,
                description = abone.KayitNo.ToString()
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }


    }
}