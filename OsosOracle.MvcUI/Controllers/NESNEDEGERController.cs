
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.NESNEDEGERComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.NESNEDEGERModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.MvcUI.Filters;
using OsosOracle.MvcUI.Resources;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class NESNEDEGERController : BaseController
    {
        private readonly INESNEDEGERService _nESNEDEGERService;

        public NESNEDEGERController(INESNEDEGERService nESNEDEGERService)
        {
            _nESNEDEGERService = nESNEDEGERService;
        }


        public ActionResult Index()
        {
            SayfaBaslik($"Nesne Değer İşlemleri");
            var model = new NESNEDEGERIndexModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, NESNEDEGERAra nESNEDEGERAra)
        {

            nESNEDEGERAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                nESNEDEGERAra.AD = dtParameterModel.Search.Value;
            }



            var kayitlar = _nESNEDEGERService.Ara(nESNEDEGERAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.NESNEDEGERDetayList.Select(t => new
                {
                    //TODO: Bu bölümü düzenle
                    t.KAYITNO,
                    t.NESNETIPKAYITNO,
                    t.AD,
                    t.DEGER,
                    t.BILGI,
                    t.SIRANO,

                    Islemler = $@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "NESNEDEGER", new { id = t.KAYITNO })}' title='Düzenle'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "NESNEDEGER", new { id = t.KAYITNO })}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "NESNEDEGER", new { id = t.KAYITNO })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Ekle()
        {
            SayfaBaslik($"Nesne Değer Ekle");

            var model = new NESNEDEGERKaydetModel
            {
                NESNEDEGER = new NESNEDEGER()
            };



            return View("Kaydet", model);
        }


        public ActionResult Guncelle(int id)
        {
            SayfaBaslik($"Nesne Değer Güncelle");

            var model = new NESNEDEGERKaydetModel
            {
                NESNEDEGER = _nESNEDEGERService.GetirById(id)
            };


            return View("Kaydet", model);
        }



        public ActionResult Detay(int id)
        {
            SayfaBaslik($"Nesne Değer Detay");

            var model = new NESNEDEGERDetayModel
            {
                NESNEDEGERDetay = _nESNEDEGERService.DetayGetirById(id)
            };


            return View("Detay", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(NESNEDEGERKaydetModel nESNEDEGERKaydetModel)
        {
            if (nESNEDEGERKaydetModel.NESNEDEGER.KAYITNO > 0)
            {
                _nESNEDEGERService.Guncelle(nESNEDEGERKaydetModel.NESNEDEGER.List());
            }
            else
            {
                _nESNEDEGERService.Ekle(nESNEDEGERKaydetModel.NESNEDEGER.List());
            }

            return Yonlendir(Url.Action("Index"), $"Nesne Değer kayıdı başarıyla gerçekleştirilmiştir.");
            //return Yonlendir(Url.Action("Detay","NESNEDEGER",new{id=nESNEDEGERKaydetModel.NESNEDEGER.Id}), $"NESNEDEGER kayıdı başarıyla gerçekleştirilmiştir.");
        }



        public ActionResult Sil(int id)
        {
            SayfaBaslik($"Nesne Değer Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel = $"/NESNEDEGER/Index/{id}" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
            _nESNEDEGERService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"), $"Nesne Değer Başarıyla silindi");
        }


        public ActionResult AjaxAra(string key, NESNEDEGERAra nESNEDEGERAra = null, int limit = 10, int baslangic = 0, int rootId = -1)
        {

            if (nESNEDEGERAra == null)
            {
                nESNEDEGERAra = new NESNEDEGERAra();
            }
           
            nESNEDEGERAra.AD = key;
            nESNEDEGERAra.Ara = new Ara
            {
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {
                    new Siralama
                    {
                        KolonAdi = LinqExtensions.GetPropertyName((NESNEDEGER t) => t.KAYITNO),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }
                }
            };

            var nESNEDEGERList = _nESNEDEGERService.Getir(nESNEDEGERAra);
            var data = nESNEDEGERList.Select(nESNEDEGER => new AutoCompleteData
            {
                //TODO: Bu bölümü düzenle
                id = nESNEDEGER.KAYITNO.ToString(),
                text = Dil.ResourceManager.GetString( nESNEDEGER.AD.ToString())==null?nESNEDEGER.AD.ToString(): Dil.ResourceManager.GetString(nESNEDEGER.AD.ToString()),
                description = nESNEDEGER.KAYITNO.ToString(),
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AjaxTekDeger(int id)
        {
            var nESNEDEGER = _nESNEDEGERService.GetirById(id);


            var data = new AutoCompleteData
            {//TODO: Bu bölümü düzenle
                id = nESNEDEGER.KAYITNO.ToString(),
                text = Dil.ResourceManager.GetString(nESNEDEGER.AD.ToString()) == null ? nESNEDEGER.AD.ToString() : Dil.ResourceManager.GetString(nESNEDEGER.AD.ToString()),
                description = nESNEDEGER.KAYITNO.ToString(),
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxCokDeger(string id)
        {
            var nESNEDEGERList = _nESNEDEGERService.Getir(new NESNEDEGERAra() { KAYITNOlar = id });


            var data = nESNEDEGERList.Select(nESNEDEGER => new AutoCompleteData
            { //TODO: Bu bölümü düzenle
                id = nESNEDEGER.KAYITNO.ToString(),
                text = Dil.ResourceManager.GetString(nESNEDEGER.AD.ToString()) == null ? nESNEDEGER.AD.ToString() : Dil.ResourceManager.GetString(nESNEDEGER.AD.ToString()),
                description = nESNEDEGER.KAYITNO.ToString()
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}

