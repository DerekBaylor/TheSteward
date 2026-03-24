using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Core.Dtos.TaskManagerDtos;

public class CreateTaskItemOccurrenceDto
{
    public DateTime ScheduledDateTime { get; set; }
    
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;
   
    public required Guid TaskItemId { get; set; }
}
