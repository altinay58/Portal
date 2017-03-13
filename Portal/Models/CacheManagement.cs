using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public static class CacheManagement
    {
        public static List<Domain> TumDomainler()
        {
            if (HttpContext.Current.Cache["TumDomainler"] == null)
            {
                var tumDomainler = Database.DbHelper.GetDb.Domains.ToList();

                HttpContext.Current.Cache.Insert("TumDomainler",
                  tumDomainler, null,
                  DateTime.Now.AddMonths(1),
                  System.Web.Caching.Cache.NoSlidingExpiration,
                  System.Web.Caching.CacheItemPriority.Normal,
                  null);
                return tumDomainler;
            }else
            {
                return (List<Domain>)HttpContext.Current.Cache["TumDomainler"];
            }
           
        }
    }
}