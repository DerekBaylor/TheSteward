using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSteward.Core.Models.KitchenManagerModels;

public class ShoppingList
{
    [Key]
    public Guid ShoppingListId { get; set; }
    public string ShoppingListName { get; set; }
    public DateOnly ShoppingListStartDate  { get; set; }
    public DateOnly ShoppingListEndDate { get; set; }
    
    public Guid KitchenId { get; set; }
    [ForeignKey(nameof(KitchenId))]
    public Kitchen Kitchen { get; set; }
    
    public List<Ingredient> Ingredients { get; set; }
    public List<Equipment> Equipments { get; set; }
    public List<DryGood> DryGoods { get; set; }
}