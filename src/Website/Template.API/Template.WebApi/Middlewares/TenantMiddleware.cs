using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Template.Security;

namespace Template.WebApi.Middlewares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration, IJwtTokenFactory jwtTokenFactory)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() is object)
            {
                await _next(context);
                return;
            }

            var hasAuthorization = context.Request.Headers.ContainsKey("Authorization");
            if (hasAuthorization is false)
            {
                await _next(context);
                return;
            }

            var token = await context.GetTokenAsync("Bearer", "access_token");
            var secret = configuration.GetValue<string>("Secret");
            var jwtToken = jwtTokenFactory.ValidateToken(token, secret);
            var enterpriseSchema = jwtToken.Claims.FirstOrDefault(x => x.Type == "enterprise_schema")?.Value;
            if (string.IsNullOrWhiteSpace(enterpriseSchema) is false)
            {
                context.Items["Schema"] = enterpriseSchema;
            }

            await _next(context);
        }
    }
}
