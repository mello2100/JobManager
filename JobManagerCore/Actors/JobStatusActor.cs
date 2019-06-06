using Akka.Actor;
using JobManagerCore.SignalR;
using System;

namespace JobManagerCore.Actors
{
    public class JobsStatusActor : ReceiveActor
    {
        private string CurrentStatus { get; set; }

        public JobsStatusActor()
        {
            StatusHubClient.Conectar();

            StatusHubClient.SendMessage("Teste");

            Receive<UpdateJobStatusMessage>(msg =>
            {
                CurrentStatus = msg.NewStatus;
                StatusHubClient.SendMessage(CurrentStatus);
            });
        }

        public static Props CreateProps()
        {
            return Props.Create(() => new JobsStatusActor());
        }

        protected override void PreStart()
        {
            CurrentStatus = "Inicial";
            Console.WriteLine($"Starting JobsStatusActor - {CurrentStatus}");
        }

        #region Messages

        public class UpdateJobStatusMessage
        {
            public string NewStatus { get; }

            public UpdateJobStatusMessage(string newStatus)
            {
                NewStatus = newStatus;
            }
        }

        #endregion Messages
    }
}