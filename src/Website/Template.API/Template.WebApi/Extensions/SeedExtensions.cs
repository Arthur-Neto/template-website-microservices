using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Template.Domain.TenantsModule;
using Template.Domain.TenantsModule.Enums;
using Template.Infra.Data.EF.Contexts;
using Template.Security;

namespace Template.WebApi.Extensions
{
    public static class SeedExtensions
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<MasterDbContext>();
                SeedData(context);
            }
        }

        private static void SeedData(MasterDbContext context)
        {
            var salt = SecurityHelper.GenerateSalt();
            var tenant = new Tenant()
            {
                Logon = "admin",
                Salt = salt,
                Password = SecurityHelper.GenerateHash("123", salt),
                Role = Role.Manager
            };

            context.Add(tenant);

            context.SaveChanges();
        }
    }
}
