using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.ENTABONESAYACComplexTypes;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.MvcUI.Filters;
using OsosOracle.MvcUI.Models.ENTABONESAYACModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class AboneSayacController : BaseController
    {
        private readonly IENTABONESAYACService _entAboneSayacService;

        public AboneSayacController(IENTABONESAYACService entAboneSayacService)
        {
            _entAboneSayacService = entAboneSayacService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DataTablesList(DtParameterModel dtParameterModel, ENTABONESAYACAra entAboneSayacAra)
        {

            entAboneSayacAra.Ara = dtParameterModel.AramaKriteri;

            if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
            { //TODO: Bu bölümü düzenle
                //acigaAlinmaAra.Adi = dtParameterModel.Search.Value;
            }

            var kayitlar = _entAboneSayacService.Ara(entAboneSayacAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.ENTABONESAYACDetayList.Select(t => new
                {
                    t.KAYITNO,
                    t.TARIFE,
                    t.TARIFEKAYITNO,
                    Islemler = $@"<a class='btn btn-xs btn-info ' href='{Url.Action("Guncelle", "Kurum", new { id = t.KAYITNO })}' title='Düzenle'><i class='fa fa-edit'></i></a>							 
								<a class='btn btn-xs btn-danger modalizer ' href='{Url.Action("Sil", "Kurum", new { id = t.KAYITNO })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Ekle(int KayitNo)
        {
            var model = new ENTABONESAYACKaydetModel() { Tip = KayitNo };
           
            return View("Kaydet", model);
        }
        
        public ActionResult Guncelle(int sayacKayitNo)
        {


            var model = new ENTABONESAYACKaydetModel
            {
                ENTABONESAYAC = _entAboneSayacService.Getir(new ENTABONESAYACAra { SAYACKAYITNO = sayacKayitNo }).FirstOrDefault()
            };


            return View("Kaydet", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(ENTABONESAYACKaydetModel aboneSayacKaydetModel)
        {
            //aboneSayacKaydetModel.ENTSAYAC.KURUMKAYITNO = AktifKullanici.KurumKayitNo;
            aboneSayacKaydetModel.ENTABONESAYAC.OLUSTURAN = AktifKullanici.KayitNo;
            aboneSayacKaydetModel.ENTABONESAYAC.GUNCELLEYEN = AktifKullanici.KayitNo;
            if (aboneSayacKaydetModel.ENTABONESAYAC.KAYITNO > 0)
            {
                _entAboneSayacService.Guncelle(aboneSayacKaydetModel.ENTABONESAYAC.List());
            }
            else
            {
                _entAboneSayacService.Ekle(aboneSayacKaydetModel.ENTABONESAYAC.List());
            }

            return Yonlendir(Url.Action("Index", "Sayac", new { sayacKayitNo = aboneSayacKaydetModel.ENTABONESAYAC.KAYITNO }), $"Abone-Sayaç kayıdı başarıyla gerçekleştirilmiştir.");
        }


    }
}