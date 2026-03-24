using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Core.Dtos.TaskItemDtos;

public class UpdateTaskItemOccurrenceDto
{
    public TaskItemStatus Status { get; set; }

    public DateTime? CompletedDate { get; set; }

    public Guid? CompletedByUserHouseholdId { get; set; }
}
