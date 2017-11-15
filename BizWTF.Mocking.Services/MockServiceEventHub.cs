using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BizWTF.Core.Entities;

namespace BizWTF.Mocking.Services
{
    public static class MockServiceEventHub
    {
        public delegate void OnMessageEventHandler(string serviceURI, MultipartMessageDefinition inputMessage);

        public static event OnMessageEventHandler OnMessageReceived;

        internal static void RaiseMessageReceived(string serviceURI, MultipartMessageDefinition inputMessage)
        {
            if (OnMessageReceived != null)
                OnMessageReceived(serviceURI, inputMessage);
        }


        public static event OnMessageEventHandler OnMessageResolved;

        internal static void RaiseMessageResolved(string serviceURI, MultipartMessageDefinition inputMessage)
        {
            if (OnMessageResolved != null)
                OnMessageResolved(serviceURI, inputMessage);
        }



        public static void ResetMessageReceivedEventHandler()
        { OnMessageReceived = null; }

        public static void ResetMessageResolvedEventHandler()
        { OnMessageResolved = null; }
    }
}
