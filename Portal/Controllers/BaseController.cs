using Microsoft.AspNet.Identity;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Security;

namespace Portal.Controllers
{
    public abstract class BaseController : Controller
    {
        public BaseController()
        {
            
        }
        protected readonly int  PagerCount = 20;
        //basarili
        protected readonly string SUCESS = "Success";
        protected readonly string ERROR = "Error";
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            Database.Db = new Models.PortalEntities();
            //TODO: sonra iptal edilecek,login ekranindan giris olmadigi icin           
            FormsAuthentication.SetAuthCookie("fatihgokce07@gmail.com",true);
            //daha sonra aktif edilecek
            //WebLoguEkle(Request.RawUrl);
        }
        protected Models.PortalEntities Db { get { return Database.Db; } private set { } }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            //Database.Db.Dispose();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(Database.Db!=null)
                {
                   Database.Db.Dispose();
                }
               
            }
            base.Dispose(disposing);
        }
        public static void WebLoguEkle(string sayfaAdresi)
        {
            if (System.Web.HttpContext.Current.User.Identity.GetUserId() != null)
            {
               
                    WebLog log = new WebLog()
                    {
                        WebLogIP = System.Web.HttpContext.Current.Request.UserHostAddress,
                        WebLogWebSayfasi = sayfaAdresi,
                        WebLogTarih = DateTime.Now,
                        WebLogUserID = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    };
                    Database.Db.WebLogs.Add(log);
                    Database.Db.SaveChanges();
                
            }

        }
    }
}