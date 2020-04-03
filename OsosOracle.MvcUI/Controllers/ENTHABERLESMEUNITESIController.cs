using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.ENTHABERLESMEUNITESIComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.MvcUI.Filters;
using OsosOracle.MvcUI.Models.ENTHABERLESMEUNITESIModels;
using OsosOracle.MvcUI.Resources;


namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class ENTHABERLESMEUNITESIController : BaseController
	{
		private readonly IENTHABERLESMEUNITESIService _eNTHABERLESMEUNITESIService;

		public ENTHABERLESMEUNITESIController(IENTHABERLESMEUNITESIService eNTHABERLESMEUNITESIService)
		{
			_eNTHABERLESMEUNITESIService = eNTHABERLESMEUNITESIService;
		}

		
		public ActionResult Index()
		{
			SayfaBaslik($"Modem İşlemleri");
			var model= new ENTHABERLESMEUNITESIIndexModel();
			return View(model);
		}

		[HttpPost]
		public ActionResult DataTablesList(DtParameterModel dtParameterModel, ENTHABERLESMEUNITESIAra eNTHABERLESMEUNITESIAra)
		{
			
			eNTHABERLESMEUNITESIAra.Ara = dtParameterModel.AramaKriteri;
			
			if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
			{ //TODO: Bu bölümü düzenle
				eNTHABERLESMEUNITESIAra.SERINO = dtParameterModel.Search.Value;
			}

	 

			var kayitlar = _eNTHABERLESMEUNITESIService.Ara(eNTHABERLESMEUNITESIAra);

			return Json(new DataTableResult()
			{
				data = kayitlar.ENTHABERLESMEUNITESIDetayList.Select(t => new
				{
				t.KAYITNO,
				t.SERINO,
				t.SIMTELNO,
				t.IP,
				t.ACIKLAMA,
				t.DURUM,
				t.VERSIYON,
				t.MARKA,
				t.MODEL,
				t.Kurum,

					Islemler =$@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "ENTHABERLESMEUNITESI", new {id = t.KAYITNO})}' title='{Dil.Duzenle}'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "ENTHABERLESMEUNITESI", new {id = t.KAYITNO })}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "ENTHABERLESMEUNITESI", new {id = t.KAYITNO })}' title='{Dil.Sil}'><i class='fa fa-trash'></i></a>"
				}),
				draw = dtParameterModel.Draw,
				recordsTotal = kayitlar.ToplamKayitSayisi,
				recordsFiltered = kayitlar.ToplamKayitSayisi
			}, JsonRequestBehavior.AllowGet);
		}


		public ActionResult Ekle()
		{
			SayfaBaslik($"Modem Ekle");

			var model = new ENTHABERLESMEUNITESIKaydetModel
			{
				ENTHABERLESMEUNITESI = new ENTHABERLESMEUNITESI()
			};
			
	

			return View("Kaydet", model);
		}


		public ActionResult Guncelle(int id)
		{
			SayfaBaslik($"Modem Güncelle");

			var model = new ENTHABERLESMEUNITESIKaydetModel
			{
				ENTHABERLESMEUNITESI = _eNTHABERLESMEUNITESIService.GetirById(id)
			};

	
			return View("Kaydet", model);
		}



		public ActionResult Detay(int id)
		{
			SayfaBaslik($"Modem Detay");

			var model = new ENTHABERLESMEUNITESIDetayModel
			{
				ENTHABERLESMEUNITESIDetay = _eNTHABERLESMEUNITESIService.DetayGetirById(id)
			};

	
			return View("Detay", model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Kaydet(ENTHABERLESMEUNITESIKaydetModel eNTHABERLESMEUNITESIKaydetModel)
		{
            eNTHABERLESMEUNITESIKaydetModel.ENTHABERLESMEUNITESI.DURUM = 1;
			if (eNTHABERLESMEUNITESIKaydetModel.ENTHABERLESMEUNITESI.KAYITNO > 0)
			{
                eNTHABERLESMEUNITESIKaydetModel.ENTHABERLESMEUNITESI.GUNCELLEYEN = AktifKullanici.KayitNo;
				_eNTHABERLESMEUNITESIService.Guncelle(eNTHABERLESMEUNITESIKaydetModel.ENTHABERLESMEUNITESI.List());
			}
			else
            {
                eNTHABERLESMEUNITESIKaydetModel.ENTHABERLESMEUNITESI.OLUSTURAN = AktifKullanici.KayitNo;
                _eNTHABERLESMEUNITESIService.Ekle(eNTHABERLESMEUNITESIKaydetModel.ENTHABERLESMEUNITESI.List());
			}

			return Yonlendir(Url.Action("Index"), $"Modem kayıdı başarıyla gerçekleştirilmiştir.");
			//return Yonlendir(Url.Action("Detay","ENTHABERLESMEUNITESI",new{id=eNTHABERLESMEUNITESIKaydetModel.ENTHABERLESMEUNITESI.Id}), $"ENTHABERLESMEUNITESI kayıdı başarıyla gerçekleştirilmiştir.");
		}



		public ActionResult Sil(int id)
		{
			SayfaBaslik($"Modem Silme İşlem Onayı");
			return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel =  $"/ENTHABERLESMEUNITESI/Index/{id}" });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Sil(DeleteViewModel model)
		{
			_eNTHABERLESMEUNITESIService.Sil(model.Id.List());

			return Yonlendir(Url.Action("Index"), $"Modem Başarıyla silindi");
		}


		public ActionResult AjaxAra(string key, ENTHABERLESMEUNITESIAra eNTHABERLESMEUNITESIAra=null, int limit = 10, int baslangic = 0)
		{
			
			 if (eNTHABERLESMEUNITESIAra == null)
			{
				eNTHABERLESMEUNITESIAra = new ENTHABERLESMEUNITESIAra();
			}

            eNTHABERLESMEUNITESIAra.Ara = new Ara
            {
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {
                    new Siralama
                    {
                        KolonAdi = LinqExtensions.GetPropertyName((ENTHABERLESMEUNITESI t) => t.KAYITNO),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }
                }
            };


			//TODO: Bu bölümü düzenle
			eNTHABERLESMEUNITESIAra.SERINO = key;

			var eNTHABERLESMEUNITESIList =  _eNTHABERLESMEUNITESIService.Getir(eNTHABERLESMEUNITESIAra);


			var data = eNTHABERLESMEUNITESIList.Select(eNTHABERLESMEUNITESI => new AutoCompleteData
			{
			//TODO: Bu bölümü düzenle
				id = eNTHABERLESMEUNITESI.KAYITNO.ToString(),
				text = eNTHABERLESMEUNITESI.SERINO.ToString(),
				description = eNTHABERLESMEUNITESI.KAYITNO.ToString(),
			}).ToList();
			return Json(data, JsonRequestBehavior.AllowGet);
		}


		public ActionResult AjaxTekDeger(int id)
		{
			var eNTHABERLESMEUNITESI = _eNTHABERLESMEUNITESIService.GetirById(id);


			var data = new AutoCompleteData
			{//TODO: Bu bölümü düzenle
				id = eNTHABERLESMEUNITESI.KAYITNO.ToString(),
				text = eNTHABERLESMEUNITESI.SERINO.ToString(),
				description = eNTHABERLESMEUNITESI.KAYITNO.ToString(),
			};

			return Json(data, JsonRequestBehavior.AllowGet);
		}

		public ActionResult AjaxCokDeger(string id)
		{
			var eNTHABERLESMEUNITESIList = _eNTHABERLESMEUNITESIService.Getir(new ENTHABERLESMEUNITESIAra() { KAYITNOlar = id });


			var data = eNTHABERLESMEUNITESIList.Select(eNTHABERLESMEUNITESI => new AutoCompleteData
			{ //TODO: Bu bölümü düzenle
				id = eNTHABERLESMEUNITESI.KAYITNO.ToString(),
				text = eNTHABERLESMEUNITESI.SERINO.ToString(),
				description = eNTHABERLESMEUNITESI.KAYITNO.ToString()
			});

			return Json(data, JsonRequestBehavior.AllowGet);
		}

	}
}

