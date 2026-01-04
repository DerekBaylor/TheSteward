using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSteward.Core.Models.FinanceManagerModels;

public class BudgetCategories
{
    [Key]
    public Guid BudgetCategoryId { get; set; }

    [Required]
    [MaxLength(200)]
    public required string BudgetCategoryName { get; set; }
    
    public int DisplayOrder { get; set; }

    #region Navigational Properties
    public Guid BudgetId { get; set; }
    [ForeignKey("BudgetId")]
    public Budget Budget { get; set; }

    #endregion Navigational Properties
}
