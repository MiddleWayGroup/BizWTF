using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using BizWTF.Core.Entities;

namespace BizWTF.Mocking.Services
{
    public class TwoWayService : ITwoWayService
    {
        public List<Settings.MessageResolutionSetting> LocalResolutionSettings
        {
            get
            {
                string serviceUri = OperationContext.Current.Channel.LocalAddress.Uri.AbsoluteUri;

                if (!MessagingHost.ResolutionSettings.ContainsKey(serviceUri))
                    MessagingHost.ResolutionSettings.Add(serviceUri, new List<Settings.MessageResolutionSetting>());
                return MessagingHost.ResolutionSettings[serviceUri];
            }
        }

        public List<MultipartMessageDefinition> LocalReceivedMessages
        {
            get
            {
                string serviceUri = OperationContext.Current.Channel.LocalAddress.Uri.AbsoluteUri;

                if (!MessagingHost.ReceivedMessages.ContainsKey(serviceUri))
                    MessagingHost.ReceivedMessages.Add(serviceUri, new List<MultipartMessageDefinition>());
                return MessagingHost.ReceivedMessages[serviceUri];
            }
        }



        public void SubmitMessage(ref MultipartMessageDefinition MultipartMessage)
        {
            Debug.WriteLine(String.Format("Mock Service : {0}", OperationContext.Current.Channel.LocalAddress.Uri.AbsoluteUri));
            Debug.WriteLine(String.Format("Received message : {0}", String.IsNullOrEmpty(MultipartMessage.Description)?"<No description provided>":MultipartMessage.Description));
            Debug.WriteLine(String.Format("Nb Parts : {0}", MultipartMessage.Parts.Length));

            this.LocalReceivedMessages.Add(MultipartMessage);
            MockServiceEventHub.RaiseMessageReceived(OperationContext.Current.Channel.LocalAddress.Uri.AbsoluteUri, MultipartMessage);

            foreach (Settings.MessageResolutionSetting setting in this.LocalResolutionSettings)
            {
                if (setting.Probe(MultipartMessage))
                {
                    MockServiceEventHub.RaiseMessageResolved(OperationContext.Current.Channel.LocalAddress.Uri.AbsoluteUri, setting.TargetMessage);
                    MultipartMessage = setting.TargetMessage;
                    return;
                }
            }

            throw new FaultException("Unable to resolve message");
        }
    }
}
