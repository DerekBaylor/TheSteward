using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.IRepositories.FinanceManagerIRepositories;
using TheSteward.Core.IRepositories.ITaskManagerRepositories;
using TheSteward.Core.IServices.FinanceManagerIServices;
using TheSteward.Core.Models.FinanceManagerModels;
using TheSteward.Core.Models.TaskManagerModels;
using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Infrastructure.Services.FinanceManagerServices;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly ITaskItemCategoryRepository _taskItemCategoryRepository;
    private readonly IRecurrenceRuleRepository _recurrenceRuleRepository;
    private readonly IMapper _mapper;

    public ExpenseService(IExpenseRepository expenseRepository, ITaskItemRepository taskItemRepository, ITaskItemCategoryRepository taskItemCategoryRepository, IRecurrenceRuleRepository recurrenceRuleRepository,  IMapper mapper)
    {
        _expenseRepository = expenseRepository;
        _taskItemRepository = taskItemRepository;
        _taskItemCategoryRepository = taskItemCategoryRepository;
        _recurrenceRuleRepository = recurrenceRuleRepository;
        _mapper = mapper;
    }

    public async Task<ExpenseDto> AddAsync(CreateExpenseDto expenseDto)
    {
        if (expenseDto == null)
            throw new ArgumentNullException(nameof(expenseDto));

        await using var transaction = await _expenseRepository.BeginTransactionAsync();

        try
        {
            var expense = new Expense
            {
                ExpenseId = Guid.NewGuid(),
                ExpenseName = expenseDto.ExpenseName,
                DueDay = expenseDto.DueDay,
                AmountDue = expenseDto.AmountDue,
                DisplayOrder = expenseDto.DisplayOrder,
                BudgetId = expenseDto.BudgetId,
                BudgetCategoryId = expenseDto.BudgetCategoryId,
                BudgetSubCategoryId = expenseDto.BudgetSubCategoryId,
                CreditId = expenseDto.CreditId,
                InvestmentId = expenseDto.InvestmentId,
            };

            await _expenseRepository.AddAsync(expense);
            await _expenseRepository.SaveChangesAsync();

            await CreateLinkedTaskAsync(expense, expenseDto.CreatedByUserHouseholdId);

            await transaction.CommitAsync();

            return _mapper.Map<ExpenseDto>(expense);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<UpdateExpenseDto> UpdateAsync(UpdateExpenseDto expenseDto)
    {
        if (expenseDto == null)
            throw new ArgumentNullException(nameof(expenseDto));

        var expense = await _expenseRepository.GetByIdAsync(expenseDto.ExpenseId);
        if (expense == null)
            throw new KeyNotFoundException($"Expense with ID {expenseDto.ExpenseId} not found.");
        
        expense.ExpenseName = expenseDto.ExpenseName;
        expense.DueDay = expenseDto.DueDay;
        expense.AmountDue = expenseDto.AmountDue;
        expense.DisplayOrder = expenseDto.DisplayOrder;
        expense.BudgetCategoryId = expenseDto.BudgetCategoryId;
        expense.BudgetSubCategoryId = expenseDto.BudgetSubCategoryId;
        expense.CreditId = expenseDto.CreditId;
        expense.InvestmentId = expenseDto.InvestmentId;

        await _expenseRepository.UpdateAsync(expense);
        await _expenseRepository.SaveChangesAsync();

        return expenseDto;
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
                // Delete the recurrence rule before the task since task holds the FK
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

        return expense == null ? null : _mapper.Map<ExpenseDto>(expense);
    }

    public async Task<ExpenseDto?> GetWithRelatedDataAsync(Guid expenseId)
    {
        if (expenseId == Guid.Empty)
            throw new ArgumentException("Expense ID cannot be empty.", nameof(expenseId));

        var expense = await _expenseRepository.GetAll()
            .Include(e => e.BudgetCategory)
            .Include (e => e.BudgetSubCategory)
            .Include(e => e.LinkedCredit)
            .Include(e => e.LinkedInvestment)
            .FirstOrDefaultAsync(e => e.ExpenseId == expenseId);

        return expense == null ? null : _mapper.Map<ExpenseDto>(expense);
    }

    public async Task<List<ExpenseDto>> GetAllByBudgetIdAsync(Guid budgetId)
    {
        if (budgetId == Guid.Empty)
            throw new ArgumentException("Budget ID cannot be empty.", nameof(budgetId));

        var expenses = await _expenseRepository.GetAll()
            .Where(e => e.BudgetId == budgetId)
            .Include(e => e.BudgetCategory)
            .Include (e => e.BudgetSubCategory)
            .OrderBy(e => e.DisplayOrder)
            .ToListAsync();

        return _mapper.Map<List<ExpenseDto>>(expenses);
    }

    public async Task<List<ExpenseDto>> GetAllByCategoryIdAsync(Guid categoryId)
    {
        if (categoryId == Guid.Empty)
            throw new ArgumentException("Category ID cannot be empty.", nameof(categoryId));

        var expenses = await _expenseRepository.GetAll()
            .Where(e => e.BudgetCategoryId == categoryId)
            .Include (e => e.BudgetSubCategory)
            .OrderBy(e => e.DisplayOrder)
            .ToListAsync();

        return _mapper.Map<List<ExpenseDto>>(expenses);
    }

    public async Task<List<ExpenseDto>> GetAllByBudgetIdWithRelatedDataAsync(Guid budgetId)
    {
        if (budgetId == Guid.Empty)
            throw new ArgumentException("Budget ID cannot be empty.", nameof(budgetId));

        var expenses = await _expenseRepository.GetAll()
            .Where(e => e.BudgetId == budgetId)
            .Include(e => e.BudgetCategory)
            .Include (e => e.BudgetSubCategory)
            .Include(e => e.LinkedCredit)
            .Include(e => e.LinkedInvestment)
            .OrderBy(e => e.DisplayOrder)
            .ToListAsync();

        return _mapper.Map<List<ExpenseDto>>(expenses);
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

        return _mapper.Map<ExpenseDto>(expense);
    }
    #endregion Get Methods

    #region Private Helper Methods

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
            RecurrenceDays = null, // Monthly recurrence uses IntervalDays/DueDay, not DaysOfWeek
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

        // If due day has already passed this month, roll to next month
        if (candidateDate <= now)
        {
            var nextMonth = now.AddMonths(1);
            var daysInNextMonth = DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month);
            resolvedDay = dueDay > daysInNextMonth ? daysInNextMonth : dueDay;
            candidateDate = new DateTime(nextMonth.Year, nextMonth.Month, resolvedDay, 0, 0, 0, DateTimeKind.Utc);
        }

        return candidateDate;
    }

    #endregion Private Helper Methods
}