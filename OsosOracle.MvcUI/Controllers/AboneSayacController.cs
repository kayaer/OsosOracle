using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.ENTABONESAYACComplexTypes;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.MvcUI.Filters;
using OsosOracle.MvcUI.Models.ENTABONESAYACModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OsosOracle.MvcUI.Resources;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Entities.ComplexType.PRMTARIFESUComplexTypes;
using OsosOracle.Entities.ComplexType.PRMTARIFEKALORIMETREComplexTypes;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.Enums;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class AboneSayacController : BaseController
    {
        private readonly IENTABONESAYACService _entAboneSayacService;
        private readonly IPRMTARIFESUService _prmTarifeSuService;
        private readonly IPRMTARIFEKALORIMETREService _prmKALORIMETREService;
        public AboneSayacController(IENTABONESAYACService entAboneSayacService, IPRMTARIFESUService prmTarifeSuService, IPRMTARIFEKALORIMETREService prmKALORIMETREService)
        {
            _entAboneSayacService = entAboneSayacService;
            _prmTarifeSuService = prmTarifeSuService;
            _prmKALORIMETREService = prmKALORIMETREService;
        }

        public ActionResult Index()
        {
            return View(new ENTABONESAYACIndexModel());
        }



        public ActionResult Ekle(int AboneKayitNo, int Tip, string returnUrl = "", string islemTipi = "Ekle")
        {
            var model = new ENTABONESAYACKaydetModel()
            {
                ReturnUrl = returnUrl,
                Tip = Tip,
                IslemTipi = islemTipi,
                ENTABONESAYAC = new Entities.Concrete.ENTABONESAYAC
                {
                    ABONEKAYITNO = AboneKayitNo,
                    TAKILMATARIH = DateTime.Now
                }
            };

            return View("Kaydet", model);
        }

        public ActionResult Guncelle(int AboneKayitNo, int SayacKayitNo, int Tip, string returnUrl = "", string islemTipi = "Guncelle")
        {


            var model = new ENTABONESAYACKaydetModel
            {
                ReturnUrl = returnUrl,
                Tip = Tip,
                IslemTipi = islemTipi,
                ENTABONESAYAC = _entAboneSayacService.Getir(new ENTABONESAYACAra { ABONEKAYITNO = AboneKayitNo, SAYACKAYITNO = SayacKayitNo, Durum = 1 }).FirstOrDefault()
            };


            return View("Kaydet", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(ENTABONESAYACKaydetModel aboneSayacKaydetModel)
        {


            aboneSayacKaydetModel.ENTABONESAYAC.OLUSTURAN = AktifKullanici.KayitNo;
            aboneSayacKaydetModel.ENTABONESAYAC.GUNCELLEYEN = AktifKullanici.KayitNo;
            if (aboneSayacKaydetModel.ENTABONESAYAC.KAYITNO > 0)
            {
                _entAboneSayacService.Guncelle(aboneSayacKaydetModel.ENTABONESAYAC.List());
                if (aboneSayacKaydetModel.IslemTipi == "Sil")
                {
                    _entAboneSayacService.Sil(aboneSayacKaydetModel.ENTABONESAYAC.KAYITNO.List());
                }
            }
            else
            {
                _entAboneSayacService.Ekle(aboneSayacKaydetModel.ENTABONESAYAC.List());
            }

            var returnUrl = string.IsNullOrEmpty(aboneSayacKaydetModel.ReturnUrl)
               ? Url.Action("Index", "Sayac")
               : aboneSayacKaydetModel.ReturnUrl;

            //return Yonlendir(returnUrl, "Abone-Sayaç kayıdı başarıyla gerçekleştirilmiştir.");
            return Yonlendir(returnUrl, Dil.Basarili);
        }
        public PartialViewResult SuPartial(PartialModel partialModel)
        {
            SayfaBaslik("Su Sayaç Bilgileri");
            if (partialModel.EntAboneSayacAra == null)
            {
                partialModel.EntAboneSayacAra = new ENTABONESAYACAra();
            }
            partialModel.ReturnUrl = partialModel.ReturnUrl;
            partialModel.EntAboneSayacAra.Durum = 1;
            return PartialView(partialModel);
        }

        public PartialViewResult KalorimetrePartial(PartialModel partialModel)
        {
            SayfaBaslik("Kalorimetre Sayaç Bilgileri");
            if (partialModel.EntAboneSayacAra == null)
            {
                partialModel.EntAboneSayacAra = new ENTABONESAYACAra();
            }
            partialModel.EntAboneSayacAra.Durum = 1;
            return PartialView(partialModel);
        }
        public PartialViewResult GazPartial(PartialModel partialModel)
        {
            SayfaBaslik("Gaz Sayaç Bilgileri");
            if (partialModel.EntAboneSayacAra == null)
            {
                partialModel.EntAboneSayacAra = new ENTABONESAYACAra();
            }
            partialModel.EntAboneSayacAra.Durum = 1;
            return PartialView(partialModel);
        }


        public PartialViewResult SokulenSayacPartial(PartialModel partialModel)
        {
            SayfaBaslik("Sökülen Sayaç Bilgileri");

            if (partialModel.EntAboneSayacAra == null)
            {
                partialModel.EntAboneSayacAra = new ENTABONESAYACAra();
            }
            partialModel.EntAboneSayacAra.Durum = 2;

            return PartialView(partialModel);
        }
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, PartialModel partialModel)
        {
            partialModel.EntAboneSayacAra.KurumKayitNo = AktifKullanici.KurumKayitNo;
            if (partialModel.EntAboneSayacAra == null)
            {
                partialModel.EntAboneSayacAra = new ENTABONESAYACAra();
            }

            partialModel.EntAboneSayacAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                partialModel.EntAboneSayacAra.AboneAdi = dtParameterModel.Search.Value;
            }

            var kayitlar = _entAboneSayacService.Ara(partialModel.EntAboneSayacAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.ENTABONESAYACDetayList.Select(t => new
                {
                    SONSATISTARIH = t.SONSATISTARIH?.ToShortDateString(),
                    t.AboneNo,
                    t.AboneAdSoyad,
                    t.SayacModel,
                    t.SayacSeriNo,
                    t.KapakSeriNo,
                    TAKILMATARIH = t.TAKILMATARIH?.ToShortDateString(),
                    SOKULMETARIH = t.SOKULMETARIH?.ToShortDateString(),
                    t.SOKULMENEDEN,
                    t.Aciklama,
                    Islemler = $@"<a class='btn btn-xs btn-info' href='{Url.Action("AboneTumBilgileri", "EntAbone", new { AboneKayitNo = t.ABONEKAYITNO, returnUrl = partialModel.ReturnUrl })}' title='Düzenle'><i class='fa fa-edit'></i></a>							 
								  "
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DataTablesListSu(DtParameterModel dtParameterModel, PartialModel partialModel)
        {
            if (partialModel.EntAboneSayacAra == null)
            {
                partialModel.EntAboneSayacAra = new ENTABONESAYACAra();
            }

            partialModel.EntAboneSayacAra.ABONEKAYITNO = partialModel.AboneKayitNo;
            partialModel.EntAboneSayacAra.SayacTur = 1;

            partialModel.EntAboneSayacAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                //acigaAlinmaAra.Adi = dtParameterModel.Search.Value;
            }

            var kayitlar = _entAboneSayacService.Ara(partialModel.EntAboneSayacAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.ENTABONESAYACDetayList.Select(t => new
                {
                    t.SayacModel,
                    t.TARIFEKAYITNO,
                    t.SuTarifeAdi,
                    t.SAYACKAYITNO,
                    t.SayacSeriNo,
                    t.KapakSeriNo,
                    TAKILMATARIH = t.TAKILMATARIH?.ToShortDateString(),
                    SOKULMETARIH = t.SOKULMETARIH?.ToShortDateString(),
                    t.SOKULMENEDEN,
                    t.Aciklama,
                    Islemler = $@"<a class='btn btn-xs btn-success' onclick=SuKartHazirla({t.SAYACKAYITNO}) title='{Dil.Karthazirla}'><i class='fa fa-list'></i></a>
                                  <a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "AboneSayac", new { AboneKayitNo = t.ABONEKAYITNO, SayacKayitNo = t.SAYACKAYITNO, Tip = 1, islemTipi = "Guncelle", returnUrl = partialModel.ReturnUrl })}' title='{Dil.Duzenle}'><i class='fa fa-edit'></i></a>							 
								  <a class='btn btn-xs btn-danger modalizer ' href='{Url.Action("Guncelle", "AboneSayac", new { AboneKayitNo = t.ABONEKAYITNO, SayacKayitNo = t.SAYACKAYITNO, Tip = 1, islemTipi = "Sil", returnUrl = partialModel.ReturnUrl })}' title='{Dil.Sil}'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DataTablesListKalorimetre(DtParameterModel dtParameterModel, PartialModel partialModel)
        {
            if (partialModel.EntAboneSayacAra == null)
            {
                partialModel.EntAboneSayacAra = new ENTABONESAYACAra();
            }

            partialModel.EntAboneSayacAra.ABONEKAYITNO = partialModel.AboneKayitNo;
            partialModel.EntAboneSayacAra.SayacTur = 2;

            partialModel.EntAboneSayacAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                //acigaAlinmaAra.Adi = dtParameterModel.Search.Value;
            }

            var kayitlar = _entAboneSayacService.Ara(partialModel.EntAboneSayacAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.ENTABONESAYACDetayList.Select(t => new
                {
                    t.SayacModel,
                    TAKILMATARIH = t.TAKILMATARIH?.ToShortDateString(),
                    SOKULMETARIH = t.SOKULMETARIH?.ToShortDateString(),
                    t.SOKULMENEDEN,
                    t.SayacSeriNo,
                    t.KapakSeriNo,
                    t.KalorimetreTarifeAdi,
                    t.Aciklama,
                    Islemler = $@"<a class='btn btn-xs btn-success' onclick=KalorimetreKartHazirla({ t.SAYACKAYITNO}) title='{Dil.Karthazirla}'><i class='fa fa-list'></i></a>
                                  <a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "AboneSayac", new { AboneKayitNo = t.ABONEKAYITNO, SayacKayitNo = t.SAYACKAYITNO, Tip = 2, islemTipi = "Guncelle", returnUrl = partialModel.ReturnUrl })}' title='{Dil.Duzenle}'><i class='fa fa-edit'></i></a>							 
								  <a class='btn btn-xs btn-danger modalizer ' href='{Url.Action("Guncelle", "AboneSayac", new { AboneKayitNo = t.ABONEKAYITNO, SayacKayitNo = t.SAYACKAYITNO, Tip = 2, islemTipi = "Sil", returnUrl = partialModel.ReturnUrl })}' title='{Dil.Sil}'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DataTablesListGaz(DtParameterModel dtParameterModel, PartialModel partialModel)
        {
            if (partialModel.EntAboneSayacAra == null)
            {
                partialModel.EntAboneSayacAra = new ENTABONESAYACAra();
            }

            partialModel.EntAboneSayacAra.ABONEKAYITNO = partialModel.AboneKayitNo;
            partialModel.EntAboneSayacAra.SayacTur = 4;

            partialModel.EntAboneSayacAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                //acigaAlinmaAra.Adi = dtParameterModel.Search.Value;
            }

            var kayitlar = _entAboneSayacService.Ara(partialModel.EntAboneSayacAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.ENTABONESAYACDetayList.Select(t => new
                {
                    t.SayacModel,
                    TAKILMATARIH = t.TAKILMATARIH?.ToShortDateString(),
                    SOKULMETARIH = t.SOKULMETARIH?.ToShortDateString(),
                    t.SOKULMENEDEN,
                    t.SayacSeriNo,
                    t.KapakSeriNo,
                    t.GazTarifeAdi,
                    t.Aciklama,
                    Islemler = $@"<a class='btn btn-xs btn-success' onclick=GazKartHazirla({ t.SAYACKAYITNO}) title='{Dil.Karthazirla}'><i class='fa fa-list'></i></a>
                                  <a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "AboneSayac", new { AboneKayitNo = t.ABONEKAYITNO, SayacKayitNo = t.SAYACKAYITNO, Tip = 2, islemTipi = "Guncelle", returnUrl = partialModel.ReturnUrl })}' title='{Dil.Duzenle}'><i class='fa fa-edit'></i></a>							 
								  <a class='btn btn-xs btn-danger modalizer ' href='{Url.Action("Guncelle", "AboneSayac", new { AboneKayitNo = t.ABONEKAYITNO, SayacKayitNo = t.SAYACKAYITNO, Tip = 2, islemTipi = "Sil", returnUrl = partialModel.ReturnUrl })}' title='{Dil.Sil}'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DataTablesListSokulen(DtParameterModel dtParameterModel, PartialModel partialModel)
        {
            if (partialModel.EntAboneSayacAra == null)
            {
                partialModel.EntAboneSayacAra = new ENTABONESAYACAra();
            }

            partialModel.EntAboneSayacAra.ABONEKAYITNO = partialModel.AboneKayitNo;


            partialModel.EntAboneSayacAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                //acigaAlinmaAra.Adi = dtParameterModel.Search.Value;
            }

            var kayitlar = _entAboneSayacService.Ara(partialModel.EntAboneSayacAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.ENTABONESAYACDetayList.Select(t => new
                {
                    t.SayacModel,
                    TAKILMATARIH = t.TAKILMATARIH?.ToShortDateString(),
                    SOKULMETARIH = t.SOKULMETARIH?.ToShortDateString(),
                    t.SOKULMENEDEN,
                    t.SayacSeriNo,
                    t.KapakSeriNo,
                    t.Aciklama,
                    Islemler = $@"<a class='btn btn-xs btn-info ' href='{Url.Action("Guncelle", "AboneSayac", new { id = t.KAYITNO })}' title='{Dil.Duzenle}'><i class='fa fa-edit'></i></a>							 
								  <a class='btn btn-xs btn-danger modalizer ' href='{Url.Action("Sil", "AboneSayac", new { id = t.KAYITNO })}' title='{Dil.Sil}'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SuTarifeGetir(int sayacKayitNo, int aboneKayitNo)
        {
            var aboneSayac = _entAboneSayacService.DetayGetir(new ENTABONESAYACAra { ABONEKAYITNO = aboneKayitNo, SAYACKAYITNO = sayacKayitNo, Durum = 1 }).FirstOrDefault();
            if (aboneSayac == null)
            {
                throw new NotificationException("Sayaç bulunamadı");
            }
            var tarife = _prmTarifeSuService.DetayGetir(new PRMTARIFESUAra { KAYITNO = aboneSayac.TARIFEKAYITNO }).FirstOrDefault();
            if (tarife == null)
            {
                throw new NotificationException("Tarife bilgileri çekilemedi");
            }
            return Json(tarife, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult KalorimetreTarifeGetir(int sayacKayitNo, int aboneKayitNo)
        {
            var aboneSayac = _entAboneSayacService.DetayGetir(new ENTABONESAYACAra { ABONEKAYITNO = aboneKayitNo, SAYACKAYITNO = sayacKayitNo, Durum = 1 }).FirstOrDefault();
            if (aboneSayac == null)
            {
                throw new NotificationException("Sayaç bulunamadı");
            }
            var tarife = _prmKALORIMETREService.DetayGetir(new PRMTARIFEKALORIMETREAra { KAYITNO = aboneSayac.TARIFEKAYITNO }).FirstOrDefault();
            if (tarife == null)
            {
                throw new NotificationException("Tarife bilgileri çekilemedi");
            }
            return Json(tarife, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AboneSayacList(DtParameterModel dtParameterModel, ENTABONESAYACAra entAboneSayacAra)
        {
            entAboneSayacAra.Ara = dtParameterModel.AramaKriteri;
            entAboneSayacAra.KurumKayitNo = AktifKullanici.KurumKayitNo;
            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            {
                entAboneSayacAra.AboneAdi = dtParameterModel.Search.Value;
            }
            entAboneSayacAra.Ara.Siralama = new List<Siralama>
            {
                new Siralama
                {
                    KolonAdi=LinqExtensions.GetPropertyName((ENTABONESAYAC t)=>t.SONSATISTARIH),
                    SiralamaTipi=EnumSiralamaTuru.Desc
                }
            };
            var kayitlar = _entAboneSayacService.Ara(entAboneSayacAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.ENTABONESAYACDetayList.Select(t => new
                {
                    SONSATISTARIH = t.SONSATISTARIH?.ToShortDateString(),
                    t.AboneNo,
                    t.AboneAdSoyad,
                    t.SayacModel,
                    t.SayacSeriNo,
                    t.KapakSeriNo,
                    TAKILMATARIH = t.TAKILMATARIH?.ToShortDateString(),
                    SOKULMETARIH = t.SOKULMETARIH?.ToShortDateString(),
                    t.SOKULMENEDEN,
                    t.Aciklama
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }
    }
}