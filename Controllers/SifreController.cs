using Blogsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blogsite.Controllers
{
    public class SifreController : Controller
    {
        BlogsiteEntities entity = new BlogsiteEntities();
        // GET: Sifre
        public ActionResult Index()
        {
            int kullanici_id = Convert.ToInt32(Session["Kullanici_id"]);

            if (kullanici_id == 0) return RedirectToAction("Index", "login");

            var kullanici=(from k in entity.Kullanici where k.Kullanici_id == kullanici_id select k).FirstOrDefault();

            ViewBag.mesaj = null;
            ViewBag.stil=null;
            ViewBag.yethiTurId=null;

            return View(kullanici);
        }

        [HttpPost]
        public ActionResult Index(int Kullanici_id, string eskiParola,string yeniParola,string yeniParolaKontrol)
        {
            var kullanici=(from k in entity.Kullanici where k.Kullanici_id==Kullanici_id select k).FirstOrDefault();

            if (eskiParola!=kullanici.KullaniciParola)
            {
                ViewBag.mesaj = "Eski parolanızı kontrol ediniz.";
                ViewBag.stil = "alert alert-danger";
                return View(kullanici);
            }
            if (yeniParola != yeniParolaKontrol)
            {
                ViewBag.mesaj = "Yeni parola ve yeni parola tekrarı eşleşmedi.";
                ViewBag.stil = "alert alert-danger";
                return View(kullanici);
            }
            if (yeniParola.Length<6 || yeniParola.Length>15)
            {
                ViewBag.mesaj = "Yeni parola en az 6 karakter en çok 15 karakter olmalı";
                ViewBag.stil = "alert alert-danger";
                return View(kullanici);
            }

            kullanici.KullaniciParola = yeniParola;
            entity.SaveChanges();

            ViewBag.mesaj = "Parolanız başarıyla değiştirildi.";
            ViewBag.stil = "alert alert-success";
            ViewBag.yetkiTurId = kullanici.KullaniciYetkiTur_id;

            return View(kullanici);
            // TempData["bilgi"]=

        }
    }
}