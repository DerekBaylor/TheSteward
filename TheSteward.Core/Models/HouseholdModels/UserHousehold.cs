using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSteward.Core.Models.HouseholdModels;

public class UserHousehold
{
    [Key]
    public Guid UserHouseholdId { get; set; }

    [Required]
    public required bool IsDefaultUserHousehold { get; set; }

    [Required]
    public required bool IsHouseholdOwner { get; set; }

    #region Permissions
    
    [Required]
    public required bool HasAdminPermissions { get; set; }

    [Required]
    public required bool HasFinanceManagerWritePermission { get; set; }

    [Required]
    public required bool HasFinanceManagerReadPermission { get; set; }

    [Required]
    public required bool HasKitchenManagerWritePermission { get; set; }

    [Required]
    public required bool HasKitchenManagerReadPermission { get; set; }

    [Required]
    public required bool HasTaskManagerWritePermission { get; set; }

    [Required]
    public required bool HasTaskManagerReadPermission { get; set; }

    [Required]
    public required bool HasFileManagerWritePermission { get; set; }

    [Required]
    public required bool HasFileManagerReadPermission { get; set; }

    #endregion Permissions

    [Required]
    [ForeignKey(nameof(UserId))]
    public required string UserId { get; set; }
    
    public ApplicationUser? User { get; set; }

    public Guid HouseholdId { get; set; }

    [ForeignKey(nameof(HouseholdId))]
    public Household? Household { get; set; }
}
