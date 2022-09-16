using SPX_WEBAPI.AuthorizationAndAuthentication.Interfaces;

namespace SPX_WEBAPI.AuthorizationAndAuthentication
{
    public class Token : IToken
    {
        public string Secret { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int ExpirationTimeInHours { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
