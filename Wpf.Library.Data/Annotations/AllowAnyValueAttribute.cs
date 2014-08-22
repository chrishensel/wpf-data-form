using System;

namespace Wpf.Library.Data.Annotations
{
    /// <summary>
    /// Specifies that the user may select any value, as opposed to choosing only one value from the list.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class AllowAnyValueAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AllowAnyValueAttribute"/> class.
        /// </summary>
        public AllowAnyValueAttribute()
        {

        }

        #endregion
    }
}
