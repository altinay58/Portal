using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public static class Extension
    {
        public static IEnumerable<T> TumKullanicilar<T>(this IQueryable<T> source) where T : AspNetUser
        {
            return source.Where(a => a.LockoutEnabled == false);
        }
        public static IEnumerable<T> TumFirmalar<T>(this IQueryable<T> source) where T : Firma
        {
            return source.Where(a => a.FirmaSilindi == false);
        }
    }
}