using TheSteward.Core.Dtos.HouseholdDtos;
using TheSteward.Core.Models.HouseholdModels;

namespace TheSteward.Core.IServices;

public interface IUserHouseholdService
{
    /// <summary>
    /// Creates a new user-household relationship with specified permissions.
    /// </summary>
    /// <param name="newUserHousehold">The user-household data transfer object containing relationship details and permissions.</param>
    /// <param name="ownerId">The ID of the user who owns or is creating this relationship.</param>
    /// <exception cref="ArgumentNullException">Thrown when newUserHousehold is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the owner user is not found.</exception>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(CreateUserHouseholdDto newUserHousehold, string ownerId);

    /// <summary>
    /// Deletes a user-household relationship, removing the user from the household.
    /// </summary>
    /// <param name="userHousehold">The user-household entity to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(UserHousehold userHousehold);

    /// <summary>
    /// Updates an existing user-household relationship and permissions.
    /// </summary>
    /// <param name="updatedUserHousehold">The updated user-household data transfer object.</param>
    /// <exception cref="ArgumentNullException">Thrown when UserHouseholdId is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the user-household relationship is not found.</exception>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(UpdateUserHouseholdDto updatedUserHousehold);

    /// <summary>
    /// Retrieves a user-household relationship by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user-household relationship.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the user-household relationship is not found.</exception>
    /// <returns>A task representing the asynchronous operation, containing the user-household entity.</returns>
    Task<UserHousehold> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all active user-household relationships for a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of user-household DTOs.</returns>
    Task<List<UserHouseholdDto>> GetAllUserHouseholdsForUserAsync(string userId);

    /// <summary>
    /// Retrieves all active households that a user is a member of.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of household DTOs.</returns>
    Task<List<HouseholdDto>> GetAllHouseholdsForUserAsync(string userId);

    /// <summary>
    /// Retrieves the default household for a specific user, including all household members.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task representing the asynchronous operation, containing the default user-household DTO or null if not found.</returns>
    Task<UserHouseholdDto?> GetDefaultUserHouseholdForUserAsync(string userId);

    /// <summary>
    /// Retrieves a specific user-household relationship by household ID and user ID, including all household members and their details.
    /// </summary>
    /// <param name="householdId">The unique identifier of the household.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task representing the asynchronous operation, containing the user-household DTO or null if not found.</returns>
    Task<UserHouseholdDto?> GetUserHouseholdByHouseholdIdAndUserIdAsync(Guid householdId, string userId);
}
