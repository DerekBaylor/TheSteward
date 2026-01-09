using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal EstMonthlyInterest { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal EstYearlyInterest { get; set; }

    /// <summary>
    /// Monthly = 12, Bi-Monthly = 24, Bi-Weekly = 26, Weekly = 52
    /// </summary>
    [Required]
    public int PaymentFrequency { get; set; }

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

    [ForeignKey("BudgetId")]
    [JsonIgnore]
    public Budget? Budget { get; set; }

    //TODO: Add ExpenseCategory navigation property once ExpenseCategories class is created

    #endregion Navigation Properties
}
