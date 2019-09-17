using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.ENTABONEBILGIComplexTypes;
using OsosOracle.Entities.ComplexType.ENTABONEComplexTypes;
using OsosOracle.Entities.ComplexType.ENTABONESAYACComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.MvcUI.Filters;
using OsosOracle.MvcUI.Models.ENTABONEModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class AboneController : BaseController
    {
        private readonly IENTABONEService _entAboneService;
        private readonly IENTABONEBILGIService _entAboneBilgiService;
        private readonly IENTABONESAYACService _entAboneSayacService;
        private readonly IPRMTARIFEELKService _prmTarifeElkService;
        private readonly IENTSAYACService _entSayacService;

        public AboneController(IENTABONEService entAboneService, IENTABONEBILGIService entAboneBilgiService, IENTABONESAYACService entAboneSayacService, IPRMTARIFEELKService prmTarifeElkService, IENTSAYACService entSayacService)
        {
            _entAboneService = entAboneService;
            _entAboneBilgiService = entAboneBilgiService;
            _entAboneSayacService = entAboneSayacService;
            _prmTarifeElkService = prmTarifeElkService;
            _entSayacService = entSayacService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DataTablesList(DtParameterModel dtParameterModel, ENTABONEAra entAboneAra)
        {

            entAboneAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            {
                entAboneAra.AboneNoVeyAdiVeyaSoyadi = dtParameterModel.Search.Value;
            }

            var kayitlar = _entAboneService.Ara(entAboneAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.ENTABONEDetayList.Select(t => new
                {
                    t.KAYITNO,
                    t.AD,
                    t.SOYAD,
                    t.AboneNo,
                    t.Eposta,
                    t.SayacModel,
                    t.SayacSeriNo,
                    t.TarifeAdi,
                    Islemler = $@"<a class='btn btn-xs btn-info' href='{Url.Action("Guncelle", "Abone", new { aboneKayitNo = t.KAYITNO })}' title='Düzenle'><i class='fa fa-edit'></i></a>		
                                  <a class='btn btn-xs btn-info' onclick=KartHazirla({t.KAYITNO}) title='Kart Hazırla'><i class='fa fa-list'></i></a>
                                  <a class='btn btn-xs btn-danger modalizer ' href='{Url.Action("Sil", "Abone", new { aboneKayitNo = t.KAYITNO })}' title='Sil'><i class='fa fa-trash'></i></a>"
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
                ENTABONE = _entAboneService.GetirById(aboneKayitNo),
                ENTABONEBILGI = _entAboneBilgiService.Getir(new ENTABONEBILGIAra { ABONEKAYITNO=aboneKayitNo}).FirstOrDefault(),
                ENTABONESAYAC=_entAboneSayacService.Getir(new ENTABONESAYACAra { ABONEKAYITNO=aboneKayitNo}).FirstOrDefault()

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
                aboneKaydetModel.ENTABONEBILGI.GUNCELLEYEN = AktifKullanici.KayitNo;
                aboneKaydetModel.ENTABONESAYAC.GUNCELLEYEN = AktifKullanici.KayitNo;
                _entAboneService.AboneGuncelle(aboneKaydetModel);
            }
            else
            {
                aboneKaydetModel.ENTABONE.OLUSTURAN = AktifKullanici.KayitNo;
                aboneKaydetModel.ENTABONEBILGI.OLUSTURAN = AktifKullanici.KayitNo;
                aboneKaydetModel.ENTABONESAYAC.OLUSTURAN = AktifKullanici.KayitNo;
                _entAboneService.AboneEkle(aboneKaydetModel);
            }

            return Yonlendir(Url.Action("Index", "Abone", new { sayacKayitNo = aboneKaydetModel.ENTABONE.KAYITNO }), $"Abone kayıdı başarıyla gerçekleştirilmiştir.");
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

            return Yonlendir(Url.Action("Index"), "Abone Başarıyla Silindi");
        }

        public ActionResult Tarife(int aboneKayitno)
        {
            var abone = _entAboneService.DetayGetirById(aboneKayitno);
            var abonesayac = _entAboneSayacService.Getir(new ENTABONESAYACAra { ABONEKAYITNO = aboneKayitno }).FirstOrDefault();
            var model = new AboneIslemleri
            {
                ENTABONE = _entAboneService.GetirById(aboneKayitno),
                PRMTARIFEELK=_prmTarifeElkService.GetirById(abone.TarifeKayitNo) ,
                ENTSAYAC=_entSayacService.GetirById(abonesayac.SAYACKAYITNO)
                //ENTABONESAYAC=_entAboneSayacService.Getir(new ENTABONESAYACAra { ABONEKAYITNO=aboneKayitno}).FirstOrDefault(),
                
            };

            return Json(model,JsonRequestBehavior.AllowGet);
        }
        public ActionResult AjaxAra(string key, ENTABONEAra entAboneAra = null, int limit = 10, int baslangic = 0)
        {

            if (entAboneAra == null)
            {
                entAboneAra = new ENTABONEAra();
            }
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
            var abone = _entAboneService.AutoCompleteBilgileriGetir(new ENTABONEAra { KAYITNO=id}).FirstOrDefault();


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