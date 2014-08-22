using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Wpf.DataForm.Library.DataForm.Builder.Factory
{
    class StaticControlFactory : IControlFactory
    {
        #region IControlFactory Members

        IEnumerable<string> IControlFactory.GetSupportedNames()
        {
            yield return "static";
            yield return "label";
        }

        UIElement IControlFactory.Build(ConstructionParameters parameters, IControlBuildService buildService)
        {
            parameters.ShowLabel = false;

            string text = buildService.LocalizationProvider.Localize("StaticElementNoTextSet");
            XAttribute textA = parameters.Node.Attribute("text");
            if (textA != null)
            {
                text = textA.Value;
            }

            return new Label() { Content = text, ToolTip = text };
        }

        #endregion
    }
}
