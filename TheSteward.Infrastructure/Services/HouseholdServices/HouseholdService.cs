using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Dtos.HouseholdDtos;
using TheSteward.Core.IRepositories.HouseholdIRepositories;
using TheSteward.Core.IServices.HouseholdIServices;
using TheSteward.Core.Models;
using TheSteward.Core.Models.HouseholdModels;

namespace TheSteward.Infrastructure.Services.HouseholdServices;

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
    public async Task AddAsync(CreateHouseholdDto newHousehold, string ownerId)
    {
        if (newHousehold == null)
            throw new ArgumentNullException(nameof(newHousehold));

        var owner = await _userManager.FindByIdAsync(ownerId);
        if (owner == null)
            throw new KeyNotFoundException($"User with ID {ownerId} not found.");

        var household = _mapper.Map<Household>(newHousehold);
        household.HouseholdId = Guid.NewGuid();
        household.OwnerId = ownerId;
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
    }

    public async Task DeleteAsync(HouseholdDto householdDto)
    { 
        var household = _mapper.Map<Household>(householdDto);

        await _householdRepository.DeleteAsync(household);
        await _householdRepository.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpdateHouseholdDto updatedHousehold)
    {
        var currentHousehold = await _householdRepository.GetByIdAsync(updatedHousehold.HouseholdId);

        if (currentHousehold == null)
            throw new KeyNotFoundException($"Household with ID {updatedHousehold.HouseholdId} not found.");

        currentHousehold.HasFileManagerAccess = updatedHousehold.HasFileManagerAccess;
        currentHousehold.HasFinanceManagerAccess = updatedHousehold.HasFinanceManagerAccess;
        currentHousehold.HasKitchenManagerAccess = updatedHousehold.HasKitchenManagerAccess;
        currentHousehold.HasTaskManagerAccess = updatedHousehold.HasTaskManagerAccess;
        currentHousehold.HouseholdName = updatedHousehold.HouseholdName;
        currentHousehold.HouseholdId = currentHousehold.HouseholdId;
        currentHousehold.IsHouseholdActive = currentHousehold.IsHouseholdActive;
        currentHousehold.OwnerId = currentHousehold.OwnerId;
        currentHousehold.Owner = currentHousehold.Owner;
        currentHousehold.UserHouseholds = currentHousehold.UserHouseholds;

        await _householdRepository.UpdateAsync(currentHousehold);
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
}
