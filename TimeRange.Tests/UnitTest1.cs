using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using NovelTheory;


namespace NovelTheory.TimeRangeX.Tests
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void OneYear_Simple()
        {
            var startDate = new DateTime(1776, 7, 4);
            var testDate = startDate.Add(TimeRange.OneYear);
            var expected = new DateTime(1777, 7, 4);

            Assert.AreEqual(expected, testDate);
        }

        [TestMethod]
        public void OneYear_PrefixMultiply()
        {
            var startDate = new DateTime(1776, 7, 4);
            var testDate = startDate.Add(10 * TimeRange.OneYear);
            var expected = new DateTime(1786, 7, 4);

            Assert.AreEqual(expected, testDate);
        }

        [TestMethod]
        public void OneYear_PostfixMultiply()
        {
            var startDate = new DateTime(1776, 7, 4);
            var testDate = startDate.Add(TimeRange.OneYear * 10);
            var expected = new DateTime(1786, 7, 4);

            Assert.AreEqual(expected, testDate);
        }
        [TestMethod]
        public void OneYear_Negate()
        {
            var startDate = new DateTime(1776, 7, 4);
            var testDate = startDate.Add(-TimeRange.OneYear);
            var expected = new DateTime(1775, 7, 4);

            Assert.AreEqual(expected, testDate);
        }
        public void OneYear_Subtract()
        {
            var startDate = new DateTime(1776, 7, 4);
            var testDate = startDate.Subtract(TimeRange.OneYear );
            var expected = new DateTime(1775, 7, 4);

            Assert.AreEqual(expected, testDate);
        }
        public void OneYear_SubtractNegative()
        {
            var startDate = new DateTime(1776, 7, 4);
            var testDate = startDate.Subtract(-TimeRange.OneYear);
            var expected = new DateTime(1777, 7, 4);

            Assert.AreEqual(expected, testDate);
        }
    }
}
