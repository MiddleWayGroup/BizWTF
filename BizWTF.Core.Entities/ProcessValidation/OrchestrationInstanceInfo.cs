using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.BizTalk.Message.Interop;

namespace BizWTF.Core.Entities.ProcessValidation
{
    
    public class OrchestrationInstanceInfo
    {
        protected ProcessFlow _flowDefinition;
        protected DebugTrace _trace;
        protected List<IBaseMessage> _messages;

        public ProcessFlow FlowDefinition
        {
            get
            {
                if (this._flowDefinition == null)
                    this._flowDefinition = new ProcessFlow();
                return this._flowDefinition;
            }
            set { this._flowDefinition = value; }
        }
        public DebugTrace Trace
        {
            get
            {
                if (this._trace == null)
                    this._trace = new DebugTrace();
                return this._trace;
            }
            set { this._trace = value; }
        }

        public List<IBaseMessage> Messages
        {
            get
            {
                if (this._messages == null)
                    this._messages = new List<IBaseMessage>();
                return this._messages;
            }
            set { this._messages = value; }
        }

        public OrchestrationInstanceInfo()
        { }

        public OrchestrationInstanceInfo(string processSymbols)
        {
            this._flowDefinition = ProcessFlow.DeserializeProcessFlow(processSymbols);
        }
    }
}
