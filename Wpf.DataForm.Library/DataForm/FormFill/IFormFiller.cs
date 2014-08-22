using System.Collections.Generic;

namespace Wpf.DataForm.Library.DataForm.FormFill
{
    /// <summary>
    /// Defines mechanisms for a type that is able to fill out a form.
    /// </summary>
    public interface IFormFiller
    {
        /// <summary>
        /// Replaces all data currently present in the data form with the one from the given dictionary.
        /// </summary>
        /// <param name="data">A dictionary containing the names and values of the properties to set.</param>
        void SetData(IDictionary<string, object> data);
        /// <summary>
        /// Queries and returns the data currently present in the data form.
        /// </summary>
        /// <returns>A dictionary containing the data currently present in the data form.
        /// The keys represent the property names, while the values represent the corresponding value data.</returns>
        IDictionary<string, object> GetData();
    }
}
