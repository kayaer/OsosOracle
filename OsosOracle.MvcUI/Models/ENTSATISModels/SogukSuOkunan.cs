using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.MvcUI.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.ENTSATISModels
{
    public class SogukSuOkunan
    {
        public SogukSuOkunan()
        {

        }
        public SogukSuOkunan(string hamdata)
        {

            string[] dataArray = hamdata.Split("|".ToCharArray());
            string[] suData = dataArray[2].Split('#');
            SayacSeriNo = suData[2];
            Kredi = suData[4];
            YedekKredi = suData[5];
            Ako = suData[6];
            if (Ako != "*" && Ako != "b")
            {
                throw new NotificationException("Okundu bilgisi alınamadı");
            }
            Yko = suData[7];
            if (Yko != "*" && Yko != "b")
            {
                throw new NotificationException("Okundu bilgisi alınamadı");
            }
            Kalan = suData[9];
            Harcanan = suData[10];

        }

        public string TarifeAdi { get; set; }
        public string AboneNo { get; set; }
        public string KartNo { get; set; }
        public string SayacSeriNo { get; set; }
        public string Kredi { get; set; }
        public string YedekKredi { get; set; }
        public string Ako { get; set; }

        public string AkoMesaj { get => Ako == "*" ? Dil.Okundu : Dil.Okunmadı; }
        public string Yko { get; set; }
        public string YkoMesaj { get => Yko == "*" ? Dil.Okundu : Dil.Okunmadı; }
        public string YeniKart { get; set; }
        public string Kalan { get; set; }
        public string Harcanan { get; set; }
    }
}