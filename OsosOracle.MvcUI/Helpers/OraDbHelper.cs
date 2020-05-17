using Oracle.ManagedDataAccess.Client;
using OsosOracle.MvcUI.Models.ReportModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Helpers
{
    public class OraDbHelper
    {
        public static DataTable GunlukSatisSayisiGetir(int kurumKayitNo)
        {
            using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["AppContext"].ConnectionString))
            {
                con.Open();
                string sql = "select olusturmaTarih,count(1) from (select  TO_CHAR( sa.olusturmaTarih, 'dd.mm.yyyy') as olusturmaTarih ,count(1) from entsatıs sa inner join entsayac s on sa.sayackayitno=s.kayitno where to_date(sa.olusturmaTarih,'DD.MM.YYYY')> to_date(add_months(sysdate, -1), 'DD.MM.YYYY')and s.KURUMKAYITNO=" + kurumKayitNo + " group by sa.olusturmaTarih order by sa.olusturmaTarih) group by olusturmaTarih order by olusturmaTarih";
                OracleDataAdapter oda = new OracleDataAdapter(sql, con);
                DataTable dt = new DataTable();
                oda.Fill(dt);
                return dt;

            }
        }
        public static int SayacSayisiGetir(int kurumKayitNo)
        {
            using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["AppContext"].ConnectionString))
            {
                con.Open();
                string sql = "SELECT *  FROM ENTSAYAC WHERE KURUMKAYITNO="+kurumKayitNo;
                OracleDataAdapter oda = new OracleDataAdapter(sql, con);
                DataTable dt = new DataTable();
                oda.Fill(dt);
                return dt.Rows.Count;

            }
        }
        public static int AboneSayisiGetir(int kurumKayitNo)
        {
            using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["AppContext"].ConnectionString))
            {
                con.Open();
                string sql = "SELECT *  FROM ENTABONE WHERE KURUMKAYITNO=" + kurumKayitNo;
                OracleDataAdapter oda = new OracleDataAdapter(sql, con);
                DataTable dt = new DataTable();
                oda.Fill(dt);
                return dt.Rows.Count;

            }
        }
        public static int SatisSayisiGetir(int kurumKayitNo)
        {
            using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["AppContext"].ConnectionString))
            {
                con.Open();
                string sql = "SELECT * FROM ENTSATIS INNER JOIN ENTSAYAC ON ENTSATIS.SAYACKAYITNO=ENTSAYAC.KAYITNO WHERE ENTSAYAC.KURUMKAYITNO=" + kurumKayitNo;
                OracleDataAdapter oda = new OracleDataAdapter(sql, con);
                DataTable dt = new DataTable();
                oda.Fill(dt);
                return dt.Rows.Count;

            }
        }
        public static int KullaniciSayisiGetir(int kurumKayitNo)
        {
            using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["AppContext"].ConnectionString))
            {
                con.Open();
                string sql = "SELECT * FROM SYSKULLANICI WHERE KURUMKAYITNO=" + kurumKayitNo;
                OracleDataAdapter oda = new OracleDataAdapter(sql, con);
                DataTable dt = new DataTable();
                oda.Fill(dt);
                return dt.Rows.Count;

            }
        }

        public static DataTable YapilanSonSatisGetir()
        {
            using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["AppContext"].ConnectionString))
            {
                con.Open();
                string sql = "SELECT * FROM ENTSATIS WHERE ROWNUM=1 ORDER BY KAYITNO DESC ";
                OracleDataAdapter oda = new OracleDataAdapter(sql, con);
                DataTable dt = new DataTable();
                oda.Fill(dt);
                return dt;

            }
        }

        public static DataTable GunlukSatisRaporu(RaporParametreModel model)
        {
            using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["AppContext"].ConnectionString))
            {
                con.Open();
                string sql = @"SELECT SATISTIPI.AD AS SATISTIPI,ODEMETIPI.ad AS ODEMETIPI, CSTSAYACMODEL.AD AS SAYACMODEL,ENTSAYAC.KAPAKSERINO,ENTABONE.ABONENO,ENTABONE.AD,ENTABONE.SOYAD, ENTSATIS.OLUSTURMATARIH,ENTSATIS.KREDI,ENTSATIS.KDV,ENTSATIS.CTV,ENTSATIS.AYLIKBAKIMBEDELI,ENTSATIS.ODEME FROM ENTSATIS INNER JOIN ENTSAYAC ON ENTSATIS.SAYACKAYITNO=ENTSAYAC.KAYITNO
                       INNER JOIN ENTABONE ON ENTSATIS.ABONEKAYITNO=ENTABONE.KAYITNO
                       INNER JOIN CSTSAYACMODEL ON ENTSAYAC.SAYACMODELKAYITNO=CSTSAYACMODEL.KAYITNO
                       INNER JOIN NESNEDEGER SATISTIPI ON ENTSATIS.SATISTIPI=SATISTIPI.KAYITNO
                       LEFT JOIN NESNEDEGER ODEMETIPI ON  ENTSATIS.ODEMETIPIKAYITNO=ODEMETIPI.KAYITNO
WHERE 1=1 {whr}
                       ORDER BY CSTSAYACMODEL.AD,ENTSATIS.OLUSTURMATARIH ";

                string where = " and entsayac.kurumkayıtno=" + model.KurumKayitNo;
                if (model.SatisTarihiBaslangic != null)
                {
                    where += "and entsatıs.olusturmaTarih>to_date('" + model.SatisTarihiBaslangic + "','dd.mm.yyyy')";
                }
                if (model.SatisTarihiBitis != null)
                {
                    where += " and entsatıs.olusturmaTarih<to_date('" + model.SatisTarihiBitis + "','dd.mm.yyyy')";
                }
                if (model.SatisTipi != null)
                {
                    where += " and entsatis.satistipi=" + model.SatisTipi;
                }
                if (model.SayacModelId != null)
                {
                    where += " and entsayac.sayacmodelkayıtno=" + model.SayacModelId;
                }
               
                sql = sql.Replace("{whr}", where); 
                OracleDataAdapter oda = new OracleDataAdapter(sql, con);
                DataTable dt = new DataTable();
                oda.Fill(dt);
                return dt;

            }
        }
    }
}