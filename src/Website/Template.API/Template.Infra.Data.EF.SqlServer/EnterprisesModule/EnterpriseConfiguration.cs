using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.EnterprisesModule;
using Template.Infra.Data.EF.Common;

namespace Template.Infra.Data.EF.SqlServer.EnterprisesModule
{
    public class EnterpriseConfiguration : IEntityConfiguration<Enterprise>
    {
        public void Configure(EntityTypeBuilder<Enterprise> builder)
        {
            builder.Property(p => p.ID)
                .HasDefaultValueSql("newsequentialid()");

            builder.Property(p => p.EnterpriseName)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(p => p.ConnectionString)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(p => p.NormalizedEnterpriseName)
                .HasColumnType("varchar(150)")
                .IsRequired();
        }
    }
}
