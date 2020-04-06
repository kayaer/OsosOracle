using Microsoft.Reporting.WebForms;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.ENTABONEComplexTypes;
using OsosOracle.Entities.ComplexType.ENTABONESAYACComplexTypes;
using OsosOracle.Entities.ComplexType.ENTSATISComplexTypes;
using OsosOracle.Entities.ComplexType.ENTSAYACComplexTypes;
using OsosOracle.Entities.ComplexType.PRMTARIFEGAZComplexTypes;
using OsosOracle.Entities.ComplexType.PRMTARIFEKALORIMETREComplexTypes;
using OsosOracle.Entities.ComplexType.PRMTARIFESUComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Entities.Enums;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.MvcUI.Filters;
using OsosOracle.MvcUI.Models.ENTABONEModels;
using OsosOracle.MvcUI.Models.ENTSATISModels;
using OsosOracle.MvcUI.Models.ENTSATISModels.Yeni;
using OsosOracle.MvcUI.Reports.ReportModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace OsosOracle.MvcUI.Controllers
{

    [AuthorizeUser]
    public class EntSatisController : BaseController
    {
        private readonly IENTSATISService _entSatisService;
        private readonly IENTSAYACService _entSayacService;
        private readonly IENTABONEService _entAboneService;
        private readonly IENTABONESAYACService _entAboneSayacService;
        private readonly IPRMTARIFESUService _prmTarifeSuService;
        private readonly IPRMTARIFEKALORIMETREService _prmKALORIMETREService;
        private readonly IPRMTARIFEGAZService _prmTarifeGazService;
        public EntSatisController(IENTSATISService entSatisService,
            IENTSAYACService entSayacService,
            IENTABONEService entAboneService,
            IENTABONESAYACService entAboneSayacService,
            IPRMTARIFESUService prmTarifeSuService,
            IPRMTARIFEKALORIMETREService prmKALORIMETREService,
            IPRMTARIFEGAZService prmTarifeGazService)
        {
            _entSatisService = entSatisService;
            _entSayacService = entSayacService;
            _entAboneService = entAboneService;
            _entAboneSayacService = entAboneSayacService;
            _prmTarifeSuService = prmTarifeSuService;
            _prmKALORIMETREService = prmKALORIMETREService;
            _prmTarifeGazService = prmTarifeGazService;
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult Ekle()
        {
            return View("Kaydet", new SatisModel()
            {
                SuSatisModel = new SuSatisModel
                {
                    Satis = new ENTSATIS
                    {
                        OdemeTipiKayitNo = enumOdemeTipi.Nakit.GetHashCode()
                    }
                },
                KalorimetreSatisModel = new KalorimetreSatisModel
                {
                    Satis = new ENTSATIS
                    {
                        OdemeTipiKayitNo = enumOdemeTipi.Nakit.GetHashCode()
                    }
                }
            });
        }
        public ActionResult BedelsizSatis()
        {
            return View(new SatisModel());
        }
        public ActionResult SatisIptal()
        {
            return View(new SatisModel());
        }
        public ActionResult GazSatis()
        {
            return View(new SatisModel());
        }
        public JsonResult SatisPars(SatisModel model)
        {
            if (string.IsNullOrEmpty(model.HamData))
            {
                model.HamData = "E#1#20191004#0/0/0#0#0#b#b#1#0#0#0#1#0#b#b#b#b#b#0#0#0#0#0#65535#65535#65535#65535#0#1#3#1#0#|G#1#20191004#0/0/0#0#0#b#b#1#0#0#1#0#b#b#b#b#b#b#b#0#0#1#3#1#0#|S#1#20191004#0/0/0#0#0#b#b#1#0#0#0#0#1#0#1#1#65527#65535#b#b#b#b#b#b#b#b#0#1#3#1#0#|K#1#20191004#0/0/0#0#0#b#b#1#0#0#0#1#0#b#b#b#b#0#0#0#0#0#0#0#1#3#1#0#|";
            }
            model.SuSatisModel.SogukSuOkunan = new SogukSuOkunan(model.HamData);
            model.SuSatisModel.AboneSayacDetay = _entAboneSayacService.DetayGetir(new ENTABONESAYACAra { SayacSeriNo = model.SuSatisModel.SogukSuOkunan.SayacSeriNo, SayacTur = 1, Durum = 1 }).FirstOrDefault();
            if (model.SuSatisModel.AboneSayacDetay != null)
            {
                model.SuSatisModel.PrmTarifeSuDetay = _prmTarifeSuService.DetayGetir(new PRMTARIFESUAra { KAYITNO = model.SuSatisModel.AboneSayacDetay.TARIFEKAYITNO }).FirstOrDefault();

            }

            model.KalorimetreSatisModel.KalorimetreOkunan = new KalorimetreOkunan(model.HamData);
            model.KalorimetreSatisModel.AboneSayacDetay = _entAboneSayacService.DetayGetir(new ENTABONESAYACAra { SayacSeriNo = model.KalorimetreSatisModel.KalorimetreOkunan.CihazNo, SayacTur = 2, Durum = 1 }).FirstOrDefault();
            if (model.KalorimetreSatisModel.AboneSayacDetay != null)
            {
                model.KalorimetreSatisModel.PrmTarifeKALORIMETREDetay = _prmKALORIMETREService.DetayGetir(new PRMTARIFEKALORIMETREAra { KAYITNO = model.KalorimetreSatisModel.AboneSayacDetay.TARIFEKAYITNO }).FirstOrDefault();

            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BedelsizSatisPars(SatisModel model)
        {
            if (string.IsNullOrEmpty(model.HamData))
            {
                model.HamData = "E#1#20191004#0/0/0#0#0#b#b#1#0#0#0#1#0#b#b#b#b#b#0#0#0#0#0#65535#65535#65535#65535#0#1#3#1#0#|G#1#20191004#0/0/0#0#0#b#b#1#0#0#1#0#b#b#b#b#b#b#b#0#0#1#3#1#0#|S#1#20191004#0/0/0#0#0#b#b#1#0#0#0#0#1#0#1#1#65527#65535#b#b#b#b#b#b#b#b#0#1#3#1#0#|K#1#20191004#0/0/0#0#0#b#b#1#0#0#0#1#0#b#b#b#b#0#0#0#0#0#0#0#1#3#1#0#|";
            }
            model.SuSatisModel.SogukSuOkunan = new SogukSuOkunan(model.HamData);
            model.SuSatisModel.AboneSayacDetay = _entAboneSayacService.DetayGetir(new ENTABONESAYACAra { SayacSeriNo = model.SuSatisModel.SogukSuOkunan.SayacSeriNo, SayacTur = 1, Durum = 1 }).FirstOrDefault();
            model.SuSatisModel.PrmTarifeSuDetay = _prmTarifeSuService.DetayGetir(new PRMTARIFESUAra { KAYITNO = model.SuSatisModel.AboneSayacDetay.TARIFEKAYITNO }).FirstOrDefault();

            model.KalorimetreSatisModel.KalorimetreOkunan = new KalorimetreOkunan(model.HamData);
            model.KalorimetreSatisModel.AboneSayacDetay = _entAboneSayacService.DetayGetir(new ENTABONESAYACAra { SayacSeriNo = model.KalorimetreSatisModel.KalorimetreOkunan.CihazNo, SayacTur = 2, Durum = 1 }).FirstOrDefault();
            model.KalorimetreSatisModel.PrmTarifeKALORIMETREDetay = _prmKALORIMETREService.DetayGetir(new PRMTARIFEKALORIMETREAra { KAYITNO = model.KalorimetreSatisModel.AboneSayacDetay.TARIFEKAYITNO }).FirstOrDefault();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SatisIptalPars(SatisModel model)
        {
            if (string.IsNullOrEmpty(model.HamData))
            {
                model.HamData = "E#1#20191004#0/0/0#0#0#b#b#1#0#0#0#1#0#b#b#b#b#b#0#0#0#0#0#65535#65535#65535#65535#0#1#3#1#0#|G#1#20191004#0/0/0#0#0#b#b#1#0#0#1#0#b#b#b#b#b#b#b#0#0#1#3#1#0#|S#1#20191004#0/0/0#0#0#b#b#1#0#0#0#0#1#0#1#1#65527#65535#b#b#b#b#b#b#b#b#0#1#3#1#0#|K#1#20191004#0/0/0#300#0#b#b#1#0#0#0#1#0#b#b#b#b#0#0#0#0#0#0#0#1#3#1#0#|";
            }
            model.SuSatisModel.SogukSuOkunan = new SogukSuOkunan(model.HamData);
            model.SuSatisModel.AboneSayacDetay = _entAboneSayacService.DetayGetir(new ENTABONESAYACAra { SayacSeriNo = model.SuSatisModel.SogukSuOkunan.SayacSeriNo, SayacTur = 1, Durum = 1 }).FirstOrDefault();
            if (model.SuSatisModel.AboneSayacDetay != null)
            {
                model.SuSatisModel.PrmTarifeSuDetay = _prmTarifeSuService.DetayGetir(new PRMTARIFESUAra { KAYITNO = model.SuSatisModel.AboneSayacDetay.TARIFEKAYITNO }).FirstOrDefault();
                model.SuSatisModel.Satis = _entSatisService.Getir(new ENTSATISAra { ABONEKAYITNO = model.SuSatisModel.AboneSayacDetay.ABONEKAYITNO, SAYACKAYITNO = model.SuSatisModel.AboneSayacDetay.SAYACKAYITNO, SatisTipi = enumSatisTipi.Satis.GetHashCode() }).OrderByDescending(x => x.OLUSTURMATARIH).FirstOrDefault();
            }


            if (model.SuSatisModel.Satis != null)
            {
                model.SuSatisModel.SatisIptal.AnaKredi = Convert.ToInt32(model.SuSatisModel.SogukSuOkunan.Kredi) - Convert.ToInt32(model.SuSatisModel.Satis.KREDI);
                model.SuSatisModel.SatisIptal.YedekKredi = Convert.ToInt32(model.SuSatisModel.SogukSuOkunan.YedekKredi) - Convert.ToInt32(model.SuSatisModel.Satis.YEDEKKREDI);

            }


            model.KalorimetreSatisModel.KalorimetreOkunan = new KalorimetreOkunan(model.HamData);
            model.KalorimetreSatisModel.AboneSayacDetay = _entAboneSayacService.DetayGetir(new ENTABONESAYACAra { SayacSeriNo = model.KalorimetreSatisModel.KalorimetreOkunan.CihazNo, SayacTur = 2, Durum = 1 }).FirstOrDefault();
            if (model.KalorimetreSatisModel.AboneSayacDetay != null)
            {
                model.KalorimetreSatisModel.PrmTarifeKALORIMETREDetay = _prmKALORIMETREService.DetayGetir(new PRMTARIFEKALORIMETREAra { KAYITNO = model.KalorimetreSatisModel.AboneSayacDetay.TARIFEKAYITNO }).FirstOrDefault();
                model.KalorimetreSatisModel.Satis = _entSatisService.Getir(new ENTSATISAra { ABONEKAYITNO = model.KalorimetreSatisModel.AboneSayacDetay.ABONEKAYITNO, SAYACKAYITNO = model.KalorimetreSatisModel.AboneSayacDetay.SAYACKAYITNO, SatisTipi = enumSatisTipi.Satis.GetHashCode() }).OrderByDescending(x => x.OLUSTURMATARIH).FirstOrDefault();

            }



            if (model.KalorimetreSatisModel.Satis != null)
            {
                model.KalorimetreSatisModel.SatisIptal.AnaKredi = Convert.ToInt32(model.KalorimetreSatisModel.KalorimetreOkunan.Kredi) - Convert.ToInt32(model.KalorimetreSatisModel.Satis.KREDI);
                model.KalorimetreSatisModel.SatisIptal.YedekKredi = Convert.ToInt32(model.KalorimetreSatisModel.KalorimetreOkunan.YedekKredi) - Convert.ToInt32(model.KalorimetreSatisModel.Satis.YEDEKKREDI);

            }
            //Negatif yükleme önlemi
            if (model.SuSatisModel.SatisIptal.AnaKredi < 0)
            {
                model.SuSatisModel.SatisIptal.AnaKredi = 0;
                model.SuSatisModel.SatisIptal.YedekKredi = 0;
            }
            //if (model.SuSatisModel.SatisIptal.YedekKredi < 0)
            //{
            //    model.SuSatisModel.SatisIptal.YedekKredi = 0;
            //}
            if (model.KalorimetreSatisModel.SatisIptal.AnaKredi < 0)
            {
                model.KalorimetreSatisModel.SatisIptal.AnaKredi = 0;
                model.KalorimetreSatisModel.SatisIptal.YedekKredi = 0;
            }
            //if (model.KalorimetreSatisModel.SatisIptal.YedekKredi < 0)
            //{
            //    model.KalorimetreSatisModel.SatisIptal.YedekKredi = 0;
            //}

            if (model.SuSatisModel.SogukSuOkunan.Ako == "*" && model.KalorimetreSatisModel.KalorimetreOkunan.Ako == "*")
            {
                throw new NotificationException("Kartta iptal edilecek satış bulunmuyor");
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SuHesapla(SatisModel model)
        {
            if (string.IsNullOrEmpty(model.HamData))
            {
                //hamdata = "E#1#20191004#0/0/0#0#0#b#b#1#0#0#0#11#0#*#*#*#*#*#0#0#0#0#0#65535#65535#65535#65535#0#0#0#0#0#|G#1#20191004#0/0/0#0#0#b#b#1#0#0#11#0#*#*#*#*#*#*#*#0#0#0#0#0#0#|S#1#20190524#16/0/9#0#0#0#b#b#b#0#0#0#11#0#16959#16959#65535#65535#*#*#*#*#*#*#*#*#0#0#0#0#0#|K#1#16022020#2/6/29#1000#0#*#0#b#b#0#0#11#1#*#*#*#*#0#0#0#0#0#0#0#0#0#0#0#|";
                model.HamData = "E#1#20191004#0/0/0#0#0#b#b#1#0#0#0#1#0#b#b#b#b#b#0#0#0#0#0#65535#65535#65535#65535#0#1#3#1#0#|G#1#20191004#0/0/0#0#0#b#b#1#0#0#1#0#b#b#b#b#b#b#b#0#0#1#3#1#0#|S#1#20191004#0/0/0#0#0#b#b#1#0#0#0#0#1#0#1#1#65527#65535#b#b#b#b#b#b#b#b#0#1#3#1#0#|K#1#20191004#0/0/0#0#0#b#b#1#0#0#0#1#0#b#b#b#b#0#0#0#0#0#0#0#1#3#1#0#|";
            }

            try
            {
                model.SuSatisModel.SogukSuOkunan = new SogukSuOkunan(model.HamData);
                model.SuSatisModel.AboneSayacDetay = _entAboneSayacService.DetayGetir(new ENTABONESAYACAra { SayacSeriNo = model.SuSatisModel.SogukSuOkunan.SayacSeriNo, SayacTur = 1, Durum = 1 }).FirstOrDefault();
                model.SuSatisModel.PrmTarifeSuDetay = _prmTarifeSuService.DetayGetir(new PRMTARIFESUAra { KAYITNO = model.SuSatisModel.AboneSayacDetay.TARIFEKAYITNO }).FirstOrDefault();

                //Aylık satın alabileceği üst limit eklendi
                string ustLimit = System.Configuration.ConfigurationManager.AppSettings["YesilVadiUstLimit"];

                var buaykiSatisList = _entSatisService.Getir(new ENTSATISAra { SatisTarihBaslangic = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), SatisTarihBitis = DateTime.Now, SAYACKAYITNO = model.SuSatisModel.AboneSayacDetay.SAYACKAYITNO });
                var satisTutarToplami = buaykiSatisList.Sum(x => x.SatisTutari);
                if (satisTutarToplami > Convert.ToDecimal(ustLimit))
                {
                    throw new NotificationException("Aylık satış üst limitine ulaştınız");
                }

                if (model.SuSatisModel.PrmTarifeSuDetay == null)
                {
                    throw new NotificationException("Tarife bilgileri çekilemedi");
                }

                if (model.SuSatisModel.Satis.ODEME == 0)
                {
                    throw new NotificationException("Tutar 0 dan farklı olmalıdır");
                }



                if (model.SuSatisModel.PrmTarifeSuDetay.TUKETIMKATSAYI == 0)
                {
                    throw new NotificationException("Tarife tüketim katsayı 0 dan farklı olmalıdır");

                }

                if (model.SuSatisModel.PrmTarifeSuDetay.BIRIMFIYAT == 0)
                {
                    throw new NotificationException("Tarife birim fiyat 0 dan farklı olmalıdır");

                }
                //Yeşil Vadi aylık bakım bedeli hesabı
                if (model.SuSatisModel.AboneSayacDetay.SonSatisTarihi != null && model.SuSatisModel.Satis.AylikBakimBedeli == 0 && model.KalorimetreSatisModel.Satis.AylikBakimBedeli == 0)
                {
                    var fark = (DateTime.Now - model.SuSatisModel.AboneSayacDetay.SonSatisTarihi).Value.TotalDays;
                    if (fark > 30)
                    {
                        const double daysToMonths = 30.4368499;

                        double months = fark / daysToMonths;

                        model.SuSatisModel.Satis.AylikBakimBedeli = Convert.ToInt32(months) * model.SuSatisModel.PrmTarifeSuDetay.AylikBakimBedeli;

                    }
                    else
                    {
                        if (DateTime.Now.Month != model.SuSatisModel.AboneSayacDetay.SonSatisTarihi.Value.Month)
                        {
                            model.SuSatisModel.Satis.AylikBakimBedeli = model.SuSatisModel.PrmTarifeSuDetay.AylikBakimBedeli;
                        }
                    }

                }



                if (model.SuSatisModel.PrmTarifeSuDetay.Ctv == 0)
                {
                    throw new NotificationException("Ctv değeri 0 girilemez");
                }

                model.SuSatisModel.Satis.Ctv = model.SuSatisModel.PrmTarifeSuDetay.Ctv * model.SuSatisModel.Satis.ODEME;
                model.SuSatisModel.Satis.Kdv = (model.SuSatisModel.Satis.ODEME - model.SuSatisModel.Satis.Ctv) * model.SuSatisModel.PrmTarifeSuDetay.Kdv;
                model.SuSatisModel.Satis.SatisTutari = (model.SuSatisModel.Satis.ODEME - model.SuSatisModel.Satis.Ctv - model.SuSatisModel.Satis.Kdv - model.SuSatisModel.Satis.AylikBakimBedeli);

                model.SuSatisModel.Satis.YEDEKKREDI = (model.SuSatisModel.PrmTarifeSuDetay.YEDEKKREDI * model.SuSatisModel.PrmTarifeSuDetay.TUKETIMKATSAYI);
                model.SuSatisModel.Satis.KREDI = ((model.SuSatisModel.Satis.SatisTutari * model.SuSatisModel.PrmTarifeSuDetay.TUKETIMKATSAYI) / model.SuSatisModel.PrmTarifeSuDetay.BIRIMFIYAT);
                //model.SuSatisModel.Satis.KREDI = Math.Round(Convert.ToDecimal( model.SuSatisModel.Satis.KREDI), 2);
                model.SuSatisModel.Satis.KREDI = Math.Floor(Convert.ToDecimal(model.SuSatisModel.Satis.KREDI));

                if (model.SuSatisModel.Satis.AylikBakimBedeli >= model.SuSatisModel.Satis.SatisTutari + model.SuSatisModel.Satis.AylikBakimBedeli)
                {
                    throw new NotificationException("Aylık Bakım Bedelinden düşük tutar giremezsiniz");
                }

                if (model.SuSatisModel.Satis.KREDI < model.SuSatisModel.Satis.YEDEKKREDI)
                {
                    throw new NotificationException("Yedek kredi den az miktarda satış yapılamaz");
                }
                if (model.SuSatisModel.SogukSuOkunan.Ako == "b" && model.SuSatisModel.SogukSuOkunan.Yko == "b")
                {
                    model.SuSatisModel.Satis.ToplamKredi = Convert.ToInt32(model.SuSatisModel.SogukSuOkunan.Kredi) +
                        Convert.ToInt32(Convert.ToInt32(model.SuSatisModel.SogukSuOkunan.YedekKredi) * model.SuSatisModel.PrmTarifeSuDetay.TUKETIMKATSAYI) +
                        model.SuSatisModel.Satis.KREDI - model.SuSatisModel.Satis.YEDEKKREDI;
                    model.SuSatisModel.Satis.YEDEKKREDI = model.SuSatisModel.Satis.YEDEKKREDI;
                }
                else if (model.SuSatisModel.SogukSuOkunan.Ako == "*" && model.SuSatisModel.SogukSuOkunan.Yko == "b")
                {
                    model.SuSatisModel.Satis.ToplamKredi = Convert.ToInt32(Convert.ToInt32(model.SuSatisModel.SogukSuOkunan.YedekKredi) * model.SuSatisModel.PrmTarifeSuDetay.TUKETIMKATSAYI)
                        + model.SuSatisModel.Satis.KREDI - model.SuSatisModel.Satis.YEDEKKREDI;
                    model.SuSatisModel.Satis.YEDEKKREDI = model.SuSatisModel.Satis.YEDEKKREDI;

                }
                else if (model.SuSatisModel.SogukSuOkunan.Ako == "*" && model.SuSatisModel.SogukSuOkunan.Yko == "*")
                {

                    model.SuSatisModel.Satis.ToplamKredi = model.SuSatisModel.Satis.KREDI - model.SuSatisModel.Satis.YEDEKKREDI;
                    model.SuSatisModel.Satis.YEDEKKREDI = model.SuSatisModel.Satis.YEDEKKREDI;
                }



                model.SuSatisModel.Satis.ABONEKAYITNO = model.SuSatisModel.AboneSayacDetay.ABONEKAYITNO;
                model.SuSatisModel.Satis.SAYACKAYITNO = model.SuSatisModel.AboneSayacDetay.SAYACKAYITNO;


            }
            catch (Exception ex)
            {
                throw new NotificationException(ex.Message);
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult KalorimetreHesapla(SatisModel model)
        {
            if (string.IsNullOrEmpty(model.HamData))
            {
                //hamdata = "E#1#20191004#0/0/0#0#0#b#b#1#0#0#0#11#0#*#*#*#*#*#0#0#0#0#0#65535#65535#65535#65535#0#0#0#0#0#|G#1#20191004#0/0/0#0#0#b#b#1#0#0#11#0#*#*#*#*#*#*#*#0#0#0#0#0#0#|S#1#20190524#16/0/9#0#0#0#b#b#b#0#0#0#11#0#16959#16959#65535#65535#*#*#*#*#*#*#*#*#0#0#0#0#0#|K#1#16022020#2/6/29#1000#0#*#0#b#b#0#0#11#1#*#*#*#*#0#0#0#0#0#0#0#0#0#0#0#|";
                model.HamData = "E#1#20191004#0/0/0#0#0#b#b#1#0#0#0#1#0#b#b#b#b#b#0#0#0#0#0#65535#65535#65535#65535#0#1#3#1#0#|G#1#20191004#0/0/0#0#0#b#b#1#0#0#1#0#b#b#b#b#b#b#b#0#0#1#3#1#0#|S#1#20191004#0/0/0#0#0#b#b#1#0#0#0#0#1#0#1#1#65527#65535#b#b#b#b#b#b#b#b#0#1#3#1#0#|K#1#20191004#0/0/0#0#0#b#b#1#0#0#0#1#0#b#b#b#b#0#0#0#0#0#0#0#1#3#1#0#|";
            }

            try
            {
                model.KalorimetreSatisModel.KalorimetreOkunan = new KalorimetreOkunan(model.HamData);
                model.KalorimetreSatisModel.AboneSayacDetay = _entAboneSayacService.DetayGetir(new ENTABONESAYACAra { SayacSeriNo = model.KalorimetreSatisModel.KalorimetreOkunan.CihazNo, SayacTur = 2, Durum = 1 }).FirstOrDefault();
                model.KalorimetreSatisModel.PrmTarifeKALORIMETREDetay = _prmKALORIMETREService.DetayGetir(new PRMTARIFEKALORIMETREAra { KAYITNO = model.KalorimetreSatisModel.AboneSayacDetay.TARIFEKAYITNO }).FirstOrDefault();
                //Aylık satın alabileceği üst limit eklendi
                string ustLimit = System.Configuration.ConfigurationManager.AppSettings["YesilVadiUstLimit"];

                var buaykiSatisList = _entSatisService.Getir(new ENTSATISAra { SatisTarihBaslangic = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), SatisTarihBitis = DateTime.Now, SAYACKAYITNO = model.KalorimetreSatisModel.AboneSayacDetay.SAYACKAYITNO });
                var satisTutarToplami = buaykiSatisList.Sum(x => x.SatisTutari);
                if (satisTutarToplami > Convert.ToDecimal(ustLimit))
                {
                    throw new NotificationException("Aylık satış üst limitine ulaştınız");
                }
                if (model.KalorimetreSatisModel.PrmTarifeKALORIMETREDetay == null)
                {
                    throw new NotificationException("Tarife bilgileri çekilemedi");
                }

                if (model.KalorimetreSatisModel.Satis.ODEME == 0)
                {
                    throw new NotificationException("Tutar 0 dan farklı olmalıdır");
                }
                if (model.KalorimetreSatisModel.PrmTarifeKALORIMETREDetay.YEDEKKREDI > 0)
                {
                    throw new NotificationException("Kalorimetre sayaçlarında yedek kredi 0 olmalıdır");
                }


                if (model.KalorimetreSatisModel.PrmTarifeKALORIMETREDetay.TUKETIMKATSAYI == 0)
                {
                    throw new NotificationException("Tarife tüketim katsayı 0 dan farklı olmalıdır");
                }



                //Yeşil Vadi aylık bakım bedeli hesabı
                if (model.KalorimetreSatisModel.AboneSayacDetay.SonSatisTarihi != null && model.KalorimetreSatisModel.Satis.AylikBakimBedeli == 0 && model.SuSatisModel.Satis.AylikBakimBedeli == 0)
                {
                    var fark = (DateTime.Now - model.KalorimetreSatisModel.AboneSayacDetay.SonSatisTarihi).Value.TotalDays;
                    if (fark > 30)
                    {
                        const double daysToMonths = 30.4368499;

                        double months = fark / daysToMonths;

                        model.KalorimetreSatisModel.Satis.AylikBakimBedeli = Convert.ToInt32(months) * model.KalorimetreSatisModel.PrmTarifeKALORIMETREDetay.AylikBakimBedeli;

                    }
                    else
                    {
                        if (DateTime.Now.Month != model.KalorimetreSatisModel.AboneSayacDetay.SonSatisTarihi.Value.Month)
                        {
                            model.KalorimetreSatisModel.Satis.AylikBakimBedeli = model.KalorimetreSatisModel.PrmTarifeKALORIMETREDetay.AylikBakimBedeli;
                        }
                    }

                }



                model.KalorimetreSatisModel.Satis.Kdv = (model.KalorimetreSatisModel.Satis.ODEME) * model.KalorimetreSatisModel.PrmTarifeKALORIMETREDetay.Kdv;
                model.KalorimetreSatisModel.Satis.SatisTutari = (model.KalorimetreSatisModel.Satis.ODEME - model.KalorimetreSatisModel.Satis.Kdv - model.KalorimetreSatisModel.Satis.AylikBakimBedeli);


                model.KalorimetreSatisModel.Satis.KREDI = (model.KalorimetreSatisModel.Satis.SatisTutari * model.KalorimetreSatisModel.PrmTarifeKALORIMETREDetay.TUKETIMKATSAYI) / model.KalorimetreSatisModel.PrmTarifeKALORIMETREDetay.BirimFiyat;
                model.KalorimetreSatisModel.Satis.KREDI = Math.Floor(Convert.ToDecimal(model.KalorimetreSatisModel.Satis.KREDI));
                if (model.KalorimetreSatisModel.Satis.AylikBakimBedeli >= model.KalorimetreSatisModel.Satis.SatisTutari + model.KalorimetreSatisModel.Satis.AylikBakimBedeli)
                {
                    decimal a = (-(decimal)model.KalorimetreSatisModel.Satis.SatisTutari + model.KalorimetreSatisModel.Satis.AylikBakimBedeli);
                    throw new NotificationException("Aylık Bakım Bedelinden düşük tutar giremezsiniz. Satış yapılacak miktar en az " + a + " olmalıdır");
                }

                if (model.KalorimetreSatisModel.KalorimetreOkunan.Ako == "b")
                {
                    model.KalorimetreSatisModel.Satis.ToplamKredi = Convert.ToInt32(model.KalorimetreSatisModel.KalorimetreOkunan.Kredi) + model.KalorimetreSatisModel.Satis.KREDI + model.KalorimetreSatisModel.PrmTarifeKALORIMETREDetay.YEDEKKREDI;
                }
                else
                {
                    model.KalorimetreSatisModel.Satis.ToplamKredi = model.KalorimetreSatisModel.Satis.KREDI + model.KalorimetreSatisModel.PrmTarifeKALORIMETREDetay.YEDEKKREDI;

                }



                model.KalorimetreSatisModel.Satis.YEDEKKREDI = model.KalorimetreSatisModel.PrmTarifeKALORIMETREDetay.YEDEKKREDI;
                model.KalorimetreSatisModel.Satis.ABONEKAYITNO = model.KalorimetreSatisModel.AboneSayacDetay.ABONEKAYITNO;
                model.KalorimetreSatisModel.Satis.SAYACKAYITNO = model.KalorimetreSatisModel.AboneSayacDetay.SAYACKAYITNO;


            }
            catch (Exception ex)
            {
                throw new NotificationException(ex.Message);
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        //[BackUpFilter]
        public JsonResult SatisYap(SatisModel model)
        {
            if (model.SuSatisModel.Satis != null && model.SuSatisModel.Satis.SatisTipi == enumSatisTipi.Satis.GetHashCode() )
            {
                if (model.SuSatisModel.Satis.KAYITNO == 0 && model.SuSatisModel.Satis.ODEME > 0)
                {

                    model.SuSatisModel.Satis.OLUSTURAN = AktifKullanici.KayitNo;
                    model.SuSatisModel.Satis = _entSatisService.Ekle(model.SuSatisModel.Satis);

                    var abone = _entAboneService.GetirById(model.SuSatisModel.Satis.ABONEKAYITNO);
                    abone.SonSatisTarih = DateTime.Now;
                    _entAboneService.Guncelle(abone.List());
                }
            }
            else if (model.SuSatisModel.Satis != null && model.SuSatisModel.Satis.SatisTipi == enumSatisTipi.BedelsizSatis.GetHashCode())
            {
                model.SuSatisModel.Satis.OLUSTURAN = AktifKullanici.KayitNo;

                model.SuSatisModel.Satis = _entSatisService.Ekle(model.SuSatisModel.Satis);
            }
            else if (model.SuSatisModel.Satis != null && model.SuSatisModel.Satis.SatisTipi == enumSatisTipi.SatisIptal.GetHashCode())
            {
                model.SuSatisModel.Satis.GUNCELLEYEN = AktifKullanici.KayitNo;
                model.SuSatisModel.Satis = _entSatisService.Guncelle(model.SuSatisModel.Satis);
            }

            if (model.KalorimetreSatisModel.Satis != null && model.KalorimetreSatisModel.Satis.SatisTipi == enumSatisTipi.Satis.GetHashCode() )
            {
                if (model.KalorimetreSatisModel.Satis.KAYITNO == 0 && model.KalorimetreSatisModel.Satis.ODEME > 0)
                {

                    model.KalorimetreSatisModel.Satis.OLUSTURAN = AktifKullanici.KayitNo;
                    model.KalorimetreSatisModel.Satis = _entSatisService.Ekle(model.KalorimetreSatisModel.Satis);
                    var abone = _entAboneService.GetirById(model.KalorimetreSatisModel.Satis.ABONEKAYITNO);
                    abone.SonSatisTarih = DateTime.Now;
                    _entAboneService.Guncelle(abone.List());
                }
            }
            else if (model.KalorimetreSatisModel.Satis != null && model.KalorimetreSatisModel.Satis.SatisTipi == enumSatisTipi.BedelsizSatis.GetHashCode())
            {
                model.KalorimetreSatisModel.Satis.OLUSTURAN = AktifKullanici.KayitNo;
                model.KalorimetreSatisModel.Satis = _entSatisService.Ekle(model.KalorimetreSatisModel.Satis);

            }
            else if (model.KalorimetreSatisModel.Satis != null && model.KalorimetreSatisModel.Satis.SatisTipi == enumSatisTipi.SatisIptal.GetHashCode())
            {
                model.KalorimetreSatisModel.Satis.GUNCELLEYEN = AktifKullanici.KayitNo;
                model.KalorimetreSatisModel.Satis = _entSatisService.Guncelle(model.KalorimetreSatisModel.Satis);

            }

            // return Json(model, JsonRequestBehavior.AllowGet);
            return Json(MakbuzOlustur(model), JsonRequestBehavior.AllowGet);
        }

        public string MakbuzOlustur(SatisModel model)
        {

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "Report.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }

            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                     "  <OutputFormat>PDF</OutputFormat>" +
                     "  <PageWidth>8.27in</PageWidth>" +
                     "  <PageHeight>11.69in</PageHeight>" +
                     "  <MarginTop>0.25in</MarginTop>" +
                     "  <MarginLeft>0.4in</MarginLeft>" +
                     "  <MarginRight>0in</MarginRight>" +
                     "  <MarginBottom>0.25in</MarginBottom>" +
                     "  <EmbedFonts>None</EmbedFonts>" +
                     "</DeviceInfo>";


            Warning[] warning;
            string[] streams;
            byte[] renderedBytes;

            Microsoft.Reporting.WebForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WebForms.ReportDataSource();
            List<YesilVadiMakbuzBilgileri> dataSource1 = new List<YesilVadiMakbuzBilgileri>();
            YesilVadiMakbuzBilgileri makbuzBilgileri = new YesilVadiMakbuzBilgileri();

            Microsoft.Reporting.WebForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WebForms.ReportDataSource();
            List<KalorimetreSatisBilgileri> dataSource2 = new List<KalorimetreSatisBilgileri>();
            KalorimetreSatisBilgileri kalorimetreSatisBilgileri = new KalorimetreSatisBilgileri();

            Microsoft.Reporting.WebForms.ReportDataSource reportDataSource3 = new Microsoft.Reporting.WebForms.ReportDataSource();
            List<SuSatisBilgileri> dataSource3 = new List<SuSatisBilgileri>();
            SuSatisBilgileri suSatisBilgileri = new SuSatisBilgileri();

            Microsoft.Reporting.WebForms.ReportDataSource reportDataSource4 = new Microsoft.Reporting.WebForms.ReportDataSource();
            List<SatisBilgileri> dataSource4 = new List<SatisBilgileri>();
            SatisBilgileri satisBilgileri = new SatisBilgileri();

            if (model.SuSatisModel.Satis!=null && model.SuSatisModel.Satis.KAYITNO > 0)
            {
                ENTSATISDetay suSatisDetay = _entSatisService.DetayGetirById(model.SuSatisModel.Satis.KAYITNO);
                var aboneSayacSu = _entAboneSayacService.DetayGetir(new ENTABONESAYACAra { SAYACKAYITNO = suSatisDetay.SAYACKAYITNO, Durum = 1 }).FirstOrDefault();
                if (aboneSayacSu.TARIFEKAYITNO == null)
                {
                    throw new NotificationException("Makbuz tarife bilgisi çekilemedi");
                }
                var tarifeSu = _prmTarifeSuService.GetirById(aboneSayacSu.TARIFEKAYITNO.Value);
                if (suSatisDetay != null)
                {
                    makbuzBilgileri.AboneAdiSoyadi = suSatisDetay.AboneAdSoyad;
                    makbuzBilgileri.AboneNo = suSatisDetay.AboneNo;
                    makbuzBilgileri.FaturaTarihi = suSatisDetay.OLUSTURMATARIH.ToString();
                    makbuzBilgileri.SuSayacNo = suSatisDetay.KapakSeriNo.ToString();

                    suSatisBilgileri.Tarih = suSatisDetay.OLUSTURMATARIH.ToString();
                    suSatisBilgileri.SayacTuru = "SU";
                    suSatisBilgileri.KontorMiktar = suSatisDetay.KREDI.ToString();

                    suSatisBilgileri.BakimBedeli = suSatisDetay.AylikBakimBedeli.ToString();
                    suSatisBilgileri.Ctv = suSatisDetay.Ctv.ToString();
                    suSatisBilgileri.Kdv = suSatisDetay.Kdv.ToString();
                    suSatisBilgileri.Tutar = suSatisDetay.SatisTutarı == null ? null : suSatisDetay.SatisTutarı.ToString();
                    suSatisBilgileri.TotalTutar = suSatisDetay.ODEME.ToString();
                    suSatisBilgileri.BirimFiyat = tarifeSu.BIRIMFIYAT.ToString();

                }

            }



            if (model.KalorimetreSatisModel.Satis!=null && model.KalorimetreSatisModel.Satis.KAYITNO > 0)
            {
                ENTSATISDetay kalorimetreSatisDetay = _entSatisService.DetayGetirById(model.KalorimetreSatisModel.Satis.KAYITNO);
                var aboneSayacKalorimetre = _entAboneSayacService.DetayGetir(new ENTABONESAYACAra { SAYACKAYITNO = kalorimetreSatisDetay.SAYACKAYITNO, Durum = 1 }).FirstOrDefault();
                if (aboneSayacKalorimetre.TARIFEKAYITNO == null)
                {
                    throw new NotificationException("Makbuz tarife bilgisi çekilemedi");
                }
                var tarifeKalorimetre = _prmKALORIMETREService.GetirById(aboneSayacKalorimetre.TARIFEKAYITNO.Value);
                if (kalorimetreSatisDetay != null)
                {
                    makbuzBilgileri.AboneAdiSoyadi = kalorimetreSatisDetay.AboneAdSoyad;
                    makbuzBilgileri.AboneNo = kalorimetreSatisDetay.AboneNo;
                    makbuzBilgileri.FaturaTarihi = kalorimetreSatisDetay.OLUSTURMATARIH.ToString();
                    makbuzBilgileri.KalorimetreNo = kalorimetreSatisDetay.KapakSeriNo.ToString();

                    kalorimetreSatisBilgileri.Tarih = kalorimetreSatisDetay.OLUSTURMATARIH.ToString();
                    kalorimetreSatisBilgileri.SayacTuru = "KALORİMETRE";
                    kalorimetreSatisBilgileri.KontorMiktar = kalorimetreSatisDetay.KREDI.ToString();

                    kalorimetreSatisBilgileri.BakimBedeli = kalorimetreSatisDetay.AylikBakimBedeli.ToString();
                    kalorimetreSatisBilgileri.Ctv = kalorimetreSatisDetay.Ctv.ToString();
                    kalorimetreSatisBilgileri.Kdv = kalorimetreSatisDetay.Kdv.ToString();
                    kalorimetreSatisBilgileri.Tutar = kalorimetreSatisDetay.SatisTutarı == null ? null : kalorimetreSatisDetay.SatisTutarı.ToString();
                    kalorimetreSatisBilgileri.TotalTutar = kalorimetreSatisDetay.ODEME.ToString();
                    kalorimetreSatisBilgileri.BirimFiyat = tarifeKalorimetre.BirimFiyat.ToString();

                }
            }






            satisBilgileri.SatisTutari = (kalorimetreSatisBilgileri.Tutar.ToDecimal() + suSatisBilgileri.Tutar.ToDecimal()).ToString();
            satisBilgileri.BakimHizmetleriBedeli = (kalorimetreSatisBilgileri.BakimBedeli.ToDecimal() + suSatisBilgileri.BakimBedeli.ToDecimal()).ToString();
            satisBilgileri.CtvBedeli = (kalorimetreSatisBilgileri.Ctv.ToDecimal() + suSatisBilgileri.Ctv.ToDecimal()).ToString();
            satisBilgileri.KdvBedeli = (kalorimetreSatisBilgileri.Kdv.ToDecimal() + suSatisBilgileri.Kdv.ToDecimal()).ToString();
            satisBilgileri.GenelToplam = (kalorimetreSatisBilgileri.TotalTutar.ToDecimal() + suSatisBilgileri.TotalTutar.ToDecimal()).ToString();

            dataSource1.Add(makbuzBilgileri);
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = dataSource1;
            lr.DataSources.Add(reportDataSource1);

            dataSource2.Add(kalorimetreSatisBilgileri);
            reportDataSource2.Name = "DataSet2";
            reportDataSource2.Value = dataSource2;
            lr.DataSources.Add(reportDataSource2);

            dataSource3.Add(suSatisBilgileri);
            reportDataSource3.Name = "DataSet3";
            reportDataSource3.Value = dataSource3;
            lr.DataSources.Add(reportDataSource3);

            dataSource4.Add(satisBilgileri);
            reportDataSource4.Name = "DataSet4";
            reportDataSource4.Value = dataSource4;
            lr.DataSources.Add(reportDataSource4);





            renderedBytes = lr.Render("PDF", deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warning);

            string filename = makbuzBilgileri.AboneNo + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + ".pdf";
            path = Server.MapPath("~/App_Data/" + filename);
            System.IO.File.WriteAllBytes(path, renderedBytes);

            return filename;
        }
        public static int ToInt32(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;
            return int.Parse(value, (IFormatProvider)CultureInfo.CurrentCulture);
        }
        public ActionResult SatistanMakbuzOlustur(int satisKayitNo, int sayacKayitNo)
        {
            var sayac = _entSayacService.DetayGetirById(sayacKayitNo);
            ENTSATISDetay suSatisDetay = new ENTSATISDetay();
            ENTSATISDetay kalorimetreSatisDetay = new ENTSATISDetay();
            PRMTARIFESU tarifeSu = new PRMTARIFESU();
            PRMTARIFEKALORIMETRE tarifeKalorimetre = new PRMTARIFEKALORIMETRE();

            if (sayac.SayacTuru == 1)
            {
                suSatisDetay = _entSatisService.DetayGetirById(satisKayitNo);
                var aboneSayacSu = _entAboneSayacService.DetayGetir(new ENTABONESAYACAra { SAYACKAYITNO = suSatisDetay.SAYACKAYITNO, Durum = 1 }).FirstOrDefault();
                if (aboneSayacSu.TARIFEKAYITNO == null)
                {
                    throw new NotificationException("Makbuz tarife bilgisi çekilemedi");
                }
                tarifeSu = _prmTarifeSuService.GetirById(aboneSayacSu.TARIFEKAYITNO.Value);
            }
            else if (sayac.SayacTuru == 2)
            {
                kalorimetreSatisDetay = _entSatisService.DetayGetirById(satisKayitNo);
                var aboneSayacKalorimetre = _entAboneSayacService.DetayGetir(new ENTABONESAYACAra { SAYACKAYITNO = kalorimetreSatisDetay.SAYACKAYITNO, Durum = 1 }).FirstOrDefault();
                if (aboneSayacKalorimetre.TARIFEKAYITNO == null)
                {
                    throw new NotificationException("Makbuz tarife bilgisi çekilemedi");
                }
                tarifeKalorimetre = _prmKALORIMETREService.GetirById(aboneSayacKalorimetre.TARIFEKAYITNO.Value);
            }



            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "Report.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }

            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                     "  <OutputFormat>PDF</OutputFormat>" +
                     "  <PageWidth>8.27in</PageWidth>" +
                     "  <PageHeight>11.69in</PageHeight>" +
                     "  <MarginTop>0.25in</MarginTop>" +
                     "  <MarginLeft>0.4in</MarginLeft>" +
                     "  <MarginRight>0in</MarginRight>" +
                     "  <MarginBottom>0.25in</MarginBottom>" +
                     "  <EmbedFonts>None</EmbedFonts>" +
                     "</DeviceInfo>";


            Warning[] warning;
            string[] streams;
            byte[] renderedBytes;

            Microsoft.Reporting.WebForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WebForms.ReportDataSource();
            List<YesilVadiMakbuzBilgileri> dataSource1 = new List<YesilVadiMakbuzBilgileri>();
            YesilVadiMakbuzBilgileri makbuzBilgileri = new YesilVadiMakbuzBilgileri();

            Microsoft.Reporting.WebForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WebForms.ReportDataSource();
            List<KalorimetreSatisBilgileri> dataSource2 = new List<KalorimetreSatisBilgileri>();
            KalorimetreSatisBilgileri kalorimetreSatisBilgileri = new KalorimetreSatisBilgileri();

            Microsoft.Reporting.WebForms.ReportDataSource reportDataSource3 = new Microsoft.Reporting.WebForms.ReportDataSource();
            List<SuSatisBilgileri> dataSource3 = new List<SuSatisBilgileri>();
            SuSatisBilgileri suSatisBilgileri = new SuSatisBilgileri();

            Microsoft.Reporting.WebForms.ReportDataSource reportDataSource4 = new Microsoft.Reporting.WebForms.ReportDataSource();
            List<SatisBilgileri> dataSource4 = new List<SatisBilgileri>();
            SatisBilgileri satisBilgileri = new SatisBilgileri();

            if (suSatisDetay.KAYITNO > 0)
            {
                makbuzBilgileri.AboneAdiSoyadi = suSatisDetay.AboneAdSoyad;
                makbuzBilgileri.AboneNo = suSatisDetay.AboneNo;
                makbuzBilgileri.FaturaTarihi = suSatisDetay.OLUSTURMATARIH.ToString();
                makbuzBilgileri.SuSayacNo = suSatisDetay.KapakSeriNo.ToString();

                suSatisBilgileri.Tarih = suSatisDetay.OLUSTURMATARIH.ToString();
                suSatisBilgileri.SayacTuru = "SU";
                suSatisBilgileri.KontorMiktar = suSatisDetay.KREDI.ToString();

                suSatisBilgileri.BakimBedeli = suSatisDetay.AylikBakimBedeli.ToString();
                suSatisBilgileri.Ctv = suSatisDetay.Ctv.ToString();
                suSatisBilgileri.Kdv = suSatisDetay.Kdv.ToString();
                suSatisBilgileri.Tutar = suSatisDetay.SatisTutarı.ToString();
                suSatisBilgileri.TotalTutar = suSatisDetay.ODEME.ToString();
                suSatisBilgileri.BirimFiyat = tarifeSu.BIRIMFIYAT.ToString();
            }
            if (kalorimetreSatisDetay.KAYITNO > 0)
            {
                makbuzBilgileri.AboneAdiSoyadi = kalorimetreSatisDetay.AboneAdSoyad;
                makbuzBilgileri.AboneNo = kalorimetreSatisDetay.AboneNo;
                makbuzBilgileri.FaturaTarihi = kalorimetreSatisDetay.OLUSTURMATARIH.ToString();
                makbuzBilgileri.KalorimetreNo = kalorimetreSatisDetay.KapakSeriNo.ToString();

                kalorimetreSatisBilgileri.Tarih = kalorimetreSatisDetay.OLUSTURMATARIH.ToString();
                kalorimetreSatisBilgileri.SayacTuru = "KALORİMETRE";
                kalorimetreSatisBilgileri.KontorMiktar = kalorimetreSatisDetay.KREDI.ToString();

                kalorimetreSatisBilgileri.BakimBedeli = kalorimetreSatisDetay.AylikBakimBedeli.ToString();
                kalorimetreSatisBilgileri.Ctv = kalorimetreSatisDetay.Ctv.ToString();
                kalorimetreSatisBilgileri.Kdv = kalorimetreSatisDetay.Kdv.ToString();
                kalorimetreSatisBilgileri.Tutar = kalorimetreSatisDetay.SatisTutarı.ToString();
                kalorimetreSatisBilgileri.TotalTutar = kalorimetreSatisDetay.ODEME.ToString();
                kalorimetreSatisBilgileri.BirimFiyat = tarifeKalorimetre.BirimFiyat.ToString();


            }

            satisBilgileri.SatisTutari = (kalorimetreSatisBilgileri.Tutar.ToDecimal() + suSatisBilgileri.Tutar.ToDecimal()).ToString();
            satisBilgileri.BakimHizmetleriBedeli = (kalorimetreSatisBilgileri.BakimBedeli.ToDecimal() + suSatisBilgileri.BakimBedeli.ToDecimal()).ToString();
            satisBilgileri.CtvBedeli = (kalorimetreSatisBilgileri.Ctv.ToDecimal() + suSatisBilgileri.Ctv.ToDecimal()).ToString();
            satisBilgileri.KdvBedeli = (kalorimetreSatisBilgileri.Kdv.ToDecimal() + suSatisBilgileri.Kdv.ToDecimal()).ToString();
            satisBilgileri.GenelToplam = (kalorimetreSatisBilgileri.TotalTutar.ToDecimal() + suSatisBilgileri.TotalTutar.ToDecimal()).ToString();

            dataSource1.Add(makbuzBilgileri);
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = dataSource1;
            lr.DataSources.Add(reportDataSource1);

            dataSource2.Add(kalorimetreSatisBilgileri);
            reportDataSource2.Name = "DataSet2";
            reportDataSource2.Value = dataSource2;
            lr.DataSources.Add(reportDataSource2);

            dataSource3.Add(suSatisBilgileri);
            reportDataSource3.Name = "DataSet3";
            reportDataSource3.Value = dataSource3;
            lr.DataSources.Add(reportDataSource3);

            dataSource4.Add(satisBilgileri);
            reportDataSource4.Name = "DataSet4";
            reportDataSource4.Value = dataSource4;
            lr.DataSources.Add(reportDataSource4);





            renderedBytes = lr.Render("PDF", deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warning);

            string filename = makbuzBilgileri.AboneNo + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + ".pdf";
            path = Server.MapPath("~/App_Data/" + filename);
            System.IO.File.WriteAllBytes(path, renderedBytes);



            byte[] bytes = System.IO.File.ReadAllBytes(Server.MapPath("~/App_Data/" + filename));
            return File(bytes, "PDF", filename);
        }


        #region GazSatis
        public JsonResult GazHamdataPars(SatisModel model)
        {
            if (string.IsNullOrEmpty(model.HamData))
            {
                model.GazSatisModel.GazOkunan.HamData = "1 | 20170608 | 123 | b | 0 | b | 0 | 0 | 0 | 01.01.2000 00:00 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0,000 | 0,000 | 0 | Close | 01.01.2000 | 01.01.2000 | 01.01.2000 | 01.01.2000 | 01.01.2000 | 01.01.2000 | 01.01.2000 | 01.01.2000 | *| *| *| *| *| *| *| *| 0 | 0 | 999999 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 0 | 10 | 17 | 1 | 20 | 16.01.2000 | *| *| *| *| 0 | 0 | 3";
            }
            else
            {
                model.GazSatisModel.GazOkunan.HamData = model.HamData;
            }
            model.GazSatisModel.GazOkunan.ParsData();

            var gazSayac = _entSayacService.Getir(new ENTSAYACAra { SERINO = model.GazSatisModel.GazOkunan.CihazNo.Trim(), SayacTuru = 4 }).FirstOrDefault();//Gaz Sayacı
            if (gazSayac == null)
            {
                throw new NotificationException("Gaz Sayacı Bulunamadı");
            }

            model.GazSatisModel.AboneSayacDetay = _entAboneSayacService.DetayGetir(new ENTABONESAYACAra { SAYACKAYITNO = gazSayac.KAYITNO, Durum = 1 }).FirstOrDefault();


            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GazHesapla(SatisModel model)
        {
            try
            {
                model.GazSatisModel.PrmTarifeGazDetay = _prmTarifeGazService.DetayGetir(new PRMTARIFEGAZAra { KAYITNO = model.GazSatisModel.AboneSayacDetay.TARIFEKAYITNO }).FirstOrDefault();

                if (model.GazSatisModel.PrmTarifeGazDetay == null)
                {
                    throw new NotificationException("Tarife bilgileri çekilemedi");
                }

                if (model.GazSatisModel.Satis.ODEME == 0)
                {
                    throw new NotificationException("Tutar 0 dan farklı olmalıdır");
                }

                if (model.GazSatisModel.PrmTarifeGazDetay.TuketimKatsayi == 0)
                {
                    throw new NotificationException("Tarife tüketim katsayı 0 dan farklı olmalıdır");
                }

                if (model.GazSatisModel.PrmTarifeGazDetay.BIRIMFIYAT == 0)
                {
                    throw new NotificationException("Tarife birim fiyat 0 dan farklı olmalıdır");

                }


                model.GazSatisModel.Satis.Ctv = model.GazSatisModel.PrmTarifeGazDetay.Ctv * model.GazSatisModel.Satis.ODEME;
                model.GazSatisModel.Satis.Kdv = (model.GazSatisModel.Satis.ODEME - model.GazSatisModel.Satis.Ctv) * model.GazSatisModel.PrmTarifeGazDetay.Kdv;
                model.GazSatisModel.Satis.SatisTutari = (model.GazSatisModel.Satis.ODEME - model.GazSatisModel.Satis.Ctv - model.GazSatisModel.Satis.Kdv);

                model.GazSatisModel.Satis.YEDEKKREDI = (model.GazSatisModel.PrmTarifeGazDetay.YEDEKKREDI * model.GazSatisModel.PrmTarifeGazDetay.TuketimKatsayi);
                model.GazSatisModel.Satis.KREDI = ((model.GazSatisModel.Satis.SatisTutari * model.GazSatisModel.PrmTarifeGazDetay.TuketimKatsayi) / model.GazSatisModel.PrmTarifeGazDetay.BIRIMFIYAT);
                //model.SuSatisModel.Satis.KREDI = Math.Round(Convert.ToDecimal( model.SuSatisModel.Satis.KREDI), 2);
                model.GazSatisModel.Satis.KREDI = Math.Floor(Convert.ToDecimal(model.GazSatisModel.Satis.KREDI));

                if (model.GazSatisModel.Satis.KREDI < model.GazSatisModel.Satis.YEDEKKREDI)
                {
                    throw new NotificationException("Yedek kredi den az miktarda satış yapılamaz");
                }
                if (model.GazSatisModel.GazOkunan.Ako == "b" && model.GazSatisModel.GazOkunan.Yko == "b")
                {
                    model.GazSatisModel.Satis.ToplamKredi = Convert.ToInt32(model.GazSatisModel.GazOkunan.Kredi) + Convert.ToInt32(model.GazSatisModel.GazOkunan.YedekKredi) + model.GazSatisModel.Satis.KREDI - model.GazSatisModel.Satis.YEDEKKREDI;
                    model.GazSatisModel.Satis.YEDEKKREDI = model.GazSatisModel.Satis.YEDEKKREDI;
                }
                else if (model.GazSatisModel.GazOkunan.Ako == "*" && model.GazSatisModel.GazOkunan.Yko == "b")
                {
                    model.GazSatisModel.Satis.ToplamKredi = Convert.ToInt32(model.GazSatisModel.GazOkunan.YedekKredi) + model.GazSatisModel.Satis.KREDI - model.GazSatisModel.Satis.YEDEKKREDI;
                    model.GazSatisModel.Satis.YEDEKKREDI = model.GazSatisModel.Satis.YEDEKKREDI;

                }
                else if (model.GazSatisModel.GazOkunan.Ako == "*" && model.GazSatisModel.GazOkunan.Yko == "*")
                {

                    model.GazSatisModel.Satis.ToplamKredi = model.GazSatisModel.Satis.KREDI - model.GazSatisModel.Satis.YEDEKKREDI;
                    model.GazSatisModel.Satis.YEDEKKREDI = model.GazSatisModel.Satis.YEDEKKREDI;
                }



                model.GazSatisModel.Satis.ABONEKAYITNO = model.GazSatisModel.AboneSayacDetay.ABONEKAYITNO;
                model.GazSatisModel.Satis.SAYACKAYITNO = model.GazSatisModel.AboneSayacDetay.SAYACKAYITNO;


            }
            catch (Exception ex)
            {
                throw new NotificationException(ex.Message);
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GazSatisYap(SatisModel model)
        {
            if (model.GazSatisModel.Satis.SatisTipi == enumSatisTipi.Satis.GetHashCode())
            {
                if (model.GazSatisModel.Satis.ODEME > 0)
                {

                    model.GazSatisModel.Satis.OLUSTURAN = AktifKullanici.KayitNo;
                    model.GazSatisModel.Satis = _entSatisService.Ekle(model.GazSatisModel.Satis);

                    var abone = _entAboneService.GetirById(model.GazSatisModel.Satis.ABONEKAYITNO);
                    abone.SonSatisTarih = DateTime.Now;
                    _entAboneService.Guncelle(abone.List());
                }
            }
            else if (model.GazSatisModel.Satis.SatisTipi == enumSatisTipi.BedelsizSatis.GetHashCode())
            {
                model.GazSatisModel.Satis.OLUSTURAN = AktifKullanici.KayitNo;
                model.GazSatisModel.Satis = _entSatisService.Ekle(model.SuSatisModel.Satis);
            }
            else if (model.SuSatisModel.Satis.SatisTipi == enumSatisTipi.SatisIptal.GetHashCode())
            {
                model.GazSatisModel.Satis.OLUSTURAN = AktifKullanici.KayitNo;
                model.GazSatisModel.Satis = _entSatisService.Ekle(model.SuSatisModel.Satis);
            }


            return Json(GazMakbuzOlustur(model), JsonRequestBehavior.AllowGet);
        }

        public string GazMakbuzOlustur(SatisModel model)
        {

            ENTSATISDetay suSatisDetay = _entSatisService.DetayGetirById(model.GazSatisModel.Satis.KAYITNO); //_entSatisService.DetayGetirById(model.SuSatisModel.Satis.KAYITNO);
            //ENTSATISDetay kalorimetreSatisDetay = _entSatisService.DetayGetirById(model.KalorimetreSatisModel.Satis.KAYITNO);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "Report.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }

            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                     "  <OutputFormat>PDF</OutputFormat>" +
                     "  <PageWidth>8.27in</PageWidth>" +
                     "  <PageHeight>11.69in</PageHeight>" +
                     "  <MarginTop>0.25in</MarginTop>" +
                     "  <MarginLeft>0.4in</MarginLeft>" +
                     "  <MarginRight>0in</MarginRight>" +
                     "  <MarginBottom>0.25in</MarginBottom>" +
                     "  <EmbedFonts>None</EmbedFonts>" +
                     "</DeviceInfo>";


            Warning[] warning;
            string[] streams;
            byte[] renderedBytes;

            Microsoft.Reporting.WebForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WebForms.ReportDataSource();
            List<YesilVadiMakbuzBilgileri> dataSource1 = new List<YesilVadiMakbuzBilgileri>();
            YesilVadiMakbuzBilgileri makbuzBilgileri = new YesilVadiMakbuzBilgileri();

            Microsoft.Reporting.WebForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WebForms.ReportDataSource();
            List<KalorimetreSatisBilgileri> dataSource2 = new List<KalorimetreSatisBilgileri>();
            KalorimetreSatisBilgileri kalorimetreSatisBilgileri = new KalorimetreSatisBilgileri();

            Microsoft.Reporting.WebForms.ReportDataSource reportDataSource3 = new Microsoft.Reporting.WebForms.ReportDataSource();
            List<SuSatisBilgileri> dataSource3 = new List<SuSatisBilgileri>();
            SuSatisBilgileri suSatisBilgileri = new SuSatisBilgileri();

            Microsoft.Reporting.WebForms.ReportDataSource reportDataSource4 = new Microsoft.Reporting.WebForms.ReportDataSource();
            List<SatisBilgileri> dataSource4 = new List<SatisBilgileri>();
            SatisBilgileri satisBilgileri = new SatisBilgileri();

            if (suSatisDetay != null)
            {
                makbuzBilgileri.AboneAdiSoyadi = suSatisDetay.AboneAdSoyad;
                makbuzBilgileri.AboneNo = suSatisDetay.AboneNo;
                makbuzBilgileri.FaturaTarihi = suSatisDetay.OLUSTURMATARIH.ToString();
                makbuzBilgileri.SuSayacNo = suSatisDetay.KapakSeriNo.ToString();

                suSatisBilgileri.Tarih = suSatisDetay.OLUSTURMATARIH.ToString();
                suSatisBilgileri.SayacTuru = "GAZ";
                suSatisBilgileri.KontorMiktar = suSatisDetay.KREDI.ToString();

                suSatisBilgileri.BakimBedeli = suSatisDetay.AylikBakimBedeli.ToString();
                suSatisBilgileri.Ctv = suSatisDetay.Ctv.ToString();
                suSatisBilgileri.Kdv = suSatisDetay.Kdv.ToString();
                suSatisBilgileri.Tutar = suSatisDetay.SatisTutarı.ToString();
                suSatisBilgileri.TotalTutar = suSatisDetay.ODEME.ToString();

            }
            //if (kalorimetreSatisDetay != null)
            //{
            //    makbuzBilgileri.AboneAdiSoyadi = kalorimetreSatisDetay.AboneAdSoyad;
            //    makbuzBilgileri.AboneNo = kalorimetreSatisDetay.AboneNo;
            //    makbuzBilgileri.FaturaTarihi = kalorimetreSatisDetay.OLUSTURMATARIH.ToString();
            //    makbuzBilgileri.KalorimetreNo = kalorimetreSatisDetay.KapakSeriNo.ToString();

            //    kalorimetreSatisBilgileri.Tarih = kalorimetreSatisDetay.OLUSTURMATARIH.ToString();
            //    kalorimetreSatisBilgileri.SayacTuru = "KALORİMETRE";
            //    kalorimetreSatisBilgileri.KontorMiktar = kalorimetreSatisDetay.KREDI.ToString();

            //    kalorimetreSatisBilgileri.BakimBedeli = kalorimetreSatisDetay.AylikBakimBedeli.ToString();
            //    kalorimetreSatisBilgileri.Ctv = kalorimetreSatisDetay.Ctv.ToString();
            //    kalorimetreSatisBilgileri.Kdv = kalorimetreSatisDetay.Kdv.ToString();
            //    kalorimetreSatisBilgileri.Tutar = kalorimetreSatisDetay.SatisTutarı.ToString();
            //    kalorimetreSatisBilgileri.TotalTutar = kalorimetreSatisDetay.ODEME.ToString();


            //}

            satisBilgileri.SatisTutari = (kalorimetreSatisBilgileri.Tutar.ToDecimal() + suSatisBilgileri.Tutar.ToDecimal()).ToString();
            satisBilgileri.BakimHizmetleriBedeli = (kalorimetreSatisBilgileri.BakimBedeli.ToDecimal() + suSatisBilgileri.BakimBedeli.ToDecimal()).ToString();
            satisBilgileri.CtvBedeli = (kalorimetreSatisBilgileri.Ctv.ToDecimal() + suSatisBilgileri.Ctv.ToDecimal()).ToString();
            satisBilgileri.KdvBedeli = (kalorimetreSatisBilgileri.Kdv.ToDecimal() + suSatisBilgileri.Kdv.ToDecimal()).ToString();
            satisBilgileri.GenelToplam = (kalorimetreSatisBilgileri.TotalTutar.ToDecimal() + suSatisBilgileri.TotalTutar.ToDecimal()).ToString();

            dataSource1.Add(makbuzBilgileri);
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = dataSource1;
            lr.DataSources.Add(reportDataSource1);

            dataSource2.Add(kalorimetreSatisBilgileri);
            reportDataSource2.Name = "DataSet2";
            reportDataSource2.Value = dataSource2;
            lr.DataSources.Add(reportDataSource2);

            dataSource3.Add(suSatisBilgileri);
            reportDataSource3.Name = "DataSet3";
            reportDataSource3.Value = dataSource3;
            lr.DataSources.Add(reportDataSource3);

            dataSource4.Add(satisBilgileri);
            reportDataSource4.Name = "DataSet4";
            reportDataSource4.Value = dataSource4;
            lr.DataSources.Add(reportDataSource4);





            renderedBytes = lr.Render("PDF", deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warning);

            string filename = makbuzBilgileri.AboneNo + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + ".pdf";
            path = Server.MapPath("~/App_Data/" + filename);
            System.IO.File.WriteAllBytes(path, renderedBytes);

            return filename;
        }
        #endregion
    }
}