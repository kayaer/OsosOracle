using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.SYSGOREVComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.SYSGOREVModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.MvcUI.Filters;
using OsosOracle.Entities.ComplexType.SYSCSTOPERASYONComplexTypes;
using OsosOracle.MvcUI.Models.SYSCSTOPERASYONModels;
using OsosOracle.Entities.ComplexType.SYSOPERASYONGOREVComplexTypes;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class SYSGOREVController : BaseController
    {
        private readonly ISYSGOREVService _sYSGOREVService;
        private readonly ISYSCSTOPERASYONService _sysOperasyonService;
        private readonly ISYSOPERASYONGOREVService _sysOperasyonGorevService;

        public SYSGOREVController(ISYSGOREVService sYSGOREVService, ISYSCSTOPERASYONService sysOperasyonService, ISYSOPERASYONGOREVService sysOperasyonGorevService)
        {
            _sYSGOREVService = sYSGOREVService;
            _sysOperasyonService = sysOperasyonService;
            _sysOperasyonGorevService = sysOperasyonGorevService;
        }


        public ActionResult Index()
        {
            SayfaBaslik($"Görev İşlemleri");
            var model = new SYSGOREVIndexModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, SYSGOREVAra sYSGOREVAra)
        {

            sYSGOREVAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                sYSGOREVAra.AD = dtParameterModel.Search.Value;
            }



            var kayitlar = _sYSGOREVService.Ara(sYSGOREVAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.SYSGOREVDetayList.Select(t => new
                {
                    //TODO: Bu bölümü düzenle
                    t.KAYITNO,
                    t.AD,
                    t.ACIKLAMA,
                    t.VERSIYON,
                    t.KURUMKAYITNO,

                    Islemler = $@"<a class='btn btn-xs btn-info' href='{Url.Action("Guncelle", "SYSGOREV", new { id = t.KAYITNO })}' title='Düzenle'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "SYSGOREV", new { id = t.KAYITNO })}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "SYSGOREV", new { id = t.KAYITNO })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Ekle()
        {
            SayfaBaslik($"Görev Ekle");

          

            var model = new SYSGOREVKaydetModel()
            {
                SYSGOREV = new SYSGOREV(),
                OperasyonListesi = _sysOperasyonService.DetayGetir(new SYSCSTOPERASYONAra() { }).Select(x =>
                    new Operasyon() { KayitNo = x.KAYITNO, Ad = x.AD, Secildi = false, Aciklama = x.ACIKLAMA }).ToList()


            };


            return View("Kaydet", model);
        }


        public ActionResult Guncelle(int id)
        {
            SayfaBaslik($"Görev Güncelle");

           

            SYSGOREVDataTable GorevOperasyon = _sYSGOREVService.Ara(new SYSGOREVAra() { KAYITNO = id });
            var OperasyonListesi = _sysOperasyonService.DetayGetir(new SYSCSTOPERASYONAra() { }).Select(x =>
                new Operasyon() { KayitNo = x.KAYITNO, Ad = x.AD, Secildi = false, Aciklama = x.ACIKLAMA }).ToList();

            foreach (Operasyon operasyon in OperasyonListesi)
            {
                if (GorevOperasyon.SYSGOREVDetayList.FirstOrDefault().SysOperasyonList.Any(x => x.KAYITNO == operasyon.KayitNo))
                {
                    operasyon.Secildi = true;
                }


            }

            var model = new SYSGOREVKaydetModel()
            {
                SYSGOREV = _sYSGOREVService.GetirById(id),
                OperasyonListesi = OperasyonListesi


            };
            return View("Kaydet", model);
        }



        public ActionResult Detay(int id)
        {
            SayfaBaslik($"Görev Detay");

            var model = new SYSGOREVDetayModel
            {
                SYSGOREVDetay = _sYSGOREVService.DetayGetirById(id)
            };


            return View("Detay", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(SYSGOREVKaydetModel sYSGOREVKaydetModel)
        {
            sYSGOREVKaydetModel.SYSGOREV.KURUMKAYITNO = AktifKullanici.KurumKayitNo;
            if (sYSGOREVKaydetModel.SYSGOREV.KAYITNO > 0)
            {
                _sYSGOREVService.Guncelle(sYSGOREVKaydetModel.SYSGOREV.List());
            }
            else
            {
                _sYSGOREVService.Ekle(sYSGOREVKaydetModel.SYSGOREV.List());
            }

            _sysOperasyonGorevService.OperasyonGorevSil(sYSGOREVKaydetModel.SYSGOREV.KAYITNO);
            foreach (var operasyon in sYSGOREVKaydetModel.OperasyonListesi.Where(x => x.Secildi))
            {
                _sysOperasyonGorevService.Ekle(new SYSOPERASYONGOREV() { GOREVKAYITNO = sYSGOREVKaydetModel.SYSGOREV.KAYITNO, OPERASYONKAYITNO = operasyon.KayitNo }.List());

            }
            return Yonlendir(Url.Action("Index"), $"Görev kayıdı başarıyla gerçekleştirilmiştir.");

        }



        public ActionResult Sil(int id)
        {
            SayfaBaslik($"SYSGOREV Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel = $"/SYSGOREV/Index/{id}" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
            _sysOperasyonGorevService.OperasyonGorevSil(model.Id);
            _sYSGOREVService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"), $"Görev Başarıyla silindi");
        }


        public ActionResult AjaxAra(string key, SYSGOREVAra sYSGOREVAra = null, int limit = 10, int baslangic = 0)
        {

            if (sYSGOREVAra == null)
            {
                sYSGOREVAra = new SYSGOREVAra();
            }

            sYSGOREVAra.Ara = new Ara
            {
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {
                    new Siralama
                    {
                        KolonAdi = LinqExtensions.GetPropertyName((SYSGOREV t) => t.KAYITNO),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }
                }
            };


            //TODO: Bu bölümü düzenle
            sYSGOREVAra.AD = key;

            var sYSGOREVList = _sYSGOREVService.Getir(sYSGOREVAra);


            var data = sYSGOREVList.Select(sYSGOREV => new AutoCompleteData
            {
                //TODO: Bu bölümü düzenle
                id = sYSGOREV.KAYITNO.ToString(),
                text = sYSGOREV.AD.ToString(),
                description = sYSGOREV.AD.ToString(),
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AjaxTekDeger(int id)
        {
            var sYSGOREV = _sYSGOREVService.GetirById(id);


            var data = new AutoCompleteData
            {//TODO: Bu bölümü düzenle
                id = sYSGOREV.KAYITNO.ToString(),
                text = sYSGOREV.AD.ToString(),
                description = sYSGOREV.AD.ToString(),
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxCokDeger(string id)
        {
            var sYSGOREVList = _sYSGOREVService.Getir(new SYSGOREVAra() { KAYITNOlar = id });


            var data = sYSGOREVList.Select(sYSGOREV => new AutoCompleteData
            { //TODO: Bu bölümü düzenle
                id = sYSGOREV.KAYITNO.ToString(),
                text = sYSGOREV.AD.ToString(),
                description = sYSGOREV.AD.ToString()
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}

