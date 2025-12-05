using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Models;
using TheSteward.Core.Models.HouseholdModels;
namespace TheSteward.Infrastructure.Data;


public class TheStewardContext(DbContextOptions<TheStewardContext> options) : IdentityDbContext<ApplicationUser>(options)

{
    public DbSet<Household> Households { get; set; }
    public DbSet<UserHousehold> UserHouseholds { get; set; }
    public DbSet<HouseholdInvitation> HouseholdInvitations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}
