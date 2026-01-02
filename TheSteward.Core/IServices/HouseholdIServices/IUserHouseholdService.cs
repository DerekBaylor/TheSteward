using TheSteward.Core.Dtos.HouseholdDtos;
using TheSteward.Core.Models.HouseholdModels;

namespace TheSteward.Core.IServices.HouseholdIServices;

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

    #region Invitation Methods

    /// <summary>
    /// Sends an invitation to a user to join a household.
    /// </summary>
    /// <param name="inviteDto">The invitation details.</param>
    /// <param name="invitingUserId">The ID of the user sending the invitation.</param>
    /// <exception cref="UnauthorizedAccessException">Thrown when user doesn't have permission to invite.</exception>
    /// <exception cref="InvalidOperationException">Thrown when user is already a member or has pending invitation.</exception>
    /// <returns>The created invitation DTO.</returns>
    Task<HouseholdInvitationDto> InviteUserToHouseholdAsync(InviteUserToHouseholdDto inviteDto, string invitingUserId);

    /// <summary>
    /// Accepts a household invitation and adds the user to the household.
    /// </summary>
    /// <param name="invitationId">The invitation ID to accept.</param>
    /// <param name="userId">The ID of the user accepting the invitation.</param>
    /// <param name="setAsDefault">Whether to set this as the user's default household.</param>
    /// <exception cref="KeyNotFoundException">Thrown when invitation not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown when invitation is expired or already accepted.</exception>
    Task AcceptInvitationAsync(Guid invitationId, string userId, bool setAsDefault);

    /// <summary>
    /// Gets all pending invitations for a user by email.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <returns>List of pending invitation DTOs.</returns>
    Task<List<HouseholdInvitationDto>> GetPendingInvitationsForUserAsync(string email);

    /// <summary>
    /// Cancels/deletes a household invitation.
    /// Can be canceled by: the person who sent it, household admin/owner, or the invited recipient.
    /// </summary>
    /// <param name="invitationId">The invitation ID to cancel.</param>
    /// <param name="userId">The ID of the user canceling the invitation.</param>
    /// <exception cref="KeyNotFoundException">Thrown when invitation not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when user doesn't have permission to cancel.</exception>
    Task CancelInvitationAsync(Guid invitationId, string userId);

    /// <summary>
    /// Checks if a user has permission to invite members to a household.
    /// </summary>
    /// <param name="householdId">The household ID to check.</param>
    /// <param name="userId">The user ID to check permissions for.</param>
    /// <returns>True if user can invite members, false otherwise.</returns>
    Task<bool> CanInviteMembersAsync(Guid householdId, string userId);
    #endregion Invitation Methods
}
