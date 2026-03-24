using TheSteward.Core.Dtos.FinanceManagerDtos;
using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Core.Dtos.TaskItemDtos;

public class TaskItemDto
{
    public Guid TaskItemId { get; set; }
    
    public required string TaskItemName { get; set; }
    
    public string? Description { get; set; }
    
    public TaskItemStatus Status { get; set; }
    
    public TaskItemPriority Priority { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    public DateTime? CompletedDate { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public DateTime UpdatedDate { get; set; }

    #region Scaler Properties
    public Guid CreatedByUserHouseholdId { get; set; }
    
    public Guid? AssignedToUserHouseholdId { get; set; }
    
    public Guid TaskItemCategoryId { get; set; }
    
    public Guid? RecurrenceId { get; set; }
    
    public Guid? ExpenseId { get; set; }
    #endregion Scaler Properties

    #region Navigation Properties

    public TaskItemCategoryDto? TaskItemCategory { get; set; }
    
    public RecurrenceRuleDto? RecurrenceRule { get; set; }
   
    public ExpenseDto? RelatedExpense { get; set; }
    #endregion Navigation Properties
}
