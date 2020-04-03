using OsosOracle.Framework.Utilities.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OsosOracle.Framework.CrossCuttingConcern.Logging
{
    public class ErrorLogger
    {
        public static bool HataLogla(string hata)
        {
            var loglandiMi = false;
            try
            {
                loglandiMi = HataKaydet(hata);
                if (!loglandiMi)
                {
                    loglandiMi = LocalLogYaz(hata);
                }

            }
            catch (Exception ex)
            {
                loglandiMi = false;

            }
            return loglandiMi;
        }

        public static bool HataLogla(Exception hata)
        {
            var exList = new List<Exception> { hata };
            while (hata.InnerException != null)
            {
                hata = hata.InnerException;
                exList.Add(hata);
            }
            string exStr = exList.Aggregate("", (current, ex) => current + ex.ToString());

            //kullanıcı bilgileri varsa eklenmeli
            //hatanı alındığı sayfa varsa eklenmeli


            return HataLogla(exStr);

        }


        public static bool LocalLogYaz(string hataMesaji)
        {
            var uygulamaAdi = ConfigurationManager.AppSettings["UygulamaAdı"];
            try
            {
                string klasorYolu = HttpContext.Current.Server.MapPath("~/App_Data/Logs");


                if (!Directory.Exists(klasorYolu))
                    Directory.CreateDirectory(klasorYolu);

                string logDosyasi = $"{klasorYolu}/{DateTime.Now.ToShortDateString()} - {uygulamaAdi}.txt";

                using (StreamWriter w = File.AppendText(logDosyasi))
                {
                    w.Write("\r\n");
                    w.WriteLine(HttpContext.Current.Request.UrlReferrer);
                    w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                        DateTime.Now.ToLongDateString());
                    w.WriteLine("  :{0}", hataMesaji);
                    w.WriteLine("-------------------------------");
                }
                return true;
            }
            catch (Exception e)
            {

                return false;

            }

        }

        public static bool HataKaydet(string sHata, string sKod = "-")
        {

            sKod = $"Sunucu: {Environment.MachineName} IP: ";//{AktifKullanici.Ip}";

            var obj = new
            {
                HttpContext.Current.Request.HttpMethod,
                HttpContext.Current.Request.UrlReferrer,
                HttpContext.Current.Request.RawUrl,
                HttpContext.Current.Request.Browser.Browser,
                HttpContext.Current.Request.Browser.Version,
                HttpContext.Current.Request.QueryString,
                HttpContext.Current.Request.Form,
                HttpContext.Current.Request.UserHostAddress,
                HttpContext.Current.Request.IsSecureConnection,

            };

            sHata += obj.ToJson();

            if (sHata.Length > 4000)
                sHata = sHata.Substring(0, 3999);

            var uygulamaAdi = "Yonca";// ConfigurationManager.AppSettings["UygulamaAdı"];
            try
            {
                string klasorYolu = HttpContext.Current.Server.MapPath("~/App_Data/Logs");


                if (!Directory.Exists(klasorYolu))
                    Directory.CreateDirectory(klasorYolu);

                string logDosyasi = $"{klasorYolu}/{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year} - {uygulamaAdi}.txt";

                using (StreamWriter w = File.AppendText(logDosyasi))
                {
                    w.Write("\r\n");
                    w.WriteLine(HttpContext.Current.Request.UrlReferrer);
                    w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                        DateTime.Now.ToLongDateString());
                    w.WriteLine("  :{0}", sHata);
                    w.WriteLine("-------------------------------");
                }
                return true;
            }
            catch (Exception e)
            {

                return false;

            }



            return true;

        }


    }
}
