using System.ComponentModel.DataAnnotations;

namespace TheSteward.Core.Dtos.FinanceManagerDtos;

public class InvestmentDto
{
    public Guid InvestmentId { get; set; }

    [Required]
    [MaxLength(200)]
    public required string InvestmentName { get; set; }

    public decimal CurrentValue { get; set; }
    public decimal InterestRate { get; set; }
    public decimal ContributionAmount { get; set; }

    /// <summary>
    /// Monthly = 12, Bi-Monthly = 24, Bi-Weekly = 26, Weekly = 52
    /// </summary>
    public int ContributionFrequency { get; set; }

    public decimal EstYearlyGrowth { get; set; }

    public int DisplayOrder { get; set; }

    #region Navigation Properties

    public Guid BudgetId { get; set; }
    
    public Guid ExpesneId { get; set; }
    public ExpenseDto LinkedExpenseDto { get; set; }

    #endregion
}