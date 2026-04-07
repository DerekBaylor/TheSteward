namespace TheSteward.Shared.Dtos.TaskManagerDtos;

public class StandardTaskSelectionDto
{
    public string TaskName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public bool IsSelected { get; set; } = true;
    public bool AlreadyExists { get; set; } = false;
}
