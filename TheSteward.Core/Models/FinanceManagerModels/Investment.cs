using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
    public decimal ContributionAmount { get; set; }

    /// <summary>
    /// Monthly = 12, Bi-Monthly = 24, Bi-Weekly = 26, Weekly = 52
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public int ContributionFrequency { get; set; }

    public decimal EstYearlyGrowth { get; set; }

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
