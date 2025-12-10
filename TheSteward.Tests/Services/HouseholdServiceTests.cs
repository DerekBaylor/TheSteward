using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using TheSteward.Core.Dtos.HouseholdDtos;
using TheSteward.Core.IRepositories;
using TheSteward.Core.IServices;
using TheSteward.Core.Models;
using TheSteward.Core.Models.HouseholdModels;
using TheSteward.Infrastructure.Services;
using TheSteward.Tests.Helpers.TestDataBuilders.HouseholdDataBuilders;
using TheSteward.Tests.Helpers.TestDataBuilders.UserDataBuilers;

namespace TheSteward.Tests.Services;

[TestFixture]
public class HouseholdServiceTests
{
    private Mock<IHouseholdRepository>? _mockHouseholdRepository;
    private Mock<UserManager<ApplicationUser>>? _mockUserManager;
    private Mock<IUserHouseholdService>? _mockUserHouseholdService;
    private Mock<IMapper>? _mockMapper;
    private HouseholdService? _service;

    [SetUp]
    public void Setup()
    {
        _mockHouseholdRepository = new Mock<IHouseholdRepository>();
        _mockUserHouseholdService = new Mock<IUserHouseholdService>();
        _mockMapper = new Mock<IMapper>();

        var userStore = new Mock<IUserStore<ApplicationUser>>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
            userStore.Object, null, null, null, null, null, null, null, null);

        _service = new HouseholdService(
            _mockHouseholdRepository.Object,
            _mockUserManager.Object,
            _mockUserHouseholdService.Object,
            _mockMapper.Object);
    }

    #region AddAsync Tests

    [Test]
    public async Task AddAsync_ValidHousehold_ShouldCreateHouseholdAndUserHousehold()
    {
        // Arrange
        var ownerId = Guid.NewGuid().ToString();
        var owner = new ApplicationUserBuilder()
            .WithId(ownerId)
            .WithEmail("owner@example.com")
            .Build();

        var createDto = new CreateHouseholdDtoBuilder()
            .WithName("My New Household")
            .IsDefault(true)
            .Build();

        var household = new HouseholdBuilder()
            .WithName("My New Household")
            .WithOwner(owner)
            .Build();

        _mockUserManager!
            .Setup(um => um.FindByIdAsync(ownerId))
            .ReturnsAsync(owner);

        _mockMapper!
            .Setup(m => m.Map<Household>(It.IsAny<CreateHouseholdDto>()))
            .Returns(household);

        _mockHouseholdRepository!
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        _mockUserHouseholdService!
            .Setup(s => s.AddAsync(It.IsAny<CreateUserHouseholdDto>(), ownerId))
            .Returns(Task.CompletedTask);

        // Act
        await _service!.AddAsync(createDto, ownerId);

        // Assert
        _mockUserManager.Verify(um => um.FindByIdAsync(ownerId), Times.Once);
        _mockHouseholdRepository.Verify(r => r.AddAsync(It.IsAny<Household>()), Times.Once);
        _mockHouseholdRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        _mockUserHouseholdService.Verify(
            s => s.AddAsync(
                It.Is<CreateUserHouseholdDto>(dto =>
                    dto.IsHouseholdOwner == true &&
                    dto.HasAdminPermissions == true &&
                    dto.UserId == ownerId),
                ownerId),
            Times.Once);
    }

    [Test]
    public void AddAsync_NullHousehold_ShouldThrowArgumentNullException()
    {
        // Arrange
        var ownerId = Guid.NewGuid().ToString();

        // Act
        Func<Task> act = async () => await _service!.AddAsync(null!, ownerId);

        // Assert
        act.Should().ThrowAsync<ArgumentNullException>()
            .WithParameterName("newHousehold");
    }

    [Test]
    public void AddAsync_NonExistentOwner_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var ownerId = Guid.NewGuid().ToString();
        var createDto = new CreateHouseholdDtoBuilder().Build();

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
    public async Task AddAsync_ShouldSetHouseholdAsActive()
    {
        // Arrange
        var ownerId = Guid.NewGuid().ToString();
        var owner = new ApplicationUserBuilder().WithId(ownerId).Build();
        var createDto = new CreateHouseholdDtoBuilder().Build();
        Household? capturedHousehold = null;

        _mockUserManager!
            .Setup(um => um.FindByIdAsync(ownerId))
            .ReturnsAsync(owner);

        _mockMapper!
            .Setup(m => m.Map<Household>(It.IsAny<CreateHouseholdDto>()))
            .Returns(new HouseholdBuilder().WithOwner(owner).Build());

        _mockHouseholdRepository!
            .Setup(r => r.AddAsync(It.IsAny<Household>()))
            .Callback<Household>(h => capturedHousehold = h)
            .Returns(Task.CompletedTask);

        _mockHouseholdRepository
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        await _service!.AddAsync(createDto, ownerId);

        // Assert
        capturedHousehold.Should().NotBeNull();
        capturedHousehold!.IsHouseholdActive.Should().BeTrue();
        capturedHousehold.OwnerId.Should().Be(ownerId);
    }

    #endregion

    #region DeleteAsync Tests

    [Test]
    public async Task DeleteAsync_ValidHousehold_ShouldDeleteHousehold()
    {
        // Arrange
        var householdDto = new HouseholdDtoBuilder().Build();
        var household = new HouseholdBuilder().Build();

        _mockMapper!
            .Setup(m => m.Map<Household>(householdDto))
            .Returns(household);

        _mockHouseholdRepository!
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        await _service!.DeleteAsync(householdDto);

        // Assert
        _mockHouseholdRepository.Verify(r => r.DeleteAsync(household), Times.Once);
        _mockHouseholdRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    #endregion

    #region UpdateAsync Tests

    [Test]
    public async Task UpdateAsync_ValidHousehold_ShouldUpdateHousehold()
    {
        // Arrange
        var householdId = Guid.NewGuid();
        var ownerId = Guid.NewGuid().ToString();
        var owner = new ApplicationUserBuilder().WithId(ownerId).Build();

        var updateDto = new UpdateHouseholdDtoBuilder()
            .WithId(householdId)
            .WithName("Updated Name")
            .Build();

        var existingHousehold = new HouseholdBuilder()
            .WithId(householdId)
            .WithName("Original Name")
            .WithOwner(owner)
            .Build();

        _mockHouseholdRepository!
            .Setup(r => r.GetByIdAsync(householdId))
            .ReturnsAsync(existingHousehold);

        _mockHouseholdRepository
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        await _service!.UpdateAsync(updateDto);

        // Assert
        _mockHouseholdRepository.Verify(r => r.GetByIdAsync(householdId), Times.Once);
        _mockHouseholdRepository.Verify(r => r.UpdateAsync(existingHousehold), Times.Once);
        _mockHouseholdRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        existingHousehold.HouseholdName.Should().Be("Updated Name");
    }

    [Test]
    public async Task UpdateAsync_ShouldOnlyUpdateAllowedProperties()
    {
        // Arrange
        var householdId = Guid.NewGuid();
        var ownerId = Guid.NewGuid().ToString();
        var owner = new ApplicationUserBuilder().WithId(ownerId).Build();

        var updateDto = new UpdateHouseholdDtoBuilder()
            .WithId(householdId)
            .WithName("Updated Name")
            .Build();

        var existingHousehold = new HouseholdBuilder()
            .WithId(householdId)
            .WithName("Original Name")
            .WithOwner(owner)
            .IsActive(true)
            .Build();

        var originalOwnerId = existingHousehold.OwnerId;
        var originalIsActive = existingHousehold.IsHouseholdActive;

        _mockHouseholdRepository!
            .Setup(r => r.GetByIdAsync(householdId))
            .ReturnsAsync(existingHousehold);

        _mockHouseholdRepository
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        await _service!.UpdateAsync(updateDto);

        // Assert
        existingHousehold.HouseholdName.Should().Be("Updated Name");
        existingHousehold.OwnerId.Should().Be(originalOwnerId); // Should not change
        existingHousehold.IsHouseholdActive.Should().Be(originalIsActive); // Should not change
    }

    [Test]
    public void UpdateAsync_NonExistentHousehold_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var householdId = Guid.NewGuid();
        var updateDto = new UpdateHouseholdDtoBuilder()
            .WithId(householdId)
            .Build();

        _mockHouseholdRepository!
            .Setup(r => r.GetByIdAsync(householdId))
            .ReturnsAsync((Household?)null);

        // Act
        Func<Task> act = async () => await _service!.UpdateAsync(updateDto);

        // Assert
        act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Household with ID {householdId} not found.");
    }

    [Test]
    public async Task UpdateAsync_ShouldUpdateAccessFlags()
    {
        // Arrange
        var householdId = Guid.NewGuid();
        var owner = new ApplicationUserBuilder().Build();

        var updateDto = new UpdateHouseholdDtoBuilder()
            .WithId(householdId)
            .Build();

        updateDto.HasFileManagerAccess = false;
        updateDto.HasFinanceManagerAccess = false;
        updateDto.HasMealManagerAccess = false;
        updateDto.HasTaskManagerAccess = false;

        var existingHousehold = new HouseholdBuilder()
            .WithId(householdId)
            .WithOwner(owner)
            .WithFileManagerAccess(true)
            .WithFinanceManagerAccess(true)
            .WithMealManagerAccess(true)
            .WithTaskManagerAccess(true)
            .Build();

        _mockHouseholdRepository!
            .Setup(r => r.GetByIdAsync(householdId))
            .ReturnsAsync(existingHousehold);

        _mockHouseholdRepository
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        await _service!.UpdateAsync(updateDto);

        // Assert
        existingHousehold.HasFileManagerAccess.Should().BeFalse();
        existingHousehold.HasFinanceManagerAccess.Should().BeFalse();
        existingHousehold.HasMealManagerAccess.Should().BeFalse();
        existingHousehold.HasTaskManagerAccess.Should().BeFalse();
    }

    #endregion

    #region GetByIdAsync Tests

    [Test]
    public async Task GetByIdAsync_ExistingHousehold_ShouldReturnHouseholdDto()
    {
        // Arrange
        var householdId = Guid.NewGuid();
        var owner = new ApplicationUserBuilder().Build();
        var household = new HouseholdBuilder()
            .WithId(householdId)
            .WithName("Test Household")
            .WithOwner(owner)
            .Build();

        var householdDto = new HouseholdDtoBuilder()
            .WithId(householdId)
            .WithName("Test Household")
            .Build();

        _mockHouseholdRepository!
            .Setup(r => r.GetByIdAsync(householdId))
            .ReturnsAsync(household);

        _mockMapper!
            .Setup(m => m.Map<HouseholdDto>(household))
            .Returns(householdDto);

        // Act
        var result = await _service!.GetByIdAsync(householdId);

        // Assert
        result.Should().NotBeNull();
        result.HouseholdId.Should().Be(householdId);
        result.HouseholdName.Should().Be("Test Household");
        _mockHouseholdRepository.Verify(r => r.GetByIdAsync(householdId), Times.Once);
    }

    [Test]
    public void GetByIdAsync_NonExistentHousehold_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var householdId = Guid.NewGuid();

        _mockHouseholdRepository!
            .Setup(r => r.GetByIdAsync(householdId))
            .ReturnsAsync((Household?)null);

        // Act
        Func<Task> act = async () => await _service!.GetByIdAsync(householdId);

        // Assert
        act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Household with ID {householdId} not found.");
    }

    #endregion
}