using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSteward.Core.Models;

public class Household
{

    public Guid HouseholdId { get; set; }

    [Required]
    [MaxLength(200)]
    public required string HouseholdName { get; set; }

    public bool IsHouseholdActive { get; set; }

    #region Household Modules
    public bool HasFinanceManagerModule { get; set; }

    public bool HasKitchenManagerModule { get; set; }

    public bool HasTaskManagerModule { get; set; }

    public bool HasFileManagerModule { get; set; }
    #endregion Household Modules

    #region Navigational Properties
    public required string UserId { get; set; }
    [ForeignKey("UserId")]
    public ApplicationUser Owner { get; set; } = default!;

    #endregion Navigational Properties
}
