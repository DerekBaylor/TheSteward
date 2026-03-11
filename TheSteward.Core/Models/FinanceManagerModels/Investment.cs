using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static TheSteward.Core.Utils.FinanceManagerUtils.FinanceManagerConstants;

namespace TheSteward.Core.Models.FinanceManagerModels;

public class Investment
{
    [Key]
    public Guid InvestmentId { get; set; }

    [Required]
    [MaxLength(200)]
    public required string InvestmentName { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal CurrentValue { get; set; }

    [Required]
    [Column(TypeName = "decimal(5,4)")]
    public decimal InterestRate { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal ContributionAmount { get; set; }

    /// <summary>
    /// Specifies how often the user contributes to the investment.
    /// </summary>
    /// <remarks>
    /// Maps to <see cref="FinanceManagerConstants.FrequencyEnum"/>:
    /// <list type="bullet">
    ///     <item><description><b>Monthly (12)</b> — Paid once per month</description></item>
    ///     <item><description><b>BiMonthly (24)</b> — Paid twice per month</description></item>
    ///     <item><description><b>BiWeekly (26)</b> — Paid every two weeks</description></item>
    ///     <item><description><b>Weekly (52)</b> — Paid every week</description></item>
    /// </list>
    /// The numeric value represents the number of payment periods per year.
    /// </remarks>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public FrequencyEnum ContributionFrequency { get; set; }

    /// <summary>
    /// Calculated on the client during Create/Update
    /// </summary>
    public decimal EstYearlyGrowth { get; set; }

    [Required]
    public int DisplayOrder { get; set; }

    #region Navigational Properties
    [Required]
    public Guid BudgetId { get; set; }

    [JsonIgnore]
    [ForeignKey("BudgetId")]
    public Budget? Budget { get; set; }
    
    public Guid ExpenseId { get; set; }
    
    [JsonIgnore]
    [ForeignKey("ExpenseId")]
    public Expense? LinkedExpense { get; set; }
    
    #endregion Navigational Properties
}
