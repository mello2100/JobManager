using Akka.Actor;
using JobManagerCore.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JobManagerCore.Actors
{
    public class WorkerActor : ReceiveActor
    {
        private IActorRef JobManagerActor { get; }

        public WorkerActor(IActorRef jobManagerActor)
        {
            JobManagerActor = jobManagerActor;

            Receive<ReceivedJobMessage>(msg => ProcessReceivedJobMessage(msg));
            Receive<RememberToAskForJobMessage>(msg => ProcessRememberToAskForJobMessage());
            Receive<ExecutedJobMessage>(msg => ProcessExecutedJobMessage(msg));

            NotifyMyselfToAskForJob();
        }

        public static Props CreateProps(IActorRef jobManagerActor)
        {
            return Props.Create(() => new WorkerActor(jobManagerActor));
        }

        private void ProcessReceivedJobMessage(ReceivedJobMessage msg)
        {
            Task<ExecutedJobMessage> resultTask = ExecuteJob(msg);
            NotifyResultToMyself(resultTask);
        }

        private void ProcessRememberToAskForJobMessage()
        {
            Job job = AskForJob();

            if (IsValidJob(job))
            {
                Self.Tell(new ReceivedJobMessage(job));
                return;
            }

            ScheduleNotifyMyselfToAskForJob();
        }

        private void ProcessExecutedJobMessage(ExecutedJobMessage msg)
        {
            if (IsNecessaryToRetryJob(msg))
            {
                Job newJob = CreateNewJobWithIncrementedJobRetryCount(msg.JobExecutionResponse.Job);
                Self.Tell(new ReceivedJobMessage(newJob));
                return;
            }

            NotifyExecution(msg);
            NotifyMyselfToAskForJob();
        }

        #region Private methods

        private Task<ExecutedJobMessage> ExecuteJob(ReceivedJobMessage msg)
        {
            Job job = msg.Job;
            return Task.Run(() => ExecuteJob(job)).ContinueWith(t => new ExecutedJobMessage(t.Result));
        }

        private void NotifyResultToMyself(Task<ExecutedJobMessage> task)
        {
            task.PipeTo(Self);
        }

        private JobExecutionResponse ExecuteJob(Job job)
        {
            Thread.Sleep(2000);
            var executionResponseData = new JobExecutionResponseData(JobExecutionReponseStatus.OK, "{}", "OK");
            return new JobExecutionResponse(job, executionResponseData);
        }

        private bool IsNecessaryToRetryJob(ExecutedJobMessage msg)
        {
            JobExecutionReponseStatus jobExecutionReponseStatus = msg.JobExecutionResponse.JobExecutionResponseData.ExecutionStatus;
            int tryExecutionCount = msg.JobExecutionResponse.Job.TryExecutionCount;

            if (jobExecutionReponseStatus == JobExecutionReponseStatus.OK)
                return false;

            if (tryExecutionCount >= 3)
                return false;

            return true;
        }

        private bool IsValidJob(Job job)
        {
            if (string.IsNullOrEmpty(job.JobId))
                return false;

            return true;
        }

        private Job AskForJob()
        {
            return JobManagerActor.Ask<Job>(new RequestForJobMessage()).Result;
        }

        private void ScheduleNotifyMyselfToAskForJob()
        {
            Context.System.Scheduler.ScheduleTellOnce(TimeSpan.FromSeconds(60), Self, new RememberToAskForJobMessage(), Self);
        }

        private void NotifyMyselfToAskForJob()
        {
            Self.Tell(new RememberToAskForJobMessage());
        }

        private Job CreateNewJobWithIncrementedJobRetryCount(Job job)
        {
            return new Job(job.JobId, job.JobData, job.TryExecutionCount + 1);
        }

        private void NotifyExecution(ExecutedJobMessage msg)
        {
            JobManagerActor.Tell(msg);
        }

        #endregion Private methods

        #region Messages

        public class RequestForJobMessage
        {
            public RequestForJobMessage()
            {
            }
        }

        public class RememberToAskForJobMessage
        {
            public RememberToAskForJobMessage()
            {
            }
        }

        public class ReceivedJobMessage
        {
            public Job Job { get; }

            public ReceivedJobMessage(Job job)
            {
                Job = job;
            }
        }

        public class ExecutedJobMessage
        {
            public JobExecutionResponse JobExecutionResponse { get; }

            public ExecutedJobMessage(JobExecutionResponse jobExecutionResponse)
            {
                JobExecutionResponse = jobExecutionResponse;
            }
        }

        #endregion Messages
    }
}