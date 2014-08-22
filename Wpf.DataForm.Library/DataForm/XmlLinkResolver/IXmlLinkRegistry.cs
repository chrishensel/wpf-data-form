
namespace Wpf.DataForm.Library.DataForm.XmlLinkResolver
{
    /// <summary>
    /// Defines members for a type that can be used to register links for link resolving within layout XML files.
    /// See documentation for further information.
    /// </summary>
    /// <remarks>This technique is similar to a preprocessor, which is run before the XML file is actually parsed.
    /// The logic will search for any occurrence of the &lt;link id="link" /&gt; element, and replaces the node
    /// with the contents of the linked XML file. If the link does not resolve, the element is removed completely.</remarks>
    public interface IXmlLinkRegistry
    {
        /// <summary>
        /// Registers an XML file with a link identifier.
        /// </summary>
        /// <param name="linkId">The link identifier. This is what is placed within the &lt;link id="<paramref name="linkId"/>" /&gt;-element.</param>
        /// <param name="xmlFilePath">The absolute path to the XML-file that shall be linked to.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="linkId"/> was null.</exception>
        /// <exception cref="System.IO.FileNotFoundException"><paramref name="xmlFilePath"/> pointed to a file that did not exist.</exception>
        void RegisterXmlLinkFile(string linkId, string xmlFilePath);
    }
}
