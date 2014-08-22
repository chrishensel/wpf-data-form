using System;
using System.Windows;

namespace Wpf.DataForm.Library.Controls
{
    /// <summary>
    /// Provides a control that allows the user to select a date and time, or to choose to ignore the value (setting it to null).
    /// </summary>
    partial class DateTimePicker
    {
        #region Properties

        /// <summary>
        /// Gets/sets whether or not the user has marked this field as having a value.
        /// If this is set to false, then the user has chosen to not pick a date and time and the values shall be ignored.
        /// </summary>
        public bool IsValueAvailable
        {
            get { return (bool)GetValue(IsValueAvailableProperty); }
            set { SetValue(IsValueAvailableProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="IsValueAvailable"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsValueAvailableProperty = DependencyProperty.Register("IsValueAvailable", typeof(bool), typeof(DateTimePicker), new PropertyMetadata(false, RelevantPropertyValueChanged));

        /// <summary>
        /// Gets/sets the date-part of the value.
        /// This value is only to be considered if <see cref="IsValueAvailable"/> is true.
        /// </summary>
        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Date"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DateProperty = DependencyProperty.Register("Date", typeof(DateTime), typeof(DateTimePicker), new PropertyMetadata(DateTime.Now, RelevantPropertyValueChanged));

        /// <summary>
        /// Gets/sets the time-part of the value.
        /// This value is only to be considered if <see cref="IsValueAvailable"/> is true.
        /// </summary>
        public TimeSpan Time
        {
            get { return (TimeSpan)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Time"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(TimeSpan), typeof(DateTimePicker), new PropertyMetadata(TimeSpan.Zero, RelevantPropertyValueChanged));

        private static void RelevantPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateTimePicker sender = (DateTimePicker)d;
            if (!sender.IsValueAvailable)
            {
                sender.Value = null;
            }
            else
            {
                sender.Value = new DateTime(sender.Date.Year, sender.Date.Month, sender.Date.Day, sender.Time.Hours, sender.Time.Minutes, sender.Time.Seconds, sender.Time.Milliseconds);
            }
        }

        /// <summary>
        /// Gets/sets the combined value of <see cref="Date"/> and <see cref="Time"/>.
        /// If <see cref="IsValueAvailable"/> is set to false, then this is null.
        /// </summary>
        public DateTime? Value
        {
            get { return (DateTime?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(DateTime?), typeof(DateTimePicker), new PropertyMetadata(null, ValuePropertyValueChanged));

        private static void ValuePropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateTimePicker sender = (DateTimePicker)d;
            DateTime? newValue = (DateTime?)e.NewValue;

            if (!newValue.HasValue)
            {
                sender.IsValueAvailable = false;
            }
            else
            {
                sender.Date = newValue.Value.Date;
                sender.Time = newValue.Value.TimeOfDay;
                sender.IsValueAvailable = true;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimePicker"/> class.
        /// </summary>
        public DateTimePicker()
        {
            this.InitializeComponent();
        }

        #endregion
    }
}