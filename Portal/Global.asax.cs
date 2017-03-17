using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Portal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        static void ScheduleTaskTrigger()
        {
            HttpRuntime.Cache.Add("ScheduledTaskTrigger",
                                  string.Empty,
                                  null,
                                  Cache.NoAbsoluteExpiration,
                                  TimeSpan.FromHours(10), // Every 1 hour
                                  CacheItemPriority.NotRemovable,
                                  new CacheItemRemovedCallback(PerformScheduledTasks));
        }
        static void PerformScheduledTasks(string key, Object value, CacheItemRemovedReason reason)
        {
            if (Fonksiyonlar.DomainMailGonderilsinMi())
            {
                Fonksiyonlar.SuresiDolanDomainleriMailGonder();
            }
            ScheduleTaskTrigger();
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ScheduleTaskTrigger();
        }
    }
}
