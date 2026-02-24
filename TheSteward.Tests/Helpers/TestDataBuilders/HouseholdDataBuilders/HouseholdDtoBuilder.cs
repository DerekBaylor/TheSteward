using TheSteward.Core.Dtos.HouseholdDtos;
using TheSteward.Core.Models;
using TheSteward.Tests.Helpers.TestDataBuilders.UserDataBuilers;

namespace TheSteward.Tests.Helpers.TestDataBuilders.HouseholdDataBuilders;

public class HouseholdDtoBuilder
{
    private Guid _householdId = Guid.NewGuid();
    private string _householdName = "Test Household";
    private bool _isHouseholdActive = true;
    private bool _hasTaskManagerAccess = true;
    private bool _hasFinanceManagerAccess = true;
    private bool _hasKitchenManagerAccess = true;
    private bool _hasFileManagerAccess = true;
    private string _ownerId = Guid.NewGuid().ToString();
    private ApplicationUser? _owner;
    private List<UserHouseholdDto> _userHouseholdDtos = new();

    public HouseholdDtoBuilder WithId(Guid id)
    {
        _householdId = id;
        return this;
    }

    public HouseholdDtoBuilder WithName(string name)
    {
        _householdName = name;
        return this;
    }

    public HouseholdDtoBuilder IsActive(bool isActive)
    {
        _isHouseholdActive = isActive;
        return this;
    }

    public HouseholdDtoBuilder WithTaskManagerAccess(bool hasAccess)
    {
        _hasTaskManagerAccess = hasAccess;
        return this;
    }

    public HouseholdDtoBuilder WithFinanceManagerAccess(bool hasAccess)
    {
        _hasFinanceManagerAccess = hasAccess;
        return this;
    }

    public HouseholdDtoBuilder WithMealManagerAccess(bool hasAccess)
    {
        _hasKitchenManagerAccess = hasAccess;
        return this;
    }

    public HouseholdDtoBuilder WithFileManagerAccess(bool hasAccess)
    {
        _hasFileManagerAccess = hasAccess;
        return this;
    }

    public HouseholdDtoBuilder WithOwner(ApplicationUser owner)
    {
        _owner = owner;
        _ownerId = owner.Id;
        return this;
    }

    public HouseholdDtoBuilder WithOwnerId(string ownerId)
    {
        _ownerId = ownerId;
        return this;
    }

    public HouseholdDtoBuilder WithUserHouseholds(List<UserHouseholdDto> userHouseholds)
    {
        _userHouseholdDtos = userHouseholds;
        return this;
    }

    public HouseholdDto Build()
    {
        // If owner wasn't set, create a default one
        if (_owner == null)
        {
            _owner = new ApplicationUserBuilder()
                .WithId(_ownerId)
                .Build();
        }

        return new HouseholdDto
        {
            HouseholdId = _householdId,
            HouseholdName = _householdName,
            IsHouseholdActive = _isHouseholdActive,
            HasTaskManagerAccess = _hasTaskManagerAccess,
            HasFinanceManagerAccess = _hasFinanceManagerAccess,
            HasKitchenManagerAccess = _hasKitchenManagerAccess,
            HasFileManagerAccess = _hasFileManagerAccess,
            OwnerId = _ownerId,
            Owner = _owner,
            UserHouseholdDtos = _userHouseholdDtos
        };
    }
}