namespace TheSteward.Core.Utils.TaskManagerUtils;

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
}