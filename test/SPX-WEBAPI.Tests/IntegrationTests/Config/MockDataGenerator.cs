using Microsoft.EntityFrameworkCore;
using SPX_WEBAPI.Domain.Models;
using SPX_WEBAPI.Infra.Data;

namespace SPX_WEBAPI.Tests.IntegrationTests.Config
{
    public static class MockDataGenerator
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.Spx.Any())
            {
                List<Spx> items = new List<Spx>()
                {
                    new Spx(1, DateTime.Now.AddDays(0), 4110.41M, 4083.67M, 4119.28M, 4083.67M),
                    new Spx(2, DateTime.Now.AddDays(-1), 4067.36M, 4022.94M, 4076.81M, 4022.94M),
                    new Spx(3, DateTime.Now.AddDays(-2), 4006.18M, 3959.94M, 4010.5M, 3944.81M),
                    new Spx(4, DateTime.Now.AddDays(-3), 3979.87M, 3909.43M, 3987.89M, 3906.03M),
                    new Spx(5, DateTime.Now.AddDays(-4), 3908.19M, 3930.89M, 3942.55M, 3886.75M),
                };

                context.Spx.AddRange(items);
            }

            if (!context.Users.Any())
            {
                List<Users> users = new List<Users>()
                {
                    new Users("ADA", "admin", "admin", "Admin"),
                    new Users("Fabio Yamashita", "fabioyamashita", "fabioyamashita", "Manager"),
                    new Users("Jose Silva", "josesilva", "josesilva", "Developer"),
                    new Users("Maria Carmo", "mariacarmo", "mariacarmo", "Junior"),
                    new Users("Rosana Pereira", "rosanapereira", "rosanapereira", "Guest"),
                };

                context.Users.AddRange(users);
            }

            context.SaveChanges();
        }
    }
}
