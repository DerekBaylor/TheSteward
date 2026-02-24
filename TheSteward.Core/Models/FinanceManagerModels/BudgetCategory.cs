using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSteward.Core.Models.FinanceManagerModels;

public class BudgetCategory
{
    [Key]
    public Guid BudgetCategoryId { get; set; }

    [Required]
    [MaxLength(200)]
    public required string BudgetCategoryName { get; set; }
    
    public int DisplayOrder { get; set; }

    #region Navigational Properties
    [Required]
    public Guid BudgetId { get; set; }
    
    [ForeignKey("BudgetId")]
    public Budget? Budget { get; set; }
    
    public List<BudgetSubCategory>? BudgetSubCategories { get; set; }
    #endregion Navigational Properties
}
