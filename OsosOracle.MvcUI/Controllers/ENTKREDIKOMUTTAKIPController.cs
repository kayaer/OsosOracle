using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.ENTKREDIKOMUTTAKIPComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.ENTKREDIKOMUTTAKIPModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.MvcUI.Filters;
using System;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class ENTKREDIKOMUTTAKIPController : BaseController
    {
        private readonly IENTKREDIKOMUTTAKIPService _eNTKREDIKOMUTTAKIPService;

        public ENTKREDIKOMUTTAKIPController(IENTKREDIKOMUTTAKIPService eNTKREDIKOMUTTAKIPService)
        {
            _eNTKREDIKOMUTTAKIPService = eNTKREDIKOMUTTAKIPService;
        }


        public ActionResult Index()
        {
            SayfaBaslik($"Kredi Komut İşlemleri");
            var model = new ENTKREDIKOMUTTAKIPIndexModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, ENTKREDIKOMUTTAKIPAra eNTKREDIKOMUTTAKIPAra)
        {

            eNTKREDIKOMUTTAKIPAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
              //eNTKREDIKOMUTTAKIPAra.Adi = dtParameterModel.Search.Value;
            }
            eNTKREDIKOMUTTAKIPAra.Ara = dtParameterModel.AramaKriteri;
            eNTKREDIKOMUTTAKIPAra.Ara.Siralama = new System.Collections.Generic.List<Framework.DataAccess.Filter.Siralama> { new Framework.DataAccess.Filter.Siralama { KolonAdi=LinqExtensions.GetPropertyName((ENTKREDIKOMUTTAKIP t) => t.ISLEMTARIH),
            SiralamaTipi=Framework.Enums.EnumSiralamaTuru.Desc} };


            var kayitlar = _eNTKREDIKOMUTTAKIPService.Ara(eNTKREDIKOMUTTAKIPAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.ENTKREDIKOMUTTAKIPDetayList.Select(t => new
                {
                    t.KONSSERINO,
                    t.SAYACSERINO,
                    t.ABONENO,
                    t.SATISKAYITNO,
                    t.KREDI,
                    ISLEMTARIH = t.ISLEMTARIH.ToString(),
                    t.ACIKLAMA,
                    t.DURUM,
                    Islemler = $@"<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "ENTKREDIKOMUTTAKIP", new { id = t.GUIDID })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Ekle()
        {
            SayfaBaslik($"Kredi Komut Ekle");

            var model = new ENTKREDIKOMUTTAKIPKaydetModel
            {
                ENTKREDIKOMUTTAKIP = new ENTKREDIKOMUTTAKIP()
            };



            return View("Kaydet", model);
        }


        public ActionResult Guncelle(int id)
        {
            SayfaBaslik($"Kredi Komut Güncelle");

            var model = new ENTKREDIKOMUTTAKIPKaydetModel
            {
                ENTKREDIKOMUTTAKIP = _eNTKREDIKOMUTTAKIPService.GetirById(id)
            };


            return View("Kaydet", model);
        }



        public ActionResult Detay(int id)
        {
            SayfaBaslik($"Kredi Komut Detay");

            var model = new ENTKREDIKOMUTTAKIPDetayModel
            {
                ENTKREDIKOMUTTAKIPDetay = _eNTKREDIKOMUTTAKIPService.DetayGetirById(id)
            };


            return View("Detay", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(ENTKREDIKOMUTTAKIPKaydetModel eNTKREDIKOMUTTAKIPKaydetModel)
        {
            if (eNTKREDIKOMUTTAKIPKaydetModel.ENTKREDIKOMUTTAKIP.GUIDID == null)
            {
                _eNTKREDIKOMUTTAKIPService.Guncelle(eNTKREDIKOMUTTAKIPKaydetModel.ENTKREDIKOMUTTAKIP.List());
            }
            else
            {
                _eNTKREDIKOMUTTAKIPService.Ekle(eNTKREDIKOMUTTAKIPKaydetModel.ENTKREDIKOMUTTAKIP.List());
            }

            return Yonlendir(Url.Action("Index"), $"Kredi Komut kayıdı başarıyla gerçekleştirilmiştir.");
            //return Yonlendir(Url.Action("Detay","ENTKREDIKOMUTTAKIP",new{id=eNTKREDIKOMUTTAKIPKaydetModel.ENTKREDIKOMUTTAKIP.Id}), $"ENTKREDIKOMUTTAKIP kayıdı başarıyla gerçekleştirilmiştir.");
        }



        public ActionResult Sil(Guid id)
        {
            SayfaBaslik($"Kredi Komut Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel() { GuidId = id, RedirectUrlForCancel = $"/ENTKREDIKOMUTTAKIP/Index" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
            _eNTKREDIKOMUTTAKIPService.Sil(model.GuidId.List());

            return Yonlendir(Url.Action("Index"), $"Kredi Komut Başarıyla silindi");
        }




    }
}

