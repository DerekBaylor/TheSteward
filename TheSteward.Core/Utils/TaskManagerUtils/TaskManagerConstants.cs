namespace TheSteward.Core.Utils.TaskManagerUtils;

public static class TaskManagerConstants
{
    public enum TaskItemStatus
    {
        Pending,
        InProgress,
        Completed,
        Skipped
    }

    public enum TaskItemPriority
    {
        Low,
        Medium,
        High,
        Urgent
    }

    public enum RecurrenceFrequency
    {
        Daily,
        Weekly,
        BiWeekly,
        Monthly,
        Custom
    }

    [Flags]
    public enum DaysOfWeek
    {
        None = 0,
        Sunday = 1,
        Monday = 2,
        Tuesday = 4,
        Wednesday = 8,
        Thursday = 16,
        Friday = 32,
        Saturday = 64
    }
}
