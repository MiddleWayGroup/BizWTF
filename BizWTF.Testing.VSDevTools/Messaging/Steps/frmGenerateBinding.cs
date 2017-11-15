using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace BizWTF.Testing.VSDevTools.Messaging.Steps
{
    public partial class frmGenerateBinding : Form
    {
        protected EnvDTE.Project _project;
        protected string _targetFolder;

        public frmGenerateBinding(EnvDTE.Project targetProject)
        {
            InitializeComponent();

            this.cbType.SelectedIndex = 0;

            this._project = targetProject;
            this._targetFolder = new FileInfo(this._project.FullName).DirectoryName + @"\Bindings\";

            if (!Directory.Exists(this._targetFolder))
            {
                Directory.CreateDirectory(this._targetFolder);
                this._project.ProjectItems.AddFolder("Bindings", null);
            }
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbType.SelectedIndex > 1)
            {
                this.txtLocationName.Enabled = false;
            }
            else
            {
                this.txtLocationName.Enabled = true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string resourceName = String.Empty;
            string bindingTemplateContent = String.Empty;
            switch (cbType.SelectedIndex)
            {
                case 0:
                    resourceName = "BizWTF.Testing.VSDevTools.Messaging.Templates.BizWTF.Bindings.Template.Receive.1Way.xml";
                    break;
                case 1:
                    resourceName = "BizWTF.Testing.VSDevTools.Messaging.Templates.BizWTF.Bindings.Template.Receive.2Way.xml";
                    break;
                case 2:
                    resourceName = "BizWTF.Testing.VSDevTools.Messaging.Templates.BizWTF.Bindings.Template.Send.1Way.xml";
                    break;
                case 3:
                    resourceName = "BizWTF.Testing.VSDevTools.Messaging.Templates.BizWTF.Bindings.Template.Send.2Way.xml";
                    break;
            }


            using (Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                bindingTemplateContent = reader.ReadToEnd();

                bindingTemplateContent = bindingTemplateContent.Replace("$PortName$", this.txtPortName.Text);
                bindingTemplateContent = bindingTemplateContent.Replace("$LocationName$", this.txtLocationName.Text);
                bindingTemplateContent = bindingTemplateContent.Replace("$URIPort$", this.txtURIPort.Text);
                bindingTemplateContent = bindingTemplateContent.Replace("$URIPath$", this.txtURIPath.Text);

                string destPath = System.IO.Path.Combine(this._targetFolder,
                            String.Format("{0}.BindingInfo.xml",
                                            this.txtPortName.Text));

                using (StreamWriter outputFile = new StreamWriter(destPath))
                {
                    outputFile.Write(bindingTemplateContent);
                }

                if (this._project != null)
                {
                    EnvDTE.ProjectItem item = this._project.ProjectItems.AddFromFile(destPath);

                    EnvDTE.Property prop = item.Properties.Item("BuildAction");
                    prop.Value = 2;

                    Tools.AddConfigKeys(this._project, "(local)", "BizTalkDTADb", "BizTalkMgmtDb");
                }
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            this.Close();

        }
    }
}
