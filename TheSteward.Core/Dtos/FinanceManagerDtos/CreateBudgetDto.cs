using System.ComponentModel.DataAnnotations;

namespace TheSteward.Core.Dtos.FinanceManagerDtos;

public class CreateBudgetDto
{
    [Required]
    [MaxLength(200)]
    public required string BudgetName { get; set; }

    public bool IsDefaultBudget { get; set; }

    public bool IsPublic { get; set; }

    [Required]
    public required string OwnerId { get; set; }

    [Required]
    public Guid HouseholdId { get; set; }
}