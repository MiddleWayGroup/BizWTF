using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using EnvDTE;
using EnvDTE100;
using Microsoft.VisualStudio.TemplateWizard;

using BizWTF.Testing.VSDevTools.Messaging.Steps;

namespace BizWTF.Testing.VSDevTools.Messaging
{
    public class GenerateBindingFile : IWizard
    {
        void IWizard.BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        void IWizard.ProjectFinishedGenerating(Project project)
        {

        }

        void IWizard.ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            //throw new NotImplementedException();
        }

        void IWizard.RunFinished()
        {
        }

        void IWizard.RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            EnvDTE.DTE dte = automationObject as EnvDTE.DTE;
            Array projects = dte.ActiveSolutionProjects as Array;

            Form nextForm = new frmGenerateBinding((Project)projects.GetValue(0));
            nextForm.ShowDialog();

        }

        bool IWizard.ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
