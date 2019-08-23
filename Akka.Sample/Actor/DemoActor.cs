using Akka.Actor;
using Serilog;
using System;

namespace Akka.Sample.Actor
{
    public class DemoActor : ReceiveActor
    {
        public DemoActor(ILogger logger)
        {
            Receive<string>(o =>
            {
                Console.WriteLine($"{o} - {typeof(DemoActor).FullName} - {Context.Self.Path}");
                //logger.Error($"{o} - {typeof(LogActor).FullName}");
            });
        }
        /// <summary>
        ///     Actor exception handle
        /// </summary>
        /// <returns></returns>
        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(maxNrOfRetries: 10, withinTimeRange: TimeSpan.FromSeconds(10),
                    localOnlyDecider: ex =>
                    {
                        switch (ex)
                        {
                            case ArithmeticException ae:
                                return Directive.Resume;
                            case NullReferenceException nre:
                                return Directive.Restart;
                            case ArgumentException are:
                                return Directive.Stop;
                            default:
                                return Akka.Actor.SupervisorStrategy.DefaultStrategy.Decider.Decide(ex);
                        }
                    });
        }
    }
}
