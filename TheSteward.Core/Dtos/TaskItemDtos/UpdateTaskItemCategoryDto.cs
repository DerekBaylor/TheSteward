namespace TheSteward.Core.Dtos.TaskItemDtos;

public class UpdateTaskItemCategoryDto
{
    public required string TaskItemCategoryName { get; set; }
    
    public string? IconName { get; set; }
    
    public required string ColorHex { get; set; }
}
