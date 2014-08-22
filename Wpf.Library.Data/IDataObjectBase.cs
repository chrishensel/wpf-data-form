using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Wpf.Library.Data
{
    /// <summary>
    /// Defines members that form the base of a generic data object.
    /// </summary>
    public interface IDataObjectBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets whether or not the properties in this object are modified.
        /// If there are properties who are of type <see cref="IDataObjectBase"/>, then those are evaluated as well.
        /// </summary>
        bool IsModified { get; }

        /// <summary>
        /// Marks the object (and its child objects) as being unmodified.
        /// </summary>
        void MarkUnmodified();
        /// <summary>
        /// Performs validation on this object and returns the result.
        /// </summary>
        /// <returns>An enumerable containing the result of validation.</returns>
        IEnumerable<ValidationResult> Validate();
    }
}
