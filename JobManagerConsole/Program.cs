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
                IActorRef jobStatusActor = RootActorSystem.ActorOf(JobStatusActor.CreateProps(), "jobStatusActor");
                IActorRef jobsManagerActor = RootActorSystem.ActorOf(JobManagerActor.CreateProps(jobStatusActor), "jobManagerActor");
                IActorRef workersManagerActor = RootActorSystem.ActorOf(WorkersManagerActor.CreateProps(jobsManagerActor), "workersManagerActor");
                Console.ReadKey();
            }
        }
    }
}
