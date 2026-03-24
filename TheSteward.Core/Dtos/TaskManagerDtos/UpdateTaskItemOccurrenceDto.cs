using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Core.Dtos.TaskManagerDtos;

public class UpdateTaskItemOccurrenceDto
{
    public TaskItemStatus Status { get; set; }

    public DateTime? CompletedDate { get; set; }

    public Guid? CompletedByUserHouseholdId { get; set; }
}
