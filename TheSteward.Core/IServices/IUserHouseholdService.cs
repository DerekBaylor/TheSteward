using TheSteward.Core.DTOs;
using TheSteward.Core.Models;

namespace TheSteward.Core.IServices;

public interface IUserHouseholdService
{
    Task AddAsync(CreateUpdateUserHouseholdDto newUserHousehold, string ownerId);
    Task DeleteAsync(UserHousehold userHousehold);
    Task UpdateAsync(CreateUpdateHouseholdDto updatedUserHousehold);
    Task<UserHousehold> GetByIdAsync(Guid id);
    Task<List<UserHousehold>> GetAllUserHouseholdsForUser(string userId);
    Task<UserHousehold?> GetDefaultUserHouseholdForUser(string userId);
}
