using System.ComponentModel.DataAnnotations;

namespace TheSteward.Core.Dtos.FinanceManagerDtos;

public class UpdateBudgetSubCategoryDto
{
    public Guid BudgetSubCategoryId { get; set; }
    
    [Required]
    [MaxLength(200)]
    public required string BudgetSubCategoryName { get; set; }
    
    public int DisplayOrder { get; set; }
    
    public Guid BudgetId { get; set; }
    public Guid BudgetCategoryId { get; set; }
}
