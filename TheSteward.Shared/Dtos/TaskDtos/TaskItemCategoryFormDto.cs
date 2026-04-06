namespace TheSteward.Shared.Dtos.TaskManagerDtos;

public class TaskItemCategoryFormDto
{
    public Guid TaskItemCategoryId { get; set; }
    public string TaskItemCategoryName { get; set; } = string.Empty;
    public string? IconName { get; set; }
    public string ColorHex { get; set; } = "#6366f1"; // sensible default
    public bool IsEditMode { get; set; }
}