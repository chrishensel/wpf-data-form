using Wpf.Library.Data;

namespace Wpf.DataForm.Library.DataForm
{
    /// <summary>
    /// Exposes the properties of the DataFormControl to internal types.
    /// </summary>
    interface IDataFormControlService
    {
        /// <summary>
        /// Gets access to the underlying data object on which the form is based on.
        /// </summary>
        DataObjectBase DataFormObject { get; }
        /// <summary>
        /// Gets an object that contains styling information.
        /// </summary>
        DataFormControlStyle Styling { get; }
        /// <summary>
        /// Gets whether or not the control is currently in debug-mode.
        /// </summary>
        bool IsDebugMode { get; }
    }
}
