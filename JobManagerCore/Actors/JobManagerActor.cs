using Akka.Actor;
using JobManagerCore.Models;
using System;

namespace JobManagerCore.Actors
{
    public class JobManagerActor : ReceiveActor
    {

        private IActorRef JobStatusActor { get; }

        public JobManagerActor(IActorRef jobStatusActor)
        {
            JobStatusActor = jobStatusActor;


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
            //JobStatusActor.Tell(new UpdateJobStatusMessage($"..................................{Sender.Path.Name} -> Notificação -> {jobId} / {message} / {tries}"));
        }

        private void ProcessRequestForJobMessage(WorkerActor.RequestForJobMessage msg)
        {
            Job job = new Job(Guid.NewGuid().ToString().Substring(0, 8), "job data");

            //TODO: remover
            Console.WriteLine($"{Sender.Path.Name} -> Solicitação -> {job.JobId} / {job.TryExecutionCount}");
            JobStatusActor.Tell(new JobStatusActor.UpdateJobStatusMessage($"{Sender.Path.Name} -> Solicitação -> {job.JobId} / {job.TryExecutionCount}"));

            Sender.Tell(job);
        }

        public static Props CreateProps(IActorRef jobStatusActor)
        {
            return Props.Create(() => new JobManagerActor(jobStatusActor));
        }

        #region Private methods

        #endregion Private methods
    }
}
