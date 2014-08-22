using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Linq;
using Wpf.DataForm.Library.DataForm.Builder.Factory;
using Wpf.DataForm.Library.Localization;

namespace Wpf.DataForm.Library.DataForm.Builder
{
    class FormBuilder : IControlBuildService
    {
        #region Fields

        private IDataFormControlService _service;
        private Grid _parent;
        private List<IControlFactory> _controlFactories;

        #endregion

        #region Constructors

        private FormBuilder()
        {
            _controlFactories = new List<IControlFactory>();
            _controlFactories.Add(new StaticControlFactory());
            _controlFactories.Add(new TextControlFactory());
            _controlFactories.Add(new ListControlFactory());
            _controlFactories.Add(new CheckControlFactory());
            _controlFactories.Add(new TableControlFactory());
            _controlFactories.Add(new DateTimeControlFactory());
            _controlFactories.Add(new ButtonControlFactory());
        }

        public FormBuilder(IDataFormControlService service, Grid parent)
            : this()
        {
            _service = service;
            _parent = parent;
        }

        #endregion

        #region Methods

        internal IEnumerable<UIElement> Build(XElement root)
        {
            List<UIElement> result = new List<UIElement>();

            foreach (XElement child in root.Elements())
            {
                if (child.Name == "table")
                {
                    ContentBuildResult buildTableResult = ((IControlBuildService)this).BuildNode(child, _parent);
                    yield return buildTableResult.Element;
                }
                else
                {
                    throw new InvalidOperationException(LocalizationManager.LocalizationProvider.Localize("InvalidRootChildNode"));
                }
            }
        }

        private PropertyInfo GetDataFormObjectPropertyInfoOrNull(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            return _service.DataFormObject.GetType().GetProperty(name);
        }

        private void TrySetupBindingPathBinding(ConstructionParameters parameters, ContentBuildResult result)
        {
            if (!parameters.KnownAttributes.HasBindingPath)
            {
                return;
            }

            string bexpr = parameters.KnownAttributes.BindingPath;
            PropertyInfo propDest = GetDataFormObjectPropertyInfoOrNull(bexpr);
            if (propDest == null)
            {
                throw new InvalidOperationException(string.Format(LocalizationManager.LocalizationProvider.Localize("PropertyNotFoundOnInstance"), bexpr, _service.DataFormObject.GetType()));
            }

            parameters.BindingSourceProperty = propDest;

            result.DisplayName = propDest.GetDisplayName();
            if (string.IsNullOrWhiteSpace(result.DisplayName))
            {
                result.DisplayName = LocalizationManager.LocalizationProvider.Localize("NoLabelText");
            }
        }

        private void TrySetupBackgroundBinding(ConstructionParameters parameters, ContentBuildResult result)
        {
            PropertyInfo propDest = GetDataFormObjectPropertyInfoOrNull(parameters.KnownAttributes.BindingPath);
            if (propDest == null || !propDest.IsMarkedAsRequired())
            {
                return;
            }

            Control control = result.Element as Control;
            if (control != null)
            {
                Binding binding = new Binding("Styling.RequiredBrush");
                binding.Source = _service;
                BindingOperations.SetBinding(control, Control.BackgroundProperty, binding);
            }
        }

        private void TrySetupIsEnabledBinding(ConstructionParameters parameters, ContentBuildResult result)
        {
            if (!parameters.KnownAttributes.HasBindingPath)
            {
                return;
            }

            string bexpr = parameters.KnownAttributes.BindingPath;
            PropertyInfo propEnabled = _service.DataFormObject.GetType().GetProperty(bexpr + "IsEnabled");
            if (propEnabled != null)
            {
                Binding enabledBinding = new Binding(propEnabled.Name);
                enabledBinding.Source = _service.DataFormObject;
                BindingOperations.SetBinding(result.Element, UIElement.IsEnabledProperty, enabledBinding);
            }
        }

        private IControlFactory GetControlFactoryWithFault(string type)
        {
            IControlFactory controlFactory = _controlFactories.FirstOrDefault(item => item.GetSupportedNames().Contains(type));
            if (controlFactory == null)
            {
                throw new InvalidOperationException(LocalizationManager.LocalizationProvider.Localize("NoFactoryFoundForGivenToken"));
            }
            return controlFactory;
        }

        private void TrySetKnownAttributes(ConstructionParameters parameters, UIElement element)
        {
            FrameworkElement fe = element as FrameworkElement;
            if (fe == null)
            {
                return;
            }

            if (parameters.KnownAttributes.Width.HasValue)
            {
                // When setting width, we also need to enforce correct alignment,
                // otherwise the control will be centered which doesn't look too good.
                fe.HorizontalAlignment = HorizontalAlignment.Left;
                fe.Width = parameters.KnownAttributes.Width.Value;
            }
            if (parameters.KnownAttributes.Height.HasValue)
            {
                fe.Height = parameters.KnownAttributes.Height.Value;
            }
            fe.ToolTip = parameters.KnownAttributes.ToolTipText;
        }

        #endregion

        #region IControlBuildService Members

        ILocalizationProvider IControlBuildService.LocalizationProvider
        {
            get { return LocalizationManager.LocalizationProvider; }
        }

        IDataFormControlService IControlBuildService.DataFormControlService
        {
            get { return _service; }
        }

        Binding IControlBuildService.CreateBinding(string path)
        {
            Binding binding = new Binding(path);
            binding.Source = _service.DataFormObject;
            return binding;
        }

        void IControlBuildService.SetBinding(DependencyObject target, DependencyProperty dp, Binding binding)
        {
            BindingOperations.SetBinding(target, dp, binding);
        }

        ContentBuildResult IControlBuildService.BuildNode(XElement node, Grid parent)
        {
            Tracing.WriteVerbose(Properties.Resources.FormBuilderBuildNodeStart, node.Name);

            ConstructionParameters parameters = new ConstructionParameters(node);
            parameters.Parent = parent;

            ContentBuildResult result = new ContentBuildResult();
            TrySetupBindingPathBinding(parameters, result);

            string type = node.Name.LocalName.ToLowerInvariant();
            IControlFactory controlFactory = GetControlFactoryWithFault(type);
            result.Element = controlFactory.Build(parameters, this);
            result.ShowLabel = parameters.ShowLabel;

            TrySetKnownAttributes(parameters, result.Element);

            TrySetupIsEnabledBinding(parameters, result);
            TrySetupBackgroundBinding(parameters, result);

            return result;
        }

        #endregion
    }
}
