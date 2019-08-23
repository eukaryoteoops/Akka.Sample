using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
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
            var config = ConfigurationFactory.ParseString(HOCONConfig);
            var actorSystem = ActorSystem.Create("root", config);
            var logger = provider.GetService<ILogger>();
            //Register Actors
            //Mailbox type : UnboundedMailbox(default), UnboundedPriorityMailbox(need override PriorityGenerator())
            //Router type : new RoundRobinPool(5), new BroadcastPool(5), new RandomPool(5), new ConsistentHashingPool(5).WithHashMapping(o => key)
            Register(ActorNames.Demo, () => actorSystem.ActorOf(Props.Create<DemoActor>(logger).WithRouter(new SmallestMailboxPool(5)), ActorNames.Demo));
            //Deploy to target client, client project must ref the project where actor located.
            Register(ActorNames.Deploy, () => actorSystem.ActorOf(Props.Create<DeployActor>()
                .WithDeploy(Deploy.None.WithScope(new RemoteScope(Address.Parse("akka.tcp://SenderClient@localhost:8080")))), ActorNames.Deploy));
        }

        public IActorRef GetDemoActor() => Resolve(ActorNames.Demo);
        public IActorRef GetDeployActor() => Resolve(ActorNames.Deploy);


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

        //Human-Optimized Config Object Notation
        private string HOCONConfig => @"
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
                            }";
    }

    public static class ActorNames
    {
        public static string Demo => "demo";
        public static string Deploy => "deploy";
    }
}
