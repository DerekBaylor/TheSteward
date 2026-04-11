using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Dtos.TaskManagerDtos;
using TheSteward.Core.IRepositories.FinanceManagerIRepositories;
using TheSteward.Core.IRepositories.HouseholdIRepositories;
using TheSteward.Core.IRepositories.ITaskManagerRepositories;
using TheSteward.Core.IServices.TaskManagerIServices;
using TheSteward.Core.MappingExtensions;
using TheSteward.Core.Models.HouseholdModels;
using TheSteward.Core.Models.TaskManagerModels;
using TheSteward.Core.Utils.TaskManagerUtils;
using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Infrastructure.Services.TaskManagerServices;

public class TaskItemService : ITaskItemService
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly ITaskItemCategoryRepository _taskItemCategoryRepository;
    private readonly IExpenseRepository _expenseRepository;
    private readonly IRecurrenceRuleRepository _recurrenceRuleRepository;
    private readonly ITaskItemOccurenceRepository _occurrenceRepository;
    private readonly IUserHouseholdRepository _userHouseholdRepository;

    public TaskItemService(
        ITaskItemRepository taskItemRepository,
        ITaskItemCategoryRepository taskItemCategoryRepository,
        IExpenseRepository expenseRepository,
        IRecurrenceRuleRepository recurrenceRuleRepository,
        ITaskItemOccurenceRepository occurrenceRepository,
        IUserHouseholdRepository userHouseholdRepository)
    {
        _taskItemRepository = taskItemRepository;
        _taskItemCategoryRepository = taskItemCategoryRepository;
        _expenseRepository = expenseRepository;
        _recurrenceRuleRepository = recurrenceRuleRepository;
        _occurrenceRepository = occurrenceRepository;
        _userHouseholdRepository = userHouseholdRepository;
    }

    #region Create Methods
    public async Task<TaskItemDto> AddAsync(CreateTaskItemDto taskItemDto)
    {
        if (taskItemDto == null)
            throw new ArgumentNullException(nameof(taskItemDto));

        await using var transaction = await _taskItemRepository.BeginTransactionAsync();

        try
        {
            RecurrenceRule? recurrenceRule = null;

            if (taskItemDto.RecurrenceRule != null)
            {
                recurrenceRule = new RecurrenceRule
                {
                    RecurrenceRuleId = Guid.NewGuid(),
                    RecurrenceFrequency = taskItemDto.RecurrenceRule.RecurrenceFrequency,
                    RecurrenceDays = taskItemDto.RecurrenceRule.RecurrenceDays,
                    IntervalDays = taskItemDto.RecurrenceRule.IntervalDays,
                    StartDateTime = DateTime.SpecifyKind(
                                                taskItemDto.RecurrenceRule.StartDateTime,
                                                DateTimeKind.Utc),
                    EndDateTime = taskItemDto.RecurrenceRule.EndDateTime.HasValue
                                                ? DateTime.SpecifyKind(
                                                    taskItemDto.RecurrenceRule.EndDateTime.Value,
                                                    DateTimeKind.Utc)
                                                : null,
                    LastGeneratedDateTime = DateTime.UtcNow
                };

                await _recurrenceRuleRepository.AddAsync(recurrenceRule);
                await _recurrenceRuleRepository.SaveChangesAsync();
            }

            var taskItem = taskItemDto.ToEntity();

            taskItem.TaskItemId = Guid.NewGuid();
            taskItem.IsArchived = false;
            taskItem.CreatedDate = DateTime.UtcNow;
            taskItem.UpdatedDate = DateTime.UtcNow;
            taskItem.DueDate = taskItemDto.DueDate.HasValue
                ? DateTime.SpecifyKind(taskItemDto.DueDate.Value, DateTimeKind.Utc)
                : null;

            // Tasks linked to an expense are always private on creation.
            taskItem.IsPrivate = taskItemDto.ExpenseId.HasValue || taskItemDto.IsPrivate;

            if (recurrenceRule != null)
                taskItem.RecurrenceId = recurrenceRule.RecurrenceRuleId;

            await _taskItemRepository.AddAsync(taskItem);
            await _taskItemRepository.SaveChangesAsync();

            if (recurrenceRule != null)
            {
                var firstOccurrenceDate = ComputeNextOccurrence(recurrenceRule, recurrenceRule.StartDateTime);

                var firstOccurrence = new TaskItemOccurrence
                {
                    TaskItemOccurrenceId = Guid.NewGuid(),
                    TaskItemId = taskItem.TaskItemId,
                    ScheduledDateTime = firstOccurrenceDate,
                    Status = TaskItemStatus.Pending
                };

                await _occurrenceRepository.AddAsync(firstOccurrence);
                await _occurrenceRepository.SaveChangesAsync();

                // Mirror the first occurrence date as the task's DueDate so the
                // task card shows a meaningful date in the list view.
                taskItem.DueDate = firstOccurrenceDate;
                await _taskItemRepository.UpdateAsync(taskItem);
                await _taskItemRepository.SaveChangesAsync();
            }

            await transaction.CommitAsync();

            return await GetMappedWithIncludesAsync(taskItem.TaskItemId);
        }
        catch
        {
            await transaction.RollbackAsync();
            _taskItemRepository.ClearChangeTracker();
            throw;
        }
    }

    public async Task AddStandardTasksAsync(Guid userHouseholdId, IEnumerable<StandardTaskDefinition> selectedTasks, IEnumerable<TaskItemDto> existingTasks)
    {
        if (userHouseholdId == Guid.Empty)
            throw new ArgumentException("UserHousehold ID cannot be empty.", nameof(userHouseholdId));

        // Resolve HouseholdId once up front so all created tasks are linked correctly.
        var userHousehold = await _userHouseholdRepository.GetByIdAsync(userHouseholdId)
            ?? throw new KeyNotFoundException($"UserHousehold with ID {userHouseholdId} not found.");

        var householdId = userHousehold.HouseholdId;

        var existingNames = existingTasks
            .Select(t => t.TaskItemName.Trim().ToLowerInvariant())
            .ToHashSet();

        var allCategories = await _taskItemCategoryRepository.GetAll().ToListAsync();

        foreach (var task in selectedTasks)
        {
            if (existingNames.Contains(task.TaskName.Trim().ToLowerInvariant()))
                continue;

            var category = await ResolveOrCreateCategoryAsync(allCategories, task.CategoryName);
            await CreateStandardTaskItemAsync(userHouseholdId, householdId, task, category);
        }

        await CreateMissingExpenseTasksAsync(userHouseholdId, householdId, existingNames, allCategories);
    }

    #endregion Create Methods

    #region Update Methods

    public async Task<UpdateTaskItemDto> UpdateAsync(UpdateTaskItemDto taskItemDto)
    {
        if (taskItemDto == null)
            throw new ArgumentNullException(nameof(taskItemDto));

        await using var transaction = await _taskItemRepository.BeginTransactionAsync();

        try
        {
            var taskItem = await _taskItemRepository.GetByIdAsync(taskItemDto.TaskItemId);
            if (taskItem == null)
                throw new KeyNotFoundException($"TaskItem with ID {taskItemDto.TaskItemId} not found.");

            taskItem.ApplyUpdate(taskItemDto);
            taskItem.DueDate = taskItemDto.DueDate.HasValue
                ? DateTime.SpecifyKind(taskItemDto.DueDate.Value, DateTimeKind.Utc)
                : null;
            taskItem.CompletedDate = taskItemDto.CompletedDate.HasValue
                ? DateTime.SpecifyKind(taskItemDto.CompletedDate.Value, DateTimeKind.Utc)
                : null;
            taskItem.UpdatedDate = DateTime.UtcNow;

            await _taskItemRepository.UpdateAsync(taskItem);
            await _taskItemRepository.SaveChangesAsync();

            await SyncLinkedExpenseAsync(taskItem);

            await transaction.CommitAsync();

            return taskItemDto;
        }
        catch
        {
            await transaction.RollbackAsync();
            _taskItemRepository.ClearChangeTracker();
            throw;
        }
    }

    public async Task<TaskItemDto> CompleteAsync(Guid taskItemId, Guid completedByUserHouseholdId)
    {
        if (taskItemId == Guid.Empty)
            throw new ArgumentException("TaskItem ID cannot be empty.", nameof(taskItemId));

        if (completedByUserHouseholdId == Guid.Empty)
            throw new ArgumentException("UserHousehold ID cannot be empty.", nameof(completedByUserHouseholdId));

        await using var transaction = await _taskItemRepository.BeginTransactionAsync();

        try
        {
            var taskItem = await _taskItemRepository.GetByIdAsync(taskItemId);
            if (taskItem == null)
                throw new KeyNotFoundException($"TaskItem with ID {taskItemId} not found.");

            var pendingOccurrence = await CompletePendingOccurrenceAsync(taskItemId, completedByUserHouseholdId);

            if (taskItem.RecurrenceId.HasValue)
                await HandleRecurrenceAsync(taskItem, pendingOccurrence);
            else
                FinalizeTaskAsCompleted(taskItem);

            await _taskItemRepository.UpdateAsync(taskItem);
            await _taskItemRepository.SaveChangesAsync();

            await transaction.CommitAsync();

            return await GetMappedWithIncludesAsync(taskItem.TaskItemId);
        }
        catch
        {
            await transaction.RollbackAsync();
            _taskItemRepository.ClearChangeTracker();
            throw;
        }
    }

    public async Task ArchiveAsync(Guid taskItemId)
    {
        if (taskItemId == Guid.Empty)
            throw new ArgumentException("TaskItem ID cannot be empty.", nameof(taskItemId));

        var taskItem = await _taskItemRepository.GetByIdAsync(taskItemId);
        if (taskItem == null)
            throw new KeyNotFoundException($"TaskItem with ID {taskItemId} not found.");

        taskItem.IsArchived = true;
        taskItem.UpdatedDate = DateTime.UtcNow;

        await _taskItemRepository.UpdateAsync(taskItem);
        await _taskItemRepository.SaveChangesAsync();
    }

    #endregion Update Methods

    public async Task DeleteAsync(Guid taskItemId)
    {
        if (taskItemId == Guid.Empty)
            throw new ArgumentException("TaskItem ID cannot be empty.", nameof(taskItemId));

        var taskItem = await _taskItemRepository.GetByIdAsync(taskItemId);
        if (taskItem == null)
            throw new KeyNotFoundException($"TaskItem with ID {taskItemId} not found.");

        // ── Delete the linked recurrence rule first (if any) ──
        if (taskItem.RecurrenceId.HasValue)
        {
            var rule = await _recurrenceRuleRepository.GetByIdAsync(taskItem.RecurrenceId.Value);
            if (rule != null)
            {
                await _recurrenceRuleRepository.DeleteAsync(rule);
                await _recurrenceRuleRepository.SaveChangesAsync();
            }
        }

        await _taskItemRepository.DeleteAsync(taskItem);
        await _taskItemRepository.SaveChangesAsync();
    }

    #region Get Methods

    public async Task<TaskItemDto?> GetAsync(Guid taskItemId)
    {
        if (taskItemId == Guid.Empty)
            throw new ArgumentException("TaskItem ID cannot be empty.", nameof(taskItemId));

        var exists = await _taskItemRepository.GetByIdAsync(taskItemId);
        if (exists == null) return null;

        return await GetMappedWithIncludesAsync(taskItemId);
    }

    public async Task<TaskItemDto?> GetWithRelatedDataAsync(Guid taskItemId)
    {
        if (taskItemId == Guid.Empty)
            throw new ArgumentException("TaskItem ID cannot be empty.", nameof(taskItemId));

        var taskItem = await _taskItemRepository.GetAll()
            .Include(t => t.TaskItemCategory)
            .Include(t => t.RecurrenceRule)
            .Include(t => t.RelatedExpense)
            .FirstOrDefaultAsync(t => t.TaskItemId == taskItemId);

        return taskItem?.ToDto();
    }

    public async Task<List<TaskItemDto>> GetAllByAssignedUserHouseholdIdAsync(Guid userHouseholdId)
    {
        if (userHouseholdId == Guid.Empty)
            throw new ArgumentException("UserHousehold ID cannot be empty.", nameof(userHouseholdId));

        var taskItems = await _taskItemRepository.GetAll()
            .Where(t => t.AssignedToUserHouseholdId == userHouseholdId && !t.IsArchived)
            .Include(t => t.TaskItemCategory)
            .Include(t => t.RelatedExpense)
            .OrderBy(t => t.DueDate)
            .ToListAsync();

        return taskItems.ToDtoList();
    }

    public async Task<List<TaskItemDto>> GetAllByCategoryIdAsync(Guid taskItemCategoryId)
    {
        if (taskItemCategoryId == Guid.Empty)
            throw new ArgumentException("TaskItemCategory ID cannot be empty.", nameof(taskItemCategoryId));

        var taskItems = await _taskItemRepository.GetAll()
            .Where(t => t.TaskItemCategoryId == taskItemCategoryId && !t.IsArchived)
            .Include(t => t.TaskItemCategory)
            .Include(t => t.RelatedExpense)
            .OrderBy(t => t.DueDate)
            .ToListAsync();

        return taskItems.ToDtoList();
    }

    public async Task<List<TaskItemDto>> GetAllByStatusAsync(Guid userHouseholdId, TaskItemStatus status)
    {
        if (userHouseholdId == Guid.Empty)
            throw new ArgumentException("UserHousehold ID cannot be empty.", nameof(userHouseholdId));

        var taskItems = await _taskItemRepository.GetAll()
            .Where(t => t.AssignedToUserHouseholdId == userHouseholdId
                     && t.Status == status
                     && !t.IsArchived)
            .Include(t => t.TaskItemCategory)
            .Include(t => t.RelatedExpense)
            .OrderBy(t => t.DueDate)
            .ToListAsync();

        return taskItems.ToDtoList();
    }

    public async Task<List<TaskItemDto>> GetAllByCreatedByUserHouseholdIdAsync(Guid createdByUserHouseholdId)
    {
        if (createdByUserHouseholdId == Guid.Empty)
            throw new ArgumentException("UserHousehold ID cannot be empty.", nameof(createdByUserHouseholdId));

        var taskItems = await _taskItemRepository.GetAll()
            .Where(t => t.CreatedByUserHouseholdId == createdByUserHouseholdId && !t.IsArchived)
            .Include(t => t.TaskItemCategory)
            .Include(t => t.RelatedExpense)
            .OrderBy(t => t.DueDate)
            .ToListAsync();

        return taskItems.ToDtoList();
    }

    public async Task<List<TaskItemDto>> GetAllByHouseholdIdAsync(Guid householdId, Guid requestingUserHouseholdId)
    {
        if (householdId == Guid.Empty)
            throw new ArgumentException("Household ID cannot be empty.", nameof(householdId));

        if (requestingUserHouseholdId == Guid.Empty)
            throw new ArgumentException("UserHousehold ID cannot be empty.", nameof(requestingUserHouseholdId));

        var taskItems = await _taskItemRepository.GetAll()
            .Where(t => t.HouseholdId == householdId
                     && !t.IsArchived
                     && (!t.IsPrivate
                         || t.CreatedByUserHouseholdId == requestingUserHouseholdId
                         || t.AssignedToUserHouseholdId == requestingUserHouseholdId))
            .Include(t => t.TaskItemCategory)
            .Include(t => t.RelatedExpense)
            .OrderBy(t => t.DueDate)
            .ToListAsync();

        return taskItems.ToDtoList();
    }


    #endregion Get Methods

    #region Recurrence Helpers

    /// <summary>
    /// Computes the next scheduled DateTime after <paramref name="fromDate"/>
    /// according to the given <paramref name="rule"/>.
    /// </summary>
    private static DateTime ComputeNextOccurrence(RecurrenceRule rule, DateTime fromDate)
    {
        var utcFrom = DateTime.SpecifyKind(fromDate, DateTimeKind.Utc);

        return rule.RecurrenceFrequency switch
        {
            RecurrenceFrequency.Daily => utcFrom.AddDays(1),
            RecurrenceFrequency.Weekly => ComputeNextWeeklyOccurrence(rule, utcFrom, 7),
            RecurrenceFrequency.BiWeekly => ComputeNextWeeklyOccurrence(rule, utcFrom, 14),
            RecurrenceFrequency.Monthly => utcFrom.AddMonths(1),
            RecurrenceFrequency.Custom => utcFrom.AddDays(rule.IntervalDays ?? 1),
            _ => utcFrom.AddDays(1)
        };
    }

    /// <summary>
    /// For Weekly/BiWeekly rules, finds the next day-of-week that is flagged
    /// in <see cref="RecurrenceRule.RecurrenceDays"/>.
    /// Falls back to adding <paramref name="intervalDays"/> when no days are flagged.
    /// </summary>
    private static DateTime ComputeNextWeeklyOccurrence(
        RecurrenceRule rule,
        DateTime fromDate,
        int intervalDays)
    {
        if (rule.RecurrenceDays == null || rule.RecurrenceDays == DaysOfWeek.None)
            return fromDate.AddDays(intervalDays);

        // Walk forward day-by-day (max 14 days) until we land on a flagged day.
        for (var i = 1; i <= intervalDays; i++)
        {
            var candidate = fromDate.AddDays(i);
            var candidateFlag = DotNetDayToFlag(candidate.DayOfWeek);

            if (rule.RecurrenceDays.Value.HasFlag(candidateFlag))
                return candidate;
        }

        // Safety fallback — should not be reached with valid data.
        return fromDate.AddDays(intervalDays);
    }

    /// <summary>Maps a .NET <see cref="DayOfWeek"/> to the custom <see cref="DaysOfWeek"/> flag.</summary>
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

    #endregion Recurrence Helpers

    #region Utility Helpers

    private async Task<TaskItemDto> GetMappedWithIncludesAsync(Guid taskItemId)
    {
        var taskItem = await _taskItemRepository.GetAll()
            .Include(t => t.TaskItemCategory)
            .Include(t => t.RelatedExpense)
            .FirstOrDefaultAsync(t => t.TaskItemId == taskItemId);

        return taskItem!.ToDto();
    }

    private async Task SyncLinkedExpenseAsync(TaskItem taskItem)
    {
        if (!taskItem.ExpenseId.HasValue) return;

        var linkedExpense = await _expenseRepository.GetByIdAsync(taskItem.ExpenseId.Value);
        if (linkedExpense == null) return;

        var strippedName = taskItem.TaskItemName.StartsWith("Pay ", StringComparison.OrdinalIgnoreCase)
            ? taskItem.TaskItemName["Pay ".Length..]
            : taskItem.TaskItemName;

        linkedExpense.ExpenseName = strippedName;

        if (taskItem.DueDate.HasValue)
            linkedExpense.DueDay = taskItem.DueDate.Value.Day;

        await _expenseRepository.UpdateAsync(linkedExpense);
        await _expenseRepository.SaveChangesAsync();
    }

    private static string GetDefaultColorForCategory(string categoryName) =>
        categoryName switch
        {
            "Daily Chores" => "#4CAF50",
            "Weekly Chores" => "#2196F3",
            "Monthly Chores" => "#FF9800",
            "Quarterly Chores" => "#9C27B0",
            "Bi-Annual Chores" => "#F44336",
            "Annual Chores" => "#795548",
            _ => "#607D8B"
        };

    private static string GetDefaultIconForCategory(string categoryName) =>
        categoryName switch
        {
            "Daily Chores" => "cleaning_services",
            "Weekly Chores" => "cleaning_services",
            "Monthly Chores" => "home",
            "Quarterly Chores" => "handyman",
            "Bi-Annual Chores" => "directions_car",
            "Annual Chores" => "calendar_month",
            _ => "task_alt"
        };

    /// <summary>
    /// Marks the oldest pending occurrence as completed and returns it.
    /// Returns null if no pending occurrence exists.
    /// </summary>
    private async Task<TaskItemOccurrence?> CompletePendingOccurrenceAsync(
        Guid taskItemId,
        Guid completedByUserHouseholdId)
    {
        var pendingOccurrence = await _occurrenceRepository.GetAll()
            .Where(o => o.TaskItemId == taskItemId && o.Status == TaskItemStatus.Pending)
            .OrderBy(o => o.ScheduledDateTime)
            .FirstOrDefaultAsync();

        if (pendingOccurrence == null)
            return null;

        pendingOccurrence.Status = TaskItemStatus.Completed;
        pendingOccurrence.CompletedDate = DateTime.UtcNow;
        pendingOccurrence.CompletedByUserHouseholdId = completedByUserHouseholdId;

        await _occurrenceRepository.UpdateAsync(pendingOccurrence);
        await _occurrenceRepository.SaveChangesAsync();

        return pendingOccurrence;
    }

    /// <summary>
    /// Handles recurrence logic — either schedules the next occurrence
    /// or finalizes the task if the recurrence has ended.
    /// </summary>
    private async Task HandleRecurrenceAsync(
        TaskItem taskItem,
        TaskItemOccurrence? completedOccurrence)
    {
        var rule = await _recurrenceRuleRepository.GetByIdAsync(taskItem.RecurrenceId!.Value);

        if (rule == null || (rule.EndDateTime != null && rule.EndDateTime <= DateTime.UtcNow))
        {
            FinalizeTaskAsCompleted(taskItem);
            return;
        }

        var baseDate = completedOccurrence?.ScheduledDateTime ?? DateTime.UtcNow;
        var nextDate = ComputeNextOccurrence(rule, baseDate);

        if (rule.EndDateTime == null || nextDate <= rule.EndDateTime)
            await ScheduleNextOccurrenceAsync(taskItem, rule, nextDate);
        else
            FinalizeTaskAsCompleted(taskItem);
    }

    /// <summary>
    /// Creates the next pending occurrence and updates the task's DueDate.
    /// </summary>
    private async Task ScheduleNextOccurrenceAsync(
        TaskItem taskItem,
        RecurrenceRule rule,
        DateTime nextDate)
    {
        var nextOccurrence = new TaskItemOccurrence
        {
            TaskItemOccurrenceId = Guid.NewGuid(),
            TaskItemId = taskItem.TaskItemId,
            ScheduledDateTime = nextDate,
            Status = TaskItemStatus.Pending
        };

        await _occurrenceRepository.AddAsync(nextOccurrence);
        await _occurrenceRepository.SaveChangesAsync();

        taskItem.DueDate = nextDate;
        taskItem.Status = TaskItemStatus.Pending;
        taskItem.UpdatedDate = DateTime.UtcNow;

        rule.LastGeneratedDateTime = DateTime.UtcNow;
        await _recurrenceRuleRepository.UpdateAsync(rule);
        await _recurrenceRuleRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Marks a task as permanently completed.
    /// </summary>
    private static void FinalizeTaskAsCompleted(TaskItem taskItem)
    {
        taskItem.Status = TaskItemStatus.Completed;
        taskItem.CompletedDate = DateTime.UtcNow;
        taskItem.UpdatedDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Finds an existing category by name or creates and persists a new one.
    /// </summary>
    private async Task<TaskItemCategory> ResolveOrCreateCategoryAsync(
        List<TaskItemCategory> allCategories,
        string categoryName)
    {
        var category = allCategories
            .FirstOrDefault(c =>
                c.TaskItemCategoryName.Trim().ToLowerInvariant() ==
                categoryName.Trim().ToLowerInvariant());

        if (category != null)
            return category;

        category = new TaskItemCategory
        {
            TaskItemCategoryId = Guid.NewGuid(),
            TaskItemCategoryName = categoryName,
            ColorHex = GetDefaultColorForCategory(categoryName),
            IconName = GetDefaultIconForCategory(categoryName)
        };

        await _taskItemCategoryRepository.AddAsync(category);
        await _taskItemCategoryRepository.SaveChangesAsync();
        allCategories.Add(category);

        return category;
    }

    /// <summary>
    /// Creates a standard task item, including its recurrence rule and first
    /// occurrence if the task definition carries a recurrence.
    /// </summary>
    private async Task CreateStandardTaskItemAsync(Guid userHouseholdId, Guid householdId, StandardTaskDefinition task, TaskItemCategory category)
    {
        var now = DateTime.UtcNow;
        RecurrenceRule? recurrenceRule = null;

        if (task.Recurrence != null)
        {
            recurrenceRule = new RecurrenceRule
            {
                RecurrenceRuleId = Guid.NewGuid(),
                RecurrenceFrequency = task.Recurrence.Frequency,
                RecurrenceDays = task.Recurrence.RecurrenceDays,
                IntervalDays = task.Recurrence.IntervalDays,
                StartDateTime = now,
                EndDateTime = null,
                LastGeneratedDateTime = now
            };

            await _recurrenceRuleRepository.AddAsync(recurrenceRule);
            await _recurrenceRuleRepository.SaveChangesAsync();
        }

        var taskItem = new TaskItem
        {
            TaskItemId = Guid.NewGuid(),
            TaskItemName = task.TaskName,
            Status = TaskItemStatus.Pending,
            Priority = TaskItemPriority.Medium,
            HouseholdId = householdId,           // ← new
            IsPrivate = false,                    // ← standard tasks are public
            CreatedByUserHouseholdId = userHouseholdId,
            AssignedToUserHouseholdId = userHouseholdId,
            TaskItemCategoryId = category.TaskItemCategoryId,
            RecurrenceId = recurrenceRule?.RecurrenceRuleId,
            IsArchived = false,
            CreatedDate = now,
            UpdatedDate = now
        };

        await _taskItemRepository.AddAsync(taskItem);
        await _taskItemRepository.SaveChangesAsync();

        if (recurrenceRule != null)
        {
            var firstOccurrenceDate = ComputeNextOccurrence(recurrenceRule, recurrenceRule.StartDateTime);

            var firstOccurrence = new TaskItemOccurrence
            {
                TaskItemOccurrenceId = Guid.NewGuid(),
                TaskItemId = taskItem.TaskItemId,
                ScheduledDateTime = firstOccurrenceDate,
                Status = TaskItemStatus.Pending
            };

            await _occurrenceRepository.AddAsync(firstOccurrence);
            await _occurrenceRepository.SaveChangesAsync();

            taskItem.DueDate = firstOccurrenceDate;
            await _taskItemRepository.UpdateAsync(taskItem);
            await _taskItemRepository.SaveChangesAsync();
        }

    }

    /// <summary>
    /// Checks the household's expenses and creates a "Pay X" bill task for
    /// any expense that doesn't already have one.
    /// </summary>
    private async Task CreateMissingExpenseTasksAsync(Guid userHouseholdId, Guid householdId, HashSet<string> existingTaskNames, List<TaskItemCategory> allCategories)
    {
        var expenses = await _expenseRepository.GetAll()
         .Where(e => e.Budget.HouseholdId == householdId)
         .ToListAsync();

        if (expenses.Count == 0)
            return;

        var expenseIdsWithTasks = await _taskItemRepository.GetAll()
            .Where(t => t.ExpenseId != null)
            .Select(t => t.ExpenseId!.Value)
            .ToHashSetAsync();

        var billsCategory = await ResolveOrCreateCategoryAsync(allCategories, "Bills");

        foreach (var expense in expenses)
        {
            if (expenseIdsWithTasks.Contains(expense.ExpenseId))
                continue;

            var expectedName = $"Pay {expense.ExpenseName}".Trim().ToLowerInvariant();
            if (existingTaskNames.Contains(expectedName))
                continue;

            var dueDate = RecurrenceUtility.GetNextDueDateFromDueDay(expense.DueDay);

            var recurrenceRule = new RecurrenceRule
            {
                RecurrenceRuleId = Guid.NewGuid(),
                RecurrenceFrequency = RecurrenceFrequency.Monthly,
                RecurrenceDays = null,
                IntervalDays = null,
                StartDateTime = dueDate,
                EndDateTime = null,
                LastGeneratedDateTime = DateTime.UtcNow
            };

            await _recurrenceRuleRepository.AddAsync(recurrenceRule);
            await _recurrenceRuleRepository.SaveChangesAsync();

            var taskItem = new TaskItem
            {
                TaskItemId = Guid.NewGuid(),
                TaskItemName = $"Pay {expense.ExpenseName}",
                Description = $"Amount due: {expense.AmountDue:C} — due on day {expense.DueDay} of each month.",
                Status = TaskItemStatus.Pending,
                Priority = TaskItemPriority.Medium,
                DueDate = dueDate,
                HouseholdId = householdId,           // ← new
                IsPrivate = true,                     // ← expense tasks are always private
                CreatedByUserHouseholdId = userHouseholdId,
                AssignedToUserHouseholdId = userHouseholdId,
                TaskItemCategoryId = billsCategory.TaskItemCategoryId,
                RecurrenceId = recurrenceRule.RecurrenceRuleId,
                ExpenseId = expense.ExpenseId,
                IsArchived = false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            await _taskItemRepository.AddAsync(taskItem);
            await _taskItemRepository.SaveChangesAsync();

            var firstOccurrence = new TaskItemOccurrence
            {
                TaskItemOccurrenceId = Guid.NewGuid(),
                TaskItemId = taskItem.TaskItemId,
                ScheduledDateTime = dueDate,
                Status = TaskItemStatus.Pending
            };

            await _occurrenceRepository.AddAsync(firstOccurrence);
            await _occurrenceRepository.SaveChangesAsync();
        }



    }


    #endregion Utility Helpers
}
