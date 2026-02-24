using TheSteward.Core.Models;
using TheSteward.Core.Models.HouseholdModels;
using TheSteward.Tests.Helpers.TestDataBuilders.UserDataBuilers;

namespace TheSteward.Tests.Helpers.TestDataBuilders.HouseholdDataBuilders;

public class HouseholdBuilder
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

    public HouseholdBuilder WithId(Guid id)
    {
        _householdId = id;
        return this;
    }

    public HouseholdBuilder WithName(string name)
    {
        _householdName = name;
        return this;
    }

    public HouseholdBuilder WithOwner(ApplicationUser owner)
    {
        _owner = owner;
        _ownerId = owner.Id;
        return this;
    }

    public HouseholdBuilder WithOwnerId(string ownerId)
    {
        _ownerId = ownerId;
        return this;
    }

    public HouseholdBuilder IsActive(bool isActive)
    {
        _isHouseholdActive = isActive;
        return this;
    }

    public HouseholdBuilder WithTaskManagerAccess(bool hasAccess)
    {
        _hasTaskManagerAccess = hasAccess;
        return this;
    }

    public HouseholdBuilder WithFinanceManagerAccess(bool hasAccess)
    {
        _hasFinanceManagerAccess = hasAccess;
        return this;
    }

    public HouseholdBuilder WithMealManagerAccess(bool hasAccess)
    {
        _hasKitchenManagerAccess = hasAccess;
        return this;
    }

    public HouseholdBuilder WithFileManagerAccess(bool hasAccess)
    {
        _hasFileManagerAccess = hasAccess;
        return this;
    }

    public Household Build()
    {
        // If owner wasn't set, create a default one
        if (_owner == null)
        {
            _owner = new ApplicationUserBuilder()
                .WithId(_ownerId)
                .Build();
        }

        return new Household
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
            UserHouseholds = new List<UserHousehold>()
        };
    }
}