using Microsoft.Reporting.WebForms;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.ENTABONESAYACComplexTypes;
using OsosOracle.Entities.ComplexType.ENTSATISComplexTypes;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.MvcUI.Filters;
using OsosOracle.MvcUI.Models.ENTSATISModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsosOracle.MvcUI.Resources;
using OsosOracle.MvcUI.Models.ENTABONESAYACModels;
using OsosOracle.MvcUI.Models.ENTSATISModels.Yeni;
using OsosOracle.Entities.ComplexType.PRMTARIFESUComplexTypes;
using System.IO;
using OsosOracle.MvcUI.Reports.ReportModel;
using OsosOracle.Entities.Enums;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class SatisController : BaseController
    {
        private readonly IENTSAYACService _entSayacService;
        private readonly IENTSATISService _entSatisService;
        private readonly IENTABONESAYACService _entAboneSayacService;
        private readonly IENTABONEService _entAboneService;
        private readonly IPRMTARIFESUService _prmTarifeSuService;
        private readonly IPRMTARIFEKALORIMETREService _prmTarifeKalorimetreService;
        public SatisController(IENTSATISService entSatisService,
            IENTSAYACService entSayacService,
            IENTABONESAYACService entAboneSayacService,
            IENTABONEService entAboneService,
            IPRMTARIFEKALORIMETREService prmKALORIMETREService,
            IPRMTARIFESUService prmTarifeSuService)
        {
            _entSatisService = entSatisService;
            _entSayacService = entSayacService;
            _entAboneSayacService = entAboneSayacService;
            _entAboneService = entAboneService;
            _prmTarifeKalorimetreService = prmKALORIMETREService;
            _prmTarifeSuService = prmTarifeSuService;

        }
        public ActionResult Index()
        {
            ENTSATISIndexModel model = new ENTSATISIndexModel { ENTSATISAra = new ENTSATISAra { SatisTarihBaslangic = DateTime.Now.AddDays(-30), SatisTarihBitis = DateTime.Now.AddDays(30) } };
            return View(model);
        }

        public ActionResult SatisIptal()
        {
            return View(new SatisModel());
        }
        public JsonResult SatisPars(SatisModel model)
        {

            if (string.IsNullOrEmpty(model.HamData))
            {
                model.HamData = "1#123213#65#44444444#b#b#b#b#b#b#0#0#0#0#0#00/00/2000#00/00/2000#00/00/2000#00/00/2000#00/00/2000#0#0#0#0#0#0#0#0#0#*#1#20#1#9999#9999#0#         0_         0#   0,000#0#0#0#00/00/2000#00/00/2000#00/00/2000#00/00/2000#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#00/00/2000 00:00#2#00/00/2000 - 0 - 0#00/00/2000 - 0 - 0#00/00/2000 - 0 - 0#00/00/2000#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#1#False#0#9999#9999#0#0#1#1#1#1#1#1#0#1000#2000#3000#4000#5000#*#0#00/00/2000#0#0#1";
            }
            model.SuSatisModel.SogukSuOkunan = ParsHamData(model.HamData);
            model.SuSatisModel.AboneSayacDetay = _entAboneSayacService.DetayGetir(new ENTABONESAYACAra { SayacSeriNo = model.SuSatisModel.SogukSuOkunan.SayacSeriNo, SayacTur = 1, Durum = 1 }).FirstOrDefault();
            model.SuSatisModel.PrmTarifeSuDetay = _prmTarifeSuService.DetayGetir(new PRMTARIFESUAra { KAYITNO = model.SuSatisModel.AboneSayacDetay.TARIFEKAYITNO }).FirstOrDefault();



            return Json(model);
        }
        public JsonResult SatisIptalPars(SatisModel model)
        {
            if (string.IsNullOrEmpty(model.HamData))
            {
                model.HamData = "1#123213#65#44444444#b#b#b#b#b#b#0#0#0#0#0#00/00/2000#00/00/2000#00/00/2000#00/00/2000#00/00/2000#0#0#0#0#0#0#0#0#0#*#1#20#1#9999#9999#0#         0_         0#   0,000#0#0#0#00/00/2000#00/00/2000#00/00/2000#00/00/2000#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#00/00/2000 00:00#2#00/00/2000 - 0 - 0#00/00/2000 - 0 - 0#00/00/2000 - 0 - 0#00/00/2000#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#1#False#0#9999#9999#0#0#1#1#1#1#1#1#0#1000#2000#3000#4000#5000#*#0#00/00/2000#0#0#1";
            }
            model.SuSatisModel.SogukSuOkunan = ParsHamData(model.HamData);
            model.SuSatisModel.AboneSayacDetay = _entAboneSayacService.DetayGetir(new ENTABONESAYACAra { SayacSeriNo = model.SuSatisModel.SogukSuOkunan.SayacSeriNo, SayacTur = 1, Durum = 1 }).FirstOrDefault();
            model.SuSatisModel.PrmTarifeSuDetay = _prmTarifeSuService.DetayGetir(new PRMTARIFESUAra { KAYITNO = model.SuSatisModel.AboneSayacDetay.TARIFEKAYITNO }).FirstOrDefault();
            model.SuSatisModel.Satis = _entSatisService.Getir(new ENTSATISAra { ABONEKAYITNO = model.SuSatisModel.AboneSayacDetay.ABONEKAYITNO, SAYACKAYITNO = model.SuSatisModel.AboneSayacDetay.SAYACKAYITNO, SatisTipi = enumSatisTipi.Satis.GetHashCode() }).OrderByDescending(x => x.OLUSTURMATARIH).FirstOrDefault();

            if (model.SuSatisModel.Satis != null)
            {
                model.SuSatisModel.SatisIptal.AnaKredi = Convert.ToInt32(model.SuSatisModel.SogukSuOkunan.Kredi) - Convert.ToInt32(model.SuSatisModel.Satis.KREDI);
                model.SuSatisModel.SatisIptal.YedekKredi = Convert.ToInt32(model.SuSatisModel.SogukSuOkunan.YedekKredi) - Convert.ToInt32(model.SuSatisModel.Satis.YEDEKKREDI);

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


            if (model.SuSatisModel.SogukSuOkunan.Ako == "*")
            {
                throw new NotificationException("Kartta iptal edilecek satış bulunmuyor");
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult SatisPartial(PartialModel partialModel)
        {
            SayfaBaslik("Satış Bilgileri");
            if (partialModel.EntSatisAra == null)
            {
                partialModel.EntSatisAra = new ENTSATISAra { ABONEKAYITNO = partialModel.AboneKayitNo };
            }

            return PartialView(partialModel);
        }

        public ActionResult DataTablesList(DtParameterModel dtParameterModel, ENTSATISAra entSatisAra)
        {
            entSatisAra.Ara = dtParameterModel.AramaKriteri;
            entSatisAra.KurumKayitNo = AktifKullanici.KurumKayitNo;
            var kayitlar = _entSatisService.Ara(entSatisAra);
            return Json(new DataTableResult()
            {
                data = kayitlar.ENTSATISDetayList.Select(t => new
                {
                    t.KAYITNO,
                    t.KapakSeriNo,
                    t.SayacSeriNo,
                    t.AboneNo,
                    t.AboneAdSoyad,
                    t.KREDI,
                    t.YEDEKKREDI,
                    t.Kdv,
                    t.Ctv,
                    t.AylikBakimBedeli,
                    t.SatisTutarı,
                    t.ToplamKredi,
                    OLUSTURMATARIH = t.OLUSTURMATARIH.ToString(),
                    t.OlusturanKullaniciAdi,
                    t.SayacTipi,
                    t.ODEME,
                    t.SatisTipi,
                    SatisTipAdi = Dil.ResourceManager.GetString(t.SatisTipAdi),
                    Islemler = $@"<a class='btn btn-xs btn-info' href='{Url.Action("SatistanMakbuzOlustur", "Satis", new { satisKayitNo = t.KAYITNO, sayacKayitNo = t.SAYACKAYITNO })}' title='Makbuz İndir'><i class='fa fa-edit'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }




        public ActionResult KartliSatis()
        {
            return View(new SatisModel());
        }

        public SogukSuOkunan ParsHamData(string hamdata)
        {

            try
            {
                SogukSuOkunan sogukSu = new SogukSuOkunan();
                string[] suData = hamdata.Split('#');
                sogukSu.AboneNo = suData[1];
                sogukSu.KartNo = suData[2];
                sogukSu.SayacSeriNo = suData[3];
                sogukSu.Ako = suData[4];
                sogukSu.Yko = suData[5];
                sogukSu.Kredi = suData[26];
                sogukSu.YedekKredi = suData[27];
                return sogukSu;

            }
            catch (Exception ex)
            {
                throw new NotificationException("Ham Data Pars Edilemedi :" + ex.Message + "\nHamdata :" + hamdata);

            }

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
                //model.SuSatisModel.SogukSuOkunan = new SogukSuOkunan(model.HamData);
                model.SuSatisModel.AboneSayacDetay = _entAboneSayacService.DetayGetir(new ENTABONESAYACAra { SayacSeriNo = model.SuSatisModel.SogukSuOkunan.SayacSeriNo, Durum = 1, SayacTur = 1 }).FirstOrDefault();
                model.SuSatisModel.PrmTarifeSuDetay = _prmTarifeSuService.DetayGetir(new PRMTARIFESUAra { KAYITNO = model.SuSatisModel.AboneSayacDetay.TARIFEKAYITNO }).FirstOrDefault();

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


                model.SuSatisModel.Satis.Ctv = model.SuSatisModel.PrmTarifeSuDetay.Ctv * model.SuSatisModel.Satis.ODEME;
                model.SuSatisModel.Satis.Kdv = (model.SuSatisModel.Satis.ODEME - model.SuSatisModel.Satis.Ctv) * model.SuSatisModel.PrmTarifeSuDetay.Kdv;
                model.SuSatisModel.Satis.SatisTutari = (model.SuSatisModel.Satis.ODEME - model.SuSatisModel.Satis.Ctv - model.SuSatisModel.Satis.Kdv - model.SuSatisModel.Satis.AylikBakimBedeli);

                model.SuSatisModel.Satis.YEDEKKREDI = (model.SuSatisModel.PrmTarifeSuDetay.YEDEKKREDI * model.SuSatisModel.PrmTarifeSuDetay.TUKETIMKATSAYI);
                model.SuSatisModel.Satis.KREDI = ((model.SuSatisModel.Satis.SatisTutari * model.SuSatisModel.PrmTarifeSuDetay.TUKETIMKATSAYI) / model.SuSatisModel.PrmTarifeSuDetay.BIRIMFIYAT);
                //model.SuSatisModel.Satis.KREDI = Math.Round(Convert.ToDecimal(model.SuSatisModel.Satis.KREDI), 2);
                model.SuSatisModel.Satis.KREDI = Math.Floor(Convert.ToDecimal(model.SuSatisModel.Satis.KREDI));

                if (model.SuSatisModel.Satis.KREDI < model.SuSatisModel.Satis.YEDEKKREDI)
                {
                    throw new NotificationException("Yedek kredi den az miktarda satış yapılamaz");
                }

                if (model.SuSatisModel.SogukSuOkunan.Ako == "b" && model.SuSatisModel.SogukSuOkunan.Yko == "b")
                {
                    model.SuSatisModel.Satis.ToplamKredi = Convert.ToInt32(model.SuSatisModel.SogukSuOkunan.Kredi) + Convert.ToInt32(model.SuSatisModel.SogukSuOkunan.YedekKredi) + model.SuSatisModel.Satis.KREDI - model.SuSatisModel.Satis.YEDEKKREDI;
                    model.SuSatisModel.Satis.YEDEKKREDI = model.SuSatisModel.Satis.YEDEKKREDI;
                }
                else if (model.SuSatisModel.SogukSuOkunan.Ako == "*" && model.SuSatisModel.SogukSuOkunan.Yko == "b")
                {
                    model.SuSatisModel.Satis.ToplamKredi = Convert.ToInt32(model.SuSatisModel.SogukSuOkunan.YedekKredi) + model.SuSatisModel.Satis.KREDI - model.SuSatisModel.Satis.YEDEKKREDI;
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

        //[BackUpFilter]
        public JsonResult SatisYap(SatisModel model)
        {
            if (model.SuSatisModel.Satis.SatisTipi == enumSatisTipi.Satis.GetHashCode())
            {
                if (model.SuSatisModel.Satis.ODEME > 0)
                {

                    model.SuSatisModel.Satis.OLUSTURAN = AktifKullanici.KayitNo;
                    model.SuSatisModel.Satis = _entSatisService.Ekle(model.SuSatisModel.Satis);

                    var abone = _entAboneService.GetirById(model.SuSatisModel.Satis.ABONEKAYITNO);
                    abone.SonSatisTarih = DateTime.Now;
                    _entAboneService.Guncelle(abone.List());
                }
            }
            else if (model.SuSatisModel.Satis.SatisTipi == enumSatisTipi.BedelsizSatis.GetHashCode())
            {
                model.SuSatisModel.Satis.OLUSTURAN = AktifKullanici.KayitNo;
                model.SuSatisModel.Satis = _entSatisService.Ekle(model.SuSatisModel.Satis);
            }
            else if (model.SuSatisModel.Satis.SatisTipi == enumSatisTipi.SatisIptal.GetHashCode())
            {
                model.SuSatisModel.Satis.OLUSTURAN = AktifKullanici.KayitNo;
                model.SuSatisModel.Satis = _entSatisService.Ekle(model.SuSatisModel.Satis);
            }


            return Json(MakbuzOlustur(model), JsonRequestBehavior.AllowGet);
        }

        public string MakbuzOlustur(SatisModel model)
        {
            if (AktifKullanici.Dil == enumDil.Turkce.GetHashCode())
            {
                ENTSATISDetay suSatisDetay = _entSatisService.DetayGetirById(model.SuSatisModel.Satis.KAYITNO);
                ENTSATISDetay kalorimetreSatisDetay = _entSatisService.DetayGetirById(model.KalorimetreSatisModel.Satis.KAYITNO);

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
                    suSatisBilgileri.SayacTuru = "SU";
                    suSatisBilgileri.KontorMiktar = suSatisDetay.KREDI.ToString();

                    suSatisBilgileri.BakimBedeli = suSatisDetay.AylikBakimBedeli.ToString();
                    suSatisBilgileri.Ctv = suSatisDetay.Ctv.ToString();
                    suSatisBilgileri.Kdv = suSatisDetay.Kdv.ToString();
                    suSatisBilgileri.Tutar = suSatisDetay.SatisTutarı.ToString();
                    suSatisBilgileri.TotalTutar = suSatisDetay.ODEME.ToString();

                }
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
                    kalorimetreSatisBilgileri.Tutar = kalorimetreSatisDetay.SatisTutarı.ToString();
                    kalorimetreSatisBilgileri.TotalTutar = kalorimetreSatisDetay.ODEME.ToString();


                }

                satisBilgileri.SatisTutari = (kalorimetreSatisBilgileri.Tutar.ToDecimal() + suSatisBilgileri.Tutar.ToDecimal()).ToString();
                satisBilgileri.BakimHizmetleriBedeli = (kalorimetreSatisBilgileri.BakimBedeli.ToInt32() + suSatisBilgileri.BakimBedeli.ToInt32()).ToString();
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
            else
            {
                ENTSATISDetay suSatisDetay = _entSatisService.DetayGetirById(model.SuSatisModel.Satis.KAYITNO);
                LocalReport lr = new LocalReport();
                string path = Path.Combine(Server.MapPath("~/Reports"), "SatisMakbuzIngilizce.rdlc");
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

                Microsoft.Reporting.WebForms.ReportDataSource reportDataSource3 = new Microsoft.Reporting.WebForms.ReportDataSource();
                List<SuSatisBilgileri> dataSource3 = new List<SuSatisBilgileri>();
                SuSatisBilgileri suSatisBilgileri = new SuSatisBilgileri();

                if (suSatisDetay != null)
                {
                    makbuzBilgileri.KurumAdi = AktifKullanici.KurumAdi;
                    makbuzBilgileri.AboneAdiSoyadi = suSatisDetay.AboneAdSoyad;
                    makbuzBilgileri.AboneNo = suSatisDetay.AboneNo;
                    makbuzBilgileri.FaturaTarihi = suSatisDetay.OLUSTURMATARIH.ToString();
                    makbuzBilgileri.SuSayacNo = suSatisDetay.KapakSeriNo.ToString();
                    makbuzBilgileri.FaturaNo = suSatisDetay.KAYITNO.ToString();

                    suSatisBilgileri.Tarih = suSatisDetay.OLUSTURMATARIH.ToString();
                    suSatisBilgileri.SayacTuru = "Water";
                    suSatisBilgileri.KontorMiktar = suSatisDetay.KREDI.ToString();

                    suSatisBilgileri.BakimBedeli = suSatisDetay.AylikBakimBedeli.ToString();
                    suSatisBilgileri.Ctv = suSatisDetay.Ctv.ToString();
                    suSatisBilgileri.Kdv = suSatisDetay.Kdv.ToString();
                    suSatisBilgileri.Tutar = suSatisDetay.SatisTutarı.ToString();
                    suSatisBilgileri.TotalTutar = suSatisDetay.ODEME.ToString();

                }

                dataSource1.Add(makbuzBilgileri);
                reportDataSource1.Name = "AboneBilgileri";
                reportDataSource1.Value = dataSource1;
                lr.DataSources.Add(reportDataSource1);



                dataSource3.Add(suSatisBilgileri);
                reportDataSource3.Name = "SatisBilgileri";
                reportDataSource3.Value = dataSource3;
                lr.DataSources.Add(reportDataSource3);







                renderedBytes = lr.Render("PDF", deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warning);

                string filename = makbuzBilgileri.AboneNo + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + ".pdf";
                path = Server.MapPath("~/App_Data/" + filename);
                System.IO.File.WriteAllBytes(path, renderedBytes);

                return filename;

            }


        }

        public ActionResult MakbuzIndir(string filename)
        {
            byte[] bytes = System.IO.File.ReadAllBytes(Server.MapPath("~/App_Data/" + filename));
            return File(bytes, "PDF", filename);
        }

        public ActionResult ToplamSatis()
        {
            return View();
        }

        public ActionResult SatistanMakbuzOlustur(int satisKayitNo, int sayacKayitNo)
        {
           
            string filename="";
            if (AktifKullanici.Dil == enumDil.Turkce.GetHashCode())
            {
                filename = TurkceMakbuzOlustur(satisKayitNo,sayacKayitNo);
            }
            else
            {
                filename = IngilizceMakbuzOlustur(satisKayitNo, sayacKayitNo);
            }





            byte[] bytes = System.IO.File.ReadAllBytes(Server.MapPath("~/App_Data/" + filename));
            return File(bytes, "PDF", filename);

        }

        private string IngilizceMakbuzOlustur(int satisKayitNo, int sayacKayitNo)
        {
            ENTSATISDetay suSatisDetay = _entSatisService.DetayGetirById(satisKayitNo);
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "SatisMakbuzIngilizce.rdlc");
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

            Microsoft.Reporting.WebForms.ReportDataSource reportDataSource3 = new Microsoft.Reporting.WebForms.ReportDataSource();
            List<SuSatisBilgileri> dataSource3 = new List<SuSatisBilgileri>();
            SuSatisBilgileri suSatisBilgileri = new SuSatisBilgileri();

            if (suSatisDetay != null)
            {
                makbuzBilgileri.KurumAdi = AktifKullanici.KurumAdi;
                makbuzBilgileri.AboneAdiSoyadi = suSatisDetay.AboneAdSoyad;
                makbuzBilgileri.AboneNo = suSatisDetay.AboneNo;
                makbuzBilgileri.FaturaTarihi = suSatisDetay.OLUSTURMATARIH.ToString();
                makbuzBilgileri.SuSayacNo = suSatisDetay.KapakSeriNo.ToString();
                makbuzBilgileri.FaturaNo = suSatisDetay.KAYITNO.ToString();

                suSatisBilgileri.Tarih = suSatisDetay.OLUSTURMATARIH.ToString();
                suSatisBilgileri.SayacTuru = "Water";
                suSatisBilgileri.KontorMiktar = suSatisDetay.KREDI.ToString();

                suSatisBilgileri.BakimBedeli = suSatisDetay.AylikBakimBedeli.ToString();
                suSatisBilgileri.Ctv = suSatisDetay.Ctv.ToString();
                suSatisBilgileri.Kdv = suSatisDetay.Kdv.ToString();
                suSatisBilgileri.Tutar = suSatisDetay.SatisTutarı.ToString();
                suSatisBilgileri.TotalTutar = suSatisDetay.ODEME.ToString();

            }

            dataSource1.Add(makbuzBilgileri);
            reportDataSource1.Name = "AboneBilgileri";
            reportDataSource1.Value = dataSource1;
            lr.DataSources.Add(reportDataSource1);



            dataSource3.Add(suSatisBilgileri);
            reportDataSource3.Name = "SatisBilgileri";
            reportDataSource3.Value = dataSource3;
            lr.DataSources.Add(reportDataSource3);







            renderedBytes = lr.Render("PDF", deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warning);

            string filename = makbuzBilgileri.AboneNo + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + ".pdf";
            path = Server.MapPath("~/App_Data/" + filename);
            System.IO.File.WriteAllBytes(path, renderedBytes);

            return filename;
        }
        private string TurkceMakbuzOlustur(int satisKayitNo, int sayacKayitNo)
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
                tarifeKalorimetre = _prmTarifeKalorimetreService.GetirById(aboneSayacKalorimetre.TARIFEKAYITNO.Value);
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

            return filename;
        }


    }
}