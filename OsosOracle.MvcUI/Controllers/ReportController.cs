using Microsoft.Reporting.WebForms;
using Oracle.ManagedDataAccess.Client;
using OsosOracle.Entities.Enums;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.MvcUI.Filters;
using OsosOracle.MvcUI.Helpers;
using OsosOracle.MvcUI.Models.ReportModels;
using OsosOracle.MvcUI.Reports.ReportModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class ReportController : BaseController
    {

        public ActionResult Index()
        {
            return View(new RaporParametreModel());
        }
        public ActionResult SatisRaporu()
        {
            return View(new RaporParametreModel() { SatisTipi = enumSatisTipi.Satis.GetHashCode() });

        }

        [HttpPost]
        public JsonResult RaporAl(RaporParametreModel model)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["AppContext"].ConnectionString;
            OracleConnection con = new OracleConnection();
            con.ConnectionString = ConnectionString;
            con.Open();
            string sql = @"select satistarihi,sum(kredi)AS KREDI,sum(KDV)AS KDV,sum(ctv) AS CTV, SUM(AYLIKBAKIMBEDELI)AS AYLIKBAKIMBEDELI,SUM(ODEME)AS TUTAR from (  select to_char( entsatis.olusturmatarih,'DD.MM.YYYY')as satistarihi ,entsatis.kredi,entsatis.kdv,entsatis.ctv,entsatis.aylikbakımbedeli,entsatis.odeme from entsatıs inner join entsayac on entsatis.sayackayitno=entsayac.kayitno
                       inner join entabone on entsatis.abonekayitno=entabone.kayitno  where entsatis.satistipi=5 {whr})
                    
                     group by satistarihi order by satistarihi";

            string where = " and entsayac.kurumkayıtno=" + AktifKullanici.KurumKayitNo;
            if (model.SatisTarihiBaslangic != null)
            {
                where += "and entsatıs.olusturmaTarih>to_date('" + model.SatisTarihiBaslangic + "','dd.mm.yyyy')";
            }
            if (model.SatisTarihiBitis != null)
            {
                where += " and entsatıs.olusturmaTarih<to_date('" + model.SatisTarihiBitis + "','dd.mm.yyyy')";
            }
            sql = sql.Replace("{whr}", where);

            OracleDataAdapter oda = new OracleDataAdapter(sql, con);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            con.Close();
            con.Dispose();

            List<GunlukSatisRaporuDataSet> dataSet = new List<GunlukSatisRaporuDataSet>();
            foreach (DataRow dr in dt.Rows)
            {
                GunlukSatisRaporuDataSet data = new GunlukSatisRaporuDataSet();
                data.Tarih = dr["SATISTARIHI"].ToString();
                data.Kredi = dr["KREDI"].ToString();
                data.Ctv = dr["CTV"].ToString();
                data.AylikBakimBedeli = dr["AYLIKBAKIMBEDELI"].ToString();
                data.Tutar = dr["TUTAR"].ToString();
                data.Kdv = dr["KDV"].ToString();
                dataSet.Add(data);
            }

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "GunlukSatisRaporu.rdlc");
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
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = dataSet;
            lr.DataSources.Add(reportDataSource1);
            renderedBytes = lr.Render("PDF", deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warning);

            string filename = DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + "GunlukSatisRaporu.pdf";
            path = Server.MapPath("~/App_Data/" + filename);
            System.IO.File.WriteAllBytes(path, renderedBytes);
            return Json(filename, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public JsonResult SatisRaporu(RaporParametreModel model)
        {
            model.KurumKayitNo = AktifKullanici.KurumKayitNo;
            DataTable dt = OraDbHelper.GunlukSatisRaporu(model);
            List<Satis> dataSet = new List<Satis>();
            foreach (DataRow dr in dt.Rows)
            {
                Satis data = new Satis();
                data.SatisTarihi = dr["OLUSTURMATARIH"].ToString();
                data.Kredi = !string.IsNullOrEmpty(dr["KREDI"].ToString()) ? Convert.ToDecimal(dr["KREDI"].ToString()) : 0;
                data.Ctv = !string.IsNullOrEmpty(dr["CTV"].ToString()) ? Convert.ToDecimal(dr["CTV"].ToString()) : 0;
                data.AylikBakimBedeli = !string.IsNullOrEmpty(dr["AYLIKBAKIMBEDELI"].ToString()) ? Convert.ToDecimal(dr["AYLIKBAKIMBEDELI"].ToString()) : 0;
                data.Tutar = !string.IsNullOrEmpty(dr["ODEME"].ToString()) ? Convert.ToDecimal(dr["ODEME"].ToString()) : 0;
                data.Kdv = !string.IsNullOrEmpty(dr["KDV"].ToString()) ? Convert.ToDecimal(dr["KDV"].ToString()) : 0;
                data.KapakSeriNo = dr["KAPAKSERINO"].ToString();
                data.AboneNo = dr["ABONENO"].ToString();
                data.AdiSoyadi = dr["AD"].ToString() + " " + dr["SOYAD"].ToString();
                data.SayacModeli = dr["SAYACMODEL"].ToString();
                data.SatisTipi = dr["SATISTIPI"].ToString();
                data.OdemeTipi = dr["ODEMETIPI"].ToString();
                dataSet.Add(data);
            }

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "SatisRaporu.rdlc");
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
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = dataSet;
            lr.DataSources.Add(reportDataSource1);
            renderedBytes = lr.Render("PDF", deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warning);

            string filename = DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + "GunlukSatisRaporu.pdf";
            path = Server.MapPath("~/App_Data/" + filename);
            System.IO.File.WriteAllBytes(path, renderedBytes);
            return Json(filename, JsonRequestBehavior.AllowGet);


        }
    }
}