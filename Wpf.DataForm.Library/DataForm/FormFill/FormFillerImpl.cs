using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Wpf.DataForm.Library.DataForm.FormFill
{
    class FormFillerImpl : IFormFiller
    {
        #region Fields

        private readonly IDataFormControlService _service;

        #endregion

        #region Constructors

        internal FormFillerImpl(IDataFormControlService service)
        {
            _service = service;
        }

        #endregion

        #region IFormFiller Members

        void IFormFiller.SetData(IDictionary<string, object> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (_service.DataFormObject == null)
            {
                return;
            }

            foreach (PropertyInfo prop in EnumerateValidProperties(_service.DataFormObject.GetType()))
            {
                object valueToSet = null;
                if (data.TryGetValue(prop.Name, out valueToSet))
                {
                    try
                    {
                        prop.SetValue(_service.DataFormObject, valueToSet, null);
                    }
                    catch (Exception)
                    {
                        Trace.WriteLine(string.Format(Properties.Resources.SetDataSingleError, valueToSet, prop.Name));
                    }
                }
            }
        }

        IDictionary<string, object> IFormFiller.GetData()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (_service.DataFormObject != null)
            {
                foreach (PropertyInfo prop in EnumerateValidProperties(_service.DataFormObject.GetType()))
                {
                    result[prop.Name] = prop.GetValue(_service.DataFormObject, null);
                }
            }

            return result;
        }

        /// <summary>
        /// Enumerates all properties of the given type and returns only those that are public and both readable and writable.
        /// </summary>
        /// <param name="type">The type to find all valid properties.</param>
        /// <returns></returns>
        private static IEnumerable<PropertyInfo> EnumerateValidProperties(Type type)
        {
            foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (prop.CanRead && prop.CanWrite && !prop.PropertyType.IsArray)
                {
                    yield return prop;
                }
            }
        }

        #endregion
    }
}
