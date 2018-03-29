using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Portal.Models
{
    public static partial class ExtensionMethods
    {
        /// <summary>
        /// Çıktı: 31.12.2017
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ddMMyyyy(this DateTime? t)
        {
            try
            {
                if (t != null)
                    return t.Value.ToString("dd.MM.yyyy");
            }
            catch { }

            return "";
        }
        /// <summary>
        /// Çıktı: 31.12.2017
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ddMMyyyy(this DateTime t)
        {
            try
            {
                return t.ToString("dd.MM.yyyy");
            }
            catch { }

            return "";
        }
        /// <summary>
        /// Çıktı: 31.12.2017 Pazar
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ddMMyyyydddd(this DateTime t)
        {
            try
            {
                return t.ToString("dd.MM.yyyy dddd");
            }
            catch { }

            return "";
        }
        /// <summary>
        /// Çıktı: 31.12.2017 Pazar
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ddMMyyyydddd(this DateTime? t)
        {
            try
            {
                if (t != null)
                    return t.Value.ToString("dd.MM.yyyy dddd");
            }
            catch { }

            return "";
        }

        /// <summary>
        /// Çıktı: 31.12.2017 23:59
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ddMMyyyyHHmm(this DateTime? t)
        {
            try
            {
                if (t != null)
                    return t.Value.ToString("dd.MM.yyyy HH:mm");
            }
            catch { }

            return "";
        }
        /// <summary>
        /// Çıktı: 31.12.2017 23:59
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ddMMyyyyHHmm(this DateTime t)
        {
            try
            {
                if (t != null)
                    return t.ToString("dd.MM.yyyy HH:mm");
            }
            catch { }

            return "";
        }
        /// <summary>
        /// Çıktı: 31.12.2017 23:59:59
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ddMMyyyyHHmmss(this DateTime t)
        {
            try
            {
                if (t != null)
                    return t.ToString("dd.MM.yyyy HH:mm:ss");
            }
            catch { }

            return "";
        }
        /// <summary>
        /// Çıktı: 31.12.2017 23:59
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ddMMyyyyHHmmss(this DateTime? t)
        {
            try
            {
                if (t != null)
                    return t.Value.ToString("dd.MM.yyyy HH:mm:ss");
            }
            catch { }

            return "";
        }

        /// <summary>
        /// Çıktı: 2017-12-32T23:59:59
        /// <para>JSON da kullanılır.</para>
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string yyyy_MM_ddTHHmmss(this DateTime? t)
        {
            try
            {
                if (t != null)
                    return t.Value.ToString("yyyy-MM-ddTHH:mm:ss");
            }
            catch { }

            return "";
        }

        /// <summary>
        /// Çıktı: 2017-12-32T23:59:59
        /// <para>JSON da kullanılır.</para>
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string yyyy_MM_ddTHHmmss(this DateTime t)
        {
            try
            {
                if (t != null)
                    return t.ToString("yyyy-MM-ddTHH:mm:ss");
            }
            catch { }

            return "";
        }

        /// <summary>
        /// Çıktı: 20171231
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string yyyyMMdd(this DateTime? t)
        {
            try
            {
                if (t != null)
                    return t.Value.ToString("yyyyMMdd");
            }
            catch { }

            return "";
        }

        /// <summary>
        /// Çıktı: 2017-12-31
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string yyyy_MM_dd(this DateTime? t)
        {
            try
            {
                if (t != null)
                    return t.Value.ToString("yyyy-MM-dd");
            }
            catch { }

            return "";
        }
        /// <summary>
        /// Çıktı: 2017-12-31
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string yyyy_MM_dd(this DateTime t)
        {
            try
            {
                return t.ToString("yyyy-MM-dd");
            }
            catch { }

            return "";
        }

        /// <summary>
        /// Çıktı: 20171231
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string yyyyMMdd(this DateTime t)
        {
            try
            {
                return t.ToString("yyyyMMdd");
            }
            catch { }

            return "";
        }
        /// <summary>
        /// Çıktı: 20181231235959
        /// <para>yıl_ay_gün_saat_dakika_saniye</para>
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string yyyyMMddHHmmss(this DateTime t)
        {
            try
            {
                return t.ToString("yyyyMMddHHmmss");
            }
            catch { }

            return "";
        }
        /// <summary>
        /// Çıktı: 201812312359599
        /// <para>yıl_ay_gün_saat_dakika_saniye_salise</para>
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string yyyyMMddHHmmssf(this DateTime t)
        {
            try
            {
                return t.ToString("yyyyMMddHHmmssf");
            }
            catch { }

            return "";
        }
        /// <summary>
        /// Çıktı: 2018123123595999
        /// <para>yıl_ay_gün_saat_dakika_saniye_salise</para>
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string yyyyMMddHHmmssff(this DateTime t)
        {
            try
            {
                return t.ToString("yyyyMMddHHmmssff");
            }
            catch { }

            return "";
        }
        /// <summary>
        /// Çıktı: 20181231235959999
        /// <para>yıl_ay_gün_saat_dakika_saniye_salise</para>
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string yyyyMMddHHmmssfff(this DateTime t)
        {
            try
            {
                return t.ToString("yyyyMMddHHmmssfff");
            }
            catch { }

            return "";
        }
        /// <summary>
        /// Çıktı: 201812312359599999
        /// <para>yıl_ay_gün_saat_dakika_saniye_salise</para>
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string yyyyMMddHHmmssffff(this DateTime t)
        {
            try
            {
                return t.ToString("yyyyMMddHHmmssffff");
            }
            catch { }

            return "";
        }
        /// <summary>
        /// Çıktı: 2018123123595999999
        /// <para>yıl_ay_gün_saat_dakika_saniye_salise</para>
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string yyyyMMddHHmmssfffff(this DateTime t)
        {
            try
            {
                return t.ToString("yyyyMMddHHmmssfffff");
            }
            catch { }

            return "";
        }
    }
}