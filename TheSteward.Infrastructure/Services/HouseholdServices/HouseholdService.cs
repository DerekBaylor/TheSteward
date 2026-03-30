using Microsoft.AspNetCore.Identity;
using TheSteward.Core.Dtos.HouseholdDtos;
using TheSteward.Core.IRepositories.HouseholdIRepositories;
using TheSteward.Core.IServices.HouseholdIServices;
using TheSteward.Core.Models;
using TheSteward.Core.MappingExtensions;

namespace TheSteward.Infrastructure.Services.HouseholdServices;

public class HouseholdService : IHouseholdService
{
    private readonly IHouseholdRepository _householdRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserHouseholdService _userHouseholdService;

    public HouseholdService(IHouseholdRepository householdRepository, UserManager<ApplicationUser> userManager, IUserHouseholdService userHouseholdService)
    {
        _householdRepository = householdRepository;
        _userManager = userManager;
        _userHouseholdService = userHouseholdService;
    }

    public async Task<HouseholdDto> AddAsync(CreateHouseholdDto newHousehold, string ownerId)
    {
        if (newHousehold == null)
            throw new ArgumentNullException(nameof(newHousehold));

        var owner = await _userManager.FindByIdAsync(ownerId)
            ?? throw new KeyNotFoundException($"User with ID {ownerId} not found.");

        var household = newHousehold.ToEntity(ownerId, owner);
        household.HouseholdId = Guid.NewGuid();
        household.IsHouseholdActive = true;

        await _householdRepository.AddAsync(household);
        await _householdRepository.SaveChangesAsync();

        var createUserHouseholdDto = new CreateUserHouseholdDto
        {
            IsDefaultUserHousehold = newHousehold.IsDefaultHousehold,
            IsHouseholdOwner = true,
            UserName = owner.UserName,
            HasAdminPermissions = true,
            HasFinanceManagerReadPermission = true,
            HasFinanceManagerWritePermission = true,
            HasKitchenManagerReadPermission = true,
            HasKitchenManagerWritePermission = true,
            HasTaskManagerReadPermission = true,
            HasTaskManagerWritePermission = true,
            HasFileManagerReadPermission = true,
            HasFileManagerWritePermission = true,
            UserId = ownerId,
            User = owner,
            HouseholdId = household.HouseholdId,
            Household = household
        };

        await _userHouseholdService.AddAsync(createUserHouseholdDto, ownerId);

        return household.ToDto();
    }

    public async Task DeleteAsync(Guid householdId)
    {
        var household = await _householdRepository.GetByIdAsync(householdId)
            ?? throw new KeyNotFoundException($"Household with ID {householdId} not found.");

        await _householdRepository.DeleteAsync(household);
        await _householdRepository.SaveChangesAsync();
    }

    public async Task<HouseholdDto> UpdateAsync(UpdateHouseholdDto updatedHousehold)
    {
        var currentHousehold = await _householdRepository.GetByIdAsync(updatedHousehold.HouseholdId)
            ?? throw new KeyNotFoundException($"Household with ID {updatedHousehold.HouseholdId} not found.");

        currentHousehold.ApplyUpdate(updatedHousehold);

        await _householdRepository.UpdateAsync(currentHousehold);
        await _householdRepository.SaveChangesAsync();

        return currentHousehold.ToDto();
    }


    public async Task<HouseholdDto> GetByIdAsync(Guid id)
    {
        var household = await _householdRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Household with ID {id} not found.");

        return household.ToDto();
    }
}
