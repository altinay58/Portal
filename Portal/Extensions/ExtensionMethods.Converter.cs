using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
    public static partial class ExtensionMethods
    {
        /// <summary>
        /// Tüm metni küçültür(Küçük harfe dönüştürür).
        /// </summary>
        /// <param name="s">Dönüştürülecek</param>
        /// <returns></returns>
        public static string ConvertToLowerString(this string s)
        {
            return !s.IsNullOrEmpty() ? s.ToLower() : "";
        }

        /// <summary>
        /// Gönderilen string listeyi virgül ile birleştirerek tek metin yapar.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Virgül(,) ile birleştirilmiş string metin döndürür.</returns>
        public static string ConvertComma(this List<string> s)
        {
            string sonuc = "";
            if (!s.IsNullOrEmpty())
                s.ForEach(f => { sonuc += $"{f},"; });

            return sonuc;
        }

        /// <summary>
        /// Gönderilen string listeyi virgül ile birleştirerek tek metin yapar.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Virgül(,) ile birleştirilmiş string metin döndürür.</returns>
        public static string ConvertComma(this string[] s)
        {
            string sonuc = "";
            if (!s.IsNullOrEmpty())
                s.ToList().ForEach(f => { sonuc += $"{f},"; });

            return sonuc;
        }

    }
}