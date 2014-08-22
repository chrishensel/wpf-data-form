using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Linq;

namespace Wpf.DataForm.Library.DataForm.Builder
{
    /// <summary>
    /// Represents a class that contains known attribute values from an XML element.
    /// </summary>
    sealed class KnownAttributeSet
    {
        #region Constants

        private static readonly string[] BindingPathAttributeNames = { "b", "bind" };
        private const string WidthAttributeName = "width";
        private const string HeightAttributeName = "height";
        private const string ToolTipTextAttributeName = "tip";

        #endregion

        #region Properties

        /// <summary>
        /// Gets the specified binding path to use.
        /// </summary>
        public string BindingPath { get; private set; }
        /// <summary>
        /// Gets the explicitly specified width of the underlying visual representation.
        /// </summary>
        public Nullable<double> Width { get; private set; }
        /// <summary>
        /// Gets the explicitly specified height of the underlying visual representation.
        /// </summary>
        public Nullable<double> Height { get; private set; }
        /// <summary>
        /// Gets the specified text to show as the tool tip.
        /// </summary>
        public string ToolTipText { get; private set; }

        /// <summary>
        /// Gets whether or not the <see cref="BindingPath"/> has been set.
        /// </summary>
        public bool HasBindingPath
        {
            get { return !string.IsNullOrWhiteSpace(BindingPath); }
        }

        #endregion

        #region Constructors

        private KnownAttributeSet()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnownAttributeSet"/> class.
        /// </summary>
        /// <param name="node">The XML element to read the attributes from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> was null.</exception>
        public KnownAttributeSet(XElement node)
            : this()
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            ReadAttributesIntoProperties(node);
        }

        #endregion

        #region Methods

        private void ReadAttributesIntoProperties(XElement node)
        {
            foreach (string bindAttributeName in BindingPathAttributeNames)
            {
                string value = GetAttributeValue(node, bindAttributeName);
                if (value != null)
                {
                    BindingPath = value;
                }
            }
            Width = GetNullableValueFrom<double>(node, WidthAttributeName);
            Height = GetNullableValueFrom<double>(node, HeightAttributeName);
            ToolTipText = GetAttributeValue(node, ToolTipTextAttributeName);
        }

        private static Nullable<T> GetNullableValueFrom<T>(XElement element, string name) where T : struct
        {
            string attValue = GetAttributeValue(element, name);
            if (attValue != null)
            {
                try
                {
                    T value = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertTo(attValue, typeof(T));
                    return new Nullable<T>(value);
                }
                catch (Exception)
                {
                    Trace.WriteLine(string.Format(Properties.Resources.ReadKnownAttributeError, name));
                }
            }
            return new Nullable<T>();
        }

        private static string GetAttributeValue(XElement element, string name)
        {
            XAttribute attr = element.Attribute(name);
            return (attr != null) ? attr.Value : null;
        }

        #endregion
    }
}
