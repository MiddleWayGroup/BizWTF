using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BizWTF.Testing.Sample
{
    [TestClass]
    public class Mocking_Call2WayMock
    {
        [TestMethod]
        public void Call2WayMock_Nominal()
        {

            using (BizWTF.Core.Tests.ActionSteps.Messaging.Init2WayMockMessagingService twoWayTarget = new BizWTF.Core.Tests.ActionSteps.Messaging.Init2WayMockMessagingService
            {
                StepName = "Init 2-Way mock service",
                ServiceURI = "http://localhost:5100/BizWTF.SystemB"
            })
            {
                #region Mocking 2-way System B
                BizWTF.Mocking.Services.Settings.MessageResolutionSetting setting = new BizWTF.Mocking.Services.Settings.MessageResolutionSetting();
                setting.Description = "SystemB OK response";
                setting.ContextProps.Add(new BizWTF.Core.Entities.ProcessValidation.ControlProperty("http://schemas.microsoft.com/BizTalk/2003/system-properties",
                                                            "MessageType",
                                                            "http://BizWTF.Sample.Processes.Schemas.SysBSchemas#Request"));
                setting.PartSettings.Add(new Mocking.Services.Settings.PartSetting
                {
                    XPathProps = new System.Collections.Generic.List<Core.Entities.ProcessValidation.ControlField> {
                        new Core.Entities.ProcessValidation.ControlField("/*[local-name()='Request']/*[local-name()='Field1']", "1"),
                        new Core.Entities.ProcessValidation.ControlField("/*[local-name()='Request']/*[local-name()='Field2']", "Top")
                    }
                });

                // Add as many ControlProperty as needed
                setting.TargetMessage = BizWTF.Core.Entities.Mocking.MultipartMessageSerializer.RetrieveDocument(@"BizWTF.Testing.Sample.Messages.OK_SystemBResponse.xml",
                                                                                    System.Reflection.Assembly.GetExecutingAssembly());
                twoWayTarget.ResolutionSettings.Clear();
                twoWayTarget.ResolutionSettings.Add(setting);
                // Add as many resolution setting as needed



                /// OPTIONAL : if you need to fire some custom code when a message is received
                //MockServiceEventHub.ResetMessageReceivedEventHandler(); // Optional, if you need to flush all event handlers
                BizWTF.Mocking.Services.MockServiceEventHub.OnMessageReceived += (string uri, BizWTF.Core.Entities.MultipartMessageDefinition message) =>
                {
                    if (uri == twoWayTarget.ServiceURI)
                    {
                        // Do something
                    }
                };

                /// OPTIONAL : if you need to fire some custom code when a message is resolved
                //MockServiceEventHub.ResetMessageResolvedEventHandler(); // Optional, if you need to flush all event handlers
                BizWTF.Mocking.Services.MockServiceEventHub.OnMessageResolved += (string uri, BizWTF.Core.Entities.MultipartMessageDefinition message) =>
                {
                    if (uri == twoWayTarget.ServiceURI)
                    {
                        // Do something
                    }
                };

                /// Following code must complete (this is were we start the mock service)
                /// In case of any error, the user that runs the test may need som additional right to reserve the URL.
                /// If so, consider using the following command : netsh http add urlacl url=... user=...
                if (!twoWayTarget.ExecuteStep())
                {
                    Assert.Fail(twoWayTarget.ResultDescription);
                }
                #endregion

                BizWTF.Core.Tests.ActionSteps.Messaging.SubmitMockMessage1WayStep send1WayMsgStep = new BizWTF.Core.Tests.ActionSteps.Messaging.SubmitMockMessage1WayStep("Submit 1Way Message");
                send1WayMsgStep.DestURI = "http://localhost:5000/BizWTF.SystemA"; // NOTE : must match a receive location URI !!!
                send1WayMsgStep.SourceResource = @"BizWTF.Testing.Sample.Messages.OK_SystemARequest.xml";
                send1WayMsgStep.SourceResourceAssembly = System.Reflection.Assembly.GetExecutingAssembly().FullName; // In this case, the resource is embedded in the current assembly
                send1WayMsgStep.TestID = Guid.NewGuid().ToString(); // NOTE : you can provide a test ID, wich will be attached as a message property. This facilitates instance tracking.
                //send1WayMsgStep.TargetContextProperty = "send2WayMsgStep_Response";

                /// NOTE : the folowwing code actually sends the message. 
                /// Nevertheless, it is recommended to first initialize all the steps of the test before executing any of them.
                if (!send1WayMsgStep.ExecuteStep())
                {
                    Assert.Fail(send1WayMsgStep.ResultDescription);
                }

                
            }
        }


        [TestMethod]
        public void Call2WayMock_FunctionalError()
        {

            using (BizWTF.Core.Tests.ActionSteps.Messaging.Init2WayMockMessagingService twoWayTarget = new BizWTF.Core.Tests.ActionSteps.Messaging.Init2WayMockMessagingService
            {
                StepName = "Init 2-Way mock service",
                ServiceURI = "http://localhost:5100/BizWTF.SystemB"
            })
            {
                #region Mocking 2-way System B
                BizWTF.Mocking.Services.Settings.MessageResolutionSetting setting = new BizWTF.Mocking.Services.Settings.MessageResolutionSetting();
                setting.Description = "SystemB KO response";
                setting.ContextProps.Add(new BizWTF.Core.Entities.ProcessValidation.ControlProperty("http://schemas.microsoft.com/BizTalk/2003/system-properties",
                                                            "MessageType",
                                                            "http://BizWTF.Sample.Processes.Schemas.SysBSchemas#Request"));
                setting.PartSettings.Add(new Mocking.Services.Settings.PartSetting
                {
                    XPathProps = new System.Collections.Generic.List<Core.Entities.ProcessValidation.ControlField> {
                        new Core.Entities.ProcessValidation.ControlField("/*[local-name()='Request']/*[local-name()='Field1']", "2"),
                        new Core.Entities.ProcessValidation.ControlField("/*[local-name()='Request']/*[local-name()='Field2']", "Flop")
                    }
                });

                // Add as many ControlProperty as needed
                setting.TargetMessage = BizWTF.Core.Entities.Mocking.MultipartMessageSerializer.RetrieveDocument(@"BizWTF.Testing.Sample.Messages.ERROR_SystemBResponse.xml",
                                                                                    System.Reflection.Assembly.GetExecutingAssembly());
                twoWayTarget.ResolutionSettings.Clear();
                twoWayTarget.ResolutionSettings.Add(setting);
                // Add as many resolution setting as needed



                /// OPTIONAL : if you need to fire some custom code when a message is received
                //MockServiceEventHub.ResetMessageReceivedEventHandler(); // Optional, if you need to flush all event handlers
                BizWTF.Mocking.Services.MockServiceEventHub.OnMessageReceived += (string uri, BizWTF.Core.Entities.MultipartMessageDefinition message) =>
                {
                    if (uri == twoWayTarget.ServiceURI)
                    {
                        // Do something
                    }
                };

                /// OPTIONAL : if you need to fire some custom code when a message is resolved
                //MockServiceEventHub.ResetMessageResolvedEventHandler(); // Optional, if you need to flush all event handlers
                BizWTF.Mocking.Services.MockServiceEventHub.OnMessageResolved += (string uri, BizWTF.Core.Entities.MultipartMessageDefinition message) =>
                {
                    if (uri == twoWayTarget.ServiceURI)
                    {
                        // Do something
                    }
                };

                /// Following code must complete (this is were we start the mock service)
                /// In case of any error, the user that runs the test may need som additional right to reserve the URL.
                /// If so, consider using the following command : netsh http add urlacl url=... user=...
                if (!twoWayTarget.ExecuteStep())
                {
                    Assert.Fail(twoWayTarget.ResultDescription);
                }
                #endregion

                BizWTF.Core.Tests.ActionSteps.Messaging.SubmitMockMessage2WayStep send2WayMsgStep = new BizWTF.Core.Tests.ActionSteps.Messaging.SubmitMockMessage2WayStep("Submit 2Way Message");
                send2WayMsgStep.DestURI = "http://localhost:5000/BizWTF.SystemA"; // NOTE : must match a receive location URI !!!
                send2WayMsgStep.SourceResource = @"BizWTF.Testing.Sample.Messages.ERROR_SystemARequest.xml";
                send2WayMsgStep.SourceResourceAssembly = System.Reflection.Assembly.GetExecutingAssembly().FullName; // In this case, the resource is embedded in the current assembly
                send2WayMsgStep.TestID = Guid.NewGuid().ToString(); // NOTE : you can provide a test ID, wich will be attached as a message property. This facilitates instance tracking.
                send2WayMsgStep.TargetContextProperty = "send2WayMsgStep_Response";

                /// NOTE : the folowwing code actually sends the message. 
                /// Nevertheless, it is recommended to first initialize all the steps of the test before executing any of them.
                if (!send2WayMsgStep.ExecuteStep())
                {
                    Assert.Fail(send2WayMsgStep.ResultDescription);
                }

                /// Once the step has executed successfully, do whatever test you need to do.
                /// The following code : BTSTestContext.GetParam(send2WayMsgStep.TargetContextProperty)
                /// retrieves the response received.
                Assert.IsNotNull(BizWTF.Core.Entities.BTSTestContext.GetParam(send2WayMsgStep.TargetContextProperty));

                /// Add the rest of your test here
                // Do something
                BizWTF.Core.Entities.MultipartMessageDefinition response = (BizWTF.Core.Entities.MultipartMessageDefinition)BizWTF.Core.Entities.BTSTestContext.GetParam(send2WayMsgStep.TargetContextProperty);


                Assert.IsNotNull(response.Parts[0].Data.SelectSingleNode("/*[local-name()='Status']"));
                Assert.AreEqual(response.Parts[0].Data.SelectSingleNode("/*[local-name()='Status']").InnerText, "FunctionalError");

                Assert.IsNotNull(response.Parts[0].Data.SelectSingleNode("/*[local-name()='ReturnValue']"));
                Assert.AreEqual(response.Parts[0].Data.SelectSingleNode("/*[local-name()='ReturnValue']").InnerText, "ERR1");



                Assert.IsTrue(twoWayTarget.ReceivedMessages.Count > 0);



                BizWTF.Core.Tests.ActionSteps.ForceTrackedMessageCopyJobStep dtaStep = new Core.Tests.ActionSteps.ForceTrackedMessageCopyJobStep("Force tracking job");
                if (!dtaStep.ExecuteStep())
                    Assert.Fail(dtaStep.ResultDescription);

                BizWTF.Core.Tests.GetSteps.GetProcessDebugStep getProcess = new BizWTF.Core.Tests.GetSteps.GetProcessDebugStep("GetProcess - <Your orchestration>");
                getProcess.TargetContextProperty = "GetProcess_Result_Call2WayMock"; // Choose any relevant name
                #region GetProcessDebugStep : Orchestration selection configuration

                getProcess.ProcessName = "BizWTF.Sample.Processes.Call2WayMock";
                /// The following context properties and XPath queries are used to precisely spot which orchestration instance you want to synchronize with.
                /// This is only an example. If nothing is specified, the most recent instance will be selected.
                /// You can add as many context props and XPath queries as you like
                getProcess.ContextProps.Add(new BizWTF.Core.Entities.ProcessValidation.ControlProperty("https://BizWTF.Mocking.Schemas.ProcessingProperties",
                                                        "TestID",
                                                        send2WayMsgStep.TestID)); // NOTE : if a test ID was set in a previous step, use it here
                #endregion

                /// NOTE : the folowing code actually executes the step.
                /// Any corresponding SynchronizeProcessDebugStep should be executed prior to this
                if (!getProcess.ExecuteStep())
                    Assert.Fail(getProcess.ResultDescription);


                BizWTF.Core.Tests.TestSteps.TestProcessDebugStep testStep = new BizWTF.Core.Tests.TestSteps.TestProcessDebugStep("TestProcess - <Your orchestration>");
                testStep.SourceContextProperty = getProcess.TargetContextProperty;
                testStep.Scenario = BizWTF.Core.Entities.ProcessValidation.DebugTrace.ImportTimeLine(
                    System.Reflection.Assembly.GetExecutingAssembly().FullName,  // In this case, the resource is embedded in the current assembly
                    "BizWTF.Testing.Sample.Processes.Call2WayMock_FunctionalError_1.xml");

                if (!testStep.ExecuteStep())
                    Assert.Fail(testStep.ResultDescription);

            }
        }


        [TestMethod]
        public void Call2WayMock_TechnicalError()
        {

            using (BizWTF.Core.Tests.ActionSteps.Messaging.Init2WayMockMessagingService twoWayTarget = new BizWTF.Core.Tests.ActionSteps.Messaging.Init2WayMockMessagingService
            {
                StepName = "Init 2-Way mock service",
                ServiceURI = "http://localhost:5100/BizWTF.SystemB"
            })
            {
                #region Mocking 2-way System B
                twoWayTarget.ResolutionSettings.Clear();
                // Add as many resolution setting as needed


                /// OPTIONAL : if you need to fire some custom code when a message is received
                //MockServiceEventHub.ResetMessageReceivedEventHandler(); // Optional, if you need to flush all event handlers
                BizWTF.Mocking.Services.MockServiceEventHub.OnMessageReceived += (string uri, BizWTF.Core.Entities.MultipartMessageDefinition message) =>
                {
                    if (uri == twoWayTarget.ServiceURI)
                    {
                        // Do something
                    }
                };

                /// OPTIONAL : if you need to fire some custom code when a message is resolved
                //MockServiceEventHub.ResetMessageResolvedEventHandler(); // Optional, if you need to flush all event handlers
                BizWTF.Mocking.Services.MockServiceEventHub.OnMessageResolved += (string uri, BizWTF.Core.Entities.MultipartMessageDefinition message) =>
                {
                    if (uri == twoWayTarget.ServiceURI)
                    {
                        // Do something
                    }
                };

                /// Following code must complete (this is were we start the mock service)
                /// In case of any error, the user that runs the test may need som additional right to reserve the URL.
                /// If so, consider using the following command : netsh http add urlacl url=... user=...
                if (!twoWayTarget.ExecuteStep())
                {
                    Assert.Fail(twoWayTarget.ResultDescription);
                }
                #endregion

                BizWTF.Core.Tests.ActionSteps.Messaging.SubmitMockMessage2WayStep send2WayMsgStep = new BizWTF.Core.Tests.ActionSteps.Messaging.SubmitMockMessage2WayStep("Submit 2Way Message");
                send2WayMsgStep.DestURI = "http://localhost:5000/BizWTF.SystemA"; // NOTE : must match a receive location URI !!!
                send2WayMsgStep.SourceResource = @"BizWTF.Testing.Sample.Messages.ERROR_SystemARequest.xml";
                send2WayMsgStep.SourceResourceAssembly = System.Reflection.Assembly.GetExecutingAssembly().FullName; // In this case, the resource is embedded in the current assembly
                send2WayMsgStep.TestID = Guid.NewGuid().ToString(); // NOTE : you can provide a test ID, wich will be attached as a message property. This facilitates instance tracking.
                send2WayMsgStep.TargetContextProperty = "send2WayMsgStep_Response";

                /// NOTE : the folowwing code actually sends the message. 
                /// Nevertheless, it is recommended to first initialize all the steps of the test before executing any of them.
                if (!send2WayMsgStep.ExecuteStep())
                {
                    Assert.Fail(send2WayMsgStep.ResultDescription);
                }

                /// Once the step has executed successfully, do whatever test you need to do.
                /// The following code : BTSTestContext.GetParam(send2WayMsgStep.TargetContextProperty)
                /// retrieves the response received.
                Assert.IsNotNull(BizWTF.Core.Entities.BTSTestContext.GetParam(send2WayMsgStep.TargetContextProperty));

                /// Add the rest of your test here
                // Do something
                BizWTF.Core.Entities.MultipartMessageDefinition response = (BizWTF.Core.Entities.MultipartMessageDefinition)BizWTF.Core.Entities.BTSTestContext.GetParam(send2WayMsgStep.TargetContextProperty);


                Assert.IsNotNull(response.Parts[0].Data.SelectSingleNode("/*[local-name()='Status']"));
                Assert.AreEqual(response.Parts[0].Data.SelectSingleNode("/*[local-name()='Status']").InnerText, "TechnicalError");



                Assert.IsTrue(twoWayTarget.ReceivedMessages.Count > 0);
            }
        }

        
    }
}
