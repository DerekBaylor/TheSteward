using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Models;
using TheSteward.Core.Models.FinanceManagerModels;
using TheSteward.Core.Models.HouseholdModels;
using TheSteward.Core.Models.TaskManagerModels;
namespace TheSteward.Infrastructure.Data;


public class TheStewardContext(DbContextOptions<TheStewardContext> options) : IdentityDbContext<ApplicationUser>(options)

{
    #region Household Manager Entities
    public DbSet<Household> Households { get; set; }
    public DbSet<UserHousehold> UserHouseholds { get; set; }
    public DbSet<HouseholdInvitation> HouseholdInvitations { get; set; }
    #endregion Household Manager Entities
    
    #region Budget Manager Entities
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<BudgetCategory> BudgetCategories { get; set; }
    public DbSet<BudgetSubCategory> BudgetSubCategories { get; set; }
    public DbSet<Credit>  Credits { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Income> Incomes { get; set; }
    public DbSet<Investment>  Investments { get; set; }
    #endregion Budget Manager Entities

    #region Task Manager Entities
    public DbSet<RecurrenceRule> RecurrenceRules { get; set; }
    public DbSet<TaskItem> TaskItems { get; set; }
    public DbSet<TaskItemCategory> TaskItemsCategories { get; set; }
    public DbSet<TaskItemOccurrence> TaskItemOccurrences { get; set; }

    #endregion Task Manager Entities

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}
