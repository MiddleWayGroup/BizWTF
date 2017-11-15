using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Microsoft.XLANGs.BaseTypes;

namespace BizWTF.Core.Entities
{
    public struct BTSProperties
    {
        public static BTS.InterchangeID interchangeId = new BTS.InterchangeID();
        public static XMLNORM.DocumentSpecName documentSpecName = new XMLNORM.DocumentSpecName();
        public static FILE.ReceivedFileName receivedFileName = new FILE.ReceivedFileName();
        public static BTS.MessageType messageType = new BTS.MessageType();
        public static BTS.ReceivePortID receivePortID = new BTS.ReceivePortID();
        public static BTS.ReceivePortName receivePortName = new BTS.ReceivePortName();
        public static BTS.SchemaStrongName schemaStrongName = new BTS.SchemaStrongName();
        public static BTS.SuspendMessageOnRoutingFailure suspendOnRoutingFailure = new BTS.SuspendMessageOnRoutingFailure();

        //public static EmulateXMLDisassembler emulateXMLDisassembler = new EmulateXMLDisassembler();
        public static ExecutePipeline executePipeline = new ExecutePipeline();
        //public static ExecutePipelineType executePipelineType = new ExecutePipelineType();
        public static TestID testID = new TestID();
    }

    //[Serializable]
    //[PropertyType("EmulateXMLDisassembler", "https://BizWTF.Mocking.Schemas.ProcessingProperties", "boolean", "System.Boolean")]
    //[IsSensitiveProperty(false)]
    //public class EmulateXMLDisassembler : MessageContextPropertyBase
    //{
    //    public EmulateXMLDisassembler() { }

    //    public override XmlQualifiedName Name
    //    {
    //        get
    //        {
    //            return new XmlQualifiedName("EmulateXMLDisassembler", "https://BizWTF.Mocking.Schemas.ProcessingProperties");
    //        }
    //    }
    //    public override Type Type
    //    {
    //        get { return typeof(bool); }
    //    }
    //}

    [Serializable]
    [PropertyType("ExecutePipeline", "https://BizWTF.Mocking.Schemas.ProcessingProperties", "string", "System.String")]
    [IsSensitiveProperty(false)]
    public class ExecutePipeline : MessageContextPropertyBase
    {
        public ExecutePipeline() { }

        public override XmlQualifiedName Name
        {
            get
            {
                return new XmlQualifiedName("ExecutePipeline", "https://BizWTF.Mocking.Schemas.ProcessingProperties");
            }
        }
        public override Type Type
        {
            get { return typeof(string); }
        }
    }

    //[Serializable]
    //[PropertyType("ExecutePipelineType", "https://BizWTF.Mocking.Schemas.ProcessingProperties", "string", "System.String")]
    //[IsSensitiveProperty(false)]
    //public class ExecutePipelineType : MessageContextPropertyBase
    //{
    //    public ExecutePipelineType() { }

    //    public override XmlQualifiedName Name
    //    {
    //        get
    //        {
    //            return new XmlQualifiedName("ExecutePipelineType", "https://BizWTF.Mocking.Schemas.ProcessingProperties");
    //        }
    //    }
    //    public override Type Type
    //    {
    //        get { return typeof(string); }
    //    }
    //}

    [Serializable]
    [PropertyType("TestID", "https://BizWTF.Mocking.Schemas.ProcessingProperties", "string", "System.String")]
    [IsSensitiveProperty(false)]
    public class TestID : MessageContextPropertyBase
    {
        public TestID() { }

        public override XmlQualifiedName Name
        {
            get
            {
                return new XmlQualifiedName("TestID", "https://BizWTF.Mocking.Schemas.ProcessingProperties");
            }
        }
        public override Type Type
        {
            get { return typeof(string); }
        }
    }
}
