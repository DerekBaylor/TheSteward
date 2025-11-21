using TheSteward.Core.DTOs;

namespace TheSteward.Core.IServices;

public interface IHouseholdService
{
    /// <summary>
    /// Adds a new household to the database.
    /// </summary>
    /// <param name="household">The household entity to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when household is null.</exception>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(CreateUpdateHouseholdDto newHousehold, string ownerId);

    /// <summary>
    /// Deletes a household from the database.
    /// </summary>
    /// <param name="household">The household entity to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(HouseholdDto householdDto);

    /// <summary>
    /// Updates an existing household in the database.
    /// </summary>
    /// <param name="household">The household entity with updated values.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(CreateUpdateHouseholdDto updatedHouseholdDto);

    /// <summary>
    /// Retrieves a household by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the household.</param>
    /// <exception cref="KeyNotFoundException">Thrown when no household with the specified ID exists.</exception>
    /// <returns>A task representing the asynchronous operation, containing the household.</returns>
    Task<HouseholdDto> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all active households where the specified user is a member.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of active households the user belongs to.</returns>
    Task<List<HouseholdDto>> GetAllHouseholdsForUser(string userId);
}
