using BizWTF.Core.Tests.ActionSteps.Messaging;
using BizWTF.Core.Tests.ActionSteps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ServiceModel;

using BizWTF.Mocking.Services;
using BizWTF.Mocking.Services.Settings;
using BizWTF.Core.Entities;
using BizWTF.Core.Entities.Mocking;
using BizWTF.Core.Entities.ProcessValidation;

namespace BizWTF.Testing.Sample
{


    /// <summary>
    ///This is a test class for InitMockMessagingServiceTest and is intended
    ///to contain all InitMockMessagingServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class InitMockMessagingServiceTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for 1Way ExecuteStep
        ///</summary>
        [TestMethod()]
        public void Init1WayTest()
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
                MockServiceEventHub.OnMessageReceived += this.messageReceived;

                /// Following code must complete (this is were we start the mock service)
                /// In case of any error, the user that runs the test may need som additional right to reserve the URL.
                /// If so, consider using the following command : netsh http add urlacl url=... user=...
                if (!oneWayTarget.ExecuteStep())
                {
                    Assert.Fail(oneWayTarget.ResultDescription);
                }
                #endregion


                /// Add the rest of your test here
                SubmitMockMessage2WayStep sendStep = new SubmitMockMessage2WayStep("Mock submission test"); // TODO: Initialize to an appropriate value
                sendStep.DestURI = "http://localhost:9000/BizWTF.Testing.Ports.IN.TwoWay";
                sendStep.SourceResource = @"BizWTF.Testing.Sample.Messages.Mock-Demo-1WayMockService.xml";
                sendStep.SourceResourceAssembly = System.Reflection.Assembly.GetExecutingAssembly().FullName;
                sendStep.TargetContextProperty = "Init1WayTest_Response";

                bool ssResult = sendStep.ExecuteStep();
                Assert.IsTrue(ssResult);

                Assert.AreEqual(oneWayTarget.ReceivedMessages.Count, 1);
            }
        }

        /// <summary>
        ///A test for 2Way ExecuteStep
        ///</summary>
        [TestMethod()]
        public void Init2WayTest()
        {
            using (Init2WayMockMessagingService twoWayTarget = new Init2WayMockMessagingService
            {
                StepName = "Init 2-Way mock service",
                ServiceURI = "http://localhost:9100/BizWTF.Testing.Ports.OUT.TwoWay"
            })
            {
                #region Mocking 2-way System (XXX)
                MessageResolutionSetting setting = new MessageResolutionSetting();
                setting.Description = "Any demo message";
                setting.ContextProps.Add(new ControlProperty("http://schemas.microsoft.com/BizTalk/2003/system-properties", "MessageType", "http://BizWTF.Sample.Processes.Demo#Demo"));
                setting.TargetMessage = MultipartMessageSerializer.RetrieveDocument(@"BizWTF.Testing.Sample.Messages.Mock-Demo-ServiceResponse.xml",
                                                                                    System.Reflection.Assembly.GetExecutingAssembly());
                twoWayTarget.ResolutionSettings.Clear();
                twoWayTarget.ResolutionSettings.Add(setting);
                // Add as many resolution setting as needed

                /// OPTIONAL : if you need to fire some custom code when a message is received
                //MockServiceEventHub.ResetMessageReceivedEventHandler(); // Optional, if you need to flush all event handlers
                MockServiceEventHub.OnMessageReceived += this.messageReceived;

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
                SubmitMockMessage2WayStep sendStep = new SubmitMockMessage2WayStep("Mock submission test"); // TODO: Initialize to an appropriate value
                sendStep.DestURI = "http://localhost:9000/BizWTF.Testing.Ports.IN.TwoWay";
                sendStep.SourceResource = @"BizWTF.Testing.Sample.Messages.Mock-Demo-2WayMockService.xml";
                sendStep.SourceResourceAssembly = System.Reflection.Assembly.GetExecutingAssembly().FullName;
                sendStep.TargetContextProperty = "Init2WayTest_Response";

                if (!sendStep.ExecuteStep())
                {
                    Assert.Fail(sendStep.ResultDescription);
                }

                Assert.AreEqual(twoWayTarget.ReceivedMessages.Count, 1);
                Assert.IsNotNull(BTSTestContext.GetParam(sendStep.TargetContextProperty));
            }
        }


        private void messageReceived(string uri, MultipartMessageDefinition message)
        {
            Assert.IsTrue(message.Parts.Length > 0);
        }

        /// <summary>
        ///A test for 2Way ExecuteStep with fault
        ///</summary>
        [TestMethod()]
        public void Test2WayFaultedService()
        {
            using (Init2WayMockMessagingService twoWayTarget = new Init2WayMockMessagingService
            {
                StepName = "Init 2-Way mock service",
                ServiceURI = "http://localhost:9100/BizWTF.Testing.Ports.OUT.TwoWay"
            })
            {
                #region Mocking 2-way System (XXX)
                twoWayTarget.ResolutionSettings.Clear();
                
                /// OPTIONAL : if you need to fire some custom code when a message is received
                //MockServiceEventHub.ResetMessageReceivedEventHandler(); // Optional, if you need to flush all event handlers
                MockServiceEventHub.OnMessageReceived += (string uri, MultipartMessageDefinition message) =>
                {
                    if (uri == twoWayTarget.ServiceURI)
                    {
                        throw new FaultException("Simulated fault");
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
                SubmitMockMessage2WayStep sendStep = new SubmitMockMessage2WayStep("Mock submission test"); // TODO: Initialize to an appropriate value
                sendStep.DestURI = "http://localhost:9099/BizWTF.Samples/TwoWay.svc";
                sendStep.SourceResource = @"BizWTF.Testing.Sample.Messages.Mock-Demo-2WayMockService-Direct.xml";
                sendStep.SourceResourceAssembly = System.Reflection.Assembly.GetExecutingAssembly().FullName;
                sendStep.TargetContextProperty = "Init2WayTest_Response";

                if (!sendStep.ExecuteStep())
                {
                    Assert.Fail(sendStep.ResultDescription);
                }

                Assert.AreEqual(twoWayTarget.ReceivedMessages.Count, 1);
                Assert.IsNotNull(BTSTestContext.GetParam(sendStep.TargetContextProperty));

            }
        }

    }
}
