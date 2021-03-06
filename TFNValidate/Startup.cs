using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using TFNValidate.API.Middleware;
using TFNValidate.Persistence;
using TFNValidate.Persistence.Implementation;
using TFNValidate.Persistence.Models;
using TFNValidate.Services;
using TFNValidate.Services.Implementation;
using TFNValidate.Services.Models;

namespace TFNValidate.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddTransient<ITfnValidator, TfnValidator>();
            services.TryAddTransient<IRateLimiter, RateLimiter>();
            services.TryAddTransient<ILinkedValueChecker, LinkedValueChecker>();
            services.TryAddTransient<IAttemptRepository, AttemptRepository>();
            services.AddOptions();
            services.Configure<RateLimitConfiguration>(Configuration.GetSection("RateLimitConfiguration"));
            services.AddDbContext<AttemptContext>(opt => opt.UseInMemoryDatabase("AttemptStore"));
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin().WithHeaders(HeaderNames.ContentType);
                });
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseRateLimiter();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}