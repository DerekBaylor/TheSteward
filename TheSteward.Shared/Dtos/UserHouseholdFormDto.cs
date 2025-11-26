using System.ComponentModel.DataAnnotations;
using TheSteward.Core.Attributes;

namespace TheSteward.Shared.Dtos;

public class UserHouseholdFormDto
{
    public Guid UserHouseholdId { get; set; }

    [Required]
    public string MemberName { get; set; } = string.Empty;

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
}
