using TheSteward.Core.Models.TaskManagerModels;
using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Core.Utils.TaskManagerUtils;

public record StandardTaskRecurrenceDefinition(
    RecurrenceFrequency Frequency,
    DaysOfWeek? RecurrenceDays = null,
    int? IntervalDays = null);

public record StandardTaskDefinition(
    string TaskName,
    string CategoryName,
    StandardTaskRecurrenceDefinition? Recurrence = null);

public record TaskCategoryIconOption(string Label, string IconName);

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

    public static readonly IReadOnlyList<TaskCategoryIconOption> CategoryIconOptions = new List<TaskCategoryIconOption>
    {
        // Home & Living
        new("Home",          "home"),
        new("Cleaning",      "cleaning_services"),
        new("Laundry",       "local_laundry_service"),
        new("Kitchen",       "kitchen"),
        new("Yard",          "yard"),
        new("Repairs",       "handyman"),
        new("Trash",         "delete"),

        // Health & Wellness
        new("Health",        "favorite"),
        new("Fitness",       "fitness_center"),
        new("Medical",       "medical_services"),
        new("Medication",    "medication"),

        // Finance
        new("Finance",       "payments"),
        new("Bills",         "receipt_long"),
        new("Shopping",      "shopping_cart"),

        // Work & Productivity
        new("Work",          "work"),
        new("Meetings",      "groups"),
        new("School",        "school"),
        new("Research",      "manage_search"),

        // Family & Social
        new("Family",        "family_restroom"),
        new("Pets",          "pets"),
        new("Events",        "celebration"),
        new("Travel",        "flight"),

        // Misc
        new("General",       "task_alt"),
        new("Errands",       "directions_car"),
        new("Food",          "restaurant"),
        new("Garden",        "park"),
        new("Subscriptions", "subscriptions"),
        new("Notes",         "sticky_note_2"),
    };

    public static readonly IReadOnlyList<StandardTaskDefinition> StandardTasks = new List<StandardTaskDefinition>
{
    // ── Daily ──────────────────────────────────────────────────────────────
    new("Sweeping hard floors",
        "Cleaning",
        new(RecurrenceFrequency.Daily)),

    new("Wiping down kitchen counters and stovetop",
        "Cleaning",
        new(RecurrenceFrequency.Daily)),

    new("Cleaning the sink and faucets",
        "Cleaning",
        new(RecurrenceFrequency.Daily)),

    new("Tidying and decluttering common areas",
        "Cleaning",
        new(RecurrenceFrequency.Daily)),

    new("Washing dishes / running the dishwasher",
        "Cleaning",
        new(RecurrenceFrequency.Daily)),

    // ── Weekly ─────────────────────────────────────────────────────────────
    new("Vacuuming floors and carpets",
        "Cleaning",
        new(RecurrenceFrequency.Weekly)),

    new("Mopping hard floors",
        "Cleaning",
        new(RecurrenceFrequency.Weekly)),

    new("Cleaning the sink and faucets (weekly)",
        "Cleaning",
        new(RecurrenceFrequency.Weekly)),

    new("Taking out the trash and recycling",
        "Cleaning",
        new(RecurrenceFrequency.Weekly)),

    new("Doing laundry (washing, drying, folding, and putting away)",
        "Cleaning",
        new(RecurrenceFrequency.Weekly)),

    new("Changing bed linens",
        "Cleaning",
        new(RecurrenceFrequency.Weekly)),

    new("Cleaning toilets, sinks, and showers/tubs",
        "Cleaning",
        new(RecurrenceFrequency.Weekly)),

    new("Wiping down mirrors and glass surfaces",
        "Cleaning",
        new(RecurrenceFrequency.Weekly)),

    new("Dusting furniture and surfaces",
        "Cleaning",
        new(RecurrenceFrequency.Weekly)),

    new("Cut Grass",
        "Yard",
        new(RecurrenceFrequency.Weekly)),

    // ── Monthly ────────────────────────────────────────────────────────────
    new("Cleaning the inside of the microwave and oven",
        "Cleaning",
        new(RecurrenceFrequency.Monthly)),

    new("Wiping down kitchen appliances",
        "Cleaning",
        new(RecurrenceFrequency.Monthly)),

    new("Cleaning inside the refrigerator",
        "Cleaning",
        new(RecurrenceFrequency.Monthly)),

    new("Washing windows (interior)",
        "Cleaning",
        new(RecurrenceFrequency.Monthly)),

    new("Dusting ceiling fans and light fixtures",
        "Cleaning",
        new(RecurrenceFrequency.Monthly)),

    new("Scrubbing grout in bathrooms",
        "Cleaning",
        new(RecurrenceFrequency.Monthly)),

    new("Cleaning out the pantry and checking expiration dates",
        "Cleaning",
        new(RecurrenceFrequency.Monthly)),

    new("Washing throw pillows and blankets",
        "Cleaning",
        new(RecurrenceFrequency.Monthly)),

    new("Cleaning baseboards and door frames",
        "Cleaning",
        new(RecurrenceFrequency.Monthly)),

    new("Vacuuming upholstered furniture",
        "Cleaning",
        new(RecurrenceFrequency.Monthly)),

    new("Checking and replacing air filters (if needed)",
        "Repairs",
        new(RecurrenceFrequency.Monthly)),

    // ── Quarterly (every 90 days via Custom) ───────────────────────────────
    new("Deep cleaning the oven",
        "Cleaning",
        new(RecurrenceFrequency.Custom, IntervalDays: 90)),

    new("Cleaning behind and underneath large appliances",
        "Cleaning",
        new(RecurrenceFrequency.Custom, IntervalDays: 90)),

    new("Washing windows (exterior)",
        "Cleaning",
        new(RecurrenceFrequency.Custom, IntervalDays: 90)),

    new("Rotating and flipping mattresses",
        "Cleaning",
        new(RecurrenceFrequency.Custom, IntervalDays: 90)),

    new("Cleaning out closets and donating unused items",
        "General",
        new(RecurrenceFrequency.Custom, IntervalDays: 90)),

    new("Descaling faucets and showerheads",
        "Cleaning",
        new(RecurrenceFrequency.Custom, IntervalDays: 90)),

    new("Cleaning window tracks and door tracks",
        "Cleaning",
        new(RecurrenceFrequency.Custom, IntervalDays: 90)),

    new("Checking smoke and carbon monoxide detector batteries",
        "Repairs",
        new(RecurrenceFrequency.Custom, IntervalDays: 90)),

    new("Washing curtains and drapes",
        "Cleaning",
        new(RecurrenceFrequency.Custom, IntervalDays: 90)),

    new("Cleaning the washing machine and dishwasher drums",
        "Cleaning",
        new(RecurrenceFrequency.Custom, IntervalDays: 90)),

    // ── Bi-Annual (every 180 days via Custom) ──────────────────────────────
    new("Vehicle oil change",
        "Errands",
        new(RecurrenceFrequency.Custom, IntervalDays: 180)),

    new("Tire Rotation",
        "Errands",
        new(RecurrenceFrequency.Custom, IntervalDays: 180)),

    new("Dental Cleaning",
        "Medical",
        new(RecurrenceFrequency.Custom, IntervalDays: 180)),

    // ── Annual (every 365 days via Custom) ─────────────────────────────────
    new("Deep cleaning carpets (steam cleaning)",
        "Cleaning",
        new(RecurrenceFrequency.Custom, IntervalDays: 365)),

    new("Inspecting and cleaning the chimney/fireplace",
        "Repairs",
        new(RecurrenceFrequency.Custom, IntervalDays: 365)),

    new("Servicing the HVAC system",
        "Repairs",
        new(RecurrenceFrequency.Custom, IntervalDays: 365)),

    new("Cleaning the dryer vent duct",
        "Repairs",
        new(RecurrenceFrequency.Custom, IntervalDays: 365)),

    new("Washing and storing seasonal items",
        "Cleaning",
        new(RecurrenceFrequency.Custom, IntervalDays: 365)),

    new("Inspecting the roof for damage",
        "Repairs",
        new(RecurrenceFrequency.Custom, IntervalDays: 365)),

    new("Deep cleaning the garage / basement",
        "General",
        new(RecurrenceFrequency.Custom, IntervalDays: 365)),

    new("Checking caulking around tubs, showers, and windows",
        "Repairs",
        new(RecurrenceFrequency.Custom, IntervalDays: 365)),

    new("Flushing the water heater",
        "Repairs",
        new(RecurrenceFrequency.Custom, IntervalDays: 365)),

    new("Cleaning and organizing the attic or basement",
        "General",
        new(RecurrenceFrequency.Custom, IntervalDays: 365)),

    new("File Taxes",
        "Finance",
        new(RecurrenceFrequency.Custom, IntervalDays: 365)),

    new("Yearly Medical Checkup",
        "Medical",
        new(RecurrenceFrequency.Custom, IntervalDays: 365)),
};


}