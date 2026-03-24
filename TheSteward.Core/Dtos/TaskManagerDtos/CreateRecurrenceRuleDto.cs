using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Core.Dtos.TaskManagerDtos;

public class CreateRecurrenceRuleDto
{
    public RecurrenceFrequency RecurrenceFrequency { get; set; }
    
    public DaysOfWeek? RecurrenceDays { get; set; }
    
    public int? IntervalDays { get; set; }
    
    public DateTime StartDateTime { get; set; }
    
    public DateTime? EndDateTime { get; set; }
}
