using System.ComponentModel.DataAnnotations;

namespace TheSteward.Core.Dtos.FinanceManagerDtos;

public class CreateIncomeDto
{
    // public Guid IncomeId { get; set; }
    //
    // [MaxLength(200)]
    // public required string IncomeName { get; set; }
    //
    // /// <summary>
    // /// Monthly = 12, Bi-Monthly = 24, Bi-Weekly = 26, Weekly = 52
    // /// </summary>
    // public int IncomeFrequency { get; set; }
    //
    // public decimal PayCheckGross { get; set; }
    //
    // /// <summary>
    // /// YearlyGrossSalary will total IncomeFrequency x PayCheckGross
    // /// </summary>
    // public decimal YearlyGrossSalary { get; set; }
    //
    // /// <summary>
    // /// EstFederalIncomeTax will calculate tax rates based on 0 allowances and the YearlyGrossSalary
    // /// </summary>
    // public decimal EstFederalIncomeTax { get; set; }
    //
    // /// <summary>
    // /// EstStateIncomeTax will calculate the tax rates based on 0 allowances and the YearlyGrossSalary
    // /// </summary>
    // public decimal? EstStateIncomeTax { get; set; }
    //
    // public decimal MonthlyNetIncome { get; set; }
    //
    // public int DisplayOrder { get; set; }
    //
    // public Guid BudgetId { get; set; }
    
    [Required]
    [MaxLength(200)]
    public required string IncomeName { get; set; }

    /// <summary>
    /// Monthly = 12, Bi-Monthly = 24, Bi-Weekly = 26, Weekly = 52
    /// </summary>
    [Required]
    [Range(12, 52, ErrorMessage = "Income frequency must be 12, 24, 26, or 52")]
    public int IncomeFrequency { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "PayCheck gross must be greater than 0")]
    public decimal PayCheckGross { get; set; }

    [Required]
    public Guid BudgetId { get; set; }

    public int DisplayOrder { get; set; }
}