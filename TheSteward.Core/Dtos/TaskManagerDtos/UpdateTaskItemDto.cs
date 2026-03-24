using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Core.Dtos.TaskManagerDtos;

public class UpdateTaskItemDto
{
    public required string TaskItemName { get; set; }
    
    public string? Description { get; set; }
    
    public TaskItemStatus Status { get; set; }
    
    public TaskItemPriority Priority { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    public DateTime? CompletedDate { get; set; }

    #region Scaler Properties
    public Guid? AssignedToUserHouseholdId { get; set; }
    
    public required Guid TaskItemCategoryId { get; set; }
    
    public Guid? RecurrenceId { get; set; }
    
    public Guid? ExpenseId { get; set; }
    #endregion Scaler Properties
}
