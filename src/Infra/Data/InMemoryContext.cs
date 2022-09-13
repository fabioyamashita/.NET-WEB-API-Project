using Microsoft.EntityFrameworkCore;
using SPX_WEBAPI.Domain.Models;

namespace SPX_WEBAPI.Infra.Data
{
    public class InMemoryContext : DbContext
    {
        public InMemoryContext(DbContextOptions<InMemoryContext> options) : base(options)
        {

        }

        public DbSet<Spx> Spx { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
    }
}
