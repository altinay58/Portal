using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
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
        public static IQueryable<T> TumFirmalar<T>(this IQueryable<T> source) where T : Firma
        {
            return source.Where(a => a.FirmaSilindi == false).OrderByDescending(x=>x.FirmaID);
        }
        public static bool ContainsNullControl(this string source,string value)
        {
            return (!string.IsNullOrEmpty(value) ? SqlFunctions.PatIndex("%"+value+"%",source)>0 : true);
        }

        /// <summary>
        /// Hata varsa hatayı html formatında string tipinde döner.
        /// <para>Fluent validation kontrollerinde ki hatayı getirir.</para>
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string GetHtmlErrors(this ValidationResult result)
        {
            if (!result.IsValid)
            {
                string sonuc = "";
                foreach (var err in result.Errors)
                    sonuc += $"{err.ErrorMessage}<br/>";

                return sonuc;
            }

            return "";
        }
    }
}