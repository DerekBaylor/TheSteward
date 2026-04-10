using System.ComponentModel.DataAnnotations;
using static TheSteward.Core.Utils.FinanceManagerUtils.FinanceManagerConstants;

namespace TheSteward.Shared.Dtos.BudgetDtos;

public class InvestmentFormDto
{
    /// <summary>Only set when editing an existing investment.</summary>
    public Guid? InvestmentId { get; set; }

    [Required(ErrorMessage = "Investment name is required.")]
    [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
    public string InvestmentName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Current value is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Current value must be $0 or greater.")]
    public decimal CurrentValue { get; set; }

    [Required(ErrorMessage = "Interest rate is required.")]
    [Range(0, 1, ErrorMessage = "Interest rate must be between 0 and 100%.")]
    public decimal InterestRate { get; set; }

    [Required(ErrorMessage = "Contribution amount is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Contribution must be $0 or greater.")]
    public decimal ContributionAmount { get; set; }

    [Required(ErrorMessage = "Contribution frequency is required.")]
    public FrequencyEnum ContributionFrequency { get; set; } = FrequencyEnum.Monthly;

    /// <summary>
    /// Estimated yearly growth — calculated on save by the service,
    /// but also computed client-side for the live preview.
    /// </summary>
    public decimal EstYearlyGrowth { get; set; }

    /// <summary>
    /// Links this investment to a budget expense (e.g. a recurring contribution line item).
    /// Required on the entity — leave as <see cref="Guid.Empty"/> if not yet linked.
    /// </summary>
    public Guid? ExpenseId { get; set; }

    public int DisplayOrder { get; set; }

    public Guid BudgetId { get; set; }

    public bool IsEditMode => InvestmentId.HasValue && InvestmentId != Guid.Empty;

    public Guid HouseholdId { get; set; }
}
