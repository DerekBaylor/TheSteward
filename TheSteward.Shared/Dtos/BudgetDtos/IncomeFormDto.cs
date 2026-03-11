using System.ComponentModel.DataAnnotations;
using static TheSteward.Core.Utils.FinanceManagerUtils.FinanceManagerConstants;

namespace TheSteward.Shared.Dtos.BudgetDtos;

/// <summary>
/// Shared form model used by IncomeForm for both add and edit scenarios.
/// Mapped to CreateIncomeDto or UpdateIncomeDto before sending to the service.
/// </summary>
public class IncomeFormDto
{
    /// <summary>Only set when editing an existing income.</summary>
    public Guid? IncomeId { get; set; }

    [Required(ErrorMessage = "Income name is required.")]
    [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
    public string IncomeName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Pay frequency is required.")]
    [Range(12, 52, ErrorMessage = "Select a valid pay frequency.")]
    public FrequencyEnum IncomeFrequency { get; set; } = FrequencyEnum.Monthly;

    [Required(ErrorMessage = "Filing status is required.")]
    public FilingStatusEnum FilingStatus { get; set; } = FilingStatusEnum.Single;

    [Required(ErrorMessage = "Gross per paycheck is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than $0.")]
    public decimal PayCheckGross { get; set; }

    public int DisplayOrder { get; set; }

    public Guid BudgetId { get; set; }

    public bool IsEditMode => IncomeId.HasValue && IncomeId != Guid.Empty;
}
