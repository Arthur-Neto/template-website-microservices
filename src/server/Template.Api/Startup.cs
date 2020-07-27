using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Template.Application.UsersModule;
using Template.Domain.UsersModule;
using Template.Infra.Data.EF.Context;
using Template.Infra.Data.EF.Repositories.UsersModule;

namespace Template.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDatabaseContext, ApiContext>();

            services.AddControllers();

            services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("Template"));

            services.AddMvc();

            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Template API");
            });

            //using (var serviceScope = app.ApplicationServices.CreateScope())
            //{
            //    var context = app.ApplicationServices.GetRequiredService<ApiContext>();
            //    SeedData(context);
            //}

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void SeedData(ApiContext context)
        {
            var user = new User()
            {
                ID = 1,
                Password = "123",
                Username = "admin"
            };

            context.Add(user);

            context.SaveChanges();
        }
    }
}
