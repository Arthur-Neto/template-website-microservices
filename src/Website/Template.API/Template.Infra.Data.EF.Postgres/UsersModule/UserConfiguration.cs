using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.UsersModule;
using Template.Infra.Data.EF.Common;

namespace Template.Infra.Data.EF.Postgres.UsersModule
{
    public class UserConfiguration : IEntityConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.ID)
                .HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(p => p.Name)
                .HasColumnType("varchar(50)")
                .IsRequired();
        }
    }
}
