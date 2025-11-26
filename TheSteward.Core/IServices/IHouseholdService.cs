using TheSteward.Core.Dtos.HouseholdDtos;

namespace TheSteward.Core.IServices;

public interface IHouseholdService
{
  /// <summary>
    /// Adds a new household to the database and creates an owner relationship for the specified user.
    /// The owner is automatically added as a member with full permissions across all household modules.
    /// </summary>
    /// <param name="newHousehold">The household data transfer object containing the household details to create.</param>
    /// <param name="ownerId">The unique identifier of the user who will own the household.</param>
    /// <exception cref="ArgumentNullException">Thrown when newHousehold is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the user with the specified ownerId does not exist.</exception>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(CreateHouseholdDto newHousehold, string ownerId);

    /// <summary>
    /// Deletes a household from the database.
    /// </summary>
    /// <param name="householdDto">The household data transfer object representing the household to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(HouseholdDto householdDto);

  /// <summary>
    /// Updates an existing household's editable properties in the database.
    /// Protected properties (IsHouseholdActive, OwnerId, Owner, Members) are preserved and cannot be modified through this method.
    /// </summary>
    /// <param name="updatedHouseholdDto">The household data transfer object containing the updated values.</param>
    /// <exception cref="ArgumentNullException">Thrown when updatedHouseholdDto.HouseholdId is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when no household with the specified ID exists.</exception>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(UpdateHouseholdDto updatedHouseholdDto);

    /// <summary>
    /// Retrieves a household by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the household.</param>
    /// <exception cref="KeyNotFoundException">Thrown when no household with the specified ID exists.</exception>
    /// <returns>A task representing the asynchronous operation, containing the household data transfer object.</returns>
    Task<HouseholdDto> GetByIdAsync(Guid id);

  /// <summary>
    /// Retrieves all active households where the specified user is a member.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of household data transfer objects for active households the user belongs to.</returns>
    Task<List<HouseholdDto>> GetAllHouseholdsForUser(string userId);
}
