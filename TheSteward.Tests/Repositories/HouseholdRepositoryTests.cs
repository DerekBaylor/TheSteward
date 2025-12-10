// Repositories/HouseholdRepositoryTests.cs
using FluentAssertions;
using NUnit.Framework;
using TheSteward.Core.Models.HouseholdModels;
using TheSteward.Infrastructure.Data;
using TheSteward.Infrastructure.Repositories;
using TheSteward.Tests.Helpers;
using TheSteward.Tests.Helpers.TestDataBuilders;
using TheSteward.Tests.Helpers.TestDataBuilders.HouseholdDataBuilders;
using TheSteward.Tests.Helpers.TestDataBuilders.UserDataBuilers;

namespace TheSteward.Tests.Repositories;

[TestFixture]
public class HouseholdRepositoryTests
{
    private BaseRepository<Household>? _repository;
    private TheStewardContext? _context;

    [SetUp]
    public void Setup()
    {
        _context = TestDbContextFactory.CreateInMemoryContext();
        _repository = new BaseRepository<Household>(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context?.Database.EnsureDeleted();
        _context?.Dispose();
    }

    [Test]
    public async Task AddAsync_AddHouseholdToDatabase()
    {
        // Arrange
        var owner = new ApplicationUserBuilder()
            .WithEmail("owner@example.com")
            .WithName("John", "Doe")
            .Build();

        var household = new HouseholdBuilder()
            .WithName("Test Household")
            .WithOwner(owner)
            .Build();

        // Add the owner first (because of FK relationship)
        await _context!.Users.AddAsync(owner);
        await _context.SaveChangesAsync();

        // Act
        await _repository!.AddAsync(household);
        await _repository.SaveChangesAsync();

        // Assert
        var result = await _repository.GetByIdAsync(household.HouseholdId);
        result.Should().NotBeNull();
        result!.HouseholdName.Should().Be("Test Household");
        result.OwnerId.Should().Be(owner.Id);
    }

    [Test]
    public async Task GetByIdAsync_ExistingHousehold_ShouldReturnHousehold()
    {
        // Arrange
        var owner = new ApplicationUserBuilder().Build();
        var household = new HouseholdBuilder()
            .WithOwner(owner)
            .WithName("My Household")
            .Build();

        await _context!.Users.AddAsync(owner);
        await _repository!.AddAsync(household);
        await _repository.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(household.HouseholdId);

        // Assert
        result.Should().NotBeNull();
        result!.HouseholdId.Should().Be(household.HouseholdId);
        result.HouseholdName.Should().Be("My Household");
    }

    [Test]
    public async Task GetByIdAsync_NonExistingId_ShouldReturnNull()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var result = await _repository!.GetByIdAsync(nonExistingId);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllHouseholds()
    {
        // Arrange
        var owner = new ApplicationUserBuilder().Build();
        await _context!.Users.AddAsync(owner);

        var households = new[]
        {
            new HouseholdBuilder().WithName("Household 1").WithOwner(owner).Build(),
            new HouseholdBuilder().WithName("Household 2").WithOwner(owner).Build(),
            new HouseholdBuilder().WithName("Household 3").WithOwner(owner).Build()
        };

        foreach (var household in households)
        {
            await _repository!.AddAsync(household);
        }
        await _repository!.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(3);
    }

    [Test]
    public async Task GetAllAsync_EmptyDatabase_ShouldReturnEmptyCollection()
    {
        // Act
        var result = await _repository!.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetAll_ShouldReturnQueryable()
    {
        // Arrange
        var owner = new ApplicationUserBuilder().Build();
        var household = new HouseholdBuilder().WithOwner(owner).Build();

        await _context!.Users.AddAsync(owner);
        await _repository!.AddAsync(household);
        await _repository.SaveChangesAsync();

        // Act
        var result = _repository.GetAll();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IQueryable<Household>>();
        result.Count().Should().Be(1);
    }

    [Test]
    public async Task UpdateAsync_ShouldModifyHousehold()
    {
        // Arrange
        var owner = new ApplicationUserBuilder().Build();
        var household = new HouseholdBuilder()
            .WithOwner(owner)
            .WithName("Original Name")
            .Build();

        await _context!.Users.AddAsync(owner);
        await _repository!.AddAsync(household);
        await _repository.SaveChangesAsync();

        // Act
        household.HouseholdName = "Updated Name";
        await _repository.UpdateAsync(household);
        await _repository.SaveChangesAsync();

        // Assert
        var result = await _repository.GetByIdAsync(household.HouseholdId);
        result!.HouseholdName.Should().Be("Updated Name");
    }

    [Test]
    public async Task DeleteAsync_ShouldRemoveHousehold()
    {
        // Arrange
        var owner = new ApplicationUserBuilder().Build();
        var household = new HouseholdBuilder()
            .WithOwner(owner)
            .Build();

        await _context!.Users.AddAsync(owner);
        await _repository!.AddAsync(household);
        await _repository.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(household);
        await _repository.SaveChangesAsync();

        // Assert
        var result = await _repository.GetByIdAsync(household.HouseholdId);
        result.Should().BeNull();
    }

    [Test]
    public async Task SaveChangesAsync_ShouldReturnNumberOfAffectedRows()
    {
        // Arrange
        var owner = new ApplicationUserBuilder().Build();
        var household = new HouseholdBuilder().WithOwner(owner).Build();

        await _context!.Users.AddAsync(owner);
        await _context.SaveChangesAsync();
        await _repository!.AddAsync(household);

        // Act
        var result = await _repository.SaveChangesAsync();

        // Assert
        result.Should().Be(1);
    }

    [Test]
    public async Task SaveChangesAsync_MultipleOperations_ShouldReturnCorrectCount()
    {
        // Arrange
        var owner = new ApplicationUserBuilder().Build();
        var household1 = new HouseholdBuilder().WithName("Household 1").WithOwner(owner).Build();
        var household2 = new HouseholdBuilder().WithName("Household 2").WithOwner(owner).Build();

        await _context!.Users.AddAsync(owner);
        await _context.SaveChangesAsync();

        await _repository!.AddAsync(household1);
        await _repository.AddAsync(household2);

        // Act
        var result = await _repository.SaveChangesAsync();

        // Assert
        result.Should().Be(2);
    }

    [Test]
    public async Task SaveChangesAsync_NoChanges_ShouldReturnZero()
    {
        // Act
        var result = await _repository!.SaveChangesAsync();

        // Assert
        result.Should().Be(0);
    }
}