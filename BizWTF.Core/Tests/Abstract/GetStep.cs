using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BizWTF.Core.Tests
{
    public abstract class GetStep : BaseStep
    {
        public override StepTypes StepType
        {
            get { return StepTypes.Get; }
        }

        public GetStep()
        { }

        public GetStep(string stepName)
            : base(stepName)
        {
        }

        //public abstract T GetData<T>();
    }
}
