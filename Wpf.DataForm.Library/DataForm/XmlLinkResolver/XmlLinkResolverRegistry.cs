using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Wpf.DataForm.Library.DataForm.XmlLinkResolver
{
    class XmlLinkResolverRegistry : IXmlLinkResolver, IXmlLinkRegistry
    {
        #region Fields

        private Dictionary<string, string> _links;

        #endregion

        #region Constructors

        internal XmlLinkResolverRegistry()
        {
            _links = new Dictionary<string, string>();
        }

        #endregion

        #region IXmlLinkResolver Members

        void IXmlLinkRegistry.RegisterXmlLinkFile(string linkId, string xmlFilePath)
        {
            if (linkId == null)
            {
                throw new ArgumentNullException("linkId");
            }
            if (!File.Exists(xmlFilePath))
            {
                throw new FileNotFoundException();
            }

            _links[linkId] = xmlFilePath;
        }

        bool IXmlLinkResolver.TryResolveLinkedXmlContents(string linkId, out XElement content)
        {
            content = null;
            if (!_links.ContainsKey(linkId))
            {
                return false;
            }

            string filePath = _links[linkId];
            try
            {
                XDocument doc = XDocument.Load(filePath);
                content = doc.Root;
                return true;
            }
            catch (Exception)
            {
                Tracing.WriteError(Properties.Resources.XmlLinkFileResolveError, filePath);
            }

            return false;
        }

        #endregion
    }
}
