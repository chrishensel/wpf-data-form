using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Linq;

namespace Wpf.DataForm.Library.DataForm.Builder.Factory
{
    class TextControlFactory : IControlFactory
    {
        #region Constants

        private const string StringFormatAttributeName = "string-format";
        private const string AcceptReturnKeyAttributeName = "accept-return";

        #endregion

        #region IControlFactory Members

        IEnumerable<string> IControlFactory.GetSupportedNames()
        {
            yield return "text";
        }

        UIElement IControlFactory.Build(ConstructionParameters parameters, IControlBuildService buildService)
        {
            TextBox text = new TextBox();

            Binding textBinding = buildService.CreateBinding(parameters.BindingSourceProperty.Name);

            XAttribute formatA = parameters.Node.Attribute(StringFormatAttributeName);
            if (formatA != null)
            {
                textBinding.StringFormat = formatA.Value;
            }
            XAttribute acceptReturnA = parameters.Node.Attribute(AcceptReturnKeyAttributeName);
            if (acceptReturnA != null && acceptReturnA.Value.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                text.AcceptsReturn = true;
                text.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            }

            buildService.SetBinding(text, TextBox.TextProperty, textBinding);

            return text;
        }

        #endregion
    }
}
