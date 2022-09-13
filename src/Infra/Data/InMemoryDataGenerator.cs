using SPX_WEBAPI.Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SPX_WEBAPI.Infra.Data
{
    public class InMemoryDataGenerator
    {
        private readonly InMemoryContext _inMemoryContext;

        public InMemoryDataGenerator(InMemoryContext inMemoryContext)
        {
            _inMemoryContext = inMemoryContext;
        }

        public void Generate()
        {
            if (!_inMemoryContext.Spx.Any())
            {
                List<Spx> items;

                using (var r = new StreamReader("SPXData-2012-2022.json"))
                {
                    var json = r.ReadToEnd();
                    items = JsonSerializer.Deserialize<List<Spx>>(json);
                }

                _inMemoryContext.Spx.AddRange(items);
                _inMemoryContext.SaveChanges();
            }
        }
    }
}
