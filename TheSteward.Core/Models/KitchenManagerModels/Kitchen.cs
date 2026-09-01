using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheSteward.Core.Models.HouseholdModels;

namespace TheSteward.Core.Models.KitchenManagerModels;

public class Kitchen
{
    [Key]
    public Guid KitchenId { get; set; }
    
    #region Navigation properties
    public Guid HouseholdId { get; set; }
    
    [ForeignKey(nameof(HouseholdId))]
    public Household Household { get; set; }
    
    public Guid ShoppingListId { get; set; }
    
    [ForeignKey(nameof(ShoppingListId))]
    public ShoppingList shoppingList { get; set; }
    
    /// <summary>
    /// Connects to ingredients and dry goods
    /// </summary>
    public Guid PantryId { get; set; }
    
    [ForeignKey(nameof(PantryId))]
    public Pantry Pantry { get; set; }
    
    
    public Guid EquipmentId { get; set; }
    
    [ForeignKey(nameof(EquipmentId))]
    public Equipment Equipment { get; set; }
    #endregion Navigation properties
}