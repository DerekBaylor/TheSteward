using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Shared.Dtos.DashboardDtos;

public class DashboardOccurrenceDto
{
    public Guid TaskItemOccurrenceId { get; set; }
    public Guid TaskItemId { get; set; }

    public string TaskItemName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? TaskItemCategoryName { get; set; }
    public string? TaskItemCategoryColorHex { get; set; }
    public string? TaskItemCategoryIconName { get; set; }

    public DateTime ScheduledDate { get; set; }
    public DateTime ScheduledDateTime { get; set; }

    public TaskItemStatus Status { get; set; }
    public TaskItemPriority Priority { get; set; }

    public bool IsRecurring { get; set; }
    public Guid? ExpenseId { get; set; }
    public string? RelatedExpenseName { get; set; }
    public decimal? RelatedExpenseAmountDue { get; set; }
}