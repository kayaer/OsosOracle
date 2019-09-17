using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.ENTSAYACComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.Framework.Entities;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.MvcUI.Filters;
using OsosOracle.MvcUI.Models.ENTSAYACModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class SayacController : BaseController
    {
        private readonly IENTSAYACService _entSayacService;
        public SayacController(IENTSAYACService entSayacService)
        {
            _entSayacService = entSayacService;
        }

        public ActionResult Index()
        {

            var model = new ENTSAYACIndexModel
            {
                ENTSAYACAra = new ENTSAYACAra()
            };
            return View(model);
        }

        public ActionResult DataTablesList(DtParameterModel dtParameterModel, ENTSAYACAra entSayacAra)
        {

            entSayacAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            {
                entSayacAra.SayacSeriNoIceren= dtParameterModel.Search.Value;

            }

            var kayitlar = _entSayacService.Ara(entSayacAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.ENTSAYACDetayList.Select(t => new
                {
                    t.Kurum,
                    t.SayacTipi,
                    t.SERINO,
                    SuTarifesi=t.PrmTarifeSuDetay.AD,
                    ElektrikTarifesi=t.PrmTarifeElkDetay.AD,
                    t.ACIKLAMA,
                    Islemler = $@"
                                <a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "AboneSayac", new { sayacKayitNo = t.KAYITNO })}' title='Abone-Tarife İşlemleri'><i class='fa fa-edit'></i></a>
                                <a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "Sayac", new { sayacKayitNo = t.KAYITNO })}' title='Düzenle'><i class='fa fa-edit'></i></a>		
                                <a class='btn btn-xs btn-info modalizer' href='{Url.Action("TuketimGrafik", "ENTTUKETIMSU", new { sayacKayitNo = t.KAYITNO })}' title='Tüketim Grafiği'><i class='fa fa-line-chart'></i></a>
                                <a class='btn btn-xs btn-info' target='_blank' href='{Url.Action("Index", "ENTTUKETIMSU", new { sayacKayitNo = t.KAYITNO })}' title='Tüketim Verileri'><i class='fa fa-database'></i></a>
								<a class='btn btn-xs btn-danger modalizer ' href='{Url.Action("Sil", "Sayac", new { sayacKayitNo = t.KAYITNO })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Ekle(int? sayacKayitNo)
        {
            var model = new ENTSAYACKaydetModel
            {
                ENTSAYAC = new ENTSAYAC()
                //{
                //    KAYITNO = sayacKayitNo.Value
                //}
            };

            return View("Kaydet", model);
        }


        public ActionResult Guncelle(int sayacKayitNo)
        {
            var model = new ENTSAYACKaydetModel
            {
                ENTSAYAC = _entSayacService.GetirById(sayacKayitNo)
            };


            return View("Kaydet", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(ENTSAYACKaydetModel sayacKaydetModel)
        {
            sayacKaydetModel.ENTSAYAC.KURUMKAYITNO = AktifKullanici.KurumKayitNo;
            
           
            sayacKaydetModel.ENTSAYAC.DURUM = 1;
            if (sayacKaydetModel.ENTSAYAC.KAYITNO > 0)
            {
                sayacKaydetModel.ENTSAYAC.GUNCELLEYEN = AktifKullanici.KayitNo;
                _entSayacService.Guncelle(sayacKaydetModel.ENTSAYAC.List());
            }
            else
            {
                sayacKaydetModel.ENTSAYAC.OLUSTURAN = AktifKullanici.KayitNo;
                _entSayacService.Ekle(sayacKaydetModel.ENTSAYAC.List());
            }

            return Yonlendir(Url.Action("Index", "Sayac", new { sayacKayitNo = sayacKaydetModel.ENTSAYAC.KAYITNO }), $"Sayaç kayıdı başarıyla gerçekleştirilmiştir.");
        }
      
        public ActionResult Sil(int sayacKayitNo)
        {
            SayfaBaslik("Sayaç Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel { Id = sayacKayitNo, RedirectUrlForCancel = $"/Sayac/Index" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
          
            _entSayacService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"), "Sayaç Başarıyla Silindi");
        }

        public ActionResult AjaxAra(string key, ENTSAYACAra entSayacAra = null, int limit = 10, int baslangic = 0)
        {

            if (entSayacAra == null)
            {
                entSayacAra = new ENTSAYACAra();
            }
            entSayacAra.Ara = new Ara
            {
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {
                    new Siralama
                    {
                        KolonAdi = LinqExtensions.GetPropertyName((ENTSAYAC t) => t.KAYITNO),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }
                }
            };
            try
            {
                entSayacAra.SERINO = Convert.ToInt32(key);
            }
            catch (Exception e)
            {

            }


            var sayacList = _entSayacService.Getir(entSayacAra);


            var data = sayacList.Select(sayac => new AutoCompleteData
            {
                //TODO: Bu bölümü düzenle
                id = sayac.KAYITNO.ToString(),
                text = sayac.SERINO.ToString(),
                description = sayac.KAYITNO.ToString(),
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AjaxTekDeger(int id)
        {
            var gorevdenUzaklastirma = _entSayacService.GetirById(id);


            var data = new AutoCompleteData
            {//TODO: Bu bölümü düzenle
                id = gorevdenUzaklastirma.KAYITNO.ToString(),
                text = gorevdenUzaklastirma.SERINO.ToString(),
                description = gorevdenUzaklastirma.KAYITNO.ToString(),
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxCokDeger(string id)
        {
            var gorevdenUzaklastirmaList = _entSayacService.Getir(new ENTSAYACAra() { KAYITNOlar = id });


            var data = gorevdenUzaklastirmaList.Select(gorevdenUzaklastirma => new AutoCompleteData
            { //TODO: Bu bölümü düzenle
                id = gorevdenUzaklastirma.KAYITNO.ToString(),
                text = gorevdenUzaklastirma.SERINO.ToString(),
                description = gorevdenUzaklastirma.KAYITNO.ToString()
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}