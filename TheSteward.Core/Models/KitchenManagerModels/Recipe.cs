using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheSteward.Core.Models.HouseholdModels;
using static TheSteward.Core.Utils.KitchenManagerUtils.KitchenManagerConstants;

namespace TheSteward.Core.Models.KitchenManagerModels;

public class Recipe
{
    [Key]
    public Guid RecipeId { get; set; }
    public string RecipeName { get; set; }
    public string RecipeImgUrl { get; set; }
    public int TotalServings { get; set; }
    public int ServingSize { get; set; }
    public int PrepTime { get; set; }
    public int CookTime { get; set; }
    public int TotalTime { get; set; }
    public Cuisine Cuisine { get; set; }
    public RecipeCategory RecipeCategory { get; set; }
    public RecipePrivacy RecipePrivacy { get; set; }
    public List<RecipeIngredient> RecipeIngredients { get; set; }
    public List<FoodAllergen>  FoodAllergens { get; set; }
    public List<RecipeText>?  RecipeText { get; set; }
    public List<NutritionFact> NutritionFacts { get; set; }
    public List<Equipment> Equipment { get; set; }
    public List<DryGood> DryGoods { get; set; }
    
    public Guid UserHouseholdId { get; set; }
    [ForeignKey(nameof(UserHouseholdId))]
    public UserHousehold UserHousehold { get; set; }
    
    public Guid CookbookId { get; set; }
    [ForeignKey(nameof(CookbookId))]
    public CookBook Cookbook { get; set; }
}