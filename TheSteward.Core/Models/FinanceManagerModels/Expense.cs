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

    [ForeignKey("BudgetId")]
    [JsonIgnore]
    public Budget? Budget { get; set; }

    //TODO: Add ExpenseCategory navigation property once ExpenseCategories class is created

    #endregion Navigation Properties
}
