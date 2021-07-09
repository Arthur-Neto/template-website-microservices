using System;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OData.Swagger.Services;
using Template.Application;
using Template.Identity;
using Template.Security;
using Template.Serilog;
using Template.WebApi.Extensions;
using Template.WebApi.Middlewares;

namespace Template.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterLogger(dispose: true);

            services.AddCors(option =>
                option.AddPolicy("TemplateCorsPolicy", builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.SetIsOriginAllowed(origin => true);
                    builder.AllowCredentials();
                })
            );

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });

            services.AddSecurityHelpers();

            // services.ConfigAuthentication(Configuration);

            services.ConfigureIdentity("template", "901564A5-E7FE-42CB-B10D-61EF6A8F3654", "/api/public/login");

            services.AddDependencies();

            services.AddDatabaseContext(Configuration);

            services.AddControllers()
                    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(AppHandlerBase<>)));

            services.AddODataConfig();

            services.AddSwagger();

            services.AddOdataSwaggerSupport();

            services.AddAutoMapper(typeof(AppHandlerBase<>));

            services.AddMediatR(typeof(AppHandlerBase<>));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMiddleware<TenantMiddleware>();

            app.ConfigExceptionHandler();

            app.UseRouting();

            app.UseCors("TemplateCorsPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.ConfigSwagger();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
