using Akka.Actor;
using Akka.Configuration;
using System;
using System.Threading;

namespace Akka.Sender
{
    class Program
    {
        static void Main(string[] args)
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
                                    port = 8080
                                    hostname = localhost
                                }
                            }
                        }");
            using (var system = ActorSystem.Create("SenderClient", config))
            {
                var greeting = system.ActorSelection("akka.tcp://root@localhost:8081/user/demo");
                while (true)
                {
                    greeting.Tell("FromRemote");
                    Console.WriteLine("Sleep 3 sec");
                    Thread.Sleep(3000);
                }
            }
        }
    }
}
