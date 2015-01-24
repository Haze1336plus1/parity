using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.App
{
    public class Time
    {

        /// <summary>
        /// Current TickCount / TicksPerMillisecond
        /// </summary>
        /// <returns>Current Time: Total Milliseconds</returns>
        public static long Get()
        {
            return (long)(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
        }

        /// <summary>
        /// (DateTime) TimeStamp/msec * TicksPerMillsecond
        /// </summary>
        /// <param name="TimeStamp">Total Milliseconds</param>
        /// <returns></returns>
        public static DateTime ToDateTime(long TimeStamp)
        {
            return new DateTime(TimeStamp * TimeSpan.TicksPerMillisecond);
        }

        /// <summary>
        /// (TimeSpan) TimeStamp/msec * TicksPerMillsecond
        /// </summary>
        /// <param name="TimeStamp">Total Milliseconds</param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(long TimeStamp)
        {
            return new TimeSpan(TimeStamp * TimeSpan.TicksPerMillisecond);
        }

        public static bool IsOlder(long a, long msec)
        {
            return a + msec <= Get();
        }

        public static bool IsYounger(long a, long msec)
        {
            return !IsOlder(a, msec);
        }

    }
}
