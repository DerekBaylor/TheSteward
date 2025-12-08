using TheSteward.Core.Models;
using TheSteward.Core.Models.HouseholdModels;

namespace TheSteward.Tests.Helpers.TestDataBuilders;

public class UserHouseholdBuilder
{
    private Guid _userHouseholdId = Guid.NewGuid();
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
    private ApplicationUser? _user;
    private Household? _household;

    public UserHouseholdBuilder WithId(Guid id)
    {
        _userHouseholdId = id;
        return this;
    }

    public UserHouseholdBuilder IsDefault(bool isDefault)
    {
        _isDefaultUserHousehold = isDefault;
        return this;
    }

    public UserHouseholdBuilder IsOwner(bool isOwner)
    {
        _isHouseholdOwner = isOwner;
        return this;
    }

    public UserHouseholdBuilder WithAdminPermissions()
    {
        _hasAdminPermissions = true;
        return this;
    }

    public UserHouseholdBuilder WithAllReadPermissions()
    {
        _hasFinanceManagerReadPermission = true;
        _hasKitchenManagerReadPermission = true;
        _hasTaskManagerReadPermission = true;
        _hasFileManagerReadPermission = true;
        return this;
    }

    public UserHouseholdBuilder WithAllWritePermissions()
    {
        _hasFinanceManagerWritePermission = true;
        _hasKitchenManagerWritePermission = true;
        _hasTaskManagerWritePermission = true;
        _hasFileManagerWritePermission = true;
        return this;
    }

    public UserHouseholdBuilder WithUser(ApplicationUser user)
    {
        _user = user;
        _userId = user.Id;
        return this;
    }

    public UserHouseholdBuilder WithUserId(string userId)
    {
        _userId = userId;
        return this;
    }

    public UserHouseholdBuilder WithHousehold(Household household)
    {
        _household = household;
        _householdId = household.HouseholdId;
        return this;
    }

    public UserHouseholdBuilder WithHouseholdId(Guid householdId)
    {
        _householdId = householdId;
        return this;
    }

    public UserHousehold Build()
    {
        return new UserHousehold
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
            User = _user,
            HouseholdId = _householdId,
            Household = _household
        };
    }
}