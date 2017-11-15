using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;


using BizWTF.Mocking.Services;
using BizWTF.Mocking.Services.Settings;

using BizWTF.Core;
using BizWTF.Core.Entities;
using BizWTF.Core.Entities.Mocking;
using BizWTF.Core.Entities.ProcessValidation;
using BizWTF.Core.Tests;
using BizWTF.Core.Tests.ActionSteps;
using BizWTF.Core.Tests.ActionSteps.Messaging;
using BizWTF.Core.Tests.GetSteps;
using BizWTF.Core.Tests.TestSteps;



namespace BizWTF.Testing.Sample
{
    [TestClass]
    public class SerializationTest
    {
        [TestMethod]
        public void TOOLS_SerializeMultipartMessage()
        {
            MultipartMessageDefinition msg = new MultipartMessageDefinition();
            msg.Description = "Test description";

            msg.PropertyBag = new List<ContextProperty>{ 
                                            new ContextProperty{Name="Prop1", Namespace="http://blabla", Promoted=true, Value="TestPropValue1"},
                                            new ContextProperty{Name="Prop2", Namespace="http://blabla", Promoted=true, Value="TestPropValue2"},
                                            new ContextProperty{Name="Prop3", Namespace="http://blabla", Promoted=true, Value="TestPropValue3"}
                                            }.ToArray();

            XmlDocument part = new XmlDocument();
            part.LoadXml("<ns0:Request xmlns:ns0=\"http://BizWTF.Sample.Processes.Schemas.SysASchemas\"><Field1>1</Field1><Field2>Top</Field2></ns0:Request>");

            msg.Parts = new List<Part>{
                                new Part{IsBodyPart=true, PartName="body", PartNumber=1, RawData="Some raw data" } //RawData="Some raw data" //Data=part.DocumentElement
                                ,new Part{IsBodyPart=true, PartName="body", PartNumber=1, Data=part.DocumentElement}
                                }.ToArray();

            XmlDocument doc = BizWTF.Core.Entities.Mocking.MultipartMessageSerializer.Serialize(msg);

            doc.Save(@"C:\Temp\GeneratedMultipartMessage.xml");

            msg = BizWTF.Core.Entities.Mocking.MultipartMessageSerializer.Deserialize(doc.DocumentElement);

            Assert.IsNotNull(msg.Parts);

        }

        [TestMethod]
        public void TOOLS_DeserializeMultipartMessage()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\Projets\BizWTF\BizWTF.Testing.Sample\Messages\OK_SystemARequest.xml"); //doc.Load(@"C:\Temp\GeneratedMultipartMessage.xml");

            MultipartMessageDefinition msg = BizWTF.Core.Entities.Mocking.MultipartMessageSerializer.Deserialize(doc.DocumentElement);

            Assert.IsNotNull(msg.Parts);

        }


    }
}
