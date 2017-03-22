using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public static class CacheKeys
    {
        public const string ETIKETS = "ETIKETS";
    }
    public static class CacheManagement
    {
        public static void SetCache<T>(string key, T data) 
        {
            HttpContext.Current.Cache.Insert(key,
              data, null,
              DateTime.Now.AddMonths(1),
              System.Web.Caching.Cache.NoSlidingExpiration,
              System.Web.Caching.CacheItemPriority.Normal,
              null);
        }
        public static void SetCache<T>(string key, List<T> data)
        {
            HttpContext.Current.Cache.Insert(key,
              data, null,
              DateTime.Now.AddMonths(1),
              System.Web.Caching.Cache.NoSlidingExpiration,
              System.Web.Caching.CacheItemPriority.Normal,
              null);
        }
        public static List<T> Get<T>(string key) where T : class
        {
            var cacheData= (List<T>)HttpContext.Current.Cache[key];
            if (cacheData == null)
            {
                var dbData= Database.DbHelper.GetDb.Set<T>().AsNoTracking().ToList();
                SetCache(key, dbData);
                return dbData;
            }
            else
            {
                return cacheData;
            }
           
        }
   
        //public static List<Domain> TumDomainler()
        //{
        //    if (HttpContext.Current.Cache["TumDomainler"] == null)
        //    {
        //        var tumDomainler = Database.DbHelper.GetDb.Domains.ToList();

        //        HttpContext.Current.Cache.Insert("TumDomainler",
        //          tumDomainler, null,
        //          DateTime.Now.AddMonths(1),
        //          System.Web.Caching.Cache.NoSlidingExpiration,
        //          System.Web.Caching.CacheItemPriority.Normal,
        //          null);
        //        return tumDomainler;
        //    }else
        //    {
        //        return (List<Domain>)HttpContext.Current.Cache["TumDomainler"];
        //    }
           
        //}
    }
}