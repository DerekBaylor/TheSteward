using Microsoft.AspNetCore.Identity;
using TheSteward.Core.Dtos.UserDtos;
using TheSteward.Core.IServices;
using TheSteward.Core.Models;

namespace TheSteward.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> UpdateUserProfileAsync(UpdateUserProfileDto updateDto)
    {
        var user = await _userManager.FindByIdAsync(updateDto.UserId);

        if (user == null)
            throw new KeyNotFoundException($"User with ID {updateDto.UserId} not found.");

        if (user.UserName != updateDto.UserName)
        {
            var setUserNameResult = await _userManager.SetUserNameAsync(user, updateDto.UserName);
            if (!setUserNameResult.Succeeded)
                return setUserNameResult;
        }

        if (user.Email != updateDto.Email)
        {
            var existingUser = await _userManager.FindByEmailAsync(updateDto.Email);
            if (existingUser != null && existingUser.Id != updateDto.UserId)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "DuplicateEmail",
                    Description = "Email is already taken."
                });
            }

            var setEmailResult = await _userManager.SetEmailAsync(user, updateDto.Email);
            if (!setEmailResult.Succeeded)
                return setEmailResult;
        }

        var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, updateDto.PhoneNumber);
        if (!setPhoneResult.Succeeded)
            return setPhoneResult;

        return IdentityResult.Success;
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }
}