using System.ComponentModel.DataAnnotations;

namespace TheSteward.Core.Dtos.FinanceManagerDtos;

public class ExpenseDto
{
    public Guid ExpenseId { get; set; }
    
    [MaxLength(200)]
    public required string ExpenseName { get; set; }

    /// <summary>
    /// Day of the month (1-31). Use 31 for "last day of month"
    /// </summary>
    public int DueDay { get; set; }
    
    public decimal AmountDue { get; set; }
    
    public int DisplayOrder { get; set; }

    #region Navigation Properties
    
    public Guid BudgetId { get; set; }
    
    public Guid BudgetCategoryId { get; set; }
    public BudgetCategoryDto BudgetCategory { get; set; }
    
    public Guid? BudgetSubCategoryId { get; set; }
    public BudgetSubCategoryDto BudgetSubCategory { get; set; }
    
    public Guid? CreditId { get; set; }
    public CreditDto? LinkedCreditDto { get; set; }
    
    public Guid? InvestmentId { get; set; }
    public InvestmentDto? LinkedInvestmentDto { get; set; }
    
    #endregion Navigation Properties
}