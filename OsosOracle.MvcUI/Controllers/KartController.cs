using OsosOracle.Framework.Web.Mvc;
using OsosOracle.MvcUI.Filters;
using OsosOracle.MvcUI.Models.ENTSATISModels;
using OsosOracle.MvcUI.Models.ENTSATISModels.Yeni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OsosOracle.MvcUI.Controllers
{
    [AuthorizeUser]
    public class KartController : BaseController
    {
        
        public ActionResult Index()
        {
            return View("Index", new SatisModel());
        }

        public JsonResult SatisPars(SatisModel model)
        {
            if (string.IsNullOrEmpty(model.HamData))
            {
                model.HamData = "E#1#20191004#0/0/0#0#0#b#b#1#0#0#0#1#0#b#b#b#b#b#0#0#0#0#0#65535#65535#65535#65535#0#1#3#1#0#|G#1#20191004#0/0/0#0#0#b#b#1#0#0#1#0#b#b#b#b#b#b#b#0#0#1#3#1#0#|S#1#20191004#0/0/0#0#0#b#b#1#0#0#0#0#1#0#1#1#65527#65535#b#b#b#b#b#b#b#b#0#1#3#1#0#|K#1#20191004#0/0/0#0#0#b#b#1#0#0#0#1#0#b#b#b#b#0#0#0#0#0#0#0#1#3#1#0#|";
            }
            model.SuSatisModel.SogukSuOkunan = new SogukSuOkunan(model.HamData);
            //model.SuSatisModel.AboneSayacDetay = _entAboneSayacService.DetayGetir(new ENTABONESAYACAra { SayacSeriNo = model.SuSatisModel.SogukSuOkunan.SayacSeriNo, SayacTur = 1, Durum = 1 }).FirstOrDefault();
            //model.SuSatisModel.PrmTarifeSuDetay = _prmTarifeSuService.DetayGetir(new PRMTARIFESUAra { KAYITNO = model.SuSatisModel.AboneSayacDetay.TARIFEKAYITNO }).FirstOrDefault();

            model.KalorimetreSatisModel.KalorimetreOkunan = new KalorimetreOkunan(model.HamData);
            //model.KalorimetreSatisModel.AboneSayacDetay = _entAboneSayacService.DetayGetir(new ENTABONESAYACAra { SayacSeriNo = model.KalorimetreSatisModel.KalorimetreOkunan.CihazNo, SayacTur = 2, Durum = 1 }).FirstOrDefault();
            //model.KalorimetreSatisModel.PrmTarifeKALORIMETREDetay = _prmKALORIMETREService.DetayGetir(new PRMTARIFEKALORIMETREAra { KAYITNO = model.KalorimetreSatisModel.AboneSayacDetay.TARIFEKAYITNO }).FirstOrDefault();

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}