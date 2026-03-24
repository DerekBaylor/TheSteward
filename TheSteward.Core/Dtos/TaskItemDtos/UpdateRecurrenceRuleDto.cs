using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Core.Dtos.TaskItemDtos;

public class UpdateRecurrenceRuleDto
{
    public RecurrenceFrequency RecurrenceFrequency { get; set; }
    
    public DaysOfWeek? RecurrenceDays { get; set; }
    
    public int? IntervalDays { get; set; }
    
    public DateTime? EndDateTime { get; set; }
}
