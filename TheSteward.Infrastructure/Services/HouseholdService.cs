using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.DTOs;
using TheSteward.Core.IRepositories;
using TheSteward.Core.IServices;
using TheSteward.Core.Models;
using AutoMapper;

namespace TheSteward.Infrastructure.Services;

public class HouseholdService : IHouseholdService
{
    private readonly IHouseholdRepository _householdRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserHouseholdService _userHouseholdService;
    private readonly IMapper _mapper;

    public HouseholdService(IHouseholdRepository householdRepository, UserManager<ApplicationUser> userManager, IUserHouseholdService userHouseholdService, IMapper mapper)
    {
        _householdRepository = householdRepository;
        _userManager = userManager;
        _userHouseholdService = userHouseholdService;
        _mapper = mapper;
    }
    public async Task AddAsync(CreateUpdateHouseholdDto newHousehold, string ownerId)
    {
        if (newHousehold == null)
            throw new ArgumentNullException(nameof(newHousehold));

        var owner = await _userManager.FindByIdAsync(ownerId);
        if (owner == null)
            throw new KeyNotFoundException($"User with ID {ownerId} not found.");

        var household = _mapper.Map<Household>(newHousehold);

        await _householdRepository.AddAsync(household);
        await _householdRepository.SaveChangesAsync();

       var createUpdateUserHouseholdDto = new CreateUpdateUserHouseholdDto
        {
            IsDefaultUserHousehold = newHousehold.IsDefaultHousehold,
            IsHouseholdOwner = true,
            HasAdminPermissions = true,
            HasFinanceManagerWritePermission = true,
            HasFinanceManagerReadPermission = true,
            HasKitchenManagerWritePermission = true,
            HasKitchenManagerReadPermission = true,
            HasTaskManagerWritePermission = true,
            HasTaskManagerReadPermission = true,
            HasFileManagerWritePermission = true,
            HasFileManagerReadPermission = true,
            UserId = ownerId,
            User = owner,
            HouseholdId = household.HouseholdId,
            Household = household
       };

        await _userHouseholdService.AddAsync(createUpdateUserHouseholdDto, ownerId);
    }

    public async Task DeleteAsync(HouseholdDto householdDto)
    { 
        var household = _mapper.Map<Household>(householdDto);

        await _householdRepository.DeleteAsync(household);
        await _householdRepository.SaveChangesAsync();
    }

    public async Task UpdateAsync(CreateUpdateHouseholdDto updatedHousehold)
    {
        if (updatedHousehold.HouseholdId == null)
            throw new ArgumentNullException(nameof(updatedHousehold.HouseholdId));

        var currentHousehold = await _householdRepository.GetByIdAsync(updatedHousehold.HouseholdId.Value);

        if (currentHousehold == null)
            throw new KeyNotFoundException($"Household with ID {updatedHousehold.HouseholdId} not found.");

        // Using Automapper on this block could potentially overwrite properties that aren't meant to be changed in this method, like IsHouseholdActive, OwnerId, Owner, and Members.
        var household = new Household
        {
            HasFileManagerAccess = updatedHousehold.HasFileManagerAccess,
            HasFinanceManagerAccess = updatedHousehold.HasFinanceManagerAccess,
            HasMealManagerAccess = updatedHousehold.HasMealManagerAccess,
            HasTaskManagerAccess = updatedHousehold.HasTaskManagerAccess,
            HouseholdId = updatedHousehold.HouseholdId.Value,
            HouseholdName = updatedHousehold.HouseholdName,
            IsHouseholdActive = currentHousehold.IsHouseholdActive,
            OwnerId = currentHousehold.OwnerId,
            Owner = currentHousehold.Owner,
            Members = currentHousehold.Members
        };

        await _householdRepository.UpdateAsync(household);
        await _householdRepository.SaveChangesAsync();
    }

    public async Task<HouseholdDto> GetByIdAsync(Guid id)
    {
        var household = await _householdRepository.GetByIdAsync(id);

        if (household == null)
        {
            throw new KeyNotFoundException($"Household with ID {id} not found.");
        }

        var householdDto = _mapper.Map<HouseholdDto>(household);

        return householdDto;
    }

    public async Task<List<HouseholdDto>> GetAllHouseholdsForUser(string userId)
    {
        var household = await _householdRepository.GetAll()
            .Where(h => h.IsHouseholdActive && h.Members.Any(m => m.Id == userId))
            .ToListAsync();

        var householdDto = _mapper.Map<List<HouseholdDto>>(household);

        return householdDto;
    }
}
