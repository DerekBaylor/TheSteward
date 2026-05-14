using TheSteward.Core.Dtos.TaskManagerDtos;
using TheSteward.Core.Models.TaskManagerModels;
using TheSteward.Shared.Dtos.DashboardDtos;
using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Core.MappingExtensions;

public static class TaskManagerMappingExtensions
{
    #region TaskItem Mappings

    /// <summary>
    /// Converts a TaskItem entity to its corresponding TaskItemDto representation.
    /// Navigation properties are flattened from loaded navigations explicitly —
    /// GenericMapper cannot resolve nested navigation properties.
    /// </summary>
    public static TaskItemDto ToDto(this TaskItem src)
    {
        var dto = GenericMapper.Map<TaskItem, TaskItemDto>(src);
        dto.TaskItemCategoryName = src.TaskItemCategory?.TaskItemCategoryName;
        dto.TaskItemCategoryColorHex = src.TaskItemCategory?.ColorHex;
        dto.TaskItemCategoryIconName = src.TaskItemCategory?.IconName;
        dto.RelatedExpenseName = src.RelatedExpense?.ExpenseName;
        dto.RelatedExpenseAmountDue = src.RelatedExpense?.AmountDue;
        dto.RecurrenceRule = src.RecurrenceRule?.ToDto();

        return dto;
    }

    /// <summary>
    /// Converts a sequence of TaskItem entities to a list of TaskItemDto objects.
    /// </summary>
    public static List<TaskItemDto> ToDtoList(this IEnumerable<TaskItem> src)
        => src.Select(t => t.ToDto()).ToList();

    /// <summary>
    /// Converts a CreateTaskItemDto to a TaskItem entity.
    /// Pure transformation only — ID stamping, audit fields, and UTC normalization
    /// are handled by the service.
    /// </summary>
    public static TaskItem ToEntity(this CreateTaskItemDto src)
        => GenericMapper.Map<CreateTaskItemDto, TaskItem>(src);

    /// <summary>
    /// Applies updates from an UpdateTaskItemDto to an existing TaskItem entity.
    /// Pure transformation only — UTC normalization and UpdatedDate stamping
    /// are handled by the service.
    /// </summary>
    public static void ApplyUpdate(this TaskItem entity, UpdateTaskItemDto src)
        => GenericMapper.MapProperties(src, entity);

    #endregion TaskItem Mappings

    #region TaskItemCategory Mappings

    /// <summary>
    /// Converts a TaskItemCategory entity to its corresponding TaskItemCategoryDto representation.
    /// If TaskItems are loaded, they are mapped explicitly — GenericMapper cannot resolve
    /// a child entity collection to a child DTO collection.
    /// </summary>
    public static TaskItemCategoryDto ToDto(this TaskItemCategory src)
    {
        var dto = GenericMapper.Map<TaskItemCategory, TaskItemCategoryDto>(src);
        dto.TaskItems = src.TaskItems?.Select(t => t.ToDto()).ToList();

        return dto;
    }

    /// <summary>
    /// Converts a sequence of TaskItemCategory entities to a list of TaskItemCategoryDto objects.
    /// Delegates to ToDto() to ensure TaskItems nav collection is mapped correctly.
    /// </summary>
    public static List<TaskItemCategoryDto> ToDtoList(this IEnumerable<TaskItemCategory> src)
        => src.Select(c => c.ToDto()).ToList();

    /// <summary>
    /// Converts a CreateTaskItemCategoryDto to a TaskItemCategory entity.
    /// Pure transformation only — ID stamping is handled by the service.
    /// </summary>
    public static TaskItemCategory ToEntity(this CreateTaskItemCategoryDto src)
        => GenericMapper.Map<CreateTaskItemCategoryDto, TaskItemCategory>(src);

    /// <summary>
    /// Applies updates from an UpdateTaskItemCategoryDto to an existing TaskItemCategory entity.
    /// Pure transformation only.
    /// </summary>
    public static void ApplyUpdate(this TaskItemCategory entity, UpdateTaskItemCategoryDto src)
        => GenericMapper.MapProperties(src, entity);

    #endregion TaskItemCategory Mappings

    #region RecurrenceRule Mappings

    /// <summary>
    /// Converts a RecurrenceRule entity to its corresponding RecurrenceRuleDto representation.
    /// TaskItems are intentionally NOT mapped here to prevent a bidirectional mapping loop
    /// (TaskItem → RecurrenceRule → TaskItems → TaskItem → ...).
    /// If you need TaskItems populated on the DTO, call ToDto(includeTaskItems: true) explicitly.
    /// </summary>
    public static RecurrenceRuleDto ToDto(this RecurrenceRule src)
    {
        var dto = GenericMapper.Map<RecurrenceRule, RecurrenceRuleDto>(src);
        // TaskItems deliberately omitted — see summary above
        return dto;
    }

    /// <summary>
    /// Converts a RecurrenceRule entity to its DTO, optionally including the TaskItems collection.
    /// Only call this overload when you are querying a rule directly and explicitly need its tasks.
    /// Never call this from within TaskItem.ToDto().
    /// </summary>
    public static RecurrenceRuleDto ToDto(this RecurrenceRule src, bool includeTaskItems)
    {
        var dto = GenericMapper.Map<RecurrenceRule, RecurrenceRuleDto>(src);

        if (includeTaskItems)
            dto.TaskItems = src.TaskItems?.Select(t => t.ToDto()).ToList();

        return dto;
    }

    /// <summary>
    /// Converts a sequence of RecurrenceRule entities to a list of RecurrenceRuleDto objects.
    /// </summary>
    public static List<RecurrenceRuleDto> ToDtoList(this IEnumerable<RecurrenceRule> src)
        => src.Select(r => r.ToDto()).ToList();

    /// <summary>
    /// Converts a CreateRecurrenceRuleDto to a RecurrenceRule entity.
    /// Pure transformation only — ID stamping, LastGeneratedDateTime, and UTC normalization
    /// are handled by the service.
    /// </summary>
    public static RecurrenceRule ToEntity(this CreateRecurrenceRuleDto src)
        => GenericMapper.Map<CreateRecurrenceRuleDto, RecurrenceRule>(src);

    /// <summary>
    /// Applies updates from an UpdateRecurrenceRuleDto to an existing RecurrenceRule entity.
    /// Pure transformation only — UTC normalization is handled by the service.
    /// StartDateTime is intentionally excluded — it is not updatable after creation.
    /// </summary>
    public static void ApplyUpdate(this RecurrenceRule entity, UpdateRecurrenceRuleDto src)
        => GenericMapper.MapProperties(src, entity);

    #endregion RecurrenceRule Mappings

    #region TaskItemOccurrence Mappings

    /// <summary>
    /// Converts a TaskItemOccurrence entity to its corresponding TaskItemOccurrenceDto representation.
    /// If TaskItem is loaded, it is mapped explicitly — GenericMapper cannot resolve
    /// a child entity navigation to a child DTO.
    /// </summary>
    public static TaskItemOccurrenceDto ToDto(this TaskItemOccurrence src)
    {
        var dto = GenericMapper.Map<TaskItemOccurrence, TaskItemOccurrenceDto>(src);
        dto.TaskItem = src.TaskItem?.ToDto();

        return dto;
    }

    /// <summary>
    /// Converts a sequence of TaskItemOccurrence entities to a list of TaskItemOccurrenceDto objects.
    /// </summary>
    public static List<TaskItemOccurrenceDto> ToDtoList(this IEnumerable<TaskItemOccurrence> src)
        => src.Select(o => o.ToDto()).ToList();

    /// <summary>
    /// Converts a CreateTaskItemOccurrenceDto to a TaskItemOccurrence entity.
    /// Pure transformation only — ID stamping and UTC normalization are handled by the service.
    /// </summary>
    public static TaskItemOccurrence ToEntity(this CreateTaskItemOccurrenceDto src)
        => GenericMapper.Map<CreateTaskItemOccurrenceDto, TaskItemOccurrence>(src);

    /// <summary>
    /// Applies updates from an UpdateTaskItemOccurrenceDto to an existing TaskItemOccurrence entity.
    /// Pure transformation only — UTC normalization is handled by the service.
    /// </summary>
    public static void ApplyUpdate(this TaskItemOccurrence entity, UpdateTaskItemOccurrenceDto src)
        => GenericMapper.MapProperties(src, entity);


    /// <summary>
    /// Converts a TaskItemOccurrenceDto (with TaskItem populated) to a DashboardOccurrenceDto.
    /// All task-level fields are read from dto.TaskItem — TaskItemOccurrenceDto does not own them.
    /// </summary>
    public static DashboardOccurrenceDto ToDashboardDto(this TaskItemOccurrenceDto src)
    {
        var task = src.TaskItem;

        return new DashboardOccurrenceDto
        {
            TaskItemOccurrenceId = src.TaskItemOccurrenceId,
            TaskItemId = src.TaskItemId,
            ScheduledDate = src.ScheduledDateTime.ToLocalTime().Date,
            ScheduledDateTime = src.ScheduledDateTime,
            Status = src.Status,

            TaskItemName = task?.TaskItemName ?? "Unknown Task",
            Description = task?.Description,
            TaskItemCategoryName = task?.TaskItemCategoryName,
            TaskItemCategoryColorHex = task?.TaskItemCategoryColorHex,
            TaskItemCategoryIconName = task?.TaskItemCategoryIconName,
            Priority = task?.Priority ?? TaskItemPriority.Medium,
            IsRecurring = task?.RecurrenceId != null,
            ExpenseId = task?.ExpenseId,
            RelatedExpenseName = task?.RelatedExpenseName,
            RelatedExpenseAmountDue = task?.RelatedExpenseAmountDue,
        };
    }

    /// <summary>
    /// Converts a sequence of TaskItemOccurrenceDtos to a list of DashboardOccurrenceDtos.
    /// </summary>
    public static List<DashboardOccurrenceDto> ToDashboardDtoList(this IEnumerable<TaskItemOccurrenceDto> src)
        => src.Select(o => o.ToDashboardDto()).ToList();

    #endregion TaskItemOccurrence Mappings

}



