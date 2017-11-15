using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
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
    public class NestedOrchestrations
    {
        [TestMethod]
        public void SendDemoMsg()
        {
            using (Init1WayMockMessagingService oneWayTarget = new Init1WayMockMessagingService
            {
                StepName = "Init 1-Way mock service",
                ServiceURI = "http://localhost:9100/BizWTF.Testing.Ports.OUT.OneWay"
            })
            {
                #region Mocking a 1-Way system (XXX)

                /// OPTIONAL : if you need to fire some custom code when a message is received
                //MockServiceEventHub.ResetMessageReceivedEventHandler(); // Optional, if you need to flush all event handlers
                MockServiceEventHub.OnMessageReceived += (string uri, MultipartMessageDefinition message) =>
                {
                    if (uri == oneWayTarget.ServiceURI)
                    {
                        // Do something
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


                using (Init2WayMockMessagingService twoWayTarget = new Init2WayMockMessagingService
                {
                    StepName = "Init 2-Way mock service",
                    ServiceURI = "http://localhost:9100/BizWTF.Testing.Ports.OUT.TwoWay"
                })
                {
                    #region Mocking 2-way System (XXX)
                    MessageResolutionSetting setting = new MessageResolutionSetting();
                    setting.Description = "Resolve service response";
                    setting.ContextProps.Add(new ControlProperty("http://schemas.microsoft.com/BizTalk/2003/system-properties",
                                                                "MessageType",
                                                                "http://BizWTF.Sample.Processes.Demo#Demo"));
                    // Add as many ControlProperty as needed
                    setting.TargetMessage = MultipartMessageSerializer.RetrieveDocument(@"<Resource name>",
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

                }
            }
        }
    }
}
