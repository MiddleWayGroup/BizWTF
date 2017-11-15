using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BizWTF.Core.Utilities;

namespace BizWTF.Core.Tests.ActionSteps
{
    public class WaitStep : ActionStep
    {
        protected int _seconds;

        public int Seconds
        {
            get
            { return this._seconds; }
            set
            {
                if (value <= 0)
                    this._seconds = 1;
                else
                    this._seconds = value;
            }
        }

        public WaitStep() { }

        public WaitStep(string stepName) : base(stepName)
        { }

        public override bool ExecuteStep()
        {
            this.AppendResultDescription(0, "Executing step [{0}] ({1}):", this.StepName, this.StepType);
            System.Threading.Thread.Sleep(new TimeSpan(0, 0, this.Seconds));

            this.Result = StepResult.OK;
            return true;
        }
    }
}
