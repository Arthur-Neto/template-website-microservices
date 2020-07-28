using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Template.Application.UsersModule;
using Template.Domain.UsersModule;
using Template.Infra.Data.EF.Context;
using Template.Infra.Data.EF.Repositories.UsersModule;

namespace Template.Api.Extensions
{
    public static class DependencyExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDatabaseContext, ApiContext>();

            services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("Template"));

            return services;
        }
    }
}
