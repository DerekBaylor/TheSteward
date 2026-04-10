namespace TheSteward.Core.Utils.TaskManagerUtils;

public static class RecurrenceUtility
{
    /// <summary>
    /// Calculates the next occurrence of the given due day in the current or
    /// next month. Treats DueDay 31 as "last day of the month".
    /// </summary>
    public static DateTime GetNextDueDateFromDueDay(int dueDay)
    {
        var now = DateTime.UtcNow;
        var daysInCurrentMonth = DateTime.DaysInMonth(now.Year, now.Month);
        var resolvedDay = dueDay > daysInCurrentMonth ? daysInCurrentMonth : dueDay;
        var candidateDate = new DateTime(now.Year, now.Month, resolvedDay, 0, 0, 0, DateTimeKind.Utc);

        if (candidateDate <= now)
        {
            var nextMonth = now.AddMonths(1);
            var daysInNextMonth = DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month);
            resolvedDay = dueDay > daysInNextMonth ? daysInNextMonth : dueDay;
            candidateDate = new DateTime(nextMonth.Year, nextMonth.Month, resolvedDay, 0, 0, 0, DateTimeKind.Utc);
        }

        return candidateDate;
    }

}

