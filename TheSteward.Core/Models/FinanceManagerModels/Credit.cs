using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static TheSteward.Core.Utils.FinanceManagerUtils.FinanceManagerConstants;

namespace TheSteward.Core.Models.FinanceManagerModels;

public class Credit
{
    [Key]
    public Guid CreditId { get; set; }

    [Required]
    [MaxLength(200)]
    public required string CreditName { get; set; }

    [MaxLength(50)]
    public string? CreditType { get; set; }

    [Required]
    [Column(TypeName = "decimal(5,4)")]
    public decimal InterestRate { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal CurrentValue { get; set; }

    /// <summary>
    /// Calculated during creation / update
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal EstMonthlyInterest { get; set; }

    /// <summary>
    /// Calculated during creation / update
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal EstYearlyInterest { get; set; }

    /// <summary>
    /// Specifies how often the Credit is paid, used to calculate.
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
    public FrequencyEnum PaymentFrequency { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PaymentAmount { get; set; }

    /// <summary>
    /// Day of the month (1-31). Use 31 for "last day of month"
    /// </summary>
    [Required]
    public int PaymentDay { get; set; }

    [Required]
    public int DisplayOrder { get; set; }

    #region Navigation Properties
    [Required]
    public Guid BudgetId { get; set; }

    [JsonIgnore]
    [ForeignKey("BudgetId")]
    public Budget? Budget { get; set; }

    public Guid ExpenseId { get; set; }
    [JsonIgnore]
    [ForeignKey("ExpenseId")]
    public Expense? LinkedExpense { get; set; }

    #endregion Navigation Properties
}
