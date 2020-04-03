using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.SYSGOREVROLComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.SYSGOREVROLModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.MvcUI.Filters;
using OsosOracle.Entities.ComplexType.SYSROLComplexTypes;
using OsosOracle.MvcUI.Models.SYSROLModels;
using OsosOracle.Entities.ComplexType.SYSGOREVComplexTypes;
using OsosOracle.MvcUI.Models.SYSGOREVModels;
using System;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class SYSGOREVROLController : BaseController
    {
        private readonly ISYSGOREVROLService _sYSGOREVROLService;
        private readonly ISYSROLService _sysRolService;
        private readonly ISYSGOREVService _sysGorevService;
        public SYSGOREVROLController(ISYSGOREVROLService sYSGOREVROLService, ISYSROLService sysRolService, ISYSGOREVService sysGorevService)
        {
            _sYSGOREVROLService = sYSGOREVROLService;
            _sysRolService = sysRolService;
            _sysGorevService = sysGorevService;
        }


        public ActionResult Index()
        {
            SayfaBaslik($"SYSGOREVROL İşlemleri");
            var model = new SYSGOREVROLIndexModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, SYSGOREVROLAra sYSGOREVROLAra)
        {

            sYSGOREVROLAra.Ara = dtParameterModel.AramaKriteri;

            //if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            //{ //TODO: Bu bölümü düzenle
            //	sYSGOREVROLAra.A = dtParameterModel.Search.Value;
            //}



            var kayitlar = _sYSGOREVROLService.Ara(sYSGOREVROLAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.SYSGOREVROLDetayList.Select(t => new
                {
                    //TODO: Bu bölümü düzenle
                    t.KAYITNO,
                    t.GOREVKAYITNO,
                    t.ROLKAYITNO,
                    t.VERSIYON,

                    Islemler = $@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "SYSGOREVROL", new { id = t.KAYITNO })}' title='Düzenle'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "SYSGOREVROL", new { id = t.KAYITNO })}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "SYSGOREVROL", new { id = t.KAYITNO })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Ekle()
        {
            //SayfaBaslik($"SYSGOREVROL Ekle");

            //var model = new SYSGOREVROLKaydetModel
            //{
            //    SYSGOREVROL = new SYSGOREVROL()
            //};

            var rollistesi = _sysRolService.DetayGetir(new SYSROLAra()).Select(x => new Rol { KayitNo = x.KAYITNO, Ad = x.AD, Aciklama = x.ACIKLAMA, SysGorevList = x.SysGorevList }).ToList();
            var gorevListesi = _sysGorevService.Getir(new SYSGOREVAra())
                .Select(x => new Gorev() { KayıtNo = x.KAYITNO, Ad = x.AD, Aciklama = x.ACIKLAMA }).ToList();

            GorevRolAtamaModel model = new GorevRolAtamaModel();
            model.RolListesi = rollistesi;
            model.GorevListesi = gorevListesi;

            return View("Kaydet", model);
        }


        public ActionResult Guncelle(int id)
        {
            SayfaBaslik($"SYSGOREVROL Güncelle");

            var model = new SYSGOREVROLKaydetModel
            {
                SYSGOREVROL = _sYSGOREVROLService.GetirById(id)
            };


            return View("Kaydet", model);
        }



        public ActionResult Detay(int id)
        {
            SayfaBaslik($"SYSGOREVROL Detay");

            var model = new SYSGOREVROLDetayModel
            {
                SYSGOREVROLDetay = _sYSGOREVROLService.DetayGetirById(id)
            };


            return View("Detay", model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Kaydet(GorevRolAtamaModel model)
        {
            try
            {
                foreach (var gorev in model.GorevListesi.Where(x => x.Secildi))
                {

                    foreach (var rol in model.RolListesi.Where(x => x.Secildi))
                    {

                        _sYSGOREVROLService.Ekle(new SYSGOREVROL() {
                            GOREVKAYITNO = gorev.KayıtNo,
                            ROLKAYITNO = rol.KayitNo,
                            OLUSTURAN = AktifKullanici.KayitNo }.List());
                    }

                }

                return Yonlendir(Url.Action("Index"), $"SYSGOREVROL kayıdı başarıyla gerçekleştirilmiştir.");
            }
            catch (Exception ex)
            {
                throw new NotificationException(ex.Message);
            }

            
        }



        public ActionResult Sil(int id)
        {
            SayfaBaslik($"SYSGOREVROL Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel = $"/SYSGOREVROL/Index/{id}" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
            _sYSGOREVROLService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"), $"SYSGOREVROL Başarıyla silindi");
        }
        public ActionResult RolGorevSil(int rolid, int gorevid)
        {

            bool durum = _sysGorevService.RolGorevSil(rolid, gorevid);
            if (durum)
            {
                return Yonlendir(Url.Action("Ekle"), "Rol görev silme başarıyla gerçekleştirilmiştir.");
            }
            else
            {
                return Yonlendir(Url.Action("GorevRolAtama"), "Rol görev silme bir hata oluştu.");
            }

        }

        //public ActionResult AjaxAra(string key, SYSGOREVROLAra sYSGOREVROLAra = null, int limit = 10, int baslangic = 0)
        //{

        //    if (sYSGOREVROLAra == null)
        //    {
        //        sYSGOREVROLAra = new SYSGOREVROLAra();
        //    }

        //    sYSGOREVROLAra.Ara = new Ara
        //    {
        //        Baslangic = baslangic,
        //        Uzunluk = limit,
        //        Siralama = new List<Siralama>
        //        {
        //            new Siralama
        //            {
        //                KolonAdi = LinqExtensions.GetPropertyName((SYSGOREVROL t) => t.KAYITNO),
        //                SiralamaTipi = EnumSiralamaTuru.Asc
        //            }
        //        }
        //    };


        //    //TODO: Bu bölümü düzenle
        //    sYSGOREVROLAra.Adi = key;

        //    var sYSGOREVROLList = _sYSGOREVROLService.Getir(sYSGOREVROLAra);


        //    var data = sYSGOREVROLList.Select(sYSGOREVROL => new AutoCompleteData
        //    {
        //        //TODO: Bu bölümü düzenle
        //        id = sYSGOREVROL.Id.ToString(),
        //        text = sYSGOREVROL.Id.ToString(),
        //        description = sYSGOREVROL.Id.ToString(),
        //    }).ToList();
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}


        //public ActionResult AjaxTekDeger(int id)
        //{
        //    var sYSGOREVROL = _sYSGOREVROLService.GetirById(id);


        //    var data = new AutoCompleteData
        //    {//TODO: Bu bölümü düzenle
        //        id = sYSGOREVROL.Id.ToString(),
        //        text = sYSGOREVROL.Id.ToString(),
        //        description = sYSGOREVROL.Id.ToString(),
        //    };

        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult AjaxCokDeger(string id)
        //{
        //    var sYSGOREVROLList = _sYSGOREVROLService.Getir(new SYSGOREVROLAra() { KAYITNOlar = id });


        //    var data = sYSGOREVROLList.Select(sYSGOREVROL => new AutoCompleteData
        //    { //TODO: Bu bölümü düzenle
        //        id = sYSGOREVROL.Id.ToString(),
        //        text = sYSGOREVROL.Id.ToString(),
        //        description = sYSGOREVROL.Id.ToString()
        //    });

        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

    }
}

