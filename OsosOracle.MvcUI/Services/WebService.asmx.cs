using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;

namespace OsosOracle.MvcUI.Services
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["AppContext"].ConnectionString;


        [WebMethod]
        public bool DurumSorgula(string macid, string macname)
        {
            try
            {
                string durum="0";
               
                OracleConnection con = new OracleConnection();
                con.ConnectionString = ConnectionString;
                con.Open();
                string sql = "select status from cınıgaz_dll_mac where MAC_ID = '" + macid + "' and MAC_NAME='" + macname + "'";
                OracleDataAdapter oda = new OracleDataAdapter(sql, con);
                DataTable dt = new DataTable();
                oda.Fill(dt);
                con.Close();
                con.Dispose();

                if (dt.Rows.Count > 0)
                {
                    durum = dt.Rows[0][0].ToString();
                }
                else
                {
                    insertDurum(macid, macname,1);
                    return true;
                }

              
              
                if (durum == "1")
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public void insertDurum(string macid, string macname,int status)
        {
            using (var con = new OracleConnection(ConnectionString))
            {
                con.Open();


                OracleParameter macidp = new OracleParameter();
                macidp.OracleDbType = OracleDbType.Varchar2;
                macidp.Value = macid;

                OracleParameter macnamep = new OracleParameter();
                macnamep.OracleDbType = OracleDbType.Varchar2;
                macnamep.Value = macname;

                OracleParameter statusp = new OracleParameter();
                statusp.OracleDbType = OracleDbType.Int16;
                statusp.Value = status;



                // create command and set properties
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandText = "insert into CINIGAZ_DLL_MAC(MAC_ID, MAC_NAME,STATUS) values (:macid, :macname,:status)";

                cmd.Parameters.Add(macidp);
                cmd.Parameters.Add(macnamep);
                cmd.Parameters.Add(statusp);
              
                cmd.ExecuteNonQuery();
            }
        }

        [WebMethod]
        public bool LogEkle(int devno, string anakredi, string islemtipi, string sonuc, string mac)
        {
            try
            {

                using (var con = new OracleConnection(ConnectionString))
                {
                    con.Open();


                    OracleParameter devnop = new OracleParameter();
                    devnop.OracleDbType = OracleDbType.Int32;
                    devnop.Value = devno;

                    OracleParameter anakredip = new OracleParameter();
                    anakredip.OracleDbType = OracleDbType.Int32;
                    anakredip.Value = anakredi;

                    OracleParameter islemtipip = new OracleParameter();
                    islemtipip.OracleDbType = OracleDbType.Varchar2;
                    islemtipip.Value = islemtipi;

                    OracleParameter sonucp = new OracleParameter();
                    sonucp.OracleDbType = OracleDbType.Varchar2;
                    sonucp.Value = sonuc;


                    OracleParameter macp = new OracleParameter();
                    macp.OracleDbType = OracleDbType.Varchar2;
                    macp.Value = mac;

                    // create command and set properties
                    OracleCommand cmd = con.CreateCommand();
                    cmd.CommandText = "insert into CINIGAZ_DLL_LOG(DEV_NO, ANA_KREDI,PROCESS_TYPE,RESULT_SC,MAC_ID) values (:devno, :anakredi,:islemtipi, :sonuc,:mac)";

                    cmd.Parameters.Add(devnop);
                    cmd.Parameters.Add(anakredip);
                    cmd.Parameters.Add(islemtipip);
                    cmd.Parameters.Add(sonucp);
                    cmd.Parameters.Add(macp);
                    cmd.ExecuteNonQuery();
                   
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
