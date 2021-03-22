using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using TFNValidate.Persistence;
using TFNValidate.Services;

namespace TFNValidate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services.TryAddTransient<ITFNValidator, TFNValidator>();
                    services.TryAddTransient<IRateLimiter, RateLimiter>();
                    services.TryAddTransient<ILinkedNumberChecker, LinkedNumberChecker>();
                    services.TryAddTransient<IAttemptRepository, AttemptRepository>();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
