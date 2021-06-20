using Microsoft.Extensions.DependencyInjection;

namespace Template.Security
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSecurityHelpers(this IServiceCollection services)
        {
            services.AddScoped<IHashing, Hashing>();
            services.AddScoped<IJwtTokenFactory, JwtTokenFactory>();
        }
    }
}
