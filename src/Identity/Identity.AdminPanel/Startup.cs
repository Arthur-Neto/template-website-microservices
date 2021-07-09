using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrchardCore.Logging;


namespace Identity.AdminPanel
{
    public class IdentityContext : DbContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        { }
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOrchardCms()
                .ConfigureServices(services =>
                {
                    services.AddDbContext<IdentityContext>(options =>
                    {
                        options.UseSqlServer("Server=ARTHUR-PC\\MSSQLSERVER2019;Database=TemplateIdentity;Trusted_Connection=True;MultipleActiveResultSets=true");
                        options.UseOpenIddict();
                    });
                    services.AddOpenIddict()
                        .AddCore()
                        .UseEntityFrameworkCore()
                        .UseDbContext<IdentityContext>();
                }, order: 10000);
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseOrchardCore(c => c.UseSerilogTenantNameLogging());
        }
    }
}
