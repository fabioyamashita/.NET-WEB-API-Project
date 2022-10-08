using System.Security.Claims;

namespace SPX_WEBAPI.Tests.IntegrationTests.Config
{
    public class MockAuthUser
    {
        public List<Claim> Claims { get; private set; } = new();

        public MockAuthUser(params Claim[] claims)
            => Claims = claims.ToList();
    }
}
