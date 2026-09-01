using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static TheSteward.Core.Utils.KitchenManagerUtils.KitchenManagerConstants;

namespace TheSteward.Core.Models.KitchenManagerModels;

public class RecipeText
{
    [Key]
    public Guid RecipeTextId { get; set; }

    public int DisplayOrder { get; set; }
    public string Text { get; set; }
    
    public RecipeTextType Type { get; set; }
    
    public Guid RecipeId { get; set; }
    
    [ForeignKey(nameof(RecipeId))]
    public Recipe Recipe { get; set; }
}