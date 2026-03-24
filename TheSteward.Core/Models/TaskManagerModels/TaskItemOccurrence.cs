using System.ComponentModel.DataAnnotations.Schema;
using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Core.Models.TaskManagerModels;

public class TaskItemOccurrence
{
    public Guid TaskItemOccurrenceId { get; set; }


    public DateTime ScheduledDateTime { get; set; }

    public DateTime? CompletedDate { get; set; }

    public TaskItemStatus Status { get; set; }

    #region Navigation Properties
    public Guid? CompletedByUserHouseholdId { get; set; }
    public Guid TaskItemId { get; set; }

    [ForeignKey("TaskItemId")]
    public TaskItem? TaskItem { get; set; }

    #endregion Navigation Properties

}
