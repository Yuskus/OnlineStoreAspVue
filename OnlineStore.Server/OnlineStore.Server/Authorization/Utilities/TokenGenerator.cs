using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using OnlineStore.Server.DTO.User;

namespace OnlineStore.Server.Authorization.Utilities
{
    public class TokenGenerator
    {
        public static void GenerateJwtToken(IConfiguration configuration, LoginResponse loginResponse)
        {
            var key = new RsaSecurityKey(KeyTool.GetPrivateKey());

            var credentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature);

            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                                             configuration["Jwt:Audience"],
                                             claims:
                                             [
                                                 new Claim(ClaimTypes.NameIdentifier, loginResponse.Username),
                                                 new Claim(ClaimTypes.Role, loginResponse.Role.ToString())
                                             ],
                                             expires: DateTime.Now.AddMinutes(60),
                                             signingCredentials: credentials);

            loginResponse.Token = new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
