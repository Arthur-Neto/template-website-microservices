using System;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.UriParser;
using Template.Application;
using Template.WebApi.Extensions;

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
            services.AddCors(option =>
                option.AddPolicy("TemplateCorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:4200");
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowCredentials();
                })
            );

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });

            services.ConfigAuthorization(Configuration);

            services.AddDependencies();

            services.AddDatabaseContext(Configuration);

            services.AddODataConfig();

            services.AddSwagger();

            services.AddAutoMapper(typeof(IUnitOfWork));

            services.AddControllersWithViews()
                    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<IUnitOfWork>());

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
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

            app.CreateSqlServerDatabase(Configuration);

            app.UseCors("TemplateCorsPolicy");

            app.ConfigExceptionHandler();

            app.SeedData();

            app.ConfigSwagger();

            app.UseStaticFiles();

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.EnableDependencyInjection(builder =>
                {
                    builder.AddService(Microsoft.OData.ServiceLifetime.Singleton, typeof(ODataUriResolver), sp => new StringAsEnumResolver());
                });
                endpoints.MapControllers();
                endpoints.MapODataEndpoints();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                spa.ApplicationBuilder.UseCors("TemplateCorsPolicy");
            });
        }
    }
}
