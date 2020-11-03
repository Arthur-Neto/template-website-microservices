using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Infra.Data.EF.Context;


namespace Template.WebApi.Extensions
{
    public static class DatabaseContextExtensions
    {
        public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            if (bool.Parse(configuration["UseInMemory"]))
            {
                services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("Template"));
            }
            else
            {
                services.AddDbContext<ApiContext>(opt => opt.UseSqlServer(configuration["SqlServerConnectionString"]));
            }
        }

        public static void CreateSqlServerDatabase(this IApplicationBuilder app, IConfiguration configuration)
        {
            if (!bool.Parse(configuration["UseInMemory"]))
            {
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<ApiContext>();
                    context.Database.EnsureCreated();
                }
            }
        }
    }
}
