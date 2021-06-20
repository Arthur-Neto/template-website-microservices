using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Template.WebApi.Extensions
{
    public static class AuthenticationExtensions
    {
        public static void ConfigAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var secret = configuration.GetValue<string>("Secret");
            if (secret == null)
            {
                throw new Exception("Secret not defined on appsettings");
            }

            var secretEncoded = Encoding.ASCII.GetBytes(secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretEncoded),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
    }
}
