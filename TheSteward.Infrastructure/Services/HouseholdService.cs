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

    public HouseholdService(IHouseholdRepository householdRepository,
    UserManager<ApplicationUser> userManager)
    {
        _householdRepository = householdRepository;
        _userManager = userManager;
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
    }

    public async Task DeleteAsync(Household household)
    {
        await _householdRepository.DeleteAsync(household);
        await _householdRepository.SaveChangesAsync();
    }

    public async Task UpdateAsync(Household household)
    {
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
