using System.ComponentModel.DataAnnotations;
using static TheSteward.Core.Utils.FinanceManagerUtils.FinanceManagerConstants;

namespace TheSteward.Core.Dtos.FinanceManagerDtos;

public class CreateCreditDto
{
    [Required]
    [MaxLength(200)]
    public required string CreditName { get; set; }

    [MaxLength(100)]
    public string? CreditType { get; set; }

    public decimal InterestRate { get; set; }
    public decimal CurrentValue { get; set; }
    public decimal EstMonthlyInterest { get; set; }
    public decimal EstYearlyInterest { get; set; }

    public FrequencyEnum PaymentFrequency { get; set; }
    public decimal PaymentAmount { get; set; }

    /// <summary>
    /// Day of month (1–31). 31 = last day of month.
    /// </summary>
    public int PaymentDay { get; set; } = 1;

    public int DisplayOrder { get; set; }

    #region Scaler Properties

    [Required]
    public Guid BudgetId { get; set; }

    /// <summary>
    /// Optional: pass an existing category ID to skip lookup.
    /// If null the service will get-or-create by <see cref="BudgetCategoryName"/>.
    /// </summary>
    public Guid? BudgetCategoryId { get; set; }
    
    /// <summary>
    /// Used to get-or-create the expense category when <see cref="BudgetCategoryId"/> is not supplied.
    /// </summary>
    public string BudgetCategoryName { get; set; } = "Debt & Credit";

    public Guid? BudgetSubCategoryId { get; set; }

    /// <summary>
    /// Used to get-or-create a subcategory when <see cref="BudgetSubCategoryId"/> is not supplied.
    /// </summary>
    public string? BudgetSubCategoryName { get; set; }

    public Guid? ExpenseId { get; set; }

    public Guid CreatedByUserHouseholdId { get; set; }

    #endregion  Scaler Properties
}