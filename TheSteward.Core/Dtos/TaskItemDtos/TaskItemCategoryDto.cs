namespace TheSteward.Core.Dtos.TaskItemDtos;

public class TaskItemCategoryDto
{
    public Guid TaskItemCategoryId { get; set; }
    
    public required string TaskItemCategoryName { get; set; }
    
    public string? IconName { get; set; }
    
    public required string ColorHex { get; set; }
    
    public IList<TaskItemDto>? TaskItems { get; set; }
}
