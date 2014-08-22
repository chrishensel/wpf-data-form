using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using Wpf.DataForm.Library.Controls;

namespace Wpf.DataForm.Library.DataForm.Builder.Factory
{
    class DateTimeControlFactory : IControlFactory
    {
        #region IControlFactory Members

        IEnumerable<string> IControlFactory.GetSupportedNames()
        {
            yield return "date";
            yield return "datetime";
        }

        UIElement IControlFactory.Build(ConstructionParameters parameters, IControlBuildService buildService)
        {
            DateTimePicker picker = new DateTimePicker();

            Binding valueBinding = buildService.CreateBinding(parameters.BindingSourceProperty.Name);
            valueBinding.Mode = BindingMode.TwoWay;
            buildService.SetBinding(picker, DateTimePicker.ValueProperty, valueBinding);

            return picker;
        }

        #endregion
    }
}
