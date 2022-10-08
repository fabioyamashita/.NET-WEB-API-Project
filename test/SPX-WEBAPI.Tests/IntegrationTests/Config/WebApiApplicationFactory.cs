using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using SPX_WEBAPI.Infra.Data;
using System.Security.Claims;

namespace SPX_WEBAPI.Tests.IntegrationTests.Config
{
    public class WebApiApplicationFactory<TStartup> 
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly MockAuthUser _user = new MockAuthUser(
            new Claim(ClaimTypes.Name, "usuario"),
            new Claim(ClaimTypes.Role, "admin"),
            new Claim("module", "Web III .net"),
            new Claim("sub", "SPX-WEBAPI"));

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");

            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("SpxWebApiDatabase"));

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<WebApiApplicationFactory<TStartup>>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        MockDataGenerator.Seed(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                        "database. Error: {ex.Message}", ex.Message);
                    }
                }

                services.AddTestAuthentication();

                services.AddScoped(_ => _user);
            });
        }
    }
}
