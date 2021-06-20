using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Template.Domain.TenantsModule.Enums;

namespace Template.WebApi.Attributes
{
    public class AuthorizeRoles : AuthorizeAttribute
    {
        public AuthorizeRoles(params Role[] roles)
        {
            var allowedRolesAsStrings = roles.Select(x => Enum.GetName(typeof(Role), x));
            Roles = string.Join(",", allowedRolesAsStrings);
        }
    }
}
