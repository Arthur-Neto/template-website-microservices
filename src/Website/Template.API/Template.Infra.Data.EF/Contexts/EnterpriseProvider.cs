using Microsoft.AspNetCore.Http;
using Template.Infra.Crosscutting.Constants;

namespace Template.Infra.Data.EF.Contexts
{
    public interface IEnterpriseProvider
    {
        string GetTenantConnectionString();
    }

    public class EnterpriseProvider : IEnterpriseProvider
    {
        private readonly IHttpContextAccessor _accesor;

        public EnterpriseProvider(IHttpContextAccessor accesor)
        {
            _accesor = accesor;
        }

        public string GetTenantConnectionString()
        {
            var connectionString = _accesor.HttpContext?.Items[HttpContextKeys.TenantConnectionString].ToString();

            return connectionString;
        }
    }
}
