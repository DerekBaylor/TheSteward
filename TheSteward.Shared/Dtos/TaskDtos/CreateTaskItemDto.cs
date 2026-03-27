using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Shared.Dtos.TaskDtos;

public class CreateTaskItemDto
{
    public required string TaskItemName { get; set; }

    public string? Description { get; set; }

    public TaskItemStatus Status { get; set; }

    public TaskItemPriority Priority { get; set; }

    public DateTime? DueDate { get; set; }

    #region Navigation Properties

    public required Guid CreatedByUserHouseholdId { get; set; }

    public Guid? AssignedToUserHouseholdId { get; set; }

    public Guid TaskItemCategoryId { get; set; }

    public Guid? RecurrenceId { get; set; }

    public Guid? ExpenseId { get; set; }
    #endregion Navigation Properties
}

