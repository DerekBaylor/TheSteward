// TaskItemService.cs
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Dtos.TaskManagerDtos;
using TheSteward.Core.IRepositories;
using TheSteward.Core.IRepositories.FinanceManagerIRepositories;
using TheSteward.Core.IRepositories.ITaskManagerRepositories;
using TheSteward.Core.IServices.TaskManagerIServices;
using TheSteward.Core.Models.TaskManagerModels;
using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Infrastructure.Services.TaskManagerServices;

public class TaskItemService : ITaskItemService
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly IExpenseRepository _expenseRepository;
    private readonly IMapper _mapper;

    public TaskItemService(ITaskItemRepository taskItemRepository, IExpenseRepository expenseRepository, IMapper mapper)
    {
        _taskItemRepository = taskItemRepository;
        _expenseRepository = expenseRepository;
        _mapper = mapper;
    }

    public async Task<TaskItemDto> AddAsync(CreateTaskItemDto taskItemDto)
    {
        if (taskItemDto == null)
            throw new ArgumentNullException(nameof(taskItemDto));

        var taskItem = new TaskItem
        {
            TaskItemId = Guid.NewGuid(),
            TaskItemName = taskItemDto.TaskItemName,
            Description = taskItemDto.Description,
            Status = taskItemDto.Status,
            Priority = taskItemDto.Priority,
            DueDate = taskItemDto.DueDate.HasValue
                ? DateTime.SpecifyKind(taskItemDto.DueDate.Value, DateTimeKind.Utc)
                : null,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            CreatedByUserHouseholdId = taskItemDto.CreatedByUserHouseholdId,
            AssignedToUserHouseholdId = taskItemDto.AssignedToUserHouseholdId,
            TaskItemCategoryId = taskItemDto.TaskItemCategoryId,
            RecurrenceId = taskItemDto.RecurrenceId,
            ExpenseId = taskItemDto.ExpenseId,
            IsArchived = false
        };

        await _taskItemRepository.AddAsync(taskItem);
        await _taskItemRepository.SaveChangesAsync();

        return _mapper.Map<TaskItemDto>(taskItem);
    }

    #region Update Methods
    public async Task<UpdateTaskItemDto> UpdateAsync(UpdateTaskItemDto taskItemDto)
    {
        if (taskItemDto == null)
            throw new ArgumentNullException(nameof(taskItemDto));

        var taskItem = await _taskItemRepository.GetByIdAsync(taskItemDto.TaskItemId);
        if (taskItem == null)
            throw new KeyNotFoundException($"TaskItem with ID {taskItemDto.TaskItemId} not found.");

        taskItem.TaskItemName = taskItemDto.TaskItemName;
        taskItem.Description = taskItemDto.Description;
        taskItem.Status = taskItemDto.Status;
        taskItem.Priority = taskItemDto.Priority;
        taskItem.DueDate = taskItemDto.DueDate.HasValue
            ? DateTime.SpecifyKind(taskItemDto.DueDate.Value, DateTimeKind.Utc)
            : null;
        taskItem.CompletedDate = taskItemDto.CompletedDate.HasValue
            ? DateTime.SpecifyKind(taskItemDto.CompletedDate.Value, DateTimeKind.Utc)
            : null;
        taskItem.AssignedToUserHouseholdId = taskItemDto.AssignedToUserHouseholdId;
        taskItem.TaskItemCategoryId = taskItemDto.TaskItemCategoryId;
        taskItem.RecurrenceId = taskItemDto.RecurrenceId;
        taskItem.ExpenseId = taskItemDto.ExpenseId;
        taskItem.UpdatedDate = DateTime.UtcNow;

        await _taskItemRepository.UpdateAsync(taskItem);
        await _taskItemRepository.SaveChangesAsync();

        await SyncLinkedExpenseAsync(taskItem);

        return taskItemDto;
    }

    public async Task<TaskItemDto> CompleteAsync(Guid taskItemId, Guid completedByUserHouseholdId)
    {
        if (taskItemId == Guid.Empty)
            throw new ArgumentException("TaskItem ID cannot be empty.", nameof(taskItemId));

        if (completedByUserHouseholdId == Guid.Empty)
            throw new ArgumentException("UserHousehold ID cannot be empty.", nameof(completedByUserHouseholdId));

        var taskItem = await _taskItemRepository.GetByIdAsync(taskItemId);
        if (taskItem == null)
            throw new KeyNotFoundException($"TaskItem with ID {taskItemId} not found.");

        taskItem.Status = TaskItemStatus.Completed;
        taskItem.CompletedDate = DateTime.UtcNow;
        taskItem.UpdatedDate = DateTime.UtcNow;

        await _taskItemRepository.UpdateAsync(taskItem);
        await _taskItemRepository.SaveChangesAsync();

        return _mapper.Map<TaskItemDto>(taskItem);
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

        await _taskItemRepository.DeleteAsync(taskItem);
        await _taskItemRepository.SaveChangesAsync();
    }

    #region Get Methods

    public async Task<TaskItemDto?> GetAsync(Guid taskItemId)
    {
        if (taskItemId == Guid.Empty)
            throw new ArgumentException("TaskItem ID cannot be empty.", nameof(taskItemId));

        var taskItem = await _taskItemRepository.GetByIdAsync(taskItemId);

        return taskItem == null ? null : _mapper.Map<TaskItemDto>(taskItem);
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

        return taskItem == null ? null : _mapper.Map<TaskItemDto>(taskItem);
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

        return _mapper.Map<List<TaskItemDto>>(taskItems);
    }

    public async Task<List<TaskItemDto>> GetAllByCategoryIdAsync(Guid taskItemCategoryId)
    {
        if (taskItemCategoryId == Guid.Empty)
            throw new ArgumentException("TaskItemCategory ID cannot be empty.", nameof(taskItemCategoryId));

        var taskItems = await _taskItemRepository.GetAll()
            .Where(t => t.TaskItemCategoryId == taskItemCategoryId && !t.IsArchived)
            .OrderBy(t => t.DueDate)
            .ToListAsync();

        return _mapper.Map<List<TaskItemDto>>(taskItems);
    }

    public async Task<List<TaskItemDto>> GetAllByStatusAsync(Guid userHouseholdId, TaskItemStatus status)
    {
        if (userHouseholdId == Guid.Empty)
            throw new ArgumentException("UserHousehold ID cannot be empty.", nameof(userHouseholdId));

        var taskItems = await _taskItemRepository.GetAll()
            .Where(t => t.AssignedToUserHouseholdId == userHouseholdId
                     && t.Status == status
                     && !t.IsArchived)
            .OrderBy(t => t.DueDate)
            .ToListAsync();

        return _mapper.Map<List<TaskItemDto>>(taskItems);
    }

    public async Task<List<TaskItemDto>> GetAllByCreatedByUserHouseholdIdAsync(Guid createdByUserHouseholdId)
    {
        if (createdByUserHouseholdId == Guid.Empty)
            throw new ArgumentException("UserHousehold ID cannot be empty.", nameof(createdByUserHouseholdId));

        var taskItems = await _taskItemRepository.GetAll()
            .Where(t => t.CreatedByUserHouseholdId == createdByUserHouseholdId && !t.IsArchived)
            .OrderBy(t => t.DueDate)
            .ToListAsync();

        return _mapper.Map<List<TaskItemDto>>(taskItems);
    }

    #endregion Get Methods

    #region Utility Helpers

    private async Task SyncLinkedExpenseAsync(TaskItem taskItem)
    {
        if (!taskItem.ExpenseId.HasValue)
            return;

        var linkedExpense = await _expenseRepository.GetByIdAsync(taskItem.ExpenseId.Value);
        if (linkedExpense == null)
            return;

        var strippedName = taskItem.TaskItemName.StartsWith("Pay ", StringComparison.OrdinalIgnoreCase)
            ? taskItem.TaskItemName["Pay ".Length..]
            : taskItem.TaskItemName;

        linkedExpense.ExpenseName = strippedName;

        if (taskItem.DueDate.HasValue)
            linkedExpense.DueDay = taskItem.DueDate.Value.Day;

        await _expenseRepository.UpdateAsync(linkedExpense);
        await _expenseRepository.SaveChangesAsync();
    }

    #endregion Utility Helpers
}