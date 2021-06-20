using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain;

namespace Template.Infra.Data.EF.Common
{
    public interface IEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : Entity
    {
        public new void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(p => p.ID);
        }
    }
}
