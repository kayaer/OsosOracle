
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.NESNETIPComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.NESNETIPModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.MvcUI.Filters;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class NESNETIPController : BaseController
    {
        private readonly INESNETIPService _nESNETIPService;

        public NESNETIPController(INESNETIPService nESNETIPService)
        {
            _nESNETIPService = nESNETIPService;
        }


        public ActionResult Index()
        {
            SayfaBaslik($"NESNETIP İşlemleri");
            var model = new NESNETIPIndexModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, NESNETIPAra nESNETIPAra)
        {

            nESNETIPAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                nESNETIPAra.ADI = dtParameterModel.Search.Value;
            }



            var kayitlar = _nESNETIPService.Ara(nESNETIPAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.NESNETIPDetayList.Select(t => new
                {
                    //TODO: Bu bölümü düzenle
                    t.KAYITNO,
                    t.ADI,

                    Islemler = $@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "NESNETIP", new { id = t.KAYITNO })}' title='Düzenle'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "NESNETIP", new { id = t.KAYITNO })}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "NESNETIP", new { id = t.KAYITNO })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Ekle()
        {
            SayfaBaslik($"NESNETIP Ekle");

            var model = new NESNETIPKaydetModel
            {
                NESNETIP = new NESNETIP()
            };



            return View("Kaydet", model);
        }


        public ActionResult Guncelle(int id)
        {
            SayfaBaslik($"NESNETIP Güncelle");

            var model = new NESNETIPKaydetModel
            {
                NESNETIP = _nESNETIPService.GetirById(id)
            };


            return View("Kaydet", model);
        }



        public ActionResult Detay(int id)
        {
            SayfaBaslik($"NESNETIP Detay");

            var model = new NESNETIPDetayModel
            {
                NESNETIPDetay = _nESNETIPService.DetayGetirById(id)
            };


            return View("Detay", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(NESNETIPKaydetModel nESNETIPKaydetModel)
        {
            if (nESNETIPKaydetModel.NESNETIP.KAYITNO > 0)
            {
                _nESNETIPService.Guncelle(nESNETIPKaydetModel.NESNETIP.List());
            }
            else
            {
                _nESNETIPService.Ekle(nESNETIPKaydetModel.NESNETIP.List());
            }

            return Yonlendir(Url.Action("Index"), $"NESNETIP kayıdı başarıyla gerçekleştirilmiştir.");
            //return Yonlendir(Url.Action("Detay","NESNETIP",new{id=nESNETIPKaydetModel.NESNETIP.Id}), $"NESNETIP kayıdı başarıyla gerçekleştirilmiştir.");
        }



        public ActionResult Sil(int id)
        {
            SayfaBaslik($"NESNETIP Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel = $"/NESNETIP/Index/{id}" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
            _nESNETIPService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"), $"NESNETIP Başarıyla silindi");
        }


        public ActionResult AjaxAra(string key, NESNETIPAra nESNETIPAra = null, int limit = 10, int baslangic = 0)
        {

            if (nESNETIPAra == null)
            {
                nESNETIPAra = new NESNETIPAra();
            }

            nESNETIPAra.Ara = new Ara
            {
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {
                    new Siralama
                    {
                        KolonAdi = LinqExtensions.GetPropertyName((NESNETIP t) => t.KAYITNO),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }
                }
            };


            //TODO: Bu bölümü düzenle
            nESNETIPAra.ADI = key;

            var nESNETIPList = _nESNETIPService.Getir(nESNETIPAra);


            var data = nESNETIPList.Select(nESNETIP => new AutoCompleteData
            {
                //TODO: Bu bölümü düzenle
                id = nESNETIP.KAYITNO.ToString(),
                text = nESNETIP.ADI.ToString(),
                description = nESNETIP.KAYITNO.ToString(),
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AjaxTekDeger(int id)
        {
            var nESNETIP = _nESNETIPService.GetirById(id);


            var data = new AutoCompleteData
            {//TODO: Bu bölümü düzenle
                id = nESNETIP.KAYITNO.ToString(),
                text = nESNETIP.ADI.ToString(),
                description = nESNETIP.KAYITNO.ToString(),
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxCokDeger(string id)
        {
            var nESNETIPList = _nESNETIPService.Getir(new NESNETIPAra() { KAYITNOlar = id });


            var data = nESNETIPList.Select(nESNETIP => new AutoCompleteData
            { //TODO: Bu bölümü düzenle
                id = nESNETIP.KAYITNO.ToString(),
                text = nESNETIP.ADI.ToString(),
                description = nESNETIP.KAYITNO.ToString()
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}

