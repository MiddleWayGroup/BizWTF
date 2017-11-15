using BizWTF.Core.Tests.ActionSteps.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BizWTF.Testing.Sample
{
    
    
    /// <summary>
    ///This is a test class for SubmitMockMessageStepTest and is intended
    ///to contain all SubmitMockMessageStepTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SubmitMockMessageStepTest
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
        ///A test for ExecuteStep
        ///</summary>
        [TestMethod()]
        public void Execute1WayStepTest()
        {
            SubmitMockMessage1WayStep target = new SubmitMockMessage1WayStep("Mock submission test"); // TODO: Initialize to an appropriate value
            target.DestURI = "http://localhost:9000/BizWTF.Testing.Ports.IN.OneWay";
            target.SourceResource = @"BizWTF.Testing.Sample.Messages.Mock-Demo-Enveloppe.xml";
            target.SourceResourceAssembly = System.Reflection.Assembly.GetExecutingAssembly().FullName;
            
            bool expected = true; 
            bool actual = target.ExecuteStep();
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ExecuteStep
        ///</summary>
        [TestMethod()]
        public void Execute2WayStepTest()
        {
            SubmitMockMessage2WayStep target = new SubmitMockMessage2WayStep("Mock submission test"); // TODO: Initialize to an appropriate value
            target.DestURI = "http://localhost:9000/BizWTF.Testing.Ports.IN.TwoWay";
            target.SourceResource = @"BizWTF.Testing.Sample.Messages.Mock-Demo.xml";
            target.SourceResourceAssembly = System.Reflection.Assembly.GetExecutingAssembly().FullName;
            target.TargetContextProperty = "BizWTFResponse";

            bool expected = true;
            bool actual = target.ExecuteStep();
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
