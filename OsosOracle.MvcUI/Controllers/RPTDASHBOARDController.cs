using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.RPTDASHBOARDComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.RPTDASHBOARDModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.MvcUI.Filters;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
	public class RPTDASHBOARDController : BaseController
	{
		private readonly IRPTDASHBOARDService _rPTDASHBOARDService;

		public RPTDASHBOARDController(IRPTDASHBOARDService rPTDASHBOARDService)
		{
			_rPTDASHBOARDService = rPTDASHBOARDService;
		}

		
		public ActionResult Index()
		{
			SayfaBaslik($"RPTDASHBOARD İşlemleri");
			var model= new RPTDASHBOARDIndexModel();
			return View(model);
		}

		[HttpPost]
		public ActionResult DataTablesList(DtParameterModel dtParameterModel, RPTDASHBOARDAra rPTDASHBOARDAra)
		{
			
			rPTDASHBOARDAra.Ara = dtParameterModel.AramaKriteri;
			
			if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
			{ //TODO: Bu bölümü düzenle
				//rPTDASHBOARDAra.Adi = dtParameterModel.Search.Value;
			}

	 

			var kayitlar = _rPTDASHBOARDService.Ara(rPTDASHBOARDAra);

			return Json(new DataTableResult()
			{
				data = kayitlar.RPTDASHBOARDDetayList.Select(t => new
				{
					//TODO: Bu bölümü düzenle
					t.KAYITNO,
				t.TARIH,
				t.ADET,
				t.KURUMKAYITNO,

					Islemler =$@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "RPTDASHBOARD", new {id = t.KAYITNO})}' title='Düzenle'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "RPTDASHBOARD", new {id = t.KAYITNO})}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "RPTDASHBOARD", new {id = t.KAYITNO})}' title='Sil'><i class='fa fa-trash'></i></a>"
				}),
				draw = dtParameterModel.Draw,
				recordsTotal = kayitlar.ToplamKayitSayisi,
				recordsFiltered = kayitlar.ToplamKayitSayisi
			}, JsonRequestBehavior.AllowGet);
		}

		
		public ActionResult Ekle()
		{
			SayfaBaslik($"RPTDASHBOARD Ekle");

			var model = new RPTDASHBOARDKaydetModel
			{
				RPTDASHBOARD = new RPTDASHBOARD()
			};
			
	

			return View("Kaydet", model);
		}

		
		public ActionResult Guncelle(int id)
		{
			SayfaBaslik($"RPTDASHBOARD Güncelle");

			var model = new RPTDASHBOARDKaydetModel
			{
				RPTDASHBOARD = _rPTDASHBOARDService.GetirById(id)
			};

	
			return View("Kaydet", model);
		}


		
		public ActionResult Detay(int id)
		{
			SayfaBaslik($"RPTDASHBOARD Detay");

			var model = new RPTDASHBOARDDetayModel
			{
				RPTDASHBOARDDetay = _rPTDASHBOARDService.DetayGetirById(id)
			};

	
			return View("Detay", model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Kaydet(RPTDASHBOARDKaydetModel rPTDASHBOARDKaydetModel)
		{
			if (rPTDASHBOARDKaydetModel.RPTDASHBOARD.KAYITNO > 0)
			{
				_rPTDASHBOARDService.Guncelle(rPTDASHBOARDKaydetModel.RPTDASHBOARD.List());
			}
			else
			{
				_rPTDASHBOARDService.Ekle(rPTDASHBOARDKaydetModel.RPTDASHBOARD.List());
			}

			return Yonlendir(Url.Action("Index"), $"RPTDASHBOARD kayıdı başarıyla gerçekleştirilmiştir.");
			//return Yonlendir(Url.Action("Detay","RPTDASHBOARD",new{id=rPTDASHBOARDKaydetModel.RPTDASHBOARD.Id}), $"RPTDASHBOARD kayıdı başarıyla gerçekleştirilmiştir.");
		}


		
		public ActionResult Sil(int id)
		{
			SayfaBaslik($"RPTDASHBOARD Silme İşlem Onayı");
			return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel =  $"/RPTDASHBOARD/Index/{id}" });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Sil(DeleteViewModel model)
		{
			_rPTDASHBOARDService.Sil(model.Id.List());

			return Yonlendir(Url.Action("Index"), $"RPTDASHBOARD Başarıyla silindi");
		}


		//public ActionResult AjaxAra(string key, RPTDASHBOARDAra rPTDASHBOARDAra=null, int limit = 10, int baslangic = 0)
		//{
			
		//	 if (rPTDASHBOARDAra == null)
		//	{
		//		rPTDASHBOARDAra = new RPTDASHBOARDAra();
		//	}

  //          rPTDASHBOARDAra.Ara = new Ara
  //          {
  //              Baslangic = baslangic,
  //              Uzunluk = limit,
  //              Siralama = new List<Siralama>
  //              {
  //                  new Siralama
  //                  {
  //                      KolonAdi = LinqExtensions.GetPropertyName((RPTDASHBOARD t) => t.KAYITNO),
  //                      SiralamaTipi = EnumSiralamaTuru.Asc
  //                  }
  //              }
  //          };


		//	//TODO: Bu bölümü düzenle
		//	//rPTDASHBOARDAra.Adi = key;

		//	var rPTDASHBOARDList =  _rPTDASHBOARDService.Getir(rPTDASHBOARDAra);


		//	var data = rPTDASHBOARDList.Select(rPTDASHBOARD => new AutoCompleteData
		//	{
		//	//TODO: Bu bölümü düzenle
		//		id = rPTDASHBOARD.Id.ToString(),
		//		text = rPTDASHBOARD.Id.ToString(),
		//		description = rPTDASHBOARD.Id.ToString(),
		//	}).ToList();
		//	return Json(data, JsonRequestBehavior.AllowGet);
		//}


		//public ActionResult AjaxTekDeger(int id)
		//{
		//	var rPTDASHBOARD = _rPTDASHBOARDService.GetirById(id);


		//	var data = new AutoCompleteData
		//	{//TODO: Bu bölümü düzenle
		//		id = rPTDASHBOARD.Id.ToString(),
		//		text = rPTDASHBOARD.Id.ToString(),
		//		description = rPTDASHBOARD.Id.ToString(),
		//	};

		//	return Json(data, JsonRequestBehavior.AllowGet);
		//}

		//public ActionResult AjaxCokDeger(string id)
		//{
		//	var rPTDASHBOARDList = _rPTDASHBOARDService.Getir(new RPTDASHBOARDAra() { KAYITNOlar = id });


		//	var data = rPTDASHBOARDList.Select(rPTDASHBOARD => new AutoCompleteData
		//	{ //TODO: Bu bölümü düzenle
		//		id = rPTDASHBOARD.Id.ToString(),
		//		text = rPTDASHBOARD.Id.ToString(),
		//		description = rPTDASHBOARD.Id.ToString()
		//	});

		//	return Json(data, JsonRequestBehavior.AllowGet);
		//}

	}
}

