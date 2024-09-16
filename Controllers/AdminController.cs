using Blogsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Generator;
using System.Globalization;
using Blogsite.Filters;
using System.Web.Caching;

namespace Blogsite.Controllers
{
    [AuthFilter(1)]
    public class AdminController : Controller
    {
        BlogsiteEntities entity = new BlogsiteEntities();
        public ActionResult Index()
        {
            int kullaniciId = Convert.ToInt32(Session["Kullanici_id"]);
            var Kullanici = (from k in entity.Kullanici where k.Kullanici_id == kullaniciId select k).FirstOrDefault();
            ViewBag.AdSoyad = Kullanici.Ad + "  " + Kullanici.Soyad;
            ViewBag.EPosta = Kullanici.EPosta;
            ViewBag.TelefonNo = Kullanici.TelefonNo;
            return View();
        }

        public ActionResult BlogEkle()
        {
            int Blog_id = Convert.ToInt32(Session["Blog_id"]);
            var blog = (from b in entity.Blog select b).ToList();
            ViewBag.blogsite = blog;
            int Kategori_id = Convert.ToInt32(Session["Kategori_id"]);
            var kategoriler = (from kat in entity.Kategori select kat).ToList();
            ViewBag.kategoriler = kategoriler;
            return View();
        }
        [HttpPost]
        [ActFilter("Yeni Blog Eklendi")]
        public ActionResult BlogEkle(string blogBaslik, string blogAciklama, int selectkategori)
        {
            Blog yeniBlog = new Blog();
            yeniBlog.Aciklama = blogAciklama;
            yeniBlog.Baslik = blogBaslik;
            yeniBlog.Kategori_id = selectkategori;
            entity.Blog.Add(yeniBlog);
            entity.SaveChanges();
            return RedirectToAction("Blog", "Admin");
        }
        public ActionResult Blog()
        {
            int Kategori_id = Convert.ToInt32(Session["Kategori_id"]);
            var kategoriler = (from kat in entity.Kategori select kat).ToList();
            ViewBag.kategoriler = kategoriler;
            int Blog_id = Convert.ToInt32(Session["Blog_id"]);
            var blog = (from b in entity.Blog select b).ToList();
            ViewBag.blogsite = blog;
            return View();
        }
        [HttpPost]
        public ActionResult Blog(int selectkategori)
        {
            var secilenkategori = (from kat in entity.Kategori where kat.Kategori_id == selectkategori select kat).FirstOrDefault();
            TempData["secilen"] = secilenkategori;
            return RedirectToAction("Liste", "Admin");
        }

        [HttpGet]
        public ActionResult Liste()
        {
            Kategori secilenkategori = (Kategori)TempData["secilen"];
            var blog = (from b in entity.Blog where b.Kategori_id == secilenkategori.Kategori_id select b).ToList();
            ViewBag.blog = blog;
            ViewBag.kategori = secilenkategori;
            ViewBag.blogSayisi = blog.Count();
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

        public ActionResult KategoriEkle()
        {
            return View();
        }

        [HttpPost]
        [ActFilter("Yeni Kategori Eklendi")]
        public ActionResult KategoriEkle(string KategoriAdi)
        {
            Kategori yenikategori = new Kategori();
            string yeniAd = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(KategoriAdi);
            yenikategori.KategoriAdi = yeniAd;
            yenikategori.Aktiflik = true;

            entity.Kategori.Add(yenikategori);
            entity.SaveChanges();
            TempData["bilgi"] = yenikategori.KategoriAdi;
            return RedirectToAction("Kategoriler");
        }

        public ActionResult Kategoriler()
        {
            var kategoriler = (from kat in entity.Kategori select kat).ToList();
            ViewBag.kategoriler = kategoriler;

            return View(kategoriler);

        }

        [HttpGet]
        public ActionResult Guncelle(int id)
        {
            var kategoriler = (from kat in entity.Kategori where kat.Kategori_id == id select kat).FirstOrDefault();
            return View(kategoriler);
        }

        [HttpPost]
        [ActFilter("Kategori Güncellendi")]
        public ActionResult Guncelle(FormCollection fc)
        {
            int kategoriId = Convert.ToInt32(fc["Kategori_id"]);
            string yeniad = fc["KategoriAdi"];
            var kategori = (from kat in entity.Kategori where kat.Kategori_id == kategoriId select kat).FirstOrDefault();

            kategori.KategoriAdi = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(yeniad);

            entity.SaveChanges();

            TempData["bilgi"] = kategori.KategoriAdi;

            return RedirectToAction("Kategoriler");
        }

        [ActFilter("Kategori Silindi")]
        public ActionResult Sil(int id)
        {
            var kategoriler = (from kat in entity.Kategori
                               where kat.Kategori_id == id
                               select kat).FirstOrDefault();

            if (kategoriler != null)
            {
                var blogs = from blog in entity.Blog
                            where blog.Kategori_id == id
                            select blog;
                if (blogs.Any())
                {
                    ViewBag.Kategori = kategoriler;
                    ViewBag.Bloglar = blogs.ToList();
                    return View("OnayliSil");
                }

                entity.Kategori.Remove(kategoriler);
                entity.SaveChanges();
                TempData["bilgi"] = kategoriler.KategoriAdi;
                return RedirectToAction("Kategoriler");

            }
            return RedirectToAction("Kategoriler");
        }
 
        public ActionResult BlogSilGuncelle()
        {
            var blog = (from b in entity.Blog select b).ToList();
            return View(blog);
        }
        [HttpPost]
        public ActionResult OnayliSil(int secilenKategoriId)
        {
            var kategori = (from kat in entity.Kategori
                            where kat.Kategori_id == secilenKategoriId
                            select kat).FirstOrDefault();

            if (kategori != null)
            {
                var blogs = from blog in entity.Blog
                            where blog.Kategori_id == secilenKategoriId
                            select blog;

                foreach (var blog in blogs)
                {
                    entity.Blog.Remove(blog);
                }

                entity.Kategori.Remove(kategori);
                entity.SaveChanges();
            }
            return RedirectToAction("Kategoriler");
        }
        public ActionResult Log()
        {
            var log=(from l in entity.Logs orderby l.tarih descending select l).ToList();
            return View(log);
        }
        public ActionResult BlogGuncelle(int id)
        {
            var blog = (from b in entity.Blog where b.Blog_id == id select b).FirstOrDefault();
            return View(blog);
        }
        [HttpPost,ActFilter("Blog Güncellendi")]
        public ActionResult BlogGuncelle(int id,string baslik,string altbaslik,string aciklama)
        {
            Blog blog = (from b in entity.Blog where b.Blog_id==id select b).FirstOrDefault();

            blog.Baslik = baslik;
            blog.AltBaslik = altbaslik;
            blog.Aciklama = aciklama;
            entity.SaveChanges();

            TempData["bilgi"] = blog.Baslik;
            TempData["altbaslik"]=blog.AltBaslik;
            TempData["aciklama"]= blog.Aciklama;   
            return RedirectToAction("BlogSilGuncelle");
        }
        public ActionResult BlogSil(int id)
        {
            Blog blog = entity.Blog.FirstOrDefault(b => b.Blog_id == id);
            return View(blog);
        }

        [HttpPost, ActFilter("Blog Silindi")]
        public ActionResult BlogSil(Blog model)
        {
            var blog = entity.Blog.FirstOrDefault(b => b.Blog_id == model.Blog_id);

            if (blog != null)
            {
                entity.Blog.Remove(blog);
                entity.SaveChanges();
                TempData["bilgi"] = blog.Baslik;
                return View("BlogSilGuncelle");
            }

            return RedirectToAction("BlogSilGuncelle");
        }

    }
}
