using Blogsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blogsite.Controllers
{
    public class MisafirController : Controller
    {
        BlogsiteEntities entity = new BlogsiteEntities();
        // GET: Misafir
        public ActionResult Index()
        {
            int yetkiTurId = Convert.ToInt32(Session["KullaniciYetkiTur_id"]);

            if (yetkiTurId == 3)
            {
                int kullaniciId = Convert.ToInt32(Session["Kullanici_id"]);
                var Kullanici = (from k in entity.Kullanici where k.Kullanici_id == kullaniciId select k).FirstOrDefault();

                ViewBag.AdminAdSoyad = Kullanici.Ad + "  " + Kullanici.Soyad;

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");

            }
        }
    }
}