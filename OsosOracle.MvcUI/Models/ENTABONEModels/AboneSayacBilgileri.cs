using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.ENTABONEModels
{
    public class AboneSayacBilgileri
    {
        public AboneSayacBilgileri()
        {

        }
        public AboneSayacBilgileri(string hamdata)
        {

            string[] dataArray = hamdata.Split("|".ToCharArray());
            string[] suData = dataArray[2].Split('#');
            SuSayacSeriNo = suData[2];
            SuKredi = suData[4];
            SuYedekKredi = suData[5];
            SuAko = suData[6];
            if (SuAko != "*" && SuAko != "b")
            {
                throw new NotificationException("Okundu bilgisi alınamadı");
            }
            SuYko = suData[7];
            if (SuYko != "*" && SuYko != "b")
            {
                throw new NotificationException("Okundu bilgisi alınamadı");
            }
            SuKalan = suData[9];
            SuHarcanan = suData[10];
            string[] kalorimetreData = dataArray[3].Split('#');
            KalorimetreSayacSeriNo = kalorimetreData[2];
            KalorimetreKredi = kalorimetreData[4];
            KalorimetreYedekKredi = kalorimetreData[5];
            KalorimetreAko = kalorimetreData[6];
            if (KalorimetreAko != "*" && KalorimetreAko != "b")
            {
                throw new NotificationException("Okundu bilgisi alınamadı");
            }
            KalorimetreYko = kalorimetreData[7];
        }

        public string TarifeAdi { get; set; }
        public string AboneNo { get; set; }
        public string KartNo { get; set; }
        public string SuSayacSeriNo { get; set; }
        public string SuAko { get; set; }
        public string SuYko { get; set; }
        public string SuKredi { get; set; }
        public string SuYedekKredi { get; set; }
        public string SuKalan { get; set; }
        public string SuHarcanan { get; set; }
        public string KalorimetreSayacSeriNo { get; set; }

        public string KalorimetreAko { get; set; }
        public string KalorimetreYko { get; set; }
        public string KalorimetreKredi { get; set; }
        public string KalorimetreYedekKredi { get; set; }

        public int AboneKayitNo { get; set; }
        public int SuSayacKayitNo { get; set; }
        public int KalorimetreKayitNo { get; set; }
    }
}