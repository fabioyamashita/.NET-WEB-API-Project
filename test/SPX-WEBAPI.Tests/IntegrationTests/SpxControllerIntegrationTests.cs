using SPX_WEBAPI.AuthorizationAndAuthentication;
using SPX_WEBAPI.Tests.IntegrationTests.Config;
using System.Net.Http.Json;
using Xunit;

namespace SPX_WEBAPI.Tests.IntegrationTests
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class SpxControllerIntegrationTests
    {
        private readonly IntegrationTestsFixture<Program> _testsFixture;

        public SpxControllerIntegrationTests(IntegrationTestsFixture<Program> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "GetById Valid Id Returns Status Code 200")]
        [Trait("SPX", "Spx Controller Integration Tests")]
        public async Task GetById_ValidId_ReturnsStatusCode200()
        {
            // Arrange
            //await _testsFixture.AuthenticateLoginApi();
            //_testsFixture.Client.AssignToken(_testsFixture.UserToken);

            // Act
            var getResponse = await _testsFixture.Client.GetAsync("/Spx/1");

            // Assert
            getResponse.EnsureSuccessStatusCode();
        }
    }
}
