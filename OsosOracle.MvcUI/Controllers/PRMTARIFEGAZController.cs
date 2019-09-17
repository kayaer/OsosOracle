using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.PRMTARIFEGAZComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.PRMTARIFEGAZModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.MvcUI.Filters;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class PRMTARIFEGAZController : BaseController
    {
        private readonly IPRMTARIFEGAZService _pRMTARIFEGAZService;

        public PRMTARIFEGAZController(IPRMTARIFEGAZService pRMTARIFEGAZService)
        {
            _pRMTARIFEGAZService = pRMTARIFEGAZService;
        }


        public ActionResult Index()
        {
            SayfaBaslik($"PRMTARIFEGAZ İşlemleri");
            var model = new PRMTARIFEGAZIndexModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, PRMTARIFEGAZAra pRMTARIFEGAZAra)
        {

            pRMTARIFEGAZAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                pRMTARIFEGAZAra.AD = dtParameterModel.Search.Value;
            }



            var kayitlar = _pRMTARIFEGAZService.Ara(pRMTARIFEGAZAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.PRMTARIFEGAZDetayList.Select(t => new
                {
                    t.Kurum,
                    t.KAYITNO,
                    t.TUKETIMLIMIT,
                    t.SABAHSAAT,
                    t.AKSAMSAAT,
                    t.PULSE,
                    t.BAYRAM1GUN,
                    t.BAYRAM1AY,
                    t.BAYRAM1SURE,
                    t.BAYRAM2GUN,
                    t.BAYRAM2SURE,
                    t.BAYRAM2AY,
                    t.KRITIKKREDI,
                    t.SAYACTUR,
                    t.SAYACCAP,
                    t.AD,
                    t.FIYAT1,
                    t.YEDEKKREDI,
                    t.KURUMKAYITNO,
                    t.BIRIMFIYAT,

                    Islemler = $@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "PRMTARIFEGAZ", new { id = t.KAYITNO })}' title='Düzenle'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "PRMTARIFEGAZ", new { id = t.KAYITNO })}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "PRMTARIFEGAZ", new { id = t.KAYITNO })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Ekle()
        {
            SayfaBaslik($"PRMTARIFEGAZ Ekle");

            var model = new PRMTARIFEGAZKaydetModel
            {
                PRMTARIFEGAZ = new PRMTARIFEGAZ()
            };



            return View("Kaydet", model);
        }


        public ActionResult Guncelle(int id)
        {
            SayfaBaslik($"PRMTARIFEGAZ Güncelle");

            var model = new PRMTARIFEGAZKaydetModel
            {
                PRMTARIFEGAZ = _pRMTARIFEGAZService.GetirById(id)
            };


            return View("Kaydet", model);
        }



        public ActionResult Detay(int id)
        {
            SayfaBaslik($"PRMTARIFEGAZ Detay");

            var model = new PRMTARIFEGAZDetayModel
            {
                PRMTARIFEGAZDetay = _pRMTARIFEGAZService.DetayGetirById(id)
            };


            return View("Detay", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(PRMTARIFEGAZKaydetModel pRMTARIFEGAZKaydetModel)
        {
            if (pRMTARIFEGAZKaydetModel.PRMTARIFEGAZ.KAYITNO > 0)
            {
                _pRMTARIFEGAZService.Guncelle(pRMTARIFEGAZKaydetModel.PRMTARIFEGAZ.List());
            }
            else
            {
                _pRMTARIFEGAZService.Ekle(pRMTARIFEGAZKaydetModel.PRMTARIFEGAZ.List());
            }

            return Yonlendir(Url.Action("Index"), $"PRMTARIFEGAZ kayıdı başarıyla gerçekleştirilmiştir.");
            //return Yonlendir(Url.Action("Detay","PRMTARIFEGAZ",new{id=pRMTARIFEGAZKaydetModel.PRMTARIFEGAZ.Id}), $"PRMTARIFEGAZ kayıdı başarıyla gerçekleştirilmiştir.");
        }



        public ActionResult Sil(int id)
        {
            SayfaBaslik($"PRMTARIFEGAZ Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel = $"/PRMTARIFEGAZ/Index/{id}" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
            _pRMTARIFEGAZService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"), $"PRMTARIFEGAZ Başarıyla silindi");
        }


        public ActionResult AjaxAra(string key, PRMTARIFEGAZAra pRMTARIFEGAZAra = null, int limit = 10, int baslangic = 0)
        {

            if (pRMTARIFEGAZAra == null)
            {
                pRMTARIFEGAZAra = new PRMTARIFEGAZAra();
            }

            pRMTARIFEGAZAra.Ara = new Ara
            {
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {
                    new Siralama
                    {
                        KolonAdi = LinqExtensions.GetPropertyName((PRMTARIFEGAZ t) => t.KAYITNO),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }
                }
            };


            //TODO: Bu bölümü düzenle
            pRMTARIFEGAZAra.AD = key;

            var pRMTARIFEGAZList = _pRMTARIFEGAZService.Getir(pRMTARIFEGAZAra);


            var data = pRMTARIFEGAZList.Select(pRMTARIFEGAZ => new AutoCompleteData
            {
                //TODO: Bu bölümü düzenle
                id = pRMTARIFEGAZ.KAYITNO.ToString(),
                text = pRMTARIFEGAZ.AD.ToString(),
                description = pRMTARIFEGAZ.AD.ToString(),
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AjaxTekDeger(int id)
        {
            var pRMTARIFEGAZ = _pRMTARIFEGAZService.GetirById(id);


            var data = new AutoCompleteData
            {//TODO: Bu bölümü düzenle
                id = pRMTARIFEGAZ.KAYITNO.ToString(),
                text = pRMTARIFEGAZ.AD.ToString(),
                description = pRMTARIFEGAZ.AD.ToString(),
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxCokDeger(string id)
        {
            var pRMTARIFEGAZList = _pRMTARIFEGAZService.Getir(new PRMTARIFEGAZAra() { KAYITNOlar = id });


            var data = pRMTARIFEGAZList.Select(pRMTARIFEGAZ => new AutoCompleteData
            { //TODO: Bu bölümü düzenle
                id = pRMTARIFEGAZ.KAYITNO.ToString(),
                text = pRMTARIFEGAZ.AD.ToString(),
                description = pRMTARIFEGAZ.AD.ToString()
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}

