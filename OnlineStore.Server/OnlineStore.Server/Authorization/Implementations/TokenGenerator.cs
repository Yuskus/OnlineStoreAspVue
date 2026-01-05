using Microsoft.IdentityModel.Tokens;
using OnlineStore.Server.Authorization.Abstractions;
using OnlineStore.Server.Authorization.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OnlineStore.Server.Authorization.Implementations
{
    public class TokenGenerator(IConfiguration configuration) : ITokenGenerator
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly TimeSpan _lifetime = TimeSpan.FromMinutes(60);

        public string GenerateToken(string username, string role)
        {
            var rsa = KeyTool.GetPrivateKey(_configuration);
            var key = new RsaSecurityKey(rsa);
            var credentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature);

            var now = DateTime.Now;
            var exp = now.Add(_lifetime);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims:
                [
                    new Claim(ClaimTypes.NameIdentifier, username),
                    new Claim(ClaimTypes.Role, role)
                ],
                notBefore: now,
                expires: exp,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
