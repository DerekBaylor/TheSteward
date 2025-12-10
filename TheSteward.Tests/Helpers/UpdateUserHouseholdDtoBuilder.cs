using TheSteward.Core.Dtos.HouseholdDtos;

namespace TheSteward.Tests.Helpers.TestDataBuilders;

public class UpdateUserHouseholdDtoBuilder
{
    private Guid? _userHouseholdId = Guid.NewGuid();
    private bool _isDefaultUserHousehold = false;
    private bool _isHouseholdOwner = false;
    private bool _hasAdminPermissions = false;
    private bool _hasFinanceManagerWritePermission = false;
    private bool _hasFinanceManagerReadPermission = false;
    private bool _hasKitchenManagerWritePermission = false;
    private bool _hasKitchenManagerReadPermission = false;
    private bool _hasTaskManagerWritePermission = false;
    private bool _hasTaskManagerReadPermission = false;
    private bool _hasFileManagerWritePermission = false;
    private bool _hasFileManagerReadPermission = false;
    private string _userId = Guid.NewGuid().ToString();
    private Guid _householdId = Guid.NewGuid();

    public UpdateUserHouseholdDtoBuilder WithId(Guid id)
    {
        _userHouseholdId = id;
        return this;
    }

    public UpdateUserHouseholdDtoBuilder WithUserId(string userId)
    {
        _userId = userId;
        return this;
    }

    public UpdateUserHouseholdDtoBuilder WithHouseholdId(Guid householdId)
    {
        _householdId = householdId;
        return this;
    }

    public UpdateUserHouseholdDtoBuilder WithAllPermissions()
    {
        _hasAdminPermissions = true;
        _hasFinanceManagerWritePermission = true;
        _hasFinanceManagerReadPermission = true;
        _hasKitchenManagerWritePermission = true;
        _hasKitchenManagerReadPermission = true;
        _hasTaskManagerWritePermission = true;
        _hasTaskManagerReadPermission = true;
        _hasFileManagerWritePermission = true;
        _hasFileManagerReadPermission = true;
        return this;
    }

    public UpdateUserHouseholdDto Build()
    {
        return new UpdateUserHouseholdDto
        {
            UserHouseholdId = _userHouseholdId,
            IsDefaultUserHousehold = _isDefaultUserHousehold,
            IsHouseholdOwner = _isHouseholdOwner,
            HasAdminPermissions = _hasAdminPermissions,
            HasFinanceManagerWritePermission = _hasFinanceManagerWritePermission,
            HasFinanceManagerReadPermission = _hasFinanceManagerReadPermission,
            HasKitchenManagerWritePermission = _hasKitchenManagerWritePermission,
            HasKitchenManagerReadPermission = _hasKitchenManagerReadPermission,
            HasTaskManagerWritePermission = _hasTaskManagerWritePermission,
            HasTaskManagerReadPermission = _hasTaskManagerReadPermission,
            HasFileManagerWritePermission = _hasFileManagerWritePermission,
            HasFileManagerReadPermission = _hasFileManagerReadPermission,
            UserId = _userId,
            HouseholdId = _householdId
        };
    }
}