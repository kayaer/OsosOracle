

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.ENTSAYACSONDURUMSUComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Entities.Enums;
using OsosOracle.MvcUI.Infrastructure;
using OsosOracle.MvcUI.Models.ENTSAYACSONDURUMSUModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;

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
			SayfaBaslik($"ENTSAYACSONDURUMSU İşlemleri");
			var model= new ENTSAYACSONDURUMSUIndexModel();
			return View(model);
		}

		[HttpPost]
		public ActionResult DataTablesList(DtParameterModel dtParameterModel, ENTSAYACSONDURUMSUAra eNTSAYACSONDURUMSUAra)
		{
			
			eNTSAYACSONDURUMSUAra.Ara = dtParameterModel.AramaKriteri;
			
			if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
			{ //TODO: Bu bölümü düzenle
				eNTSAYACSONDURUMSUAra.Adi = dtParameterModel.Search.Value;
			}

	 

			var kayitlar = _eNTSAYACSONDURUMSUService.Ara(eNTSAYACSONDURUMSUAra);

			return Json(new DataTableResult()
			{
				data = kayitlar.ENTSAYACSONDURUMSUDetayList.Select(t => new
				{
					//TODO: Bu bölümü düzenle
					t.KAYITNO,
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
				t.BORC,
				t.ITERASYON,
				t.PILAKIM,
				t.PILVOLTAJ,
				t.SONYUKLENENKREDITARIH,
				t.ABONENO,
				t.ABONETIP,
				t.ILKPULSETARIH,
				t.SONPULSETARIH,
				t.BORCTARIH,
				t.MAXDEBI,
				t.MAXDEBISINIR,
				t.DONEMGUN,
				t.DONEM,
				t.VANAACMATARIH,
				t.VANAKAPAMATARIH,
				t.SICAKLIK,
				t.MINSICAKLIK,
				t.MAXSICAKLIK,
				t.YANGINMODU,
				t.ASIRITUKETIM,
				t.ARIZA,
				t.SONBILGILENDIRMEZAMANI,
				t.PARAMETRE,
				t.ACIKLAMA,
				t.DURUM,
				t.KREDIBITTI,
				t.KREDIAZ,
				t.ANAPILBITTI,
				t.SONBAGLANTIZAMAN,
				t.SONOKUMATARIH,
				t.PULSEHATA,
				t.RTCHATA,
				t.VANA,
				t.CEZA1,
				t.CEZA2,
				t.CEZA3,
				t.CEZA4,
				t.MAGNET,
				t.ANAPILZAYIF,

					Islemler =$@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "ENTSAYACSONDURUMSU", new {id = t.KAYITNO})}' title='Düzenle'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "ENTSAYACSONDURUMSU", new {id = t.KAYITNO})}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "ENTSAYACSONDURUMSU", new {id = t.KAYITNO})}' title='Sil'><i class='fa fa-trash'></i></a>"
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
			return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel =  $"/ENTSAYACSONDURUMSU/Index/{id}" });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Sil(DeleteViewModel model)
		{
			_eNTSAYACSONDURUMSUService.Sil(model.Id.List());

			return Yonlendir(Url.Action("Index"), $"ENTSAYACSONDURUMSU Başarıyla silindi");
		}


		public ActionResult AjaxAra(string key, ENTSAYACSONDURUMSUAra eNTSAYACSONDURUMSUAra=null, int limit = 10, int baslangic = 0)
		{
			
			 if (eNTSAYACSONDURUMSUAra == null)
			{
				eNTSAYACSONDURUMSUAra = new ENTSAYACSONDURUMSUAra();
			}

            eNTSAYACSONDURUMSUAra.Ara = new Ara
            {
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {
                    new Siralama
                    {
                        KolonAdi = LinqExtensions.GetPropertyName((ENTSAYACSONDURUMSU t) => t.KAYITNO),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }
                }
            };


			//TODO: Bu bölümü düzenle
			eNTSAYACSONDURUMSUAra.Adi = key;

			var eNTSAYACSONDURUMSUList =  _eNTSAYACSONDURUMSUService.Getir(eNTSAYACSONDURUMSUAra);


			var data = eNTSAYACSONDURUMSUList.Select(eNTSAYACSONDURUMSU => new AutoCompleteData
			{
			//TODO: Bu bölümü düzenle
				id = eNTSAYACSONDURUMSU.Id.ToString(),
				text = eNTSAYACSONDURUMSU.Id.ToString(),
				description = eNTSAYACSONDURUMSU.Id.ToString(),
			}).ToList();
			return Json(data, JsonRequestBehavior.AllowGet);
		}


		public ActionResult AjaxTekDeger(int id)
		{
			var eNTSAYACSONDURUMSU = _eNTSAYACSONDURUMSUService.GetirById(id);


			var data = new AutoCompleteData
			{//TODO: Bu bölümü düzenle
				id = eNTSAYACSONDURUMSU.Id.ToString(),
				text = eNTSAYACSONDURUMSU.Id.ToString(),
				description = eNTSAYACSONDURUMSU.Id.ToString(),
			};

			return Json(data, JsonRequestBehavior.AllowGet);
		}

		public ActionResult AjaxCokDeger(string id)
		{
			var eNTSAYACSONDURUMSUList = _eNTSAYACSONDURUMSUService.Getir(new ENTSAYACSONDURUMSUAra() { KAYITNOlar = id });


			var data = eNTSAYACSONDURUMSUList.Select(eNTSAYACSONDURUMSU => new AutoCompleteData
			{ //TODO: Bu bölümü düzenle
				id = eNTSAYACSONDURUMSU.Id.ToString(),
				text = eNTSAYACSONDURUMSU.Id.ToString(),
				description = eNTSAYACSONDURUMSU.Id.ToString()
			});

			return Json(data, JsonRequestBehavior.AllowGet);
		}

	}
}

