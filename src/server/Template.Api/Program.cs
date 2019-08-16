using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Template.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateWebHostBuilder(args).Build().RunAsync();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                    .ConfigureServices(services => services.AddAutofac())
                    .UseStartup<Startup>();
    }
}
