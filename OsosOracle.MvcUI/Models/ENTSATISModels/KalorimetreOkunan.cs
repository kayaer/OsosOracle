using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.ENTSATISModels
{
    public class KalorimetreOkunan
    {
        public KalorimetreOkunan()
        {

        }
        public KalorimetreOkunan(string hamdata)
        {
            string[] dataArray = hamdata.Split("|".ToCharArray());
            string[] kalorimetreData = dataArray[3].Split('#');
            CihazNo = kalorimetreData[2];
            Kredi = kalorimetreData[4];
            YedekKredi = kalorimetreData[5];
            Ako = kalorimetreData[6];

        }
        public string TarifeAdi { get; set; }
        public string AboneNo { get; set; }
        public string CihazNo { get; set; }
        public string Kredi { get; set; }
        public string YedekKredi { get; set; }
        public string Ako { get; set; }
        public string Kalan { get; set; }
        public string Harcanan { get; set; }
        public string AkoMesaj { get => Ako == "*" ? "okundu" : "okunmadı"; }


    }
}