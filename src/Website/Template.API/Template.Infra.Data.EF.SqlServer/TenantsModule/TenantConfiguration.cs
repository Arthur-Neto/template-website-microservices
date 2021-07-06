using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.TenantsModule;
using Template.Infra.Data.EF.Common;

namespace Template.Infra.Data.EF.SqlServer.TenantsModule
{
    public class TenantConfiguration : IEntityConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.Property(p => p.ID)
                .HasDefaultValueSql("newsequentialid()");

            builder.Property(p => p.Logon)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(p => p.Salt)
                .HasColumnType("varchar(MAX)")
                .IsRequired();

            builder.Property(p => p.Password)
                .HasColumnType("varchar(MAX)")
                .IsRequired();

            builder.Property(p => p.Role)
                .HasConversion<short>()
                .IsRequired();

            builder.Ignore(p => p.Token);

            builder.HasOne(p => p.Enterprise)
                .WithMany(p => p.Tenants)
                .HasForeignKey(p => p.EnterpriseID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
