using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.ENTSAYACDURUMSUComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.ENTSAYACDURUMSUModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.MvcUI.Filters;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class ENTSAYACDURUMSUController : BaseController
    {
        private readonly IENTSAYACDURUMSUService _eNTSAYACDURUMSUService;

        public ENTSAYACDURUMSUController(IENTSAYACDURUMSUService eNTSAYACDURUMSUService)
        {
            _eNTSAYACDURUMSUService = eNTSAYACDURUMSUService;
        }


        public ActionResult Index()
        {
            SayfaBaslik($"ENTSAYACDURUMSU İşlemleri");
            var model = new ENTSAYACDURUMSUIndexModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, ENTSAYACDURUMSUAra eNTSAYACDURUMSUAra)
        {

            eNTSAYACDURUMSUAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                eNTSAYACDURUMSUAra.SAYACID = dtParameterModel.Search.Value;
            }



            var kayitlar = _eNTSAYACDURUMSUService.Ara(eNTSAYACDURUMSUAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.ENTSAYACDURUMSUDetayList.Select(t => new
                {
                    //TODO: Bu bölümü düzenle
                    t.KAYITNO,
                    t.SAYACID,
                   
                    Islemler = $@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "ENTSAYACDURUMSU", new { id = t.KAYITNO })}' title='Düzenle'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "ENTSAYACDURUMSU", new { id = t.KAYITNO })}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "ENTSAYACDURUMSU", new { id = t.KAYITNO })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Ekle()
        {
            SayfaBaslik($"ENTSAYACDURUMSU Ekle");

            var model = new ENTSAYACDURUMSUKaydetModel
            {
                ENTSAYACDURUMSU = new ENTSAYACDURUMSU()
            };



            return View("Kaydet", model);
        }

        public ActionResult Guncelle(int id)
        {
            SayfaBaslik($"ENTSAYACDURUMSU Güncelle");

            var model = new ENTSAYACDURUMSUKaydetModel
            {
                ENTSAYACDURUMSU = _eNTSAYACDURUMSUService.GetirById(id)
            };


            return View("Kaydet", model);
        }



        public ActionResult Detay(int id)
        {
            SayfaBaslik($"ENTSAYACDURUMSU Detay");

            var model = new ENTSAYACDURUMSUDetayModel
            {
                ENTSAYACDURUMSUDetay = _eNTSAYACDURUMSUService.DetayGetirById(id)
            };


            return View("Detay", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(ENTSAYACDURUMSUKaydetModel eNTSAYACDURUMSUKaydetModel)
        {
            if (eNTSAYACDURUMSUKaydetModel.ENTSAYACDURUMSU.KAYITNO > 0)
            {
                _eNTSAYACDURUMSUService.Guncelle(eNTSAYACDURUMSUKaydetModel.ENTSAYACDURUMSU.List());
            }
            else
            {
                _eNTSAYACDURUMSUService.Ekle(eNTSAYACDURUMSUKaydetModel.ENTSAYACDURUMSU.List());
            }

            return Yonlendir(Url.Action("Index"), $"ENTSAYACDURUMSU kayıdı başarıyla gerçekleştirilmiştir.");
            //return Yonlendir(Url.Action("Detay","ENTSAYACDURUMSU",new{id=eNTSAYACDURUMSUKaydetModel.ENTSAYACDURUMSU.Id}), $"ENTSAYACDURUMSU kayıdı başarıyla gerçekleştirilmiştir.");
        }



        public ActionResult Sil(int id)
        {
            SayfaBaslik($"ENTSAYACDURUMSU Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel = $"/ENTSAYACDURUMSU/Index/{id}" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
            _eNTSAYACDURUMSUService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"), $"ENTSAYACDURUMSU Başarıyla silindi");
        }


        public ActionResult AjaxAra(string key, ENTSAYACDURUMSUAra eNTSAYACDURUMSUAra = null, int limit = 10, int baslangic = 0)
        {

            if (eNTSAYACDURUMSUAra == null)
            {
                eNTSAYACDURUMSUAra = new ENTSAYACDURUMSUAra();
            }

            eNTSAYACDURUMSUAra.Ara = new Ara
            {
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {
                    new Siralama
                    {
                        KolonAdi = LinqExtensions.GetPropertyName((ENTSAYACDURUMSU t) => t.KAYITNO),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }
                }
            };


            //TODO: Bu bölümü düzenle
            eNTSAYACDURUMSUAra.SAYACID = key;

            var eNTSAYACDURUMSUList = _eNTSAYACDURUMSUService.Getir(eNTSAYACDURUMSUAra);


            var data = eNTSAYACDURUMSUList.Select(eNTSAYACDURUMSU => new AutoCompleteData
            {
                //TODO: Bu bölümü düzenle
                id = eNTSAYACDURUMSU.KAYITNO.ToString(),
                text = eNTSAYACDURUMSU.SayacId.ToString(),
                description = eNTSAYACDURUMSU.KAYITNO.ToString(),
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AjaxTekDeger(int id)
        {
            var eNTSAYACDURUMSU = _eNTSAYACDURUMSUService.GetirById(id);


            var data = new AutoCompleteData
            {//TODO: Bu bölümü düzenle
                id = eNTSAYACDURUMSU.KAYITNO.ToString(),
                text = eNTSAYACDURUMSU.SayacId.ToString(),
                description = eNTSAYACDURUMSU.KAYITNO.ToString(),
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxCokDeger(string id)
        {
            var eNTSAYACDURUMSUList = _eNTSAYACDURUMSUService.Getir(new ENTSAYACDURUMSUAra() { KAYITNOlar = id });


            var data = eNTSAYACDURUMSUList.Select(eNTSAYACDURUMSU => new AutoCompleteData
            { //TODO: Bu bölümü düzenle
                id = eNTSAYACDURUMSU.KAYITNO.ToString(),
                text = eNTSAYACDURUMSU.SayacId.ToString(),
                description = eNTSAYACDURUMSU.KAYITNO.ToString()
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}

