using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSteward.Core.Models.KitchenManagerModels;

public class Pantry
{
    [Key]
    public Guid PantryId { get; set; }
    public Guid KitchenId { get; set; }
    [ForeignKey(nameof(KitchenId))]
    public Kitchen Kitchen { get; set; }
    
    public List<Ingredient> Ingredients { get; set; } 
    public List<Equipment> Equipment { get; set; }
    public List<DryGood> DryGoods { get; set; }
}