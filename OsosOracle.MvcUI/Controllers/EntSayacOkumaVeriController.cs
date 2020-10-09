using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.ENTSAYACDURUMSUComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.MvcUI.Models.EntSayacOkumaVeriModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.MvcUI.Filters;
using OsosOracle.MvcUI.Resources;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class EntSayacOkumaVeriController : BaseController
    {
        private readonly IEntSayacOkumaVeriService _eNTSAYACDURUMSUService;
        private readonly IENTSAYACService _entSayacService;

        public EntSayacOkumaVeriController(IEntSayacOkumaVeriService eNTSAYACDURUMSUService, IENTSAYACService entSayacService)
        {
            _eNTSAYACDURUMSUService = eNTSAYACDURUMSUService;
            _entSayacService = entSayacService;
        }


        public ActionResult Index()
        {
            SayfaBaslik(Dil.SayacVerileri);
            var model = new EntSayacOkumaVeriIndexModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DataTablesList(DtParameterModel dtParameterModel, EntSayacOkumaVeriAra EntSayacOkumaVeriAra)
        {
            if (EntSayacOkumaVeriAra.SayacKayitNo != null)
            {
                var sayac = _entSayacService.GetirById((int)EntSayacOkumaVeriAra.SayacKayitNo);
                EntSayacOkumaVeriAra.SayacId = "ELM" + sayac.SERINO;
            }
            EntSayacOkumaVeriAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                EntSayacOkumaVeriAra.SayacId = dtParameterModel.Search.Value;
            }
            EntSayacOkumaVeriAra.Ara.Siralama = new List<Siralama>
            {
                new Siralama
                {
                    KolonAdi=LinqExtensions.GetPropertyName((EntSayacOkumaVeri t)=>t.OkumaTarih),
                    SiralamaTipi=EnumSiralamaTuru.Desc
                }
            };


            var kayitlar = _eNTSAYACDURUMSUService.Ara(EntSayacOkumaVeriAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.EntSayacOkumaVeriDetayList.Select(t => new
                {
                    //TODO: Bu bölümü düzenle
                    t.KayitNo,
                    t.SayacId,
                    OkumaTarih = t.OkumaTarih.ToString(),
                    t.Ceza1,
                    t.Ceza2,
                    t.Ceza3,
                    t.Ceza4,
                    t.Magnet,
                    t.Ariza,
                    t.Vana,
                    t.PulseHata,
                    t.AnaPilZayif,
                    t.AnaPilBitti,
                    t.MotorPilZayif,
                    t.KrediAz,
                    t.KrediBitti,
                    t.RtcHata,
                    t.AsiriTuketim,
                    t.Ceza4Iptal,
                    t.ArizaK,
                    t.ArizaP,
                    t.ArizaPil,
                    t.Borc,
                    t.Cap,
                    t.BaglantiSayisi,
                    t.KritikKredi,
                    t.Limit1,
                    t.Limit2,
                    t.Limit3,
                    t.Limit4,
                    t.Fiyat1,
                    t.Fiyat2,
                    t.Fiyat3,
                    t.Fiyat4,
                    t.Fiyat5,
                    t.Iterasyon,
                    t.PilAkim,
                    t.PilVoltaj,
                    t.AboneNo,
                    t.AboneTip,
                    t.IlkPulseTarih,
                    t.SonPulseTarih,
                    t.BorcTarih,
                    t.MaxDebi,
                    t.MaxDebiSinir,
                    t.DonemGun,
                    t.Donem,
                    t.VanaAcmaTarih,
                    t.VanaKapamaTarih,
                    t.Sicaklik,
                    t.MinSicaklik,
                    t.MaxSicaklik,
                    t.YanginModu,
                    t.SonYuklenenKrediTarih,
                    t.SayacTarih,
                    t.HaftaninGunu,
                    t.Tuketim,
                    t.Tuketim1,
                    t.Tuketim2,
                    t.Tuketim3,
                    t.Tuketim4,
                    t.HarcananKredi,
                    t.KalanKredi,
                    t.KonsSeriNo,
                    t.Ip,
                    t.Rssi,

                    Islemler = $@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "ENTSAYACDURUMSU", new { id = t.KayitNo })}' title='Düzenle'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{Url.Action("Detay", "ENTSAYACDURUMSU", new { id = t.KayitNo })}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{Url.Action("Sil", "ENTSAYACDURUMSU", new { id = t.KayitNo })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Ekle()
        {
            SayfaBaslik($"ENTSAYACDURUMSU Ekle");

            var model = new EntSayacOkumaVeriKaydetModel
            {
                EntSayacOkumaVeri = new EntSayacOkumaVeri()
            };



            return View("Kaydet", model);
        }

        public ActionResult Guncelle(int id)
        {
            SayfaBaslik($"ENTSAYACDURUMSU Güncelle");

            var model = new EntSayacOkumaVeriKaydetModel
            {
                EntSayacOkumaVeri = _eNTSAYACDURUMSUService.GetirById(id)
            };


            return View("Kaydet", model);
        }



        public ActionResult Detay(int id)
        {
            SayfaBaslik($"ENTSAYACDURUMSU Detay");

            var model = new EntSayacOkumaVeriDetayModel
            {
                EntSayacOkumaVeriDetay = _eNTSAYACDURUMSUService.DetayGetirById(id)
            };


            return View("Detay", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(EntSayacOkumaVeriKaydetModel eNTSAYACDURUMSUKaydetModel)
        {
            if (eNTSAYACDURUMSUKaydetModel.EntSayacOkumaVeri.KayitNo > 0)
            {
                _eNTSAYACDURUMSUService.Guncelle(eNTSAYACDURUMSUKaydetModel.EntSayacOkumaVeri.List());
            }
            else
            {
                _eNTSAYACDURUMSUService.Ekle(eNTSAYACDURUMSUKaydetModel.EntSayacOkumaVeri.List());
            }

            return Yonlendir(Url.Action("Index"), $"ENTSAYACDURUMSU kayıdı başarıyla gerçekleştirilmiştir.");
            //return Yonlendir(Url.Action("Detay","ENTSAYACDURUMSU",new{id=eNTSAYACDURUMSUKaydetModel.ENTSAYACDURUMSU.Id}), $"ENTSAYACDURUMSU kayıdı başarıyla gerçekleştirilmiştir.");
        }



        public ActionResult Sil(int id)
        {
            SayfaBaslik($"ENTSAYACDURUMSU Silme İşlem Onayı");
            return View("_SilOnay", new DeleteViewModel() { Id = id, RedirectUrlForCancel = $"/ENTSAYACDURUMSU/Index/{id}" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(DeleteViewModel model)
        {
            _eNTSAYACDURUMSUService.Sil(model.Id.List());

            return Yonlendir(Url.Action("Index"), $"ENTSAYACDURUMSU Başarıyla silindi");
        }


        public ActionResult AjaxAra(string key, EntSayacOkumaVeriAra eNTSAYACDURUMSUAra = null, int limit = 10, int baslangic = 0)
        {

            if (eNTSAYACDURUMSUAra == null)
            {
                eNTSAYACDURUMSUAra = new EntSayacOkumaVeriAra();
            }

            eNTSAYACDURUMSUAra.Ara = new Ara
            {
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {
                    new Siralama
                    {
                        KolonAdi = LinqExtensions.GetPropertyName((EntSayacOkumaVeri t) => t.KayitNo),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }
                }
            };


            //TODO: Bu bölümü düzenle
            eNTSAYACDURUMSUAra.SayacId = key;

            var eNTSAYACDURUMSUList = _eNTSAYACDURUMSUService.Getir(eNTSAYACDURUMSUAra);


            var data = eNTSAYACDURUMSUList.Select(eNTSAYACDURUMSU => new AutoCompleteData
            {
                //TODO: Bu bölümü düzenle
                id = eNTSAYACDURUMSU.KayitNo.ToString(),
                text = eNTSAYACDURUMSU.SayacId.ToString(),
                description = eNTSAYACDURUMSU.KayitNo.ToString(),
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AjaxTekDeger(int id)
        {
            var eNTSAYACDURUMSU = _eNTSAYACDURUMSUService.GetirById(id);


            var data = new AutoCompleteData
            {//TODO: Bu bölümü düzenle
                id = eNTSAYACDURUMSU.KayitNo.ToString(),
                text = eNTSAYACDURUMSU.SayacId.ToString(),
                description = eNTSAYACDURUMSU.KayitNo.ToString(),
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxCokDeger(string id)
        {
            var eNTSAYACDURUMSUList = _eNTSAYACDURUMSUService.Getir(new EntSayacOkumaVeriAra() { });


            var data = eNTSAYACDURUMSUList.Select(eNTSAYACDURUMSU => new AutoCompleteData
            { //TODO: Bu bölümü düzenle
                id = eNTSAYACDURUMSU.KayitNo.ToString(),
                text = eNTSAYACDURUMSU.SayacId.ToString(),
                description = eNTSAYACDURUMSU.KayitNo.ToString()
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}

