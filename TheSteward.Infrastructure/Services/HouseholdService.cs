using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.DTOs;
using TheSteward.Core.IRepositories;
using TheSteward.Core.IServices;
using TheSteward.Core.Models;

namespace TheSteward.Infrastructure.Services;

public class HouseholdService : IHouseholdService
{
    private readonly IHouseholdRepository _householdRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserHouseholdService _userHouseholdService;

    public HouseholdService(IHouseholdRepository householdRepository, UserManager<ApplicationUser> userManager, IUserHouseholdService userHouseholdService)
    {
        _householdRepository = householdRepository;
        _userManager = userManager;
        _userHouseholdService = userHouseholdService;
    }
    public async Task AddAsync(CreateUpdateHouseholdDto newHousehold, string ownerId)
    {
        if (newHousehold == null)
            throw new ArgumentNullException(nameof(newHousehold));

        var owner = await _userManager.FindByIdAsync(ownerId);
        if (owner == null)
            throw new KeyNotFoundException($"User with ID {ownerId} not found.");

        var household = new Household
        {
            HouseholdId = Guid.NewGuid(),
            HouseholdName = newHousehold.HouseholdName,
            IsHouseholdActive = true,
            HasTaskManagerAccess = newHousehold.HasTaskManagerAccess,
            HasFinanceManagerAccess = newHousehold.HasFinanceManagerAccess,
            HasMealManagerAccess = newHousehold.HasMealManagerAccess,
            HasFileManagerAccess = newHousehold.HasFileManagerAccess,
            OwnerId = ownerId,
            Owner = null!,
            Members = new List<ApplicationUser>() { owner }
        };

        await _householdRepository.AddAsync(household);
        await _householdRepository.SaveChangesAsync();

        // create userhousehold
       var createUpdateUserHouseholdDto = new CreateUpdateUserHouseholdDto
        {
            IsDefaultUserHousehold = newHousehold.IsDefaultHousehold,
            IsHouseholdOwner = true,
            HasAdminPermissions = true,
            HasFinanceManagerWritePermission = true,
            HasFinanceManagerReadPermission = true,
            HasKitchenManagerWritePermission = true,
            HasKitchenManagerReadPermission = true,
            HasTaskManagerWritePermission = true,
            HasTaskManagerReadPermission = true,
            HasFileManagerWritePermission = true,
            HasFileManagerReadPermission = true,
            UserId = ownerId,
            User = owner,
            HouseholdId = household.HouseholdId,
            Household = household
       };

        await _userHouseholdService.AddAsync(createUpdateUserHouseholdDto, ownerId);
    }

    public async Task DeleteAsync(Household household)
    {
        await _householdRepository.DeleteAsync(household);
        await _householdRepository.SaveChangesAsync();
    }

    public async Task UpdateAsync(CreateUpdateHouseholdDto updatedHousehold)
    {
        if (updatedHousehold.HouseholdId == null)
            throw new ArgumentNullException(nameof(updatedHousehold.HouseholdId));

        var currentHousehold = await _householdRepository.GetByIdAsync(updatedHousehold.HouseholdId.Value);

        if (currentHousehold == null)
            throw new KeyNotFoundException($"Household with ID {updatedHousehold.HouseholdId} not found.");

        var household = new Household
        { 
            HasFileManagerAccess = updatedHousehold.HasFileManagerAccess,
            HasFinanceManagerAccess = updatedHousehold.HasFinanceManagerAccess,
            HasMealManagerAccess = updatedHousehold.HasMealManagerAccess,
            HasTaskManagerAccess = updatedHousehold.HasTaskManagerAccess,
            HouseholdId = updatedHousehold.HouseholdId.Value,
            HouseholdName = updatedHousehold.HouseholdName,
            IsHouseholdActive = currentHousehold.IsHouseholdActive,
            OwnerId = currentHousehold.OwnerId,
            Owner = currentHousehold.Owner,
            Members = currentHousehold.Members
        };

        await _householdRepository.UpdateAsync(household);
        await _householdRepository.SaveChangesAsync();
    }

    public async Task<Household> GetByIdAsync(Guid id)
    {
        var household = await _householdRepository.GetByIdAsync(id);

        if (household == null)
        {
            throw new KeyNotFoundException($"Household with ID {id} not found.");
        }

        return household;
    }

    public async Task<List<Household>> GetAllHouseholdsForUser(string userId)
    {
        var household = await _householdRepository.GetAll()
            .Where(h => h.IsHouseholdActive && h.Members.Any(m => m.Id == userId))
            .ToListAsync();

        return household;
    }
}
