using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using Wpf.DataForm.Library.DataForm;
using Wpf.DataForm.Library.DataForm.Builder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMock;
using Wpf.Library.Data.Annotations;
using Wpf.Library.Data;

namespace Wpf.DataForm.Tests.TestCases
{
    [TestClass()]
    public class FormBuilderTests
    {
        [TestMethod]
        public void BuildTest()
        {
            MockFactory factory = new MockFactory();
            var dfcs = factory.CreateMock<IDataFormControlService>();
            dfcs.Expects.Any.GetProperty(_ => _.DataFormObject).WillReturn(new DataObject());
            dfcs.Expects.Any.GetProperty(_ => _.IsDebugMode).WillReturn(false);
            dfcs.Expects.Any.GetProperty(_ => _.Styling).WillReturn(new DataFormControlStyle());

            Grid layoutRoot = new Grid();

            FormBuilder builder = new FormBuilder(dfcs.MockObject, layoutRoot);
            var result = builder.Build(XDocument.Parse(Properties.Resources.SampleLayout1).Root).ToList();

            Assert.AreEqual(1, result.Count);
        }

        class DataObject : DataObjectBase
        {
            [Required()]
            [Display(Name = "Is valid")]
            public bool IsValid
            {
                get { return GetValue(() => IsValid); }
                set { SetValue(() => IsValid, value); }
            }

            [Required()]
            [Display(Name = "Comment")]
            public string Comment
            {
                get { return GetValue(() => Comment); }
                set { SetValue(() => Comment, value); }
            }

            [Display(Name = "My test")]
            public ICommand DoSomethingCommand { get; set; }

            [Required()]
            [Display(Name = "First name")]
            public string FirstName
            {
                get { return GetValue(() => FirstName); }
                set
                {
                    SetValue(() => FirstName, value);
                    OnPropertyChanged("JoinDateIsEnabled");
                }
            }

            [Required()]
            [Display(Name = "Last name")]
            public string LastName
            {
                get { return GetValue(() => LastName); }
                set
                {
                    SetValue(() => LastName, value);
                    OnPropertyChanged("JoinDateIsEnabled");
                }
            }

            [Required()]
            [Display(Name = "Join date")]
            public DateTime JoinDate
            {
                get { return GetValue(() => JoinDate); }
                set { SetValue(() => JoinDate, value); }
            }

            public bool JoinDateIsEnabled
            {
                get { return !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName); }
            }

            [Display(Name = "Leave date")]
            public DateTime? LeaveDate
            {
                get { return GetValue(() => LeaveDate); }
                set { SetValue(() => LeaveDate, value); }
            }

            [Required()]
            [Display(Name = "Age")]
            public int Age
            {
                get { return GetValue(() => Age); }
                set { SetValue(() => Age, value); }
            }

            public IEnumerable<int> AgeItemsSource
            {
                get
                {
                    for (int i = 0; i < 130; i++)
                    {
                        yield return i;
                    }
                }
            }

            [Required()]
            [Display(Name = "City")]
            public string City
            {
                get { return GetValue(() => City); }
                set { SetValue(() => City, value); }
            }

            [AllowAnyValue()]
            public IEnumerable<string> CityItemsSource
            {
                get
                {
                    yield return "Nürnberg";
                    yield return "Ansbach";
                }
            }

            protected override bool ShouldValidateProperty(PropertyInfo property)
            {
                if (property.Name == "FirstName")
                {
                    return false;
                }
                return base.ShouldValidateProperty(property);
            }

            public DataObject()
            {
            }
        }
    }
}
