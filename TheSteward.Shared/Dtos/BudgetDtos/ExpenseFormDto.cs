namespace TheSteward.Shared.Dtos.BudgetDtos;

public class ExpenseFormDto
{
    public Guid? ExpenseId { get; set; }
    public bool IsEditMode => ExpenseId.HasValue;

    public string ExpenseName { get; set; } = string.Empty;
    public int DueDay { get; set; } = 1;
    public decimal AmountDue { get; set; }
    public int DisplayOrder { get; set; }

    #region Scaler Properties
    public Guid BudgetId { get; set; }
    public Guid CreatedByUserHouseholdId { get; set; }
    public Guid? BudgetCategoryId { get; set; }
    public string BudgetCategoryName { get; set; } = string.Empty;
    public Guid? BudgetSubCategoryId { get; set; }
    public string? BudgetSubCategoryName { get; set; }
    public Guid? CreditId { get; set; }
    public Guid? InvestmentId { get; set; }
    #endregion Scaler Properties
}