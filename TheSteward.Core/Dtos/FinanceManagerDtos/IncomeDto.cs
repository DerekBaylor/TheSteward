using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static TheSteward.Core.Utils.FinanceManagerUtils.FinanceManagerConstants;

namespace TheSteward.Core.Dtos.FinanceManagerDtos;

public class IncomeDto
{
    public Guid IncomeId { get; set; }

    [Required]
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
    public FilingStatusEnum FilingStatus { get; set; }

    public decimal PayCheckGross { get; set; }

    /// <summary>
    /// YearlyGrossSalary will total IncomeFrequency x PayCheckGross
    /// </summary>
    public decimal YearlyGrossSalary { get; set; }

    /// <summary>
    /// EstFederalIncomeTax will calculate tax rates based on 0 allowances and the YearlyGrossSalary
    /// </summary>
    public decimal EstFederalIncomeTax { get; set; }

    /// <summary>
    /// EstStateIncomeTax will calculate the tax rates based on 0 allowances and the YearlyGrossSalary
    /// </summary>
    public decimal? EstStateIncomeTax { get; set; }

    public decimal MonthlyNetIncome { get; set; }

    public int DisplayOrder { get; set; }

    public Guid BudgetId { get; set; }
}