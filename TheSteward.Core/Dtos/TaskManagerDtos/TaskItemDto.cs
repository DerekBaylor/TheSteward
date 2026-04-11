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
    
    public bool IsPrivate { get; set; }

    #region Navigation Properties

    public Guid HouseholdId { get; set; }
    
    public Guid CreatedByUserHouseholdId { get; set; }
    
    public Guid? AssignedToUserHouseholdId { get; set; }
    
    public Guid? RecurrenceId { get; set; }
    
    public Guid TaskItemCategoryId { get; set; }
    
    public string? TaskItemCategoryName { get; set; }
    
    public string? TaskItemCategoryColorHex { get; set; }
    
    public string? TaskItemCategoryIconName { get; set; }
    
    public Guid? ExpenseId { get; set; }
    
    public string? RelatedExpenseName { get; set; }
    
    public decimal? RelatedExpenseAmountDue { get; set; }

    #endregion Navigation Properties
}
