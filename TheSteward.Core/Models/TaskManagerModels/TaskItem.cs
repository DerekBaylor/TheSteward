using System.ComponentModel.DataAnnotations.Schema;
using TheSteward.Core.Models.FinanceManagerModels;
using TheSteward.Core.Models.HouseholdModels;
using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Core.Models.TaskManagerModels;

public class TaskItem
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

    #region Navigation Properties
    public required Guid CreatedByUserHouseholdId { get; set; }

    [ForeignKey("CreatedByUserHouseholdId")]
    public UserHousehold? CreatedByUserHousehold { get; set; }

    public required Guid? AssignedToUserHouseholdId { get; set; }

    [ForeignKey("AssignedToUserHouseholdId")]
    public UserHousehold? AssignedToUserHousehold { get; set; }

    public Guid? RecurrenceId { get; set; }
    
    [ForeignKey("RecurrenceId")]
    public RecurrenceRule? RecurrenceRule { get; set; }

    public Guid TaskItemCategoryId { get; set; }
    
    [ForeignKey("TaskItemCategoryId")]
    public required TaskItemCategory TaskItemCategory { get; set; }
    public Guid? ExpenseId { get; set; }

    [ForeignKey("ExpenseId")]
    public Expense? RelatedExpense { get; set; }

    #endregion Navigation Properties
}
