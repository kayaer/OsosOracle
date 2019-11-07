

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.SYSKULLANICIComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.SYSKULLANICIModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.MvcUI.Filters;
using OsosOracle.Entities.ComplexType.SYSKULLANICIDETAYComplexTypes;
using OsosOracle.MvcUI.Resources;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
	public class SYSKULLANICIController : BaseController
	{
		private readonly ISYSKULLANICIService _sYSKULLANICIService;
        private readonly ISYSKULLANICIDETAYService _sysKullaniciDetayService;
        private readonly ISYSROLKULLANICIService _sysRolKullaniciService;

		public SYSKULLANICIController(ISYSKULLANICIService sYSKULLANICIService,ISYSKULLANICIDETAYService sysKullaniciDetayService, ISYSROLKULLANICIService sysRolKullaniciService)
		{
			_sYSKULLANICIService = sYSKULLANICIService;
            _sysKullaniciDetayService = sysKullaniciDetayService;
            _sysRolKullaniciService = sysRolKullaniciService;
		}

		
		public ActionResult Index()
		{
			SayfaBaslik(Dil.KullaniciIslemleri);
			var model= new SYSKULLANICIIndexModel();
			return View(model);
		}

		[HttpPost]
		public ActionResult DataTablesList(DtParameterModel dtParameterModel, SYSKULLANICIAra sYSKULLANICIAra)
		{
			
			sYSKULLANICIAra.Ara = dtParameterModel.AramaKriteri;
			
			if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
			{ //TODO: Bu bölümü düzenle
				sYSKULLANICIAra.AD = dtParameterModel.Search.Value;
			}

	 

			var kayitlar = _sYSKULLANICIService.Ara(sYSKULLANICIAra);

			return Json(new DataTableResult()
			{
				data = kayitlar.SYSKULLANICIDetayList.Select(t => new
				{
					//TODO: Bu bölümü düzenle
				t.KAYITNO,
				t.KULLANICIAD,
				t.SIFRE,
				t.BIRIMKAYITNO,
				t.VERSIYON,
				t.AD,
				t.SOYAD,
				t.GRUPKAYITNO,
				t.DURUM,
				t.SIFREKOD,
				t.DIL,
				t.KULLANICITUR,
				t.KURUMKAYITNO,
                t.EPosta,
                t.Gsm,
                t.KurumAdi,

					Islemler =$@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "SYSKULLANICI", new {id = t.KAYITNO})}' title='Düzenle'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "SYSKULLANICI", new {id = t.KAYITNO})}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "SYSKULLANICI", new {id = t.KAYITNO})}' title='Sil'><i class='fa fa-trash'></i></a>"
				}),
				draw = dtParameterModel.Draw,
				recordsTotal = kayitlar.ToplamKayitSayisi,
				recordsFiltered = kayitlar.ToplamKayitSayisi
			}, JsonRequestBehavior.AllowGet);
		}

		
		public ActionResult Ekle()
		{
			SayfaBaslik($"Kullanıcı Ekle");

			var model = new SYSKULLANICIKaydetModel
			{
				SYSKULLANICI = new SYSKULLANICI()
			};
			
	

			return View("Kaydet", model);
		}

		
		public ActionResult Guncelle(int id)
		{
			SayfaBaslik($"Kullanıcı Güncelle");

			var model = new SYSKULLANICIKaydetModel
			{
				SYSKULLANICI = _sYSKULLANICIService.GetirById(id)
			};

	
			return View("Kaydet", model);
		}


		
		public ActionResult Detay(int id)
		{
			SayfaBaslik($"Kullanıcı Detay");

			var model = new SYSKULLANICIDetayModel
			{
				SYSKULLANICIDetay = _sYSKULLANICIService.DetayGetirById(id)
			};

	
			return View("Detay", model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Kaydet(SYSKULLANICIKaydetModel sYSKULLANICIKaydetModel)
		{
			if (sYSKULLANICIKaydetModel.SYSKULLANICI.KAYITNO > 0)
			{
				_sYSKULLANICIService.Guncelle(sYSKULLANICIKaydetModel.SYSKULLANICI.List());
			}
			else
			{
				_sYSKULLANICIService.Ekle(sYSKULLANICIKaydetModel.SYSKULLANICI.List());
			}

			return Yonlendir(Url.Action("Index"), $"SYSKULLANICI kayıdı başarıyla gerçekleştirilmiştir.");
			//return Yonlendir(Url.Action("Detay","SYSKULLANICI",new{id=sYSKULLANICIKaydetModel.SYSKULLANICI.Id}), $"SYSKULLANICI kayıdı başarıyla gerçekleştirilmiştir.");
		}


		
		public ActionResult Sil(int id)
		{
			SayfaBaslik($"Kullanıcı Silme İşlem Onayı");
			return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel =  $"/SYSKULLANICI/Index/{id}" });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Sil(DeleteViewModel model)
		{

            var kullaniciDetay=_sysKullaniciDetayService.Getir(new SYSKULLANICIDETAYAra { KULLANICIKAYITNO = model.Id }).FirstOrDefault();
            if (kullaniciDetay != null)
            {
                _sysKullaniciDetayService.Sil(kullaniciDetay.KAYITNO.List());
            }
            _sysRolKullaniciService.RolKullaniciSil(null, model.Id);
			_sYSKULLANICIService.Sil(model.Id.List());

			return Yonlendir(Url.Action("Index"), $"Kullanıcı Başarıyla silindi");
		}


		public ActionResult AjaxAra(string key, SYSKULLANICIAra sYSKULLANICIAra=null, int limit = 10, int baslangic = 0)
		{
			
			 if (sYSKULLANICIAra == null)
			{
				sYSKULLANICIAra = new SYSKULLANICIAra();
			}

            sYSKULLANICIAra.Ara = new Ara
            {
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {
                    new Siralama
                    {
                        KolonAdi = LinqExtensions.GetPropertyName((SYSKULLANICI t) => t.KAYITNO),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }
                }
            };


			//TODO: Bu bölümü düzenle
			sYSKULLANICIAra.KULLANICIAD = key;

			var sYSKULLANICIList =  _sYSKULLANICIService.Getir(sYSKULLANICIAra);


			var data = sYSKULLANICIList.Select(sYSKULLANICI => new AutoCompleteData
			{
			//TODO: Bu bölümü düzenle
				id = sYSKULLANICI.KAYITNO.ToString(),
				text = sYSKULLANICI.KULLANICIAD.ToString(),
				description = sYSKULLANICI.AD.ToString(),
			}).ToList();
			return Json(data, JsonRequestBehavior.AllowGet);
		}


		public ActionResult AjaxTekDeger(int id)
		{
			var sYSKULLANICI = _sYSKULLANICIService.GetirById(id);


			var data = new AutoCompleteData
			{//TODO: Bu bölümü düzenle
				id = sYSKULLANICI.KAYITNO.ToString(),
				text = sYSKULLANICI.KULLANICIAD.ToString(),
				description = sYSKULLANICI.AD.ToString(),
			};

			return Json(data, JsonRequestBehavior.AllowGet);
		}

		public ActionResult AjaxCokDeger(string id)
		{
			var sYSKULLANICIList = _sYSKULLANICIService.Getir(new SYSKULLANICIAra() { KAYITNOlar = id });


			var data = sYSKULLANICIList.Select(sYSKULLANICI => new AutoCompleteData
			{ //TODO: Bu bölümü düzenle
				id = sYSKULLANICI.KAYITNO.ToString(),
				text = sYSKULLANICI.KULLANICIAD.ToString(),
				description = sYSKULLANICI.AD.ToString()
			});

			return Json(data, JsonRequestBehavior.AllowGet);
		}

	}
}
