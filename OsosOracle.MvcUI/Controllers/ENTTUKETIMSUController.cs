using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.ENTTUKETIMSUComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.ENTTUKETIMSUModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.MvcUI.Filters;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class ENTTUKETIMSUController : BaseController
    {
        private readonly IENTTUKETIMSUService _eNTTUKETIMSUService;
        private readonly IENTSAYACService _entSayacService;

        public ENTTUKETIMSUController(IENTTUKETIMSUService eNTTUKETIMSUService, IENTSAYACService entSayacService)
        {
            _eNTTUKETIMSUService = eNTTUKETIMSUService;
            _entSayacService = entSayacService;
        }


        public ActionResult Index(int? sayacKayitNo)
        {
            SayfaBaslik($"Sayaç Veri İşlemleri");
            var sayac = _entSayacService.GetirById(sayacKayitNo.Value);
            var model = new ENTTUKETIMSUIndexModel()
            {
                ENTTUKETIMSUAra = new ENTTUKETIMSUAra { SAYACID = "ELM"+sayac.SERINO,SayacKayitNo=sayacKayitNo }
            };

            return View(model);
        }

        public ActionResult TuketimGrafik(int? sayacKayitNo)
        {
            var model = new ENTTUKETIMSUIndexModel
            {
                ENTTUKETIMSUAra = new ENTTUKETIMSUAra()
                {
                    SayacKayitNo = sayacKayitNo.Value
                }
            };

            return View(model);
        }

        public ActionResult TuketimGetir(int sayacKayitNo)
        {
            var sayac = _entSayacService.GetirById(sayacKayitNo);

            var kayitlar = _eNTTUKETIMSUService.Ara(new ENTTUKETIMSUAra { SAYACID = "ELM"+sayac.SERINO});
            var model = kayitlar.ENTTUKETIMSUDetayList.Select(t => new { OKUMATARIH = t.OKUMATARIH?.ToString(), t.TUKETIM ,t.KALANKREDI}).OrderBy(t=>t.OKUMATARIH).Take(250);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, ENTTUKETIMSUAra eNTTUKETIMSUAra)
        {

            eNTTUKETIMSUAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                eNTTUKETIMSUAra.SAYACID = dtParameterModel.Search.Value;
            }



            var kayitlar = _eNTTUKETIMSUService.Ara(eNTTUKETIMSUAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.ENTTUKETIMSUDetayList.Select(t => new
                {
                    //TODO: Bu bölümü düzenle
                    t.KAYITNO,
                    t.SAYACID,
                    t.TUKETIM,
                    t.HARCANANKREDI,
                    t.KALANKREDI,
                    OKUMATARIH = t.OKUMATARIH?.ToString(),
                    SAYACTARIH = t.SAYACTARIH?.ToString(),
                    t.KrediDurumu,
                    Islemler = $@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "ENTTUKETIMSU", new { id = t.KAYITNO })}' title='Düzenle'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "ENTTUKETIMSU", new { id = t.KAYITNO })}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "ENTTUKETIMSU", new { id = t.KAYITNO })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Ekle()
        {
            SayfaBaslik($"ENTTUKETIMSU Ekle");

            var model = new ENTTUKETIMSUKaydetModel
            {
                ENTTUKETIMSU = new ENTTUKETIMSU()
            };



            return View("Kaydet", model);
        }


        public ActionResult Guncelle(int id)
        {
            SayfaBaslik($"ENTTUKETIMSU Güncelle");

            var model = new ENTTUKETIMSUKaydetModel
            {
                ENTTUKETIMSU = _eNTTUKETIMSUService.GetirById(id)
            };


            return View("Kaydet", model);
        }



        public ActionResult Detay(int id)
        {
            SayfaBaslik($"ENTTUKETIMSU Detay");

            var model = new ENTTUKETIMSUDetayModel
            {
                ENTTUKETIMSUDetay = _eNTTUKETIMSUService.DetayGetirById(id)
            };


            return View("Detay", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(ENTTUKETIMSUKaydetModel eNTTUKETIMSUKaydetModel)
        {
            if (eNTTUKETIMSUKaydetModel.ENTTUKETIMSU.KAYITNO > 0)
            {
                _eNTTUKETIMSUService.Guncelle(eNTTUKETIMSUKaydetModel.ENTTUKETIMSU.List());
            }
            else
            {
                _eNTTUKETIMSUService.Ekle(eNTTUKETIMSUKaydetModel.ENTTUKETIMSU.List());
            }

            return Yonlendir(Url.Action("Index"), $"ENTTUKETIMSU kayıdı başarıyla gerçekleştirilmiştir.");
            //return Yonlendir(Url.Action("Detay","ENTTUKETIMSU",new{id=eNTTUKETIMSUKaydetModel.ENTTUKETIMSU.Id}), $"ENTTUKETIMSU kayıdı başarıyla gerçekleştirilmiştir.");
        }



        public ActionResult Sil(int id)
        {
            SayfaBaslik($"ENTTUKETIMSU Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel = $"/ENTTUKETIMSU/Index/{id}" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
            _eNTTUKETIMSUService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"), $"ENTTUKETIMSU Başarıyla silindi");
        }


        public ActionResult AjaxAra(string key, ENTTUKETIMSUAra eNTTUKETIMSUAra = null, int limit = 10, int baslangic = 0)
        {

            if (eNTTUKETIMSUAra == null)
            {
                eNTTUKETIMSUAra = new ENTTUKETIMSUAra();
            }

            eNTTUKETIMSUAra.Ara = new Ara
            {
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {
                    new Siralama
                    {
                        KolonAdi = LinqExtensions.GetPropertyName((ENTTUKETIMSU t) => t.KAYITNO),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }
                }
            };


            //TODO: Bu bölümü düzenle
            eNTTUKETIMSUAra.SAYACID = key;

            var eNTTUKETIMSUList = _eNTTUKETIMSUService.Getir(eNTTUKETIMSUAra);


            var data = eNTTUKETIMSUList.Select(eNTTUKETIMSU => new AutoCompleteData
            {
                //TODO: Bu bölümü düzenle
                id = eNTTUKETIMSU.KAYITNO.ToString(),
                text = eNTTUKETIMSU.SAYACID.ToString(),
                description = eNTTUKETIMSU.KAYITNO.ToString(),
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AjaxTekDeger(int id)
        {
            var eNTTUKETIMSU = _eNTTUKETIMSUService.GetirById(id);


            var data = new AutoCompleteData
            {//TODO: Bu bölümü düzenle
                id = eNTTUKETIMSU.KAYITNO.ToString(),
                text = eNTTUKETIMSU.SAYACID.ToString(),
                description = eNTTUKETIMSU.KAYITNO.ToString(),
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxCokDeger(string id)
        {
            var eNTTUKETIMSUList = _eNTTUKETIMSUService.Getir(new ENTTUKETIMSUAra() { KAYITNOlar = id });


            var data = eNTTUKETIMSUList.Select(eNTTUKETIMSU => new AutoCompleteData
            { //TODO: Bu bölümü düzenle
                id = eNTTUKETIMSU.KAYITNO.ToString(),
                text = eNTTUKETIMSU.SAYACID.ToString(),
                description = eNTTUKETIMSU.KAYITNO.ToString()
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}

