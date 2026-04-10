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
    
    public Guid CreatedByUserHouseholdId { get; set; }
    
    public Guid AssignedToUserHouseholdId { get; set; }
    
    public Guid TaskItemCategoryId { get; set; }
    
    public Guid? RecurrenceId { get; set; }
    
    public Guid? ExpenseId { get; set; }
    
    public bool IsEditMode { get; set; }

    /// <summary>True when the user wants this task to repeat.</summary>
    public bool IsRecurring { get; set; }

    public RecurrenceFrequency RecurrenceFrequency { get; set; } = RecurrenceFrequency.Weekly;

    /// <summary>Bit-flag days; only relevant when frequency is Weekly or BiWeekly.</summary>
    public DaysOfWeek RecurrenceDays { get; set; } = DaysOfWeek.None;

    /// <summary>Only used when frequency is Custom.</summary>
    public int? IntervalDays { get; set; }

    public DateTime RecurrenceStartDate { get; set; } = DateTime.Today;

    public DateTime? RecurrenceEndDate { get; set; }

    public Guid HouseholdId { get; set; }
}

