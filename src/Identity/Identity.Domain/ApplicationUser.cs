using Microsoft.AspNetCore.Identity;

namespace Identity.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string EnterpriseName { get; set; }
    }
}
