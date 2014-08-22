using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Wpf.DataForm.Library.DataForm.Builder.Factory
{
    class CheckControlFactory : IControlFactory
    {
        #region IControlFactory Members

        IEnumerable<string> IControlFactory.GetSupportedNames()
        {
            yield return "check";
        }

        UIElement IControlFactory.Build(ConstructionParameters parameters, IControlBuildService buildService)
        {
            CheckBox checkBox = new CheckBox();
            checkBox.VerticalAlignment = VerticalAlignment.Center;

            XAttribute textA = parameters.Node.Attribute("text");
            if (textA != null)
            {
                TextBlock text = new TextBlock();
                text.Focusable = false;
                text.Text = textA.Value;
                checkBox.Content = text;
            }

            buildService.SetBinding(checkBox, CheckBox.IsCheckedProperty, buildService.CreateBinding(parameters.BindingSourceProperty.Name));

            return checkBox;
        }

        #endregion
    }
}
