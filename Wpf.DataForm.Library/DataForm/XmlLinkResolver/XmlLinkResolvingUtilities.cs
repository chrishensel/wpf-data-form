using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Wpf.DataForm.Library.DataForm.XmlLinkResolver
{
    static class XmlLinkResolvingUtilities
    {
        #region Constants

        private const string LinkElementName = "link";
        private const string LinkIdAttributeName = "id";
        private const int MaxIterationCount = 16;

        #endregion

        #region Methods

        internal static XElement PreprocessAndParseLayout(string layout, IXmlLinkResolver resolver)
        {
            XDocument doc = XDocument.Parse(layout);

            bool success = false;
            for (int i = 0; i < MaxIterationCount; i++)
            {
                Tracing.WriteInfo(Properties.Resources.PreprocessorPass, i + 1, MaxIterationCount);

                if (!ProcessOnePass(resolver, doc))
                {
                    success = true;
                    break;
                }
            }

            if (!success && DetectAndStripLinkElements(doc))
            {
                Tracing.WriteError(Properties.Resources.PreprocessorInfiniteRecursionDetectedError);
            }

            Tracing.WriteInfo(Properties.Resources.PreprocessorFinished);

            return doc.Root;
        }

        private static bool ProcessOnePass(IXmlLinkResolver resolver, XDocument doc)
        {
            bool encounteredLink = false;
            foreach (XElement element in doc.Descendants(LinkElementName).ToList())
            {
                encounteredLink = true;

                try
                {
                    XAttribute linkIdA = element.Attribute(LinkIdAttributeName);
                    if (linkIdA == null)
                    {
                        element.Remove();
                        continue;
                    }

                    XElement linkContent = null;
                    if (!resolver.TryResolveLinkedXmlContents(linkIdA.Value, out linkContent))
                    {
                        element.Remove();
                        continue;
                    }

                    element.ReplaceWith(linkContent);
                }
                catch (Exception)
                {
                    element.Remove();
                }
            }
            return encounteredLink;
        }

        private static bool DetectAndStripLinkElements(XDocument doc)
        {
            IList<XElement> links = doc.Descendants(LinkElementName).ToList();
            if (!links.Any())
            {
                return false;
            }

            foreach (XElement element in links)
            {
                element.Remove();
            }

            return true;
        }

        #endregion
    }
}
