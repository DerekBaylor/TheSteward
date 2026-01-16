using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TheSteward.Core.Models.FinanceManagerModels;

public class Expense
{
    [Key]
    public Guid ExpenseId { get; set; }

    [Required]
    [MaxLength(200)]
    public required string ExpenseName { get; set; }

    /// <summary>
    /// Day of the month (1-31). Use 31 for "last day of month"
    /// </summary>
    [Required]
    public int DueDay { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal AmountDue { get; set; }

    [Required]
    public int DisplayOrder { get; set; }

    #region Navigation Properties
    [Required]
    public Guid BudgetId { get; set; }

    [JsonIgnore]
    [ForeignKey("BudgetId")]
    public Budget? Budget { get; set; }

    [Required]
    public Guid BudgetCategoryId { get; set; }
    public BudgetCategory? BudgetCategory { get; set; }

    public Guid? CreditId { get; set; }
    public Credit? LinkedCredit { get; set; }

    public Guid? InvestmentId { get; set; }
    public Investment? LinkedInvestment { get; set; }

    #endregion Navigation Properties
}
