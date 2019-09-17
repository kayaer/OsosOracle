using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.ENTSAYACSONDURUMSUComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.ENTSAYACSONDURUMSUModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.MvcUI.Filters;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class ENTSAYACSONDURUMSUController : BaseController
    {
        private readonly IENTSAYACSONDURUMSUService _eNTSAYACSONDURUMSUService;

        public ENTSAYACSONDURUMSUController(IENTSAYACSONDURUMSUService eNTSAYACSONDURUMSUService)
        {
            _eNTSAYACSONDURUMSUService = eNTSAYACSONDURUMSUService;
        }


        public ActionResult Index()
        {
            SayfaBaslik($"Sayaç Durum İşlemleri");
            var model = new ENTSAYACSONDURUMSUIndexModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, ENTSAYACSONDURUMSUAra eNTSAYACSONDURUMSUAra)
        {
            if (eNTSAYACSONDURUMSUAra.ArizaCheck)
            {
                eNTSAYACSONDURUMSUAra.ARIZA = "1";
            }
            if (eNTSAYACSONDURUMSUAra.AsiriTuketimCheck)
            {
                eNTSAYACSONDURUMSUAra.ASIRITUKETIM = "1";
            }
            if (eNTSAYACSONDURUMSUAra.Ceza1Check)
            {
                eNTSAYACSONDURUMSUAra.CEZA1 = "1";
            }
            if (eNTSAYACSONDURUMSUAra.Ceza2Check)
            {
                eNTSAYACSONDURUMSUAra.CEZA2 = "1";
            }
            if (eNTSAYACSONDURUMSUAra.Ceza3Check)
            {
                eNTSAYACSONDURUMSUAra.CEZA3 = "1";
            }
            if (eNTSAYACSONDURUMSUAra.Ceza4Check)
            {
                eNTSAYACSONDURUMSUAra.CEZA4 = "1";
            }

            if (eNTSAYACSONDURUMSUAra.MagnetCheck)
            {
                eNTSAYACSONDURUMSUAra.MAGNET = "1";
            }
            if (eNTSAYACSONDURUMSUAra.PulseHataCheck)
            {
                eNTSAYACSONDURUMSUAra.PULSEHATA = "1";
            }
            if (eNTSAYACSONDURUMSUAra.RtcHataCheck)
            {
                eNTSAYACSONDURUMSUAra.RTCHATA = "1";
            }
            if (eNTSAYACSONDURUMSUAra.MaxDebiCheck)
            {
                eNTSAYACSONDURUMSUAra.MAXDEBI = 1;
            }
            if (eNTSAYACSONDURUMSUAra.VanaCheck)
            {
                eNTSAYACSONDURUMSUAra.VANA = "1";
            }
            if (eNTSAYACSONDURUMSUAra.AnaPilBittiCheck)
            {
                eNTSAYACSONDURUMSUAra.ANAPILBITTI = "1";
            }



            eNTSAYACSONDURUMSUAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
              //eNTSAYACSONDURUMSUAra.Adi = dtParameterModel.Search.Value;
            }



            var kayitlar = _eNTSAYACSONDURUMSUService.Ara(eNTSAYACSONDURUMSUAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.ENTSAYACSONDURUMSUDetayList.Select(t => new
                {
                    //TODO: Bu bölümü düzenle
                    t.KAYITNO,
                    t.SayacSeriNo,
                    t.CAP,
                    t.BAGLANTISAYISI,
                    t.LIMIT1,
                    t.LIMIT2,
                    t.LIMIT3,
                    t.LIMIT4,
                    t.KRITIKKREDI,
                    t.FIYAT1,
                    t.FIYAT2,
                    t.FIYAT3,
                    t.FIYAT4,
                    t.FIYAT5,
                    t.ARIZAA,
                    t.ARIZAK,
                    t.ARIZAP,
                    t.ARIZAPIL,
                    BORC=t.BORC=="1"?"Evet":"Hayır",
                    t.ITERASYON,
                    t.PILAKIM,
                    t.PILVOLTAJ,
                    t.SONYUKLENENKREDITARIH,
                    t.ABONENO,
                    t.ABONETIP,
                    t.ILKPULSETARIH,
                    t.SONPULSETARIH,
                    t.BORCTARIH,
                    MAXDEBI=t.MAXDEBI==1?"Evet":"Hayır",
                    MAXDEBISINIR=t.MAXDEBISINIR==1?"Evet":"Hayır",
                    t.DONEMGUN,
                    t.DONEM,
                    t.VANAACMATARIH,
                    t.VANAKAPAMATARIH,
                    t.SICAKLIK,
                    t.MINSICAKLIK,
                    t.MAXSICAKLIK,
                    YANGINMODU=t.YANGINMODU==1?"Evet":"Hayır",
                    ASIRITUKETIM=t.ASIRITUKETIM=="1"?"Evet":"Hayır",
                    ARIZA=t.ARIZA=="1"?"Evet":"Hayır",
                    t.SONBILGILENDIRMEZAMANI,
                    t.PARAMETRE,
                    t.ACIKLAMA,
                    t.DURUM,
                    KREDIBITTI=t.KREDIBITTI=="1"?"Evet":"Hayır",
                    KREDIAZ=t.KREDIAZ=="1"?"Evet":"Hayır",
                    ANAPILBITTI=t.ANAPILBITTI=="1"?"Evet":"Hayır",
                    t.SONBAGLANTIZAMAN,
                    SONOKUMATARIH = t.SONOKUMATARIH.ToString(),
                    PULSEHATA=t.PULSEHATA=="1"?"Evet":"Hayır",
                    RTCHATA=t.RTCHATA=="1"?"Evet":"Hayır",
                    VANA = t.VANA == "1" ? "Açık" : "Kapalı",
                    CEZA1 = t.CEZA1 == "1" ? "Evet" : "Hayır",
                    CEZA2 = t.CEZA2 == "1" ? "Evet" : "Hayır",
                    CEZA3 = t.CEZA3 == "1" ? "Evet" : "Hayır",
                    CEZA4 = t.CEZA4 == "1" ? "Evet" : "Hayır",
                    MAGNET= t.MAGNET=="1"?"Evet":"Hayır",
                    ANAPILZAYIF= t.ANAPILZAYIF=="1"?"Evet":"Hayır",

                    Islemler = $@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "ENTSAYACSONDURUMSU", new { id = t.KAYITNO })}' title='Düzenle'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "ENTSAYACSONDURUMSU", new { id = t.KAYITNO })}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "ENTSAYACSONDURUMSU", new { id = t.KAYITNO })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Ekle()
        {
            SayfaBaslik($"ENTSAYACSONDURUMSU Ekle");

            var model = new ENTSAYACSONDURUMSUKaydetModel
            {
                ENTSAYACSONDURUMSU = new ENTSAYACSONDURUMSU()
            };



            return View("Kaydet", model);
        }


        public ActionResult Guncelle(int id)
        {
            SayfaBaslik($"ENTSAYACSONDURUMSU Güncelle");

            var model = new ENTSAYACSONDURUMSUKaydetModel
            {
                ENTSAYACSONDURUMSU = _eNTSAYACSONDURUMSUService.GetirById(id)
            };


            return View("Kaydet", model);
        }



        public ActionResult Detay(int id)
        {
            SayfaBaslik($"ENTSAYACSONDURUMSU Detay");

            var model = new ENTSAYACSONDURUMSUDetayModel
            {
                ENTSAYACSONDURUMSUDetay = _eNTSAYACSONDURUMSUService.DetayGetirById(id)
            };


            return View("Detay", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(ENTSAYACSONDURUMSUKaydetModel eNTSAYACSONDURUMSUKaydetModel)
        {
            if (eNTSAYACSONDURUMSUKaydetModel.ENTSAYACSONDURUMSU.KAYITNO > 0)
            {
                _eNTSAYACSONDURUMSUService.Guncelle(eNTSAYACSONDURUMSUKaydetModel.ENTSAYACSONDURUMSU.List());
            }
            else
            {
                _eNTSAYACSONDURUMSUService.Ekle(eNTSAYACSONDURUMSUKaydetModel.ENTSAYACSONDURUMSU.List());
            }

            return Yonlendir(Url.Action("Index"), $"ENTSAYACSONDURUMSU kayıdı başarıyla gerçekleştirilmiştir.");
            //return Yonlendir(Url.Action("Detay","ENTSAYACSONDURUMSU",new{id=eNTSAYACSONDURUMSUKaydetModel.ENTSAYACSONDURUMSU.Id}), $"ENTSAYACSONDURUMSU kayıdı başarıyla gerçekleştirilmiştir.");
        }



        public ActionResult Sil(int id)
        {
            SayfaBaslik($"ENTSAYACSONDURUMSU Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel = $"/ENTSAYACSONDURUMSU/Index/{id}" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
            _eNTSAYACSONDURUMSUService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"), $"ENTSAYACSONDURUMSU Başarıyla silindi");
        }


        //public ActionResult AjaxAra(string key, ENTSAYACSONDURUMSUAra eNTSAYACSONDURUMSUAra=null, int limit = 10, int baslangic = 0)
        //{

        //	 if (eNTSAYACSONDURUMSUAra == null)
        //	{
        //		eNTSAYACSONDURUMSUAra = new ENTSAYACSONDURUMSUAra();
        //	}

        //          eNTSAYACSONDURUMSUAra.Ara = new Ara
        //          {
        //              Baslangic = baslangic,
        //              Uzunluk = limit,
        //              Siralama = new List<Siralama>
        //              {
        //                  new Siralama
        //                  {
        //                      KolonAdi = LinqExtensions.GetPropertyName((ENTSAYACSONDURUMSU t) => t.KAYITNO),
        //                      SiralamaTipi = EnumSiralamaTuru.Asc
        //                  }
        //              }
        //          };


        //	//TODO: Bu bölümü düzenle
        //	eNTSAYACSONDURUMSUAra.Adi = key;

        //	var eNTSAYACSONDURUMSUList =  _eNTSAYACSONDURUMSUService.Getir(eNTSAYACSONDURUMSUAra);


        //	var data = eNTSAYACSONDURUMSUList.Select(eNTSAYACSONDURUMSU => new AutoCompleteData
        //	{
        //	//TODO: Bu bölümü düzenle
        //		id = eNTSAYACSONDURUMSU.Id.ToString(),
        //		text = eNTSAYACSONDURUMSU.Id.ToString(),
        //		description = eNTSAYACSONDURUMSU.Id.ToString(),
        //	}).ToList();
        //	return Json(data, JsonRequestBehavior.AllowGet);
        //}


        //public ActionResult AjaxTekDeger(int id)
        //{
        //	var eNTSAYACSONDURUMSU = _eNTSAYACSONDURUMSUService.GetirById(id);


        //	var data = new AutoCompleteData
        //	{//TODO: Bu bölümü düzenle
        //		id = eNTSAYACSONDURUMSU.Id.ToString(),
        //		text = eNTSAYACSONDURUMSU.Id.ToString(),
        //		description = eNTSAYACSONDURUMSU.Id.ToString(),
        //	};

        //	return Json(data, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult AjaxCokDeger(string id)
        //{
        //	var eNTSAYACSONDURUMSUList = _eNTSAYACSONDURUMSUService.Getir(new ENTSAYACSONDURUMSUAra() { KAYITNOlar = id });


        //	var data = eNTSAYACSONDURUMSUList.Select(eNTSAYACSONDURUMSU => new AutoCompleteData
        //	{ //TODO: Bu bölümü düzenle
        //		id = eNTSAYACSONDURUMSU.Id.ToString(),
        //		text = eNTSAYACSONDURUMSU.Id.ToString(),
        //		description = eNTSAYACSONDURUMSU.Id.ToString()
        //	});

        //	return Json(data, JsonRequestBehavior.AllowGet);
        //}

    }
}

