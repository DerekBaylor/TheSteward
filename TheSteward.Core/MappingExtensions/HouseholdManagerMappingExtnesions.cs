using TheSteward.Core.Dtos.HouseholdDtos;
using TheSteward.Core.Models;
using TheSteward.Core.Models.HouseholdModels;

namespace TheSteward.Core.MappingExtensions;

public static class HouseholdManagerMappingExtensions
{
    #region Household Mapping

    /// <summary>
    /// Converts a Household entity to its corresponding HouseholdDto representation.
    /// Explicit override required — UserHouseholdDtos is flattened from the UserHouseholds
    /// navigation collection and the circular reference guard on includeHousehold: false
    /// cannot be expressed generically.
    /// </summary>
    public static HouseholdDto ToDto(this Household src) => new()
    {
        HouseholdId = src.HouseholdId,
        HouseholdName = src.HouseholdName,
        IsHouseholdActive = src.IsHouseholdActive,
        HasTaskManagerAccess = src.HasTaskManagerAccess,
        HasFinanceManagerAccess = src.HasFinanceManagerAccess,
        HasKitchenManagerAccess = src.HasKitchenManagerAccess,
        HasFileManagerAccess = src.HasFileManagerAccess,
        OwnerId = src.OwnerId,
        Owner = src.Owner,
        UserHouseholdDtos = src.UserHouseholds?
                             .Select(uh => uh.ToDto(includeHousehold: false))
                             .ToList() ?? new List<UserHouseholdDto>()
    };

    /// <summary>
    /// Converts a sequence of Household entities to a list of HouseholdDto objects.
    /// </summary>
    public static List<HouseholdDto> ToDtoList(this IEnumerable<Household> src)
        => src.Select(h => h.ToDto()).ToList();

    /// <summary>
    /// Creates a new Household entity from the specified CreateHouseholdDto and owner information.
    /// Explicit override required — takes extra parameters not present on the DTO and
    /// hard-codes IsHouseholdActive = true on creation.
    /// </summary>
    public static Household ToEntity(this CreateHouseholdDto src, string ownerId, ApplicationUser owner)
    {
        var entity = GenericMapper.Map<CreateHouseholdDto, Household>(src);
        entity.OwnerId = ownerId;
        entity.Owner = owner;

        return entity;
    }


    /// <summary>
    /// Updates the properties of the specified Household entity with values from the
    /// provided UpdateHouseholdDto. All property names match exactly between DTO and
    /// entity so GenericMapper handles the full update.
    /// </summary>
    public static void ApplyUpdate(this Household entity, UpdateHouseholdDto src)
        => GenericMapper.MapProperties(src, entity);

    #endregion Household Mapping

    #region UserHousehold Mapping

    /// <summary>
    /// Converts a UserHousehold entity to its corresponding UserHouseholdDto representation.
    /// Explicit override required — the includeHousehold flag guards against circular
    /// references when mapping from within a Household context.
    /// </summary>
    public static UserHouseholdDto ToDto(this UserHousehold src, bool includeHousehold = true) => new()
    {
        UserHouseholdId = src.UserHouseholdId,
        UserName = src.UserName,
        IsDefaultUserHousehold = src.IsDefaultUserHousehold,
        IsHouseholdOwner = src.IsHouseholdOwner,
        IsActive = src.IsActive,

        HasAdminPermissions = src.HasAdminPermissions,
        HasFinanceManagerReadPermission = src.HasFinanceManagerReadPermission,
        HasFinanceManagerWritePermission = src.HasFinanceManagerWritePermission,
        HasKitchenManagerReadPermission = src.HasKitchenManagerReadPermission,
        HasKitchenManagerWritePermission = src.HasKitchenManagerWritePermission,
        HasTaskManagerReadPermission = src.HasTaskManagerReadPermission,
        HasTaskManagerCompletePermission = src.HasTaskManagerCompletePermission,
        HasTaskManagerWritePermission = src.HasTaskManagerWritePermission,
        HasFileManagerReadPermission = src.HasFileManagerReadPermission,
        HasFileManagerWritePermission = src.HasFileManagerWritePermission,

        UserId = src.UserId,
        User = src.User,
        HouseholdId = src.HouseholdId,
        Household = includeHousehold ? src.Household?.ToDto() : null,
        DefaultBudgetId = src.DefaultBudgetId
    };

    /// <summary>
    /// Converts a sequence of UserHousehold entities to a list of UserHouseholdDto objects.
    /// </summary>
    public static List<UserHouseholdDto> ToDtoList(
        this IEnumerable<UserHousehold> src, bool includeHousehold = true)
        => src.Select(uh => uh.ToDto(includeHousehold)).ToList();

    /// <summary>
    /// Converts a CreateUserHouseholdDto to a UserHousehold entity.
    /// GenericMapper handles all scalar and permission properties.
    /// UserHouseholdId is resolved after the generic copy since it requires
    /// null-coalescing logic not expressible generically.
    /// </summary>
    public static UserHousehold ToEntity(this CreateUserHouseholdDto src)
    {
        var entity = GenericMapper.Map<CreateUserHouseholdDto, UserHousehold>(src);
        // UserHouseholdId is nullable on the DTO — coalesce after the generic copy
        entity.UserHouseholdId = src.UserHouseholdId ?? Guid.NewGuid();
        return entity;
    }

    /// <summary>
    /// Updates the properties of the specified UserHousehold entity with values from the
    /// provided UpdateUserHouseholdDto. All property names match exactly between DTO and
    /// entity so GenericMapper handles the full update.
    /// </summary>
    public static void ApplyUpdate(this UserHousehold entity, UpdateUserHouseholdDto src)
        => GenericMapper.MapProperties(src, entity);

    /// <summary>
    /// Converts a HouseholdInvitation entity to its corresponding HouseholdInvitationDto.
    /// Explicit override required — HouseholdName and InvitedByUserName are flattened
    /// from navigation properties. Ensure Household and InvitedByUser are eagerly loaded
    /// before calling this method.
    /// </summary>
    public static HouseholdInvitationDto ToDto(this HouseholdInvitation src) => new()
    {
        InvitationId = src.InvitationId,
        HouseholdId = src.HouseholdId,
        HouseholdName = src.Household.HouseholdName,
        InvitedByUserName = src.InvitedByUser.UserName ?? src.InvitedByUserId,
        InvitedUserEmail = src.InvitedUserEmail,
        InvitedDate = src.InvitedDate,
        ExpirationDate = src.ExpirationDate,
        IsAccepted = src.IsAccepted
    };

    /// <summary>
    /// Converts a sequence of HouseholdInvitation entities to a list of HouseholdInvitationDto objects.
    /// </summary>
    public static List<HouseholdInvitationDto> ToDtoList(this IEnumerable<HouseholdInvitation> src)
        => src.Select(i => i.ToDto()).ToList();

    #endregion UserHousehold Mapping
}


