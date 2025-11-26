using AutoMapper;
using TheSteward.Core.Dtos.HouseholdDtos;
using TheSteward.Core.Models.HouseholdModels;
namespace TheSteward.Core.Profiles;

public class HouseholdProfiles : Profile
{
    public HouseholdProfiles()
    {
        CreateMap<Household, HouseholdDto>().ReverseMap();
        CreateMap<Household, CreateHouseholdDto>().ReverseMap();
        CreateMap<Household, UpdateHouseholdDto>().ReverseMap();

        CreateMap<UserHousehold, UserHouseholdDto>()
            .ForMember(h => h.User, opt => opt.MapFrom(src => src.User))
            .ForMember(h => h.Household, opt => opt.MapFrom(src => src.Household))
            .ReverseMap();
        CreateMap<UserHousehold, CreateUserHouseholdDto>().ReverseMap();
        CreateMap<UserHousehold, UpdateUserHouseholdDto>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.HouseholdId, opt => opt.Ignore())
            .ForMember(dest => dest.IsDefaultUserHousehold, opt => opt.Ignore())
            .ForMember(dest => dest.IsHouseholdOwner, opt => opt.Ignore())
            .ReverseMap();
    }
}
