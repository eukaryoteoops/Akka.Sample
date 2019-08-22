using Akka.Actor;
using Serilog;
using System;

namespace Akka.Sample.Actor
{
    public class LogActor : ReceiveActor
    {
        public LogActor(ILogger logger)
        {
            Receive<string>(o =>
            {
                Console.WriteLine($"{o} - {typeof(LogActor).FullName}");
                logger.Error($"{o} - {typeof(LogActor).FullName}");
            });
        }
    }
}
