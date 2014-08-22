using System;
using System.Globalization;
using System.Windows.Data;

namespace Wpf.DataForm.Library.Converters
{
    /// <summary>
    /// Represents a value converter that converts a <see cref="TimeSpan"/> to a string and vice versa.
    /// </summary>
    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public class TimeSpanToStringConverter : IValueConverter
    {
        #region IValueConverter Members

        private static TimeSpan GetLimited(TimeSpan input)
        {
            if (input.Days < 0)
            {
                return TimeSpan.Zero;
            }
            else if (input.Days > 1)
            {
                return new TimeSpan(0, 23, 59, 59);
            }
            return input;
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan)
            {
                return GetLimited(((TimeSpan)value)).ToString(null, culture);
            }
            return TimeSpan.Zero.ToString(null, culture);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = value as string;
            if (str != null)
            {
                TimeSpan vv = TimeSpan.Zero;
                TimeSpan.TryParse(str, culture, out vv);
                return GetLimited(vv);
            }
            return TimeSpan.Zero;
        }

        #endregion
    }
}
