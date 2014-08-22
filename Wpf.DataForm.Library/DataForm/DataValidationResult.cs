using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Wpf.DataForm.Library.DataForm
{
    /// <summary>
    /// Represents a container for validation result items.
    /// </summary>
    public class DataValidationResult : INotifyPropertyChanged
    {
        #region Properties

        /// <summary>
        /// Gets a list of validation result items.
        /// </summary>
        public ObservableCollection<ValidationResult> Items { get; private set; }

        /// <summary>
        /// Gets whether or not this instance has any items.
        /// </summary>
        public bool HasItems
        {
            get { return Items.Count > 0; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataValidationResult"/> class.
        /// </summary>
        public DataValidationResult()
        {
            Items = new ObservableCollection<ValidationResult>();
        }

        #endregion

        #region Methods

        internal void SetItems(IEnumerable<ValidationResult> source)
        {
            Items.Clear();
            if (source != null)
            {
                foreach (ValidationResult item in source)
                {
                    Items.Add(item);
                }
            }

            OnPropertyChanged("HasItems");
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
