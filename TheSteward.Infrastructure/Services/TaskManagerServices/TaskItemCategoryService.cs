using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Dtos.TaskManagerDtos;
using TheSteward.Core.IRepositories;
using TheSteward.Core.IServices.TaskManagerIServices;
using TheSteward.Core.Models.TaskManagerModels;

namespace TheSteward.Infrastructure.Services.TaskManagerServices;

public class TaskItemCategoryService : ITaskItemCategoryService
{
    private readonly IBaseRepository<TaskItemCategory> _taskItemCategoryRepository;
    private readonly IMapper _mapper;

    public TaskItemCategoryService(IBaseRepository<TaskItemCategory> taskItemCategoryRepository, IMapper mapper)
    {
        _taskItemCategoryRepository = taskItemCategoryRepository;
        _mapper = mapper;
    }

    public async Task<TaskItemCategoryDto> AddAsync(CreateTaskItemCategoryDto taskItemCategoryDto)
    {
        if (taskItemCategoryDto == null)
            throw new ArgumentNullException(nameof(taskItemCategoryDto));

        var taskItemCategory = new TaskItemCategory
        {
            TaskItemCategoryId = Guid.NewGuid(),
            TaskItemCategoryName = taskItemCategoryDto.TaskItemCategoryName,
            ColorHex = taskItemCategoryDto.ColorHex,
            IconName = taskItemCategoryDto.IconName
        };

        await _taskItemCategoryRepository.AddAsync(taskItemCategory);
        await _taskItemCategoryRepository.SaveChangesAsync();

        return _mapper.Map<TaskItemCategoryDto>(taskItemCategory);
    }

    public async Task<UpdateTaskItemCategoryDto> UpdateAsync(UpdateTaskItemCategoryDto taskItemCategoryDto)
    {
        if (taskItemCategoryDto == null)
            throw new ArgumentNullException(nameof(taskItemCategoryDto));

        var taskItemCategory = await _taskItemCategoryRepository.GetByIdAsync(taskItemCategoryDto.TaskItemCategoryId);
        if (taskItemCategory == null)
            throw new KeyNotFoundException($"TaskItemCategory with ID {taskItemCategoryDto.TaskItemCategoryId} not found.");

        taskItemCategory.TaskItemCategoryName = taskItemCategoryDto.TaskItemCategoryName;
        taskItemCategory.ColorHex = taskItemCategoryDto.ColorHex;
        taskItemCategory.IconName = taskItemCategoryDto.IconName;

        await _taskItemCategoryRepository.UpdateAsync(taskItemCategory);
        await _taskItemCategoryRepository.SaveChangesAsync();

        return taskItemCategoryDto;
    }

    public async Task DeleteAsync(Guid taskItemCategoryId)
    {
        if (taskItemCategoryId == Guid.Empty)
            throw new ArgumentException("TaskItemCategory ID cannot be empty.", nameof(taskItemCategoryId));

        var taskItemCategory = await _taskItemCategoryRepository.GetByIdAsync(taskItemCategoryId);
        if (taskItemCategory == null)
            throw new KeyNotFoundException($"TaskItemCategory with ID {taskItemCategoryId} not found.");

        await _taskItemCategoryRepository.DeleteAsync(taskItemCategory);
        await _taskItemCategoryRepository.SaveChangesAsync();
    }

    #region Get Methods
    public async Task<TaskItemCategoryDto?> GetAsync(Guid taskItemCategoryId)
    {
        if (taskItemCategoryId == Guid.Empty)
            throw new ArgumentException("TaskItemCategory ID cannot be empty.", nameof(taskItemCategoryId));

        var taskItemCategory = await _taskItemCategoryRepository.GetByIdAsync(taskItemCategoryId);

        return taskItemCategory == null ? null : _mapper.Map<TaskItemCategoryDto>(taskItemCategory);
    }

    public async Task<TaskItemCategoryDto?> GetWithTaskItemsAsync(Guid taskItemCategoryId)
    {
        if (taskItemCategoryId == Guid.Empty)
            throw new ArgumentException("TaskItemCategory ID cannot be empty.", nameof(taskItemCategoryId));

        var taskItemCategory = await _taskItemCategoryRepository.GetAll()
            .Include(c => c.TaskItems)
            .FirstOrDefaultAsync(c => c.TaskItemCategoryId == taskItemCategoryId);

        return taskItemCategory == null ? null : _mapper.Map<TaskItemCategoryDto>(taskItemCategory);
    }

    public async Task<List<TaskItemCategoryDto>> GetAllAsync()
    {
        var categories = await _taskItemCategoryRepository.GetAll()
            .OrderBy(c => c.TaskItemCategoryName)
            .ToListAsync();

        return _mapper.Map<List<TaskItemCategoryDto>>(categories);
    }

    public async Task<TaskItemCategoryDto> GetByNameAsync(string taskItemCategoryName)
    {
        if (string.IsNullOrWhiteSpace(taskItemCategoryName))
            throw new ArgumentException("TaskItemCategory name cannot be null or whitespace.", nameof(taskItemCategoryName));

        var taskItemCategory = await _taskItemCategoryRepository.GetAll()
            .FirstOrDefaultAsync(c => c.TaskItemCategoryName == taskItemCategoryName);

        if (taskItemCategory == null)
            throw new KeyNotFoundException($"TaskItemCategory with name '{taskItemCategoryName}' not found.");

        return _mapper.Map<TaskItemCategoryDto>(taskItemCategory);
    }
    #endregion Get Methods
}