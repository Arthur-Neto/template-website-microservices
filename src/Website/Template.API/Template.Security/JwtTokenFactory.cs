using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Template.Security
{
    public interface IJwtTokenFactory
    {
        string CreateToken(string secret, double tokenExpiration, string claimName, string claimRole, string claimEnterprise);
        JwtSecurityToken ValidateToken(string token, string secret);
    }

    public class JwtTokenFactory : IJwtTokenFactory
    {
        public string CreateToken(string secret, double tokenExpiration, string claimName, string claimRole, string claimEnterprise)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, claimName),
                    new Claim(ClaimTypes.Role, claimRole),
                    new Claim(CustomClaims.EnterpriseName, claimEnterprise),
                }),
                Expires = DateTime.UtcNow.AddMinutes(tokenExpiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public JwtSecurityToken ValidateToken(string token, string secret)
        {
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            return (JwtSecurityToken)validatedToken;
        }
    }
}
