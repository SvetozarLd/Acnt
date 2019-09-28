using System;

namespace AccentBase.Utils
{
    internal static class CheckDBNull
    {
        public static int ToInt32(object ob)
        {
            try
            {
                if (ob != System.DBNull.Value)
                {
                    return Convert.ToInt32(ob);

                }
                else
                {
                    return 0;
                }
            }
            catch { return 0; }
        }
        public static int ToInt(object ob)
        {
            try
            {
                if (ob != System.DBNull.Value)
                {
                    if (int.TryParse(ob.ToString(), out int result))
                    {
                        return result;
                    }
                    else { return 0; }
                }
                else
                {
                    return 0;
                }
            }
            catch { return 0; }
        }
        public static int ToIntFromBool(object ob)
        {
            try
            {
                if (ob != System.DBNull.Value)
                {
                    bool result;
                    result = Convert.ToBoolean(ob);
                    if (result) { return 1; } else { return 0; }
                }
                else
                {
                    return 0;
                }
            }
            catch { return 0; }
        }
        public static string ToString(object ob)
        {
            try
            {
                if (ob != System.DBNull.Value)
                {
                    return Convert.ToString(ob);
                }
                else
                {
                    return string.Empty;
                }
            }
            catch { return string.Empty; }
        }
        //static public byte[] ToByte(object ob)
        //{
        //    try
        //    {
        //        if (ob != System.DBNull.Value)
        //        {
        //            return Convert.ToString(ob);
        //        }
        //        else
        //        {
        //            return string.Empty;
        //        }
        //    }
        //    catch { return new byte[]; }
        //}
        public static double ToDouble(object ob)
        {
            try
            {
                if (ob != System.DBNull.Value)
                {
                    return Convert.ToDouble(ob);
                }
                else
                {
                    return 0;
                }
            }
            catch { return 0; }
        }
        public static bool ToBoolean(object ob)
        {
            try
            {
                if (ob != System.DBNull.Value)
                {
                    return Convert.ToBoolean(ob);
                }
                else { return false; }
            }
            catch { return false; }
        }
        public static long ToLong(object ob)
        {
            try
            {
                if (ob != System.DBNull.Value)
                {
                    return Convert.ToInt64(ob);
                }
                else
                {
                    return 0;
                }
            }
            catch { return 0; }
        }
    }

}
