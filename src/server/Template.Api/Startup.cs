using System.Reflection;
using Autofac;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Api.DependencyResolution;
using Template.Api.Filters;
using Template.Application.FeatureExampleModule.Models.Commands;
using Template.Application.FeatureExampleModule.Profiles;
using Template.Infra.Data.EF.Context;

namespace Template.Api
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
            services.AddMvcCore(
                config =>
                {
                    config.Filters.Add(new CheckInvalidIdOnRouteFilterAttribute());
                })
                .AddJsonFormatters()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<FeatureExampleAddCommandCommandValidator>());

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options =>
                {
                    options.AllowAnyOrigin();
                    options.AllowAnyHeader();
                    options.AllowAnyMethod();
                });
            });
            services.AddDbContext<ExampleContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Example_Database")));
            services.AddAutoMapper(Assembly.GetAssembly(typeof(FeatureExampleProfile)));
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModule());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyHeader();
                options.AllowAnyMethod();
            });
            app.UseMvc();
        }
    }
}
