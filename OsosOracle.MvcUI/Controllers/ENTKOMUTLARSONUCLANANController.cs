using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.ENTKOMUTLARSONUCLANANComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.ENTKOMUTLARSONUCLANANModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.MvcUI.Filters;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class ENTKOMUTLARSONUCLANANController : BaseController
    {
        private readonly IENTKOMUTLARSONUCLANANService _eNTKOMUTLARSONUCLANANService;

        public ENTKOMUTLARSONUCLANANController(IENTKOMUTLARSONUCLANANService eNTKOMUTLARSONUCLANANService)
        {
            _eNTKOMUTLARSONUCLANANService = eNTKOMUTLARSONUCLANANService;
        }


        public ActionResult Index()
        {
            SayfaBaslik($"Sonuçlanan Komut İşlemleri");
            var model = new ENTKOMUTLARSONUCLANANIndexModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, ENTKOMUTLARSONUCLANANAra eNTKOMUTLARSONUCLANANAra)
        {

            eNTKOMUTLARSONUCLANANAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
              //eNTKOMUTLARSONUCLANANAra.Adi = dtParameterModel.Search.Value;
            }



            var kayitlar = _eNTKOMUTLARSONUCLANANService.Ara(eNTKOMUTLARSONUCLANANAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.ENTKOMUTLARSONUCLANANDetayList.Select(t => new
                {
                    //TODO: Bu bölümü düzenle
                    t.KAYITNO,
                    t.KONSSERINO,
                    t.KOMUTKODU,
                    t.KOMUT,
                    ISLEMTARIH=t.ISLEMTARIH.ToString(),
                    t.SONUC,
                    t.ISLEMSURESI,
                    t.KOMUTID,
                    t.ACIKLAMA,
                    t.CEVAP,

                    Islemler = $@"<a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "ENTKOMUTLARSONUCLANAN", new { id = t.KAYITNO })}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "ENTKOMUTLARSONUCLANAN", new { id = t.KAYITNO })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Ekle()
        {
            SayfaBaslik($"ENTKOMUTLARSONUCLANAN Ekle");

            var model = new ENTKOMUTLARSONUCLANANKaydetModel
            {
                ENTKOMUTLARSONUCLANAN = new ENTKOMUTLARSONUCLANAN()
            };



            return View("Kaydet", model);
        }


        public ActionResult Guncelle(int id)
        {
            SayfaBaslik($"Sonuçlanan Komut  Güncelle");

            var model = new ENTKOMUTLARSONUCLANANKaydetModel
            {
                ENTKOMUTLARSONUCLANAN = _eNTKOMUTLARSONUCLANANService.GetirById(id)
            };


            return View("Kaydet", model);
        }



        public ActionResult Detay(int id)
        {
            SayfaBaslik($"Sonuçlanan Komut  Detay");

            var model = new ENTKOMUTLARSONUCLANANDetayModel
            {
                ENTKOMUTLARSONUCLANANDetay = _eNTKOMUTLARSONUCLANANService.DetayGetirById(id)
            };


            return View("Detay", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(ENTKOMUTLARSONUCLANANKaydetModel eNTKOMUTLARSONUCLANANKaydetModel)
        {
            if (eNTKOMUTLARSONUCLANANKaydetModel.ENTKOMUTLARSONUCLANAN.KAYITNO > 0)
            {
                _eNTKOMUTLARSONUCLANANService.Guncelle(eNTKOMUTLARSONUCLANANKaydetModel.ENTKOMUTLARSONUCLANAN.List());
            }
            else
            {
                _eNTKOMUTLARSONUCLANANService.Ekle(eNTKOMUTLARSONUCLANANKaydetModel.ENTKOMUTLARSONUCLANAN.List());
            }

            return Yonlendir(Url.Action("Index"), $"Sonuçlanan Komut  kayıdı başarıyla gerçekleştirilmiştir.");
            //return Yonlendir(Url.Action("Detay","ENTKOMUTLARSONUCLANAN",new{id=eNTKOMUTLARSONUCLANANKaydetModel.ENTKOMUTLARSONUCLANAN.Id}), $"ENTKOMUTLARSONUCLANAN kayıdı başarıyla gerçekleştirilmiştir.");
        }



        public ActionResult Sil(int id)
        {
            SayfaBaslik($"ENTKOMUTLARSONUCLANAN Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel = $"/ENTKOMUTLARSONUCLANAN/Index/{id}" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
            _eNTKOMUTLARSONUCLANANService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"), $"Sonuçlanan Komut  Başarıyla silindi");
        }


        
    }
}

