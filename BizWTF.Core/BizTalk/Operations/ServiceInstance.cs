using System;
using System.Collections.Generic;
using System.Management;
using System.Linq;
using System.Text;

using Microsoft.BizTalk.Operations;


namespace BizWTF.Core.BizTalk.Operations
{
    public class ServiceInstance
    {
        protected ManagementObject _wmiInstance;


        public Guid InstanceID { get; set; }
        public string ServiceName
        {
            get
            {   return this._wmiInstance["ServiceName"].ToString(); }
        }
        public string ServiceClassId
        {
            get
            { return this._wmiInstance["ServiceClassId"].ToString(); }
        }
        public string ServiceTypeId
        {
            get
            { return this._wmiInstance["ServiceTypeId"].ToString(); }
        }
        public string ErrorDescription
        {
            get
            { return this._wmiInstance["Errordescription"].ToString(); }
        }
        public string HostName
        {
            get
            { return this._wmiInstance["HostName"].ToString(); }
        }



        public ServiceInstance(Guid instanceID)
        {
            this.InstanceID = instanceID;

            string strWQL = string.Format(
                    "SELECT * FROM MSBTS_ServiceInstance WHERE InstanceID = '{{{0}}}'", this.InstanceID);
            ManagementObjectSearcher searcherServiceInstance = new ManagementObjectSearcher (new ManagementScope ("root\\MicrosoftBizTalkServer"), new WqlObjectQuery(strWQL), null);

            if (searcherServiceInstance.Get().Count > 0)
            {
                foreach (ManagementObject objServiceInstance in searcherServiceInstance.Get())
                {
                    this._wmiInstance = objServiceInstance;
                    break;
                }
            }   
        }

        public bool Resume()
        {
            if (this._wmiInstance != null)
            {
                string strHostQueueFullPath = string.Format("root\\MicrosoftBizTalkServer:MSBTS_HostQueue.HostName=\"{0}\"", this.HostName);
                ManagementObject objHostQueue = new ManagementObject(strHostQueueFullPath);

                // Note: The ResumeServiceInstanceByID() method processes at most 2047 service instances with each call.
                //   If you are dealing with larger number of service instances, this script needs to be modified to break down the
                //   service instances into multiple batches.
                objHostQueue.InvokeMethod("ResumeServiceInstancesByID",
                    new object[] { new string[] { this.ServiceClassId }, new string[] { this.ServiceTypeId }, new string[] { String.Format("{{{0}}}", this.InstanceID) }, 1 }
                );

                return true;
            }
            else
                return false;
        }
    }
}
