using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.UsersModule;

namespace Template.Infra.Data.EF.Configurations.UsersModule
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(p => p.ID);
            builder.Property(p => p.ID)
                .HasColumnType("int")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Username)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(p => p.Password)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(p => p.Role)
                .HasColumnType("tinyint")
                .IsRequired();
        }
    }
}
