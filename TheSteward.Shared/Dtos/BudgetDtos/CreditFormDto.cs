using System.ComponentModel.DataAnnotations;
using TheSteward.Core.Dtos.FinanceManagerDtos;
using static TheSteward.Core.Utils.FinanceManagerUtils.FinanceManagerConstants;

namespace TheSteward.Shared.Dtos.BudgetDtos;

public class CreditFormDto
{
    public Guid? CreditId { get; set; }

    [Required(ErrorMessage = "Credit name is required.")]
    [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
    public string CreditName { get; set; } = string.Empty;

    /// <summary>
    /// e.g. "Credit Card", "Auto Loan", "Student Loan", "Mortgage.
    /// </summary>
    [MaxLength(100)]
    public string? CreditType { get; set; }

    [Required(ErrorMessage = "Current balance is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Balance must be $0 or greater.")]
    public decimal CurrentValue { get; set; }

    /// <summary>
    /// Stored as a decimal fraction (e.g. 0.1999 = 19.99%).
    /// </summary>
    [Required(ErrorMessage = "Interest rate is required.")]
    [Range(0, 1, ErrorMessage = "Interest rate must be between 0 and 100%.")]
    public decimal InterestRate { get; set; }

    [Required(ErrorMessage = "Payment amount is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Payment must be $0 or greater.")]
    public decimal PaymentAmount { get; set; }

    [Required(ErrorMessage = "Payment frequency is required.")]
    public FrequencyEnum PaymentFrequency { get; set; } = FrequencyEnum.Monthly;

    /// <summary>
    /// Day of month (1–31). 31 = last day of month.
    /// </summary>
    [Range(1, 31, ErrorMessage = "Payment day must be between 1 and 31.")]
    public int PaymentDay { get; set; } = 1;


    /// <summary>
    /// Estimated monthly interest = CurrentValue * (InterestRate / 12).
    /// </summary>
    public decimal EstMonthlyInterest { get; set; }

    /// <summary>
    /// Estimated yearly interest = CurrentValue * InterestRate.
    /// </summary>
    public decimal EstYearlyInterest { get; set; }


    /// <summary>
    /// The budget category to assign the linked expense to.
    /// On create the service will get-or-create this category.
    /// </summary>
    public Guid? BudgetCategoryId { get; set; }

    /// <summary>
    /// Display name used when the category doesn't exist yet and must be created.
    /// </summary>
    public string BudgetCategoryName { get; set; } = "Debt & Credit";

    public Guid? BudgetSubCategoryId { get; set; }
    public string? BudgetSubCategoryName { get; set; }

    /// <summary>
    /// Id of the expense that mirrors this credit's payment.
    /// </summary>
    public Guid? ExpenseId { get; set; }

    public int DisplayOrder { get; set; }

    public Guid BudgetId { get; set; }

    public bool IsEditMode => CreditId.HasValue && CreditId != Guid.Empty;

    public Guid CreatedById { get; set; }
}
