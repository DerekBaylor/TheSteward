using System.ComponentModel.DataAnnotations;

namespace TheSteward.Core.Utils.KitchenManagerUtils;

public static class KitchenManagerConstants
{
    public enum RecipeCategory
    {
        Breakfast,
        Brunch,
        Lunch,
        Dinner,
        Dessert,
        Snack,
        Appetizer,
        Soup,
        Side,
        Beverage
    }
    
    public enum RecipePrivacy
    {
        Secret,
        SharedWithHousehold,
        Public
    }
    
    public enum EquipmentCategory
    {
        Cookware,
        Glassware,
        Flatware,
        Appliance
    }
    
    public enum MealCategory
    {
        Breakfast, 
        Brunch,
        Lunch, 
        Dinner,
        Dessert,
        Snack,
        Appetizer,
        Other
    }

    public enum MeasuringType
    {
        Cup,
        Tablespoon, 
        Teaspoon,
        Gallon,
        Liter,
        Milliliter,
        Ounce,
        Pound,
        Gram,
        Pinch,
        [Display(Name="Too Taste")]
        ToTaste,
        Large,
        Small, 
        Medium
    }

    public enum FoodAllergen
    {
        Other,
        Milk,
        Eggs,
        Peanuts,
        [Display(Name = "Tree Nuts")]
        TreeNuts,
        Wheat,
        Soy,
        Glutton,
        Fish,
        Shellfish,
        Sesame
    }

    public enum Cuisine
    {
        American,
        Asian,
        Chinese,
        Japanese,
        Korean,
        Mexican,
        Italian,
        German,
        Indian,
        Mediterranean,
        Seafood,
        British,
        French,
        Greek
    }

    public enum RecipeTextType
    {
        Instruction,
        Note,
        Description
    }
}