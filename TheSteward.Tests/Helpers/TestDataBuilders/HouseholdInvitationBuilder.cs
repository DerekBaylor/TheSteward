using TheSteward.Core.Models;
using TheSteward.Core.Models.HouseholdModels;

namespace TheSteward.Tests.Helpers.TestDataBuilders;

public class HouseholdInvitationBuilder
{
    private Guid _invitationId = Guid.NewGuid();
    private Guid _householdId = Guid.NewGuid();
    private Household? _household;
    private string _invitedUserEmail = "invited@example.com";
    private string _invitedByUserId = Guid.NewGuid().ToString();
    private ApplicationUser? _invitedByUser;
    private DateTime _invitedDate = DateTime.UtcNow;
    private DateTime? _expirationDate = DateTime.UtcNow.AddDays(7);
    private bool _isAccepted = false;
    private DateTime? _acceptedDate = null;

    public HouseholdInvitationBuilder WithId(Guid id)
    {
        _invitationId = id;
        return this;
    }

    public HouseholdInvitationBuilder WithHousehold(Household household)
    {
        _household = household;
        _householdId = household.HouseholdId;
        return this;
    }

    public HouseholdInvitationBuilder WithInvitedEmail(string email)
    {
        _invitedUserEmail = email;
        return this;
    }

    public HouseholdInvitationBuilder WithInvitedByUser(ApplicationUser user)
    {
        _invitedByUser = user;
        _invitedByUserId = user.Id;
        return this;
    }

    public HouseholdInvitationBuilder ExpiresOn(DateTime expirationDate)
    {
        _expirationDate = expirationDate;
        return this;
    }

    public HouseholdInvitationBuilder IsAccepted(DateTime acceptedDate)
    {
        _isAccepted = true;
        _acceptedDate = acceptedDate;
        return this;
    }

    public HouseholdInvitationBuilder NotAccepted()
    {
        _isAccepted = false;
        _acceptedDate = null;
        return this;
    }

    public HouseholdInvitation Build()
    {
        // If household wasn't set, create a default one
        if (_household == null)
        {
            _household = new HouseholdBuilder()
                .WithId(_householdId)
                .Build();
        }

        // If invitedByUser wasn't set, create a default one
        if (_invitedByUser == null)
        {
            _invitedByUser = new ApplicationUserBuilder()
                .WithId(_invitedByUserId)
                .Build();
        }

        return new HouseholdInvitation
        {
            InvitationId = _invitationId,
            HouseholdId = _householdId,
            Household = _household,
            InvitedUserEmail = _invitedUserEmail,
            InvitedByUserId = _invitedByUserId,
            InvitedByUser = _invitedByUser,
            InvitedDate = _invitedDate,
            ExpirationDate = _expirationDate,
            IsAccepted = _isAccepted,
            AcceptedDate = _acceptedDate
        };
    }
}