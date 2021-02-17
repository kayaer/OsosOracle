using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.ENTSATISModels
{
    public class GazOkunan
    {
        public GazOkunan()
        {

        }
        public GazOkunan(string hamdata)
        {
            //string[] dataArray = hamdata.Split("|".ToCharArray());
            string[] gazData = hamdata.Split('|');
            CihazNo = gazData[1];
            AboneNo = gazData[2];
            Ako = gazData[3];
            Kredi = gazData[4];
            Yko = gazData[5];
            YedekKredi = gazData[6];
            Kalan = gazData[7];
            Harcanan = gazData[8];
            

        }

        public GazOkunan ParsData()
        {
            string[] gazData = HamData.Split('|');
            CihazNo = gazData[1];
            AboneNo = gazData[2];
            Ako = gazData[3];
            Kredi = gazData[4];
            Yko = gazData[5];
            YedekKredi = gazData[6];
            Kalan = gazData[7];
            Harcanan = gazData[8];
            return this;
        }
        public string HamData { get; set; }
        public string TarifeAdi { get; set; }
        public string AboneNo { get; set; }
        public string CihazNo { get; set; }
        public string Kredi { get; set; }
        public string YedekKredi { get; set; }
        public string Ako { get; set; }
        public string AnaKrediOkunduBilgisi { get; set; }
        public string Yko { get; set; }
        public string YedekKrediOkunduBilgisi { get; set; }
        public string Kalan { get; set; }
        public string Harcanan { get; set; }
        public string Cap { get; set; }


    }
}