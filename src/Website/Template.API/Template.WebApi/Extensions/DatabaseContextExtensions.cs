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
    // TODO: REVER
    public static class DatabaseContextExtensions
    {
        public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            //Action<DbContextOptionsBuilder> action = (DbContextOptionsBuilder builder) => builder.UseInMemoryDatabase("TemplateMaster"); ;
            if (bool.Parse(configuration["UseInMemory"]))
            {
                //action = (DbContextOptionsBuilder builder) => builder.UseInMemoryDatabase("TemplateMaster");
                //services.AddDbContext<TenantDbContext>(opt => action.BuildDatabaseContext(opt));
            }
            else
            {
                if (bool.Parse(configuration["UseMultiTenantDatabase"]))
                {
                    services.AddEntityFrameworkNpgsql()
                        .AddDbContext<MasterDbContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("Postgres")))
                        .AddDbContext<TenantDbContext>((serviceProvider, options) => options
                            .UseNpgsql(configuration.GetConnectionString("Postgres"))
                            .UseInternalServiceProvider(serviceProvider));
                }
                else
                {
                    //action = (DbContextOptionsBuilder builder) => builder.UseSqlServer(configuration.GetConnectionString("SqlServer"));
                    //services.AddDbContext<TenantDbContext>(opt => action.BuildDatabaseContext(opt));
                }
            }

            //services.AddDbContext<MasterDbContext>(opt => action.BuildDatabaseContext(opt));

            services.Replace(ServiceDescriptor.Singleton<IModelCacheKeyFactory, MultiTenantModelCacheKeyFactory>());
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
