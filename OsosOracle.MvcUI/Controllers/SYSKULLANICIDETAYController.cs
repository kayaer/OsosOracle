using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.SYSKULLANICIDETAYComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.SYSKULLANICIDETAYModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.MvcUI.Filters;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
	public class SYSKULLANICIDETAYController : BaseController
	{
		private readonly ISYSKULLANICIDETAYService _sYSKULLANICIDETAYService;

		public SYSKULLANICIDETAYController(ISYSKULLANICIDETAYService sYSKULLANICIDETAYService)
		{
			_sYSKULLANICIDETAYService = sYSKULLANICIDETAYService;
		}

		
		public ActionResult Index()
		{
			SayfaBaslik($"Kullanıcı Detay İşlemleri");
			var model= new SYSKULLANICIDETAYIndexModel();
			return View(model);
		}

		[HttpPost]
		public ActionResult DataTablesList(DtParameterModel dtParameterModel, SYSKULLANICIDETAYAra sYSKULLANICIDETAYAra)
		{
			
			sYSKULLANICIDETAYAra.Ara = dtParameterModel.AramaKriteri;
			
			//if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
			//{ //TODO: Bu bölümü düzenle
			//	sYSKULLANICIDETAYAra. = dtParameterModel.Search.Value;
			//}

	 

			var kayitlar = _sYSKULLANICIDETAYService.Ara(sYSKULLANICIDETAYAra);

			return Json(new DataTableResult()
			{
				data = kayitlar.SYSKULLANICIDETAYDetayList.Select(t => new
				{
					//TODO: Bu bölümü düzenle
					t.KAYITNO,
				t.EPOSTA,
				t.GSM,
				t.KULLANICIKAYITNO,

					Islemler =$@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "SYSKULLANICIDETAY", new {id = t.KAYITNO})}' title='Düzenle'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "SYSKULLANICIDETAY", new {id = t.KAYITNO})}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "SYSKULLANICIDETAY", new {id = t.KAYITNO})}' title='Sil'><i class='fa fa-trash'></i></a>"
				}),
				draw = dtParameterModel.Draw,
				recordsTotal = kayitlar.ToplamKayitSayisi,
				recordsFiltered = kayitlar.ToplamKayitSayisi
			}, JsonRequestBehavior.AllowGet);
		}

		
		public ActionResult Ekle()
		{
			SayfaBaslik($"Kullanıcı Detay Ekle");

			var model = new SYSKULLANICIDETAYKaydetModel
			{
				SYSKULLANICIDETAY = new SYSKULLANICIDETAY()
			};
			
	

			return View("Kaydet", model);
		}

		
		public ActionResult Guncelle(int id)
		{
			SayfaBaslik($"Kullanıcı Detay Güncelle");

			var model = new SYSKULLANICIDETAYKaydetModel
			{
				SYSKULLANICIDETAY = _sYSKULLANICIDETAYService.GetirById(id)
			};

	
			return View("Kaydet", model);
		}


		
		public ActionResult Detay(int id)
		{
			SayfaBaslik($"Kullanıcı Detay Detay");

			var model = new SYSKULLANICIDETAYDetayModel
			{
				SYSKULLANICIDETAYDetay = _sYSKULLANICIDETAYService.DetayGetirById(id)
			};

	
			return View("Detay", model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Kaydet(SYSKULLANICIDETAYKaydetModel sYSKULLANICIDETAYKaydetModel)
		{
			if (sYSKULLANICIDETAYKaydetModel.SYSKULLANICIDETAY.KAYITNO > 0)
			{
				_sYSKULLANICIDETAYService.Guncelle(sYSKULLANICIDETAYKaydetModel.SYSKULLANICIDETAY.List());
			}
			else
			{
				_sYSKULLANICIDETAYService.Ekle(sYSKULLANICIDETAYKaydetModel.SYSKULLANICIDETAY.List());
			}

			return Yonlendir(Url.Action("Index"), $"SYSKULLANICIDETAY kayıdı başarıyla gerçekleştirilmiştir.");
			//return Yonlendir(Url.Action("Detay","SYSKULLANICIDETAY",new{id=sYSKULLANICIDETAYKaydetModel.SYSKULLANICIDETAY.Id}), $"SYSKULLANICIDETAY kayıdı başarıyla gerçekleştirilmiştir.");
		}


		
		public ActionResult Sil(int id)
		{
			SayfaBaslik($"Kullanıcı Detay Silme İşlem Onayı");
			return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel =  $"/SYSKULLANICIDETAY/Index/{id}" });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Sil(DeleteViewModel model)
		{
			_sYSKULLANICIDETAYService.Sil(model.Id.List());

			return Yonlendir(Url.Action("Index"), $"Kullanıcı Detay Başarıyla silindi");
		}


		//public ActionResult AjaxAra(string key, SYSKULLANICIDETAYAra sYSKULLANICIDETAYAra=null, int limit = 10, int baslangic = 0)
		//{
			
		//	 if (sYSKULLANICIDETAYAra == null)
		//	{
		//		sYSKULLANICIDETAYAra = new SYSKULLANICIDETAYAra();
		//	}

  //          sYSKULLANICIDETAYAra.Ara = new Ara
  //          {
  //              Baslangic = baslangic,
  //              Uzunluk = limit,
  //              Siralama = new List<Siralama>
  //              {
  //                  new Siralama
  //                  {
  //                      KolonAdi = LinqExtensions.GetPropertyName((SYSKULLANICIDETAY t) => t.KAYITNO),
  //                      SiralamaTipi = EnumSiralamaTuru.Asc
  //                  }
  //              }
  //          };


		//	//TODO: Bu bölümü düzenle
		//	sYSKULLANICIDETAYAra.Adi = key;

		//	var sYSKULLANICIDETAYList =  _sYSKULLANICIDETAYService.Getir(sYSKULLANICIDETAYAra);


		//	var data = sYSKULLANICIDETAYList.Select(sYSKULLANICIDETAY => new AutoCompleteData
		//	{
		//	//TODO: Bu bölümü düzenle
		//		id = sYSKULLANICIDETAY.Id.ToString(),
		//		text = sYSKULLANICIDETAY.Id.ToString(),
		//		description = sYSKULLANICIDETAY.Id.ToString(),
		//	}).ToList();
		//	return Json(data, JsonRequestBehavior.AllowGet);
		//}


		//public ActionResult AjaxTekDeger(int id)
		//{
		//	var sYSKULLANICIDETAY = _sYSKULLANICIDETAYService.GetirById(id);


		//	var data = new AutoCompleteData
		//	{//TODO: Bu bölümü düzenle
		//		id = sYSKULLANICIDETAY.Id.ToString(),
		//		text = sYSKULLANICIDETAY.Id.ToString(),
		//		description = sYSKULLANICIDETAY.Id.ToString(),
		//	};

		//	return Json(data, JsonRequestBehavior.AllowGet);
		//}

		//public ActionResult AjaxCokDeger(string id)
		//{
		//	var sYSKULLANICIDETAYList = _sYSKULLANICIDETAYService.Getir(new SYSKULLANICIDETAYAra() { KAYITNOlar = id });


		//	var data = sYSKULLANICIDETAYList.Select(sYSKULLANICIDETAY => new AutoCompleteData
		//	{ //TODO: Bu bölümü düzenle
		//		id = sYSKULLANICIDETAY.Id.ToString(),
		//		text = sYSKULLANICIDETAY.Id.ToString(),
		//		description = sYSKULLANICIDETAY.Id.ToString()
		//	});

		//	return Json(data, JsonRequestBehavior.AllowGet);
		//}

	}
}

