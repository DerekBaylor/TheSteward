using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static TheSteward.Core.Utils.FinanceManagerUtils.FinanceManagerConstants;

namespace TheSteward.Core.Models.FinanceManagerModels;

public class Income
{
    [Key]
    public Guid IncomeId { get; set; }

    [MaxLength(200)]
    public required string IncomeName { get; set; }

    /// <summary>
    /// Specifies how often the income is received, used to calculate <see cref="YearlyGrossSalary"/>.
    /// </summary>
    /// <remarks>
    /// Maps to <see cref="FinanceManagerConstants.FrequencyEnum"/>:
    /// <list type="bullet">
    ///     <item><description><b>Monthly (12)</b> — Paid once per month</description></item>
    ///     <item><description><b>BiMonthly (24)</b> — Paid twice per month</description></item>
    ///     <item><description><b>BiWeekly (26)</b> — Paid every two weeks</description></item>
    ///     <item><description><b>Weekly (52)</b> — Paid every week</description></item>
    /// </list>
    /// The numeric value represents the number of pay periods per year.
    /// </remarks>
    [Required]
    public FrequencyEnum IncomeFrequency { get; set; }

    /// <summary>
    /// The IRS filing status used to determine the applicable federal tax brackets
    /// for <see cref="EstFederalIncomeTax"/> calculations.
    /// </summary>
    /// <remarks>
    /// Maps to <see cref="FinanceManagerConstants.FilingStatusEnum"/>:
    /// <list type="bullet">
    ///     <item><description><b>Single</b> — Unmarried or legally separated</description></item>
    ///     <item><description><b>MarriedJointly</b> — Married and filing a combined return</description></item>
    ///     <item><description><b>MarriedSeparately</b> — Married but filing individual returns</description></item>
    ///     <item><description><b>HeadOfHousehold</b> — Unmarried and supporting a qualifying dependent</description></item>
    /// </list>
    /// </remarks>
    [Required]
    public FilingStatusEnum FilingStatus { get; set; }


    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PayCheckGross { get; set; }

    /// <summary>
    /// GrossYearlySalary will total IncomeFrequency x PayCheckGross
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal YearlyGrossSalary { get; set; }

    /// <summary>
    /// EstFederalIncomeTax will calculate tax rates based on 0 allowances and the GrossYearlySalary
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal EstFederalIncomeTax { get; set; }

    /// <summary>
    /// EstStateIncomeTax will calculate the tax rates base on 0 allowances and the GrossYearlySalary
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal EstStateIncomeTax { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal MonthlyNetIncome { get; set; }

    [Required]
    public int DisplayOrder { get; set; }
    
    #region Navigational Properties
    [Required]
    public Guid BudgetId { get; set; }

    [ForeignKey("BudgetId")]
    [JsonIgnore]
    public Budget? Budget { get; set; }

    #endregion Navigational Properties
}
