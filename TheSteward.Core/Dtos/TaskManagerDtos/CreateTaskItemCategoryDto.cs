namespace TheSteward.Core.Dtos.TaskManagerDtos;

public class CreateTaskItemCategoryDto
{    
    public required string TaskItemCategoryName { get; set; }
    
    public string? IconName { get; set; }
    
    public required string ColorHex { get; set; }
}
