using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Template.Domain.UsersModule;
using Template.Infra.Data.EF.Context;

namespace Template.Api.Extensions
{
    public static class SeedExtensions
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApiContext>();
                SeedData(context);
            }
        }

        private static void SeedData(ApiContext context)
        {
            var user = new User()
            {
                Username = "admin",
                Password = "123"
            };

            context.Add(user);

            context.SaveChanges();
        }
    }
}
