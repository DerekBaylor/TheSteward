using System.ComponentModel.DataAnnotations;

namespace TheSteward.Core.Dtos.FinanceManagerDtos;

public class UpdateBudgetDto
{
    [Required]
    public Guid BudgetId { get; set; }

    [Required]
    [MaxLength(200)]
    public required string BudgetName { get; set; }

    public bool IsDefaultBudget { get; set; }

    public bool IsPublic { get; set; } 
}