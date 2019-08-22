using Akka.Actor;
using Akka.Configuration;
using Akka.Sample.Actor;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;

namespace Akka.Sample.Common
{
    public class ActorFactory : IActorFactory
    {
        private readonly Dictionary<object, Lazy<IActorRef>> _Actors = new Dictionary<object, Lazy<IActorRef>>();

        public ActorFactory(IServiceProvider provider)
        {
            var config = ConfigurationFactory.ParseString(@"
                            akka {  
                                actor {
                                    provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
                                }
                                remote {
                                    helios.tcp {
                                        transport-class = ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
                                        applied-adapters = []
                                        transport-protocol = tcp
                                        port = 8081
                                        hostname = localhost
                                    }
                                }
                            }");
            var actorSystem = ActorSystem.Create("root", config);
            var logger = provider.GetService<ILogger>();
            //Register Actors
            Register(ActorNames.Log, () => actorSystem.ActorOf(Props.Create<LogActor>(logger), ActorNames.Log));
        }

        public IActorRef GetLogActor() => Resolve(ActorNames.Log);

        private void Register(object key, Func<IActorRef> actorFactory)
        {
            _Actors.Add(key, new Lazy<IActorRef>(actorFactory));
        }

        private IActorRef Resolve(object key)
        {
            if (!_Actors.ContainsKey(key))
                throw new Exception($"No actor found for key '{key}'.");

            return _Actors[key].Value;
        }
    }

    public static class ActorNames
    {
        public static string Log => "log";
    }
}
