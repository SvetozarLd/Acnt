using System;

namespace AccentBase.Utils
{
    public static class UnixDate
    {
        public static Int32 DateTimeToInt32(DateTime theDate)
        {
            theDate = theDate.ToUniversalTime();
            Int32 unixTime = (Int32)(DateTime.SpecifyKind(theDate, DateTimeKind.Utc) - new DateTime(1970, 1, 1)).TotalMinutes;
            return unixTime;
        }

        public static DateTime Int32ToDateTime(Int32 theDate)
        {
            DateTime pDate = (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddMinutes(theDate);
            return pDate.ToLocalTime();
        }

        public static Int64 DateTimeToInt64(DateTime theDate)
        {
            theDate = theDate.ToUniversalTime();
            Int64 unixTime = (Int64)(DateTime.SpecifyKind(theDate, DateTimeKind.Utc) - new DateTime(1970, 1, 1)).TotalMilliseconds;
            return unixTime;
        }

        public static DateTime Int64ToDateTime(Int64 theDate)
        {
            DateTime pDate = (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddMilliseconds(theDate);
            return pDate.ToLocalTime();
        }

        public static Int64 GetNowInt64()
        {
            Int64 result = DateTimeToInt64(DateTime.UtcNow);
            return result;
        }

        public static Int64 CheckedDateTimeToInt64(object ob)
        {
            if (ob != System.DBNull.Value)
            {
                return 0;
            }
            else
            {
                try
                {
                    return DateTimeToInt64(Convert.ToDateTime(ob));
                }
                catch
                {
                    return 0;
                }
            }
        }
    }
}
