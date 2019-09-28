using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccentServer.Utils
{
    static class CheckDBNull
    {
        static public Int32 ToInt32(object ob)
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
        static public string ToString(object ob)
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
        static public double ToDouble(object ob)
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

        static public long ToLong(object ob)
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

        static public bool ToBoolean(object ob)
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
        static public DateTime ToDateTime(object ob)
        {
            try
            {
                if (ob != System.DBNull.Value)
                {
                    return Convert.ToDateTime(ob);
                }
                else { return DateTime.MinValue; }
            }
            catch { return DateTime.MinValue; }
        }
    }
}
