namespace TheSteward.Shared.Dtos.TaskManagerDtos;

public class TaskItemCategoryFormDto
{
    public Guid? TaskItemCategoryId { get; set; }
    public string TaskItemCategoryName { get; set; } = string.Empty;
    public string? IconName { get; set; }
    public string ColorHex { get; set; } = "#607D8B";
    public bool IsEditing { get; set; } = false;
    public bool IsEditMode { get; set; } = false;
}