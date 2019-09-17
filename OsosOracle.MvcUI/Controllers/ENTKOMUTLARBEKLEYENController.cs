using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.ENTKOMUTLARBEKLEYENComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.ENTKOMUTLARBEKLEYENModels;
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
    public class ENTKOMUTLARBEKLEYENController : BaseController
    {
        private readonly IENTKOMUTLARBEKLEYENService _eNTKOMUTLARBEKLEYENService;

        public ENTKOMUTLARBEKLEYENController(IENTKOMUTLARBEKLEYENService eNTKOMUTLARBEKLEYENService)
        {
            _eNTKOMUTLARBEKLEYENService = eNTKOMUTLARBEKLEYENService;
        }


        public ActionResult Index()
        {
            SayfaBaslik($"Bekleyen Komut İşlemleri");
            var model = new ENTKOMUTLARBEKLEYENIndexModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, ENTKOMUTLARBEKLEYENAra eNTKOMUTLARBEKLEYENAra)
        {

            eNTKOMUTLARBEKLEYENAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                //eNTKOMUTLARBEKLEYENAra.Adi = dtParameterModel.Search.Value;
            }



            var kayitlar = _eNTKOMUTLARBEKLEYENService.Ara(eNTKOMUTLARBEKLEYENAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.ENTKOMUTLARBEKLEYENDetayList.Select(t => new
                {

                    t.KOMUTID,
                    t.BAGLANTIDENEMESAYISI,
                    t.ACIKLAMA,
                    t.GUIDID,
                    t.KONSSERINO,
                    t.KOMUTKODU,
                    t.KOMUT,
                    ISLEMTARIH=t.ISLEMTARIH.ToString(),

                    Islemler = $@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "ENTKOMUTLARBEKLEYEN", new { id = t.GUIDID })}' title='Düzenle'><i class='fa fa-edit'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "ENTKOMUTLARBEKLEYEN", new { id = t.GUIDID })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Ekle()
        {
            SayfaBaslik($"Komut Ekle");

            var model = new ENTKOMUTLARBEKLEYENKaydetModel
            {
                ENTKOMUTLARBEKLEYEN = new ENTKOMUTLARBEKLEYEN()
            };



            return View("Kaydet", model);
        }


        public ActionResult Guncelle(int id)
        {
            SayfaBaslik($"Komut Güncelle");

            var model = new ENTKOMUTLARBEKLEYENKaydetModel
            {
                ENTKOMUTLARBEKLEYEN = _eNTKOMUTLARBEKLEYENService.GetirById(id)
            };


            return View("Kaydet", model);
        }



        public ActionResult Detay(int id)
        {
            SayfaBaslik($"Bekleyen Komut Detay");

            var model = new ENTKOMUTLARBEKLEYENDetayModel
            {
                ENTKOMUTLARBEKLEYENDetay = _eNTKOMUTLARBEKLEYENService.DetayGetirById(id)
            };


            return View("Detay", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(ENTKOMUTLARBEKLEYENKaydetModel eNTKOMUTLARBEKLEYENKaydetModel)
        {
            if (eNTKOMUTLARBEKLEYENKaydetModel.ENTKOMUTLARBEKLEYEN.GUIDID == null)
            {
                _eNTKOMUTLARBEKLEYENService.Guncelle(eNTKOMUTLARBEKLEYENKaydetModel.ENTKOMUTLARBEKLEYEN.List());
            }
            else
            {
                _eNTKOMUTLARBEKLEYENService.Ekle(eNTKOMUTLARBEKLEYENKaydetModel.ENTKOMUTLARBEKLEYEN.List());
            }

            return Yonlendir(Url.Action("Index"), $"Bekleyen Komut kayıdı başarıyla gerçekleştirilmiştir.");
            //return Yonlendir(Url.Action("Detay","ENTKOMUTLARBEKLEYEN",new{id=eNTKOMUTLARBEKLEYENKaydetModel.ENTKOMUTLARBEKLEYEN.Id}), $"ENTKOMUTLARBEKLEYEN kayıdı başarıyla gerçekleştirilmiştir.");
        }



        public ActionResult Sil(Guid id)
        {
            SayfaBaslik($"Bekleyen Komut Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel() {GuidId=id, RedirectUrlForCancel = $"/ENTKOMUTLARBEKLEYEN/Index" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
            _eNTKOMUTLARBEKLEYENService.Sil(model.GuidId.List());

            return Yonlendir(Url.Action("Index"), $"Bekleyen Komut Başarıyla silindi");
        }


    }
}

