using AutoMapper;

namespace TheSteward.Core.Profiles;

public class HouseholdProfiles : Profile
{
    public HouseholdProfiles()
    {
        CreateMap<Models.Household, DTOs.HouseholdDto>().ReverseMap();
        CreateMap<Models.Household, DTOs.CreateUpdateHouseholdDto>().ReverseMap();
    }
}
