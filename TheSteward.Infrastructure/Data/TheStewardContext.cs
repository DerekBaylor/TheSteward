using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Models;
using TheSteward.Core.Models.FinanceManagerModels;
using TheSteward.Core.Models.HouseholdModels;
namespace TheSteward.Infrastructure.Data;


public class TheStewardContext(DbContextOptions<TheStewardContext> options) : IdentityDbContext<ApplicationUser>(options)

{
    #region Household Entities
    public DbSet<Household> Households { get; set; }
    public DbSet<UserHousehold> UserHouseholds { get; set; }
    public DbSet<HouseholdInvitation> HouseholdInvitations { get; set; }
    #endregion Household Entities
    
    #region Budget Entities
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<BudgetCategory> BudgetCategories { get; set; }
    public DbSet<BudgetSubCategory> BudgetSubCategories { get; set; }
    public DbSet<Credit>  Credits { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Income> Incomes { get; set; }
    public DbSet<Investment>  Investments { get; set; }
    #endregion Budget Entities

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}
