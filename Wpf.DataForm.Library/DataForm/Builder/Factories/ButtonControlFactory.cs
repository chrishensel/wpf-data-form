using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Linq;

namespace Wpf.DataForm.Library.DataForm.Builder.Factory
{
    class ButtonControlFactory : IControlFactory
    {
        #region Constants

        private const string TextAttributeName = "text";

        #endregion

        #region IControlFactory Members

        IEnumerable<string> IControlFactory.GetSupportedNames()
        {
            yield return "button";
        }

        UIElement IControlFactory.Build(ConstructionParameters parameters, IControlBuildService buildService)
        {
            parameters.ShowLabel = false;

            Button button = new Button();

            Binding onClickBinding = buildService.CreateBinding(parameters.BindingSourceProperty.Name);
            buildService.SetBinding(button, Button.CommandProperty, onClickBinding);
            button.Content = parameters.BindingSourceProperty.GetDisplayName();

            XAttribute textA = parameters.Node.Attribute(TextAttributeName);
            if (textA != null)
            {
                button.Content = textA.Value;
            }

            return button;
        }

        #endregion
    }
}
