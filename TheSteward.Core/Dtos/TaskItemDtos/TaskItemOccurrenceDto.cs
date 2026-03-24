using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Core.Dtos.TaskItemDtos;

public class TaskItemOccurrenceDto
{
    public Guid TaskItemOccurrenceId { get; set; }
    
    public DateTime ScheduledDateTime { get; set; }
    
    public DateTime? CompletedDate { get; set; }
    
    public TaskItemStatus Status { get; set; }

    #region Scaler Properties
    public Guid TaskItemId { get; set; }
    
    public Guid? CompletedByUserHouseholdId { get; set; }

    #endregion  Scaler Properties

    #region Navigation Properties
    public TaskItemDto? TaskItem { get; set; }
    #endregion Navigation Properties
}
