using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Template.Infra.Data.EF.Contexts
{
    public class MasterDbContext : DbContext, IMasterDbContext
    {
        public MasterDbContext(DbContextOptions<MasterDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (Database.ProviderName.Equals(ProviderNames.Postgres))
            {
                modelBuilder.HasDefaultSchema("TemplateMaster");

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
