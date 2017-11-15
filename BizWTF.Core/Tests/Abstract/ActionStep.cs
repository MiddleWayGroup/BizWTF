using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BizWTF.Core.Tests
{
    public abstract class ActionStep : BaseStep
    {
        public override StepTypes StepType
        {
            get { return StepTypes.Action; }
        }

        public ActionStep():base()
        {}

        public ActionStep(string stepName)
            : base(stepName)
        {
        }
    }
}
