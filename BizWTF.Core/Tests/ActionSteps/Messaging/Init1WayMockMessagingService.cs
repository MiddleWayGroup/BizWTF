using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BizWTF.Mocking.Services;
using BizWTF.Mocking.Services.Settings;

using BizWTF.Core.Entities;

namespace BizWTF.Core.Tests.ActionSteps.Messaging
{
    public class Init1WayMockMessagingService : ActionStep, IDisposable
    {
        private MessagingHost mockHost;
        protected string _serviceURI;
        protected List<MessageResolutionSetting> _resolutionSettings;

        public string ServiceURI
        {
            get { return this._serviceURI; }
            set {   this._serviceURI = value; }
        }

        public List<MultipartMessageDefinition> ReceivedMessages
        {
            get
            {
                if (!MessagingHost.ReceivedMessages.ContainsKey(this.ServiceURI))
                    MessagingHost.ReceivedMessages.Add(this.ServiceURI, new List<MultipartMessageDefinition>());

                return MessagingHost.ReceivedMessages[this.ServiceURI];
            }
        }



        public Init1WayMockMessagingService()
            : base()
        { }

        public Init1WayMockMessagingService(string stepName)
            : base(stepName)
        {
        }

        public override bool ExecuteStep()
        {
            try
            {
                this.mockHost = new MessagingHost(this.ServiceURI);
                this.AppendResultDescription(0, "Starting 1-Way mock service. Base Address : {0}", this.ServiceURI);
                this.mockHost.Listen();

                this.AppendResultDescription(0, "Mock service started successfully");
                this.Result = StepResult.Working;
            }
            catch (Exception exc)
            {
                this.AppendResultDescription(0, "Failed to start mock service with address {0} : {1}", this.ServiceURI, exc.Message);
                this.Result = StepResult.Error;
            }

            return (this.Result == StepResult.Working);
        }

        protected void Close()
        {
            this.ReceivedMessages.Clear();
            this.mockHost.Close();
        }

        public void Dispose()
        {
            this.Close();
        }
    }
}
