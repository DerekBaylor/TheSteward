using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Shared.Dtos.TaskDtos;

public class TaskItemFormDto
{
    public Guid? TaskItemId { get; set; }
   
    public string TaskItemName { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;
    
    public TaskItemPriority Priority { get; set; } = TaskItemPriority.Medium;
    
    public DateTime? DueDate { get; set; }
    
    public bool IsEditMode { get; set; }

    #region Navigation Properties

    public Guid CreatedByUserHouseholdId { get; set; }
    
    public Guid? AssignedToUserHouseholdId { get; set; }
    
    public Guid TaskItemCategoryId { get; set; }
    
    public Guid? RecurrenceId { get; set; }
    
    public Guid? ExpenseId { get; set; }
    #endregion

}

