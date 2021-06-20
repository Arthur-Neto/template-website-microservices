using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.UsersModule;
using Template.Infra.Data.EF.Common;

namespace Template.Infra.Data.EF.SqlServer.UsersModule
{
    public class UserConfiguration : IEntityConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.ID)
                .HasDefaultValueSql("newsequentialid()");

            builder.Property(p => p.Name)
                .HasColumnType("varchar(50)")
                .IsRequired();
        }
    }
}
