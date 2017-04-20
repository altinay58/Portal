using Microsoft.AspNet.Identity;
using Portal.Models;
using System;
using System.Web.Mvc;
using System.Threading;
using System.Globalization;
using Portal.Filters;
using Portal.Helper;

namespace Portal.Controllers
{   
    [Authorize]
    public abstract class BaseController : Controller
    {        
        public BaseController()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("tr-TR");
        }      
        protected readonly int  PagerCount = 20;
        //basarili
        protected readonly string SUCESS = "Success";
        protected readonly string ERROR = "Error";
        protected dynamic GuncelMenu {
            get { return ViewBag.guncelMenu; }
            set { ViewBag.guncelMenu = value;   }
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            Db = Database.DbHelper.GetDb ;
          
          
            //TODO: sonra iptal edilecek,login ekranindan giris olmadigi icin           
            //FormsAuthentication.SetAuthCookie("fatihgokce07@gmail.com",true);
            //daha sonra aktif edilecek
            //WebLoguEkle(Request.RawUrl);
        }
        protected Models.PortalEntities Db { get; private set; }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            //Database.Db.Dispose();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(Db!=null)
                {
                   Db.Dispose();
                }
               
            }
            base.Dispose(disposing);
        }
        public static void WebLoguEkle(string sayfaAdresi)
        {
            if (System.Web.HttpContext.Current.User.Identity.GetUserId() != null)
            {
                using (PortalEntities Db=new PortalEntities())
                {
                    WebLog log = new WebLog()
                    {
                        WebLogIP = System.Web.HttpContext.Current.Request.UserHostAddress,
                        WebLogWebSayfasi = sayfaAdresi,
                        WebLogTarih = DateTime.Now,
                        WebLogUserID = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    };
                    Db.WebLogs.Add(log);
                    Db.SaveChanges();
                }
                    
                
            }

        }
    }
}