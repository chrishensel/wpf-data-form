using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Linq;
using Wpf.DataForm.Library.Localization;

namespace Wpf.DataForm.Library.DataForm.Builder
{
    /// <summary>
    /// Defines members for the type that is responsible for building the controls.
    /// </summary>
    interface IControlBuildService
    {
        /// <summary>
        /// Gets the localization provider instance to use for localizing tokens.
        /// </summary>
        ILocalizationProvider LocalizationProvider { get; }
        /// <summary>
        /// Gets the <see cref="IDataFormControlService"/>-instance that gives information about the host.
        /// </summary>
        IDataFormControlService DataFormControlService { get; }

        /// <summary>
        /// Builds content for one single XML node.
        /// </summary>
        /// <param name="node">The XML node to construct.</param>
        /// <param name="parent">The parent grid, if any.</param>
        /// <returns>The content that has been built.</returns>
        ContentBuildResult BuildNode(XElement node, Grid parent);
        /// <summary>
        /// Factory method to create a new binding that has all necessary properties set to interact with the host.
        /// </summary>
        /// <param name="path">The binding path to use.</param>
        /// <returns>A convenient binding.</returns>
        Binding CreateBinding(string path);
        /// <summary>
        /// Sets the binding that has been created by <see cref="CreateBinding(string)"/>.
        /// </summary>
        /// <param name="target">The target object to establish the binding on.</param>
        /// <param name="dp">The dependency property for the binding.</param>
        /// <param name="binding">The binding to use.</param>
        void SetBinding(DependencyObject target, DependencyProperty dp, Binding binding);
    }
}
