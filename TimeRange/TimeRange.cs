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
        private Func<DateTime, int, DateTime> _delta;
        private int _count = 1;

        private TimeRange(Func<DateTime, int, DateTime> delta, int count = 1)
        {
            _delta = delta;
            _count = count;
        }

        public DateTime Transform(DateTime date)
        {
            return _delta(date, _count);
        }
        public static TimeRange operator -(TimeRange tr)
        {
            return new TimeRange(tr._delta, -tr._count);
        }

        public static TimeRange operator *(TimeRange tr, int multiplier)
        {
            return new TimeRange(tr._delta, tr._count * multiplier);
        }
        public static TimeRange operator *(int multiplicand, TimeRange tr)
        {
            return new TimeRange(tr._delta, multiplicand * tr._count);
        }

        public static readonly TimeRange OneDay = new TimeRange((dt, count) => dt.AddDays(count));
        public static readonly TimeRange OneYear = new TimeRange((dt, count) => dt.AddYears(count));
        public static readonly TimeRange OneMonth = new TimeRange((dt, count) => dt.AddMonths(count));
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
