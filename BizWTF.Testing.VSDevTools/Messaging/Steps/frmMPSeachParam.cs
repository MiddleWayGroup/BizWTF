using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.EntityClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Xml;
using Microsoft.BizTalk.Operations;
using Microsoft.BizTalk.Message.Interop;

using Microsoft.XLANGs.BaseTypes;

using BizWTF.Core.BizTalk.Entities;
using BizWTF.Core.Entities;
using BizWTF.Core.Entities.Mocking;
using BizWTF.Core.Entities.ProcessValidation;
using BizWTF.Core.Tests;

namespace BizWTF.Testing.VSDevTools.Messaging.Steps
{
    public partial class frmMPSeachParam : Form
    {
        protected EnvDTE.Project _project;
        protected string _targetFolder;

        public frmMPSeachParam(EnvDTE.Project targetProject)
        {
            InitializeComponent();
            this.dtFrom.Value = DateTime.Now.Subtract(new TimeSpan(14, 0, 0, 0));

            this._project = targetProject;
            this._targetFolder = new FileInfo(this._project.FullName).DirectoryName + @"\Messages\";

            if (!Directory.Exists(this._targetFolder))
            {
                Directory.CreateDirectory(this._targetFolder);
                this._project.ProjectItems.AddFolder("Messages", null);
            }
        }

        public frmMPSeachParam(string targetFolder)
        {
            InitializeComponent();
            this.dtFrom.Value = DateTime.Now.Subtract(new TimeSpan(14, 0, 0, 0));

            this._targetFolder = targetFolder;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                this.dgProps.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void btnDTATest_Click(object sender, EventArgs e)
        {
            try
            {
                string connStr = String.Empty;
                this.testConnection(out connStr, true);

                
            }
            catch(Exception exc)
            {
                MessageBox.Show("Unable to connect to DTA Db : " + exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //entities.
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgMessages.Rows.Clear();

            List<ControlProperty> contextProps = new List<ControlProperty>();
            List<string> serviceNames = new List<string>();
            foreach (DataGridViewRow dr in this.dgProps.Rows)
            {
                if (dr.Cells[0].Value != null)
                {
                    //ControlProperty cp = new ControlProperty();
                    //cp.Namespace = (string)dr.Cells[1].Value;
                    //cp.Property = (string)dr.Cells[0].Value;
                    //cp.Value = (string)dr.Cells[2].Value;

                    //contextProps.Add(cp);
                    serviceNames.Add((string) dr.Cells[0].Value);
                }
            }

            string entityConnectionStr = String.Empty;
            this.testConnection(out entityConnectionStr, false);

            using (BizTalkDTADbEntities btsDTA = new BizTalkDTADbEntities(entityConnectionStr))
            {
                TrackingDatabase dta = new TrackingDatabase(this.txtDTAServerName.Text,
                                                            this.txtDTADBName.Text);
                BizTalkOperations operations = new BizTalkOperations(this.txtDTAServerName.Text, this.txtMgmtDBName.Text);

                var msgIds = from mioe in btsDTA.dta_MessageInOutEvents
                             join si in btsDTA.dta_ServiceInstances on mioe.uidServiceInstanceId equals si.uidServiceInstanceId
                             join s in btsDTA.dta_Services on si.uidServiceId equals s.uidServiceId
                             join pn in btsDTA.dta_PortName on mioe.nPortId equals pn.nPortId
                             join sn in btsDTA.dta_SchemaName on mioe.nSchemaId equals sn.nSchemaId
                             where mioe.dtTimestamp > this.dtFrom.Value
                              && mioe.dtTimestamp < this.dtTo.Value
                              && serviceNames.Contains(s.strServiceName)
                             orderby mioe.dtInsertionTimeStamp descending
                             select new tempMsgInfo
                             {
                                strServiceName = s.strServiceName
                                ,uidMessageInstanceId = mioe.uidMessageInstanceId
                                ,strPortName = pn.strPortName
                                ,dtTimestamp = mioe.dtTimestamp
                                ,nStatus = mioe.nStatus
                                ,strSchemaName = sn.strSchemaName
                             };

                ControlProperty tempCP = contextProps.Find(cp =>
                                                cp.Namespace == BizWTF.Core.Entities.BTSProperties.messageType.Name.Namespace
                                                && 
                                                cp.Property == BizWTF.Core.Entities.BTSProperties.messageType.Name.Name);
                if (tempCP != null)
                {
                    msgIds = msgIds.Where(rec => rec.strSchemaName == tempCP.Value);
                }

                /// Looping through all the messages in order to identify the one we are looking for
                foreach (var msg in msgIds.Take(Int32.Parse(this.txtMaxRecord.Text)))
                {
                    bool match = MatchingPartInfo.PerformMessageLookup(msg.uidMessageInstanceId, contextProps, new List<ControlField>(), dta, operations);

                    if (match)
                    {   
                        string status = String.Empty;
                        switch (msg.nStatus)
                        {
                            case 0:
                                status = "Receive";
                                break;
                            case 1:
                                status = "Send";
                                break;
                            case 5:
                                status = "Transmission Failure";
                                break;
                            default :
                                status = "?";
                                break;
                        }
                        this.dgMessages.Rows.Add(status, msg.strServiceName, msg.strPortName, msg.strSchemaName, msg.dtTimestamp, msg.uidMessageInstanceId);
                    }
                }
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            //string targetDirectory = new FileInfo(this._project.FullName).DirectoryName;

            BizTalkOperations operations = new BizTalkOperations(this.txtDTAServerName.Text, this.txtMgmtDBName.Text);
            TrackingDatabase dta = new TrackingDatabase(this.txtDTAServerName.Text,
                                                            this.txtDTADBName.Text);

            if (this.dgMessages.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in this.dgMessages.SelectedRows)
                {
                    IBaseMessage btsMessage = operations.GetTrackedMessage((Guid)row.Cells["InstanceID"].Value, dta);
                    MultipartMessageDefinition tempMsg = MultipartMessageManager.GenerateFromMessage(btsMessage);

                    string destPath = System.IO.Path.Combine(this._targetFolder,
                            String.Format("{0}-{1}-{2}.xml",
                                            row.Cells["EventType"].Value,
                                            row.Cells["PortName"].Value,
                                            row.Cells["InstanceID"].Value));
                    MultipartMessageSerializer.Serialize(tempMsg).Save(destPath);

                    if (this._project != null)
                    {
                        EnvDTE.ProjectItem item = this._project.ProjectItems.AddFromFile(destPath);

                        EnvDTE.Property prop = item.Properties.Item("BuildAction");
                        prop.Value = 2;
                    }
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select one or more messages to generate.", "No message selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private void testConnection(out string entityConnStr, bool displayAlert)
        {
            string sqlMgmtConnStr = string.Empty;
            Tools.GetConnectionStrings(this.txtDTAServerName.Text, this.txtDTADBName.Text, this.txtMgmtDBName.Text, out entityConnStr, out sqlMgmtConnStr);
            
            using (EntityConnection conn =
                    new EntityConnection(entityConnStr))
            {
                conn.Open();
                if (displayAlert)
                    MessageBox.Show("Connected to DTA Db sucessfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.Close();
            }

            using (SqlConnection conn =
                    new SqlConnection(sqlMgmtConnStr))
            {
                conn.Open();
                if (displayAlert)
                    MessageBox.Show("Connected to Mgmt Db sucessfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.Close();
            }
        }


        private class tempMsgInfo
        {
            public string strServiceName{get; set;}
            public Guid uidMessageInstanceId{get; set;}
            public string strPortName{get; set;}
            public DateTime dtTimestamp{get; set;}
            public int nStatus{get; set;}
            public string strSchemaName { get; set; }
        }
    }
}
