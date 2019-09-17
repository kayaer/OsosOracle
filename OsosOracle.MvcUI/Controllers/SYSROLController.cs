using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.SYSROLComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.SYSROLModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.MvcUI.Filters;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class SYSROLController : BaseController
    {
        private readonly ISYSROLService _sYSROLService;
        private readonly ISYSROLKULLANICIService _sysRolKullaniciService;
        public SYSROLController(ISYSROLService sYSROLService, ISYSROLKULLANICIService sysRolKullaniciService)
        {
            _sYSROLService = sYSROLService;
            _sysRolKullaniciService = sysRolKullaniciService;
        }


        public ActionResult Index()
        {
            SayfaBaslik($"Rol İşlemleri");
            var model = new SYSROLIndexModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, SYSROLAra sYSROLAra)
        {

            sYSROLAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                sYSROLAra.AD = dtParameterModel.Search.Value;
            }



            var kayitlar = _sYSROLService.Ara(sYSROLAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.SYSROLDetayList.Select(t => new
                {
                    //TODO: Bu bölümü düzenle
                    t.KAYITNO,
                    t.AD,
                    t.ACIKLAMA,
                    t.VERSIYON,
                    t.KURUMKAYITNO,
                    t.Kurum,

                    Islemler = $@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "SYSROL", new { id = t.KAYITNO })}' title='Düzenle'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "SYSROL", new { id = t.KAYITNO })}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "SYSROL", new { id = t.KAYITNO })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Ekle()
        {
            SayfaBaslik($"Rol Ekle");

            var model = new SYSROLKaydetModel
            {
                SYSROL = new SYSROL()
            };



            return View("Kaydet", model);
        }


        public ActionResult Guncelle(int id)
        {
            SayfaBaslik($"Rol Güncelle");

            var model = new SYSROLKaydetModel
            {
                SYSROL = _sYSROLService.GetirById(id)
            };


            return View("Kaydet", model);
        }



        public ActionResult Detay(int id)
        {
            SayfaBaslik($"Rol Detay");

            var model = new SYSROLDetayModel
            {
                SYSROLDetay = _sYSROLService.DetayGetirById(id)
            };


            return View("Detay", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(SYSROLKaydetModel sYSROLKaydetModel)
        {
            if (sYSROLKaydetModel.SYSROL.KAYITNO > 0)
            {
                _sYSROLService.Guncelle(sYSROLKaydetModel.SYSROL.List());
            }
            else
            {
                _sYSROLService.Ekle(sYSROLKaydetModel.SYSROL.List());
            }

            return Yonlendir(Url.Action("Index"), $"SYSROL kayıdı başarıyla gerçekleştirilmiştir.");
            //return Yonlendir(Url.Action("Detay","SYSROL",new{id=sYSROLKaydetModel.SYSROL.Id}), $"SYSROL kayıdı başarıyla gerçekleştirilmiştir.");
        }



        public ActionResult Sil(int id)
        {
            SayfaBaslik($"SYSROL Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel = $"/SYSROL/Index/{id}" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
            _sysRolKullaniciService.RolKullaniciSil(model.Id, null);
            _sYSROLService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"), $"Rol Başarıyla silindi");
        }


        public ActionResult AjaxAra(string key, SYSROLAra sYSROLAra = null, int limit = 10, int baslangic = 0)
        {

            if (sYSROLAra == null)
            {
                sYSROLAra = new SYSROLAra();
            }

            sYSROLAra.Ara = new Ara
            {
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {
                    new Siralama
                    {
                        KolonAdi = LinqExtensions.GetPropertyName((SYSROL t) => t.KAYITNO),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }
                }
            };


            //TODO: Bu bölümü düzenle
            sYSROLAra.AD = key;

            var sYSROLList = _sYSROLService.Getir(sYSROLAra);


            var data = sYSROLList.Select(sYSROL => new AutoCompleteData
            {
                //TODO: Bu bölümü düzenle
                id = sYSROL.KAYITNO.ToString(),
                text = sYSROL.AD.ToString(),
                description = sYSROL.AD.ToString(),
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AjaxTekDeger(int id)
        {
            var sYSROL = _sYSROLService.GetirById(id);


            var data = new AutoCompleteData
            {//TODO: Bu bölümü düzenle
                id = sYSROL.KAYITNO.ToString(),
                text = sYSROL.AD.ToString(),
                description = sYSROL.AD.ToString(),
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxCokDeger(string id)
        {
            var sYSROLList = _sYSROLService.Getir(new SYSROLAra() { KAYITNOlar = id });


            var data = sYSROLList.Select(sYSROL => new AutoCompleteData
            { //TODO: Bu bölümü düzenle
                id = sYSROL.KAYITNO.ToString(),
                text = sYSROL.AD.ToString(),
                description = sYSROL.AD.ToString()
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}

