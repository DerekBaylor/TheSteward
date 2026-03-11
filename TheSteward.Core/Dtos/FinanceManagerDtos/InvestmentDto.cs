using System.ComponentModel.DataAnnotations;
using static TheSteward.Core.Utils.FinanceManagerUtils.FinanceManagerConstants;

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
    public FrequencyEnum ContributionFrequency { get; set; }

    /// <summary>
    /// Estimated yearly growth calculated on the client during Create/Update
    /// </summary>
    public decimal EstYearlyGrowth { get; set; }
    
    public int DisplayOrder { get; set; }

    #region Navigation Properties
    
    [Required]
    public Guid BudgetId { get; set; }
    
   public Guid? ExpenseId { get; set; }
    
    public ExpenseDto? LinkedExpenseDto { get; set; }

    #endregion

}