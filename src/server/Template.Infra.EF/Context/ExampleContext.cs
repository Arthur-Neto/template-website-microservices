using Microsoft.EntityFrameworkCore;
using Template.Domain.FeatureExampleModule;

namespace Template.Infra.Data.EF.Context
{
    public class ExampleContext : DbContext
    {
        public ExampleContext(DbContextOptions<ExampleContext> options)
            : base(options)
        { }

        public DbSet<FeatureExample> FeaturesExamples { get; set; }
    }
}
