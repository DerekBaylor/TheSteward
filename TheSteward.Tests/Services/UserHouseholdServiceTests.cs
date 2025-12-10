using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using TheSteward.Core.Dtos.HouseholdDtos;
using TheSteward.Core.IRepositories;
using TheSteward.Core.Models;
using TheSteward.Core.Models.HouseholdModels;
using TheSteward.Infrastructure.Services;
using TheSteward.Tests.Helpers.TestDataBuilders;
using TheSteward.Tests.Helpers.TestDataBuilders.HouseholdDataBuilders;
using TheSteward.Tests.Helpers.TestDataBuilders.UserDataBuilers;

namespace TheSteward.Tests.Services;

[TestFixture]
public class UserHouseholdServiceTests
{
    private Mock<IUserHouseholdRepository>? _mockUserHouseholdRepository;
    private Mock<IInvitationRepository>? _mockInvitationRepository;
    private Mock<UserManager<ApplicationUser>>? _mockUserManager;
    private Mock<IMapper>? _mockMapper;
    private UserHouseholdService? _service;

    [SetUp]
    public void Setup()
    {
        _mockUserHouseholdRepository = new Mock<IUserHouseholdRepository>();
        _mockInvitationRepository = new Mock<IInvitationRepository>();
        _mockMapper = new Mock<IMapper>();

        var userStore = new Mock<IUserStore<ApplicationUser>>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
            userStore.Object, null, null, null, null, null, null, null, null);

        _service = new UserHouseholdService(
            _mockUserHouseholdRepository.Object,
            _mockUserManager.Object,
            _mockInvitationRepository.Object,
            _mockMapper.Object);
    }

    #region AddAsync Tests

    [Test]
    public async Task AddAsync_ValidUserHousehold_ShouldCreateUserHousehold()
    {
        // Arrange
        var ownerId = Guid.NewGuid().ToString();
        var owner = new ApplicationUserBuilder().WithId(ownerId).Build();
        var household = new HouseholdBuilder().WithOwner(owner).Build();

        var createDto = new CreateUserHouseholdDtoBuilder()
            .WithUser(owner)
            .WithHousehold(household)
            .WithAllPermissions()
            .IsOwner(true)
            .Build();

        _mockUserManager!
            .Setup(um => um.FindByIdAsync(ownerId))
            .ReturnsAsync(owner);

        _mockUserHouseholdRepository!
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        await _service!.AddAsync(createDto, ownerId);

        // Assert
        _mockUserManager.Verify(um => um.FindByIdAsync(ownerId), Times.Once);
        _mockUserHouseholdRepository.Verify(
            r => r.AddAsync(It.Is<UserHousehold>(uh =>
                uh.UserId == ownerId &&
                uh.IsHouseholdOwner == true &&
                uh.HasAdminPermissions == true)),
            Times.Once);
        _mockUserHouseholdRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void AddAsync_NullDto_ShouldThrowArgumentNullException()
    {
        // Arrange
        var ownerId = Guid.NewGuid().ToString();

        // Act
        Func<Task> act = async () => await _service!.AddAsync(null!, ownerId);

        // Assert
        act.Should().ThrowAsync<ArgumentNullException>()
            .WithParameterName("newUserHousehold");
    }

    [Test]
    public void AddAsync_NonExistentOwner_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var ownerId = Guid.NewGuid().ToString();
        var createDto = new CreateUserHouseholdDtoBuilder().WithUserId(ownerId).Build();

        _mockUserManager!
            .Setup(um => um.FindByIdAsync(ownerId))
            .ReturnsAsync((ApplicationUser?)null);

        // Act
        Func<Task> act = async () => await _service!.AddAsync(createDto, ownerId);

        // Assert
        act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"User with ID {ownerId} not found.");
    }

    [Test]
    public async Task AddAsync_ShouldGenerateNewGuid()
    {
        // Arrange
        var ownerId = Guid.NewGuid().ToString();
        var owner = new ApplicationUserBuilder().WithId(ownerId).Build();
        var createDto = new CreateUserHouseholdDtoBuilder().WithUser(owner).Build();
        UserHousehold? capturedUserHousehold = null;

        _mockUserManager!
            .Setup(um => um.FindByIdAsync(ownerId))
            .ReturnsAsync(owner);

        _mockUserHouseholdRepository!
            .Setup(r => r.AddAsync(It.IsAny<UserHousehold>()))
            .Callback<UserHousehold>(uh => capturedUserHousehold = uh)
            .Returns(Task.CompletedTask);

        _mockUserHouseholdRepository
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        await _service!.AddAsync(createDto, ownerId);

        // Assert
        capturedUserHousehold.Should().NotBeNull();
        capturedUserHousehold!.UserHouseholdId.Should().NotBe(Guid.Empty);
    }

    #endregion

    #region DeleteAsync Tests

    [Test]
    public async Task DeleteAsync_ValidUserHousehold_ShouldDelete()
    {
        // Arrange
        var userHousehold = new UserHouseholdBuilder().Build();

        _mockUserHouseholdRepository!
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        await _service!.DeleteAsync(userHousehold);

        // Assert
        _mockUserHouseholdRepository.Verify(r => r.DeleteAsync(userHousehold), Times.Once);
        _mockUserHouseholdRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    #endregion

    #region UpdateAsync Tests

    [Test]
    public async Task UpdateAsync_ValidDto_ShouldUpdate()
    {
        // Arrange
        var userHouseholdId = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        var householdId = Guid.NewGuid();

        var updateDto = new UpdateUserHouseholdDtoBuilder()
            .WithId(userHouseholdId)
            .WithUserId(userId)
            .WithHouseholdId(householdId)
            .WithAllPermissions()
            .Build();

        var existingUserHousehold = new UserHouseholdBuilder()
            .WithId(userHouseholdId)
            .WithUserId(userId)
            .WithHouseholdId(householdId)
            .Build();

        _mockUserHouseholdRepository!
            .Setup(r => r.GetByIdAsync(userHouseholdId))
            .ReturnsAsync(existingUserHousehold);

        _mockMapper!
            .Setup(m => m.Map(updateDto, existingUserHousehold))
            .Returns(existingUserHousehold);

        _mockUserHouseholdRepository
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        await _service!.UpdateAsync(updateDto);

        // Assert
        _mockUserHouseholdRepository.Verify(r => r.GetByIdAsync(userHouseholdId), Times.Once);
        _mockMapper.Verify(m => m.Map(updateDto, existingUserHousehold), Times.Once);
        _mockUserHouseholdRepository.Verify(r => r.UpdateAsync(existingUserHousehold), Times.Once);
        _mockUserHouseholdRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void UpdateAsync_NullId_ShouldThrowArgumentNullException()
    {
        // Arrange
        var updateDto = new UpdateUserHouseholdDtoBuilder().Build();
        updateDto.UserHouseholdId = null;

        // Act
        Func<Task> act = async () => await _service!.UpdateAsync(updateDto);

        // Assert
        act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public void UpdateAsync_NonExistentUserHousehold_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var userHouseholdId = Guid.NewGuid();
        var updateDto = new UpdateUserHouseholdDtoBuilder()
            .WithId(userHouseholdId)
            .Build();

        _mockUserHouseholdRepository!
            .Setup(r => r.GetByIdAsync(userHouseholdId))
            .ReturnsAsync((UserHousehold?)null);

        // Act
        Func<Task> act = async () => await _service!.UpdateAsync(updateDto);

        // Assert
        act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Household with ID {userHouseholdId} not found.");
    }

    #endregion

    #region GetByIdAsync Tests

    [Test]
    public async Task GetByIdAsync_ExistingUserHousehold_ShouldReturn()
    {
        // Arrange
        var userHouseholdId = Guid.NewGuid();
        var userHousehold = new UserHouseholdBuilder()
            .WithId(userHouseholdId)
            .Build();

        _mockUserHouseholdRepository!
            .Setup(r => r.GetByIdAsync(userHouseholdId))
            .ReturnsAsync(userHousehold);

        // Act
        var result = await _service!.GetByIdAsync(userHouseholdId);

        // Assert
        result.Should().NotBeNull();
        result.UserHouseholdId.Should().Be(userHouseholdId);
    }

    [Test]
    public void GetByIdAsync_NonExistentId_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var userHouseholdId = Guid.NewGuid();

        _mockUserHouseholdRepository!
            .Setup(r => r.GetByIdAsync(userHouseholdId))
            .ReturnsAsync((UserHousehold?)null);

        // Act
        Func<Task> act = async () => await _service!.GetByIdAsync(userHouseholdId);

        // Assert
        act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Household with ID {userHouseholdId} not found.");
    }

    #endregion

    #region GetAllUserHouseholdsForUserAsync Tests

    [Test]
    public async Task GetAllUserHouseholdsForUserAsync_ShouldReturnActiveUserHouseholds()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var owner = new ApplicationUserBuilder().Build();
        var household1 = new HouseholdBuilder().WithOwner(owner).IsActive(true).Build();
        var household2 = new HouseholdBuilder().WithOwner(owner).IsActive(true).Build();

        var userHouseholds = new List<UserHousehold>
        {
            new UserHouseholdBuilder().WithUserId(userId).WithHousehold(household1).Build(),
            new UserHouseholdBuilder().WithUserId(userId).WithHousehold(household2).Build()
        };

        var userHouseholdDtos = new List<UserHouseholdDto>
        {
            new UserHouseholdDto { UserHouseholdId = userHouseholds[0].UserHouseholdId },
            new UserHouseholdDto { UserHouseholdId = userHouseholds[1].UserHouseholdId }
        };

        var mockQueryable = userHouseholds.AsQueryable().BuildMock();

        _mockUserHouseholdRepository!
            .Setup(r => r.GetAll())
            .Returns(mockQueryable);

        _mockMapper!
            .Setup(m => m.Map<List<UserHouseholdDto>>(It.IsAny<List<UserHousehold>>()))
            .Returns(userHouseholdDtos);

        // Act
        var result = await _service!.GetAllUserHouseholdsForUserAsync(userId);

        // Assert
        result.Should().HaveCount(2);
    }

    #endregion

    #region CanInviteMembersAsync Tests

    [Test]
    public async Task CanInviteMembersAsync_HouseholdOwner_ShouldReturnTrue()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var householdId = Guid.NewGuid();
        var userHousehold = new UserHouseholdBuilder()
            .WithUserId(userId)
            .WithHouseholdId(householdId)
            .IsOwner(true)
            .Build();

        var mockQueryable = new List<UserHousehold> { userHousehold }.AsQueryable().BuildMock();

        _mockUserHouseholdRepository!
            .Setup(r => r.GetAll())
            .Returns(mockQueryable);

        // Act
        var result = await _service!.CanInviteMembersAsync(householdId, userId);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task CanInviteMembersAsync_HasAdminPermissions_ShouldReturnTrue()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var householdId = Guid.NewGuid();
        var userHousehold = new UserHouseholdBuilder()
            .WithUserId(userId)
            .WithHouseholdId(householdId)
            .WithAdminPermissions()
            .Build();

        var mockQueryable = new List<UserHousehold> { userHousehold }.AsQueryable().BuildMock();

        _mockUserHouseholdRepository!
            .Setup(r => r.GetAll())
            .Returns(mockQueryable);

        // Act
        var result = await _service!.CanInviteMembersAsync(householdId, userId);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task CanInviteMembersAsync_NoPermissions_ShouldReturnFalse()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var householdId = Guid.NewGuid();
        var userHousehold = new UserHouseholdBuilder()
            .WithUserId(userId)
            .WithHouseholdId(householdId)
            .Build();

        var mockQueryable = new List<UserHousehold> { userHousehold }.AsQueryable().BuildMock();

        _mockUserHouseholdRepository!
            .Setup(r => r.GetAll())
            .Returns(mockQueryable);

        // Act
        var result = await _service!.CanInviteMembersAsync(householdId, userId);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public async Task CanInviteMembersAsync_NotMemberOfHousehold_ShouldReturnFalse()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var householdId = Guid.NewGuid();

        var mockQueryable = new List<UserHousehold>().AsQueryable().BuildMock();

        _mockUserHouseholdRepository!
            .Setup(r => r.GetAll())
            .Returns(mockQueryable);

        // Act
        var result = await _service!.CanInviteMembersAsync(householdId, userId);

        // Assert
        result.Should().BeFalse();
    }

    #endregion
}