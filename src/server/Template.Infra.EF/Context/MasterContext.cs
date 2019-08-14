using Microsoft.EntityFrameworkCore;
using Template.Domain.FeatureExampleModule;

namespace Template.Infra.Data.EF.Context
{
    public class MasterContext : DbContext
    {
        public DbSet<FeatureExample> FeatureExamples { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=DB_TEST;Trusted_Connection=True;");
        }
    }
}
