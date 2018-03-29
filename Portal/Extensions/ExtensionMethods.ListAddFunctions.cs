using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Portal.Models
{
    public static partial class ExtensionMethods
    {
        public static void Ekle(this List<string> l)
        {
            if (l != null)
                l.Add("");
        }
        public static void Ekle(this List<string> l, string s)
        {
            if (l != null)
                l.Add(s.ConvertToString());
        }
        public static void Ekle(this List<SqlParameter> l, string parametre, object deger)
        {
            if (l != null)
                l.Add(new SqlParameter(parametre, deger));
        }
        public static void Ekle(this List<string> l, List<string> s)
        {
            if (l != null)
                l.AddRange(s);
        }
        /// <summary>
        /// Gönderilen hatayı tipine göre(Type) hata mesajı döner.
        /// </summary>
        /// <param name="l"></param>
        /// <param name="ex"></param>
        public static void Ekle(this List<string> l, Exception ex)
        {
            l.Add("");
            l.Add("Beklenmeyen bir hata oluştu.");
            l.Add($"Hata Detayı: {ex.Message}");
            l.Add($"Kaynak: {ex.Source}");
            l.Add($"Yığın İzleme: {ex.StackTrace}");
        }


        /// <summary>
        /// Eğer "s" parametresinde değer varsa "l" nesnesine değer ekler.
        /// <para>Bu validasyon kontrolleri için kullanılmalıdır.</para>
        /// <para>"s" parametresinin null ve boş olmaması durumunda hata var kabul edilir ve "l" nesnesine "s" parametresi eklenir.</para>
        /// </summary>
        /// <param name="l">Mesaj listesi</param>
        /// <param name="s">eklenecek hata değeri</param>
        public static void KontrolEt(this List<string> l, string s)
        {
            if (s != null)
                if (!s.IsNullOrEmpty())
                    l.Add(s);
        }

        public static string SubStr(this string s, int length)
        {
            s = s.ConvertToString();
            if (s.Length > length)
                return s.Substring(0, length);

            return s;
        }
        public static string SubStr(this string s, int startIndex, int length)
        {
            s = s.ConvertToString();
            if (s.Length > (startIndex + length))
                return s.Substring(0, length);

            return s;
        }
    }
}