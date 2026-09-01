using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSteward.Core.Models.KitchenManagerModels;

public class NutritionFact
{
    [Key]
    public Guid NutritionId { get; set; }
    public string NutritionName { get; set; }
    public string NutritionDescription { get; set; }
    
    public Guid RecipeId { get; set; }
    
    [ForeignKey(nameof(RecipeId))]
    public Recipe Recipe { get; set; } 
}