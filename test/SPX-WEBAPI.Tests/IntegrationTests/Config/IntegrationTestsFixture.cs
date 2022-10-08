using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using SPX_WEBAPI.AuthorizationAndAuthentication;
using SPX_WEBAPI.Domain.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace SPX_WEBAPI.Tests.IntegrationTests.Config
{
    [CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<Program>> { }

    public class IntegrationTestsFixture<TStartup> : IDisposable where TStartup : class
    {
        public readonly WebApiApplicationFactory<TStartup> Factory;
        public HttpClient Client;

        public string UserToken;

        public IntegrationTestsFixture()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("http://localhost:5000"),
                HandleCookies = true,
            };

            Factory = new WebApiApplicationFactory<TStartup>();


            Client = Factory.CreateClient(clientOptions);

            //var userData = new Authenticate
            //{
            //    Login = "usuario",
            //    Password = "m1nh@s3nh@"
            //};

            //Client = Factory.WithWebHostBuilder(builder =>
            //{
            //    builder.ConfigureTestServices(services => services.AddScoped(_ => userData));
            //}).CreateClient(clientOptions);

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        }

        public async Task AuthenticateLoginApi()
        {
            var userData = new Authenticate
            {
                Login = "usuario",
                Password = "m1nh@s3nh@"
            };

            // Recreating client to avoid web config
            Client = Factory.CreateClient();

            var response = await Client.PostAsJsonAsync("/Login", userData);
            response.EnsureSuccessStatusCode();
            UserToken = await response.Content.ReadAsStringAsync();
        }

        public void Dispose()
        {
            Factory.Dispose();
            Client.Dispose();
        }
    }
}
