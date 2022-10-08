using SPX_WEBAPI.AuthorizationAndAuthentication;
using SPX_WEBAPI.Tests.IntegrationTests.Config;
using System.Net.Http.Json;
using Xunit;

namespace SPX_WEBAPI.Tests.IntegrationTests
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class LoginControllerIntegrationTests
    {
        private readonly IntegrationTestsFixture<Program> _testsFixture;

        public LoginControllerIntegrationTests(IntegrationTestsFixture<Program> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Authenticate ValidLogin Returns Status Code 200")]
        [Trait("SPX", "Login Controller Integration Tests")]
        public async Task Authenticate_ValidLogin_ReturnsStatusCode200()
        {
            // Arrange
            var userData = new Authenticate
            {
                Login = "usuario",
                Password = "m1nh@s3nh@"
            };

            // Act
            var getResponse = await _testsFixture.Client.PostAsJsonAsync("/Login", userData);

            // Assert
            getResponse.EnsureSuccessStatusCode();

        }
    }
}
