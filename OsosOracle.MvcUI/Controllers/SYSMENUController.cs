using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.SYSMENUComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.SYSMENUModels;
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
    public class SYSMENUController : BaseController
    {
        private readonly ISYSMENUService _sYSMENUService;

        public SYSMENUController(ISYSMENUService sYSMENUService)
        {
            _sYSMENUService = sYSMENUService;
        }


        public ActionResult Index()
        {
            SayfaBaslik($"Menü İşlemleri");
            var model = new SYSMENUIndexModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, SYSMENUAra sYSMENUAra)
        {

            sYSMENUAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                sYSMENUAra.TR = dtParameterModel.Search.Value;
            }



            var kayitlar = _sYSMENUService.Ara(sYSMENUAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.SYSMENUDetayList.Select(t => new
                {
                    //TODO: Bu bölümü düzenle
                    t.KAYITNO,
                    t.TR,
                    t.PARENTKAYITNO,
                    t.MENUORDER,
               
                    t.AREA,
                    t.ACTION,
                    t.CONTROLLER,
                  
                    t.DURUM,
                    t.VERSIYON,
                    t.ICON,

                    Islemler = $@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "SYSMENU", new { id = t.KAYITNO })}' title='{Dil.Duzenle}'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "SYSMENU", new { id = t.KAYITNO })}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "SYSMENU", new { id = t.KAYITNO })}' title='{Dil.Sil}'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Ekle()
        {
            SayfaBaslik($"Menü Ekle");

            var model = new SYSMENUKaydetModel
            {
                SYSMENU = new SYSMENU()
            };
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (SYSMENU parentmenu in _sYSMENUService.ParentMenuGetir())
            {

                SelectListItem selectListItem = new SelectListItem()
                {
                    Text = parentmenu.TR,
                    Value = parentmenu.KAYITNO.ToString(),
                    Selected = false
                };
                selectListItems.Add(selectListItem);


            }

            ViewBag.ParentMenu = selectListItems;


            return View("Kaydet", model);
        }


        public ActionResult Guncelle(int id)
        {
            SayfaBaslik($"Menü Güncelle");

            var model = new SYSMENUKaydetModel
            {
                SYSMENU = _sYSMENUService.GetirById(id)
            };

            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (SYSMENU parentmenu in _sYSMENUService.ParentMenuGetir())
            {

                SelectListItem selectListItem = new SelectListItem()
                {
                    Text = parentmenu.TR,
                    Value = parentmenu.KAYITNO.ToString(),
                    Selected = false
                };
                selectListItems.Add(selectListItem);


            }

            ViewBag.ParentMenu = selectListItems;
            return View("Kaydet", model);
        }



        public ActionResult Detay(int id)
        {
            SayfaBaslik($"Menü Detay");

            var model = new SYSMENUDetayModel
            {
                SYSMENUDetay = _sYSMENUService.DetayGetirById(id)
            };


            return View("Detay", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(SYSMENUKaydetModel sYSMENUKaydetModel)
        {
            if (sYSMENUKaydetModel.SYSMENU.KAYITNO > 0)
            {
                _sYSMENUService.Guncelle(sYSMENUKaydetModel.SYSMENU.List());
            }
            else
            {
                _sYSMENUService.Ekle(sYSMENUKaydetModel.SYSMENU.List());
            }

            return Yonlendir(Url.Action("Index"), $"SYSMENU kayıdı başarıyla gerçekleştirilmiştir.");
            //return Yonlendir(Url.Action("Detay","SYSMENU",new{id=sYSMENUKaydetModel.SYSMENU.Id}), $"SYSMENU kayıdı başarıyla gerçekleştirilmiştir.");
        }



        public ActionResult Sil(int id)
        {
            SayfaBaslik($"Menü Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel = $"/SYSMENU/Index/{id}" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
            _sYSMENUService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"), $"Menü Başarıyla silindi");
        }


        public ActionResult AjaxAra(string key, SYSMENUAra sYSMENUAra = null, int limit = 10, int baslangic = 0)
        {

            if (sYSMENUAra == null)
            {
                sYSMENUAra = new SYSMENUAra();
            }

            sYSMENUAra.Ara = new Ara
            {
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {
                    new Siralama
                    {
                        KolonAdi = LinqExtensions.GetPropertyName((SYSMENU t) => t.KAYITNO),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }
                }
            };


            //TODO: Bu bölümü düzenle
            sYSMENUAra.TR = key;

            var sYSMENUList = _sYSMENUService.Getir(sYSMENUAra);


            var data = sYSMENUList.Select(sYSMENU => new AutoCompleteData
            {
                //TODO: Bu bölümü düzenle
                id = sYSMENU.KAYITNO.ToString(),
                text = sYSMENU.TR.ToString(),
                description = sYSMENU.TR.ToString(),
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AjaxTekDeger(int id)
        {
            var sYSMENU = _sYSMENUService.GetirById(id);


            var data = new AutoCompleteData
            {//TODO: Bu bölümü düzenle
                id = sYSMENU.KAYITNO.ToString(),
                text = sYSMENU.TR.ToString(),
                description = sYSMENU.TR.ToString(),
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxCokDeger(string id)
        {
            var sYSMENUList = _sYSMENUService.Getir(new SYSMENUAra() { KAYITNOlar = id });


            var data = sYSMENUList.Select(sYSMENU => new AutoCompleteData
            { //TODO: Bu bölümü düzenle
                id = sYSMENU.KAYITNO.ToString(),
                text = sYSMENU.TR.ToString(),
                description = sYSMENU.TR.ToString()
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetParentMenu()
        {
            IEnumerable<SelectListItem> parentMenu = _sYSMENUService.DetayGetir(new SYSMENUAra() { PARENTKAYITNO = null }).Select(x => new SelectListItem() { Text = x.TR, Value = x.KAYITNO.ToString() }).ToList();
            return Json(parentMenu);
        }

    }
}

