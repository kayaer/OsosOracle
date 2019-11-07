using Microsoft.Reporting.WebForms;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.ENTABONEBILGIComplexTypes;
using OsosOracle.Entities.ComplexType.ENTABONEComplexTypes;
using OsosOracle.Entities.ComplexType.ENTABONESAYACComplexTypes;
using OsosOracle.Entities.ComplexType.ENTSATISComplexTypes;
using OsosOracle.Entities.ComplexType.ENTSAYACComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.MvcUI.Filters;
using OsosOracle.MvcUI.Models.ENTSATISModels;
using OsosOracle.MvcUI.ReportDataSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class SatisController : BaseController
    {
        private readonly IENTSATISService _entSatisService;
        private readonly IENTSAYACService _entSayacService;
        private readonly IENTABONESAYACService _entAboneSayacService;
        private readonly IENTABONEBILGIService _entAboneBilgiService;
        private readonly IENTABONEService _entAboneService;
        private readonly IPRMTARIFEORTAKAVMService _prmOrtakAvmService;
        private readonly IPRMTARIFESUService _prmTarifeSuService;
        public SatisController(IENTSATISService entSatisService,
            IENTSAYACService entSayacService,
            IENTABONESAYACService entAboneSayacService,
            IENTABONEBILGIService entAboneBilgiService,
            IENTABONEService entAboneService,
            IPRMTARIFEORTAKAVMService prmOrtakAvmService,
            IPRMTARIFESUService prmTarifeSuService)
        {
            _entSatisService = entSatisService;
            _entSayacService = entSayacService;
            _entAboneSayacService = entAboneSayacService;
            _entAboneBilgiService = entAboneBilgiService;
            _entAboneService = entAboneService;
            _prmOrtakAvmService = prmOrtakAvmService;
            _prmTarifeSuService = prmTarifeSuService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DataTablesList(DtParameterModel dtParameterModel, ENTSATISAra entSatisAra)
        {

            entSatisAra.Ara = dtParameterModel.AramaKriteri;


            var kayitlar = _entSatisService.Ara(entSatisAra);

            return Json(new DataTableResult()
            {
                data = kayitlar.ENTSATISDetayList.Select(t => new
                {
                    t.KAYITNO,
                    t.SayacSeriNo,
                    t.AboneNo,
                    t.KREDI,
                    OLUSTURMATARIH = t.OLUSTURMATARIH.ToString(),
                    Islemler = $@"<a class='btn btn-xs btn-info modalizer' href='{Url.Action("Guncelle", "Satis", new { t.KAYITNO })}' title='Düzenle'><i class='fa fa-edit'></i></a>							 
								<a class='btn btn-xs btn-danger modalizer ' href='{Url.Action("Sil", "Satis", new { t.KAYITNO })}' title='Sil'><i class='fa fa-trash'></i></a>"
                }),
                draw = dtParameterModel.Draw,
                recordsTotal = kayitlar.ToplamKayitSayisi,
                recordsFiltered = kayitlar.ToplamKayitSayisi
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult KartliSatis()
        {
            return View();
        }


        public ActionResult Ekle(int? sayacKayitno)
        {
            var sayac = _entSayacService.DetayGetir(new ENTSAYACAra { KAYITNO = sayacKayitno.Value }).FirstOrDefault();

            if (sayac.PrmTarifeElkDetay.AD == null && sayac.PrmTarifeSuDetay.AD == null)
            {
                throw new NotificationException("Sayaç Tarifesi girilmemiş");
            }
            var model = new ENTSATISKaydetModel
            {
                ENTSATIS = new ENTSATIS()
                {
                    SAYACKAYITNO = sayacKayitno.Value,
                    ABONEKAYITNO = sayacKayitno.Value,
                    FATURANO = 0,
                    VERSIYON = 0,
                    IPTAL = 0
                },
                PrmTarifeSuDetay = sayac.PrmTarifeSuDetay,
                PrmTarifeElkDetay = sayac.PrmTarifeElkDetay,
                SayacTipi = sayac.SayacTipi,
                SayacSeriNo = sayac.SERINO
            };

            return View("Kaydet", model);
        }

        public ElkKartOkunan ElkParseEt(string data)
        {
            ElkKartOkunan elk = new ElkKartOkunan();

            try
            {
                string[] dataArray = data.Split("|".ToCharArray());

                elk.OkumaKontrol = dataArray[0];
                elk.SayacSeriNo = Convert.ToInt64(dataArray[1]);
                elk.AnaKredi = Convert.ToInt64(dataArray[2]);
                elk.AnaKrediKontrol = dataArray[3] == "*" ? true : false;
                elk.YedekKrediKontrol = dataArray[4] == "*" ? true : false;
                elk.KartNo = Convert.ToInt64(dataArray[5]);
                elk.KalanKredi = Convert.ToInt64(dataArray[6]);
                elk.TuketilenKredi = Convert.ToInt64(dataArray[7]);
                elk.SayacTarihi = dataArray[8];
                elk.KlemensCeza = dataArray[9];
                elk.Ariza = dataArray[10];
                elk.DusukPilDurumu = dataArray[11];
                elk.BitikPilDurumu = dataArray[12];
                elk.BirOncekiDonemTuketim = Convert.ToInt64(dataArray[13]);
                elk.IkiOncekiDonemTuketim = Convert.ToInt64(dataArray[14]);
                elk.UcOncekiDonemTuketim = Convert.ToInt64(dataArray[15]);
                elk.GercekTuketim = Convert.ToInt64(dataArray[16]);
                elk.Ekim = Convert.ToInt64(dataArray[17]);
                elk.Aralik = Convert.ToInt64(dataArray[18]);
                elk.KademeBir = Convert.ToInt64(dataArray[19]);
                elk.KademeIki = Convert.ToInt64(dataArray[20]);
                elk.KademeUc = Convert.ToInt64(dataArray[21]);
                elk.Limit1 = Convert.ToInt64(dataArray[22]);
                elk.Limit2 = Convert.ToInt64(dataArray[23]);
                elk.YuklemeLimiti = Convert.ToInt64(dataArray[24]);
                elk.AksamSaati = Convert.ToDecimal(dataArray[25]);
                elk.SabahSaati = Convert.ToDecimal(dataArray[26]);
                elk.Kademe = Convert.ToInt64(dataArray[27]);
                elk.HaftaSonuAksam = (dataArray[28]);
                elk.FixCharge = Convert.ToInt64(dataArray[29]);
                elk.TotalFixCharge = Convert.ToInt64(dataArray[30]);
                elk.YedekKredi = Convert.ToInt64(dataArray[31]);
                elk.KritikKredi = Convert.ToInt64(dataArray[32]);
                elk.Tip = 1;

                if (elk.AnaKrediKontrol == true)
                {
                    elk.OkunduAnaBilgi = "Used";// Dil.Yuklenmis;
                }
                else
                {
                    elk.OkunduAnaBilgi = "Not Used";// Dil.Yuklenmemis;
                }

                if (elk.YedekKrediKontrol == true)
                {
                    elk.OkunduYedekBilgi = "Used";// Dil.Yuklenmis;
                }
                else
                {
                    elk.OkunduYedekBilgi = "Not Used";// Dil.Yuklenmemis;
                }

            }
            catch
            {
            }



            return elk;
        }
        public ENTSATISKaydetModel Hesapla(ENTSATISKaydetModel model)
        {
            double xelkTutar, islemMiktarElk, islemTutarElk = 0;

            decimal fiyatElk1, fiyatElk2, fiyatElk3, miktar;

            int katsayiElk;

            string sayacBilgi = string.Empty;

            xelkTutar = 0; islemMiktarElk = 0; islemTutarElk = 0; islemTutarElk = 0;
            fiyatElk1 = 0; fiyatElk2 = 0; fiyatElk3 = 0;
            katsayiElk = 0;

            //model.OdenenTutar = Convert.ToDouble(model.SetOdenenTutar);  //model.SetOdenenTutar.Replace('.', ',')
            xelkTutar = Convert.ToDouble(model.GirilenTutar);
            fiyatElk1 = model.PrmTarifeElkDetay.FIYAT1;
            fiyatElk2 = fiyatElk1;
            fiyatElk3 = fiyatElk1;
            katsayiElk = model.PrmTarifeElkDetay.KREDIKATSAYI;

            islemTutarElk = xelkTutar;
            islemMiktarElk = islemTutarElk / Convert.ToDouble(fiyatElk1);
            islemMiktarElk = islemMiktarElk * katsayiElk;
            islemMiktarElk = Math.Round(islemMiktarElk);
            var OdenenTutar = Math.Round(xelkTutar, 2);

            miktar = Convert.ToDecimal(islemMiktarElk);
            var toplamYuklenecekMiktar = miktar;

            if (model.ElkKartOkunan.AnaKrediKontrol == false && model.ElkKartOkunan.YedekKrediKontrol == true)// b *
            {
                model.ElkKartYuklenecek.sayacBilgi = string.Format("Yeni Kart Uyarı", "Elektrik");
                if (miktar > model.PrmTarifeElkDetay.YEDEKKREDI)
                {
                    model.ElkKartYuklenecek.AnaKredi = Convert.ToInt64(toplamYuklenecekMiktar - model.PrmTarifeElkDetay.YEDEKKREDI);
                    model.ElkKartYuklenecek.YedekKredi = Convert.ToInt64(model.PrmTarifeElkDetay.YEDEKKREDI);
                }
                else
                {
                    model.ElkKartYuklenecek.AnaKredi = Convert.ToInt64(toplamYuklenecekMiktar);
                    model.ElkKartYuklenecek.YedekKredi = 0;
                }
            }
            else if (miktar > model.PrmTarifeElkDetay.YEDEKKREDI || (model.ElkKartOkunan.YedekKredi > 0 && model.ElkKartOkunan.YedekKrediKontrol == false))
            {
                if (model.ElkKartOkunan.AnaKrediKontrol == false) toplamYuklenecekMiktar += model.ElkKartOkunan.AnaKredi;
                if (model.ElkKartOkunan.YedekKrediKontrol == false) toplamYuklenecekMiktar += model.ElkKartOkunan.YedekKredi;

                model.ElkKartYuklenecek.AnaKredi = Convert.ToInt64(toplamYuklenecekMiktar - model.PrmTarifeElkDetay.YEDEKKREDI);

                model.ElkKartYuklenecek.YedekKredi = Convert.ToInt64(model.PrmTarifeElkDetay.YEDEKKREDI);

                //model.SatisTur = 0;
            }
            else
            {
                model.ElkKartYuklenecek.sayacBilgi = string.Format("YuklenenKrediMiktariYedekKredidenYuksekOlmali", model.YedekKredi);
            }

            model.ElkKartYuklenecek.SayacSeriNo = model.ElkKartOkunan.SayacSeriNo;
            //model.ElkKartYuklenecek.Fiyat1 = model.PrmTarifeElkDetay.FIYAT1 * katsayiElk;
            //model.ElkKartYuklenecek.Fiyat2 = model.PrmTarifeElkDetay.FIYAT2 * katsayiElk;
            //model.ElkKartYuklenecek.Fiyat3 = model.PrmTarifeElkDetay.FIYAT3 * katsayiElk;
            //model.ElkKartYuklenecek.Limit1 = model.PrmTarifeElkDetay.LIMIT1;
            //model.ElkKartYuklenecek.Limit2 = model.PrmTarifeElkDetay.LIMIT2;
            //model.ElkKartYuklenecek.YuklemeLimiti = YuklemeLimitiHesaplaElektrik(model.PrmTarifeElkDetay.YUKLEMELIMIT);
            //model.ElkKartYuklenecek.AksamSaati = Convert.ToDecimal(model.PrmTarifeElkDetay.AKSAMSAAT);
            //model.ElkKartYuklenecek.SabahSaati = Convert.ToDecimal(model.PrmTarifeElkDetay.SABAHSAAT);
            //model.ElkKartYuklenecek.HaftaSonuAksam = Convert.ToDecimal(model.PrmTarifeElkDetay.HAFTASONUAKSAM);
            //model.ElkKartYuklenecek.FixCharge = model.PrmTarifeElkDetay.SABITUCRET;
            //model.ElkKartYuklenecek.Tatil1Gun = model.PrmTarifeElkDetay.BAYRAM1GUN;
            //model.ElkKartYuklenecek.Tatil1Ay = model.PrmTarifeElkDetay.BAYRAM1AY;         
            //model.ElkKartYuklenecek.Tatil1Sure = model.PrmTarifeElkDetay.BAYRAM1SURE;
            //model.ElkKartYuklenecek.Tatil2Gun = model.PrmTarifeElkDetay.BAYRAM2GUN;
            //model.ElkKartYuklenecek.Tatil2Ay = model.PrmTarifeElkDetay.BAYRAM2AY;       
            //model.ElkKartYuklenecek.Tatil2Sure = model.PrmTarifeElkDetay.BAYRAM2SURE;          
            //model.ElkKartYuklenecek.KritikKredi = model.PrmTarifeElkDetay.KRITIKKREDI;



            return model;

        }

        public int YuklemeLimitiHesaplaElektrik(int yuklemeLimiti)
        {
            int result = 0;

            if (yuklemeLimiti <= 60)
                result = Convert.ToInt32(yuklemeLimiti * 4);
            else if (yuklemeLimiti > 60 && yuklemeLimiti < 90)
                result = Convert.ToInt32(240 + (yuklemeLimiti - 60) * 2.25m);
            else if (yuklemeLimiti >= 90 && yuklemeLimiti <= 100)
                result = 255;
            else
                result = 255;//300 Kullanılmıyor demektir

            return result;
        }

        public PartialViewResult KartOkunan(string hamdata)
        {
            var model = ElkParseEt(hamdata);

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult HesaplaYeni(ENTSATISKaydetModel model)
        {
            var modelYeni = Hesapla(model);
            return Json(modelYeni, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult KartOkunanModel(string hamdata)
        {
            ENTSATISKaydetModel model = new ENTSATISKaydetModel();
            model.ElkKartOkunan = ElkParseEt(hamdata);

            var sayac = _entSayacService.DetayGetir(new ENTSAYACAra { SERINO = (int)model.ElkKartOkunan.SayacSeriNo }).FirstOrDefault();
            if (sayac == null)
            {
                throw new NotificationException("Sayaç sistemde kayıtlı değildir");
            }

            var aboneSayac = _entAboneSayacService.Getir(new ENTABONESAYACAra { SAYACKAYITNO = sayac.KAYITNO }).FirstOrDefault();
            var aboneBilgi = _entAboneBilgiService.Getir(new ENTABONEBILGIAra { ABONEKAYITNO = aboneSayac.ABONEKAYITNO }).FirstOrDefault();
            model.ElkKartOkunan.AboneNo = Convert.ToInt64(aboneBilgi.ABONENO);
            model.HamData = hamdata;
            model.PrmTarifeElkDetay = sayac.PrmTarifeElkDetay;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SatisKaydet(ENTSATISKaydetModel satisKaydetModel)
        {
            if (satisKaydetModel.ElkKartOkunan == null)
            {
                throw new NotificationException("Kart okuma yapmalısınız");
            }

            if (satisKaydetModel.ENTSATIS == null)
            {
                satisKaydetModel.ENTSATIS = new ENTSATIS();
            }




            satisKaydetModel.ENTSATIS.OLUSTURAN = AktifKullanici.KayitNo;
            satisKaydetModel.ENTSATIS.GUNCELLEYEN = AktifKullanici.KayitNo;
            satisKaydetModel.ENTSATIS.KREDI = (int)satisKaydetModel.ElkKartYuklenecek.AnaKredi;
            satisKaydetModel.ENTSATIS.YEDEKKREDI = (int)satisKaydetModel.ElkKartYuklenecek.YedekKredi;
            satisKaydetModel.ENTSATIS.ODEME = (int)satisKaydetModel.GirilenTutar;

            ENTSAYAC sayac = _entSayacService.Getir(new ENTSAYACAra { SERINO = (int)satisKaydetModel.ElkKartOkunan.SayacSeriNo }).FirstOrDefault();
            satisKaydetModel.ENTSATIS.SAYACKAYITNO = sayac.KAYITNO;

            ENTABONESAYAC aboneSayac = _entAboneSayacService.Getir(new ENTABONESAYACAra { SAYACKAYITNO = sayac.KAYITNO }).FirstOrDefault();
            if (aboneSayac == null)
            {
                throw new NotificationException("Abone Sayaç bulunamadı" + sayac.KAYITNO);
            }
            aboneSayac.SONSATISTARIH = DateTime.Now;
            _entAboneSayacService.Guncelle(aboneSayac.List());

            //ENTABONEBILGI aboneBilgi = _entAboneBilgiService.Getir(new ENTABONEBILGIAra { ABONEKAYITNO = aboneSayac.ABONEKAYITNO }).FirstOrDefault();
            //ENTABONE abone = _entAboneService.Getir(new ENTABONEAra { KAYITNO = aboneSayac.ABONEKAYITNO }).FirstOrDefault();
            satisKaydetModel.ENTSATIS.ABONEKAYITNO = aboneSayac.ABONEKAYITNO;
            if (satisKaydetModel.ENTSATIS.KAYITNO > 0)
            {
                satisKaydetModel.ENTSATIS.VERSIYON += 1;
                _entSatisService.Guncelle(satisKaydetModel.ENTSATIS.List());
            }
            else
            {
                _entSatisService.Ekle(satisKaydetModel.ENTSATIS.List());
            }
            MakbuzOlustur(satisKaydetModel);
            return Json(satisKaydetModel, JsonRequestBehavior.AllowGet);
        }

        public ENTSATISKaydetModel MakbuzOlustur(ENTSATISKaydetModel satisKaydetModel)
        {
            ENTSAYAC sayac = _entSayacService.Getir(new ENTSAYACAra { SERINO = (int)satisKaydetModel.ElkKartOkunan.SayacSeriNo }).FirstOrDefault();
            satisKaydetModel.ENTSATIS.SAYACKAYITNO = sayac.KAYITNO;

            ENTABONESAYAC aboneSayac = _entAboneSayacService.Getir(new ENTABONESAYACAra { SAYACKAYITNO = sayac.KAYITNO }).FirstOrDefault();
            if (aboneSayac == null)
            {
                throw new NotificationException("Abone Sayaç bulunamadı" + sayac.KAYITNO);
            }
            ENTABONEBILGI aboneBilgi = _entAboneBilgiService.Getir(new ENTABONEBILGIAra { ABONEKAYITNO = aboneSayac.ABONEKAYITNO }).FirstOrDefault();
            if (aboneBilgi == null)
            {
                throw new NotificationException("Abone Bilgi bulunamadı" + aboneSayac.ABONEKAYITNO);
            }
            ENTABONE abone = _entAboneService.Getir(new ENTABONEAra { KAYITNO = aboneSayac.ABONEKAYITNO }).FirstOrDefault();
            if (abone == null)
            {
                throw new NotificationException("Abone  bulunamadı" + aboneSayac.ABONEKAYITNO);
            }

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/TahsilatMakbuz.rdlc");
            Microsoft.Reporting.WebForms.ReportDataSource reportDataSource = new Microsoft.Reporting.WebForms.ReportDataSource();
            reportDataSource.Name = "DataSet1";
            List<SatisMakbuz> dataSource = new List<SatisMakbuz>();
            dataSource.Add(new SatisMakbuz { AboneAdiSoyadi = abone.AD + " " + abone.SOYAD, AboneNo = aboneBilgi.ABONENO, MakbuzNo = 1, OdemeMiktar = satisKaydetModel.GirilenTutar, OdemeTarih = DateTime.Now, SayacSeriNo = satisKaydetModel.ElkKartOkunan.SayacSeriNo.ToString() });

            reportDataSource.Value = dataSource;
            localReport.DataSources.Add(reportDataSource);

            string mimeType;
            string encoding;
            string fileNameExtension;


            fileNameExtension = "pdf";


            string[] streams;
            Warning[] warnings;
            byte[] renderedByte;

            renderedByte = localReport.Render("PDF", "", out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            string filename = aboneBilgi.ABONENO + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + ".pdf";
            string path = Server.MapPath("~/App_Data/" + filename);
            System.IO.File.WriteAllBytes(path, renderedByte);
            satisKaydetModel.fileName = filename;
            return satisKaydetModel;
            //return File(renderedByte, "PDF", "Makbuz.pdf");
        }

        public ActionResult MakbuzIndir(string filename)
        {
            byte[] bytes = System.IO.File.ReadAllBytes(Server.MapPath("~/App_Data/" + filename));
            return File(bytes, "PDF", filename);
        }


        #region Ortak Avm Fonksiyonları
        public ActionResult OrtakAvm()
        {
            return View();
        }
        public PartialViewResult KartOkunanOrtakAvm(string hamdata)
        {
            var model = OrtakAvmPars(hamdata);

            return PartialView(model);
        }

        private object OrtakAvmPars(string hamdata)
        {
            OrtakAvmOkunan ortak = new OrtakAvmOkunan();

            try
            {
                string[] dataArray = hamdata.Split("#".ToCharArray());

                //elk.OkumaKontrol = dataArray[0];
                //elk.SayacSeriNo = Convert.ToInt64(dataArray[1]);
                //elk.AnaKredi = Convert.ToInt64(dataArray[2]);
                //elk.AnaKrediKontrol = dataArray[3] == "*" ? true : false;
                //elk.YedekKrediKontrol = dataArray[4] == "*" ? true : false;
                //elk.KartNo = Convert.ToInt64(dataArray[5]);
                //elk.KalanKredi = Convert.ToInt64(dataArray[6]);
                //elk.TuketilenKredi = Convert.ToInt64(dataArray[7]);
                //elk.SayacTarihi = dataArray[8];
                //elk.KlemensCeza = dataArray[9];
                //elk.Ariza = dataArray[10];
                //elk.DusukPilDurumu = dataArray[11];
                //elk.BitikPilDurumu = dataArray[12];
                //elk.BirOncekiDonemTuketim = Convert.ToInt64(dataArray[13]);
                //elk.IkiOncekiDonemTuketim = Convert.ToInt64(dataArray[14]);
                //elk.UcOncekiDonemTuketim = Convert.ToInt64(dataArray[15]);
                //elk.GercekTuketim = Convert.ToInt64(dataArray[16]);
                //elk.Ekim = Convert.ToInt64(dataArray[17]);
                //elk.Aralik = Convert.ToInt64(dataArray[18]);
                //elk.KademeBir = Convert.ToInt64(dataArray[19]);
                //elk.KademeIki = Convert.ToInt64(dataArray[20]);
                //elk.KademeUc = Convert.ToInt64(dataArray[21]);
                //elk.Limit1 = Convert.ToInt64(dataArray[22]);
                //elk.Limit2 = Convert.ToInt64(dataArray[23]);
                //elk.YuklemeLimiti = Convert.ToInt64(dataArray[24]);
                //elk.AksamSaati = Convert.ToDecimal(dataArray[25]);
                //elk.SabahSaati = Convert.ToDecimal(dataArray[26]);
                //elk.Kademe = Convert.ToInt64(dataArray[27]);
                //elk.HaftaSonuAksam = (dataArray[28]);
                //elk.FixCharge = Convert.ToInt64(dataArray[29]);
                //elk.TotalFixCharge = Convert.ToInt64(dataArray[30]);
                //elk.YedekKredi = Convert.ToInt64(dataArray[31]);
                //elk.KritikKredi = Convert.ToInt64(dataArray[32]);
                //elk.Tip = 1;

                //if (elk.AnaKrediKontrol == true)
                //{
                //    elk.OkunduAnaBilgi = "Used";// Dil.Yuklenmis;
                //}
                //else
                //{
                //    elk.OkunduAnaBilgi = "Not Used";// Dil.Yuklenmemis;
                //}

                //if (elk.YedekKrediKontrol == true)
                //{
                //    elk.OkunduYedekBilgi = "Used";// Dil.Yuklenmis;
                //}
                //else
                //{
                //    elk.OkunduYedekBilgi = "Not Used";// Dil.Yuklenmemis;
                //}

            }
            catch
            {
            }



            return ortak;
        }
        #endregion

        #region Kalorimetre Satış
        public ActionResult Kalorimetre()
        {
            return View(new ENTSATISKaydetModel());
        }
        public JsonResult KartOkunanKalorimetre(string hamdata)
        {
            var model = KalorimetrePars(hamdata);

            return Json(model);
        }

        private KalorimetreOkunan KalorimetrePars(string hamdata)
        {
            hamdata = "E#1#20191106#0/0/0#0#0#b#b#1#0#0#0#12#0#b#b#b#b#b#0#0#0#0#0#0#0#0#65535#0#12#3#1#0#|G#1#20191003#0/0/0#0#0#b#b#1#0#0#12#0#b#b#b#b#b#b#b#0#0#12#3#1#0#|S#1#20191003#3/10/19#0#0#*#b#0#0#0#0#0#12#0#1#1#65535#65535#*#*#*#*#*#*#*#*#0#12#3#1#0#|K#1#20191106#0/0/0#0#0#b#b#1#0#0#0#12#0#b#b#b#b#0#0#0#0#0#0#0#12#3#1#0#|";
            KalorimetreOkunan kalorimetre = new KalorimetreOkunan();

            try
            {
                string[] dataArray = hamdata.Split("|".ToCharArray());
                string[] kalorimetreData = dataArray[3].Split('#');
                kalorimetre.CihazNo = kalorimetreData[2];
                kalorimetre.Kredi = kalorimetreData[4];
                kalorimetre.YedekKredi = kalorimetreData[5];
                kalorimetre.Ako = kalorimetreData[6];


            }
            catch
            {
            }



            return kalorimetre;
        }

        [HttpPost]
        public ActionResult HesaplaKalorimetre(ENTSATISKaydetModel model)
        {
            var sayac = _entSayacService.Getir(new ENTSAYACAra { SERINO = Convert.ToInt32(model.KalorimetreOkunan.CihazNo) }).FirstOrDefault();
            var aboneSayac = _entAboneSayacService.DetayGetir(new ENTABONESAYACAra { SAYACKAYITNO = sayac.KAYITNO }).FirstOrDefault();
            var tarife = _prmOrtakAvmService.DetayGetirById(aboneSayac.TARIFEKAYITNO);

            if (aboneSayac.SONSATISTARIH != null)
            {
                var fark = (DateTime.Now - Convert.ToDateTime(aboneSayac.SONSATISTARIH)).TotalDays;
                if (fark > 30)
                {
                    model.KalorimetreYazilacak.anakr = Convert.ToInt32(model.GirilenTutar - tarife.AylikBakimBedeli * tarife.TUKETIMKATSAYI);
                }
                else
                {
                    model.KalorimetreYazilacak.anakr = Convert.ToInt32(model.GirilenTutar * tarife.TUKETIMKATSAYI);
                }
            }
            else
            {
                model.KalorimetreYazilacak.anakr = Convert.ToInt32(model.GirilenTutar * tarife.TUKETIMKATSAYI);
            }


            model.KalorimetreYazilacak.devno = Convert.ToUInt32(model.KalorimetreOkunan.CihazNo);
            model.KalorimetreYazilacak.yedekkr = tarife.YEDEKKREDI;
            model.KalorimetreYazilacak.Bayram1Gunn = tarife.BAYRAM1GUN;
            model.KalorimetreYazilacak.Bayram1Ayy = tarife.BAYRAM1AY;
            model.KalorimetreYazilacak.Bayram1Suree = tarife.BAYRAM1SURE;
            model.KalorimetreYazilacak.Bayram2Gunn = tarife.BAYRAM2GUN;
            model.KalorimetreYazilacak.Bayram2Ayy = tarife.BAYRAM2AY;
            model.KalorimetreYazilacak.Bayram2Suree = tarife.BAYRAM2SURE;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SatisKaydetKalorimetre(ENTSATISKaydetModel satisKaydetModel)
        {
            if (satisKaydetModel.KalorimetreOkunan == null)
            {
                throw new NotificationException("Kart okuma yapmalısınız");
            }

            if (satisKaydetModel.ENTSATIS == null)
            {
                satisKaydetModel.ENTSATIS = new ENTSATIS();
            }




            satisKaydetModel.ENTSATIS.OLUSTURAN = AktifKullanici.KayitNo;
            satisKaydetModel.ENTSATIS.GUNCELLEYEN = AktifKullanici.KayitNo;
            satisKaydetModel.ENTSATIS.KREDI = (int)satisKaydetModel.KalorimetreYazilacak.anakr;
            satisKaydetModel.ENTSATIS.YEDEKKREDI = (int)satisKaydetModel.KalorimetreYazilacak.yedekkr;
            satisKaydetModel.ENTSATIS.ODEME = (int)satisKaydetModel.GirilenTutar;
            int serino = Convert.ToInt32(satisKaydetModel.KalorimetreOkunan.CihazNo);
            ENTSAYAC sayac = _entSayacService.Getir(new ENTSAYACAra { SERINO = serino }).FirstOrDefault();
            satisKaydetModel.ENTSATIS.SAYACKAYITNO = sayac.KAYITNO;

            ENTABONESAYAC aboneSayac = _entAboneSayacService.Getir(new ENTABONESAYACAra { SAYACKAYITNO = sayac.KAYITNO }).FirstOrDefault();
            if (aboneSayac == null)
            {
                throw new NotificationException("Abone Sayaç bulunamadı" + sayac.KAYITNO);
            }
            aboneSayac.SONSATISTARIH = DateTime.Now;
            _entAboneSayacService.Guncelle(aboneSayac.List());

            //ENTABONEBILGI aboneBilgi = _entAboneBilgiService.Getir(new ENTABONEBILGIAra { ABONEKAYITNO = aboneSayac.ABONEKAYITNO }).FirstOrDefault();
            //ENTABONE abone = _entAboneService.Getir(new ENTABONEAra { KAYITNO = aboneSayac.ABONEKAYITNO }).FirstOrDefault();
            satisKaydetModel.ENTSATIS.ABONEKAYITNO = aboneSayac.ABONEKAYITNO;
            if (satisKaydetModel.ENTSATIS.KAYITNO > 0)
            {
                satisKaydetModel.ENTSATIS.VERSIYON += 1;
                _entSatisService.Guncelle(satisKaydetModel.ENTSATIS.List());
            }
            else
            {
                _entSatisService.Ekle(satisKaydetModel.ENTSATIS.List());
            }
            KalorimetreMakbuzOlustur(satisKaydetModel);
            return Json(satisKaydetModel, JsonRequestBehavior.AllowGet);
        }
        public ENTSATISKaydetModel KalorimetreMakbuzOlustur(ENTSATISKaydetModel satisKaydetModel)
        {
            int serino = Convert.ToInt32(satisKaydetModel.KalorimetreOkunan.CihazNo);
            ENTSAYAC sayac = _entSayacService.Getir(new ENTSAYACAra { SERINO = serino }).FirstOrDefault();
            satisKaydetModel.ENTSATIS.SAYACKAYITNO = sayac.KAYITNO;

            ENTABONESAYAC aboneSayac = _entAboneSayacService.Getir(new ENTABONESAYACAra { SAYACKAYITNO = sayac.KAYITNO }).FirstOrDefault();
            if (aboneSayac == null)
            {
                throw new NotificationException("Abone Sayaç bulunamadı" + sayac.KAYITNO);
            }
            ENTABONEBILGI aboneBilgi = _entAboneBilgiService.Getir(new ENTABONEBILGIAra { ABONEKAYITNO = aboneSayac.ABONEKAYITNO }).FirstOrDefault();
            if (aboneBilgi == null)
            {
                throw new NotificationException("Abone Bilgi bulunamadı" + aboneSayac.ABONEKAYITNO);
            }
            ENTABONE abone = _entAboneService.Getir(new ENTABONEAra { KAYITNO = aboneSayac.ABONEKAYITNO }).FirstOrDefault();
            if (abone == null)
            {
                throw new NotificationException("Abone  bulunamadı" + aboneSayac.ABONEKAYITNO);
            }

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/TahsilatMakbuz.rdlc");
            Microsoft.Reporting.WebForms.ReportDataSource reportDataSource = new Microsoft.Reporting.WebForms.ReportDataSource();
            reportDataSource.Name = "DataSet1";
            List<SatisMakbuz> dataSource = new List<SatisMakbuz>();
            dataSource.Add(new SatisMakbuz { AboneAdiSoyadi = abone.AD + " " + abone.SOYAD, AboneNo = aboneBilgi.ABONENO, MakbuzNo = 1, OdemeMiktar = satisKaydetModel.GirilenTutar, OdemeTarih = DateTime.Now, SayacSeriNo = satisKaydetModel.KalorimetreOkunan.CihazNo.ToString() });

            reportDataSource.Value = dataSource;
            localReport.DataSources.Add(reportDataSource);

            string mimeType;
            string encoding;
            string fileNameExtension;


            fileNameExtension = "pdf";


            string[] streams;
            Warning[] warnings;
            byte[] renderedByte;

            renderedByte = localReport.Render("PDF", "", out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            string filename = aboneBilgi.ABONENO + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + ".pdf";
            string path = Server.MapPath("~/App_Data/" + filename);
            System.IO.File.WriteAllBytes(path, renderedByte);
            satisKaydetModel.fileName = filename;
            return satisKaydetModel;
            //return File(renderedByte, "PDF", "Makbuz.pdf");
        }
        #endregion

        #region Soğuk Su Satış
        public ActionResult SogukSu()
        {
            return View(new ENTSATISKaydetModel());
        }
        public JsonResult KartOkunanSogukSu(string hamdata)
        {
            var model = SogukSuPars(hamdata);

            return Json(model);
        }

        private SogukSuOkunan SogukSuPars(string hamdata)
        {
            SogukSuOkunan sogukSu = new SogukSuOkunan();

            try
            {
               
                sogukSu.SayacSeriNo = "52344677";
                sogukSu.Kredi = "10";
                sogukSu.YedekKredi = "5";



            }
            catch
            {
            }



            return sogukSu;
        }

        [HttpPost]
        public ActionResult HesaplaSogukSu(ENTSATISKaydetModel model)
        {
            var sayac = _entSayacService.Getir(new ENTSAYACAra { SERINO = Convert.ToInt32(model.SogukSuOkunan.SayacSeriNo) }).FirstOrDefault();
            var aboneSayac = _entAboneSayacService.DetayGetir(new ENTABONESAYACAra { SAYACKAYITNO = sayac.KAYITNO }).FirstOrDefault();
            var tarife = _prmTarifeSuService.DetayGetirById(aboneSayac.TARIFEKAYITNO);

            if (aboneSayac.SONSATISTARIH != null)
            {
                var fark = (DateTime.Now - Convert.ToDateTime(aboneSayac.SONSATISTARIH)).TotalDays;
                if (fark > 30)
                {
                    model.SogukSuYazilacak.YuklenecekM3 = (model.GirilenTutar*tarife.TUKETIMKATSAYI)/tarife.BIRIMFIYAT;
                }
                else
                {
                    model.SogukSuYazilacak.YuklenecekM3 = (model.GirilenTutar * tarife.TUKETIMKATSAYI) / tarife.BIRIMFIYAT;
                }
            }
            else
            {
                model.SogukSuYazilacak.YuklenecekM3 = (model.GirilenTutar * tarife.TUKETIMKATSAYI) / tarife.BIRIMFIYAT;
            }


          
            model.SogukSuYazilacak.YedekKredi = tarife.YEDEKKREDI;
            model.SogukSuYazilacak.Bayram1Gunn = tarife.BAYRAM1GUN;
            model.SogukSuYazilacak.Bayram1Ayy = tarife.BAYRAM1AY;
            model.SogukSuYazilacak.Bayram1Suree = tarife.BAYRAM1SURE;
            model.SogukSuYazilacak.Bayram2Gunn = tarife.BAYRAM2GUN;
            model.SogukSuYazilacak.Bayram2Ayy = tarife.BAYRAM2AY;
            model.SogukSuYazilacak.Bayram2Suree = tarife.BAYRAM2SURE;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SatisKaydetSogukSu(ENTSATISKaydetModel satisKaydetModel)
        {
            if (satisKaydetModel.SogukSuOkunan == null)
            {
                throw new NotificationException("Kart okuma yapmalısınız");
            }

            if (satisKaydetModel.ENTSATIS == null)
            {
                satisKaydetModel.ENTSATIS = new ENTSATIS();
            }




            satisKaydetModel.ENTSATIS.OLUSTURAN = AktifKullanici.KayitNo;
            satisKaydetModel.ENTSATIS.GUNCELLEYEN = AktifKullanici.KayitNo;
            satisKaydetModel.ENTSATIS.KREDI = (int)satisKaydetModel.SogukSuYazilacak.YuklenecekM3;
            satisKaydetModel.ENTSATIS.YEDEKKREDI = (int)satisKaydetModel.SogukSuYazilacak.YedekKredi;
            satisKaydetModel.ENTSATIS.ODEME = (int)satisKaydetModel.GirilenTutar;
            int serino = Convert.ToInt32(satisKaydetModel.SogukSuOkunan.SayacSeriNo);
            ENTSAYAC sayac = _entSayacService.Getir(new ENTSAYACAra { SERINO = serino }).FirstOrDefault();
            satisKaydetModel.ENTSATIS.SAYACKAYITNO = sayac.KAYITNO;

            ENTABONESAYAC aboneSayac = _entAboneSayacService.Getir(new ENTABONESAYACAra { SAYACKAYITNO = sayac.KAYITNO }).FirstOrDefault();
            if (aboneSayac == null)
            {
                throw new NotificationException("Abone Sayaç bulunamadı" + sayac.KAYITNO);
            }
            aboneSayac.SONSATISTARIH = DateTime.Now;
            _entAboneSayacService.Guncelle(aboneSayac.List());

            //ENTABONEBILGI aboneBilgi = _entAboneBilgiService.Getir(new ENTABONEBILGIAra { ABONEKAYITNO = aboneSayac.ABONEKAYITNO }).FirstOrDefault();
            //ENTABONE abone = _entAboneService.Getir(new ENTABONEAra { KAYITNO = aboneSayac.ABONEKAYITNO }).FirstOrDefault();
            satisKaydetModel.ENTSATIS.ABONEKAYITNO = aboneSayac.ABONEKAYITNO;
            if (satisKaydetModel.ENTSATIS.KAYITNO > 0)
            {
                satisKaydetModel.ENTSATIS.VERSIYON += 1;
                _entSatisService.Guncelle(satisKaydetModel.ENTSATIS.List());
            }
            else
            {
                _entSatisService.Ekle(satisKaydetModel.ENTSATIS.List());
            }
            SogukSuMakbuzOlustur(satisKaydetModel);
            return Json(satisKaydetModel, JsonRequestBehavior.AllowGet);
        }
        public ENTSATISKaydetModel SogukSuMakbuzOlustur(ENTSATISKaydetModel satisKaydetModel)
        {
            int serino = Convert.ToInt32(satisKaydetModel.SogukSuOkunan.SayacSeriNo);
            ENTSAYAC sayac = _entSayacService.Getir(new ENTSAYACAra { SERINO = serino }).FirstOrDefault();
            satisKaydetModel.ENTSATIS.SAYACKAYITNO = sayac.KAYITNO;

            ENTABONESAYAC aboneSayac = _entAboneSayacService.Getir(new ENTABONESAYACAra { SAYACKAYITNO = sayac.KAYITNO }).FirstOrDefault();
            if (aboneSayac == null)
            {
                throw new NotificationException("Abone Sayaç bulunamadı" + sayac.KAYITNO);
            }
            ENTABONEBILGI aboneBilgi = _entAboneBilgiService.Getir(new ENTABONEBILGIAra { ABONEKAYITNO = aboneSayac.ABONEKAYITNO }).FirstOrDefault();
            if (aboneBilgi == null)
            {
                throw new NotificationException("Abone Bilgi bulunamadı" + aboneSayac.ABONEKAYITNO);
            }
            ENTABONE abone = _entAboneService.Getir(new ENTABONEAra { KAYITNO = aboneSayac.ABONEKAYITNO }).FirstOrDefault();
            if (abone == null)
            {
                throw new NotificationException("Abone  bulunamadı" + aboneSayac.ABONEKAYITNO);
            }

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/TahsilatMakbuz.rdlc");
            Microsoft.Reporting.WebForms.ReportDataSource reportDataSource = new Microsoft.Reporting.WebForms.ReportDataSource();
            reportDataSource.Name = "DataSet1";
            List<SatisMakbuz> dataSource = new List<SatisMakbuz>();
            dataSource.Add(new SatisMakbuz { AboneAdiSoyadi = abone.AD + " " + abone.SOYAD, AboneNo = aboneBilgi.ABONENO, MakbuzNo = 1, OdemeMiktar = satisKaydetModel.GirilenTutar, OdemeTarih = DateTime.Now, SayacSeriNo = satisKaydetModel.SogukSuOkunan.SayacSeriNo.ToString() });

            reportDataSource.Value = dataSource;
            localReport.DataSources.Add(reportDataSource);

            string mimeType;
            string encoding;
            string fileNameExtension;


            fileNameExtension = "pdf";


            string[] streams;
            Warning[] warnings;
            byte[] renderedByte;

            renderedByte = localReport.Render("PDF", "", out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            string filename = aboneBilgi.ABONENO + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + ".pdf";
            string path = Server.MapPath("~/App_Data/" + filename);
            System.IO.File.WriteAllBytes(path, renderedByte);
            satisKaydetModel.fileName = filename;
            return satisKaydetModel;
            //return File(renderedByte, "PDF", "Makbuz.pdf");
        }
        #endregion


    }
}