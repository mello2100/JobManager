using Akka.Actor;
using Akka.Routing;

namespace JobManagerCore.Actors
{
    public class WorkersManagerActor : ReceiveActor
    {
        private IActorRef WorkersBroadcastPool { get; }

        private IActorRef JobManagerActor { get; }

        public WorkersManagerActor(IActorRef jobManagerActor)
        {
            WorkersBroadcastPool = Context.ActorOf(WorkerActor.CreateProps(jobManagerActor).WithRouter(new BroadcastPool(2)));
        }

        public static Props CreateProps(IActorRef jobManagerActor)
        {
            return Props.Create(() => new WorkersManagerActor(jobManagerActor));
        }
    }
}