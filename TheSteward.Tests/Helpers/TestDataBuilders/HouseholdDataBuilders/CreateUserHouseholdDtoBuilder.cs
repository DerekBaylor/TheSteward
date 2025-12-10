using TheSteward.Core.Dtos.HouseholdDtos;
using TheSteward.Core.Models;
using TheSteward.Core.Models.HouseholdModels;
using TheSteward.Tests.Helpers.TestDataBuilders.UserDataBuilers;

namespace TheSteward.Tests.Helpers.TestDataBuilders.HouseholdDataBuilders;

public class CreateUserHouseholdDtoBuilder
{
    private Guid? _userHouseholdId = null;
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
    private ApplicationUser? _user;
    private Guid _householdId = Guid.NewGuid();
    private Household? _household;

    public CreateUserHouseholdDtoBuilder WithId(Guid id)
    {
        _userHouseholdId = id;
        return this;
    }

    public CreateUserHouseholdDtoBuilder IsDefault(bool isDefault)
    {
        _isDefaultUserHousehold = isDefault;
        return this;
    }

    public CreateUserHouseholdDtoBuilder IsOwner(bool isOwner)
    {
        _isHouseholdOwner = isOwner;
        return this;
    }

    public CreateUserHouseholdDtoBuilder WithAdminPermissions()
    {
        _hasAdminPermissions = true;
        return this;
    }

    public CreateUserHouseholdDtoBuilder WithAllReadPermissions()
    {
        _hasFinanceManagerReadPermission = true;
        _hasKitchenManagerReadPermission = true;
        _hasTaskManagerReadPermission = true;
        _hasFileManagerReadPermission = true;
        return this;
    }

    public CreateUserHouseholdDtoBuilder WithAllWritePermissions()
    {
        _hasFinanceManagerWritePermission = true;
        _hasKitchenManagerWritePermission = true;
        _hasTaskManagerWritePermission = true;
        _hasFileManagerWritePermission = true;
        return this;
    }

    public CreateUserHouseholdDtoBuilder WithAllPermissions()
    {
        WithAdminPermissions();
        WithAllReadPermissions();
        WithAllWritePermissions();
        return this;
    }

    public CreateUserHouseholdDtoBuilder WithUser(ApplicationUser user)
    {
        _user = user;
        _userId = user.Id;
        return this;
    }

    public CreateUserHouseholdDtoBuilder WithUserId(string userId)
    {
        _userId = userId;
        return this;
    }

    public CreateUserHouseholdDtoBuilder WithHousehold(Household household)
    {
        _household = household;
        _householdId = household.HouseholdId;
        return this;
    }

    public CreateUserHouseholdDtoBuilder WithHouseholdId(Guid householdId)
    {
        _householdId = householdId;
        return this;
    }

    public CreateUserHouseholdDto Build()
    {
        if (_user == null)
        {
            _user = new ApplicationUserBuilder().WithId(_userId).Build();
        }

        if (_household == null)
        {
            _household = new HouseholdBuilder()
                .WithId(_householdId)
                .WithOwner(_user)
                .Build();
        }

        return new CreateUserHouseholdDto
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