using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.EntityClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

using System.ServiceModel;
using System.ServiceModel.Configuration;

namespace BizWTF.Testing.VSDevTools
{
    internal static class Tools
    {
        public static void GetConnectionStrings(string serverName, string dtaDBName, string mgmtDBName, out string entityDTAConnStr, out string sqlMgmtConnStr)
        {
            string providerName = "System.Data.SqlClient";

            // Initialize the connection string builder for the
            // underlying provider.
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            // Set the properties for the data source.
            sqlBuilder.DataSource = serverName;
            sqlBuilder.InitialCatalog = dtaDBName;
            sqlBuilder.IntegratedSecurity = true;

            // Initialize the EntityConnectionStringBuilder.
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();

            //Set the provider name.
            entityBuilder.Provider = providerName;
            entityBuilder.ProviderConnectionString = sqlBuilder.ToString();
            entityBuilder.Metadata = @"res://*/BizTalk.Entities.BizTalkDTAModel.csdl|res://*/BizTalk.Entities.BizTalkDTAModel.ssdl|res://*/BizTalk.Entities.BizTalkDTAModel.msl";

            entityDTAConnStr = entityBuilder.ToString();

            sqlBuilder.DataSource = serverName;
            sqlBuilder.InitialCatalog = mgmtDBName;
            sqlBuilder.IntegratedSecurity = true;

            sqlMgmtConnStr = sqlBuilder.ToString();
        }

        public static void AddConfigKeys(EnvDTE.Project project, string sqlServerName, string btsDTADb, string btsMgmtDb)
        {
            Configuration conf = null;

            foreach (EnvDTE.ProjectItem item in project.ProjectItems)
            {
                if (item.Name.ToLower() == "app.config")
                {
                    ExeConfigurationFileMap map = new ExeConfigurationFileMap { ExeConfigFilename = item.FileNames[0] };
                    conf = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
                    break;
                }
            }
            if (conf == null)
            {
                XmlDocument appConfigXml = new XmlDocument();
                string configPath = Path.Combine(new FileInfo(project.FullName).Directory.FullName, "App.config");
                appConfigXml.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><configuration></configuration>");
                appConfigXml.Save(configPath);

                project.ProjectItems.AddFromFile(configPath);
                ExeConfigurationFileMap map = new ExeConfigurationFileMap { ExeConfigFilename = configPath };
                conf = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            }

            if (conf.AppSettings.Settings["BizTalkDbServer"] == null)
                conf.AppSettings.Settings.Add("BizTalkDbServer", sqlServerName);
            if (conf.AppSettings.Settings["BizTalkDTADb"] == null)
                conf.AppSettings.Settings.Add("BizTalkDTADb", btsDTADb);
            if (conf.AppSettings.Settings["BizTalkMgmtDb"] == null)
                conf.AppSettings.Settings.Add("BizTalkMgmtDb", btsMgmtDb);
            if (conf.AppSettings.Settings["TrackedMessageCopyJobName"] == null)
                conf.AppSettings.Settings.Add("TrackedMessageCopyJobName", "TrackedMessages_Copy_" + btsMgmtDb);
            if (conf.AppSettings.Settings["MaxHistory"] == null)
                conf.AppSettings.Settings.Add("MaxHistory", "100");

            if (conf.ConnectionStrings.ConnectionStrings["BizTalkDTADbEntities"] == null)
                conf.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("BizTalkDTADbEntities", String.Format("metadata=res://*/BizTalk.Entities.BizTalkDTAModel.csdl|res://*/BizTalk.Entities.BizTalkDTAModel.ssdl|res://*/BizTalk.Entities.BizTalkDTAModel.msl;provider=System.Data.SqlClient;provider connection string=\";Data Source={0};Initial Catalog={1};Integrated Security=True;MultipleActiveResultSets=True\"", sqlServerName, btsDTADb), "System.Data.EntityClient"));


            ServiceModelSectionGroup serviceModel = (ServiceModelSectionGroup)conf.GetSectionGroup("system.serviceModel");

            BasicHttpBindingElement oneWayBinding = new BasicHttpBindingElement("BasicHttpBinding_ITwoWayAsyncVoid");
            oneWayBinding.Security.Mode = BasicHttpSecurityMode.None;
            oneWayBinding.TextEncoding = Encoding.UTF8;
            oneWayBinding.AllowCookies = false;
            oneWayBinding.BypassProxyOnLocal = false;
            oneWayBinding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            oneWayBinding.MessageEncoding = WSMessageEncoding.Text;
            oneWayBinding.TransferMode = TransferMode.Buffered;
            oneWayBinding.UseDefaultWebProxy = true;
            oneWayBinding.ReaderQuotas.MaxDepth = 32;
            oneWayBinding.ReaderQuotas.MaxStringContentLength = 8192;
            oneWayBinding.ReaderQuotas.MaxArrayLength = 16384;
            oneWayBinding.ReaderQuotas.MaxBytesPerRead = 4096;
            oneWayBinding.ReaderQuotas.MaxNameTableCharCount = 16384;
            serviceModel.Bindings.BasicHttpBinding.Bindings.Add(oneWayBinding);

            BasicHttpBindingElement twoWayBinding = new BasicHttpBindingElement("BasicHttpBinding_ITwoWayAsync");
            twoWayBinding.Security.Mode = BasicHttpSecurityMode.None;
            twoWayBinding.TextEncoding = Encoding.UTF8;
            twoWayBinding.AllowCookies = false;
            twoWayBinding.BypassProxyOnLocal = false;
            twoWayBinding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            twoWayBinding.MessageEncoding = WSMessageEncoding.Text;
            twoWayBinding.TransferMode = TransferMode.Buffered;
            twoWayBinding.UseDefaultWebProxy = true;
            twoWayBinding.ReaderQuotas.MaxDepth = 32;
            twoWayBinding.ReaderQuotas.MaxStringContentLength = 8192;
            twoWayBinding.ReaderQuotas.MaxArrayLength = 16384;
            twoWayBinding.ReaderQuotas.MaxBytesPerRead = 4096;
            twoWayBinding.ReaderQuotas.MaxNameTableCharCount = 16384;
            serviceModel.Bindings.BasicHttpBinding.Bindings.Add(twoWayBinding);

            ChannelEndpointElement ep1Way = new ChannelEndpointElement(new EndpointAddress("http://localhost:9000/BizWTF.Testing.Ports.IN.OneWay"), "BizTalk1WayReference.ITwoWayAsyncVoid");
            ep1Way.Binding = "basicHttpBinding";
            ep1Way.Name = "BasicHttpBinding_ITwoWayAsyncVoid";
            ep1Way.BindingConfiguration = "BasicHttpBinding_ITwoWayAsyncVoid";
            serviceModel.Client.Endpoints.Add(ep1Way);
            ChannelEndpointElement ep2Way = new ChannelEndpointElement(new EndpointAddress("http://localhost:9000/BizWTF.Testing.Ports.IN.TwoWay"), "BizTalk2WayReference.ITwoWayAsync");
            ep2Way.Binding = "basicHttpBinding";
            ep2Way.Name = "BasicHttpBinding_ITwoWayAsync";
            ep2Way.BindingConfiguration = "BasicHttpBinding_ITwoWayAsync";
            serviceModel.Client.Endpoints.Add(ep2Way);

            //conf.SectionGroups.Add("system.serviceModel", serviceModel);

            conf.Save(ConfigurationSaveMode.Minimal, true);
        }
    }
}
