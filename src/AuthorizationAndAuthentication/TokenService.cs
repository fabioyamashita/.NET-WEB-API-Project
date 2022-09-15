using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SPX_WEBAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SPX_WEBAPI.AuthorizationAndAuthentication
{
    public class TokenService
    {
        private readonly Token _tokenConfiguration;
        private readonly IConfiguration _configuration;

        public TokenService(Token tokenConfiguration, IConfiguration configuration)
        {
            _tokenConfiguration = tokenConfiguration;
            _configuration = configuration;
        }

        public string GenerateToken(Users user)
        {
            new ConfigureFromConfigurationOptions<Token>
                (_configuration.GetSection("TokenConfiguration")).Configure(_tokenConfiguration);

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenConfiguration.Secret));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("module", "Web III .net"),
                    new Claim("sub","SPX-WEBAPI")
                }),
                Expires = DateTime.UtcNow.AddHours(_tokenConfiguration.ExpirationTimeInHours),
                Issuer = _tokenConfiguration.Issuer,
                Audience = _tokenConfiguration.Audience,
                SigningCredentials =
                    new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
