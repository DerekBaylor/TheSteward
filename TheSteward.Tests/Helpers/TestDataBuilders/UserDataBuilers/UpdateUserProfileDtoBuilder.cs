using TheSteward.Core.Dtos.UserDtos;

namespace TheSteward.Tests.Helpers.TestDataBuilders.UserDataBuilders;

public class UpdateUserProfileDtoBuilder
{
    private string _userId = Guid.NewGuid().ToString();
    private string _userName = "testuser";
    private string _email = "testuser@example.com";
    private string? _phoneNumber = null;

    public UpdateUserProfileDtoBuilder WithUserId(string userId)
    {
        _userId = userId;
        return this;
    }

    public UpdateUserProfileDtoBuilder WithUserName(string userName)
    {
        _userName = userName;
        return this;
    }

    public UpdateUserProfileDtoBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UpdateUserProfileDtoBuilder WithPhoneNumber(string? phoneNumber)
    {
        _phoneNumber = phoneNumber;
        return this;
    }

    public UpdateUserProfileDto Build()
    {
        return new UpdateUserProfileDto
        {
            UserId = _userId,
            UserName = _userName,
            Email = _email,
            PhoneNumber = _phoneNumber
        };
    }
}