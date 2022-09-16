using Microsoft.EntityFrameworkCore;
using SPX_WEBAPI.Domain.Models;
using System;

namespace SPX_WEBAPI.Infra.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Spx> Spx { get; set; }
        public DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Spx>().Property(p => p.Open).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<Spx>().Property(p => p.Close).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<Spx>().Property(p => p.High).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<Spx>().Property(p => p.Low).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<Spx>().Property(p => p.Date).HasColumnType("date");
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>()
                .HaveMaxLength(100);
        }
    }
}
