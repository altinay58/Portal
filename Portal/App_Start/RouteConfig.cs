using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Portal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                "MailOkundu",
                "mail/oku/{id}",
                new { Controller = "MailSablonlari", action = "MailOkundu", id = (int?)null }
                );

            routes.MapRoute(
                "KullaniciParolaSifirlama",
                "kullanici/parola/sifirla/{sifirlamaSifresi}/{userID}",
                new { Controller = "Account", action = "KullaniciParolaSifirla", dil = (string)null, sifirlamaSifresi = (string)null, userID = (string)null }
                );



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
          
        }
    }
}
