using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.EntIsEmriComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.EntIsEmriModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.MvcUI.Filters;
using System;
using OsosOracle.Entities.Enums;
using OsosOracle.MvcUI.Resources;
namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class EntIsEmriController : BaseController
    {
        private readonly INESNEDEGERService _nesneDegerService;
        private readonly IEntIsEmriService _eNTKOMUTLARBEKLEYENService;
        private readonly IENTSAYACService _entSayacService;
        public EntIsEmriController(IEntIsEmriService eNTKOMUTLARBEKLEYENService, IENTSAYACService entSayacService, INESNEDEGERService nesneDegerService)
        {
            _eNTKOMUTLARBEKLEYENService = eNTKOMUTLARBEKLEYENService;
            _entSayacService = entSayacService;
            _nesneDegerService = nesneDegerService;
        }


        public ActionResult Index()
        {
           // SayfaBaslik(Dil.ise);
            var model = new EntIsEmriIndexModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, EntIsEmriAra entIsEmriAra)
        {

            entIsEmriAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                //eNTKOMUTLARBEKLEYENAra.Adi = dtParameterModel.Search.Value;
            }



            var kayitlar = _eNTKOMUTLARBEKLEYENService.Ara(entIsEmriAra);

            var result = Json(new DataTableResult()
            {
                data = kayitlar.EntIsEmriDetayList.Select(t => new
                {

                    t.KayitNo,
                    t.SayacKayitNo,
                    t.IsEmriKayitNo,
                    t.IsEmriDurumKayitNo,
                    t.IsEmriCevap,
                    t.SayacSeriNo,
                    t.IsEmriAdi,
                    t.IsEmriDurum,
                    OlusturmaTarih=t.OlusturmaTarih.ToString(),
                    GuncellemeTarih = t.GuncellemeTarih.ToString(),
                    t.Cevap,
                    Islemler = $@"<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "EntIsEmri", new { id = t.KayitNo })}' title='{Dil.Sil}'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);

            return result;
        }

       


        public ActionResult Ekle()
        {
            SayfaBaslik($"Komut Ekle");

            var model = new EntIsEmriKaydetModel
            {
                EntIsEmri = new EntIsEmri()
            };



            return View("Kaydet", model);
        }


        public ActionResult Guncelle(int id)
        {
            SayfaBaslik($"Komut Güncelle");

            var model = new EntIsEmriKaydetModel
            {
                EntIsEmri = _eNTKOMUTLARBEKLEYENService.GetirById(id)
            };


            return View("Kaydet", model);
        }



        public ActionResult Detay(int id)
        {
            SayfaBaslik($"Bekleyen Komut Detay");

            var model = new EntIsEmriDetayModel
            {
                EntIsEmriDetay = _eNTKOMUTLARBEKLEYENService.DetayGetirById(id)
            };


            return View("Detay", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(EntIsEmriKaydetModel eNTKOMUTLARBEKLEYENKaydetModel)
        {
            if (eNTKOMUTLARBEKLEYENKaydetModel.EntIsEmri.KAYITNO > 0)
            {
                _eNTKOMUTLARBEKLEYENService.Guncelle(eNTKOMUTLARBEKLEYENKaydetModel.EntIsEmri.List());
            }
            else
            {
                eNTKOMUTLARBEKLEYENKaydetModel.EntIsEmri.OlusturmaTarih = DateTime.Now;
                _eNTKOMUTLARBEKLEYENService.Ekle(eNTKOMUTLARBEKLEYENKaydetModel.EntIsEmri.List());
            }

            return Yonlendir(Url.Action("Index"), Dil.Basarili);
            //return Yonlendir(Url.Action("Detay","ENTKOMUTLARBEKLEYEN",new{id=eNTKOMUTLARBEKLEYENKaydetModel.ENTKOMUTLARBEKLEYEN.Id}), $"ENTKOMUTLARBEKLEYEN kayıdı başarıyla gerçekleştirilmiştir.");
        }



        public ActionResult Sil(int id)
        {
            SayfaBaslik($"İş Emri Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel = $"/EntIsEmri/Index" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
            _eNTKOMUTLARBEKLEYENService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"), Dil.Basarili);
        }


        #region Komut İşlemleri
        [HttpPost]
        public ActionResult VanaAc(EntIsEmriKaydetModel model)
        {
            var sayac = _entSayacService.DetayGetirById(model.EntIsEmri.SayacKayitNo);
            model.EntIsEmri.IsEmriKayitNo = enumIsEmirleri.VanaIslemi.GetHashCode();
            model.EntIsEmri.IsEmriDurumKayitNo = enumIsEmirleriDurum.Bekliyor.GetHashCode();
            model.EntIsEmri.Parametre = "ROLE_ON" + ",ELM" + sayac.SERINO + "-1-0-*-*-0";



            Kaydet(model);
            return Yonlendir(Url.Action("Index", "EntIsEmri", new { id = model.EntIsEmri.SayacKayitNo }), Dil.Basarili);

        }

        [HttpPost]
        public ActionResult VanaKapat(EntIsEmriKaydetModel model)
        {
            var sayac = _entSayacService.DetayGetirById(model.EntIsEmri.SayacKayitNo);
            model.EntIsEmri.IsEmriKayitNo = enumIsEmirleri.VanaIslemi.GetHashCode();
            model.EntIsEmri.IsEmriDurumKayitNo = enumIsEmirleriDurum.Bekliyor.GetHashCode();
            model.EntIsEmri.Parametre = "ROLE_OFF" + ",ELM" + sayac.SERINO + "-1-0-*-*-0";



            Kaydet(model);
            return Yonlendir(Url.Action("Index", "EntIsEmri", new { id = model.EntIsEmri.SayacKayitNo }), Dil.Basarili);

        }
        [HttpPost]
        public ActionResult Reset(EntIsEmriKaydetModel model)
        {
            // var sayac = _entSayacService.DetayGetirById(model.EntIsEmri.SayacKayitNo);
            model.EntIsEmri.IsEmriKayitNo = enumIsEmirleri.Reset.GetHashCode();
            model.EntIsEmri.IsEmriDurumKayitNo = enumIsEmirleriDurum.Bekliyor.GetHashCode();
            model.EntIsEmri.Parametre = "RESET";
            Kaydet(model);
            return Yonlendir(Url.Action("Index", "EntIsEmri", new { id = model.EntIsEmri.SayacKayitNo }), Dil.Basarili);

        }
        [HttpPost]
        public ActionResult YetkiKapat(EntIsEmriKaydetModel model)
        {
            // var sayac = _entSayacService.DetayGetirById(model.EntIsEmri.SayacKayitNo);
            model.EntIsEmri.IsEmriKayitNo = enumIsEmirleri.YetkiKapat.GetHashCode();
            model.EntIsEmri.IsEmriDurumKayitNo = enumIsEmirleriDurum.Bekliyor.GetHashCode();
            model.EntIsEmri.Parametre = "YETKI_KAPAT";
            Kaydet(model);
            return Yonlendir(Url.Action("Index", "EntIsEmri", new { id = model.EntIsEmri.SayacKayitNo }), Dil.Basarili);

        }
        [HttpPost]
        public ActionResult KrediYukle(EntIsEmriKaydetModel model)
        { 
            Random rnd = new Random();
            var sayac = _entSayacService.DetayGetirById(model.EntIsEmri.SayacKayitNo);
            model.EntIsEmri.IsEmriKayitNo = enumIsEmirleri.KrediYukle.GetHashCode();
            model.EntIsEmri.IsEmriDurumKayitNo = enumIsEmirleriDurum.Bekliyor.GetHashCode();
            //string komut = String.Concat(Flag, SayacSeriNo, "-7-0-*-*-0", ",", islemId, ",", credit);
            model.EntIsEmri.Parametre = String.Concat("ELM", sayac.SERINO, "-7-0-*-*-0", ",", rnd.Next(1, 100), ",", model.Kredi);
            Kaydet(model);
            return Yonlendir(Url.Action("Index", "EntIsEmri", new { id = model.EntIsEmri.SayacKayitNo }), Dil.Basarili);

        }

        [HttpPost]
        public ActionResult ZamanlanmisGorevEkle(EntIsEmriKaydetModel model)
        {
           
           // var sayac = _entSayacService.DetayGetirById(model.EntIsEmri.SayacKayitNo);
            model.EntIsEmri.IsEmriKayitNo = enumIsEmirleri.ZamanlanmisGorevEkleme.GetHashCode();
            model.EntIsEmri.IsEmriDurumKayitNo = enumIsEmirleriDurum.Bekliyor.GetHashCode();

            model.EntIsEmri.Parametre = _nesneDegerService.DetayGetirById(model.ZamanlanmisGorev).DEGER;
            Kaydet(model);
            return Yonlendir(Url.Action("Index", "EntIsEmri", new { id = model.EntIsEmri.SayacKayitNo }), Dil.Basarili);

        }

        [HttpPost]
        public ActionResult ServerIpPortSet(EntIsEmriKaydetModel model)
        {
            // var sayac = _entSayacService.DetayGetirById(model.EntIsEmri.SayacKayitNo);
            model.EntIsEmri.IsEmriKayitNo = enumIsEmirleri.ServerIpPortSet.GetHashCode();
            model.EntIsEmri.IsEmriDurumKayitNo = enumIsEmirleriDurum.Bekliyor.GetHashCode();

            model.EntIsEmri.Parametre = "1-" + model.ServerIp + "-" + model.ServerPort;
            Kaydet(model);
            return Yonlendir(Url.Action("Index", "EntIsEmri", new { id = model.EntIsEmri.SayacKayitNo }), Dil.Basarili);

        }

        [HttpPost]
        public ActionResult ApnSet(EntIsEmriKaydetModel model)
        {
            // var sayac = _entSayacService.DetayGetirById(model.EntIsEmri.SayacKayitNo);
            model.EntIsEmri.IsEmriKayitNo = enumIsEmirleri.ApnSet.GetHashCode();
            model.EntIsEmri.IsEmriDurumKayitNo = enumIsEmirleriDurum.Bekliyor.GetHashCode();

            model.EntIsEmri.Parametre = model.ApnAdi;
            Kaydet(model);
            return Yonlendir(Url.Action("Index", "EntIsEmri", new { id = model.EntIsEmri.SayacKayitNo }), Dil.Basarili);

        }

        [HttpPost]
        public ActionResult YetkiAc(EntIsEmriKaydetModel model)
        {
            var sayac = _entSayacService.DetayGetirById(model.EntIsEmri.SayacKayitNo);
            model.EntIsEmri.IsEmriKayitNo = enumIsEmirleri.YetkiAc.GetHashCode();
            model.EntIsEmri.IsEmriDurumKayitNo = enumIsEmirleriDurum.Bekliyor.GetHashCode();

            model.EntIsEmri.Parametre = "ELM"+sayac.SERINO;
            Kaydet(model);
            return Yonlendir(Url.Action("Index", "EntIsEmri", new { id = model.EntIsEmri.SayacKayitNo }), Dil.Basarili);

        }

        [HttpPost]
        public ActionResult ZamanlanmisGorevSil(EntIsEmriKaydetModel model)
        {
            // var sayac = _entSayacService.DetayGetirById(model.EntIsEmri.SayacKayitNo);
            model.EntIsEmri.IsEmriKayitNo = enumIsEmirleri.ZamanlanmisGorevSilme.GetHashCode();
            model.EntIsEmri.IsEmriDurumKayitNo = enumIsEmirleriDurum.Bekliyor.GetHashCode();

            model.EntIsEmri.Parametre = "ALL_TASK";
            Kaydet(model);
            return Yonlendir(Url.Action("Index", "EntIsEmri", new { id = model.EntIsEmri.SayacKayitNo }), Dil.Basarili);

        }
        #endregion
    }
}

