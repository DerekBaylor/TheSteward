using System.ComponentModel.DataAnnotations;

namespace TheSteward.Shared.Dtos.BudgetDtos;

public class ExpenseFormDto
{
    public Guid? ExpenseId { get; set; }
    public bool IsEditMode => ExpenseId.HasValue;

    [Required(ErrorMessage = "Expense name is required.")]
    [MaxLength(200, ErrorMessage = "Expense name cannot exceed 200 characters.")]
    public string ExpenseName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Due day is required.")]
    [Range(1, 31, ErrorMessage = "Due day must be between 1 and 31.")]
    public int DueDay { get; set; } = 1;

    [Required(ErrorMessage = "Amount due is required.")]
    [Range(0.00, (double)decimal.MaxValue, ErrorMessage = "Amount due must be 0 or greater.")]
    public decimal AmountDue { get; set; }

    public int DisplayOrder { get; set; }

    #region Scalar Properties

    [Required(ErrorMessage = "Budget is required.")]
    public Guid BudgetId { get; set; }

    [Required(ErrorMessage = "Created by user household is required.")]
    public Guid CreatedByUserHouseholdId { get; set; }

    [Required(ErrorMessage = "Please select a budget category.")]
    public Guid? BudgetCategoryId { get; set; }

    public string BudgetCategoryName { get; set; } = string.Empty;

    // Optional — only required when BudgetCategoryId is set and subcategories exist
    public Guid? BudgetSubCategoryId { get; set; }
    public string? BudgetSubCategoryName { get; set; }

    // Optional links
    public Guid? CreditId { get; set; }
    public Guid? InvestmentId { get; set; }

    #endregion Scalar Properties
}