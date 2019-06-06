using Akka.Actor;
using JobManagerCore.Models;
using System;

namespace JobManagerCore.Actors
{
    public class JobManagerActor : ReceiveActor
    {

        private IActorRef JobsStatusActor { get; }

        public JobManagerActor(IActorRef jobsStatusActor)
        {
            JobsStatusActor = jobsStatusActor;


            Receive<WorkerActor.RequestForJobMessage>(msg => ProcessRequestForJobMessage(msg));
            Receive<WorkerActor.ExecutedJobMessage>(msg => ProcessExecutedJobMessage(msg));
        }

        private void ProcessExecutedJobMessage(WorkerActor.ExecutedJobMessage msg)
        {
            var jobId = msg.JobExecutionResponse.Job.JobId;
            var message = msg.JobExecutionResponse.JobExecutionResponseData.Message;
            var tries = msg.JobExecutionResponse.Job.TryExecutionCount;

            //TODO: remover
            //Console.WriteLine($"..................................{Sender.Path.Name} -> Notificação -> {jobId} / {message} / {tries}");
            //JobsStatusActor.Tell(new UpdateJobStatusMessage($"..................................{Sender.Path.Name} -> Notificação -> {jobId} / {message} / {tries}"));
        }

        private void ProcessRequestForJobMessage(WorkerActor.RequestForJobMessage msg)
        {
            Job job = new Job(Guid.NewGuid().ToString().Substring(0, 8), "job data");

            //TODO: remover
            Console.WriteLine($"{Sender.Path.Name} -> Solicitação -> {job.JobId} / {job.TryExecutionCount}");
            JobsStatusActor.Tell(new JobsStatusActor.UpdateJobStatusMessage($"{Sender.Path.Name} -> Solicitação -> {job.JobId} / {job.TryExecutionCount}"));

            Sender.Tell(job);
        }

        public static Props CreateProps(IActorRef jobsStatusActor)
        {
            return Props.Create(() => new JobManagerActor(jobsStatusActor));
        }

        #region Private methods

        #endregion Private methods
    }
}
