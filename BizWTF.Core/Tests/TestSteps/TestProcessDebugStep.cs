using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using BizWTF.Core.Entities;
using BizWTF.Core.Entities.ProcessValidation;
using BizWTF.Core.Utilities;
using System.Diagnostics;

namespace BizWTF.Core.Tests.TestSteps
{
    [Serializable]
    public class TestProcessDebugStep : TestStep
    {
        protected DebugTrace _scenario;
        protected string _sourceContextProperty;

        public string SourceContextProperty
        {
            get { return this._sourceContextProperty; }
            set { this._sourceContextProperty = value; }
        }

        [XmlIgnore]
        public List<OrchestrationInstanceInfo> InstanceInfo
        {
            get
            {
                if (BTSTestContext.Params[this.SourceContextProperty] == null)
                    BTSTestContext.Params[this.SourceContextProperty] = new List<OrchestrationInstanceInfo>();
                return (List<OrchestrationInstanceInfo>)BTSTestContext.Params[this.SourceContextProperty];
            }
        }

        public DebugTrace Scenario
        {
            get
            {
                if (this._scenario == null)
                    this._scenario = new DebugTrace();
                return this._scenario;
            }
            set { this._scenario = value; }
        }



        public TestProcessDebugStep()
        { }


        public TestProcessDebugStep(string stepName)
            : base(stepName)
        {}



        public override bool ExecuteStep()
        {
            this.AppendResultDescription(0, "Executing step [{0}] ({1}):", this.StepName, this.StepType);

            foreach (OrchestrationInstanceInfo info in this.InstanceInfo)
            {
                this.AppendResultDescription(0, "Flow Validation on process '{0}'", info.FlowDefinition.ProcessName);
                foreach (DebugShape shape in this.Scenario.TraceDetails)
                {
                    if (!info.Trace.FindDebugShape(shape.ShapeID, shape.RepeatCount, shape.Completed))
                    {
                        this.AppendResultDescription(1, "[KO] Shape {0} ({1})", shape.shapeText, shape.ShapeID);
                        this.Result = StepResult.Error;
                    }
                    else
                    {
                        this.AppendResultDescription(1, "[OK] Shape {0} ({1})", shape.shapeText, shape.ShapeID);
                        //this.Result = StepResult.OK;
                    }
                }
            }
            if (this.Result == StepResult.Working)
                this.Result = StepResult.OK;

            return (this.Result == StepResult.OK);
        }
    }
}
