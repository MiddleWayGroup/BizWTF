using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Xml;
using Microsoft.BizTalk.Operations;
using Microsoft.BizTalk.Message.Interop;

using BizWTF.Core.Entities;
using BizWTF.Core.Entities.ProcessValidation;
using BizWTF.Core.BizTalk.Entities;
using BizWTF.Core.Utilities;

namespace BizWTF.Core.Tests
{
    public enum ServiceStatus
    {
        Started = 1,
        Success = 2,
        Terminated = 3
    }


    public class MatchingPartInfo
    {
        public Guid ServiceId;
        public string ServiceName;
        public string txtSymbol;
        public Guid ServiceInstanceId;
        public Guid MessageInstanceId;
        public ServiceStatus Status;

        public MatchingPartInfo() { }

        public static List<MatchingPartInfo> GetMatchingPartInfo(BizTalkDTADbEntities btsDTA, TrackingDatabase dta, BizTalkOperations operations, List<ControlProperty> contextProps, List<ControlField> xpathProps, string processName, string sourceContextProperty)
        {
            DateTime startDate = DateTime.Now.Subtract(new TimeSpan(BTSTestContext.MaxHistory, 0, 0, 0));
            List<MatchingPartInfo> info = new List<MatchingPartInfo>();

            if (!String.IsNullOrEmpty(sourceContextProperty))
            {
                Logger.CurrentLogger.Write(1, "Getting service instance info according to cached property '{0}'", System.Diagnostics.EventLogEntryType.Information, sourceContextProperty);
                //Guid serviceInstanceID = (Guid)BTSTestContext.Params[sourceContextProperty];
                //List<Guid> serviceInstanceIDs = (List<Guid>)BTSTestContext.Params[sourceContextProperty];
                List<OrchestrationInstance> serviceInstanceIDs = (List<OrchestrationInstance>)BTSTestContext.Params[sourceContextProperty];


                foreach (OrchestrationInstance serviceInstance in serviceInstanceIDs)
                {
                    Logger.CurrentLogger.Write(2, "(Value = '{0}')", System.Diagnostics.EventLogEntryType.Information, serviceInstanceIDs[0]);

                    /// Query the DTA Db to retrieve all messages related to 
                    /// the type of process we are looking for.
                    var msgIds = from s in btsDTA.dta_Services
                                 join si in btsDTA.dta_ServiceInstances on s.uidServiceId equals si.uidServiceId
                                 //join ss in btsDTA.dta_ServiceState on si.nServiceStateId equals ss.nServiceStateId
                                 join me in btsDTA.dta_MessageInOutEvents on si.uidServiceInstanceId equals me.uidServiceInstanceId
                                 where s.strServiceName == processName
                                   && si.uidServiceInstanceId == serviceInstance.ID
                                 orderby me.dtInsertionTimeStamp descending
                                 select new MatchingPartInfo
                                 {
                                     ServiceId = s.uidServiceId,
                                     txtSymbol = s.txtSymbol,
                                     ServiceInstanceId = si.uidServiceInstanceId,
                                     MessageInstanceId = me.uidMessageInstanceId,
                                     Status = (ServiceStatus)si.nServiceStateId
                                 };

                    if (msgIds.Count() == 0)
                        info = GetMatchingPartInfo(btsDTA, dta, operations, contextProps, xpathProps, processName, null);
                    else
                        info.AddRange(msgIds.ToList<MatchingPartInfo>());
                }
            }
            else
            {
                Logger.CurrentLogger.Write(1, "Looking for service instance into DTA Db", System.Diagnostics.EventLogEntryType.Information);

                /// Query the DTA Db to retrieve all messages related to 
                /// the type of process we are looking for.
                var msgIds = from s in btsDTA.dta_Services
                             join si in btsDTA.dta_ServiceInstances on s.uidServiceId equals si.uidServiceId
                             join me in btsDTA.dta_MessageInOutEvents on si.uidServiceInstanceId equals me.uidServiceInstanceId
                             where s.strServiceName == processName
                               && si.dtStartTime > startDate
                             orderby me.dtInsertionTimeStamp descending
                             select new MatchingPartInfo
                             {
                                 ServiceId = s.uidServiceId,
                                 ServiceName = s.strServiceName,
                                 txtSymbol = s.txtSymbol,
                                 ServiceInstanceId = si.uidServiceInstanceId,
                                 MessageInstanceId = me.uidMessageInstanceId,
                                 Status = (ServiceStatus) si.nServiceStateId
                             };

                /// Looping through all the messages in order to identify the one we are looking for
                Logger.CurrentLogger.Write(2, "Retrieved {0} messages", System.Diagnostics.EventLogEntryType.Information, msgIds.Count());
                foreach (MatchingPartInfo part in msgIds)
                {
                    bool match = PerformMessageLookup(part.MessageInstanceId, contextProps, xpathProps, dta, operations);

                    if (match)
                    {
                        Logger.CurrentLogger.Write(2, "[OK] Message match", System.Diagnostics.EventLogEntryType.Information);
                        info.Add(part);
                        //break;
                    }
                }

            }

            return info;
        }

        public static bool PerformMessageLookup(Guid messageInstanceID, List<ControlProperty> contextProps, List<ControlField> xpathProps, TrackingDatabase dta, BizTalkOperations operations)
        {
            bool result = true;

            try
            {
                /// NOTE :
                /// Following instruction will throw an exception if the corresponding message was not found in the DTA Db
                IBaseMessage message = operations.GetTrackedMessage(messageInstanceID, dta);
                Logger.CurrentLogger.Write(2, "Testing message {0}", System.Diagnostics.EventLogEntryType.Information, messageInstanceID);
                //BizTalkMessage btsMsg = (BizTalkMessage)rawMessage;

                foreach (ControlProperty prop in contextProps)
                {
                    string expectedValue = prop.Value;
                    string foundValue = (string)message.Context.Read(
                                                                prop.Property,
                                                                prop.Namespace);

                    if (expectedValue != foundValue)
                    {
                        Logger.CurrentLogger.Write(3, "[KO] Prop '{0}/{1}' : mismatch - Expected {2}, found {3}.", System.Diagnostics.EventLogEntryType.Information,
                                                prop.Namespace,
                                                prop.Property,
                                                expectedValue,
                                                foundValue);
                        result = false;
                    }
                    else
                    {
                        Logger.CurrentLogger.Write(3, "[OK] Prop '{0}/{1}' : match - Expected {2}, found {3}.", System.Diagnostics.EventLogEntryType.Information,
                                                prop.Namespace,
                                                prop.Property,
                                                expectedValue,
                                                foundValue);
                    }
                }

                if (xpathProps.Count > 0)
                {
                    List<ControlField> tmpXpathProps = xpathProps.ToList<ControlField>();
                    for (int i = 0; i < message.PartCount; i++)
                    {
                        string body = string.Empty;
                        string partName = string.Empty;
                        using (StreamReader streamReader = new StreamReader(message.GetPartByIndex(i, out partName).Data))
                        {
                            Logger.CurrentLogger.Write(3, "Testing part {0}", System.Diagnostics.EventLogEntryType.Information, partName);

                            Logger.CurrentLogger.Write(3, "Retrieving body...", System.Diagnostics.EventLogEntryType.Information);
                            body = streamReader.ReadToEnd();
                            Logger.CurrentLogger.Write(3, "Body retrieved successfully : Length={0}", System.Diagnostics.EventLogEntryType.Information, body.Length);
                            XmlDocument testedMsg = new XmlDocument();
                            testedMsg.LoadXml(body);

                            foreach (ControlField field in xpathProps)
                            {
                                string expectedValue = field.Value;
                                XmlNode tmpNode = testedMsg.SelectSingleNode(field.XPath);
                                if (tmpNode != null)
                                {
                                    string foundValue = tmpNode.InnerText;

                                    if (expectedValue != foundValue)
                                    {
                                        Logger.CurrentLogger.Write(3, "[KO] XPath '{0}' : mismatch - Expected {1}, found {2}.", System.Diagnostics.EventLogEntryType.Information,
                                                            field.XPath,
                                                            expectedValue,
                                                            foundValue);
                                    }
                                    else
                                    {
                                        Logger.CurrentLogger.Write(3, "[OK] XPath '{0}' : match - Expected {1}, found {2}.", System.Diagnostics.EventLogEntryType.Information,
                                                            field.XPath,
                                                            expectedValue,
                                                            foundValue);
                                        tmpXpathProps.Remove(tmpXpathProps.Find(tmpField => tmpField.XPath == field.XPath && tmpField.Value == field.Value));
                                    }
                                }
                            }

                            if (tmpXpathProps.Count == 0)
                                break;
                        }
                    }

                    if (tmpXpathProps.Count == 0)
                        result = true;
                    else
                        result = false;


                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.CurrentLogger.Write(2, "Error when retrieving message {0} from DTA Db : {1}", System.Diagnostics.EventLogEntryType.Error, messageInstanceID, ex.Message);
                return false;
            }
        }
    }


    public class MatchingPartServiceIDComparer : IEqualityComparer<MatchingPartInfo>
    {
        #region IEqualityComparer<MatchingPartInfo> Members

        public bool Equals(MatchingPartInfo x, MatchingPartInfo y)
        {
            return x.ServiceInstanceId.Equals(y.ServiceInstanceId);
        }

        public int GetHashCode(MatchingPartInfo obj)
        {
            return obj.ServiceInstanceId.GetHashCode();
        }

        #endregion
    }
}
