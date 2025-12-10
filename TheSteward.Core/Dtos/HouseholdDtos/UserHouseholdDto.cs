using System.ComponentModel.DataAnnotations;
using TheSteward.Core.Attributes;
using TheSteward.Core.Models;

namespace TheSteward.Core.Dtos.HouseholdDtos;

public class UserHouseholdDto
{
    public Guid UserHouseholdId { get; set; }

    public bool IsDefaultUserHousehold { get; set; }

    public bool IsHouseholdOwner { get; set; }

    #region Permissions

    [UserPermission("Admin Permissions", "General")]
    public bool HasAdminPermissions { get; set; }

    [UserPermission("Finance Manager - Write", "Finance")]
    public bool HasFinanceManagerWritePermission { get; set; }

    [UserPermission("Finance Manager - Read", "Finance")]
    public bool HasFinanceManagerReadPermission { get; set; }

    [UserPermission("Kitchen Manager - Write", "Kitchen")]
    public bool HasKitchenManagerWritePermission { get; set; }

    [UserPermission("Kitchen Manager - Read", "Kitchen")]
    public bool HasKitchenManagerReadPermission { get; set; }

    [UserPermission("Task Manager - Write", "Tasks")]
    public bool HasTaskManagerWritePermission { get; set; }

    [UserPermission("Task Manager - Read", "Tasks")]
    public bool HasTaskManagerReadPermission { get; set; }

    [UserPermission("File Manager - Write", "Files")]
    public bool HasFileManagerWritePermission { get; set; }

    [UserPermission("File Manager - Read")]
    public bool HasFileManagerReadPermission { get; set; }

    #endregion Permissions

    [Required]
    public string UserId { get; set; }

    public ApplicationUser User { get; set; }

    public Guid HouseholdId { get; set; }

    public HouseholdDto Household { get; set; }
}
