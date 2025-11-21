using AutoMapper;
using TheSteward.Core.DTOs;
using TheSteward.Core.Models;
namespace TheSteward.Core.Profiles;

public class HouseholdProfiles : Profile
{
    public HouseholdProfiles()
    {
        CreateMap<Household, HouseholdDto>().ReverseMap();
        CreateMap<Household, CreateUpdateHouseholdDto>().ReverseMap();

        CreateMap<UserHousehold, UserHouseholdDto>()
            .ForMember(h => h.Household, opt => opt.MapFrom(src => src.Household))
            .ReverseMap();
        CreateMap<UserHousehold, CreateUpdateUserHouseholdDto>().ReverseMap();
    }
}
