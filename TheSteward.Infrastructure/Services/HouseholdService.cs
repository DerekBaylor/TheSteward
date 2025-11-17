using Microsoft.EntityFrameworkCore;
using TheSteward.Core.IRepositories;
using TheSteward.Core.IServices;
using TheSteward.Core.Models;

namespace TheSteward.Infrastructure.Services;

public class HouseholdService : IHouseholdService
{
    private readonly IHouseholdRepository _householdRepository;

    public HouseholdService(IHouseholdRepository householdRepository)
    {
        _householdRepository = householdRepository;
    }
    public async Task AddAsync(Household household)
    {
        if (household == null)
            throw new ArgumentNullException(nameof(household));        

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
        return await _householdRepository.GetAll()
            .Where(h => h.IsHouseholdActive && h.Members.Any(m => m.Id == userId))
            .ToListAsync();
    }
}
