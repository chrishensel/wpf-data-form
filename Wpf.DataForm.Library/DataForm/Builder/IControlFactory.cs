using System.Collections.Generic;
using System.Windows;

namespace Wpf.DataForm.Library.DataForm.Builder
{
    /// <summary>
    /// Defines a contract for a type that represents a control factory that can build controls based upon a declarative description.
    /// </summary>
    interface IControlFactory
    {
        /// <summary>
        /// Returns the names of all supported tags of controls (types) that this factory can build.
        /// </summary>
        /// <returns>The names of all supported tags of controls (types) that this factory can build.</returns>
        IEnumerable<string> GetSupportedNames();
        /// <summary>
        /// Builds the control using the provided parameters and services.
        /// </summary>
        /// <param name="parameters">The parameters to use for building the control.</param>
        /// <param name="buildService">The build service that provides additional information.</param>
        /// <returns>A specialized instance of <see cref="UIElement"/> that represents the control that has been built.</returns>
        UIElement Build(ConstructionParameters parameters, IControlBuildService buildService);
    }
}
