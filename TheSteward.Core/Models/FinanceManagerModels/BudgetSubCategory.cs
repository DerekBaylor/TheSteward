using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSteward.Core.Models.FinanceManagerModels;

public class BudgetSubCategory
{
    [Key]
    public Guid BudgetSubCategoryId { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string BudgetSubCategoryName { get; set; }
    
    public int DisplayOrder { get; set; }
    
    #region Navigation Properties
    [Required]
    public Guid BudgetId { get; set; }
    
    [ForeignKey("BudgetId")]
    public Budget? Budget { get; set; }
    
    [Required]
    public Guid BudgetCategoryId { get; set; }
    
    [ForeignKey("BudgetCategoryId")]
    public BudgetCategory? BudgetCategory { get; set; }
    #endregion
}