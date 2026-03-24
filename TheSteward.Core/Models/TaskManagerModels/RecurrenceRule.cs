using System.ComponentModel.DataAnnotations.Schema;
using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Core.Models.TaskManagerModels;

public class RecurrenceRule
{
    public Guid RecurrenceRuleId { get; set; }

    public RecurrenceFrequency RecurrenceFrequency { get; set; }

    public DaysOfWeek? RecurrenceDays { get; set; }

    public int? IntervalDays { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime? EndDateTime { get; set; }
    
    public DateTime LastGeneratedDateTime { get; set; }

    #region Navigation Properties

    public IList<TaskItem>? TaskItems { get; set; }

    #endregion Navigation Properties
}
