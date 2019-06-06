using Akka.Actor;
using JobManagerCore.Actors;
using System;

namespace JobManagerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using (var RootActorSystem = ActorSystem.Create("rootActorSystem"))
            {
                IActorRef jobsStatusActor = RootActorSystem.ActorOf(JobsStatusActor.CreateProps(), "jobsStatusActor");
                IActorRef jobsManagerActor = RootActorSystem.ActorOf(JobManagerActor.CreateProps(jobsStatusActor), "jobManagerActor");
                IActorRef workersManagerActor = RootActorSystem.ActorOf(WorkersManagerActor.CreateProps(jobsManagerActor), "workersManagerActor");
                Console.ReadKey();
            }
        }
    }
}
