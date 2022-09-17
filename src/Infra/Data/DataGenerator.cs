using SPX_WEBAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SPX_WEBAPI.Infra.Data
{
    public class DataGenerator
    {
        private readonly ApplicationDbContext _context;

        public DataGenerator(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Generate()
        {
            if (!_context.Spx.Any())
            {
                List<Spx> items;

                using (var r = new StreamReader("SPXData-2012-2022.json"))
                {
                    var json = r.ReadToEnd();
                    items = JsonSerializer.Deserialize<List<Spx>>(json);
                }

                _context.Spx.AddRange(items);
                _context.SaveChanges();
            }

            if (!_context.Users.Any())
            {
                List<Users> users = new List<Users>()
                {
                    new Users("ADA", "admin", "admin", "Admin"),
                    new Users("Fabio Yamashita", "fabioyamashita", "fabioyamashita", "Manager"),
                    new Users("Jose Silva", "josesilva", "josesilva", "Developer"),
                    new Users("Maria Carmo", "mariacarmo", "mariacarmo", "Junior"),
                    new Users("Rosana Pereira", "rosanapereira", "rosanapereira", "Guest"),
                };

                _context.Users.AddRange(users);
                _context.SaveChanges();
            }
        }
    }
}
