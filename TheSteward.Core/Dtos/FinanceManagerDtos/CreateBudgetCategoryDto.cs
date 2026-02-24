using System.ComponentModel.DataAnnotations;

namespace TheSteward.Core.Dtos.FinanceManagerDtos;

public class CreateBudgetCategoryDto
{
    public Guid BudgetCategoryId { get; set; }

    [Required]
    [MaxLength(200)]
    public required string BudgetCategoryName { get; set; }
    
    public int DisplayOrder { get; set; }

    [Required]
    public Guid BudgetId { get; set; }
}