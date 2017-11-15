using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BizWTF.Core.Tests
{
    public abstract class TestStep : BaseStep
    {
        public override StepTypes StepType
        {
            get { return StepTypes.Test; }
        }

        public TestStep()
            : base()
        {}

        public TestStep(string stepName)
            : base(stepName)
        {
        }
    }
}
