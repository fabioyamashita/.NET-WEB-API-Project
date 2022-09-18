namespace SPX_WEBAPI.AuthorizationAndAuthentication.Interfaces
{
    public interface IToken
    {
        string Audience { get; set; }
        int ExpirationTimeInHours { get; set; }
        string Issuer { get; set; }
        string Role { get; set; }
        string Secret { get; set; }
        string Username { get; set; }
    }
}