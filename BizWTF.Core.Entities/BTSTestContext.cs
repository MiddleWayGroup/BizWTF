using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace BizWTF.Core.Entities
{
    public enum CommonParamNames
    {
        BizTalkDbServer,
        BizTalkDTADb,
        BizTalkMgmtDb,
        TrackedMessageCopyJobName,
        MaxHistory
    }

    public static class BTSTestContext
    {
        public static string BizTalkDbServer
        {
            get { return ConfigurationManager.AppSettings[CommonParamNames.BizTalkDbServer.ToString()]; }
        }
        public static string BizTalkDTADb
        {
            get { return ConfigurationManager.AppSettings[CommonParamNames.BizTalkDTADb.ToString()]; }
        }
        public static string BizTalkMgmtDb
        {
            get { return ConfigurationManager.AppSettings[CommonParamNames.BizTalkMgmtDb.ToString()]; }
        }
        public static string TrackedMessageCopyJobName
        {
            get { return ConfigurationManager.AppSettings[CommonParamNames.TrackedMessageCopyJobName.ToString()]; }
        }
        public static int MaxHistory
        {
            get { return Int32.Parse(ConfigurationManager.AppSettings[CommonParamNames.MaxHistory.ToString()]); }
        }


        private static Dictionary<string, object> _params;

        public static Dictionary<string, object> Params
        {
            get
            {
                if (_params == null)
                    _params = new Dictionary<string, object>();
                return _params;
            }
            set { _params = value; }
        }

        public static object GetParam(CommonParamNames paramName)
        {
            return GetParam(paramName.ToString());
        }
        public static object GetParam(string paramName)
        {
            return Params[paramName];
        }

        public static void AddParam(CommonParamNames paramName, object paramValue)
        {
            AddParam(paramName.ToString(), paramValue);
        }
        public static void AddParam(string paramName, object paramValue)
        {
            if (Params.ContainsKey(paramName))
                Params[paramName] = paramValue;
            else
                Params.Add(paramName, paramValue);
        }
    }
}
