using System.ComponentModel.DataAnnotations;
using TheSteward.Core.Models.TaskManagerModels;
using static TheSteward.Core.Utils.KitchenManagerUtils.KitchenManagerConstants;

namespace TheSteward.Core.Models.KitchenManagerModels;

public class PlannedMeal
{
    [Key]
    public Guid PlannedMealId { get; set; }
    
    public DateOnly Date { get; set; }
    
    public MealCategory MealCategory { get; set; }
    
    
    public List<Recipe> Recipes { get; set; }
    
    public TaskItem TaskItem { get; set; }
}