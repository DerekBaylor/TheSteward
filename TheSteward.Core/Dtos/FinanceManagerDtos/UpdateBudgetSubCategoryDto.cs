using System.ComponentModel.DataAnnotations;

namespace TheSteward.Core.Dtos.FinanceManagerDtos;

public class UpdateBudgetSubCategoryDto
{
    public Guid BudgetSubCategoryId { get; set; }
    
    [Required]
    [MaxLength(200)]
    public required string BudgetSubCategoryName { get; set; }
    
    public int DisplayOrder { get; set; }
    
    [Required]
    public Guid BudgetId { get; set; }
    
    [Required]
    public Guid BudgetCategoryId { get; set; }
}
