using AutoMapper;
using TheSteward.Core.Dtos.TaskItemDtos;
using TheSteward.Core.Models.TaskManagerModels;

namespace TheSteward.Core.Profiles;

public class TaskManagerProfiles : Profile
{
    public TaskManagerProfiles()
    {
        // TaskItemCategory
        CreateMap<TaskItemCategory, TaskItemCategoryDto>()
            .ForMember(dest => dest.TaskItems,
                opt => opt.MapFrom(src => src.TaskItems ?? new List<TaskItem>()))
            .ReverseMap();

        CreateMap<TaskItemCategory, CreateTaskItemCategoryDto>().ReverseMap();

        CreateMap<TaskItemCategory, UpdateTaskItemCategoryDto>().ReverseMap();

        // RecurrenceRule
        CreateMap<RecurrenceRule, RecurrenceRuleDto>()
                .ForMember(dest => dest.TaskItems,
                    opt => opt.MapFrom(src => src.TaskItems ?? new List<TaskItem>()))
                .ReverseMap()
                .ForMember(dest => dest.TaskItems, opt => opt.Ignore());

        CreateMap<RecurrenceRule, CreateRecurrenceRuleDto>().ReverseMap();

        CreateMap<RecurrenceRule, UpdateRecurrenceRuleDto>()
                .ReverseMap()
                .ForMember(dest => dest.StartDateTime, opt => opt.Ignore());

        // TaskItem
        CreateMap<TaskItem, TaskItemDto>()
                .ForMember(dest => dest.TaskItemCategory, opt => opt.MapFrom(src => src.TaskItemCategory))
                .ForMember(dest => dest.RecurrenceRule, opt => opt.MapFrom(src => src.RecurrenceRule))
                .ForMember(dest => dest.RelatedExpense, opt => opt.MapFrom(src => src.RelatedExpense))
                .ReverseMap()
                .ForMember(dest => dest.CreatedByUserHousehold, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedToUserHousehold, opt => opt.Ignore())
                .ForMember(dest => dest.TaskItemCategory, opt => opt.Ignore())
                .ForMember(dest => dest.RecurrenceRule, opt => opt.Ignore())
                .ForMember(dest => dest.RelatedExpense, opt => opt.Ignore());

        CreateMap<TaskItem, CreateTaskItemDto>()
                .ReverseMap()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserHousehold, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedToUserHousehold, opt => opt.Ignore())
                .ForMember(dest => dest.TaskItemCategory, opt => opt.Ignore())
                .ForMember(dest => dest.RecurrenceRule, opt => opt.Ignore())
                .ForMember(dest => dest.RelatedExpense, opt => opt.Ignore());

        CreateMap<TaskItem, UpdateTaskItemDto>()
                .ReverseMap()
                .ForMember(dest => dest.CreatedByUserHouseholdId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserHousehold, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedToUserHousehold, opt => opt.Ignore())
                .ForMember(dest => dest.TaskItemCategory, opt => opt.Ignore())
                .ForMember(dest => dest.RecurrenceRule, opt => opt.Ignore())
                .ForMember(dest => dest.RelatedExpense, opt => opt.Ignore());

        // TaskItemOccurrence
        CreateMap<TaskItemOccurrence, TaskItemOccurrenceDto>()
                .ForMember(dest => dest.TaskItem, opt => opt.MapFrom(src => src.TaskItem))
                .ReverseMap()
                .ForMember(dest => dest.TaskItem, opt => opt.Ignore());

        CreateMap<TaskItemOccurrence, CreateTaskItemOccurrenceDto>()
                .ReverseMap()
                .ForMember(dest => dest.CompletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CompletedByUserHouseholdId, opt => opt.Ignore())
                .ForMember(dest => dest.TaskItem, opt => opt.Ignore());

        CreateMap<TaskItemOccurrence, UpdateTaskItemOccurrenceDto>()
                .ReverseMap()
                .ForMember(dest => dest.TaskItemId, opt => opt.Ignore())
                .ForMember(dest => dest.ScheduledDateTime, opt => opt.Ignore())
                .ForMember(dest => dest.TaskItem, opt => opt.Ignore());
    }
}
