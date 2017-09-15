using NovelTheory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRangeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var testDate = new DateTime(1776, 7, 4);

            Debug.Assert(testDate.Add(TimeRange.OneYear) == new DateTime(1777, 7, 4));
            Debug.Assert(testDate.Add(10 * TimeRange.OneYear) == new DateTime(1786, 7, 4));
            Debug.Assert(testDate.Add(-TimeRange.OneYear) == new DateTime(1775, 7, 4));
            Debug.Assert(testDate.Subtract(TimeRange.OneYear) == new DateTime(1775, 7, 4));
            Debug.Assert(testDate.Subtract(-TimeRange.OneYear) == new DateTime(1777, 7, 4));

            Debug.Assert(testDate.Add(TimeRange.OneMonth) == new DateTime(1776, 8, 4));
            Debug.Assert(testDate.Add(4*TimeRange.OneMonth) == new DateTime(1776, 11, 4));


            Debug.Assert(testDate.Subtract(TimeRange.OneMonth) == new DateTime(1776, 6, 4));
            Debug.Assert(testDate.Subtract(4 * TimeRange.OneMonth) == new DateTime(1776, 3, 4));



        }
    }
}
