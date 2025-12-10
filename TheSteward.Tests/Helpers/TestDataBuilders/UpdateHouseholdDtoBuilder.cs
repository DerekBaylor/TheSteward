// Helpers/TestDataBuilders/UpdateHouseholdDtoBuilder.cs
using TheSteward.Core.Dtos.HouseholdDtos;

namespace TheSteward.Tests.Helpers.TestDataBuilders;

public class UpdateHouseholdDtoBuilder
{
    private Guid _householdId = Guid.NewGuid();
    private string _householdName = "Updated Household";
    private bool _isDefaultHousehold = false;
    private bool _hasTaskManagerAccess = true;
    private bool _hasFinanceManagerAccess = true;
    private bool _hasMealManagerAccess = true;
    private bool _hasFileManagerAccess = true;

    public UpdateHouseholdDtoBuilder WithId(Guid id)
    {
        _householdId = id;
        return this;
    }

    public UpdateHouseholdDtoBuilder WithName(string name)
    {
        _householdName = name;
        return this;
    }

    public UpdateHouseholdDtoBuilder IsDefault(bool isDefault)
    {
        _isDefaultHousehold = isDefault;
        return this;
    }

    public UpdateHouseholdDtoBuilder WithTaskManagerAccess(bool hasAccess)
    {
        _hasTaskManagerAccess = hasAccess;
        return this;
    }

    public UpdateHouseholdDtoBuilder WithFinanceManagerAccess(bool hasAccess)
    {
        _hasFinanceManagerAccess = hasAccess;
        return this;
    }

    public UpdateHouseholdDtoBuilder WithMealManagerAccess(bool hasAccess)
    {
        _hasMealManagerAccess = hasAccess;
        return this;
    }

    public UpdateHouseholdDtoBuilder WithFileManagerAccess(bool hasAccess)
    {
        _hasFileManagerAccess = hasAccess;
        return this;
    }

    public UpdateHouseholdDtoBuilder WithAllAccess()
    {
        _hasTaskManagerAccess = true;
        _hasFinanceManagerAccess = true;
        _hasMealManagerAccess = true;
        _hasFileManagerAccess = true;
        return this;
    }

    public UpdateHouseholdDtoBuilder WithNoAccess()
    {
        _hasTaskManagerAccess = false;
        _hasFinanceManagerAccess = false;
        _hasMealManagerAccess = false;
        _hasFileManagerAccess = false;
        return this;
    }

    public UpdateHouseholdDto Build()
    {
        return new UpdateHouseholdDto
        {
            HouseholdId = _householdId,
            HouseholdName = _householdName,
            IsDefaultHousehold = _isDefaultHousehold,
            HasTaskManagerAccess = _hasTaskManagerAccess,
            HasFinanceManagerAccess = _hasFinanceManagerAccess,
            HasMealManagerAccess = _hasMealManagerAccess,
            HasFileManagerAccess = _hasFileManagerAccess
        };
    }
}