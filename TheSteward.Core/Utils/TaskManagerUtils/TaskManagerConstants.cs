namespace TheSteward.Core.Utils.TaskManagerUtils;

public record TaskCategoryIconOption(string Label, string IconName);
public record StandardTaskDefinition(string TaskName, string CategoryName);

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
    // Daily Chores
    new("Sweeping hard floors",                                         "Daily Chores"),
    new("Wiping down kitchen counters and stovetop",                    "Daily Chores"),
    new("Cleaning the sink and faucets",                                "Daily Chores"),
    new("Tidying and decluttering common areas",                        "Daily Chores"),
    new("Washing dishes / running the dishwasher",                      "Daily Chores"),

    // Weekly Chores
    new("Vacuuming floors and carpets",                                 "Weekly Chores"),
    new("Mopping hard floors",                                          "Weekly Chores"),
    new("Cleaning the sink and faucets (weekly)",                       "Weekly Chores"),
    new("Taking out the trash and recycling",                           "Weekly Chores"),
    new("Doing laundry (washing, drying, folding, and putting away)",   "Weekly Chores"),
    new("Changing bed linens",                                          "Weekly Chores"),
    new("Cleaning toilets, sinks, and showers/tubs",                    "Weekly Chores"),
    new("Wiping down mirrors and glass surfaces",                        "Weekly Chores"),
    new("Dusting furniture and surfaces",                               "Weekly Chores"),
    new("Cut Grass",                                                    "Weekly Chores"),

    // Monthly Chores
    new("Cleaning the inside of the microwave and oven",                "Monthly Chores"),
    new("Wiping down kitchen appliances",                               "Monthly Chores"),
    new("Cleaning inside the refrigerator",                             "Monthly Chores"),
    new("Washing windows (interior)",                                   "Monthly Chores"),
    new("Dusting ceiling fans and light fixtures",                      "Monthly Chores"),
    new("Scrubbing grout in bathrooms",                                 "Monthly Chores"),
    new("Cleaning out the pantry and checking expiration dates",        "Monthly Chores"),
    new("Washing throw pillows and blankets",                           "Monthly Chores"),
    new("Cleaning baseboards and door frames",                          "Monthly Chores"),
    new("Vacuuming upholstered furniture",                              "Monthly Chores"),
    new("Checking and replacing air filters (if needed)",               "Monthly Chores"),

    // Quarterly Chores
    new("Deep cleaning the oven",                                       "Quarterly Chores"),
    new("Cleaning behind and underneath large appliances",              "Quarterly Chores"),
    new("Washing windows (exterior)",                                   "Quarterly Chores"),
    new("Rotating and flipping mattresses",                             "Quarterly Chores"),
    new("Cleaning out closets and donating unused items",               "Quarterly Chores"),
    new("Descaling faucets and showerheads",                            "Quarterly Chores"),
    new("Cleaning window tracks and door tracks",                       "Quarterly Chores"),
    new("Flushing and cleaning garbage disposal",                       "Quarterly Chores"),
    new("Checking smoke and carbon monoxide detector batteries",        "Quarterly Chores"),
    new("Washing curtains and drapes",                                  "Quarterly Chores"),
    new("Cleaning the washing machine and dishwasher drums",            "Quarterly Chores"),

    // Bi-Annual Chores
    new("Vehicle oil change",                                           "Bi-Annual Chores"),

    // Annual Chores
    new("Deep cleaning carpets (steam cleaning)",                       "Annual Chores"),
    new("Cleaning gutters and downspouts",                              "Annual Chores"),
    new("Inspecting and cleaning the chimney/fireplace",                "Annual Chores"),
    new("Servicing the HVAC system",                                    "Annual Chores"),
    new("Cleaning the dryer vent duct",                                 "Annual Chores"),
    new("Washing and storing seasonal items",                           "Annual Chores"),
    new("Inspecting the roof for damage",                               "Annual Chores"),
    new("Deep cleaning the garage / basement",                          "Annual Chores"),
    new("Checking caulking around tubs, showers, and windows",         "Annual Chores"),
    new("Flushing the water heater",                                    "Annual Chores"),
    new("Cleaning and organizing the attic or basement",               "Annual Chores"),
    new("File Taxes",                                                   "Annual Chores"),
};

}