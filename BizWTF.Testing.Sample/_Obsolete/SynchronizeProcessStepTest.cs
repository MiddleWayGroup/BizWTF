using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.Xml.Serialization;

using System.Reflection;
using System.Threading;

using BizWTF.Core;
using BizWTF.Core.Entities.ProcessValidation;
using BizWTF.Core.Tests;
using BizWTF.Core.Tests.ActionSteps;
using BizWTF.Core.Tests.ActionSteps.Messaging;
using BizWTF.Core.Tests.GetSteps;
using BizWTF.Core.Tests.TestSteps;

using Microsoft.XLANGs.BaseTypes;

namespace BizWTF.Core.Entities
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class SynchronizeProcessStepTest
    {
        public SynchronizeProcessStepTest()
        {   
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        //[TestMethod]
        public void TestProcess_Standard()
        {
            SubmitMockMessage1WayStep smm = new SubmitMockMessage1WayStep("Mock submission test"); // TODO: Initialize to an appropriate value
            smm.DestURI = "http://localhost:9000/BizWTF.Testing.Ports.IN.OneWay";
            smm.SourceFile = @"C:\Temp\BizWTF\BizWTF.SubmissionTest - Enveloppe.xml"; 
            
            //WaitStep ws1 = new WaitStep("Wait until process starts");
            //ws1.Seconds = 1;

            SynchronizeProcessDebugStep sp = new SynchronizeProcessDebugStep("Test synchro : BizWTF.Sample.Processes.Processes.TestOrch1");
            sp.ProcessName = "BizWTF.Sample.Processes.TestOrch1";
            sp.TargetContextProperty = "ServiceInstanceID";
            sp.ContextProps.Add(new ControlProperty(BTSProperties.messageType.Name.Namespace,
                                                    BTSProperties.messageType.Name.Name, 
                                                    "http://BizWTF.Sample.Processes.Demo#Demo"));
            sp.XPathProps.Add(new ControlField("/*[local-name()='Demo']/*[local-name()='Field1']", "Test1"));

            ForceTrackedMessageCopyJobStep ftmc = new ForceTrackedMessageCopyJobStep("Wait until tracking job executes");
            
            GetProcessDebugStep gp = new GetProcessDebugStep("Orchestration query : BizWTF.Sample.Processes.Processes.TestOrch1");
            gp.ProcessName = sp.ProcessName;
            gp.SourceContextProperty = "ServiceInstanceID";
            gp.TargetContextProperty = "OrchestrationInstanceInfo";
            gp.ContextProps.Add(new ControlProperty(BTSProperties.messageType.Name.Namespace,
                                                    BTSProperties.messageType.Name.Name,
                                                    "http://BizWTF.Sample.Processes.Demo#Demo"));
            gp.XPathProps.Add(new ControlField("/*[local-name()='Demo']/*[local-name()='Field1']", "Test1"));

            TestProcessDebugStep tp = new TestProcessDebugStep("Orchestration test : BizWTF.Sample.Processes.Processes.TestOrch1");
            tp.SourceContextProperty = "OrchestrationInstanceInfo";
            DebugTrace expectedTrace = new DebugTrace();
            DebugShape shape = new DebugShape();
            shape.shapeType = ShapeTypes.ReceiveShape;
            shape.shapeText = "Rcv input msg";
            shape.ShapeID = new Guid("5386ed66-aca3-4f68-af0b-d5c9f042a7f6");
            expectedTrace.TraceDetails.Add(shape);
            shape = new DebugShape();
            shape.shapeType = ShapeTypes.DelayShape; 
            shape.shapeText = "Delay 10s";
            shape.ShapeID = new Guid("5e08ebcc-ac83-4b52-a48f-d4f94b723c9e");
            expectedTrace.TraceDetails.Add(shape);
            shape = new DebugShape();
            shape.shapeType = ShapeTypes.SendShape;
            shape.shapeText = "Send output msg";
            shape.ShapeID = new Guid("345b65b4-a222-4703-b55a-946bd4550d11");
            expectedTrace.TraceDetails.Add(shape);
            tp.Scenario = expectedTrace;


            TimeLine tl = new TimeLine();
            List<TimeLineStep> ts = new List<TimeLineStep>();
            ts.Add(new TimeLineStep(smm));
            //ts.Add(new TimeLineStep(ws1));
            ts.Add(new TimeLineStep(sp));
            ts.Add(new TimeLineStep(ftmc));
            ts.Add(new TimeLineStep(gp));
            ts.Add(new TimeLineStep(tp));
            tl.Steps = ts;

            tl.ExportTimeLine(@"C:\Projects\BizWTF\BizWTF.Testing.Sample\Resources\TimeLine1.xml");

            tl.Execute();

        }

        [TestMethod]
        public void TestProcess_Synchronize()
        {
            SynchronizeProcessDebugStep sp = new SynchronizeProcessDebugStep("Test synchro : BizWTF.Sample.Processes.Receive2WayDemo");
            sp.ProcessName = "BizWTF.Sample.Processes.Receive2WayDemo";
            sp.ContextProps.Add(new ControlProperty(BTSProperties.messageType.Name.Namespace,
                                                    BTSProperties.messageType.Name.Name,
                                                    "http://BizWTF.Sample.Processes.Demo#Demo"));
            sp.XPathProps.Add(new ControlField("/*[local-name()='Demo']/*[local-name()='Field1']", "Field1_0"));
            sp.PollingCount = 10;
            sp.PollingInterval = 1000;
            sp.TargetContextProperty = "ServiceInstanceIDs";


            SubmitMockMessage2WayStep target = new SubmitMockMessage2WayStep("Mock message submission");
            target.DestURI = "http://localhost:9000/BizWTF.Testing.Ports.IN.TwoWay";
            target.SourceFile = @"C:\Temp\BizWTF\BizWTF.SubmissionTest - Demo.xml";
            target.TargetContextProperty = "BizWTFResponse";

            ThreadStart ts = new ThreadStart(() => {
                                    bool msgResult = false;
                                    msgResult = target.ExecuteStep(); 
                                    });
            Thread t = new Thread(ts);
            t.Start();
            
            bool result = sp.ExecuteStep();
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void TestProcess_Receive2WayDemo()
        {
            SubmitMockMessage2WayStep send2WayMsgStep = new SubmitMockMessage2WayStep("Submit 2Way Message");
            send2WayMsgStep.DestURI = "http://localhost:9000/BizWTF.Testing.Ports.IN.TwoWay"; // NOTE : must match a receive location URI !!!
            send2WayMsgStep.SourceResource = @"<your embedded resources here>";
            send2WayMsgStep.SourceResourceAssembly = System.Reflection.Assembly.GetExecutingAssembly().FullName; // In this case, the resource is embedded in the current assembly
            send2WayMsgStep.TestID = Guid.NewGuid().ToString(); // NOTE : you can provide a test ID, wich will be attached as a message property. This facilitates instance tracking.
            send2WayMsgStep.TargetContextProperty = "send2WayMsgStep_Response";


            SynchronizeProcessDebugStep synchroStep = new SynchronizeProcessDebugStep("Synchronization : <Your orchestration>");
            synchroStep.ProcessName = "BizWTF.Sample.Processes.Receive2WayDemo";
            synchroStep.TargetContextProperty = "SyncProcess_Result_Receive2WayDemo_OK"; // Choose any relevant name
            synchroStep.ContextProps.Add(new ControlProperty("https://BizWTF.Mocking.Schemas.ProcessingProperties",
                                                    "TestID",
                                                    send2WayMsgStep.TestID)); // NOTE : if a test ID was set in a previous step, use it here
            
            /// NOTE : Polling settings are used to determine how long BizWTF will keep waiting for a matching instance to be started.
            /// If no matching instance is found, the step fails.
            synchroStep.PollingCount = 10;
            synchroStep.PollingInterval = 500; // Interval in ms

            /// ForceTrackedMessageCopyJobStep : used to force execution of the TrackedMessageCopy job
            ForceTrackedMessageCopyJobStep ftmc = new ForceTrackedMessageCopyJobStep("Wait until tracking job executes");

            if (!synchroStep.ExecuteStep())
            {
                Assert.Fail(synchroStep.ResultDescription);
                // NOTE : in case of short living orchestrations, the synchronization step may fail because it didn't have to time to execute before the orchestration finished
            }
            if (!ftmc.ExecuteStep())
                Assert.Fail(ftmc.ResultDescription); //NOTE : this step should DEFINITELY complete
                    

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
