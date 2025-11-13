using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Models;
namespace TheSteward.Infrastructure.Data;


public class TheStewardContext(DbContextOptions<TheStewardContext> options) : IdentityDbContext<ApplicationUser>(options)

{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}
