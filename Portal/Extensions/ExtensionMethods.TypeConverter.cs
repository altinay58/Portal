using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Portal.Models
{
    public static partial class ExtensionMethods
    {
        public static string ConvertToString(this string s)
        {
            return s != null && s != "" ? s : "";
        }
        public static string ConvertToString(this bool s)
        {
            return s.ToString().ToLower();
        }
        
        public static bool ConvertToBoolean(this string s)
        {
            try
            {
                return Convert.ToBoolean(s);
            }
            catch
            {
                return false;
            }
        }
        public static bool ConvertToBoolean(this string s, bool errorValue)
        {
            try
            {
                return Convert.ToBoolean(s);
            }
            catch
            {
                return errorValue;
            }
        }
        public static bool ConvertToBoolean(this object s)
        {
            try
            {
                return Convert.ToBoolean(s);
            }
            catch
            {
                return false;
            }
        }
        public static bool ConvertToBoolean(this object s, bool errorValue)
        {
            try
            {
                return Convert.ToBoolean(s);
            }
            catch
            {
                return errorValue;
            }
        }
        
        public static byte ConvertToByte(this string s)
        {
            try
            {
                return Convert.ToByte(s);
            }
            catch
            {
                return 0;
            }
        }

        public static short ConvertToInt16(this string s)
        {
            try
            {
                return Convert.ToInt16(s);
            }
            catch
            {
                return -1;
            }
        }
        public static short ConvertToInt16(this string s, short errorValue)
        {
            try
            {
                return Convert.ToInt16(s);
            }
            catch
            {
                return errorValue;
            }
        }
        public static short ConvertToInt16(this short? s)
        {
            if (s != null)
                return s.Value;
            else
                return -1;
        }

        public static int ConvertToInt32(this int? s)
        {
            if (s != null)
                return s.Value;
            else
                return -1;
        }
        public static int ConvertToInt32(this int? s, int errorValue)
        {
            if (s != null)
                return s.Value;
            else
                return errorValue;
        }
        public static int ConvertToInt32(this string s)
        {
            try
            {
                return Convert.ToInt32(s);
            }
            catch
            {
                return -1;
            }
        }
        public static int ConvertToInt32(this string s, int errorValue)
        {
            try
            {
                return Convert.ToInt32(s);
            }
            catch
            {
                return errorValue;
            }
        }

        public static List<int> ConvertToInt32(this string[] s)
        {
            try
            {
                if (!s.IsNullOrEmpty())
                    return s.Select(s1 => s1.ConvertToInt32()).ToList();

                return null;
            }
            catch
            {
                return null;
            }
        }
        public static List<int> ConvertToInt32(this List<string> s)
        {
            try
            {
                if (!s.IsNullOrEmpty())
                    return s.Select(s1 => s1.ConvertToInt32()).ToList();

                return null;
            }
            catch
            {
                return null;
            }
        }
        public static List<int> ConvertToInt32(this List<byte> s)
        {
            try
            {
                if (!s.IsNullOrEmpty())
                    return s.Select(s1 => (int)s1).ToList();

                return null;
            }
            catch
            {
                return null;
            }
        }
        public static List<int> ConvertToInt32(this List<short> s)
        {
            try
            {
                if (!s.IsNullOrEmpty())
                    return s.Select(s1 => (int)s1).ToList();

                return null;
            }
            catch
            {
                return null;
            }
        }

        public static decimal ConvertToDecimal(this string s)
        {
            try
            {
                return Convert.ToDecimal(s);
            }
            catch
            {
                return -1;
            }
        }
        public static decimal ConvertToDecimal(this string s, decimal errorValue)
        {
            try
            {
                return Convert.ToDecimal(s);
            }
            catch
            {
                return errorValue;
            }
        }

        public static decimal ConvertToDecimal(this decimal? s)
        {
            if (s != null)
                return s.Value;
            else
                return -1;
        }
        public static decimal ConvertToDecimal(this decimal? s, decimal errorValue)
        {
            if (s != null)
                return s.Value;
            else
                return errorValue;
        }

        public static long ConvertToInt64(this string s)
        {
            try
            {
                return Convert.ToInt64(s);
            }
            catch
            {
                return -1;
            }
        }
        public static long ConvertToInt64(this string s, long errorValue)
        {
            try
            {
                return Convert.ToInt64(s);
            }
            catch
            {
                return errorValue;
            }
        }
        public static long ConvertToInt64(this long? s)
        {
            if (s != null)
                return s.Value;
            else
                return -1;
        }

        public static DateTime ConvertToDateTime(this string s)
        {
            try
            {
                return Convert.ToDateTime(s);
            }
            catch
            {
                return DateTime.Now;
            }
        }
        public static DateTime ConvertToDateTime(this string s, DateTime errorValue)
        {
            try
            {
                return Convert.ToDateTime(s);
            }
            catch
            {
                return errorValue;
            }
        }
        public static DateTime ConvertToDateTime(this DateTime? s)
        {
            return s != null ? s.Value : DateTime.Now;
        }
        public static DateTime ConvertToDateTime(this DateTime? s, DateTime errorValue)
        {
            return s != null ? s.Value : errorValue;
        }
    }
}