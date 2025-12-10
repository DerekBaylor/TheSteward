using TheSteward.Core.Dtos.HouseholdDtos;

namespace TheSteward.Tests.Helpers.TestDataBuilders;

public class InviteUserToHouseholdDtoBuilder
{
    private Guid _householdId = Guid.NewGuid();
    private string _email = "invited@example.com";
    private bool _setAsDefaultHousehold = false;

    public InviteUserToHouseholdDtoBuilder WithHouseholdId(Guid householdId)
    {
        _householdId = householdId;
        return this;
    }

    public InviteUserToHouseholdDtoBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public InviteUserToHouseholdDtoBuilder SetAsDefault(bool setAsDefault)
    {
        _setAsDefaultHousehold = setAsDefault;
        return this;
    }

    public InviteUserToHouseholdDto Build()
    {
        return new InviteUserToHouseholdDto
        {
            HouseholdId = _householdId,
            Email = _email,
            SetAsDefaultHousehold = _setAsDefaultHousehold
        };
    }
}