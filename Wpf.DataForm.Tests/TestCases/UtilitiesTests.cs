using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Wpf.DataForm.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wpf.Library.Data.Annotations;

namespace Wpf.DataForm.Tests.TestCases
{
    [TestClass()]
    public class UtilitiesTests
    {
        [TestMethod()]
        public void GetDisplayNameTest()
        {
            PropertyInfo pi = typeof(AnnotatedClass).GetProperty("DisplayNameProperty");

            string result = pi.GetDisplayName();

            Assert.AreEqual("Test", result);
        }

        [TestMethod()]
        public void GetDisplayNameNegativeTest()
        {
            PropertyInfo pi = typeof(AnnotatedClass).GetProperty("NoDisplayNameProperty");

            string result = pi.GetDisplayName();

            Assert.AreEqual("NoDisplayNameProperty", result);
        }

        [TestMethod()]
        public void IsMarkedAsRequiredTest()
        {
            PropertyInfo pi = typeof(AnnotatedClass).GetProperty("RequiredProperty");

            bool result = pi.IsMarkedAsRequired();

            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        public void AllowAnyValueTest()
        {
            PropertyInfo pi = typeof(AnnotatedClass).GetProperty("AllowAnyValueProperty");

            bool result = pi.IsMarkedAsAllowAnyValue();

            Assert.AreEqual(true, result);
        }

        class AnnotatedClass
        {
            [Display(Name = "Test")]
            public string DisplayNameProperty { get; set; }
            public string NoDisplayNameProperty { get; set; }
            [Required()]
            public bool RequiredProperty { get; set; }
            [AllowAnyValue()]
            public object AllowAnyValueProperty { get; set; }
        }
    }
}
