using TheSteward.Core.Dtos.HouseholdDtos;
using TheSteward.Core.Models.HouseholdModels;

namespace TheSteward.Core.IServices.HouseholdIServices;

public interface IUserHouseholdService
{
    /// <summary>
    /// Creates a new user-household relationship with specified permissions.
    /// Owners are granted full permissions; all other setup is delegated to the internal helper.
    /// </summary>
    /// <param name="newUserHousehold">The user-household data transfer object containing relationship details and permissions.</param>
    /// <param name="ownerId">The ID of the user who owns or is creating this relationship.</param>
    /// <exception cref="ArgumentNullException">Thrown when newUserHousehold is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the owner user is not found.</exception>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(CreateUserHouseholdDto newUserHousehold, string ownerId);

    /// <summary>
    /// Deletes a user-household relationship, permanently removing the user from the household.
    /// If the deleted membership was the user's default household and the user belongs to other
    /// active households, the first available active household will be set as the new default.
    /// </summary>
    /// <param name="userHouseholdId">The unique identifier of the user household to delete.</param>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when no user household matching <paramref name="userHouseholdId"/> is found.
    /// </exception>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(Guid userHouseholdId);

    #region Update Methods

    /// <summary>
    /// Updates an existing user-household relationship and permissions.
    /// </summary>
    /// <param name="updatedUserHousehold">The updated user-household data transfer object.</param>
    /// <exception cref="ArgumentNullException">Thrown when UserHouseholdId is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the user-household relationship is not found.</exception>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(UpdateUserHouseholdDto updatedUserHousehold);

    /// <summary>
    /// Sets the default budget for a user household.
    /// </summary>
    /// <param name="userHouseholdId">The unique identifier of the user household to update.</param>
    /// <param name="budgetId">The unique identifier of the budget to set as default.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="userHouseholdId"/> or <paramref name="budgetId"/> is empty.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when no user household matching <paramref name="userHouseholdId"/> is found.
    /// </exception>
    Task SetDefaultBudgetAsync(Guid userHouseholdId, Guid budgetId);

    /// <summary>
    /// Deactivates a user-household relationship, revoking the user's access to the household
    /// without permanently deleting the record. If the deactivated membership was the user's
    /// default household and the user belongs to other active households, the first available
    /// active household will be set as the new default.
    /// </summary>
    /// <param name="userHouseholdId">The unique identifier of the user household to deactivate.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="userHouseholdId"/> is empty.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when no user household matching <paramref name="userHouseholdId"/> is found.
    /// </exception>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeactivateUserAsync(Guid userHouseholdId);

    /// <summary>
    /// Reactivates a previously deactivated user-household relationship, restoring the user's
    /// access to the household. Does not update any permissions.
    /// </summary>
    /// <param name="userHouseholdId">The unique identifier of the user household to reactivate.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="userHouseholdId"/> is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when no user household matching <paramref name="userHouseholdId"/> is found.</exception>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ReactivateUserAsync(Guid userHouseholdId);
    #endregion Update Methods

    #region Get Methods

    /// <summary>
    /// Retrieves a user-household relationship by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user-household relationship.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the user-household relationship is not found.</exception>
    /// <returns>A task representing the asynchronous operation, containing the user-household entity.</returns>
    Task<UserHousehold> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all active user-household relationships for a specific user.
    /// Inactive user-household relationships are excluded.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of active user-household DTOs.</returns>
    Task<List<UserHouseholdDto>> GetAllUserHouseholdsForUserAsync(string userId);

    /// <summary>
    /// Retrieves all active households that a user is an active member of.
    /// Households where the user's membership is inactive are excluded.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of household DTOs.</returns>
    Task<List<HouseholdDto>> GetAllHouseholdsForUserAsync(string userId);

    /// <summary>
    /// Retrieves the default active household for a specific user, including all household members.
    /// Returns null if the user has no active default household.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task representing the asynchronous operation, containing the default user-household DTO or null if not found.</returns>
    Task<UserHouseholdDto?> GetDefaultUserHouseholdForUserAsync(string userId);

    /// <summary>
    /// Retrieves a specific user-household relationship by household ID and user ID, including all household members and their details.
    /// </summary>
    /// <param name="householdId">The unique identifier of the household.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="activeOnly">
    /// When true (default), only returns the relationship if the user's membership is active.
    /// Pass false to allow admins to retrieve and manage inactive memberships.
    /// </param>
    /// <returns>A task representing the asynchronous operation, containing the user-household DTO or null if not found.</returns>
    Task<UserHouseholdDto?> GetUserHouseholdByHouseholdIdAndUserIdAsync(Guid householdId, string userId, bool activeOnly = true);
    #endregion Get Methods

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
    /// Accepts a household invitation and adds the user to the household with no permissions by default.
    /// Permissions can be granted separately by a household admin after joining.
    /// The user-household record and the invitation acceptance are committed in a single transaction
    /// to ensure neither change is persisted without the other.
    /// </summary>
    /// <param name="invitationId">The invitation ID to accept.</param>
    /// <param name="userId">The ID of the user accepting the invitation.</param>
    /// <param name="setAsDefault">Whether to set this as the user's default household.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the invitation is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the invitation does not belong to the user.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the invitation is expired or already accepted.</exception>
    /// <returns>A task representing the asynchronous operation.</returns>
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
