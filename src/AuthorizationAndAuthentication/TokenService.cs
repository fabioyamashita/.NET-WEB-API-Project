using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SPX_WEBAPI.AuthorizationAndAuthentication.Interfaces;
using SPX_WEBAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SPX_WEBAPI.AuthorizationAndAuthentication
{
    public class TokenService : ITokenService
    {
        private readonly IToken _token;
        private readonly IConfiguration _configuration;

        public TokenService(IToken token, IConfiguration configuration)
        {
            _token = token;
            _configuration = configuration;
        }

        public string GenerateToken(Users user)
        {
            new ConfigureFromConfigurationOptions<IToken>
                (_configuration.GetSection("Token")).Configure(_token);

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_token.Secret));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("module", "Web III .net"),
                    new Claim("sub","SPX-WEBAPI")
                }),
                Expires = DateTime.UtcNow.AddHours(_token.ExpirationTimeInHours),
                Issuer = _token.Issuer,
                Audience = _token.Audience,
                SigningCredentials =
                    new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
