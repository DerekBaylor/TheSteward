using TheSteward.Core.Dtos.HouseholdDtos;
using TheSteward.Core.Models.HouseholdModels;

namespace TheSteward.Core.IServices;

public interface IUserHouseholdService
{
    Task AddAsync(CreateUserHouseholdDto newUserHousehold, string ownerId);
    Task DeleteAsync(UserHousehold userHousehold);
    Task UpdateAsync(UpdateUserHouseholdDto updatedUserHousehold);
    Task<UserHousehold> GetByIdAsync(Guid id);
    Task<List<UserHouseholdDto>> GetAllUserHouseholdsForUser(string userId);
    Task<UserHouseholdDto?> GetDefaultUserHouseholdForUser(string userId);
}
