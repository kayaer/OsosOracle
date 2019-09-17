using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.CONKURUMComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.MvcUI.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class KurumController : BaseController
    {
        private readonly ICONKURUMService _conKurumService;

        public KurumController(ICONKURUMService conKurumService)
        {
            _conKurumService = conKurumService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DataTablesList(DtParameterModel dtParameterModel, CONKURUMAra conKurumAra)
        {

            conKurumAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                //acigaAlinmaAra.Adi = dtParameterModel.Search.Value;
            }

            var kayitlar = _conKurumService.Ara(conKurumAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.CONKURUMDetayList.Select(t => new
                {
                    t.KAYITNO,
                    t.AD,
                    t.ACIKLAMA,
                    Islemler = $@"<a class='btn btn-xs btn-info ' href='{Url.Action("Guncelle", "Kurum", new { id = t.KAYITNO })}' title='Düzenle'><i class='fa fa-edit'></i></a>							 
								<a class='btn btn-xs btn-danger modalizer ' href='{Url.Action("Sil", "Kurum", new { id = t.KAYITNO })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Sil(int id)
        {
            SayfaBaslik("Kurum Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel { Id = id, RedirectUrlForCancel = $"/Kurum/Index" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {

            _conKurumService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"), "Kurum Başarıyla Silindi");
        }

        public ActionResult AjaxAra(string key, CONKURUMAra conKurumAra = null, int limit = 10, int baslangic = 0)
        {

            if (conKurumAra == null)
            {
                conKurumAra = new CONKURUMAra();
            }

            conKurumAra.Ara = new Ara
            {
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {
                    new Siralama
                    {
                        KolonAdi = LinqExtensions.GetPropertyName((CONKURUM t) => t.KAYITNO),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }
                }
            };


            //TODO: Bu bölümü düzenle
            conKurumAra.AD = key;

            var conKurumList = _conKurumService.Getir(conKurumAra);


            var data = conKurumList.Select(conKurum => new AutoCompleteData
            {
                //TODO: Bu bölümü düzenle
                id = conKurum.KAYITNO.ToString(),
                text = conKurum.AD.ToString(),
                description = conKurum.KAYITNO.ToString(),
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AjaxTekDeger(int id)
        {
            var conKurum = _conKurumService.GetirById(id);


            var data = new AutoCompleteData
            {//TODO: Bu bölümü düzenle
                id = conKurum.KAYITNO.ToString(),
                text = conKurum.AD.ToString(),
                description = conKurum.KAYITNO.ToString(),
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxCokDeger(string id)
        {
            var conKurumList = _conKurumService.Getir(new CONKURUMAra() { KAYITNOlar = id });


            var data = conKurumList.Select(conKurum => new AutoCompleteData
            { //TODO: Bu bölümü düzenle
                id = conKurum.KAYITNO.ToString(),
                text = conKurum.AD.ToString(),
                description = conKurum.KAYITNO.ToString()
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

     
    }
}