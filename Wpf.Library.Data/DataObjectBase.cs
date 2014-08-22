using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Wpf.Library.Data
{
    /// <summary>
    /// Represents the base class for any type that provides comfortable change notification and other feats that are helpful when working with WPF.
    /// </summary>
    [Serializable()]
    public abstract class DataObjectBase : IDataObjectBase
    {
        #region Fields

        private bool _isModified;
        private Dictionary<string, object> _values;

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether or not the properties in this object are modified.
        /// </summary>
        public bool IsModified
        {
            get
            {
                // If this instance is modified, return true.
                if (_isModified)
                {
                    return true;
                }
                else
                {
                    // Otherwise walk the values dictionary and check if any is modified.
                    foreach (IDataObjectBase child in _values.Values.OfType<IDataObjectBase>())
                    {
                        if (child.IsModified)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            private set { _isModified = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataObjectBase"/> class.
        /// </summary>
        public DataObjectBase()
        {
            _values = new Dictionary<string, object>(16);
        }

        #endregion

        #region Methods

        private void SetNewValue(string propertyName, object value)
        {
            if (_values.ContainsKey(propertyName))
            {
                object oldValue = _values[propertyName];
                if (object.Equals(value, oldValue))
                {
                    return;
                }
            }

            _values[propertyName] = value;

            IsModified = true;
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Returns the value of a specific property.
        /// </summary>
        /// <typeparam name="T">The type of the value to expect.</typeparam>
        /// <param name="name">The name of the property.</param>
        /// <returns>The value of the property.</returns>
        protected T GetValue<T>(string name)
        {
            object result = null;
            if (_values.TryGetValue(name, out result))
            {
                return (T)result;
            }

            return default(T);
        }

        /// <summary>
        /// Returns the value of a specific property.
        /// </summary>
        /// <typeparam name="T">The type of the value to set.</typeparam>
        /// <param name="expr">The expression representing the property to set a new value.</param>
        /// <returns>The value of the property.</returns>
        protected T GetValue<T>(Expression<Func<T>> expr)
        {
            if (expr.Body.NodeType != ExpressionType.MemberAccess)
            {
                throw new InvalidOperationException();
            }

            MemberExpression me = (MemberExpression)expr.Body;

            return this.GetValue<T>(me.Member.Name);
        }

        /// <summary>
        /// Marks the object (and its child objects) as being unmodified.
        /// </summary>
        public void MarkUnmodified()
        {
            IsModified = false;

            // Mark all children unmodified as well.
            foreach (DataObjectBase child in _values.Values.OfType<DataObjectBase>())
            {
                child.MarkUnmodified();
            }
        }

        /// <summary>
        /// Sets the value of the given property.
        /// </summary>
        /// <param name="name">The name of the property to set a new value.</param>
        /// <param name="value">The new value to set.</param>
        protected void SetValue(string name, object value)
        {
            SetNewValue(name, value);
        }

        /// <summary>
        /// Sets the value of the given property.
        /// </summary>
        /// <typeparam name="T">The type of the value to set.</typeparam>
        /// <param name="expr">The expression representing the property to set a new value.</param>
        /// <param name="value">The new value to set.</param>
        protected void SetValue<T>(Expression<Func<T>> expr, T value)
        {
            if (expr.Body.NodeType != ExpressionType.MemberAccess)
            {
                throw new InvalidOperationException();
            }

            MemberExpression me = (MemberExpression)expr.Body;

            this.SetValue(me.Member.Name, value);
        }

        /// <summary>
        /// When overridden in a derived class, determines whether a property should be validated, or skipped.
        /// </summary>
        /// <param name="property">The property to determine evaluation should take place, or be skipped.</param>
        /// <returns>true, if validation should be performed.
        /// -or- false, if validation should be skipped for this property.</returns>
        protected virtual bool ShouldValidateProperty(PropertyInfo property)
        {
            return true;
        }

        /// <summary>
        /// Performs validation for one specific property.
        /// </summary>
        /// <param name="property">The property to perform validation for.</param>
        /// <param name="value">The value of the property.</param>
        /// <returns>A list with all validation results (in case they failed).</returns>
        protected virtual IEnumerable<ValidationResult> ValidateProperty(PropertyInfo property, object value)
        {
            ValidationContext context = new ValidationContext(this, null, null);
            context.MemberName = property.Name;

            ICollection<ValidationResult> results = new Collection<ValidationResult>();
            if (!Validator.TryValidateProperty(value, context, results))
            {
                foreach (var item in results)
                {
                    yield return item;
                }
            }

            yield break;
        }

        /// <summary>
        /// Performs validation on this object and returns the result.
        /// </summary>
        /// <returns>An enumerable containing the result of validation.</returns>
        public IEnumerable<ValidationResult> Validate()
        {
            List<ValidationResult> result = new List<ValidationResult>();
            foreach (PropertyInfo property in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!ShouldValidateProperty(property))
                {
                    continue;
                }

                object value = property.GetValue(this, null);
                result.AddRange(ValidateProperty(property, value));
            }
            return result;
        }

        #endregion

        #region INotifyPropertyChanged Member

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged-event.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        protected virtual void OnPropertyChanged(string propertyName)
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
