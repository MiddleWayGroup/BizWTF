using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using BizWTF.Core;
using BizWTF.Core.BizTalkEntities;
using BizWTF.Core.Entities;
using BizWTF.Core.Tests;
using BizWTF.Core.Tests.ActionSteps;
using BizWTF.Core.Tests.GetSteps;
using BizWTF.Core.Tests.TestSteps;

namespace BizWTF.Core.Utilities
{
    public class TimeLineGenerator
    {
        protected TimeLine _timeLine;

        public TimeLine TimeLine
        {
            get
            {
                if (this._timeLine == null)
                    this._timeLine = new TimeLine();
                return this._timeLine;
            }
            set
            { this._timeLine = value; }
        }


        public TimeLineGenerator()
        { }

        public DebugTrace GenerateDebugTrace(ProcessFlow flow)
        {
            DebugTrace trace = new DebugTrace();

            trace.ProcessName = flow.ProcessName;
            this.decompileFlow(flow, ref trace);

            return trace;
        }

        private void decompileFlow(ShapeInfo info, ref DebugTrace trace)
        {
            DebugShape shape = new DebugShape();
            shape.shapeText = info.shapeText;
            shape.ShapeID = info.ShapeID;
            shape.shapeType = info.shapeType;
            trace.TraceDetails.Add(shape);

            foreach (ShapeInfo child in info.children)
            {
                this.decompileFlow(child, ref trace);
            }
        }
    }
}
