using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Dtos.HouseholdDtos;
using TheSteward.Core.IRepositories.HouseholdIRepositories;
using TheSteward.Core.IServices.HouseholdIServices;
using TheSteward.Core.Models;
using TheSteward.Core.Models.HouseholdModels;
using TheSteward.Core.MappingExtensions;

namespace TheSteward.Infrastructure.Services.HouseholdServices;

public class UserHouseholdService : IUserHouseholdService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHouseholdRepository _householdRepository;
    private readonly IInvitationRepository _invitationRepository;
    private readonly IUserHouseholdRepository _userHouseholdRepository;

    public UserHouseholdService(IUserHouseholdRepository userHouseholdRepository, IHouseholdRepository householdRepository, UserManager<ApplicationUser> userManager, IInvitationRepository invitationRepository)
    {
        _userHouseholdRepository = userHouseholdRepository;
        _householdRepository = householdRepository;
        _userManager = userManager;
        _invitationRepository = invitationRepository;
    }

    public async Task<UserHouseholdDto> AddAsync(CreateUserHouseholdDto newUserHousehold, string ownerId)
    {
        if (newUserHousehold == null)
            throw new ArgumentNullException(nameof(newUserHousehold));

        var owner = await _userManager.FindByIdAsync(ownerId)
            ?? throw new KeyNotFoundException($"User with ID {ownerId} not found.");

        var userHousehold = await CreateUserHouseholdAsync(
            newUserHousehold.HouseholdId,
            newUserHousehold.UserId,
            newUserHousehold.UserName,
            newUserHousehold.IsDefaultUserHousehold,
            newUserHousehold.IsHouseholdOwner,
            newUserHousehold.HasAdminPermissions,
            hasAllPermissions: true);

        await _userHouseholdRepository.SaveChangesAsync();

        return userHousehold.ToDto();
    }


    public async Task DeleteAsync(Guid userHouseholdId)
    {
        var userHousehold = await _userHouseholdRepository.GetByIdAsync(userHouseholdId)
            ?? throw new KeyNotFoundException($"UserHousehold with ID {userHouseholdId} was not found.");

        await ReassignDefaultIfNeededAsync(userHousehold);

        await _userHouseholdRepository.DeleteAsync(userHousehold);
        await _userHouseholdRepository.SaveChangesAsync();
    }

    #region Update Methods
    public async Task<UserHouseholdDto> UpdateAsync(UpdateUserHouseholdDto updatedUserHousehold)
    {
        if (updatedUserHousehold.UserHouseholdId == null)
            throw new ArgumentNullException(nameof(updatedUserHousehold.UserHouseholdId));

        var currentUserHousehold = await _userHouseholdRepository.GetByIdAsync(updatedUserHousehold.UserHouseholdId.Value)
            ?? throw new KeyNotFoundException($"UserHousehold with ID {updatedUserHousehold.UserHouseholdId} not found.");

        currentUserHousehold.ApplyUpdate(updatedUserHousehold);

        await _userHouseholdRepository.UpdateAsync(currentUserHousehold);
        await _userHouseholdRepository.SaveChangesAsync();

        return currentUserHousehold.ToDto();
    }


    public async Task<UserHouseholdDto> SetDefaultBudgetAsync(Guid userHouseholdId, Guid budgetId)
    {
        if (userHouseholdId == Guid.Empty)
            throw new ArgumentException("UserHousehold ID cannot be empty.", nameof(userHouseholdId));

        if (budgetId == Guid.Empty)
            throw new ArgumentException("Budget ID cannot be empty.", nameof(budgetId));

        var userHousehold = await _userHouseholdRepository.GetByIdAsync(userHouseholdId)
            ?? throw new KeyNotFoundException($"UserHousehold with ID {userHouseholdId} was not found.");

        userHousehold.DefaultBudgetId = budgetId;

        await _userHouseholdRepository.UpdateAsync(userHousehold);
        await _userHouseholdRepository.SaveChangesAsync();

        return userHousehold.ToDto();
    }


    public async Task<UserHouseholdDto> DeactivateUserAsync(Guid userHouseholdId)
    {
        if (userHouseholdId == Guid.Empty)
            throw new ArgumentException("UserHousehold ID cannot be empty.", nameof(userHouseholdId));

        var userHousehold = await _userHouseholdRepository.GetByIdAsync(userHouseholdId)
            ?? throw new KeyNotFoundException($"UserHousehold with ID {userHouseholdId} was not found.");

        await ReassignDefaultIfNeededAsync(userHousehold);

        userHousehold.IsActive = false;

        await _userHouseholdRepository.UpdateAsync(userHousehold);
        await _userHouseholdRepository.SaveChangesAsync();

        return userHousehold.ToDto();
    }


    public async Task<UserHouseholdDto> ReactivateUserAsync(Guid userHouseholdId)
    {
        if (userHouseholdId == Guid.Empty)
            throw new ArgumentException("UserHousehold ID cannot be empty.", nameof(userHouseholdId));

        var userHousehold = await _userHouseholdRepository.GetByIdAsync(userHouseholdId)
            ?? throw new KeyNotFoundException($"UserHousehold with ID {userHouseholdId} was not found.");

        userHousehold.IsActive = true;

        await _userHouseholdRepository.UpdateAsync(userHousehold);
        await _userHouseholdRepository.SaveChangesAsync();

        return userHousehold.ToDto();
    }


    #endregion Update Methods

    #region Get UserHousehold Methods
    public async Task<UserHousehold> GetByIdAsync(Guid id)
    {
        return await _userHouseholdRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"UserHousehold with ID {id} not found.");
    }

    public async Task<List<UserHouseholdDto>> GetAllUserHouseholdsForUserAsync(string userId)
    {
        var userHouseholds = await _userHouseholdRepository.GetAll()
            .Include(uh => uh.Household)
            .Where(uh => uh.Household.IsHouseholdActive && uh.UserId == userId && uh.IsActive)
            .ToListAsync();

        return userHouseholds.ToDtoList();
    }

    public async Task<List<HouseholdDto>> GetAllHouseholdsForUserAsync(string userId)
    {
        var households = await _userHouseholdRepository.GetAll()
            .Include(uh => uh.Household)
            .Where(uh => uh.UserId == userId &&
                         uh.Household.IsHouseholdActive &&
                         uh.IsActive)
            .Select(uh => uh.Household)
            .ToListAsync();

        return households.ToDtoList();
    }

    public async Task<UserHouseholdDto?> GetDefaultUserHouseholdForUserAsync(string userId)
    {
        var userHousehold = await _userHouseholdRepository.GetAll()
            .Include(uh => uh.Household)
            .Where(uh =>
                uh.UserId == userId &&
                uh.IsDefaultUserHousehold &&
                uh.IsActive &&
                uh.Household.IsHouseholdActive)
            .FirstOrDefaultAsync();

        return userHousehold?.ToDto();
    }

    public async Task<UserHouseholdDto?> GetUserHouseholdByHouseholdIdAndUserIdAsync(
        Guid householdId, string userId, bool activeOnly = true)
    {
        var query = _userHouseholdRepository.GetAll()
            .Include(uh => uh.Household)
                .ThenInclude(h => h.UserHouseholds)
                    .ThenInclude(uh => uh.User)
            .Include(uh => uh.User)
            .Where(uh => uh.UserId == userId && uh.HouseholdId == householdId);

        if (activeOnly)
            query = query.Where(uh => uh.IsActive);

        var userHousehold = await query.FirstOrDefaultAsync();

        return userHousehold?.ToDto();
    }

    public async Task<List<UserHouseholdDto>> GetAllUserHouseholdsForHouseholdAsync(Guid householdId)
    {
        var household = await _householdRepository.GetAll()
            .Include(h => h.UserHouseholds)
            .FirstOrDefaultAsync(h => h.HouseholdId == householdId && h.IsHouseholdActive)
            ?? throw new KeyNotFoundException($"Household with ID {householdId} not found.");

        return household.UserHouseholds
            .Where(uh => uh.IsActive)
            .ToDtoList();
    }

    #endregion Get UserHousehold Methods

    #region Invitation Methods

    public async Task<HouseholdInvitationDto> InviteUserToHouseholdAsync(InviteUserToHouseholdDto inviteDto, string invitingUserId)
    {
        var canInvite = await CanInviteMembersAsync(inviteDto.HouseholdId, invitingUserId);
        if (!canInvite)
            throw new UnauthorizedAccessException("You don't have permission to invite members to this household.");

        var existingMember = await _userHouseholdRepository.GetAll()
            .FirstOrDefaultAsync(uh => uh.HouseholdId == inviteDto.HouseholdId && uh.User.Email == inviteDto.Email);

        if (existingMember != null)
            throw new InvalidOperationException("This user is already a member of the household.");

        var existingInvitation = await _invitationRepository.GetAll()
            .FirstOrDefaultAsync(i => i.HouseholdId == inviteDto.HouseholdId
                && i.InvitedUserEmail == inviteDto.Email
                && !i.IsAccepted
                && (i.ExpirationDate == null || i.ExpirationDate > DateTime.UtcNow));

        if (existingInvitation != null)
            throw new InvalidOperationException("This user already has a pending invitation to this household.");

        var household = await _userHouseholdRepository.GetAll()
            .Include(uh => uh.Household)
            .Where(uh => uh.HouseholdId == inviteDto.HouseholdId)
            .Select(uh => uh.Household)
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException("Household not found.");

        var invitingUser = await _userManager.FindByIdAsync(invitingUserId)
            ?? throw new KeyNotFoundException("Inviting user not found.");

        var invitation = new HouseholdInvitation
        {
            InvitationId = Guid.NewGuid(),
            HouseholdId = inviteDto.HouseholdId,
            Household = household,
            InvitedUserEmail = inviteDto.Email.ToLower(),
            InvitedByUserId = invitingUserId,
            InvitedByUser = invitingUser,
            InvitedDate = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddDays(7),
            IsAccepted = false
        };

        await _invitationRepository.AddAsync(invitation);
        await _invitationRepository.SaveChangesAsync();

        var invitationDto = invitation.ToDto();
        return invitationDto;
    }

    public async Task<UserHouseholdDto> AcceptInvitationAsync(Guid invitationId, string userId, bool setAsDefault)
    {
        var invitation = await _invitationRepository.GetAll()
            .Include(i => i.Household)
            .Include(i => i.InvitedByUser)
            .FirstOrDefaultAsync(i => i.InvitationId == invitationId)
            ?? throw new KeyNotFoundException("Invitation not found.");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || user.Email?.ToLower() != invitation.InvitedUserEmail.ToLower())
            throw new UnauthorizedAccessException("This invitation is not for you.");

        if (invitation.IsAccepted)
            throw new InvalidOperationException("This invitation has already been accepted.");

        if (invitation.ExpirationDate.HasValue && invitation.ExpirationDate.Value < DateTime.UtcNow)
            throw new InvalidOperationException("This invitation has expired.");

        if (setAsDefault)
        {
            var userHouseholds = await _userHouseholdRepository.GetAll()
                .Where(uh => uh.UserId == userId && uh.IsDefaultUserHousehold)
                .ToListAsync();

            foreach (var uh in userHouseholds)
            {
                uh.IsDefaultUserHousehold = false;
                await _userHouseholdRepository.UpdateAsync(uh);
            }
        }
        else
        {
            var hasHouseholds = await _userHouseholdRepository.GetAll()
                .AnyAsync(uh => uh.UserId == userId);

            if (!hasHouseholds)
                setAsDefault = true;
        }

        var userHousehold = await CreateUserHouseholdAsync(
            invitation.HouseholdId,
            userId,
            user.UserName,
            isDefaultUserHousehold: setAsDefault,
            isHouseholdOwner: false,
            hasAdminPermissions: false,
            hasAllPermissions: false);

        invitation.IsAccepted = true;
        invitation.AcceptedDate = DateTime.UtcNow;
        await _invitationRepository.UpdateAsync(invitation);

        await _userHouseholdRepository.SaveChangesAsync();

        return userHousehold.ToDto();
    }

    public async Task<List<HouseholdInvitationDto>> GetPendingInvitationsForUserAsync(string email)
    {
        var invitations = await _invitationRepository.GetAll()
            .Include(i => i.Household)
            .Include(i => i.InvitedByUser)
            .Where(i => i.InvitedUserEmail.ToLower() == email.ToLower()
                && !i.IsAccepted
                && (i.ExpirationDate == null || i.ExpirationDate > DateTime.UtcNow))
            .OrderByDescending(i => i.InvitedDate)
            .ToListAsync();

        return invitations.ToDtoList();
    }

    public async Task CancelInvitationAsync(Guid invitationId, string userId)
    {
        var invitation = await _invitationRepository.GetAll()
            .Include(i => i.Household)
            .FirstOrDefaultAsync(i => i.InvitationId == invitationId);

        if (invitation == null)
            throw new KeyNotFoundException("Invitation not found.");

        var user = await _userManager.FindByIdAsync(userId);

        var canCancel = invitation.InvitedByUserId == userId ||
                        await CanInviteMembersAsync(invitation.HouseholdId, userId) ||
                        user != null && user.Email?.ToLower() == invitation.InvitedUserEmail.ToLower();

        if (!canCancel)
            throw new UnauthorizedAccessException("You don't have permission to cancel this invitation.");

        await _invitationRepository.DeleteAsync(invitation);
        await _invitationRepository.SaveChangesAsync();
    }

    public async Task<bool> CanInviteMembersAsync(Guid householdId, string userId)
    {
        var userHousehold = await _userHouseholdRepository.GetAll()
            .FirstOrDefaultAsync(uh => uh.HouseholdId == householdId && uh.UserId == userId);

        if (userHousehold == null)
            return false;

        return userHousehold.IsHouseholdOwner || userHousehold.HasAdminPermissions;
    }
    #endregion Invitation Methods

    #region Private Helper Methods

    /// <summary>
    /// Builds and stages a new <see cref="UserHousehold"/> entity for insertion without calling SaveChangesAsync.
    /// This allows callers to control when the transaction is committed, ensuring atomicity when
    /// multiple changes need to be persisted together.
    /// </summary>
    /// <param name="householdId">The unique identifier of the household.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="userName">The display name of the user.</param>
    /// <param name="isDefaultUserHousehold">Whether this should be the user's default household.</param>
    /// <param name="isHouseholdOwner">Whether the user is the owner of the household.</param>
    /// <param name="hasAdminPermissions">Whether the user has admin permissions.</param>
    /// <param name="hasAllPermissions">
    /// When true, all feature read and write permissions are granted.
    /// When false, all permissions are set to false — used for invited members who start with no access.
    /// </param>
    private async Task<UserHousehold> CreateUserHouseholdAsync(Guid householdId, string userId, string userName, bool isDefaultUserHousehold, bool isHouseholdOwner, bool hasAdminPermissions, bool hasAllPermissions)
    {
        var userHousehold = new UserHousehold
        {
            UserHouseholdId = Guid.NewGuid(),
            UserId = userId,
            UserName = userName,
            HouseholdId = householdId,
            IsDefaultUserHousehold = isDefaultUserHousehold,
            IsHouseholdOwner = isHouseholdOwner,
            IsActive = true,
            HasAdminPermissions = hasAdminPermissions,
            HasFinanceManagerReadPermission = hasAllPermissions,
            HasFinanceManagerWritePermission = hasAllPermissions,
            HasKitchenManagerReadPermission = hasAllPermissions,
            HasKitchenManagerWritePermission = hasAllPermissions,
            HasTaskManagerReadPermission = hasAllPermissions,
            HasTaskManagerCompletePermission = hasAllPermissions,
            HasTaskManagerWritePermission = hasAllPermissions,
            HasFileManagerReadPermission = hasAllPermissions,
            HasFileManagerWritePermission = hasAllPermissions,
        };

        await _userHouseholdRepository.AddAsync(userHousehold);

        return userHousehold;
    }

    /// <summary>
    /// Checks whether the given user-household relationship is the user's current default, and if so,
    /// reassigns the default to the first available active household the user belongs to.
    /// Called before deleting or deactivating a membership to ensure the user always has a
    /// valid default household if one is available.
    /// </summary>
    /// <param name="userHousehold">The user-household relationship that is about to be deleted or deactivated.</param>
    private async Task ReassignDefaultIfNeededAsync(UserHousehold userHousehold)
    {
        if (!userHousehold.IsDefaultUserHousehold) return;

        var newDefault = await _userHouseholdRepository.GetAll()
            .Include(uh => uh.Household)
            .Where(uh =>
                uh.UserId == userHousehold.UserId &&
                uh.UserHouseholdId != userHousehold.UserHouseholdId &&
                uh.IsActive &&
                uh.Household.IsHouseholdActive)
            .OrderBy(uh => uh.UserHouseholdId)
            .FirstOrDefaultAsync();

        if (newDefault != null)
        {
            newDefault.IsDefaultUserHousehold = true;
            await _userHouseholdRepository.UpdateAsync(newDefault);
        }
    }
    #endregion Private Helper Methods
}
