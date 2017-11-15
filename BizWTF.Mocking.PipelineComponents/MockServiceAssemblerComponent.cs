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

using NWT.Testing.Library.Entities;



namespace NWT.Mocking.PipelineComponents
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_Any)]
    [ComponentCategory(CategoryTypes.CATID_Streamer)]
    [Guid("E142C2EE-DF99-4A1B-990D-59FAB8A7F449")]
    public class MockServiceAssemblerComponent : IBaseComponent, Microsoft.BizTalk.Component.Interop.IComponent, IComponentUI, IProbeMessage
    {
        System.Collections.Queue qOutputMsgs = new System.Collections.Queue();

        static ResourceManager resManager = new ResourceManager("NWT.Mocking.PipelineComponents.Resources.MockServiceAssemblerComponent", Assembly.GetExecutingAssembly());


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
        public IBaseMessage Execute(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            XmlDocument sourceDoc = new XmlDocument();
            pInMsg.BodyPart.Data.Position = 0;
            sourceDoc.Load(pInMsg.BodyPart.Data);


            XmlDocument outputDoc = Tools.ApplyXSLTransform(sourceDoc, TransformResources.MAP_ToMockService);

            byte[] arrByte = System.Text.Encoding.UTF8.GetBytes(outputDoc.OuterXml.Replace("utf-16", "utf-8"));
            MemoryStream tempStream = new MemoryStream(arrByte);
            tempStream.Seek(0, SeekOrigin.Begin);


            IBaseMessage outMsg = pContext.GetMessageFactory().CreateMessage();
            IBaseMessagePart bodyMsgPart = pContext.GetMessageFactory().CreateMessagePart();
            bodyMsgPart.Data = tempStream;
            bodyMsgPart.Charset = "utf-8";
            outMsg.AddPart("body", bodyMsgPart, true);
            
            outMsg.Context = pInMsg.Context;
            outMsg.Context.Promote(BTSProperties.messageType.Name.Name, BTSProperties.messageType.Name.Namespace, "http://NWT.Mocking.Services#SubmitMessage");
            
            
            pContext.ResourceTracker.AddResource(tempStream);
            return outMsg;
        }   

        internal byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
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
            //if ((string)pInMsg.Context.Read(BTSProperties.messageType.Name.Name, BTSProperties.messageType.Name.Namespace) == "http://NWT.Mocking.Schemas.Submission#MultipartMessage")
            //    return true;
            //else
            //    return false;

            return true;
        }
        #endregion


        

        
    }
}
