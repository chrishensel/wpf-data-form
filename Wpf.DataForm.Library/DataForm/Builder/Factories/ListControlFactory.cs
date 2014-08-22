using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Wpf.DataForm.Library.DataForm.Builder.Factory
{
    class ListControlFactory : IControlFactory
    {
        #region IControlFactory Members

        IEnumerable<string> IControlFactory.GetSupportedNames()
        {
            yield return "list";
        }

        UIElement IControlFactory.Build(ConstructionParameters parameters, IControlBuildService buildService)
        {
            string path = parameters.BindingSourceProperty.Name;

            PropertyInfo propItemsSource = buildService.DataFormControlService.DataFormObject.GetType().GetProperty(path + "ItemsSource");
            if (propItemsSource == null)
            {
                throw new InvalidOperationException(string.Format(buildService.LocalizationProvider.Localize("CouldNotFindItemsSourceProperty"), propItemsSource.Name, parameters.BindingSourceProperty.Name));
            }

            ComboBox list = new ComboBox();

            Binding itemsSourceBinding = buildService.CreateBinding(propItemsSource.Name);
            buildService.SetBinding(list, ItemsControl.ItemsSourceProperty, itemsSourceBinding);

            Binding textOrSelectedItemBinding = buildService.CreateBinding(path);
            // We need to bind to different properties depending on whether any input is allowed.
            if (propItemsSource.IsMarkedAsAllowAnyValue())
            {
                list.IsEditable = true;
                buildService.SetBinding(list, ComboBox.TextProperty, textOrSelectedItemBinding);
            }
            else
            {
                buildService.SetBinding(list, ComboBox.SelectedItemProperty, textOrSelectedItemBinding);
            }

            return list;
        }

        #endregion
    }
}
