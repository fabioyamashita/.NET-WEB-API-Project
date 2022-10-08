using FluentAssertions;
using SPX_WEBAPI.AuthorizationAndAuthentication;
using SPX_WEBAPI.Tests.IntegrationTests.Config;
using System.Net;
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

        [Fact(DisplayName = "Authenticate Valid Login Returns Status Code 200")]
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
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        }

        [Fact(DisplayName = "Authenticate Invalid Login And Correct Password Returns Status Code 401")]
        [Trait("SPX", "Login Controller Integration Tests")]
        public async Task Authenticate_InvalidLoginAndCorrectPassword_ReturnsStatusCode401()
        {
            // Arrange
            var userData = new Authenticate
            {
                Login = "wrongUser",
                Password = "m1nh@s3nh@"
            };

            // Act
            var getResponse = await _testsFixture.Client.PostAsJsonAsync("/Login", userData);

            // Assert
            getResponse.IsSuccessStatusCode.Should().BeFalse();
            getResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact(DisplayName = "Authenticate Correct Login And invalid Password Returns Status Code 401")]
        [Trait("SPX", "Login Controller Integration Tests")]
        public async Task Authenticate_CorrectLoginAndInvalidPassword_ReturnsStatusCode401()
        {
            // Arrange
            var userData = new Authenticate
            {
                Login = "usuario",
                Password = "wrongPassword"
            };

            // Act
            var getResponse = await _testsFixture.Client.PostAsJsonAsync("/Login", userData);

            // Assert
            getResponse.IsSuccessStatusCode.Should().BeFalse();
            getResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
