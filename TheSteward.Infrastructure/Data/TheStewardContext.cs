using Microsoft.EntityFrameworkCore;
using System;
namespace TheSteward.Infrastructure.Data;

public class TheStewardContext : DbContext
{
    public TheStewardContext(DbContextOptions<TheStewardContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}
