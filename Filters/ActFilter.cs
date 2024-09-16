using Blogsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace Blogsite.Filters
{
    public class ActFilter : FilterAttribute, IActionFilter
    {
        BlogsiteEntities entity = new BlogsiteEntities();

        protected string aciklama;
        public ActFilter(string actaciklama) {
            this.aciklama = actaciklama;
        }
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Logs log = new Logs();
            log.logAciklama = this.aciklama + " (" + filterContext.Controller.TempData["bilgi"] + ")";
            log.actionAd=filterContext.ActionDescriptor.ActionName;
            log.controllerAd=filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            log.tarih= DateTime.Now;
            log.kullaniciId = Convert.ToInt32(filterContext.HttpContext.Session["Kullanici_id"]);

            entity.Logs.Add(log);
            entity.SaveChanges();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
        }
    }
}