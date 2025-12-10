using TheSteward.Core.Dtos.HouseholdDtos;

namespace TheSteward.Tests.Helpers.TestDataBuilders.HouseholdDataBuilders;

public class CreateHouseholdDtoBuilder
{
    private string _householdName = "Test Household";
    private bool _isDefaultHousehold = false;
    private bool _hasTaskManagerAccess = true;
    private bool _hasFinanceManagerAccess = true;
    private bool _hasMealManagerAccess = true;
    private bool _hasFileManagerAccess = true;

    public CreateHouseholdDtoBuilder WithName(string name)
    {
        _householdName = name;
        return this;
    }

    public CreateHouseholdDtoBuilder IsDefault(bool isDefault)
    {
        _isDefaultHousehold = isDefault;
        return this;
    }

    public CreateHouseholdDtoBuilder WithTaskManagerAccess(bool hasAccess)
    {
        _hasTaskManagerAccess = hasAccess;
        return this;
    }

    public CreateHouseholdDtoBuilder WithFinanceManagerAccess(bool hasAccess)
    {
        _hasFinanceManagerAccess = hasAccess;
        return this;
    }

    public CreateHouseholdDtoBuilder WithMealManagerAccess(bool hasAccess)
    {
        _hasMealManagerAccess = hasAccess;
        return this;
    }

    public CreateHouseholdDtoBuilder WithFileManagerAccess(bool hasAccess)
    {
        _hasFileManagerAccess = hasAccess;
        return this;
    }

    public CreateHouseholdDtoBuilder WithAllAccess()
    {
        _hasTaskManagerAccess = true;
        _hasFinanceManagerAccess = true;
        _hasMealManagerAccess = true;
        _hasFileManagerAccess = true;
        return this;
    }

    public CreateHouseholdDtoBuilder WithNoAccess()
    {
        _hasTaskManagerAccess = false;
        _hasFinanceManagerAccess = false;
        _hasMealManagerAccess = false;
        _hasFileManagerAccess = false;
        return this;
    }

    public CreateHouseholdDto Build()
    {
        return new CreateHouseholdDto
        {
            HouseholdName = _householdName,
            IsDefaultHousehold = _isDefaultHousehold,
            HasTaskManagerAccess = _hasTaskManagerAccess,
            HasFinanceManagerAccess = _hasFinanceManagerAccess,
            HasMealManagerAccess = _hasMealManagerAccess,
            HasFileManagerAccess = _hasFileManagerAccess
        };
    }
}