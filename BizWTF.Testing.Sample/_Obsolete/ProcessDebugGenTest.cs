using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;

using BizWTF.Testing.VSDevTools.Processing;
using BizWTF.Testing.VSDevTools.Processing.Steps;

namespace BizWTF.Testing.Sample
{
    [TestClass]
    public class ProcessDebugGenTest
    {
        [TestMethod]
        public void WIZARDS_GenerateOrchestrationScenario()
        {
            Form frm = new frmProcessDebugGen(@"C:\Temp\BizWTF\Exported", "MyTimeline.xml");
            frm.ShowDialog();
        }
    }
}
