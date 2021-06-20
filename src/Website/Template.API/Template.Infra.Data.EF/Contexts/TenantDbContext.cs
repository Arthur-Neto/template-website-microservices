using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Template.Infra.Data.EF.Contexts
{
    public class TenantDbContext : DbContext, ITenantDbContext
    {
        public readonly string SchemaName;

        public TenantDbContext(DbContextOptions<TenantDbContext> options, IEnterpriseProvider entepriseProvider)
            : base(options)
        {
            SchemaName = entepriseProvider.GetSchemaName();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(SchemaName);

            if (Database.ProviderName.Equals(ProviderNames.Postgres))
            {
                modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(Postgres.UsersModule.UserConfiguration)));
            }
            else
            {
                modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(SqlServer.UsersModule.UserConfiguration)));
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
