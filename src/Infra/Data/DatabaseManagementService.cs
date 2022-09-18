using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SPX_WEBAPI.Infra.Data
{
    public static class DatabaseManagementService
    {
        public static void ExecuteMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var serviceDb = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                serviceDb.Database.Migrate();
            }
        }
    }
}
