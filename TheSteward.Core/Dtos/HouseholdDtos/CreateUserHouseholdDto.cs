using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheSteward.Core.Models;
using TheSteward.Core.Models.HouseholdModels;

namespace TheSteward.Core.Dtos.HouseholdDtos;

public class CreateUserHouseholdDto
{
    public Guid? UserHouseholdId { get; set; }
    
    [Required]
    [MaxLength(200)]
    public required string UserName { get; set; }

    public bool IsDefaultUserHousehold { get; set; }

    public bool IsHouseholdOwner { get; set; }

    public bool IsActive { get; set; }

    #region Permissions
    public bool HasAdminPermissions { get; set; }
    public bool HasFinanceManagerReadPermission { get; set; }
    public bool HasFinanceManagerWritePermission { get; set; }
    public bool HasKitchenManagerReadPermission { get; set; }
    public bool HasKitchenManagerWritePermission { get; set; }
    public bool HasTaskManagerReadPermission { get; set; }
    public bool HasTaskManagerWritePermission { get; set; }
    public bool HasTaskManagerCompletePermission { get; set; }
    public bool HasFileManagerReadPermission { get; set; }
    public bool HasFileManagerWritePermission { get; set; }

    #endregion Permissions

    [Required]
    [ForeignKey(nameof(UserId))]
    public required string UserId { get; set; }

    [Required]
    public required ApplicationUser User { get; set; }

    public Guid HouseholdId { get; set; }

    [ForeignKey(nameof(HouseholdId))]
    public required Household Household { get; set; }
}

