

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.CSTHUMARKAComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.CSTHUMARKAModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.MvcUI.Filters;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
	public class CSTHUMARKAController : BaseController
	{
		private readonly ICSTHUMARKAService _cSTHUMARKAService;

		public CSTHUMARKAController(ICSTHUMARKAService cSTHUMARKAService)
		{
			_cSTHUMARKAService = cSTHUMARKAService;
		}

		
		public ActionResult Index()
		{
			SayfaBaslik($"Modem Marka İşlemleri");
			var model= new CSTHUMARKAIndexModel();
			return View(model);
		}

		[HttpPost]
		public ActionResult DataTablesList(DtParameterModel dtParameterModel, CSTHUMARKAAra cSTHUMARKAAra)
		{
			
			cSTHUMARKAAra.Ara = dtParameterModel.AramaKriteri;
			
			if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
			{ //TODO: Bu bölümü düzenle
				cSTHUMARKAAra.AD = dtParameterModel.Search.Value;
			}

	 

			var kayitlar = _cSTHUMARKAService.Ara(cSTHUMARKAAra);

			return Json(new DataTableResult()
			{
				data = kayitlar.CSTHUMARKADetayList.Select(t => new
				{
					//TODO: Bu bölümü düzenle
					t.KAYITNO,
				t.AD,
				t.ACIKLAMA,
				t.DURUM,
				t.VERSIYON,

					Islemler =$@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "CSTHUMARKA", new {id = t.KAYITNO})}' title='Düzenle'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "CSTHUMARKA", new {id = t.KAYITNO})}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "CSTHUMARKA", new {id = t.KAYITNO})}' title='Sil'><i class='fa fa-trash'></i></a>"
				}),
				draw = dtParameterModel.Draw,
				recordsTotal = kayitlar.ToplamKayitSayisi,
				recordsFiltered = kayitlar.ToplamKayitSayisi
			}, JsonRequestBehavior.AllowGet);
		}


		public ActionResult Ekle()
		{
			SayfaBaslik($"Modem marka Ekle");

			var model = new CSTHUMARKAKaydetModel
			{
				CSTHUMARKA = new CSTHUMARKA()
			};
			
	

			return View("Kaydet", model);
		}


		public ActionResult Guncelle(int id)
		{
			SayfaBaslik($"Modem marka Güncelle");

			var model = new CSTHUMARKAKaydetModel
			{
				CSTHUMARKA = _cSTHUMARKAService.GetirById(id)
			};

	
			return View("Kaydet", model);
		}



		public ActionResult Detay(int id)
		{
			SayfaBaslik($"Modem marka Detay");

			var model = new CSTHUMARKADetayModel
			{
				CSTHUMARKADetay = _cSTHUMARKAService.DetayGetirById(id)
			};

	
			return View("Detay", model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Kaydet(CSTHUMARKAKaydetModel cSTHUMARKAKaydetModel)
		{
            cSTHUMARKAKaydetModel.CSTHUMARKA.DURUM = 1;
			if (cSTHUMARKAKaydetModel.CSTHUMARKA.KAYITNO > 0)
			{
                cSTHUMARKAKaydetModel.CSTHUMARKA.GUNCELLEYEN = AktifKullanici.KayitNo;
				_cSTHUMARKAService.Guncelle(cSTHUMARKAKaydetModel.CSTHUMARKA.List());
			}
			else
            {
                cSTHUMARKAKaydetModel.CSTHUMARKA.OLUSTURAN = AktifKullanici.KayitNo;
                _cSTHUMARKAService.Ekle(cSTHUMARKAKaydetModel.CSTHUMARKA.List());
			}

			return Yonlendir(Url.Action("Index"), $"Modem marka kayıdı başarıyla gerçekleştirilmiştir.");
			//return Yonlendir(Url.Action("Detay","CSTHUMARKA",new{id=cSTHUMARKAKaydetModel.CSTHUMARKA.Id}), $"CSTHUMARKA kayıdı başarıyla gerçekleştirilmiştir.");
		}



		public ActionResult Sil(int id)
		{
			SayfaBaslik($"Modem marka Silme İşlem Onayı");
			return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel =  $"/CSTHUMARKA/Index/{id}" });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Sil(DeleteViewModel model)
		{
			_cSTHUMARKAService.Sil(model.Id.List());

			return Yonlendir(Url.Action("Index"), $"Modem marka Başarıyla silindi");
		}


		public ActionResult AjaxAra(string key, CSTHUMARKAAra cSTHUMARKAAra=null, int limit = 10, int baslangic = 0)
		{
			
			 if (cSTHUMARKAAra == null)
			{
				cSTHUMARKAAra = new CSTHUMARKAAra();
			}

            cSTHUMARKAAra.Ara = new Ara
            {
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {
                    new Siralama
                    {
                        KolonAdi = LinqExtensions.GetPropertyName((CSTHUMARKA t) => t.KAYITNO),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }
                }
            };


			//TODO: Bu bölümü düzenle
			cSTHUMARKAAra.AD = key;

			var cSTHUMARKAList =  _cSTHUMARKAService.Getir(cSTHUMARKAAra);


			var data = cSTHUMARKAList.Select(cSTHUMARKA => new AutoCompleteData
			{
			//TODO: Bu bölümü düzenle
				id = cSTHUMARKA.KAYITNO.ToString(),
				text = cSTHUMARKA.AD.ToString(),
				description = cSTHUMARKA.KAYITNO.ToString(),
			}).ToList();
			return Json(data, JsonRequestBehavior.AllowGet);
		}


		public ActionResult AjaxTekDeger(int id)
		{
			var cSTHUMARKA = _cSTHUMARKAService.GetirById(id);


			var data = new AutoCompleteData
			{//TODO: Bu bölümü düzenle
				id = cSTHUMARKA.KAYITNO.ToString(),
				text = cSTHUMARKA.AD.ToString(),
				description = cSTHUMARKA.KAYITNO.ToString(),
			};

			return Json(data, JsonRequestBehavior.AllowGet);
		}

		public ActionResult AjaxCokDeger(string id)
		{
			var cSTHUMARKAList = _cSTHUMARKAService.Getir(new CSTHUMARKAAra() { KAYITNOlar = id });


			var data = cSTHUMARKAList.Select(cSTHUMARKA => new AutoCompleteData
			{ //TODO: Bu bölümü düzenle
				id = cSTHUMARKA.KAYITNO.ToString(),
				text = cSTHUMARKA.AD.ToString(),
				description = cSTHUMARKA.KAYITNO.ToString()
			});

			return Json(data, JsonRequestBehavior.AllowGet);
		}

	}
}

