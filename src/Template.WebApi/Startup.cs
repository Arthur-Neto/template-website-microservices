using System;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OData.Swagger.Services;
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

            services.AddControllers()
                    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<IUnitOfWork>());

            services.AddODataConfig();

            services.AddSwagger();

            services.AddOdataSwaggerSupport();

            services.AddAutoMapper(typeof(IUnitOfWork));

            services.AddMediatR(typeof(IUnitOfWork));

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

            app.UseStaticFiles();

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.ConfigSwagger();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                spa.ApplicationBuilder.UseCors("TemplateCorsPolicy");
            });
        }
    }
}