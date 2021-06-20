using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Template.Serilog
{
    public static class SerilogExtensions
    {
        public static void RegisterLogger(this IServiceCollection services, IConfiguration configuration = null, bool dispose = false)
        {
            if (configuration == null)
            {
                configuration = new ConfigurationBuilder()
                    .AddJsonFile("serilogsettings.json")
                    .AddJsonFile($"serilogsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
                    .Build();
            }

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: dispose));
        }
    }
}
