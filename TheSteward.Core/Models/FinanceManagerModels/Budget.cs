using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheSteward.Core.Models.HouseholdModels;

namespace TheSteward.Core.Models.FinanceManagerModels;

public class Budget
{
    [Key]
    public Guid BudgetId { get; set; }
    
    [Required]
    [MaxLength(200)]
    public required string Name { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public DateTime? ModifiedDate { get; set; }

    public bool IsDefaultBudget { get; set; }

    public bool IsPublic { get; set; }

    [Required]
    public required string OwnerId { get; set; }

    #region Navigational Properties
    public Guid HouseholdId { get; set; }
    
    [ForeignKey("HouseholdId")]
    public required Household Household { get; set; }

    public List<BudgetCategories> BudgetCategories { get; set; } = new();

    #endregion Navigational Properties
}
