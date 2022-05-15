using Hyka.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hyka.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
    public DbSet<Blockbuster> Blockbusters { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<Territory> Territories { get; set; }
    public DbSet<Tariff> Tariffs { get; set; }
    public DbSet<History> Histories { get; set; } 

}
