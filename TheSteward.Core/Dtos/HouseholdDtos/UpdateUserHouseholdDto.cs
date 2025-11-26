using System.ComponentModel.DataAnnotations;
using TheSteward.Core.Models;
using TheSteward.Core.Models.HouseholdModels;

namespace TheSteward.Core.Dtos.HouseholdDtos;

public class UpdateUserHouseholdDto
{
    public Guid? UserHouseholdId { get; set; }
    public bool IsDefaultUserHousehold { get; set; }
    public bool IsHouseholdOwner { get; set; }

    #region Permissions
    public bool HasAdminPermissions { get; set; }
    public bool HasFinanceManagerWritePermission { get; set; }
    public bool HasFinanceManagerReadPermission { get; set; }
    public bool HasKitchenManagerWritePermission { get; set; }
    public bool HasKitchenManagerReadPermission { get; set; }
    public bool HasTaskManagerWritePermission { get; set; }
    public bool HasTaskManagerReadPermission { get; set; }
    public bool HasFileManagerWritePermission { get; set; }
    public bool HasFileManagerReadPermission { get; set; }

    #endregion Permissions

    [Required]
    public required string UserId { get; set; }

    public Guid HouseholdId { get; set; }
}
