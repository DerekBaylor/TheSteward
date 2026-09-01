namespace TheSteward.Core.Models.KitchenManagerModels;
using static TheSteward.Core.Utils.KitchenManagerUtils.KitchenManagerConstants;


public class RecipeIngredient
{
    public Guid RecipeIngredientId { get; set; }
    public Guid IngredientId { get; set; }
    public Guid RecipeId { get; set; }
    public Ingredient Ingredient { get; set; }
    public Recipe Recipe { get; set; }
    public int RecipeQuantity { get; set; }
    public MeasuringType MeasuringType { get; set; }
}