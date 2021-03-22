using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using TFNValidate.Persistence;
using TFNValidate.Persistence.Implementation;
using TFNValidate.Services;
using TFNValidate.Services.Implementation;

namespace TFNValidate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services.TryAddTransient<ITFNValidator, TFNValidator>();
                    services.TryAddTransient<IRateLimiter, RateLimiter>();
                    services.TryAddTransient<ILinkedNumberChecker, LinkedNumberChecker>();
                    services.TryAddTransient<IAttemptRepository, AttemptRepository>();
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}