using Microsoft.AspNetCore.Http;

namespace Template.Infra.Data.EF.Contexts
{
    public interface IEnterpriseProvider
    {
        string GetSchemaName();
    }

    public class EnterpriseProvider : IEnterpriseProvider
    {
        private readonly string _schemaFormat = "Template_{0}";

        private readonly IHttpContextAccessor _accesor;

        public EnterpriseProvider(IHttpContextAccessor accesor)
        {
            _accesor = accesor;
        }

        public string GetSchemaName()
        {
            var schemaName = _accesor.HttpContext?.Items["Schema"].ToString();

            return string.Format(_schemaFormat, schemaName);
        }
    }
}
