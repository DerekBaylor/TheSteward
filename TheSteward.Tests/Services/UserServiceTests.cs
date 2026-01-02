using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using TheSteward.Core.Models;
using TheSteward.Infrastructure.Services.HouseholdServices;
using TheSteward.Tests.Helpers.TestDataBuilders.UserDataBuilders;
using TheSteward.Tests.Helpers.TestDataBuilders.UserDataBuilers;

namespace TheSteward.Tests.Services;

[TestFixture]
public class UserServiceTests
{
    private Mock<UserManager<ApplicationUser>>? _mockUserManager;
    private UserService? _service;

    [SetUp]
    public void Setup()
    {
        var userStore = new Mock<IUserStore<ApplicationUser>>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
            userStore.Object, null, null, null, null, null, null, null, null);

        _service = new UserService(_mockUserManager.Object);
    }

    #region UpdateUserProfileAsync Tests

    [Test]
    public async Task UpdateUserProfileAsync_ValidUpdate_ShouldSucceed()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var user = new ApplicationUserBuilder()
            .WithId(userId)
            .WithEmail("old@example.com")
            .Build();

        user.UserName = "oldusername";

        var updateDto = new UpdateUserProfileDtoBuilder()
            .WithUserId(userId)
            .WithUserName("newusername")
            .WithEmail("new@example.com")
            .WithPhoneNumber("555-1234")
            .Build();

        _mockUserManager!
            .Setup(um => um.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(um => um.SetUserNameAsync(user, updateDto.UserName))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManager
            .Setup(um => um.FindByEmailAsync(updateDto.Email))
            .ReturnsAsync((ApplicationUser?)null);

        _mockUserManager
            .Setup(um => um.SetEmailAsync(user, updateDto.Email))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManager
            .Setup(um => um.SetPhoneNumberAsync(user, updateDto.PhoneNumber))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _service!.UpdateUserProfileAsync(updateDto);

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        _mockUserManager.Verify(um => um.FindByIdAsync(userId), Times.Once);
        _mockUserManager.Verify(um => um.SetUserNameAsync(user, updateDto.UserName), Times.Once);
        _mockUserManager.Verify(um => um.SetEmailAsync(user, updateDto.Email), Times.Once);
        _mockUserManager.Verify(um => um.SetPhoneNumberAsync(user, updateDto.PhoneNumber), Times.Once);
    }

    [Test]
    public void UpdateUserProfileAsync_NonExistentUser_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var updateDto = new UpdateUserProfileDtoBuilder()
            .WithUserId(userId)
            .Build();

        _mockUserManager!
            .Setup(um => um.FindByIdAsync(userId))
            .ReturnsAsync((ApplicationUser?)null);

        // Act
        Func<Task> act = async () => await _service!.UpdateUserProfileAsync(updateDto);

        // Assert
        act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"User with ID {userId} not found.");
    }

    [Test]
    public async Task UpdateUserProfileAsync_SameUserName_ShouldNotUpdateUserName()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var userName = "existingusername";
        var user = new ApplicationUserBuilder()
            .WithId(userId)
            .WithEmail("user@example.com")
            .Build();

        user.UserName = userName;

        var updateDto = new UpdateUserProfileDtoBuilder()
            .WithUserId(userId)
            .WithUserName(userName) // Same username
            .WithEmail("user@example.com")
            .WithPhoneNumber("555-1234")
            .Build();

        _mockUserManager!
            .Setup(um => um.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(um => um.SetPhoneNumberAsync(user, updateDto.PhoneNumber))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _service!.UpdateUserProfileAsync(updateDto);

        // Assert
        result.Succeeded.Should().BeTrue();
        _mockUserManager.Verify(um => um.SetUserNameAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task UpdateUserProfileAsync_SameEmail_ShouldNotUpdateEmail()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var email = "user@example.com";
        var user = new ApplicationUserBuilder()
            .WithId(userId)
            .WithEmail(email)
            .Build();

        user.UserName = "username";

        var updateDto = new UpdateUserProfileDtoBuilder()
            .WithUserId(userId)
            .WithUserName("username")
            .WithEmail(email) // Same email
            .WithPhoneNumber("555-1234")
            .Build();

        _mockUserManager!
            .Setup(um => um.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(um => um.SetPhoneNumberAsync(user, updateDto.PhoneNumber))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _service!.UpdateUserProfileAsync(updateDto);

        // Assert
        result.Succeeded.Should().BeTrue();
        _mockUserManager.Verify(um => um.FindByEmailAsync(It.IsAny<string>()), Times.Never);
        _mockUserManager.Verify(um => um.SetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task UpdateUserProfileAsync_DuplicateEmail_ShouldReturnFailure()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var otherUserId = Guid.NewGuid().ToString();
        var duplicateEmail = "duplicate@example.com";

        var user = new ApplicationUserBuilder()
            .WithId(userId)
            .WithEmail("original@example.com")
            .Build();

        user.UserName = "username";

        var otherUser = new ApplicationUserBuilder()
            .WithId(otherUserId)
            .WithEmail(duplicateEmail)
            .Build();

        var updateDto = new UpdateUserProfileDtoBuilder()
            .WithUserId(userId)
            .WithUserName("username")
            .WithEmail(duplicateEmail)
            .WithPhoneNumber("555-1234")
            .Build();

        _mockUserManager!
            .Setup(um => um.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(um => um.FindByEmailAsync(duplicateEmail))
            .ReturnsAsync(otherUser);

        // Act
        var result = await _service!.UpdateUserProfileAsync(updateDto);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Errors.First().Code.Should().Be("DuplicateEmail");
        result.Errors.First().Description.Should().Be("Email is already taken.");
        _mockUserManager.Verify(um => um.SetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task UpdateUserProfileAsync_SameUserWithNewEmail_ShouldSucceed()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var newEmail = "newemail@example.com";

        var user = new ApplicationUserBuilder()
            .WithId(userId)
            .WithEmail("old@example.com")
            .Build();

        user.UserName = "username";

        var updateDto = new UpdateUserProfileDtoBuilder()
            .WithUserId(userId)
            .WithUserName("username")
            .WithEmail(newEmail)
            .WithPhoneNumber("555-1234")
            .Build();

        _mockUserManager!
            .Setup(um => um.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(um => um.FindByEmailAsync(newEmail))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(um => um.SetEmailAsync(user, newEmail))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManager
            .Setup(um => um.SetPhoneNumberAsync(user, updateDto.PhoneNumber))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _service!.UpdateUserProfileAsync(updateDto);

        // Assert
        result.Succeeded.Should().BeTrue();
        _mockUserManager.Verify(um => um.SetEmailAsync(user, newEmail), Times.Once);
    }

    [Test]
    public async Task UpdateUserProfileAsync_SetUserNameFails_ShouldReturnFailure()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var user = new ApplicationUserBuilder()
            .WithId(userId)
            .WithEmail("user@example.com")
            .Build();

        user.UserName = "oldusername";

        var updateDto = new UpdateUserProfileDtoBuilder()
            .WithUserId(userId)
            .WithUserName("newusername")
            .WithEmail("user@example.com")
            .Build();

        var failureResult = IdentityResult.Failed(new IdentityError
        {
            Code = "InvalidUserName",
            Description = "Username is invalid."
        });

        _mockUserManager!
            .Setup(um => um.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(um => um.SetUserNameAsync(user, updateDto.UserName))
            .ReturnsAsync(failureResult);

        // Act
        var result = await _service!.UpdateUserProfileAsync(updateDto);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Errors.First().Code.Should().Be("InvalidUserName");
        _mockUserManager.Verify(um => um.SetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        _mockUserManager.Verify(um => um.SetPhoneNumberAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task UpdateUserProfileAsync_SetEmailFails_ShouldReturnFailure()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var user = new ApplicationUserBuilder()
            .WithId(userId)
            .WithEmail("old@example.com")
            .Build();

        user.UserName = "username";

        var updateDto = new UpdateUserProfileDtoBuilder()
            .WithUserId(userId)
            .WithUserName("username")
            .WithEmail("invalid-email")
            .Build();

        var failureResult = IdentityResult.Failed(new IdentityError
        {
            Code = "InvalidEmail",
            Description = "Email is invalid."
        });

        _mockUserManager!
            .Setup(um => um.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(um => um.FindByEmailAsync(updateDto.Email))
            .ReturnsAsync((ApplicationUser?)null);

        _mockUserManager
            .Setup(um => um.SetEmailAsync(user, updateDto.Email))
            .ReturnsAsync(failureResult);

        // Act
        var result = await _service!.UpdateUserProfileAsync(updateDto);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Errors.First().Code.Should().Be("InvalidEmail");
        _mockUserManager.Verify(um => um.SetPhoneNumberAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task UpdateUserProfileAsync_SetPhoneNumberFails_ShouldReturnFailure()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var user = new ApplicationUserBuilder()
            .WithId(userId)
            .WithEmail("user@example.com")
            .Build();

        user.UserName = "username";

        var updateDto = new UpdateUserProfileDtoBuilder()
            .WithUserId(userId)
            .WithUserName("username")
            .WithEmail("user@example.com")
            .WithPhoneNumber("invalid-phone")
            .Build();

        var failureResult = IdentityResult.Failed(new IdentityError
        {
            Code = "InvalidPhoneNumber",
            Description = "Phone number is invalid."
        });

        _mockUserManager!
            .Setup(um => um.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(um => um.SetPhoneNumberAsync(user, updateDto.PhoneNumber))
            .ReturnsAsync(failureResult);

        // Act
        var result = await _service!.UpdateUserProfileAsync(updateDto);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Errors.First().Code.Should().Be("InvalidPhoneNumber");
    }

    [Test]
    public async Task UpdateUserProfileAsync_OnlyUpdatePhoneNumber_ShouldSucceed()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var email = "user@example.com";
        var userName = "username";

        var user = new ApplicationUserBuilder()
            .WithId(userId)
            .WithEmail(email)
            .Build();

        user.UserName = userName;

        var updateDto = new UpdateUserProfileDtoBuilder()
            .WithUserId(userId)
            .WithUserName(userName)
            .WithEmail(email)
            .WithPhoneNumber("555-9999")
            .Build();

        _mockUserManager!
            .Setup(um => um.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(um => um.SetPhoneNumberAsync(user, updateDto.PhoneNumber))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _service!.UpdateUserProfileAsync(updateDto);

        // Assert
        result.Succeeded.Should().BeTrue();
        _mockUserManager.Verify(um => um.SetUserNameAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        _mockUserManager.Verify(um => um.FindByEmailAsync(It.IsAny<string>()), Times.Never);
        _mockUserManager.Verify(um => um.SetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        _mockUserManager.Verify(um => um.SetPhoneNumberAsync(user, updateDto.PhoneNumber), Times.Once);
    }

    #endregion

    #region GetUserByIdAsync Tests

    [Test]
    public async Task GetUserByIdAsync_ExistingUser_ShouldReturnUser()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var user = new ApplicationUserBuilder()
            .WithId(userId)
            .WithEmail("test@example.com")
            .Build();

        _mockUserManager!
            .Setup(um => um.FindByIdAsync(userId))
            .ReturnsAsync(user);

        // Act
        var result = await _service!.GetUserByIdAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(userId);
        _mockUserManager.Verify(um => um.FindByIdAsync(userId), Times.Once);
    }

    [Test]
    public async Task GetUserByIdAsync_NonExistentUser_ShouldReturnNull()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();

        _mockUserManager!
            .Setup(um => um.FindByIdAsync(userId))
            .ReturnsAsync((ApplicationUser?)null);

        // Act
        var result = await _service!.GetUserByIdAsync(userId);

        // Assert
        result.Should().BeNull();
        _mockUserManager.Verify(um => um.FindByIdAsync(userId), Times.Once);
    }

    #endregion
}