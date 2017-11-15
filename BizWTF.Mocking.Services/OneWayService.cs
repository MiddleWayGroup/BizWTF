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
    public class OneWayService : IOneWayService
    {
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



        public void SubmitMessage(MultipartMessageDefinition MultipartMessage)
        {
            Debug.WriteLine(String.Format("Mock Service : {0}", OperationContext.Current.Channel.LocalAddress.Uri.AbsoluteUri));
            Debug.WriteLine(String.Format("Received message : {0}", String.IsNullOrEmpty(MultipartMessage.Description)?"<No description provided>":MultipartMessage.Description));
            Debug.WriteLine(String.Format("Nb Parts : {0}", MultipartMessage.Parts.Length));

            this.LocalReceivedMessages.Add(MultipartMessage);
            MockServiceEventHub.RaiseMessageReceived(OperationContext.Current.Channel.LocalAddress.Uri.AbsoluteUri, MultipartMessage);
        }
    }
}
