using System;
using System.Globalization;
using System.Windows.Data;
using Wpf.DataForm.Library.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Wpf.DataForm.Tests.TestCases
{
    [TestClass()]
    public class TimeSpanToStringConverterTests
    {
        [TestMethod()]
        public void Convert_ValidInput_Test()
        {
            IValueConverter converter = new TimeSpanToStringConverter();
            TimeSpan input = new TimeSpan(13, 37, 55);

            object result = converter.Convert(input, typeof(TimeSpan), null, CultureInfo.InvariantCulture);

            Assert.AreEqual(input.ToString(), result);
        }

        [TestMethod()]
        public void Convert_ValidInput_ButTooLarge_Test()
        {
            IValueConverter converter = new TimeSpanToStringConverter();
            TimeSpan input = new TimeSpan(2, 13, 37, 55);

            object result = converter.Convert(input, typeof(TimeSpan), null, CultureInfo.InvariantCulture);

            Assert.AreEqual(new TimeSpan(23, 59, 59).ToString(), result);
        }

        [TestMethod()]
        public void Convert_ValidInput_ButTooSmall_Test()
        {
            IValueConverter converter = new TimeSpanToStringConverter();
            TimeSpan input = new TimeSpan(-2, 13, 37, 55);

            object result = converter.Convert(input, typeof(TimeSpan), null, CultureInfo.InvariantCulture);

            Assert.AreEqual(TimeSpan.Zero.ToString(), result);
        }

        [TestMethod()]
        public void Convert_InvalidInput_Test()
        {
            IValueConverter converter = new TimeSpanToStringConverter();

            object result = converter.Convert("42", typeof(string), null, CultureInfo.InvariantCulture);

            Assert.AreEqual(TimeSpan.Zero.ToString(), result);
        }

        [TestMethod()]
        public void ConvertBack_ValidInput_Test()
        {
            IValueConverter converter = new TimeSpanToStringConverter();
            string input = "13:37:55";

            object result = converter.ConvertBack(input, typeof(string), null, CultureInfo.InvariantCulture);

            Assert.AreEqual(new TimeSpan(13, 37, 55), result);
        }

        [TestMethod()]
        public void ConvertBack_ValidInput_ButTooLarge_Test()
        {
            IValueConverter converter = new TimeSpanToStringConverter();
            string input = "42:00:00";

            object result = converter.ConvertBack(input, typeof(string), null, CultureInfo.InvariantCulture);

            Assert.AreEqual(new TimeSpan(23, 59, 59), result);
        }

        [TestMethod()]
        public void ConvertBack_InvalidInput_Test()
        {
            IValueConverter converter = new TimeSpanToStringConverter();
            int input = 1337;

            object result = converter.ConvertBack(input, typeof(int), null, CultureInfo.InvariantCulture);

            Assert.AreEqual(TimeSpan.Zero, result);
        }
    }
}
