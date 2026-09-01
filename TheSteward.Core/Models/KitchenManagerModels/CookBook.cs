using System.ComponentModel.DataAnnotations.Schema;
using TheSteward.Core.Models.HouseholdModels;

namespace TheSteward.Core.Models.KitchenManagerModels;

public class CookBook
{
    public Guid CookBookId { get; set; }
    public String CookBookName { get; set; }
    public String CookBookDescription { get; set; }
    
    public Guid UserHouseholdId { get; set; }
    [ForeignKey(nameof(UserHouseholdId))]
    public UserHousehold UserHousehold { get; set; }
    
    public List<Recipe> Recipes { get; set; }
}