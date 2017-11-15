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

using BizWTF.Core.Entities;
using BizWTF.Core.Entities.Mocking;


namespace BizWTF.Mocking.PipelineComponents
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_AssemblingSerializer)]
    [ComponentCategory(CategoryTypes.CATID_Streamer)]
    [Guid("DD217DD9-4EBC-4CC1-89D8-F9D6A6C84BEB")]
    public class MockMessageAssemblerComponent : IBaseComponent, IAssemblerComponent, IComponentUI, IProbeMessage
    {
        System.Collections.Queue qOutputMsgs = new System.Collections.Queue();

        static ResourceManager resManager = new ResourceManager("BizWTF.Mocking.PipelineComponents.Resources.MockMessageAssemblerComponent", Assembly.GetExecutingAssembly());


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



        #region IAssemblerComponent
        void IAssemblerComponent.AddDocument(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            if (this.GetFault(pInMsg) != null)
                qOutputMsgs.Enqueue(pInMsg);
            else
            {
                MultipartMessageDefinition tempMessage = MultipartMessageManager.GenerateFromMessage(pInMsg);

                StringWriter sw = new StringWriter();
                XmlSerializer ser = new XmlSerializer(typeof(MultipartMessageDefinition), Constants.SUBMISSION_NAMESPACE);
                ser.Serialize(sw, tempMessage);
                byte[] arrByte = System.Text.Encoding.UTF8.GetBytes(sw.ToString().Replace("utf-16", "utf-8")); //GetBytes(sw.ToString().Replace("utf-16", "utf-8")); //GetBytes(sw.ToString());
                MemoryStream tempStream = new MemoryStream(arrByte);
                tempStream.Seek(0, SeekOrigin.Begin);

                IBaseMessage outMsg = pContext.GetMessageFactory().CreateMessage();
                IBaseMessagePart outPart = pContext.GetMessageFactory().CreateMessagePart();
                outPart.Data = tempStream;
                outPart.Charset = "utf-8";
                outMsg.AddPart("ConstructedPart", outPart, true);
                //outMsg.BodyPart.Data = tempStream;

                outMsg.Context = pInMsg.Context;
                outMsg.Context.Promote(BTSProperties.messageType.Name.Name, BTSProperties.messageType.Name.Namespace, "http://BizWTF.Mocking.Schemas.Submission#MultipartMessage");


                qOutputMsgs.Enqueue(outMsg);
                pContext.ResourceTracker.AddResource(tempStream);
            }
        }

        internal byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private XmlDocument GetFault(IBaseMessage inMsg)
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                inMsg.BodyPart.Data.Position = 0;
                doc.Load(inMsg.BodyPart.Data);
                inMsg.BodyPart.Data.Position = 0;

                if (doc.DocumentElement.LocalName == "Fault"
                    || doc.SelectSingleNode("/*[local-name()='Enveloppe']/*[local-name()='Body']/*[local-name()='Fault']") != null)
                    return doc;
                else
                    return null;
            }
            catch
            {
                return null; //throw new ArgumentNullException("inMsg");
            }

        }

        IBaseMessage IAssemblerComponent.Assemble(IPipelineContext pContext)
        {
            if (qOutputMsgs.Count > 0)
            {
                IBaseMessage msg = (IBaseMessage)qOutputMsgs.Dequeue();
                return msg;
            }
            else
                return null;
        }
        #endregion


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





    }
}
