using System;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Component;
//using Microsoft.BizTalk.Messaging;
using Microsoft.XLANGs.BaseTypes;
using Microsoft.XLANGs.RuntimeTypes;
using System.Drawing;
using System.Runtime.InteropServices;

using System.Resources;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Diagnostics;


using System.Globalization;
using DocumentSpec = Microsoft.BizTalk.Component.Interop.DocumentSpec;
//    using PipelineContext = Microsoft.Practices.ESB.ExceptionResolving.PipelineComponents.PipelineContext;
//    using PStage = Microsoft.Test.BizTalk.PipelineObjects.Stage;
//using Microsoft.Practices.ESB.ExceptionResolving.Pipelines.Errors;
//using Microsoft.Practices.ESB.ExceptionResolving.PipelineComponents;


using NWT.Testing.Library.Entities;



namespace NWT.Mocking.PipelineComponents
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_Decoder)]
    [ComponentCategory(CategoryTypes.CATID_Streamer)]
    [Guid("606F9EC2-3C25-4958-B817-6CF30C7B5A0C")]
    public class MockServiceDisassemblerComponent : IBaseComponent, Microsoft.BizTalk.Component.Interop.IComponent, IComponentUI, IProbeMessage
    {
        private IBaseMessage _inputmessage;
        private IBaseMessage _outputmessage;
        System.Collections.Queue qOutputMsgs = new System.Collections.Queue();

        static ResourceManager resManager = new ResourceManager("NWT.Mocking.PipelineComponents.Resources.MockServiceDisassemblerComponent", Assembly.GetExecutingAssembly());


        #region IBaseComponent
        public string Description
        {
            get { return resManager.GetString("Description"); }
        }

        public string Name
        {
            get { return resManager.GetString("Name"); }
        }

        public string Version
        {
            get { return resManager.GetString("Version"); }
        }
        #endregion



        public IBaseMessage Execute(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            XmlDocument sourceDoc = new XmlDocument();
            pInMsg.BodyPart.Data.Position = 0;
            sourceDoc.Load(pInMsg.BodyPart.Data);


            XmlDocument outputDoc = Tools.ApplyXSLTransform(sourceDoc, TransformResources.MAP_FromMockService);

            byte[] arrByte = System.Text.Encoding.UTF8.GetBytes(outputDoc.OuterXml.Replace("utf-16", "utf-8"));
            MemoryStream tempStream = new MemoryStream(arrByte);
            tempStream.Seek(0, SeekOrigin.Begin);


            IBaseMessage outMsg = pContext.GetMessageFactory().CreateMessage();
            IBaseMessagePart bodyMsgPart = pContext.GetMessageFactory().CreateMessagePart();
            bodyMsgPart.Data = tempStream;
            bodyMsgPart.Charset = "utf-8";
            outMsg.AddPart("body", bodyMsgPart, true);

            outMsg.Context = pInMsg.Context;
            outMsg.Context.Promote(BTSProperties.messageType.Name.Name, BTSProperties.messageType.Name.Namespace, "http://NWT.Mocking.Schemas.Submission#MultipartMessage");


            pContext.ResourceTracker.AddResource(tempStream);
            return outMsg;
        }

        internal byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }



        #region IComponentUI
        IntPtr IComponentUI.Icon
        {
            get { return ((Bitmap)resManager.GetObject("Icon")).GetHicon(); }
        }

        IEnumerator IComponentUI.Validate(object projectSystem)
        {
            return null;
        }
        #endregion

        
        #region IProbeMessage
        bool IProbeMessage.Probe(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            return true;
        }
        #endregion


        private Stream GetPartStream(XmlElement xmlElement)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlElement.OuterXml);

            XmlNode elemsub = xmlElement.FirstChild;

            Stream partstm = new MemoryStream();
            if ((elemsub != null) && (xmlElement.Name.ToLower() == "bin"))
            {
                Encoding encoding = Encoding.UTF8;
                byte[] bin = Convert.FromBase64String(elemsub.InnerText);
                partstm.Write(bin, 0, bin.Length);
            }
            else if ((elemsub != null) && (xmlElement.Name.ToLower() == "text"))
            {
                Encoding encoding = Encoding.UTF8;
                byte[] text = encoding.GetBytes(elemsub.InnerText);
                partstm.Write(text, 0, text.Length);
            }
            else if ((elemsub != null) && (xmlElement.Name.ToLower() == "edifact"))
            {
                Encoding encoding = Encoding.UTF8;
                byte[] text = encoding.GetBytes(elemsub.InnerText);
                partstm.Write(text, 0, text.Length);
            }
            else
            {
                doc.Save(partstm);
            }
            partstm.Position = 0;
            return partstm;
        }

        
    }
}
