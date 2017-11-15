using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.EntityClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using BizWTF.Core;
using BizWTF.Core.Tests.GetSteps;
using BizWTF.Core.Tests.TestSteps;
using BizWTF.Core.Entities;
using BizWTF.Core.Entities.ProcessValidation;

namespace BizWTF.Testing.VSDevTools.Processing.Steps
{
    public partial class frmProcessDebugGen : Form
    {
        protected EnvDTE.Project _project;
        protected string _targetFolder;
        protected string _requestedFileName;

        public frmProcessDebugGen(EnvDTE.Project targetProject, string requestedName)
        {
            InitializeComponent();

            this._project = targetProject;
            this._requestedFileName = requestedName;
            this._targetFolder = new FileInfo(this._project.FullName).DirectoryName + @"\Processes\";

            if (!Directory.Exists(this._targetFolder))
            {
                Directory.CreateDirectory(this._targetFolder);
                this._project.ProjectItems.AddFolder("Processes", null);
            }
        }

        public frmProcessDebugGen(string targetFolder, string requestedName)
        {
            InitializeComponent();

            this._targetFolder = targetFolder;
            this._requestedFileName = requestedName;
        }

        private void btnGetOrch_Click(object sender, EventArgs e)
        {
            string connStr = String.Empty;
            string sqlMgmtConnStr = String.Empty;
            try
            {
                string mgmtDBName = string.Empty;
                Tools.GetConnectionStrings(this.txtDTAServerName.Text, this.txtDTADBName.Text, mgmtDBName, out connStr, out sqlMgmtConnStr);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Unable to connect to DTA Db : " + exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                foreach (OrchestrationInfo info in BTSOrchestrationExplorer.GetDeployedOrchestrations(connStr))
                {
                    this.dgOrchestration.Rows.Add(info.ServiceName, info.AssemblyName, info.TxtSymbols);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error when retrieving orchestrations", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgOrchestration_SelectionChanged(object sender, EventArgs e)
        {
            this.dgDebugShapes.Rows.Clear();

            if (this.dgOrchestration.SelectedRows.Count > 0)
            {
                this.dgDebugShapes.Enabled = true;
                this.btnGenerate.Enabled = true;

                string serviceName = (string)this.dgOrchestration.SelectedRows[0].Cells["ServiceName"].Value;
                string debugSymbols = (string)this.dgOrchestration.SelectedRows[0].Cells["DebugSymbols"].Value;
                DebugTrace trace = BTSOrchestrationExplorer.GetDebugTrace(serviceName, debugSymbols, FlatteningPrefixType.Indentation);

                foreach (DebugShape shape in trace.TraceDetails)
                {
                    this.dgDebugShapes.Rows.Add(
                            shape.shapeText,
                            shape.shapeType.ToString(),
                            "true",
                            "true",
                            "1",
                            shape.ShapeID.ToString());
                }
            }
        }

        private void dgDebugShapes_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 4)
                {
                    string textValue = (string)dgDebugShapes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    int outputValue = 0;
                    if (!Int32.TryParse(textValue, out outputValue))
                        dgDebugShapes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "0";
                }

                if (e.ColumnIndex == 3)
                {
                    if (dgDebugShapes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "false")
                        dgDebugShapes.Rows[e.RowIndex].DefaultCellStyle = new DataGridViewCellStyle
                        {
                            BackColor = Color.LightGray,
                            ForeColor = Color.Gray
                        };
                    else
                        dgDebugShapes.Rows[e.RowIndex].DefaultCellStyle = new DataGridViewCellStyle { };
                }
            }
        }



        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (this.dgOrchestration.SelectedRows.Count > 0)
            {
                DataGridViewRow dr = this.dgOrchestration.SelectedRows[0];

                string serviceName = (string)dr.Cells["ServiceName"].Value;
                string debugSymbols = (string)dr.Cells["DebugSymbols"].Value;

                string destPath;
                destPath = Path.Combine(this._targetFolder, this._requestedFileName);
                FileInfo fi = new FileInfo(destPath);

                int i = 0;
                while (fi.Exists)
                {
                    i++;
                    destPath = Path.Combine(this._targetFolder,
                                String.Format("{0}-{2}{1}", fi.Name.Replace(fi.Extension, ""), fi.Extension, i));
                    fi = new FileInfo(destPath);
                }

                DebugTrace originalTrace = BTSOrchestrationExplorer.GetDebugTrace(serviceName, debugSymbols, FlatteningPrefixType.ParentName);
                DebugTrace trace = new DebugTrace();
                foreach (DataGridViewRow row in this.dgDebugShapes.Rows)
                {
                    if (Boolean.Parse((string)row.Cells["AddToScenario"].Value))
                    {
                        DebugShape shape = originalTrace.TraceDetails.Single(s => s.ShapeID.ToString() == (string)row.Cells["ShapeID"].Value);
                        shape.Completed = Boolean.Parse((string)row.Cells["MustComplete"].Value);
                        shape.RepeatCount = Int32.Parse((string)row.Cells["RepeatCount"].Value);
                        trace.TraceDetails.Add(shape);
                    }
                }
                trace.Export(destPath);

                if (this._project != null)
                {
                    EnvDTE.ProjectItem item = this._project.ProjectItems.AddFromFile(destPath);

                    EnvDTE.Property prop = item.Properties.Item("BuildAction");
                    prop.Value = 2;

                    Tools.AddConfigKeys(this._project, this.txtDTAServerName.Text, this.txtDTADBName.Text, "BizTalkMgmtDb");
                }

                this.Close();
            }
        }


    }
}
