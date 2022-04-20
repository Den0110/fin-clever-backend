using System;
using System.Collections.Generic;

namespace FinClever
{
	public class TimeUtils
	{

        public static long GetTime()
        {
            return ((DateTimeOffset)DateTime.Now).ToUnixTimeMilliseconds();
        }

        public static IEnumerable<long> GetDaysForRange(string range)
        {
            var endDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            var startDate = endDate.AddMonths(-1);
            var dayStep = 1;
            switch (range)
            {
                case "W":
                    startDate = endDate.AddDays(-7);
                    dayStep = 1;
                    break;
                case "M":
                    startDate = endDate.AddMonths(-1);
                    dayStep = 1;
                    break;
                case "6M":
                    startDate = GetLastFriday(endDate.AddMonths(-6));
                    dayStep = 7;
                    break;
                case "1Y":
                    startDate = GetLastFriday(endDate.AddYears(-1));
                    dayStep = 7;
                    break;
                case "ALL":
                    startDate = GetLastYearHalf(endDate.AddYears(-10));
                    dayStep = 6 * 30;
                    break;
            }

            var startDay = ((DateTimeOffset)startDate).ToUnixTimeSeconds();
            var endDay = ((DateTimeOffset)endDate).ToUnixTimeSeconds();
            var step = dayStep * 24 * 60 * 60;
            long i;
            for (i = startDay; i <= endDay; i += step)
                yield return i;
            if (i != endDay + step)
                yield return endDay;
        }

        private static DateTime GetLastFriday(DateTime? startDate = null)
        {
            var initDate = startDate ?? DateTime.Now;
            DateTime lastFriday = initDate.Date.AddDays(1).AddSeconds(-1);
            while (lastFriday.DayOfWeek != DayOfWeek.Friday)
                lastFriday = lastFriday.AddDays(-1);
            return lastFriday;
        }

        private static DateTime GetLastYearHalf(DateTime? startDate = null)
        {
            var initDate = startDate ?? DateTime.Now;
            DateTime lastYearHalf = initDate.Date.AddDays(1).AddSeconds(-1);
            while (lastYearHalf.Month != 1 && lastYearHalf.Month != 7)
                lastYearHalf = lastYearHalf.AddMonths(-1);
            while (lastYearHalf.Day != 1)
                lastYearHalf = lastYearHalf.AddDays(-1);
            while (lastYearHalf.DayOfWeek != DayOfWeek.Friday)
                lastYearHalf = lastYearHalf.AddDays(-1);
            return lastYearHalf;
        }

        public static long ParseDate(string date)
        {
            return ((DateTimeOffset)DateTime.Parse(date)).ToUnixTimeSeconds();
        }
    }
}

