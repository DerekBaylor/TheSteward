using System.ComponentModel.DataAnnotations;
using TheSteward.Core.Dtos.HouseholdDtos;

namespace TheSteward.Core.Dtos.FinanceManagerDtos;

public class BudgetDto
{
    public Guid BudgetId { get; set; }
    
    [Required]
    [MaxLength(200)]
    public required string BudgetName { get; set; }

    public DateTime CreatedDate { get; set; }
    
    public DateTime? ModifiedDate { get; set; }

    public bool IsDefaultBudget { get; set; }

    public bool IsPublic { get; set; }

    [Required]
    public required string OwnerId { get; set; }

    public Guid HouseholdId { get; set; }
    
    // Optional: Only include these if you need full nested data
    public HouseholdDto? Household { get; set; }
    public List<BudgetCategoryDto> BudgetCategories { get; set; } = new();
    public List<CreditDto> Credits { get; set; } = new();
    public List<ExpenseDto> Expenses { get; set; } = new();
    public List<IncomeDto> Incomes { get; set; } = new();
    public List<InvestmentDto> Investments { get; set; } = new();
}