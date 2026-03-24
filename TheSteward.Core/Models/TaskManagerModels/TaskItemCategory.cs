namespace TheSteward.Core.Models.TaskManagerModels;

public class TaskItemCategory
{
    public Guid TaskItemCategoryId { get; set; }

    public required string TaskItemCategoryName { get; set; }

    public string? IconName { get; set; }

    public required string ColorHex { get; set; }

    public IList<TaskItem>? TaskItems { get; set; }
}
