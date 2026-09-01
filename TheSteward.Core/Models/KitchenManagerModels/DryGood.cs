using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSteward.Core.Models.KitchenManagerModels;

public class DryGood
{
    [Key]
    public Guid DryGoodId { get; set; }
    public string DryGoodName { get; set; }
    public int PurchaseQuantity { get; set; }
    public int CurrentQuantity { get; set; }
    public int MinimumQuantity { get; set; }
    public int MaximumQuantity { get; set; }
    
    
    public Guid PantryId { get; set; }
    [ForeignKey(nameof(PantryId))]
    public Pantry Pantry { get; set; }
}