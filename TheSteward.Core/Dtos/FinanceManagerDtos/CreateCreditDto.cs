using System.ComponentModel.DataAnnotations;
using static TheSteward.Core.Utils.FinanceManagerUtils.FinanceManagerConstants;

namespace TheSteward.Core.Dtos.FinanceManagerDtos;

public class CreateCreditDto
{
    [Required]
    [MaxLength(200)]
    public required string CreditName { get; set; }
    
    [MaxLength(50)]
    public string? CreditType { get; set; }
    
    [Required]
    public decimal InterestRate { get; set; }
    
    [Required]
    public decimal CurrentValue { get; set; }
    
    /// <summary>
    /// Calculated during creation
    /// </summary>
    public decimal EstMonthlyInterest { get; set; }
    
    /// <summary>
    /// Calculated during creation
    /// </summary>
    public decimal EstYearlyInterest { get; set; }

    /// <summary>
    /// Monthly = 12, Bi-Monthly = 24, Bi-Weekly = 26, Weekly = 52
    /// </summary>
    public FrequencyEnum PaymentFrequency { get; set; }
    
    public decimal PaymentAmount { get; set; }

    /// <summary>
    /// Day of the month (1-31). Use 31 for "last day of month"
    /// </summary>
    [Range(1, 31)]
    public int PaymentDay { get; set; }
    
    public int DisplayOrder { get; set; }

    #region Navigation Properties

    [Required]
    public Guid BudgetId { get; set; }
    
    [Required]
    public Guid ExpenseId { get; set; }

    #endregion  
}