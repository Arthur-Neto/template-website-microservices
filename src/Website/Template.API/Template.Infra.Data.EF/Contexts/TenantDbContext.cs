using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Template.Infra.Data.EF.Contexts
{
    public class TenantDbContext : DbContext, ITenantDbContext
    {
        public readonly string ConnectionString;

        public TenantDbContext(DbContextOptions<TenantDbContext> options, IEnterpriseProvider entepriseProvider)
            : base(options)
        {
            ConnectionString = entepriseProvider.GetTenantConnectionString();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (Database.ProviderName.Equals(ProviderNames.Postgres))
            {
                modelBuilder.HasDefaultSchema(ConnectionString);

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
