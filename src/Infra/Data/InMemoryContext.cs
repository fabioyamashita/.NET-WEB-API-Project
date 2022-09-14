using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SPX_WEBAPI.Domain.Models;

namespace SPX_WEBAPI.Infra.Data
{
    public class InMemoryContext : DbContext
    {
        public InMemoryContext(DbContextOptions<InMemoryContext> options) : base(options)
        {

        }

        public DbSet<Spx> Spx { get; set; }
        public DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
    }
}
