using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.UsersModule;

namespace Template.Infra.Data.EF.Configurations.UsersModule
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.ID);

            builder.Property(u => u.Username)
                .HasColumnType("varchar")
                .IsRequired();

            builder.Property(u => u.Password)
                .HasColumnType("varchar")
                .IsRequired();
        }
    }
}
