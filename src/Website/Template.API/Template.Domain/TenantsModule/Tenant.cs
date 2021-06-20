using System;
using Template.Domain.EnterprisesModule;
using Template.Domain.TenantsModule.Enums;

namespace Template.Domain.TenantsModule
{
    public class Tenant : Entity
    {
        public string Logon { get; set; }
        public string Salt { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        public string Token { get; set; }

        // Foreign Keys
        public Guid EnterpriseID { get; set; }
        public Enterprise Enterprise { get; set; }
    }
}
