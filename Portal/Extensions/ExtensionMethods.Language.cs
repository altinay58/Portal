using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Portal.Models
{
    public static partial class ExtensionMethods
    {
        public static string Turkcelestir(this string s)
        {
            if (!s.IsNullOrEmpty())
                return Encoding.Default.GetString(Encoding.GetEncoding(1252).GetBytes(s));

            return s.ConvertToString();
        }
    }
}
