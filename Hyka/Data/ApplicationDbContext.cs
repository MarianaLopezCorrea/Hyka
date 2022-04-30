using Hyka.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Hyka.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Blockbuster> Blockbusters { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Territory> Territories { get; set; }
        public DbSet<Tariff> Tariff { get; set; }


    }
}
