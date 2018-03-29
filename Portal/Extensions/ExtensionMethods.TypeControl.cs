using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Portal.Models
{
    public static partial class ExtensionMethods
    {
        public static bool IsBool(this string value)
        {
            try
            {
                Convert.ToBoolean(value.ConvertToLowerString());
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsByte(this string[] value)
        {
            if (value != null)
            {
                if (value.Length != 0)
                {
                    foreach (string s in value)
                    {
                        if (s != null)
                        {
                            try
                            {
                                Convert.ToByte(s);
                            }
                            catch
                            {
                                return false;
                            }
                        }
                        else
                            return false;
                    }
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        public static bool IsByte(this string value)
        {
            try
            {
                Convert.ToByte(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsByteZeroBigValue(this string value)
        {
            try
            {
                short val = Convert.ToByte(value);
                if (val <= 0)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsSByte(this string value)
        {
            try
            {
                Convert.ToSByte(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsShort(this string[] value)
        {
            if (value != null)
            {
                if (value.Length != 0)
                {
                    foreach (string s in value)
                    {
                        if (s != null)
                        {
                            try
                            {
                                Convert.ToInt16(s);
                            }
                            catch
                            {
                                return false;
                            }
                        }
                        else
                            return false;
                    }
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        public static bool IsShort(this string value)
        {
            try
            {
                Convert.ToInt16(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsShortZeroBigValue(this string value)
        {
            try
            {
                short val = Convert.ToInt16(value);
                if (val <= 0)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsUShort(this string value)
        {
            try
            {
                Convert.ToUInt16(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsInt(this string value)
        {
            try
            {
                Convert.ToInt32(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsIntZeroBigValue(this string value)
        {
            try
            {
                int val = Convert.ToInt32(value);
                if (val <= 0)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsIntZeroBigValue(this object value)
        {
            try
            {
                int val = Convert.ToInt32(value);
                if (val <= 0)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsIntZeroEqualOrBigValue(this object value)
        {
            try
            {
                int val = Convert.ToInt32(value);
                if (val < 0)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsInt(this string[] value)
        {
            if (!value.IsNullOrEmpty())
            {
                foreach (string s in value)
                    if (!s.IsInt())
                        return false;

                return true;
            }

            return false;
        }

        public static bool IsUInt(this string value)
        {
            try
            {
                Convert.ToUInt32(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public static bool IsLong(this string value)
        {
            try
            {
                Convert.ToInt64(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsULong(this string value)
        {
            try
            {
                Convert.ToUInt64(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsLong(this string[] value)
        {
            if (!value.IsNullOrEmpty())
            {
                foreach (string s in value)
                    if (!s.IsLong())
                        return false;

                return true;
            }

            return false;
        }

        public static bool IsDecimal(this string value)
        {
            try
            {
                Convert.ToDecimal(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsDecimalZeroBigValue(this string value)
        {
            try
            {
                decimal val = Convert.ToDecimal(value);
                if (val <= 0)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsDecimalZeroBigValue(this object value)
        {
            try
            {
                decimal val = Convert.ToDecimal(value);
                if (val <= 0)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsDecimal(this string[] value)
        {
            if (!value.IsNullOrEmpty())
            {
                foreach (string s in value)
                    if (!s.IsDecimal())
                        return false;

                return true;
            }

            return false;
        }
        
        public static bool IsDouble(this string value)
        {
            try
            {
                Convert.ToDouble(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsDoubleZeroBigValue(this string value)
        {
            try
            {
                double val = Convert.ToDouble(value);
                if (val <= 0)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsDateTime(this string value)
        {
            try
            {
                Convert.ToDateTime(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}