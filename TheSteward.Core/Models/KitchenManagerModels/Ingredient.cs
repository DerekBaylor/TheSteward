using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static TheSteward.Core.Utils.KitchenManagerUtils.KitchenManagerConstants;

namespace TheSteward.Core.Models.KitchenManagerModels;

public class Ingredient
{
    [Key]
    public Guid IngredientId { get; set; }
    
    public string IngredientName { get; set; }
    
    public MeasuringType MeasuringType { get; set; }
    public MeasuringType BuyingAmountType { get; set; }
    
    public int PurchaseQuantity { get; set; }
    
    public int CurrentQuantity { get; set; }
    
    public int MinimumQuantity { get; set; }
    
    public int MaximumQuantity { get; set; }
    
    public string Notes { get; set; }

    public Guid PantryId { get; set; }
    
    [ForeignKey(nameof(PantryId))]
    public Pantry Pantry { get; set; }
    
    public Recipe Recipe { get; set; }
    
    
}