using Microsoft.AspNetCore.Identity;
using TheSteward.Core.Dtos.UserDtos;
using TheSteward.Core.Models;

namespace TheSteward.Core.IServices;

public interface IUserService
{
    /// <summary>
    /// Updates a user's profile information (username, email, phone number).
    /// </summary>
    /// <param name="updateDto">The updated profile information.</param>
    /// <returns>IdentityResult indicating success or failure.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when user is not found.</exception>
    Task<IdentityResult> UpdateUserProfileAsync(UpdateUserProfileDto updateDto);

    /// <summary>
    /// Gets a user's profile information by user ID.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>The user entity or null if not found.</returns>
    Task<ApplicationUser?> GetUserByIdAsync(string userId);
}