using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Core.Dtos.TaskManagerDtos;

public class CreateTaskItemDto
{
    public required string TaskItemName { get; set; }
    public string? Description { get; set; }
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;
    public TaskItemPriority Priority { get; set; } = TaskItemPriority.Medium;
    public DateTime? DueDate { get; set; }

    public required Guid CreatedByUserHouseholdId { get; set; }
    public Guid? AssignedToUserHouseholdId { get; set; }
    public required Guid TaskItemCategoryId { get; set; }
    public Guid? RecurrenceId { get; set; }
    public Guid? ExpenseId { get; set; }

    /// <summary>
    /// When provided, the service will create the RecurrenceRule, link it to
    /// the task, and generate the first TaskItemOccurrence automatically.
    /// </summary>
    public CreateRecurrenceRuleDto? RecurrenceRule { get; set; }
}
