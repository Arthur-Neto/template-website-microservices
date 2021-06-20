using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Template.Infra.Data.EF.Contexts
{
    public class MultiTenantModelCacheKeyFactory : IModelCacheKeyFactory
    {
        private string _schemaName;

        public object Create(DbContext context)
        {
            var dataContext = context as TenantDbContext;
            if (dataContext != null)
            {
                _schemaName = dataContext.SchemaName;
            }

            return new MultiTenantModelCacheKey(_schemaName, context);
        }
    }

    public class MultiTenantModelCacheKey : ModelCacheKey
    {
        private readonly string _schemaName;

        public MultiTenantModelCacheKey(string schemaName, DbContext context)
            : base(context)
        {
            _schemaName = schemaName;
        }

        public override int GetHashCode()
        {
            return _schemaName.GetHashCode();
        }
    }
}
