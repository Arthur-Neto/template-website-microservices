using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Template.Infra.Crosscutting.Security;

namespace Template.Infra.Crosscutting.Http
{
    public static class HttpContextExtensions
    {
        public static async Task<JwtSecurityToken> GetTokenFromCurrentUserAsync(this HttpContext httpContext, IConfiguration configuration)
        {
            var token = await httpContext.GetTokenAsync("Bearer", "access_token");
            var secret = configuration.GetValue<string>("Secret");

            return JwtTokenHelper.ValidateToken(token, secret);
        }
    }
}
