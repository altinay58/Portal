using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Portal.Models
{
    public static partial class ExtensionMethods
    {
        public static string _0n0(this decimal s1)
        {
            return String.Format("{0:n0}", s1);
        }
        public static string _0n1(this decimal s1)
        {
            return String.Format("{0:n1}", s1);
        }
        public static string _0n(this decimal s1)
        {
            return String.Format("{0:n}", s1);
        }
        public static string _0n3(this decimal s1)
        {
            return String.Format("{0:n3}", s1);
        }
        public static string _0n4(this decimal s1)
        {
            return String.Format("{0:n4}", s1);
        }
        public static string _0n5(this decimal s1)
        {
            return String.Format("{0:n5}", s1);
        }
        public static string _0n6(this decimal s1)
        {
            return String.Format("{0:n6}", s1);
        }
        public static string _0n7(this decimal s1)
        {
            return String.Format("{0:n7}", s1);
        }
        public static string _0n8(this decimal s1)
        {
            return String.Format("{0:n8}", s1);
        }

        public static string _0n(this decimal? s1)
        {
            if (s1 != null)
                return String.Format("{0:n}", s1);
            else
                return "";
        }
        public static string _0n1(this decimal? s1)
        {
            if (s1 != null)
                return String.Format("{0:n1}", s1);
            else
                return "";
        }
    }
}