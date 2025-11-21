using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Models;
namespace TheSteward.Infrastructure.Data;


public class TheStewardContext(DbContextOptions<TheStewardContext> options) : IdentityDbContext<ApplicationUser>(options)

{
    public DbSet<Household> Households { get; set; }
    public DbSet<UserHousehold> UserHouseholds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}
