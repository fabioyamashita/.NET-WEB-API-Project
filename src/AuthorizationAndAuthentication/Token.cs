namespace SPX_WEBAPI.AuthorizationAndAuthentication
{
    public class Token
    {
        public string Secret { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int ExpirationTimeInHours { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
