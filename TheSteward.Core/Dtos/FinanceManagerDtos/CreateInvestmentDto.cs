using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static TheSteward.Core.Utils.FinanceManagerUtils.FinanceManagerConstants;

namespace TheSteward.Core.Dtos.FinanceManagerDtos;

public class CreateInvestmentDto
{
    [Required]
    [MaxLength(200)]
    public required string InvestmentName { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Current value must be greater than or equal to 0")]
    public decimal CurrentValue { get; set; }

    [Required]
    [Range(0, 1, ErrorMessage = "Interest rate must be between 0 and 1 (e.g., 0.07 for 7%)")]
    public decimal InterestRate { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Contribution amount must be greater than or equal to 0")]
    public decimal ContributionAmount { get; set; }

    /// <summary>
    /// Monthly = 12, Bi-Monthly = 24, Bi-Weekly = 26, Weekly = 52
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public FrequencyEnum ContributionFrequency { get; set; }

    /// <summary>
    /// Calculated on the client during Create/Update
    /// </summary>
    [Required]
    public decimal EstYearlyGrowth { get; set; }
    
    public int DisplayOrder { get; set; }

    #region Navigational Properties
    
    [Required]
    public Guid BudgetId { get; set; }

    public Guid? ExpenseId { get; set; }

    public Guid HouseholdId { get; set; }

    #endregion Navigational Properties
}