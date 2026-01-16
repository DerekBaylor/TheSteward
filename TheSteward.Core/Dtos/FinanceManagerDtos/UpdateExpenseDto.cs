using System.ComponentModel.DataAnnotations;

namespace TheSteward.Core.Dtos.FinanceManagerDtos;

public class UpdateExpenseDto
{
    [Required]
    public Guid ExpenseId { get; set; }

    [Required]
    [MaxLength(200)]
    public required string ExpenseName { get; set; }

    /// <summary>
    /// Day of the month (1-31). Use 31 for "last day of month"
    /// </summary>
    [Required]
    [Range(1, 31, ErrorMessage = "Due day must be between 1 and 31")]
    public int DueDay { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount due must be greater than 0")]
    public decimal AmountDue { get; set; }
    
    public int DisplayOrder { get; set; }

    #region Navigation Properties
    
    [Required]
    public Guid BudgetId { get; set; }

    [Required]
    public Guid BudgetCategoryId { get; set; }

    public Guid? CreditId { get; set; }

    public Guid? InvestmentId { get; set; }

    #endregion Navigation Properties 
}