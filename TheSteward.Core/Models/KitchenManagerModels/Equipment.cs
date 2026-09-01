using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static TheSteward.Core.Utils.KitchenManagerUtils.KitchenManagerConstants;

namespace TheSteward.Core.Models.KitchenManagerModels;

public class Equipment
{
    [Key]
    public Guid EquipmentId  { get; set; }
    public string EquipmentName { get; set; }
    public string EquipmentDescription { get; set; }
    public EquipmentCategory Category { get; set; }
    public Guid KitchenId { get; set; }
    
    [ForeignKey(nameof(KitchenId))]
    public Kitchen Kitchen { get; set; }
}