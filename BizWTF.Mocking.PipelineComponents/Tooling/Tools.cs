using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;

using System.Xml;
using System.Xml.Xsl;

namespace BizWTF.Mocking.PipelineComponents
{
    public struct TransformResources
    {
        public const string MAP_FromMockService = "BizWTF.Mocking.PipelineComponents.Resources.map_MockServiceResponse_to_Multipart.xsl";
        public const string MAP_ToMockService = "BizWTF.Mocking.PipelineComponents.Resources.map_MultiPart_to_MockServiceSubmission.xsl";
    }

    public class Tools
    {
        /// <summary>
        /// Applies an XSL transform to an XML doc.
        /// </summary>
        /// <param name="originalDoc">Original document to transform</param>
        /// <param name="xslResourceName">Resource name (XSLT file)</param>
        /// <returns></returns>
        public static XmlDocument ApplyXSLTransform(XmlDocument originalDoc, string xslResourceName)
        {
            XmlDocument outputDoc = new XmlDocument(); ;

            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(XmlReader.Create(GetResource(xslResourceName)),
                        new XsltSettings(true, true),
                        new XmlUrlResolver());


            StringWriter originalSw = new StringWriter();
            originalDoc.Save(originalSw);
            XmlTextReader originalReader = new XmlTextReader(new StringReader(originalSw.ToString()));

            // Paramètres du XSLT
            XsltArgumentList args = new XsltArgumentList();
            StringWriter filteredDoc = new StringWriter();
            xslt.Transform(originalReader, args, new XmlTextWriter(filteredDoc));

            outputDoc.LoadXml(filteredDoc.ToString());
            return outputDoc;
        }

        /// <summary>
        /// Extrait d'une assembly une ressource embarquée
        /// </summary>
        /// <param name="assembly">Assembly source</param>
        /// <param name="resourceName">Nom de la ressource</param>
        /// <returns></returns>
        public static Stream GetResource(string resourceName)
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        }
    }
}
