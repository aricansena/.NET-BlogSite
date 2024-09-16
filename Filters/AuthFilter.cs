using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Blogsite.Filters
{
    public class AuthFilter : FilterAttribute, IAuthorizationFilter
    {
        protected int yetkiTur;
        public AuthFilter(int yethiTur) {
            this.yetkiTur = yethiTur;
        }
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            int yetkkiTurId = Convert.ToInt32( filterContext.HttpContext.Session["KullaniciYetkiTur_id"]);
            if (this.yetkiTur !=yetkkiTurId)
            {
                filterContext.Result = new RedirectResult("/Login/Index");
            }


        }
    }
}