using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Template.Infra.Data.EF.Contexts;

namespace Template.WebApi.Extensions
{
    public static class DatabaseContextExtensions
    {
        public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            if (bool.Parse(configuration["UseInMemory"]))
            {
                services.AddEntityFrameworkInMemoryDatabase()
                    .AddDbContext<MasterDbContext>(opt => opt.UseInMemoryDatabase("Template"))
                    .AddDbContext<TenantDbContext>((serviceProvider, options) => options
                        .UseInMemoryDatabase("Template")
                        .UseInternalServiceProvider(serviceProvider));
                return;
            }

            if (bool.Parse(configuration["UseMultiTenantDatabase"]))
            {
                if (bool.Parse(configuration["UseSQLServer"]))
                {
                    services.AddEntityFrameworkSqlServer()
                        .AddDbContext<MasterDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("SqlServer")))
                        .AddDbContext<TenantDbContext>((serviceProvider, options) => options
                            .UseSqlServer(configuration.GetConnectionString("SqlServer"))
                            .UseInternalServiceProvider(serviceProvider));
                }
                else
                {
                    services.AddEntityFrameworkNpgsql()
                        .AddDbContext<MasterDbContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("Postgres")))
                        .AddDbContext<TenantDbContext>((serviceProvider, options) => options
                            .UseNpgsql(configuration.GetConnectionString("Postgres"))
                            .UseInternalServiceProvider(serviceProvider));
                }

                services.Replace(ServiceDescriptor.Singleton<IModelCacheKeyFactory, MultiTenantModelCacheKeyFactory>());
            }
            else
            {
                if (bool.Parse(configuration["UseSQLServer"]))
                {
                    services.AddEntityFrameworkSqlServer()
                        .AddDbContext<MasterDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("SqlServer")))
                        .AddDbContext<TenantDbContext>((serviceProvider, options) => options
                            .UseSqlServer(configuration.GetConnectionString("SqlServer"))
                            .UseInternalServiceProvider(serviceProvider));
                }
                else
                {
                    services.AddEntityFrameworkNpgsql()
                        .AddDbContext<MasterDbContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("Postgres")))
                        .AddDbContext<TenantDbContext>((serviceProvider, options) => options
                            .UseNpgsql(configuration.GetConnectionString("Postgres"))
                            .UseInternalServiceProvider(serviceProvider));
                }
            }
        }

        public static void BuildDatabaseContext(this Action<DbContextOptionsBuilder> action, DbContextOptionsBuilder builder)
        {
            action.Invoke(builder);
        }

        public static void CreateSqlServerDatabase(this IApplicationBuilder app, IConfiguration configuration)
        {
            if (bool.Parse(configuration["UseInMemory"]) is false)
            {
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<MasterDbContext>();
                    context.Database.EnsureCreated();
                }
            }
        }
    }
}
