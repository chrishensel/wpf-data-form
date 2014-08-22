using System.ComponentModel;
using System.Windows.Media;

namespace Wpf.DataForm.Library.DataForm
{
    /// <summary>
    /// Controls some of the styling of the <see cref="DataFormControl"/> and its elements.
    /// </summary>
    public class DataFormControlStyle : INotifyPropertyChanged
    {
        #region Fields

        private Brush _requiredBrush;

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets the brush that is displayed if the control is marked as "required".
        /// </summary>
        public Brush RequiredBrush
        {
            get { return _requiredBrush; }
            set
            {
                if (value != null && value != _requiredBrush)
                {
                    _requiredBrush = value;
                    OnPropertyChanged("RequiredBrush");
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataFormControlStyle"/> class.
        /// </summary>
        public DataFormControlStyle()
        {
            RequiredBrush = System.Windows.SystemColors.InfoBrush;
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var copy = PropertyChanged;
            if (copy != null)
            {
                copy(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
