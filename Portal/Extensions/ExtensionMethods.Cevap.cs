using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Models
{
    public static partial class ExtensionMethods
    {
        /// <summary>
        /// "System.ComponentModel.DataAnnotations" sınıfı için Model(ModelState) kontrolünü yapar.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="ModelState"></param>
        public static void KontrolEt(this Cevap c, ModelStateDictionary ModelState)
        {
            if (!ModelState.IsValid)
                c.Mesaj.Ekle(ModelState);
        }

        /// <summary>
        /// "System.ComponentModel.DataAnnotations" sınıfı için kullanılan "ModelState" nesnesinin hatalarını yakalar ve "l" parametresine ekler.
        /// </summary>
        /// <param name="l"></param>
        /// <param name="ms"></param>
        public static void Ekle(this List<string> l, ModelStateDictionary ms)
        {
            if (ms != null)
                foreach (var modelState in ms.Values)
                    foreach (var modelError in modelState.Errors)
                        l.Ekle(modelError.ErrorMessage);
        }
    }
}