using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;

using BizWTF.Core.Entities;

namespace BizWTF.Mocking.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITwoWayService" in both code and config file together.
    [ServiceContract(Name = "TwoWayMockService", Namespace = "http://BizWTF.Mocking.Schemas.Submission")]
    public interface ITwoWayService
    {
        [OperationContract(Action = "SubmitMessage", ReplyAction = "SubmitMessageResponse", Name = "SubmitMessage", IsOneWay=false)]
        void SubmitMessage(ref MultipartMessageDefinition MultipartMessage);
        
    }
}
