using System.ComponentModel.DataAnnotations;

namespace TheSteward.Core.Dtos.FinanceManagerDtos;

public class BudgetCategoryDto
{
    public Guid BudgetCategoryId { get; set; }

    [Required]
    [MaxLength(200)]
    public required string BudgetCategoryName { get; set; }
    
    public int DisplayOrder { get; set; }

    public Guid BudgetId { get; set; }
    
    public List<BudgetSubCategoryDto> BudgetSubCategories { get; set; } = new();
}