using OsosOracle.Business.Abstract;
using OsosOracle.Entities.ComplexType.SYSKULLANICIComplexTypes;
using OsosOracle.Framework.Entities;
using OsosOracle.Framework.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OsosOracle.MvcUI.Controllers
{
    public class LoginController : BaseController
    {
        private readonly ISYSKULLANICIService _sysKullaniciService;

        public LoginController(ISYSKULLANICIService sysKullaniciService)
        {
            _sysKullaniciService = sysKullaniciService;
        }

        public ActionResult Index(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(SYSKULLANICIDetay model, string ReturnUrl)
        {

            var kullanici = _sysKullaniciService.DetayGetir(new SYSKULLANICIAra { KULLANICIAD = model.KULLANICIAD, SIFRE = model.SIFRE }).FirstOrDefault();

            if (kullanici!= null)
            {

                switch (kullanici.DIL)
                {
                    case 3:
                        Session["CurrentCulture"] = "ar";
                        break;
                    case 1:
                        Session["CurrentCulture"] = "en";
                        break;

                    case 2:
                        Session["CurrentCulture"] = "tr";
                        break;

                }


                AktifKullanici = new AktifKullanici { KullaniciAd=kullanici.KULLANICIAD,Dil=kullanici.DIL,Ad=kullanici.AD,Soyad=kullanici.SOYAD,KurumAdi=kullanici.KurumAdi,KurumKayitNo=kullanici.KURUMKAYITNO,KayitNo=kullanici.KAYITNO,KurumDllAdi=kullanici.KurumDllAdi};
                System.Web.HttpCookie cookie = new System.Web.HttpCookie("Yonca", model.KULLANICIAD);
                cookie.Expires = DateTime.Now.AddMonths(3);
                Response.Cookies.Add(cookie);

                

                if (!string.IsNullOrEmpty(ReturnUrl))
                    return Redirect(ReturnUrl);
                return RedirectToAction("Index", "Home", new { area = "" });


            }
            else
            {
                ModelState.AddModelError("", "Hatalı Kullanıcı Adı Yada Şifre");
                return View("Index");
            }

        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}