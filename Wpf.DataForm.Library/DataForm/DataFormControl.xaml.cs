using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using Wpf.DataForm.Library.DataForm.Builder;
using Wpf.DataForm.Library.DataForm.FormFill;
using Wpf.DataForm.Library.DataForm.XmlLinkResolver;
using Wpf.Library.Data;

namespace Wpf.DataForm.Library.DataForm
{
    /// <summary>
    /// Interaction logic for DataFormControl.xaml.
    /// </summary>
    public partial class DataFormControl : UserControl, IDataFormControlService
    {
        #region Fields

        private XmlLinkResolverRegistry _xmlLinkResolverRegistry;

        /// <summary>
        /// Identifies the TriggerValidation command, which triggers validation immediately.
        /// </summary>
        public static readonly RoutedCommand TriggerValidationCommand = new RoutedCommand("TriggerValidation", typeof(DataFormControl));

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets the <see cref="DataObject"/> to edit.
        /// </summary>
        public DataObjectBase DataFormObject
        {
            get { return (DataObjectBase)GetValue(DataFormObjectProperty); }
            set { SetValue(DataFormObjectProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="DataFormObject"/> property.
        /// </summary>
        public static readonly DependencyProperty DataFormObjectProperty = DependencyProperty.Register("DataFormObject", typeof(DataObjectBase), typeof(DataFormControl), new PropertyMetadata(null, DataFormObjectPropertyChanged));

        private static void DataFormObjectPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataFormControl control = (DataFormControl)d;

            control.TryWireupDataFormObjectPropertyChanged((DataObjectBase)e.OldValue, false);
            control.TryWireupDataFormObjectPropertyChanged((DataObjectBase)e.NewValue, true);
        }

        /// <summary>
        /// Gets/sets the layout description for this data form.
        /// </summary>
        public string Layout
        {
            get { return (string)GetValue(LayoutProperty); }
            set { SetValue(LayoutProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Layout"/> property.
        /// </summary>
        public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register("Layout", typeof(string), typeof(DataFormControl), new PropertyMetadata(null, LayoutPropertyChanged));

        private static void LayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string value = e.NewValue as string;
            ((DataFormControl)d).ApplyLayout(value);
        }

        /// <summary>
        /// Gets/sets whether or not this control is in debug mode. If this is set to true, then additional visuals are being displayed.
        /// </summary>
        public bool IsDebugMode
        {
            get { return (bool)GetValue(IsDebugModeProperty); }
            set { SetValue(IsDebugModeProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="IsDebugMode"/> property.
        /// </summary>
        public static readonly DependencyProperty IsDebugModeProperty = DependencyProperty.Register("IsDebugMode", typeof(bool), typeof(DataFormControl), new UIPropertyMetadata(false));

        /// <summary>
        /// Gets/sets the styling information for the <see cref="DataFormControl"/>.
        /// </summary>
        public DataFormControlStyle Styling
        {
            get { return (DataFormControlStyle)GetValue(StylingProperty); }
            set { SetValue(StylingProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Styling"/> property.
        /// </summary>
        public static readonly DependencyProperty StylingProperty = DependencyProperty.Register("Styling", typeof(DataFormControlStyle), typeof(DataFormControl), new UIPropertyMetadata(new DataFormControlStyle()));

        /// <summary>
        /// Gets/sets whether or not the data form shall perform validation if a property has changed its value.
        /// </summary>
        public bool ValidateOnPropertyChange
        {
            get { return (bool)GetValue(ValidateOnPropertyChangeProperty); }
            set { SetValue(ValidateOnPropertyChangeProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ValidateOnPropertyChange"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValidateOnPropertyChangeProperty = DependencyProperty.Register("ValidateOnPropertyChange", typeof(bool), typeof(DataFormControl), new UIPropertyMetadata(true));

        /// <summary>
        /// Gets/sets the data validation result, that is a collection of validation results.
        /// </summary>
        public DataValidationResult ValidationResult
        {
            get { return (DataValidationResult)GetValue(ValidationResultProperty.DependencyProperty); }
            set { SetValue(ValidationResultProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ValidationResult"/> dependency property.
        /// </summary>
        public static readonly DependencyPropertyKey ValidationResultProperty = DependencyProperty.RegisterReadOnly("ValidationResult", typeof(DataValidationResult), typeof(DataFormControl), new UIPropertyMetadata(new DataValidationResult()));

        /// <summary>
        /// Gets whether or not the validation did succeed, or fail.
        /// </summary>
        public bool IsValidationSuccess
        {
            get { return (bool)GetValue(IsValidationSuccessProperty); }
            set { SetValue(IsValidationSuccessProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="IsValidationSuccess"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsValidationSuccessProperty = DependencyProperty.Register("IsValidationSuccess", typeof(bool), typeof(DataFormControl), new UIPropertyMetadata(true));

        /// <summary>
        /// Gets the <see cref="IFormFiller"/> instance that can be used to set or query the form data.
        /// </summary>
        public IFormFiller FormFiller { get; private set; }
        /// <summary>
        /// Gets the <see cref="IXmlLinkRegistry"/> instance that can be used to register links within the layout XML file.
        /// </summary>
        public IXmlLinkRegistry XmlLinkRegistry
        {
            get { return _xmlLinkResolverRegistry; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataFormControl"/> class.
        /// </summary>
        public DataFormControl()
        {
            InitializeComponent();

            Loaded += DataFormControl_Loaded;

            CommandBindings.Add(new CommandBinding(TriggerValidationCommand, TriggerValidationCommand_Execute));

            FormFiller = new FormFillerImpl(this);
            _xmlLinkResolverRegistry = new XmlLinkResolverRegistry();
        }

        #endregion

        #region Methods

        private void DataFormControl_Loaded(object sender, RoutedEventArgs e)
        {
            Validate();
        }

        private void TriggerValidationCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            Validate();
        }

        private void ApplyLayout(string layout)
        {
            this.LayoutRoot.Children.Clear();
            this.LayoutRoot.ColumnDefinitions.Clear();
            this.LayoutRoot.RowDefinitions.Clear();

            if (!string.IsNullOrWhiteSpace(layout))
            {
                XElement root = XmlLinkResolvingUtilities.PreprocessAndParseLayout(layout, _xmlLinkResolverRegistry);
                Build(root);
            }

            Validate();
        }

        private void Build(XElement root)
        {
            FormBuilder builder = new FormBuilder(this, this.LayoutRoot);

            foreach (UIElement item in builder.Build(root))
            {
                this.LayoutRoot.Children.Add(item);
            }
        }

        /// <summary>
        /// Forces a validation pass to happen and returns the result.
        /// </summary>
        /// <returns>true, if validation was successful.
        /// -or- false, if validation yielded at least one error, thus failing validation.</returns>
        public bool Validate()
        {
            if (this.DataFormObject == null)
            {
                this.ValidationResult.SetItems(null);
                IsValidationSuccess = true;
                return false;
            }

            var result = DataFormObject.Validate().ToList();
            this.ValidationResult.SetItems(result);

            bool success = !result.Any();
            IsValidationSuccess = success;
            return success;
        }

        private void TryWireupDataFormObjectPropertyChanged(DataObjectBase obj, bool value)
        {
            if (obj != null)
            {
                if (value)
                {
                    obj.PropertyChanged += DataFormObject_PropertyChanged;
                }
                else
                {
                    obj.PropertyChanged -= DataFormObject_PropertyChanged;
                }
            }
        }

        private void DataFormObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ValidateOnPropertyChange)
            {
                Validate();
            }
        }

        #endregion
    }
}
