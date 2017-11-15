using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;

using BizWTF.Core.Entities;
using BizWTF.Core.BizTalk.Entities;
using BizWTF.Core.Utilities;

namespace BizWTF.Core.Tests.ActionSteps
{
    [Serializable]
    public class ForceTrackedMessageCopyJobStep : ActionStep
    {
        const int timeoutInterval = 60;

        bool loopContinuity = false;
        Timer stateTimer;
        int CurrentRunRetryAttempt = 0;
        ServerConnection conn;
        Server server;
        Job job;

        public ForceTrackedMessageCopyJobStep()
        { }

        public ForceTrackedMessageCopyJobStep(string stepName)
            : base(stepName)
        { }

        public override bool ExecuteStep()
        {
            this.AppendResultDescription(0, "Executing step [{0}] ({1}):", this.StepName, this.StepType);

            try
            {
                conn = new ServerConnection(BTSTestContext.BizTalkDbServer); //Create SQL server conn, Windows Authentication
                server = new Server(conn); //Connect SQL Server
                job = server.JobServer.Jobs[BTSTestContext.TrackedMessageCopyJobName]; //Get the specified job
                StartJob();

                this.AppendResultDescription(1, "[OK] {0} job executed successfully", BTSTestContext.TrackedMessageCopyJobName);
                this.Result = StepResult.OK;
            }
            catch (Exception ex)
            {
                this.AppendResultDescription(1, "[KO] Failed to start the job : {0}", ex.Message);
                this.Result = StepResult.Error;
            }
            finally
            {
                Destroyobjects();
            }

            return (this.Result == StepResult.OK);
        }




        void Destroyobjects()
        {
            if (job != null)
                job = null;
            if (server != null)
                server = null;
            if (conn != null)
            {
                conn.Disconnect();
                conn = null;
            }
        }

        void StartJob()
        {

            try
            {
                while (loopContinuity == false) //Wait till the job is idle
                {
                    job.Refresh();
                    if (job.CurrentRunStatus == JobExecutionStatus.Executing) //Check Job status and find if it’s running now
                    {
                        CurrentRunRetryAttempt++;
                        //We are not ready to fire the job
                        loopContinuity = false;
                    }
                    else
                    {
                        //We are ready to fire the job
                        loopContinuity = true; //Set loop exit
                        job.Start();//Start the job
                    }
                    System.Threading.Thread.Sleep(1 * 1000); //Fail to start, wait 10 seconds and try again
                    this.AppendResultDescription(1, "Job started. Current status={0}", job.CurrentRunStatus);

                }

                job.Refresh();
                while (job.CurrentRunStatus != JobExecutionStatus.Idle) //Wait till the job is idle
                {
                    System.Threading.Thread.Sleep(1 * 1000);
                    job.Refresh();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
