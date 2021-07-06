using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Template.Application.EnterprisesModule.Queries;
using Template.Infra.Crosscutting.Constants;
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

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration, IJwtTokenFactory jwtTokenFactory, IMediator mediator)
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
            var enterpriseName = jwtToken.Claims.FirstOrDefault(x => x.Type == CustomClaims.EnterpriseName)?.Value;
            if (string.IsNullOrWhiteSpace(enterpriseName) is false)
            {
                var query = new RetrieveConnectionStringByEnterpriseNameQuery() { EnterpriseName = enterpriseName };
                var result = await mediator.Send(query);

                if (result.IsFailure)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync(result.Value);
                }
                else
                {
                    context.Items[HttpContextKeys.TenantConnectionString] = result.Value;
                }
            }

            await _next(context);
        }
    }
}
