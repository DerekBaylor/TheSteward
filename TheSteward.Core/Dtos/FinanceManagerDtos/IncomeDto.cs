using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSteward.Core.Dtos.FinanceManagerDtos;

public class IncomeDto
{
    public Guid IncomeId { get; set; }

    [Required]
    [MaxLength(200)]
    public required string IncomeName { get; set; }

    /// <summary>
    /// Monthly = 12, Bi-Monthly = 24, Bi-Weekly = 26, Weekly = 52
    /// </summary>
    public int IncomeFrequency { get; set; }

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