using TheSteward.Core.Dtos.HouseholdDtos;

namespace TheSteward.Tests.Helpers.TestDataBuilders;

public class HouseholdInvitationDtoBuilder
{
    private Guid _invitationId = Guid.NewGuid();
    private Guid _householdId = Guid.NewGuid();
    private string _householdName = "Test Household";
    private string _invitedByUserName = "Test User";
    private string _invitedUserEmail = "invited@example.com";
    private DateTime _invitedDate = DateTime.UtcNow;
    private DateTime? _expirationDate = DateTime.UtcNow.AddDays(7);
    private bool _isAccepted = false;

    public HouseholdInvitationDtoBuilder WithId(Guid id)
    {
        _invitationId = id;
        return this;
    }

    public HouseholdInvitationDtoBuilder WithHouseholdId(Guid householdId)
    {
        _householdId = householdId;
        return this;
    }

    public HouseholdInvitationDtoBuilder WithInvitedEmail(string email)
    {
        _invitedUserEmail = email;
        return this;
    }

    public HouseholdInvitationDtoBuilder IsAccepted()
    {
        _isAccepted = true;
        return this;
    }

    public HouseholdInvitationDtoBuilder IsExpired()
    {
        _expirationDate = DateTime.UtcNow.AddDays(-1);
        return this;
    }

    public HouseholdInvitationDto Build()
    {
        return new HouseholdInvitationDto
        {
            InvitationId = _invitationId,
            HouseholdId = _householdId,
            HouseholdName = _householdName,
            InvitedByUserName = _invitedByUserName,
            InvitedUserEmail = _invitedUserEmail,
            InvitedDate = _invitedDate,
            ExpirationDate = _expirationDate,
            IsAccepted = _isAccepted
        };
    }
}