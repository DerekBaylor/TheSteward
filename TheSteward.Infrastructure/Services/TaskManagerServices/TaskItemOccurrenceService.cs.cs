using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Dtos.TaskManagerDtos;
using TheSteward.Core.IRepositories;
using TheSteward.Core.IServices.TaskManagerIServices;
using TheSteward.Core.Models.TaskManagerModels;
using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Infrastructure.Services.TaskManagerServices;

public class TaskItemOccurrenceService : ITaskItemOccurrenceService
{
    private readonly IBaseRepository<TaskItemOccurrence> _taskItemOccurrenceRepository;
    private readonly IBaseRepository<TaskItem> _taskItemRepository;
    private readonly IMapper _mapper;

    public TaskItemOccurrenceService(
        IBaseRepository<TaskItemOccurrence> taskItemOccurrenceRepository,
        IBaseRepository<TaskItem> taskItemRepository,
        IMapper mapper)
    {
        _taskItemOccurrenceRepository = taskItemOccurrenceRepository;
        _taskItemRepository = taskItemRepository;
        _mapper = mapper;
    }

    public async Task<TaskItemOccurrenceDto> AddAsync(CreateTaskItemOccurrenceDto taskItemOccurrenceDto)
    {
        if (taskItemOccurrenceDto == null)
            throw new ArgumentNullException(nameof(taskItemOccurrenceDto));

        var taskItem = await _taskItemRepository.GetByIdAsync(taskItemOccurrenceDto.TaskItemId);
        if (taskItem == null)
            throw new KeyNotFoundException($"TaskItem with ID {taskItemOccurrenceDto.TaskItemId} not found.");

        var taskItemOccurrence = new TaskItemOccurrence
        {
            TaskItemOccurrenceId = Guid.NewGuid(),
            TaskItemId = taskItemOccurrenceDto.TaskItemId,
            ScheduledDateTime = DateTime.SpecifyKind(taskItemOccurrenceDto.ScheduledDateTime, DateTimeKind.Utc),
            Status = taskItemOccurrenceDto.Status
        };

        await _taskItemOccurrenceRepository.AddAsync(taskItemOccurrence);
        await _taskItemOccurrenceRepository.SaveChangesAsync();

        return _mapper.Map<TaskItemOccurrenceDto>(taskItemOccurrence);
    }

    public async Task<UpdateTaskItemOccurrenceDto> UpdateAsync(UpdateTaskItemOccurrenceDto taskItemOccurrenceDto)
    {
        if (taskItemOccurrenceDto == null)
            throw new ArgumentNullException(nameof(taskItemOccurrenceDto));

        var taskItemOccurrence = await _taskItemOccurrenceRepository.GetByIdAsync(taskItemOccurrenceDto.TaskItemOccurrenceId);
        if (taskItemOccurrence == null)
            throw new KeyNotFoundException($"TaskItemOccurrence with ID {taskItemOccurrenceDto.TaskItemOccurrenceId} not found.");

        taskItemOccurrence.Status = taskItemOccurrenceDto.Status;
        taskItemOccurrence.CompletedDate = taskItemOccurrenceDto.CompletedDate.HasValue
            ? DateTime.SpecifyKind(taskItemOccurrenceDto.CompletedDate.Value, DateTimeKind.Utc)
            : null;
        taskItemOccurrence.CompletedByUserHouseholdId = taskItemOccurrenceDto.CompletedByUserHouseholdId;

        await _taskItemOccurrenceRepository.UpdateAsync(taskItemOccurrence);
        await _taskItemOccurrenceRepository.SaveChangesAsync();

        return taskItemOccurrenceDto;
    }

    public async Task<TaskItemOccurrenceDto> CompleteAsync(Guid taskItemOccurrenceId, Guid completedByUserHouseholdId)
    {
        if (taskItemOccurrenceId == Guid.Empty)
            throw new ArgumentException("TaskItemOccurrence ID cannot be empty.", nameof(taskItemOccurrenceId));

        if (completedByUserHouseholdId == Guid.Empty)
            throw new ArgumentException("UserHousehold ID cannot be empty.", nameof(completedByUserHouseholdId));

        var taskItemOccurrence = await _taskItemOccurrenceRepository.GetByIdAsync(taskItemOccurrenceId);
        if (taskItemOccurrence == null)
            throw new KeyNotFoundException($"TaskItemOccurrence with ID {taskItemOccurrenceId} not found.");

        taskItemOccurrence.Status = TaskItemStatus.Completed;
        taskItemOccurrence.CompletedDate = DateTime.UtcNow;
        taskItemOccurrence.CompletedByUserHouseholdId = completedByUserHouseholdId;

        await _taskItemOccurrenceRepository.UpdateAsync(taskItemOccurrence);
        await _taskItemOccurrenceRepository.SaveChangesAsync();

        return _mapper.Map<TaskItemOccurrenceDto>(taskItemOccurrence);
    }

    public async Task<TaskItemOccurrenceDto> SkipAsync(Guid taskItemOccurrenceId)
    {
        if (taskItemOccurrenceId == Guid.Empty)
            throw new ArgumentException("TaskItemOccurrence ID cannot be empty.", nameof(taskItemOccurrenceId));

        var taskItemOccurrence = await _taskItemOccurrenceRepository.GetByIdAsync(taskItemOccurrenceId);
        if (taskItemOccurrence == null)
            throw new KeyNotFoundException($"TaskItemOccurrence with ID {taskItemOccurrenceId} not found.");

        taskItemOccurrence.Status = TaskItemStatus.Skipped;

        await _taskItemOccurrenceRepository.UpdateAsync(taskItemOccurrence);
        await _taskItemOccurrenceRepository.SaveChangesAsync();

        return _mapper.Map<TaskItemOccurrenceDto>(taskItemOccurrence);
    }

    public async Task DeleteAsync(Guid taskItemOccurrenceId)
    {
        if (taskItemOccurrenceId == Guid.Empty)
            throw new ArgumentException("TaskItemOccurrence ID cannot be empty.", nameof(taskItemOccurrenceId));

        var taskItemOccurrence = await _taskItemOccurrenceRepository.GetByIdAsync(taskItemOccurrenceId);
        if (taskItemOccurrence == null)
            throw new KeyNotFoundException($"TaskItemOccurrence with ID {taskItemOccurrenceId} not found.");

        await _taskItemOccurrenceRepository.DeleteAsync(taskItemOccurrence);
        await _taskItemOccurrenceRepository.SaveChangesAsync();
    }

    #region Get Methods
    public async Task<TaskItemOccurrenceDto?> GetAsync(Guid taskItemOccurrenceId)
    {
        if (taskItemOccurrenceId == Guid.Empty)
            throw new ArgumentException("TaskItemOccurrence ID cannot be empty.", nameof(taskItemOccurrenceId));

        var taskItemOccurrence = await _taskItemOccurrenceRepository.GetByIdAsync(taskItemOccurrenceId);

        return taskItemOccurrence == null ? null : _mapper.Map<TaskItemOccurrenceDto>(taskItemOccurrence);
    }

    public async Task<TaskItemOccurrenceDto?> GetWithTaskItemAsync(Guid taskItemOccurrenceId)
    {
        if (taskItemOccurrenceId == Guid.Empty)
            throw new ArgumentException("TaskItemOccurrence ID cannot be empty.", nameof(taskItemOccurrenceId));

        var taskItemOccurrence = await _taskItemOccurrenceRepository.GetAll()
            .Include(o => o.TaskItem)
            .FirstOrDefaultAsync(o => o.TaskItemOccurrenceId == taskItemOccurrenceId);

        return taskItemOccurrence == null ? null : _mapper.Map<TaskItemOccurrenceDto>(taskItemOccurrence);
    }

    public async Task<List<TaskItemOccurrenceDto>> GetAllByTaskItemIdAsync(Guid taskItemId)
    {
        if (taskItemId == Guid.Empty)
            throw new ArgumentException("TaskItem ID cannot be empty.", nameof(taskItemId));

        var occurrences = await _taskItemOccurrenceRepository.GetAll()
            .Where(o => o.TaskItemId == taskItemId)
            .OrderBy(o => o.ScheduledDateTime)
            .ToListAsync();

        return _mapper.Map<List<TaskItemOccurrenceDto>>(occurrences);
    }

    public async Task<List<TaskItemOccurrenceDto>> GetAllByTaskItemIdAndStatusAsync(Guid taskItemId, TaskItemStatus status)
    {
        if (taskItemId == Guid.Empty)
            throw new ArgumentException("TaskItem ID cannot be empty.", nameof(taskItemId));

        var occurrences = await _taskItemOccurrenceRepository.GetAll()
            .Where(o => o.TaskItemId == taskItemId && o.Status == status)
            .OrderBy(o => o.ScheduledDateTime)
            .ToListAsync();

        return _mapper.Map<List<TaskItemOccurrenceDto>>(occurrences);
    }

    public async Task<List<TaskItemOccurrenceDto>> GetAllByUserHouseholdIdAndDateRangeAsync(
        Guid userHouseholdId,
        DateTime startDate,
        DateTime endDate)
    {
        if (userHouseholdId == Guid.Empty)
            throw new ArgumentException("UserHousehold ID cannot be empty.", nameof(userHouseholdId));

        if (startDate > endDate)
            throw new ArgumentException("Start date cannot be after end date.", nameof(startDate));

        var utcStart = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
        var utcEnd = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);

        var occurrences = await _taskItemOccurrenceRepository.GetAll()
            .Include(o => o.TaskItem)
            .Where(o => o.TaskItem!.AssignedToUserHouseholdId == userHouseholdId
                     && o.ScheduledDateTime >= utcStart
                     && o.ScheduledDateTime <= utcEnd)
            .OrderBy(o => o.ScheduledDateTime)
            .ToListAsync();

        return _mapper.Map<List<TaskItemOccurrenceDto>>(occurrences);
    }
    #endregion Get Methods
}