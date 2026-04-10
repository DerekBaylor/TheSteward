using System.ComponentModel.DataAnnotations;

namespace TheSteward.Core.Dtos.FinanceManagerDtos;

public class CreateExpenseDto
{
    [Required]
    [MaxLength(200)]
    public required string ExpenseName { get; set; }

    /// <summary>
    /// Day of the month (1-31). Use 31 for "last day of month"
    /// </summary>
    [Range(1, 31)]
    public int DueDay { get; set; }
    
    [Required]
    [Range(0.00, (double)decimal.MaxValue, ErrorMessage = "Amount due must be 0 or greater.")]
    public decimal AmountDue { get; set; }
    
    public int DisplayOrder { get; set; }

    #region Scaler Properties

    [Required]
    public Guid BudgetId { get; set; }
    public Guid? CreditId { get; set; }

    public Guid? InvestmentId { get; set; }

    public required Guid CreatedByUserHouseholdId { get; set; }

    public Guid HouseholdId { get; set; }

    #endregion Scaler Properties

    #region Navigation Properties

    [Required]
    public Guid BudgetCategoryId { get; set; }
    
    public Guid? BudgetSubCategoryId { get; set; }    
    
    #endregion Navigation Properties
}