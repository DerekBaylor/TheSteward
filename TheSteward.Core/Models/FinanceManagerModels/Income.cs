using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TheSteward.Core.Models.FinanceManagerModels;

public class Income
{
    [Key]
    public Guid IncomeId { get; set; }

    [MaxLength(200)]
    public required string IncomeName { get; set; }

    /// <summary>
    /// Monthly = 12, Bi-Monthly = 24, Bi-Weekly = 26, Weekly = 52
    /// </summary>
    [Required]
    public int IncomeFrequency { get; set; }

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
    public decimal? EstStateIncomeTax { get; set; }

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

    //TODO: Add ExpenseCategory navigation property once ExpenseCategories class is created

    #endregion Navigational Properties
}
