using Akka.Actor;
using System;

namespace Akka.Sample.Actor
{
    public class DeployActor : ReceiveActor
    {
        private ICancelable _task;
        public DeployActor()
        {
            Receive<string>(o =>
            {
                Console.WriteLine($"{o} - {typeof(DeployActor).FullName} - {Context.Self.Path}");
            });
        }

        protected override void PreStart()
        {
            _task = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(3), Context.Self, "deployedMessage", ActorRefs.NoSender);
        }

        protected override void PostStop()
        {
            _task.Cancel();
        }

    }
}
