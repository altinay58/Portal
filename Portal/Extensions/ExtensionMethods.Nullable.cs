using System.Collections.Generic;
using System.Linq;

namespace Portal.Models
{
    public static partial class ExtensionMethods
    {
        public static bool IsNull(this object s)
        {
            if (s != null)
                return false;

            return true;
        }
        public static bool IsNullOrEmpty(this string s)
        {
            if (s != null && s != "")
                return false;
            else
                return true;
        }

        public static bool IsNullOrEmpty(this byte[] s)
        {
            return s != null && s.Length > 0 ? false : true;
        }
        public static bool IsNullOrEmpty(this short[] s)
        {
            return s != null && s.Length > 0 ? false : true;
        }
        public static bool IsNullOrEmpty(this int[] s)
        {
            return s != null && s.Length > 0 ? false : true;
        }
        public static bool IsNullOrEmpty(this long[] s)
        {
            return s != null && s.Length > 0 ? false : true;
        }
        public static bool IsNullOrEmpty(this decimal[] s)
        {
            return s != null && s.Length > 0 ? false : true;
        }
        public static bool IsNullOrEmpty(this float[] s)
        {
            return s != null && s.Length > 0 ? false : true;
        }

        public static bool IsNullOrEmpty(this List<byte> s)
        {
            return s != null && s.Count() > 0 ? false : true;
        }
        public static bool IsNullOrEmpty(this List<short> s)
        {
            return s != null && s.Count() > 0 ? false : true;
        }
        public static bool IsNullOrEmpty(this List<int> s)
        {
            return s != null && s.Count() > 0 ? false : true;
        }
        public static bool IsNullOrEmpty(this List<long> s)
        {
            return s != null && s.Count() > 0 ? false : true;
        }
        public static bool IsNullOrEmpty(this List<decimal> s)
        {
            return s != null && s.Count() > 0 ? false : true;
        }
        public static bool IsNullOrEmpty(this List<float> s)
        {
            return s != null && s.Count() > 0 ? false : true;
        }
        
        public static bool IsNullOrEmpty(this List<char> s)
        {
            return s != null && s.Count > 0 ? false : true;
        }
        public static bool IsNullOrEmpty(this List<string> s)
        {
            return s != null && s.Count > 0 ? false : true;
        }
        public static bool IsNullOrEmpty(this string[] s)
        {
            return s != null && s.Count() > 0 ? false : true;
        }
        public static bool IsNullOrEmpty(this List<object> s)
        {
            return s != null && s.Count() > 0 ? false : true;
        }
        public static bool IsNullOrEmpty(this IEnumerable<object> s)
        {
            return s != null && s.Count() > 0 ? false : true;
        }
        public static bool IsNullOrEmpty(this System.Data.DataTable dt)
        {
            return dt != null && dt.Rows.Count > 0 ? false : true;
        }
    }
}