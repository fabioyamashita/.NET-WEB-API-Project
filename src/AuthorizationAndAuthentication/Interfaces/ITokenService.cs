using SPX_WEBAPI.Domain.Models;

namespace SPX_WEBAPI.AuthorizationAndAuthentication.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Users user);
    }
}