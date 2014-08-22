using System.Xml.Linq;

namespace Wpf.DataForm.Library.DataForm.XmlLinkResolver
{
    /// <summary>
    /// Defines members for a type that can be used to resolve links within the layout XML file.
    /// See documentation for further information.
    /// </summary>
    /// <remarks>This technique is similar to a preprocessor, which is run before the XML file is actually parsed.
    /// The logic will search for any occurrence of the &lt;link id="link" /&gt; element, and replaces the node
    /// with the contents of the linked XML file. If the link does not resolve, the element is removed completely.</remarks>
    interface IXmlLinkResolver
    {
        /// <summary>
        /// Tries to resolve the linked XML file.
        /// </summary>
        /// <param name="linkId"></param>
        /// <param name="content"></param>
        /// <returns>A boolean value indicating whether or not the XML file could be resolved
        /// from the registered path.
        /// Returns true if a corresponding link was registered and the linked file existed.
        /// -or- false if the link was not registered.</returns>
        bool TryResolveLinkedXmlContents(string linkId, out XElement content);
    }
}
