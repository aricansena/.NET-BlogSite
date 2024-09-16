using Blogsite.Filters;
using Blogsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blogsite.Controllers
{
    [AuthFilter(2)]
    public class KullaniciController : Controller
    {
        BlogsiteEntities entity = new BlogsiteEntities();
        // GET: Kullanici
        public ActionResult Index()
        {
            int kullaniciId = Convert.ToInt32(Session["Kullanici_id"]);
            var Kullanici = (from k in entity.Kullanici where k.Kullanici_id == kullaniciId select k).FirstOrDefault();

            ViewBag.AdSoyad = Kullanici.Ad + "  " + Kullanici.Soyad;
            ViewBag.EPosta = Kullanici.EPosta;
            ViewBag.TelefonNo = Kullanici.TelefonNo;
            return View();
        }

        [HttpGet]
        public ActionResult Anasayfa()
        {
            var blog = (from b in entity.Blog
                        join k in entity.Kategori on b.Kategori_id equals k.Kategori_id
                        select b).ToList();

            return View(blog);
        }

        [HttpPost]
        public ActionResult Anasayfa(FormCollection fc)
        {
            int blogId = Convert.ToInt32(fc["Blog_id"]);

            var blog = (from b in entity.Blog where b.Blog_id == blogId select b).ToList();

            return RedirectToAction("BlogIcerik", new { id = blogId });
        }
        public ActionResult BlogIcerik(int id)
        {
            var blog = (from b in entity.Blog
                        where b.Blog_id == id
                        select b).FirstOrDefault();

            return View(blog);
        }
    }

}