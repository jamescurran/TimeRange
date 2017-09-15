using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelTheory
{
    //     DateTime.Now.Subtract(5 * TimeRange.OneYear).Dump();

    public class TimeRange
    {
        private Func<DateTime, int, int, DateTime> _delta;
        private int _sign = 1;
        private int _count = 1;

        private TimeRange(Func<DateTime, int, int, DateTime> delta, int sign = 1, int count = 1)
        {
            _delta = delta;
            _sign = sign;
            _count = count;
         }

        public DateTime Transform(DateTime date)
        {
            return _delta(date, _sign, _count);
        }
        public static TimeRange operator -(TimeRange tr)
        {
            return new TimeRange(tr._delta, tr._sign * -1, tr._count);
        }

        public static TimeRange operator *(TimeRange tr, int count)
        {
            return new TimeRange(tr._delta, tr._sign, count);
        }
        public static TimeRange operator *(int count, TimeRange tr)
        {
            return new TimeRange(tr._delta, tr._sign, count);
        }

        public static readonly TimeRange OneDay = new TimeRange((dt, sign, count) => dt.AddDays(sign * count));
        public static readonly TimeRange OneYear = new TimeRange((dt, sign, count) => dt.AddYears(sign * count));
        public static readonly TimeRange OneMonth = new TimeRange((dt, sign, count) => dt.AddMonths(sign * count));
    }

    public static class TimeRangeExt
    {
        public static DateTime Add(this DateTime date, TimeRange range)
        {
            return range.Transform(date);
        }

        public static DateTime Subtract(this DateTime date, TimeRange range)
        {
            return Add(date, -range);
        }
    }
}
