using TheSteward.Core.Dtos.FinanceManagerDtos;
using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Core.Dtos.TaskManagerDtos;

public class TaskItemDto
{
    public Guid TaskItemId { get; set; }
    
    public string TaskItemName { get; set; }
    
    public string? Description { get; set; }
    
    public TaskItemStatus Status { get; set; }
    
    public TaskItemPriority Priority { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    public DateTime? CompletedDate { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public DateTime UpdatedDate { get; set; }
    
    public bool IsArchived { get; set; }

    public string? LinkedExpenseName { get; set; }
    
    public decimal? LinkedExpenseAmount { get; set; }

    #region Navigation Properties
    
    public Guid CreatedByUserHouseholdId { get; set; }
    
    public Guid? AssignedToUserHouseholdId { get; set; }
        
    public Guid? RecurrenceId { get; set; }
    
    public Guid TaskItemCategoryId { get; set; }
    
    public string? TaskItemCategoryName { get; set; }
    
    public string? TaskItemCategoryColor { get; set; }
    
    public string? TaskItemCategoryIcon { get; set; }
    
    public Guid? ExpenseId { get; set; }

    #endregion Navigation Properties
}
