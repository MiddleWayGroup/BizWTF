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
    public class Mocking
    {
        [TestMethod]
        public void Init2WayMock()
        {   
            using (Init2WayMockMessagingService twoWayTarget = new Init2WayMockMessagingService
            {
                StepName = "Init 2-Way mock service",
                ServiceURI = "http://localhost:9100/BizWTF.Testing.Ports.OUT.TwoWay"
            })
            {
                #region Mocking 2-way System (XXX)
                MessageResolutionSetting setting = new MessageResolutionSetting();
                setting.Description = "Always return the same response";
                setting.ContextProps.Add(new ControlProperty("http://schemas.microsoft.com/BizTalk/2003/system-properties",
                                                            "MessageType",
                                                            "http://BizWTF.Sample.Processes.Demo#Demo"));
                // Add as many ControlProperty as needed
                setting.TargetMessage = MultipartMessageSerializer.RetrieveDocument(@"BizWTF.Testing.Sample.Messages.Mock-Demo-2WayMockService-Response.xml",
                                                                                    System.Reflection.Assembly.GetExecutingAssembly());
                twoWayTarget.ResolutionSettings.Clear();
                twoWayTarget.ResolutionSettings.Add(setting);
                // Add as many resolution setting as needed

                /// OPTIONAL : if you need to fire some custom code when a message is received
                //MockServiceEventHub.ResetMessageReceivedEventHandler(); // Optional, if you need to flush all event handlers
                MockServiceEventHub.OnMessageReceived += (string uri, MultipartMessageDefinition message) =>
                {
                    if (uri == twoWayTarget.ServiceURI)
                    {
                        // Do something
                    }
                };

                /// OPTIONAL : if you need to fire some custom code when a message is resolved
                //MockServiceEventHub.ResetMessageResolvedEventHandler(); // Optional, if you need to flush all event handlers
                MockServiceEventHub.OnMessageResolved += (string uri, MultipartMessageDefinition message) =>
                {
                    if (uri == twoWayTarget.ServiceURI)
                    {
                        setting.TargetMessage.Description = message == null ? "empy message received" : message.Description;
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



                /// Add the rest of your test here
                WaitStep ws = new WaitStep("Wait for 10min");
                ws.Seconds = 600;
                if (!ws.ExecuteStep())
                {
                    Assert.Fail(ws.ResultDescription);
                }

            }

        }

        [TestMethod]
        public void Call1WayMock()
        {
            using (Init1WayMockMessagingService oneWayTarget = new Init1WayMockMessagingService
            {
                StepName = "Init 1-Way mock service",
                ServiceURI = "http://localhost:9100/BizWTF.Testing.Ports.OUT.OneWay"
            })
            {
                bool msgReceived = false;

                #region Mocking a 1-Way system (XXX)

                /// OPTIONAL : if you need to fire some custom code when a message is received
                //MockServiceEventHub.ResetMessageReceivedEventHandler(); // Optional, if you need to flush all event handlers
                MockServiceEventHub.OnMessageReceived += (string uri, MultipartMessageDefinition message) =>
                {
                    if (uri == oneWayTarget.ServiceURI)
                    {
                        msgReceived = true;
                    }
                };

                /// Following code must complete (this is were we start the mock service)
                /// In case of any error, the user that runs the test may need som additional right to reserve the URL.
                /// If so, consider using the following command : netsh http add urlacl url=... user=...
                if (!oneWayTarget.ExecuteStep())
                {
                    Assert.Fail(oneWayTarget.ResultDescription);
                }
                #endregion


                /// Add the rest of your test here
                // Do something
                SubmitMockMessage1WayStep send1WayMsgStep = new SubmitMockMessage1WayStep("Submit 1Way Message");
                send1WayMsgStep.DestURI = "http://localhost:9100/BizWTF.Testing.Ports.IN.OneWay"; // NOTE : must match a receive location URI !!!
                send1WayMsgStep.SourceResource = @"BizWTF.Testing.Sample.Messages.Mock-Demo-1WayMockService.xml";
                send1WayMsgStep.SourceResourceAssembly = System.Reflection.Assembly.GetExecutingAssembly().FullName; // In this case, the resource is embedded in the current assembly
                send1WayMsgStep.TestID = send1WayMsgStep.TestID; // NOTE : you can provide a test ID, wich will be attached as a message property. This facilitates instance tracking.


                     

                /// NOTE : the folowwing code actually sends the message. 
                /// Nevertheless, it is recommended to first initialize all the steps of the test before executing any of them.
                if (!send1WayMsgStep.ExecuteStep())
                {
                    Assert.Fail(send1WayMsgStep.ResultDescription);
                }


                int iterationCount = 0;
                while (!msgReceived)
                {
                    iterationCount++;
                    System.Threading.Thread.Sleep(1000);
                    if (iterationCount == 1000)
                    {
                        Assert.Fail("No message was received");
                        break;
                    }
                }
               
            }
        }

        [TestMethod]
        public void Call2WayMock()
        {
            using (Init2WayMockMessagingService twoWayTarget = new Init2WayMockMessagingService
            {
                StepName = "Init 2-Way mock service",
                ServiceURI = "http://localhost:9100/BizWTF.Testing.Ports.OUT.TwoWay"
            })
            {
                #region Mocking 2-way System (XXX)
                MessageResolutionSetting setting = new MessageResolutionSetting();
                setting.Description = "Always return the same response";
                setting.ContextProps.Add(new ControlProperty("http://schemas.microsoft.com/BizTalk/2003/system-properties",
                                                            "MessageType",
                                                            "http://BizWTF.Sample.Processes.Demo#Demo"));
                // Add as many ControlProperty as needed
                setting.TargetMessage = MultipartMessageSerializer.RetrieveDocument(@"BizWTF.Testing.Sample.Messages.Mock-Demo-2WayMockService-Response.xml",
                                                                                    System.Reflection.Assembly.GetExecutingAssembly());
                twoWayTarget.ResolutionSettings.Clear();
                twoWayTarget.ResolutionSettings.Add(setting);
                // Add as many resolution setting as needed

                /// OPTIONAL : if you need to fire some custom code when a message is received
                //MockServiceEventHub.ResetMessageReceivedEventHandler(); // Optional, if you need to flush all event handlers
                MockServiceEventHub.OnMessageReceived += (string uri, MultipartMessageDefinition message) =>
                {
                    if (uri == twoWayTarget.ServiceURI)
                    {
                        // Do something
                    }
                };

                /// OPTIONAL : if you need to fire some custom code when a message is resolved
                //MockServiceEventHub.ResetMessageResolvedEventHandler(); // Optional, if you need to flush all event handlers
                MockServiceEventHub.OnMessageResolved += (string uri, MultipartMessageDefinition message) =>
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



                /// Add the rest of your test here
                // Do something
                SubmitMockMessage2WayStep send2WayMsgStep = new SubmitMockMessage2WayStep("Submit 2Way Message");
                send2WayMsgStep.DestURI = "http://localhost:9100/BizWTF.Testing.Ports.IN.TwoWay"; // NOTE : must match a receive location URI !!!
                send2WayMsgStep.SourceResource = @"BizWTF.Testing.Sample.Messages.Mock-Demo-1WayMockService.xml";
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
                Assert.IsNotNull(BTSTestContext.GetParam(send2WayMsgStep.TargetContextProperty));
            }
        }
    }
}
