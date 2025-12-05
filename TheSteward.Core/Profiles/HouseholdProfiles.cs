using AutoMapper;
using TheSteward.Core.Dtos.HouseholdDtos;
using TheSteward.Core.Models.HouseholdModels;

namespace TheSteward.Core.Profiles;

public class HouseholdProfiles : Profile
{
    public HouseholdProfiles()
    {
        CreateMap<Household, HouseholdDto>()
            .ForMember(dest => dest.UserHouseholdDtos, opt => opt.MapFrom(src => src.UserHouseholds))
            .ReverseMap();

        CreateMap<Household, CreateHouseholdDto>().ReverseMap();

        CreateMap<Household, UpdateHouseholdDto>().ReverseMap();

        CreateMap<UserHousehold, UserHouseholdDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.Household, opt => opt.MapFrom(src => src.Household))
            .ReverseMap();

        CreateMap<UserHousehold, CreateUserHouseholdDto>().ReverseMap();

        CreateMap<UserHousehold, UpdateUserHouseholdDto>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.HouseholdId, opt => opt.Ignore())
            .ForMember(dest => dest.IsDefaultUserHousehold, opt => opt.Ignore())
            .ForMember(dest => dest.IsHouseholdOwner, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<HouseholdInvitation, HouseholdInvitationDto>()
            .ForMember(dest => dest.HouseholdName, opt => opt.MapFrom(src => src.Household.HouseholdName))
            .ForMember(dest => dest.InvitedByUserName, opt => opt.MapFrom(src => src.InvitedByUser.UserName))
            .ReverseMap();

        CreateMap<InviteUserToHouseholdDto, HouseholdInvitation>()
            .ReverseMap();
    }
}