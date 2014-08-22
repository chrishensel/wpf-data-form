using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Wpf.Library.Data.Annotations;

namespace Wpf.DataForm.Library
{
    static class Utilities
    {
        internal static bool IsMarkedAsRequired(this PropertyInfo property)
        {
            return property.GetCustomAttributes(typeof(RequiredAttribute), false).Length == 1;
        }

        internal static bool IsMarkedAsAllowAnyValue(this PropertyInfo property)
        {
            return property.GetCustomAttributes(typeof(AllowAnyValueAttribute), false).Length == 1;
        }

        internal static string GetDisplayName(this PropertyInfo property)
        {
            DisplayAttribute attr = property.GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().FirstOrDefault();
            if (attr != null)
            {
                return attr.Name;
            }
            return property.Name;
        }
    }
}
