using Akka.Sample.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Akka.Sample
{
    public partial class Startup
    {
        public void ConfigureActor(IServiceCollection services)
        {
            services.AddSingleton<IActorFactory, ActorFactory>(o => new ActorFactory(o));
        }
    }
}
