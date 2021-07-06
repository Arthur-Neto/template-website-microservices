using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Template.Infra.Data.EF.Contexts
{
    public class MultiTenantModelCacheKeyFactory : IModelCacheKeyFactory
    {
        private string _connectionString;

        public object Create(DbContext context)
        {
            var dataContext = context as TenantDbContext;
            if (dataContext != null)
            {
                _connectionString = dataContext.ConnectionString;
            }

            return new MultiTenantModelCacheKey(_connectionString, context);
        }
    }

    public class MultiTenantModelCacheKey : ModelCacheKey
    {
        private readonly string _connectionString;

        public MultiTenantModelCacheKey(string connectionString, DbContext context)
            : base(context)
        {
            _connectionString = connectionString;
        }

        public override int GetHashCode()
        {
            return _connectionString.GetHashCode();
        }
    }
}
