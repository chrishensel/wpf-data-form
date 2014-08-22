using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Wpf.DataForm.Library.DataForm.Builder
{
    /// <summary>
    /// Provides parameters that are used to construct a specific element.
    /// </summary>
    [DebuggerDisplay("Node = {Node.Name}")]
    sealed class ConstructionParameters
    {
        #region Properties

        /// <summary>
        /// Gets the XML element that represents the layout of this particular part.
        /// </summary>
        public XElement Node { get; private set; }
        /// <summary>
        /// Gets the known attributes for the current <see cref="Node"/>.
        /// </summary>
        public KnownAttributeSet KnownAttributes { get; private set; }
        /// <summary>
        /// Gets/sets the parental grid, if any.
        /// </summary>
        public Grid Parent { get; set; }
        /// <summary>
        /// Gets/sets the <see cref="PropertyInfo"/> instance representing the property that is about being bound.
        /// </summary>
        public PropertyInfo BindingSourceProperty { get; set; }
        /// <summary>
        /// Gets/sets whether or not the label for this element shall be shown.
        /// You can set this value to manually override label creation within the parent container.
        /// </summary>
        public bool ShowLabel { get; set; }

        #endregion

        #region Constructors

        private ConstructionParameters()
        {
            ShowLabel = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructionParameters"/> class.
        /// </summary>
        /// <param name="node">The XML element that represents the layout of this particular part.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> was null.</exception>
        public ConstructionParameters(XElement node)
            : this()
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            Node = node;
            KnownAttributes = new KnownAttributeSet(Node);
        }

        #endregion
    }
}
