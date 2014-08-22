using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Wpf.Library.Data.Annotations;

namespace Wpf.DataForm.Sample.DataObjects
{
    class Person : ArtificalBase
    {
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
    }
}
