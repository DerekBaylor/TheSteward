using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.IRepositories.FinanceManagerIRepositories;
using TheSteward.Core.IRepositories.ITaskManagerRepositories;
using TheSteward.Core.IServices.FinanceManagerIServices;
using TheSteward.Core.IServices.TaskManagerIServices;
using TheSteward.Core.Models.FinanceManagerModels;
using TheSteward.Core.Models.TaskManagerModels;
using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;
using TheSteward.Core.MappingExtensions;

namespace TheSteward.Infrastructure.Services.FinanceManagerServices;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly ITaskItemCategoryRepository _taskItemCategoryRepository;
    private readonly IRecurrenceRuleRepository _recurrenceRuleRepository;

    public ExpenseService(IExpenseRepository expenseRepository, ITaskItemRepository taskItemRepository, ITaskItemCategoryRepository taskItemCategoryRepository, IRecurrenceRuleRepository recurrenceRuleRepository)
    {
        _expenseRepository = expenseRepository;
        _taskItemRepository = taskItemRepository;
        _taskItemCategoryRepository = taskItemCategoryRepository;
        _recurrenceRuleRepository = recurrenceRuleRepository;
    }

    public async Task<ExpenseDto> AddAsync(CreateExpenseDto expenseDto)
    {
        if (expenseDto == null)
            throw new ArgumentNullException(nameof(expenseDto));

        await using var transaction = await _expenseRepository.BeginTransactionAsync();

        try
        {
            var expenseId = Guid.NewGuid();
            var expense = expenseDto.ToEntity(expenseId);

            await _expenseRepository.AddAsync(expense);
            await _expenseRepository.SaveChangesAsync();

            await CreateLinkedTaskAsync(expense, expenseDto.CreatedByUserHouseholdId);

            await transaction.CommitAsync();

            return expense.ToDto();
        }
        catch
        {
            await transaction.RollbackAsync();
            _expenseRepository.ClearChangeTracker();
            throw;
        }
    }


    public async Task<ExpenseDto> UpdateAsync(UpdateExpenseDto expenseDto)
    {
        if (expenseDto == null)
            throw new ArgumentNullException(nameof(expenseDto));

        var expense = await _expenseRepository.GetByIdAsync(expenseDto.ExpenseId);
        if (expense == null)
            throw new KeyNotFoundException($"Expense with ID {expenseDto.ExpenseId} not found.");

        expense.ApplyUpdate(expenseDto);

        await _expenseRepository.UpdateAsync(expense);
        await _expenseRepository.SaveChangesAsync();

        await SyncLinkedTaskAsync(expense);

        return expense.ToDto();
    }


    public async Task DeleteAsync(Guid expenseId)
    {
        if (expenseId == Guid.Empty)
            throw new ArgumentException("Expense ID cannot be empty.", nameof(expenseId));

        await using var transaction = await _expenseRepository.BeginTransactionAsync();

        try
        {
            var expense = await _expenseRepository.GetByIdAsync(expenseId);
            if (expense == null)
                throw new KeyNotFoundException($"Expense with ID {expenseId} not found.");

            var linkedTask = await _taskItemRepository.GetAll()
                .FirstOrDefaultAsync(t => t.ExpenseId == expenseId);

            if (linkedTask != null)
            {
                if (linkedTask.RecurrenceId.HasValue)
                {
                    var recurrenceRule = await _recurrenceRuleRepository.GetByIdAsync(linkedTask.RecurrenceId.Value);
                    if (recurrenceRule != null)
                    {
                        // Null out the FK first to avoid constraint violation
                        linkedTask.RecurrenceId = null;
                        await _taskItemRepository.UpdateAsync(linkedTask);
                        await _taskItemRepository.SaveChangesAsync();

                        await _recurrenceRuleRepository.DeleteAsync(recurrenceRule);
                        await _recurrenceRuleRepository.SaveChangesAsync();
                    }
                }

                await _taskItemRepository.DeleteAsync(linkedTask);
                await _taskItemRepository.SaveChangesAsync();
            }

            await _expenseRepository.DeleteAsync(expense);
            await _expenseRepository.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }


    #region Get Methods

    public async Task<ExpenseDto?> GetAsync(Guid expenseId)
    {
        if (expenseId == Guid.Empty)
            throw new ArgumentException("Expense ID cannot be empty.", nameof(expenseId));

        var expense = await _expenseRepository.GetByIdAsync(expenseId);
        return expense?.ToDto();
    }

    public async Task<ExpenseDto?> GetWithRelatedDataAsync(Guid expenseId)
    {
        if (expenseId == Guid.Empty)
            throw new ArgumentException("Expense ID cannot be empty.", nameof(expenseId));

        var expense = await _expenseRepository.GetAll()
            .Include(e => e.BudgetCategory)
            .Include(e => e.BudgetSubCategory)
            .Include(e => e.LinkedCredit)
            .Include(e => e.LinkedInvestment)
            .FirstOrDefaultAsync(e => e.ExpenseId == expenseId);

        return expense?.ToDto();
    }

    public async Task<List<ExpenseDto>> GetAllByBudgetIdAsync(Guid budgetId)
    {
        if (budgetId == Guid.Empty)
            throw new ArgumentException("Budget ID cannot be empty.", nameof(budgetId));

        var expenses = await _expenseRepository.GetAll()
            .Where(e => e.BudgetId == budgetId)
            .Include(e => e.BudgetCategory)
            .Include(e => e.BudgetSubCategory)
            .OrderBy(e => e.DisplayOrder)
            .ToListAsync();

        return expenses.ToDtoList();
    }

    public async Task<List<ExpenseDto>> GetAllByCategoryIdAsync(Guid categoryId)
    {
        if (categoryId == Guid.Empty)
            throw new ArgumentException("Category ID cannot be empty.", nameof(categoryId));

        var expenses = await _expenseRepository.GetAll()
            .Where(e => e.BudgetCategoryId == categoryId)
            .Include(e => e.BudgetSubCategory)
            .OrderBy(e => e.DisplayOrder)
            .ToListAsync();

        return expenses.ToDtoList();
    }

    public async Task<List<ExpenseDto>> GetAllByBudgetIdWithRelatedDataAsync(Guid budgetId)
    {
        if (budgetId == Guid.Empty)
            throw new ArgumentException("Budget ID cannot be empty.", nameof(budgetId));

        var expenses = await _expenseRepository.GetAll()
            .Where(e => e.BudgetId == budgetId)
            .Include(e => e.BudgetCategory)
            .Include(e => e.BudgetSubCategory)
            .Include(e => e.LinkedCredit)
            .Include(e => e.LinkedInvestment)
            .OrderBy(e => e.DisplayOrder)
            .ToListAsync();

        return expenses.ToDtoList();
    }

    public async Task<ExpenseDto> GetByBudgetIdAndExpenseNameAsync(Guid budgetId, string expenseName)
    {
        if (budgetId == Guid.Empty)
            throw new ArgumentException("Budget ID cannot be empty.", nameof(budgetId));

        if (string.IsNullOrWhiteSpace(expenseName))
            throw new ArgumentException("Expense name cannot be null or whitespace.", nameof(expenseName));

        var expense = await _expenseRepository.GetAll()
            .Where(e => e.BudgetId == budgetId && e.ExpenseName == expenseName)
            .FirstOrDefaultAsync();

        if (expense == null)
            throw new KeyNotFoundException($"Expense with name '{expenseName}' not found in budget with ID {budgetId}.");

        return expense.ToDto();
    }

    #endregion Get Methods


    #region Private Helper Methods

    /// <summary>
    /// Creates a new recurring task item linked to the specified expense and associates it with the given user
    /// household.
    /// </summary>
    /// <remarks>The created task item will be set to recur monthly and will be categorized under "Bills &
    /// Payments". If the category does not exist, it will be created automatically. The task will be assigned to the
    /// specified user household and linked to the provided expense.</remarks>
    /// <param name="expense">The expense for which the linked recurring task item will be created. Must not be null.</param>
    /// <param name="createdByUserHouseholdId">The unique identifier of the user household that will be set as the creator and assignee of the new task item.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task CreateLinkedTaskAsync(Expense expense, Guid createdByUserHouseholdId)
    {
        var category = await _taskItemCategoryRepository.GetAll()
            .FirstOrDefaultAsync(c => c.TaskItemCategoryName == "Bills & Payments");

        if (category == null)
        {
            category = new TaskItemCategory
            {
                TaskItemCategoryId = Guid.NewGuid(),
                TaskItemCategoryName = "Bills & Payments",
                ColorHex = "#1a56db",
                IconName = "receipt_long"
            };

            await _taskItemCategoryRepository.AddAsync(category);
            await _taskItemCategoryRepository.SaveChangesAsync();
        }

        var dueDate = GetNextDueDateFromDueDay(expense.DueDay);

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
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            CreatedByUserHouseholdId = createdByUserHouseholdId,
            AssignedToUserHouseholdId = createdByUserHouseholdId,
            TaskItemCategoryId = category.TaskItemCategoryId,
            RecurrenceId = recurrenceRule.RecurrenceRuleId,
            ExpenseId = expense.ExpenseId,
            IsArchived = false
        };

        await _taskItemRepository.AddAsync(taskItem);
        await _taskItemRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Calculates the next occurrence of the given due day in the current or next month.
    /// Handles DueDay 31 as the last day of the month.
    /// </summary>
    private static DateTime GetNextDueDateFromDueDay(int dueDay)
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

    /// <summary>
    /// Synchronizes the linked task item with the specified expense, updating its details to reflect the current
    /// expense information.
    /// </summary>
    /// <remarks>If no task item is linked to the specified expense, the method completes without making any
    /// changes.</remarks>
    /// <param name="expense">The expense whose associated task item will be updated. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task SyncLinkedTaskAsync(Expense expense)
    {
        var linkedTask = await _taskItemRepository.GetAll()
            .FirstOrDefaultAsync(t => t.ExpenseId == expense.ExpenseId);

        if (linkedTask == null)
            return;

        linkedTask.TaskItemName = $"Pay {expense.ExpenseName}";
        linkedTask.Description =
            $"Amount due: {expense.AmountDue:C} — due on day {expense.DueDay} of each month.";
        linkedTask.DueDate = GetNextDueDateFromDueDay(expense.DueDay);
        linkedTask.UpdatedDate = DateTime.UtcNow;

        await _taskItemRepository.UpdateAsync(linkedTask);
        await _taskItemRepository.SaveChangesAsync();
    }

    #endregion Private Helper Methods

}