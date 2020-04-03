using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.CONDILComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.CONDILModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.MvcUI.Filters;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
	public class CONDILController : BaseController
	{
		private readonly ICONDILService _cONDILService;

		public CONDILController(ICONDILService cONDILService)
		{
			_cONDILService = cONDILService;
		}

		
		public ActionResult Index()
		{
			SayfaBaslik($"CONDIL İşlemleri");
			var model= new CONDILIndexModel();
			return View(model);
		}

		[HttpPost]
		public ActionResult DataTablesList(DtParameterModel dtParameterModel, CONDILAra cONDILAra)
		{
			
			cONDILAra.Ara = dtParameterModel.AramaKriteri;
			
			if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
			{ //TODO: Bu bölümü düzenle
				cONDILAra.DIL = dtParameterModel.Search.Value;
			}

	 

			var kayitlar = _cONDILService.Ara(cONDILAra);

			return Json(new DataTableResult()
			{
				data = kayitlar.CONDILDetayList.Select(t => new
				{
					//TODO: Bu bölümü düzenle
					t.KAYITNO,
				t.DIL,

					Islemler =$@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "CONDIL", new {id = t.KAYITNO})}' title='Düzenle'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "CONDIL", new {id = t.KAYITNO})}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "CONDIL", new {id = t.KAYITNO})}' title='Sil'><i class='fa fa-trash'></i></a>"
				}),
				draw = dtParameterModel.Draw,
				recordsTotal = kayitlar.ToplamKayitSayisi,
				recordsFiltered = kayitlar.ToplamKayitSayisi
			}, JsonRequestBehavior.AllowGet);
		}

		
		public ActionResult Ekle()
		{
			SayfaBaslik($"CONDIL Ekle");

			var model = new CONDILKaydetModel
			{
				CONDIL = new CONDIL()
			};
			
	

			return View("Kaydet", model);
		}

		
		public ActionResult Guncelle(int id)
		{
			SayfaBaslik($"CONDIL Güncelle");

			var model = new CONDILKaydetModel
			{
				CONDIL = _cONDILService.GetirById(id)
			};

	
			return View("Kaydet", model);
		}


		
		public ActionResult Detay(int id)
		{
			SayfaBaslik($"CONDIL Detay");

			var model = new CONDILDetayModel
			{
				CONDILDetay = _cONDILService.DetayGetirById(id)
			};

	
			return View("Detay", model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Kaydet(CONDILKaydetModel cONDILKaydetModel)
		{
			if (cONDILKaydetModel.CONDIL.KAYITNO > 0)
			{
				_cONDILService.Guncelle(cONDILKaydetModel.CONDIL.List());
			}
			else
			{
				_cONDILService.Ekle(cONDILKaydetModel.CONDIL.List());
			}

			return Yonlendir(Url.Action("Index"), $"CONDIL kayıdı başarıyla gerçekleştirilmiştir.");
			//return Yonlendir(Url.Action("Detay","CONDIL",new{id=cONDILKaydetModel.CONDIL.Id}), $"CONDIL kayıdı başarıyla gerçekleştirilmiştir.");
		}


		
		public ActionResult Sil(int id)
		{
			SayfaBaslik($"CONDIL Silme İşlem Onayı");
			return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel =  $"/CONDIL/Index/{id}" });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Sil(DeleteViewModel model)
		{
			_cONDILService.Sil(model.Id.List());

			return Yonlendir(Url.Action("Index"), $"CONDIL Başarıyla silindi");
		}


		public ActionResult AjaxAra(string key, CONDILAra cONDILAra=null, int limit = 10, int baslangic = 0)
		{
			
			 if (cONDILAra == null)
			{
				cONDILAra = new CONDILAra();
			}

            cONDILAra.Ara = new Ara
            {
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {
                    new Siralama
                    {
                        KolonAdi = LinqExtensions.GetPropertyName((CONDIL t) => t.KAYITNO),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }
                }
            };


			//TODO: Bu bölümü düzenle
			cONDILAra.DIL = key;

			var cONDILList =  _cONDILService.Getir(cONDILAra);


			var data = cONDILList.Select(cONDIL => new AutoCompleteData
			{
			//TODO: Bu bölümü düzenle
				id = cONDIL.KAYITNO.ToString(),
				text = cONDIL.DIL.ToString(),
				description = cONDIL.KAYITNO.ToString(),
			}).ToList();
			return Json(data, JsonRequestBehavior.AllowGet);
		}


		public ActionResult AjaxTekDeger(int id)
		{
			var cONDIL = _cONDILService.GetirById(id);


			var data = new AutoCompleteData
			{//TODO: Bu bölümü düzenle
				id = cONDIL.KAYITNO.ToString(),
				text = cONDIL.DIL.ToString(),
				description = cONDIL.KAYITNO.ToString(),
			};

			return Json(data, JsonRequestBehavior.AllowGet);
		}

		public ActionResult AjaxCokDeger(string id)
		{
			var cONDILList = _cONDILService.Getir(new CONDILAra() { KAYITNOlar = id });


			var data = cONDILList.Select(cONDIL => new AutoCompleteData
			{ //TODO: Bu bölümü düzenle
				id = cONDIL.KAYITNO.ToString(),
				text = cONDIL.DIL.ToString(),
				description = cONDIL.KAYITNO.ToString()
			});

			return Json(data, JsonRequestBehavior.AllowGet);
		}

	}
}

