using TheSteward.Core.Dtos.TaskManagerDtos;
using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Core.Utils.TaskManagerUtils;

public static class RecurrenceProjector
{
    /// <summary>
    /// Projects all dates a single RecurrenceRule falls on within [rangeStart, rangeEnd].
    /// Returns dates with no time component (midnight).
    /// </summary>
    public static IEnumerable<DateTime> ProjectRule(RecurrenceRuleDto rule, DateTime rangeStart, DateTime rangeEnd)
    {
        var from = rangeStart.Date;
        var to = rangeEnd.Date;

        // If the rule hasn't started yet within our range, nothing to show
        if (rule.StartDateTime.Date > to)
            yield break;

        // If the rule has already ended before our range, nothing to show
        if (rule.EndDateTime.HasValue && rule.EndDateTime.Value.Date < from)
            yield break;

        // Clamp walk start to whichever is later: rule start or range start
        var walkFrom = rule.StartDateTime.Date > from
            ? rule.StartDateTime.Date
            : from;

        // Clamp walk end to whichever is earlier: rule end or range end
        var walkTo = rule.EndDateTime.HasValue && rule.EndDateTime.Value.Date < to
            ? rule.EndDateTime.Value.Date
            : to;

        switch (rule.RecurrenceFrequency)
        {
            case RecurrenceFrequency.Once:
                if (rule.StartDateTime.Date >= walkFrom && rule.StartDateTime.Date <= walkTo)
                    yield return rule.StartDateTime.Date;
                break;

            case RecurrenceFrequency.Daily:
                foreach (var d in ProjectDaily(walkFrom, walkTo))
                    yield return d;
                break;

            case RecurrenceFrequency.Weekly:
                foreach (var d in ProjectWeekly(rule, walkFrom, walkTo, intervalDays: 7))
                    yield return d;
                break;

            case RecurrenceFrequency.BiWeekly:
                foreach (var d in ProjectWeekly(rule, walkFrom, walkTo, intervalDays: 14))
                    yield return d;
                break;

            case RecurrenceFrequency.Monthly:
                foreach (var d in ProjectMonthly(rule, walkFrom, walkTo))
                    yield return d;
                break;

            case RecurrenceFrequency.Custom:
                foreach (var d in ProjectCustom(rule, walkFrom, walkTo))
                    yield return d;
                break;
        }
    }


    // ── Daily ──────────────────────────────────────────────────────────────

    private static IEnumerable<DateTime> ProjectDaily(DateTime from, DateTime to)
    {
        for (var d = from; d <= to; d = d.AddDays(1))
            yield return d;
    }

    // ── Weekly / BiWeekly ──────────────────────────────────────────────────

    private static IEnumerable<DateTime> ProjectWeekly(
        RecurrenceRuleDto rule,
        DateTime from,
        DateTime to,
        int intervalDays)
    {
        // If specific days-of-week are flagged, walk day-by-day and match flags
        if (rule.RecurrenceDays.HasValue && rule.RecurrenceDays != DaysOfWeek.None)
        {
            for (var d = from; d <= to; d = d.AddDays(1))
            {
                if (rule.RecurrenceDays.Value.HasFlag(DotNetDayToFlag(d.DayOfWeek)))
                    yield return d;
            }
            yield break;
        }

        // No specific days flagged — step by interval anchored to rule start
        var anchor = rule.StartDateTime.Date;

        // Advance anchor to the first step that lands on or after [from]
        if (anchor < from)
        {
            var diff = (from - anchor).Days;
            var steps = (int)Math.Ceiling((double)diff / intervalDays);
            anchor = anchor.AddDays(steps * intervalDays);
        }

        for (var d = anchor; d <= to; d = d.AddDays(intervalDays))
            yield return d;
    }

    // ── Monthly ────────────────────────────────────────────────────────────

    private static IEnumerable<DateTime> ProjectMonthly(
        RecurrenceRuleDto rule,
        DateTime from,
        DateTime to)
    {
        // Anchor day-of-month is taken from the rule's StartDateTime
        var anchorDay = rule.StartDateTime.Day;

        // Start iterating from the first day of the month containing [from]
        var cursor = new DateTime(from.Year, from.Month, 1);

        while (cursor <= to)
        {
            // Clamp to last valid day of this month (handles Feb, 31-day rules, etc.)
            var daysInMonth = DateTime.DaysInMonth(cursor.Year, cursor.Month);
            var candidateDay = Math.Min(anchorDay, daysInMonth);
            var candidate = new DateTime(cursor.Year, cursor.Month, candidateDay);

            if (candidate >= from && candidate <= to)
                yield return candidate;

            cursor = cursor.AddMonths(1);
        }
    }

    // ── Custom (fixed interval in days) ───────────────────────────────────

    private static IEnumerable<DateTime> ProjectCustom(
        RecurrenceRuleDto rule,
        DateTime from,
        DateTime to)
    {
        var interval = rule.IntervalDays ?? 1;
        var anchor = rule.StartDateTime.Date;

        // Advance anchor to the first step that lands on or after [from]
        if (anchor < from)
        {
            var diff = (from - anchor).Days;
            var steps = (int)Math.Ceiling((double)diff / interval);
            anchor = anchor.AddDays(steps * interval);
        }

        for (var d = anchor; d <= to; d = d.AddDays(interval))
            yield return d;
    }

    // ── Shared helper ──────────────────────────────────────────────────────

    private static DaysOfWeek DotNetDayToFlag(DayOfWeek day) => day switch
    {
        DayOfWeek.Sunday => DaysOfWeek.Sunday,
        DayOfWeek.Monday => DaysOfWeek.Monday,
        DayOfWeek.Tuesday => DaysOfWeek.Tuesday,
        DayOfWeek.Wednesday => DaysOfWeek.Wednesday,
        DayOfWeek.Thursday => DaysOfWeek.Thursday,
        DayOfWeek.Friday => DaysOfWeek.Friday,
        DayOfWeek.Saturday => DaysOfWeek.Saturday,
        _ => DaysOfWeek.None
    };
}


