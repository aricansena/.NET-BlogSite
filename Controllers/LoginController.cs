using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Blogsite.Filters;
using Blogsite.Models;

namespace Blogsite.Controllers
{
    public class LoginController : Controller
    {
        BlogsiteEntities entity = new BlogsiteEntities();
        public ActionResult Index()
        {
            ViewBag.message = null;
            return View();
        }
        [HttpPost,ExcFilter]
        public ActionResult Index(string KullaniciAd,string Parola)
        {
            Kullanici kullanici = (from k in entity.Kullanici where k.KullaniciAdi == KullaniciAd && k.KullaniciParola == Parola select k).FirstOrDefault();

            if (kullanici!=null)
            {
                Session["Ad"] = kullanici.Ad;
                Session["KullaniciAd"] = kullanici.KullaniciAdi;
                Session["Kullanici_id"] = kullanici.Kullanici_id;
                Session["KullaniciYetkiTur_id"] = kullanici.KullaniciYetkiTur_id;

                switch(kullanici.KullaniciYetkiTur_id)
                {
                    case 1:
                        return RedirectToAction("Anasayfa", "Admin");
                    case 2:
                        return RedirectToAction("Anasayfa", "Kullanici");
                    case 3:
                        return RedirectToAction("Index","Misafir");
                    default:
                        return View();
                }
            }
            else
            {
                ViewBag.message = "Kullanıcı adı ya da parola yanlış";
                return View();
            }
        }
    }
}
