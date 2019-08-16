using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.FeatureExampleModule;

namespace Template.Infra.Data.EF.Configurations.FeatureExampleModule
{
    public class FeatureExampleConfiguration : IEntityTypeConfiguration<FeatureExample>
    {
        public void Configure(EntityTypeBuilder<FeatureExample> builder)
        {
            builder.ToTable("FeaturesExamples", "dbo");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.FeatureExampleType).HasColumnType("INT").IsRequired();
        }
    }
}
