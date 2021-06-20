using System.Collections.Generic;
using Template.Domain.TenantsModule;

namespace Template.Domain.EnterprisesModule
{
    public class Enterprise : Entity
    {
        public string EnterpriseName { get; set; }
        public string SchemaName { get; set; }

        // Reverse Navigation
        public ICollection<Tenant> Tenants { get; set; }
    }
}
