using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Filters;
using System.IO;

namespace Akka.Sample
{
    public partial class Startup
    {
        public void ConfigureLog(IServiceCollection services)
        {
            services.AddSingleton<ILogger>(o => CreateSerilog());
        }

        private ILogger CreateSerilog()
        {
            var config = new LoggerConfiguration()
                .MinimumLevel.Error()
                .Enrich.FromLogContext()
                .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                .WriteTo.RollingFile($"{Directory.GetCurrentDirectory()}/Log/log-{{Date}}.txt");
            return config.CreateLogger();
        }
    }
}
