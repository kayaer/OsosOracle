using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.ENTHUSAYACComplexTypes;
using OsosOracle.Entities.ComplexType.ENTSATISComplexTypes;
using OsosOracle.Entities.ComplexType.SYSMENUComplexTypes;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.MvcUI.Filters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class HomeController : BaseController
    {
        private readonly ISYSMENUService _sysMenuService;
        private readonly IRPTDASHBOARDService _rptDashboardService;

        public HomeController(ISYSMENUService sysMenuService, IRPTDASHBOARDService rptDashboardService)
        {
            _sysMenuService = sysMenuService;
            _rptDashboardService = rptDashboardService;
        }
        public ActionResult Index()
        {
            var user = HttpContext.User.Identity;
            return View();
        }


        [ChildActionOnly]
        public PartialViewResult MenuItems()
        {
            //var menuler = _sysMenuService.DetayGetir();
            //menuler = menuler.Where(x => x.PARENTKAYITNO == null).ToList();
            //foreach (var item in menuler)
            //{
            //    item.Children = _sysMenuService.DetayGetir(new Entities.ComplexType.SYSMENUComplexTypes.SYSMENUAra { PARENTKAYITNO = item.KAYITNO });
            //}

            try
            {
                var menulist = _sysMenuService.YetkiGetir(AktifKullanici.KayitNo);
                List<SYSMENUDetay> menuler = new List<SYSMENUDetay>();
                foreach (var item in menulist)
                {
                    SYSMENUDetay menu = new SYSMENUDetay
                    {
                        KAYITNO = item.KAYITNO,
                        ACTION = item.ACTION,
                        CONTROLLER = item.CONTROLLER,
                        TR = item.TR,
                        ICON = item.ICON,
                        MENUORDER = item.MENUORDER,
                        PARENTKAYITNO = item.PARENTKAYITNO
                    };
                    menuler.Add(menu);

                }
                var parentmenu = _sysMenuService.ParentMenuGetir();
                List<SYSMENUDetay> parentList = new List<SYSMENUDetay>();
                foreach (var item in parentmenu)
                {
                    SYSMENUDetay menu = new SYSMENUDetay
                    {
                        KAYITNO = item.KAYITNO,
                        ACTION = item.ACTION,
                        CONTROLLER = item.CONTROLLER,
                        TR = item.TR,
                        ICON = item.ICON,
                        MENUORDER = item.MENUORDER,
                        PARENTKAYITNO = item.PARENTKAYITNO
                    };
                    parentList.Add(menu);

                }
                foreach (var item in parentList)
                {
                    item.Children = menuler.Where(x => x.PARENTKAYITNO == item.KAYITNO).ToList();
                }
                return PartialView("MenuItems", parentList);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }





        }
        public ActionResult HaberlesenCihazSayisiGetir()
        {
            var kayitlar = _rptDashboardService.Getir(new Entities.ComplexType.RPTDASHBOARDComplexTypes.RPTDASHBOARDAra { KURUMKAYITNO = AktifKullanici.KurumKayitNo }).OrderBy(t => t.TARIH).Take(30);
            var model = kayitlar.Select(t => new { TARIH = t.TARIH.ToString(), t.ADET });
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SatisSayisiGetir()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["AppContext"].ConnectionString;
            OracleConnection con = new OracleConnection();
            con.ConnectionString = ConnectionString;
            con.Open();
            //string sql = "select sa.olusturmaTarih,count(1) from entsatıs sa inner join entsayac s on sa.sayackayitno=s.kayitno where to_date( sa.olusturmaTarih,'DD.MM.YYYY')>to_date(  add_months(sysdate,-1),'DD.MM.YYYY') and s.KURUMKAYITNO=" + AktifKullanici.KurumKayitNo+ " group by sa.olusturmaTarih order by sa.olusturmaTarih";
            string sql = "select olusturmaTarih,count(1) from (select  TO_CHAR( sa.olusturmaTarih, 'dd.mm.yyyy') as olusturmaTarih ,count(1) from entsatıs sa inner join entsayac s on sa.sayackayitno=s.kayitno where to_date(sa.olusturmaTarih,'DD.MM.YYYY')> to_date(add_months(sysdate, -1), 'DD.MM.YYYY')and s.KURUMKAYITNO=" + AktifKullanici.KurumKayitNo + " group by sa.olusturmaTarih order by sa.olusturmaTarih) group by olusturmaTarih";


            OracleDataAdapter oda = new OracleDataAdapter(sql, con);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            con.Close();
            con.Dispose();

            var model = dt.AsEnumerable().ToList().ConvertAll(x => new { TARIH = x.ItemArray[0].ToString(), ADET = x.ItemArray[1] });// kayitlar.Select(t => new { TARIH = t.TARIH.ToString(), t.ADET });
            return Json(model, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ToplamSatisGetir(DtParameterModel dtParameterModel, ENTSATISAra entSatisAra)
        {
            entSatisAra.KurumKayitNo = AktifKullanici.KurumKayitNo;
            string ConnectionString = ConfigurationManager.ConnectionStrings["AppContext"].ConnectionString;
            OracleConnection con = new OracleConnection();
            con.ConnectionString = ConnectionString;
            con.Open();
            string sql = @"select sayac.serıno,
    sayac.kapakserino,
    a.ad,
    a.soyad,
    a.ABONENO,
    s.kredi,
    nesnedeger.ad as SATISTIPI
from entsayac sayac inner join(
select sayackayıtno, abonekayıtno,satistipi,sum(kredı)as kredi
from entsatıs inner join entsayac ss on entsatıs.sayackayıtno=ss.kayıtno  where 1=1 {whr}
group by sayackayıtno,abonekayıtno,satistipi) 
s on sayac.kayıtno = s.sayackayitno
inner join entabone a on s.abonekayıtno = a.kayıtno
ınner joın nesnedeger on s.satistipi=nesnedeger.kayıtno {whr2}";

            string where = "";
            string where2 = "";
            if (entSatisAra.ABONEKAYITNO != null)
            {
                where += " and entsatıs.abonekayitno=" + entSatisAra.ABONEKAYITNO;
            }
            if (entSatisAra.SatisTipi != null)
            {
                where += " and entsatıs.satistipi=" + entSatisAra.SatisTipi;
            }
            if (entSatisAra.SatisTarihBaslangic != null)
            {
                where += " and entsatıs.olusturmaTarih>to_date('" + entSatisAra.SatisTarihBaslangic?.ToShortDateString() + "','dd.mm.yyyy')";
            }
            if (entSatisAra.SatisTarihBitis != null)
            {
                where += " and entsatıs.olusturmaTarih<to_date('" + entSatisAra.SatisTarihBitis?.ToShortDateString() + "','dd.mm.yyyy')";
            }
            if (entSatisAra.KurumKayitNo != null)
            {
                where += " and ss.kurumkayıtno=" + entSatisAra.KurumKayitNo;
            }

            if (!string.IsNullOrEmpty(entSatisAra.Blok))
            {
                where2 += " where a.blok ='" + entSatisAra.Blok+"'";
                
            }
            sql = sql.Replace("{whr}", where);
            sql = sql.Replace("{whr2}", where2);
            OracleDataAdapter oda = new OracleDataAdapter(sql, con);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            con.Close();
            con.Dispose();


            var satisList = from satis in dt.AsEnumerable()
                            select
                            new
                            {
                                KapakSerino = satis.Field<string>("KAPAKSERINO"),
                                SayacSeriNo = satis.Field<string>("SERINO"),
                                AdSoyad = satis.Field<string>("AD") + "  " + satis.Field<string>("SOYAD"),
                                AboneNo = satis.Field<string>("ABONENO"),
                                Kredi = satis.Field<decimal>("KREDI"),
                                SatisTipi=satis.Field<string>("SATISTIPI")
                            };

            return Json(new DataTableResult()
            {
                data = satisList,
                draw = dtParameterModel.Draw,
                recordsTotal = dt.Rows.Count,
                recordsFiltered = dt.Rows.Count
            }, JsonRequestBehavior.AllowGet);
        }


    }
}
