using Hyka.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Hyka.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        var tariffs = JsonConvert.DeserializeObject<List<Tariff>>(File.ReadAllText("Areas/Identity/Data/Tariffs.json"));
        var territories = JsonConvert.DeserializeObject<List<Territory>>(File.ReadAllText("Areas/Identity/Data/Territories.json"));
        builder.Entity<Tariff>().HasData(tariffs);
        builder.Entity<Territory>().HasData(territories);
        builder.Entity<IdentityRole>().HasData(new[]
        {
            new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Name = "Blockbuster", NormalizedName = "BLOCKBUSTER"},
            new IdentityRole { Name = "User", NormalizedName = "USER" }
        });
    }

    public DbSet<Person> People { get; set; }
    public DbSet<Territory> Territories { get; set; }
    public DbSet<Tariff> Tariffs { get; set; }
    public DbSet<Record> History { get; set; }

}
