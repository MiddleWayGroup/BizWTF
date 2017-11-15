using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.ServiceModel.Description;

using BizWTF.Core.Entities;
using BizWTF.Mocking.Services.Settings;

namespace BizWTF.Mocking.Services
{
    public class MessagingHost
    {
        protected ServiceHost _host;
        protected string _targetContextProperty;

        public string TargetContextProperty
        {
            set { this._targetContextProperty = value; }
            get { return this._targetContextProperty; }
        }

        protected static Dictionary<string, List<Settings.MessageResolutionSetting>> _resSettings;
        public static Dictionary<string, List<Settings.MessageResolutionSetting>> ResolutionSettings
        {
            get
            {
                if (_resSettings == null)
                    _resSettings = new Dictionary<string, List<Settings.MessageResolutionSetting>>();
                return _resSettings;
            }
        }

        protected static Dictionary<string, List<MultipartMessageDefinition>> _receivedMessages;
        public static Dictionary<string, List<MultipartMessageDefinition>> ReceivedMessages
        {
            get
            {
                if (_receivedMessages == null)
                    _receivedMessages = new Dictionary<string, List<MultipartMessageDefinition>>();
                return _receivedMessages;
            }
        }


        /// <summary>
        /// Initiates a One-Way messaging host
        /// </summary>
        /// <param name="uri">UIR of the service</param>
        public MessagingHost(string uri)
        {
            this._host = new ServiceHost(typeof(OneWayService), new Uri(uri));

            ServiceEndpoint ep = this._host.AddServiceEndpoint("BizWTF.Mocking.Services.IOneWayService",
                                            new BasicHttpBinding(),
                                            "");

            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            this._host.Description.Behaviors.Add(smb);

            this._host.AddServiceEndpoint(
                                      ServiceMetadataBehavior.MexContractName,
                                      MetadataExchangeBindings.CreateMexHttpBinding(),
                                      "mex"
                                    );

            ((ServiceDebugBehavior)this._host.Description.Behaviors.Single(b => b.GetType() == typeof(ServiceDebugBehavior))).IncludeExceptionDetailInFaults = true;
        }

        /// <summary>
        /// Initiates a Two-Way messaging host
        /// </summary>
        /// <param name="uri">URI of the service</param>
        /// <param name="settings">Message resolution settings. These are used by the 2-Way service to determine what message must be </param>
        public MessagingHost(string uri, List<MessageResolutionSetting> settings)
        {
            this._host = new ServiceHost(typeof(TwoWayService), new Uri(uri));

            ServiceEndpoint ep = this._host.AddServiceEndpoint("BizWTF.Mocking.Services.ITwoWayService", 
                                            new BasicHttpBinding(),
                                            "");

            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            this._host.Description.Behaviors.Add(smb);

            this._host.AddServiceEndpoint(
                                      ServiceMetadataBehavior.MexContractName,
                                      MetadataExchangeBindings.CreateMexHttpBinding(),
                                      "mex"
                                    );

            ((ServiceDebugBehavior) this._host.Description.Behaviors.Single(b => b.GetType() == typeof(ServiceDebugBehavior))).IncludeExceptionDetailInFaults = true;



            if (ResolutionSettings.ContainsKey(uri))
                ResolutionSettings.Remove(uri);
            ResolutionSettings.Add(uri, settings);
        }

        public void Listen()
        {
            this._host.Open();
        }

        public void Close()
        {
            if (this._host.State == CommunicationState.Opened)
                this._host.Close();
        }
    }
}
