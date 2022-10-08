using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SPX_WEBAPI.Tests.IntegrationTests.Config
{
    public static class TestsExtensions
    {
        public static void AssignToken(this HttpClient client, string token)
        {
            client.AssignJsonMediaType();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public static void AssignJsonMediaType(this HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static AuthenticationBuilder AddTestAuthentication(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // AuthConstants.Scheme is just a scheme we define. I called it "TestAuth"
                options.DefaultPolicy = new AuthorizationPolicyBuilder("Test")
                .RequireAuthenticatedUser()
                    .Build();
            });

            // Register our custom authentication handler
            return services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    "Test", options => { });
        }
    }
}
