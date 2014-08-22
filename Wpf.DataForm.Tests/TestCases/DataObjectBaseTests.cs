using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wpf.Library.Data;

namespace Wpf.DataForm.Tests.TestCases
{
    [TestClass()]
    public class DataObjectBaseTests
    {
        [TestMethod()]
        public void IsUnmodifiedOnConstructionTest()
        {
            MockObject obj = new MockObject();
            Assert.IsFalse(obj.IsModified);
        }

        [TestMethod()]
        public void GetDefaultValueIfNotAssignedYetTest()
        {
            MockObject obj = new MockObject();

            Assert.AreEqual(default(int), obj.IntProperty);
        }

        [TestMethod()]
        public void SetAndGetIntPropertyValueTest()
        {
            MockObject obj = new MockObject();
            obj.IntProperty = 42;

            Assert.AreEqual(42, obj.IntProperty);
        }

        [TestMethod()]
        public void AssertPropertyChangedRaisedTest()
        {
            bool success = false;

            MockObject obj = new MockObject();
            obj.PropertyChanged += (o, e) =>
            {
                success = true;
            };

            obj.IntProperty = 1;

            if (!success)
            {
                Assert.Fail("Event not called!");
            }
        }

        [TestMethod()]
        public void AssertModifiedChangeOnSetTest()
        {
            MockObject obj = new MockObject();
            obj.IntProperty = 42;

            Assert.IsTrue(obj.IsModified);
        }

        [TestMethod()]
        public void AssertModifiedChangeOnSetDataObjectChildTest()
        {
            MockObject obj = new MockObject();
            obj.AnotherDobProperty = new MockObject();
            Assert.IsTrue(obj.IsModified);

            obj.MarkUnmodified();

            obj.AnotherDobProperty.IntProperty = 42;
            Assert.IsTrue(obj.IsModified);
        }

        [TestMethod()]
        public void AssertSetUnmodifiedWorksOnDataObjectChildTest()
        {
            MockObject obj = new MockObject();
            obj.AnotherDobProperty = new MockObject();
            obj.AnotherDobProperty.IntProperty = 42;

            obj.MarkUnmodified();

            Assert.IsFalse(obj.IsModified);
        }

        [TestMethod()]
        public void AssertSetUnmodifiedWorksTest()
        {
            MockObject obj = new MockObject();
            obj.IntProperty = 42;

            Assert.IsTrue(obj.IsModified);

            obj.MarkUnmodified();

            Assert.IsFalse(obj.IsModified);
        }

        [TestMethod()]
        public void DontModifyIfValueIsSameTest()
        {
            MockObject obj = new MockObject();
            obj.IntProperty = 42;
            obj.MarkUnmodified();

            obj.IntProperty = 42;

            Assert.IsFalse(obj.IsModified);
        }

        [TestMethod()]
        public void ValidationTest()
        {
            MockObject obj = new MockObject();
            obj.IntProperty = 11;

            var result = obj.Validate();
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod()]
        public void ShouldValidatePropertyTest()
        {
            MockObject obj = new MockObject();
            obj.DontValidateMeProperty = "A string that is way too long!";

            var result = obj.Validate();
            Assert.AreEqual(0, result.Count());
        }

        class MockObject : DataObjectBase
        {
            [Range(0, 10)]
            public int IntProperty
            {
                get { return GetValue(() => IntProperty); }
                set { SetValue(() => IntProperty, value); }
            }
            
            [StringLength(1)]
            public string DontValidateMeProperty
            {
                get { return GetValue(() => DontValidateMeProperty); }
                set { SetValue(() => DontValidateMeProperty, value); }
            }

            public MockObject AnotherDobProperty
            {
                get { return GetValue(() => AnotherDobProperty); }
                set { SetValue(() => AnotherDobProperty, value); }
            }

            protected override bool ShouldValidateProperty(System.Reflection.PropertyInfo property)
            {
                switch (property.Name)
                {
                    case "DontValidateMeProperty":
                        return false;
                }
                return base.ShouldValidateProperty(property);
            }
        }
    }
}
