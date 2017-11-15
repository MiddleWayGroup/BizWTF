using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using BizWTF.Core;
using BizWTF.Core.BizTalk.Entities;
using BizWTF.Core.Entities;
using BizWTF.Core.Entities.ProcessValidation;
using BizWTF.Core.Tests;

namespace BizWTF.Testing.VSDevTools.Processing
{
    internal static class BTSOrchestrationExplorer
    {
        internal static List<OrchestrationInfo> GetDeployedOrchestrations(string entityDTAConnStr)
        {
            List<OrchestrationInfo> orchs = new List<OrchestrationInfo>();

            using (BizTalkDTADbEntities btsDTA = new BizTalkDTADbEntities(entityDTAConnStr))
            {
                var rawOrchs = from s in btsDTA.dta_Services
                             where s.strServiceType == "Orchestration"
                             select new OrchestrationInfo
                             {
                                 ServiceName = s.strServiceName,
                                 AssemblyName = s.strAssemblyName,
                                 TxtSymbols = s.txtSymbol
                             };

                orchs = rawOrchs.ToList<OrchestrationInfo>();
            }
            return orchs;
        }


        public static DebugTrace GetDebugTrace(string processName, string processFlowSymbols, FlatteningPrefixType prefixType)
        {
            ProcessFlow flow = ProcessFlow.DeserializeProcessFlow(processFlowSymbols);
            flow.ProcessName = processName;

            return flow.Flatten(prefixType);
        }

    }

    internal class OrchestrationInfo
    {
        public string ServiceName;
        public string AssemblyName;
        public string TxtSymbols;
    }
}
