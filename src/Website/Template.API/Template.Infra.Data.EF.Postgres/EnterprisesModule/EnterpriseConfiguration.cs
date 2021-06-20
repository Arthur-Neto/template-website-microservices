using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.EnterprisesModule;
using Template.Infra.Data.EF.Common;

namespace Template.Infra.Data.EF.Postgres.EnterprisesModule
{
    public class EnterpriseConfiguration : IEntityConfiguration<Enterprise>
    {
        public void Configure(EntityTypeBuilder<Enterprise> builder)
        {
            builder.Property(p => p.ID)
                .HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(p => p.EnterpriseName)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(p => p.SchemaName)
                .HasColumnType("varchar(50)")
                .IsRequired();
        }
    }
}
