using TheSteward.Core.Models;

namespace TheSteward.Tests.Helpers.TestDataBuilders.UserDataBuilers;

public class ApplicationUserBuilder
{
    private string _id = Guid.NewGuid().ToString();
    private string _userName = "testuser@example.com";
    private string _email = "testuser@example.com";
    private string _firstName = "Test";
    private string _lastName = "User";

    public ApplicationUserBuilder WithId(string id)
    {
        _id = id;
        return this;
    }

    public ApplicationUserBuilder WithEmail(string email)
    {
        _email = email;
        _userName = email;
        return this;
    }

    public ApplicationUserBuilder WithName(string firstName, string lastName)
    {
        _firstName = firstName;
        _lastName = lastName;
        return this;
    }

    public ApplicationUser Build()
    {
        return new ApplicationUser
        {
            Id = _id,
            UserName = _userName,
            Email = _email,
            NormalizedUserName = _userName.ToUpperInvariant(),
            NormalizedEmail = _email.ToUpperInvariant(),
            EmailConfirmed = true,
        };
    }
}