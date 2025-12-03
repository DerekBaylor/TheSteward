using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Dtos.HouseholdDtos;
using TheSteward.Core.IRepositories;
using TheSteward.Core.IServices;
using TheSteward.Core.Models;
using TheSteward.Core.Models.HouseholdModels;

namespace TheSteward.Infrastructure.Services;

public class UserHouseholdService : IUserHouseholdService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserHouseholdRepository _userHouseholdRepository;
    private readonly IMapper _mapper;

    public UserHouseholdService(IUserHouseholdRepository userHouseholdRepository, UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userHouseholdRepository = userHouseholdRepository;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task AddAsync(CreateUserHouseholdDto newUserHousehold, string ownerId)
    {
        if (newUserHousehold == null)
            throw new ArgumentNullException(nameof(newUserHousehold));

        var owner = await _userManager.FindByIdAsync(ownerId);
        if (owner == null)
            throw new KeyNotFoundException($"User with ID {ownerId} not found.");

        var userHousehold = new UserHousehold
        {
            UserHouseholdId = Guid.NewGuid(),
            IsDefaultUserHousehold = newUserHousehold.IsDefaultUserHousehold,
            IsHouseholdOwner = newUserHousehold.IsHouseholdOwner,
            HasAdminPermissions = newUserHousehold.HasAdminPermissions,
            HasFinanceManagerWritePermission = newUserHousehold.HasFinanceManagerWritePermission,
            HasFinanceManagerReadPermission = newUserHousehold.HasFinanceManagerReadPermission,
            HasKitchenManagerWritePermission = newUserHousehold.HasKitchenManagerWritePermission,
            HasKitchenManagerReadPermission = newUserHousehold.HasKitchenManagerReadPermission,
            HasTaskManagerWritePermission = newUserHousehold.HasTaskManagerWritePermission,
            HasTaskManagerReadPermission = newUserHousehold.HasTaskManagerReadPermission,
            HasFileManagerWritePermission = newUserHousehold.HasFileManagerWritePermission,
            HasFileManagerReadPermission = newUserHousehold.HasFileManagerReadPermission,
            UserId = newUserHousehold.UserId,
            HouseholdId = newUserHousehold.HouseholdId
        };

        await _userHouseholdRepository.AddAsync(userHousehold);
        await _userHouseholdRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(UserHousehold userHousehold)
    {
        await _userHouseholdRepository.DeleteAsync(userHousehold);
        await _userHouseholdRepository.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpdateUserHouseholdDto updatedUserHousehold)
    {
        if (updatedUserHousehold.UserHouseholdId == null)
            throw new ArgumentNullException(nameof(updatedUserHousehold.UserHouseholdId));

        var currentUserHousehold = await GetByIdAsync(updatedUserHousehold.UserHouseholdId.Value);

        if (currentUserHousehold == null)
            throw new KeyNotFoundException($"UserHousehold with ID {updatedUserHousehold.UserHouseholdId} not found.");

        _mapper.Map(updatedUserHousehold, currentUserHousehold);

        await _userHouseholdRepository.UpdateAsync(currentUserHousehold);
        await _userHouseholdRepository.SaveChangesAsync();
    }

    public async Task<UserHousehold> GetByIdAsync(Guid id)
    {
        var userHousehold = await _userHouseholdRepository.GetByIdAsync(id);

        if (userHousehold == null)
        {
            throw new KeyNotFoundException($"Household with ID {id} not found.");
        }

        return userHousehold;
    }

    public async Task<List<UserHouseholdDto>> GetAllUserHouseholdsForUserAsync(string userId)
    {
        var userHousehold = await _userHouseholdRepository.GetAll()
            .Where(uh => uh.Household.IsHouseholdActive && uh.UserId == userId)
            .ToListAsync();

        var userHouseholdDto = _mapper.Map<List<UserHouseholdDto>>(userHousehold);

        return userHouseholdDto;
    }

    public async Task<UserHouseholdDto?> GetDefaultUserHouseholdForUserAsync(string userId)
    {
        var userHousehold = await _userHouseholdRepository.GetAll()
            .Include(uh => uh.Household)
            .ThenInclude(h => h.Members)
            .Where(uh => 
                uh.UserId == userId && 
                uh.IsDefaultUserHousehold && 
                uh.Household.IsHouseholdActive)
            .FirstOrDefaultAsync();

        var userHouseholdDto = _mapper.Map<UserHouseholdDto>(userHousehold);

        return userHouseholdDto;
    }

    public async Task<UserHouseholdDto?> GetUserHouseholdByHouseholdIdAndUserIdAsync(Guid householdId, string userId)
    {
        var userHousehold = await _userHouseholdRepository.GetAll()
            .Include(uh => uh.Household)
            .ThenInclude(h => h.Members)
            .Where(uh => uh.UserId == userId && uh.HouseholdId == householdId)
            .FirstOrDefaultAsync();

        var userHouseholdDto = _mapper.Map<UserHouseholdDto>(userHousehold);

        return userHouseholdDto;
    }
}
