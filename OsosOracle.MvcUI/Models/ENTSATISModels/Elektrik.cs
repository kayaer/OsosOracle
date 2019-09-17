using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.ENTSATISModels
{
    public  class Elektrik
    {

        public static int YuklemeLimitiHesaplaElektrik(int yuklemeLimiti)
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

      

        //Yaz sınıfları hesaplamaların yapılıp karta yazılıcak değerleri taşır
        //Okunan sınıfları karttan okunan bilgilerin pars edilmiş haşşerini taşır
        //Modal sınıfları ise veritabanından tarife bilgilerini taşır
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
                sayacBilgi = string.Format("YeniKartUyari", "Elektrik");
            }
            else if (miktar > model.PrmTarifeElkDetay.YEDEKKREDI || (model.ElkKartOkunan.YedekKredi > 0 && model.ElkKartOkunan.YedekKrediKontrol == false))
            {
                if (model.ElkKartOkunan.AnaKrediKontrol == false) toplamYuklenecekMiktar += model.ElkKartOkunan.AnaKredi;
                if (model.ElkKartOkunan.YedekKrediKontrol == false) toplamYuklenecekMiktar += model.ElkKartOkunan.YedekKredi;

                model.ElkKartYuklenecek.AnaKredi = Convert.ToInt64( toplamYuklenecekMiktar - model.PrmTarifeElkDetay.YEDEKKREDI);

                model.ElkKartYuklenecek.YedekKredi = Convert.ToInt64(model.PrmTarifeElkDetay.YEDEKKREDI);

                //model.SatisTur = 0;
            }
            else
            {
                sayacBilgi = string.Format("YuklenenKrediMiktariYedekKredidenYuksekOlmali", model.YedekKredi);
            }

            model.ElkKartYuklenecek.AksamSaati = Convert.ToDecimal(model.PrmTarifeElkDetay.AKSAMSAAT);
            model.ElkKartYuklenecek.SabahSaati = Convert.ToDecimal(model.PrmTarifeElkDetay.SABAHSAAT);
            model.ElkKartYuklenecek.SayacSeriNo = model.ElkKartOkunan.SayacSeriNo;
            //model.ElkKartYuklenecek.TarifeTip = model.PrmTarifeElkDetay.TarifeTip;
            model.ElkKartYuklenecek.Tatil1Ay = model.PrmTarifeElkDetay.BAYRAM1AY;
            model.ElkKartYuklenecek.Tatil1Gun = model.PrmTarifeElkDetay.BAYRAM1GUN;
            model.ElkKartYuklenecek.Tatil1Sure = model.PrmTarifeElkDetay.BAYRAM1SURE;
            model.ElkKartYuklenecek.Tatil2Ay = model.PrmTarifeElkDetay.BAYRAM2AY;
            model.ElkKartYuklenecek.Tatil2Gun = model.PrmTarifeElkDetay.BAYRAM2GUN;
            model.ElkKartYuklenecek.Tatil2Sure = model.PrmTarifeElkDetay.BAYRAM2SURE;
            model.ElkKartYuklenecek.FixCharge = model.PrmTarifeElkDetay.SABITUCRET;

            model.ElkKartYuklenecek.KritikKredi = model.PrmTarifeElkDetay.KRITIKKREDI;
            model.ElkKartYuklenecek.YuklemeLimiti = YuklemeLimitiHesaplaElektrik(model.PrmTarifeElkDetay.YUKLEMELIMIT);
            model.ElkKartYuklenecek.Fiyat1 = model.PrmTarifeElkDetay.FIYAT1 * katsayiElk;
            model.ElkKartYuklenecek.Fiyat2 = model.PrmTarifeElkDetay.FIYAT2 * katsayiElk;
            model.ElkKartYuklenecek.Fiyat3 = model.PrmTarifeElkDetay.FIYAT3 * katsayiElk;
            model.ElkKartYuklenecek.Limit1 = Convert.ToInt64(model.PrmTarifeElkDetay.LIMIT1);
            model.ElkKartYuklenecek.Limit2 = Convert.ToInt64(model.PrmTarifeElkDetay.LIMIT2);

            //Tuple<TarifeElk, decimal, string> res = new Tuple<TarifeElk, decimal, string>(model.ElektrikYuklenecek, miktar, sayacBilgi);

            return model;

        }

        //public static Tuple<TarifeElk, decimal, string> SatisIptalEt(AboneSatisBilgiViewModel model)
        //{
        //    #region sabit
        //    model.ElektrikYuklenecek.AksamSaati = model.ElektrikModal.AksamSaati;
        //    model.ElektrikYuklenecek.SabahSaati = model.ElektrikModal.SabahSaati;
        //    model.ElektrikYuklenecek.SayacSeriNo = model.ElektrikModal.SayacSeriNo;
        //    model.ElektrikYuklenecek.TarifeTip = model.ElektrikModal.TarifeTip;
        //    model.ElektrikYuklenecek.Tatil1Ay = model.ElektrikModal.Tatil1Ay;
        //    model.ElektrikYuklenecek.Tatil1Gun = model.ElektrikModal.Tatil1Gun;
        //    model.ElektrikYuklenecek.Tatil1Sure = model.ElektrikModal.Tatil1Sure;
        //    model.ElektrikYuklenecek.Tatil2Ay = model.ElektrikModal.Tatil2Ay;
        //    model.ElektrikYuklenecek.Tatil2Gun = model.ElektrikModal.Tatil2Gun;
        //    model.ElektrikYuklenecek.Tatil2Sure = model.ElektrikModal.Tatil2Sure;
        //    model.ElektrikYuklenecek.FixCharge = model.ElektrikModal.FixCharge;

        //    model.ElektrikYuklenecek.KritikKredi = model.ElektrikModal.KritikKredi;
        //    model.ElektrikYuklenecek.YuklemeLimiti = YuklemeLimitiHesaplaElektrik(model.ElektrikModal.YuklemeLimiti);
        //    model.ElektrikYuklenecek.Fiyat1 = model.ElektrikModal.Fiyat1 * model.ElektrikModal.KrediKatsayisi;
        //    model.ElektrikYuklenecek.Fiyat2 = model.ElektrikModal.Fiyat2 * model.ElektrikModal.KrediKatsayisi;
        //    model.ElektrikYuklenecek.Fiyat3 = model.ElektrikModal.Fiyat3 * model.ElektrikModal.KrediKatsayisi;
        //    model.ElektrikYuklenecek.Limit1 = model.ElektrikModal.Limit1;
        //    model.ElektrikYuklenecek.Limit2 = model.ElektrikModal.Limit2;
        //    #endregion

        //    string sayacBilgi = string.Empty;

        //    var toplamYuklenecekMiktar = Convert.ToDecimal(model.SatisIptalBilgisi.Kredi + model.SatisIptalBilgisi.YedekKredi);

        //    if (model.ElektrikOkunan.AnaKrediKontrol == false)
        //    {
        //        model.ElektrikYuklenecek.AnaKredi = model.ElektrikOkunan.AnaKredi - (int)model.SatisIptalBilgisi.AnaKredi;

        //        model.ElektrikYuklenecek.YedekKredi = model.ElektrikOkunan.YedekKredi - (int)model.SatisIptalBilgisi.YedekKredi;

        //        model.SatisTur = 1;

        //    }
        //    else if (model.ElektrikOkunan.AnaKrediKontrol == true)
        //    {
        //        sayacBilgi = Dil.IptalIslemiGerceklestirilemezUyari;
        //    }

        //    Tuple<TarifeElk, decimal, string> res = new Tuple<TarifeElk, decimal, string>(model.ElektrikYuklenecek, toplamYuklenecekMiktar, sayacBilgi);

        //    return res;

        //}

    }
}