using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blogsite.Controllers
{
    public class SignupController : Controller
    {
        // GET: Signup
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string ad,string soyad,string parola,string telefon, string parolatekrar, string eposta,string kullaniciadi) {
            if (parola == parolatekrar)
            {
                return RedirectToAction("Index", "Misafir");
            }
            else {
                return RedirectToAction("Index","Signup");
            }
           
        }


    }
}